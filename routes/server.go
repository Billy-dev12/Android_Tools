package routes

import (
	"android-tools/detector"
	"android-tools/extractor"
	"encoding/json"
	"fmt"
	"net/http"
	"os/exec"
	"path/filepath"
	"runtime"
	"strings"
	"sync"
	"time"
)

var (
	logChan      = make(chan LogMessage, 100)
	mu           sync.Mutex
	monitoring   bool
	monitorMode  string
	stopMonitor  chan struct{}
)

type LogMessage struct {
	Level   string `json:"level"`
	Message string `json:"message"`
}

func logToUI(level, message string) {
	select {
	case logChan <- LogMessage{Level: level, Message: message}:
	default:
		// Drop if buffer full
	}
	// Also log to console just in case
	fmt.Printf("[%s] %s\n", level, message)
}

func StartWebServer() {
	// Serve static files from "public" directory
	fs := http.FileServer(http.Dir("./public"))
	http.Handle("/", fs)

	// API Endpoints
	http.HandleFunc("/api/os", handleOSInfo)
	http.HandleFunc("/api/stream", handleSSE)
	http.HandleFunc("/api/monitor/start", handleMonitorStart)
	http.HandleFunc("/api/monitor/stop", handleMonitorStop)
	http.HandleFunc("/api/extract", handleExtract)
	http.HandleFunc("/api/browse", handleBrowse)

	port := ":8080"
	url := "http://localhost" + port

	fmt.Printf("\033[1;36mMulai Web Server UI di %s\033[0m\n", url)
	
	// Automatically open the browser
	go openBrowser(url)

	if err := http.ListenAndServe(port, nil); err != nil {
		fmt.Printf("\033[1;31mError starting server: %v\033[0m\n", err)
	}
}

func handleOSInfo(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(map[string]string{"os": runtime.GOOS + " " + runtime.GOARCH})
}

// Server-Sent Events endpoint to stream logs to the UI
func handleSSE(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "text/event-stream")
	w.Header().Set("Cache-Control", "no-cache")
	w.Header().Set("Connection", "keep-alive")

	flusher, ok := w.(http.Flusher)
	if !ok {
		http.Error(w, "Streaming unsupported!", http.StatusInternalServerError)
		return
	}

	for {
		select {
		case msg := <-logChan:
			data, _ := json.Marshal(msg)
			fmt.Fprintf(w, "data: %s\n\n", string(data))
			flusher.Flush()
		case <-r.Context().Done():
			return
		}
	}
}

func handleMonitorStart(w http.ResponseWriter, r *http.Request) {
	var req struct{ Mode string `json:"mode"` }
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	mu.Lock()
	defer mu.Unlock()

	if monitoring {
		logToUI("warning", "Already monitoring: "+monitorMode)
		w.WriteHeader(http.StatusOK)
		return
	}

	monitoring = true
	monitorMode = req.Mode
	stopMonitor = make(chan struct{})

	logToUI("info", "Memulai pemantauan Mode: "+req.Mode+"...")

	var detectFunc func() ([]detector.DeviceInfo, error)
	if req.Mode == "fastboot" {
		detectFunc = detector.DetectFastbootDevices
	} else if req.Mode == "mtk" {
		detectFunc = detector.DetectMtkDevices
	} else {
		monitoring = false
		http.Error(w, "Invalid mode", http.StatusBadRequest)
		return
	}

	// Run monitoring loop in background
	go func() {
		ticker := time.NewTicker(2 * time.Second)
		defer ticker.Stop()
		var lastCount = -1

		for {
			select {
			case <-stopMonitor:
				logToUI("warning", "Pemantauan dihentikan.")
				return
			case <-ticker.C:
				devices, err := detectFunc()
				if err != nil {
					logToUI("error", fmt.Sprintf("Error saat deteksi %s: %v", req.Mode, err))
					continue
				}

				currentCount := len(devices)
				if currentCount != lastCount {
					lastCount = currentCount
					if currentCount == 0 {
						logToUI("info", fmt.Sprintf("Menunggu perangkat %s terhubung...", req.Mode))
					} else {
						logToUI("success", fmt.Sprintf("%d perangkat %s terdeteksi!", currentCount, req.Mode))
						for i, dev := range devices {
							details := fmt.Sprintf("  #%d: VendorID:%s ProductID:%s", i+1, dev.VendorID, dev.ProductID)
							if dev.Product != "" {
								details += " Produk:" + dev.Product
							}
							logToUI("info", details)

							// Jalankan Handshake jika mendeteksi port COM MediaTek
							if req.Mode == "mtk" && dev.PortName != "" {
								go func(port string) {
									logToUI("warning", fmt.Sprintf("Melakukan Handshake BROM di %s...", port))
									ok, err := detector.HandshakeMtkDevice(port)
									if err != nil {
										logToUI("error", fmt.Sprintf("Handshake Gagal di %s: %v", port, err))
									} else if ok {
										logToUI("success", fmt.Sprintf("Handshake Sukses! Perangkat terkunci di %s.", port))
									}
								}(dev.PortName)
							}
						}
					}
				}
			}
		}
	}()

	w.WriteHeader(http.StatusOK)
}

func handleMonitorStop(w http.ResponseWriter, r *http.Request) {
	mu.Lock()
	defer mu.Unlock()

	if monitoring {
		close(stopMonitor)
		monitoring = false
		monitorMode = ""
	}
	w.WriteHeader(http.StatusOK)
}

func handleExtract(w http.ResponseWriter, r *http.Request) {
	var req struct {
		Path string `json:"path"`
		Dest string `json:"dest"`
	}
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	logToUI("info", "Mengekstrak firmware dari: "+req.Path)
	destDir := req.Dest
	if destDir == "" {
		dir := filepath.Dir(req.Path)
		filename := filepath.Base(req.Path)
		folderName := filename
		for _, ext := range []string{".tgz", ".tar.gz", ".tar"} {
			if strings.HasSuffix(strings.ToLower(folderName), ext) {
				folderName = folderName[:len(folderName)-len(ext)]
				break
			}
		}
		folderName += "_extracted"
		destDir = filepath.Join(dir, folderName)
	}
	
	err := extractor.ExtractFirmware(req.Path, destDir)
	
	w.Header().Set("Content-Type", "application/json")
	if err != nil {
		logToUI("error", fmt.Sprintf("Error saat mengekstrak: %v", err))
		json.NewEncoder(w).Encode(map[string]interface{}{"error": err.Error()})
	} else {
		logToUI("success", "Ekstraksi berhasil ke: "+destDir)
		json.NewEncoder(w).Encode(map[string]interface{}{"outputDir": destDir})
	}
}

func handleBrowse(w http.ResponseWriter, r *http.Request) {
	var path string
	var err error
	var out []byte

	w.Header().Set("Content-Type", "application/json")

	if runtime.GOOS == "windows" {
		// Run PowerShell command to open OpenFileDialog
		psCommand := `
		Add-Type -AssemblyName System.Windows.Forms
		$f = New-Object System.Windows.Forms.OpenFileDialog
		$f.Filter = "Firmware Files (*.tar;*.tgz)|*.tar;*.tgz|All Files (*.*)|*.*"
		$f.Title = "Select Firmware Archive"
		if ($f.ShowDialog() -eq "OK") {
			Write-Output $f.FileName
		}
		`
		cmd := exec.Command("powershell", "-NoProfile", "-Command", psCommand)
		out, err = cmd.Output()
	} else if runtime.GOOS == "linux" {
		// Run Zenity to open File Selection Dialog on Linux
		cmd := exec.Command("zenity", "--file-selection", "--file-filter=Firmware Files (*.tar *.tgz) | *.tar *.tgz", "--title=Select Firmware File")
		out, err = cmd.Output()
	} else {
		err = fmt.Errorf("unsupported operating system for file browser")
	}

	if err != nil {
		// If cancelled or command failed, we return empty path
		json.NewEncoder(w).Encode(map[string]string{"path": ""})
		return
	}

	// Trim trailing space/new lines
	path = string(out)
	if len(path) > 0 {
		// Strip newlines
		if path[len(path)-1] == '\n' {
			path = path[:len(path)-1]
		}
		if len(path) > 0 && path[len(path)-1] == '\r' {
			path = path[:len(path)-1]
		}
	}

	json.NewEncoder(w).Encode(map[string]string{"path": path})
}

// openBrowser opens the specified URL in the default browser of the user.
func openBrowser(url string) {
	var err error
	switch runtime.GOOS {
	case "linux":
		err = exec.Command("xdg-open", url).Start()
	case "windows":
		err = exec.Command("rundll32", "url.dll,FileProtocolHandler", url).Start()
	case "darwin":
		err = exec.Command("open", url).Start()
	default:
		err = fmt.Errorf("unsupported platform")
	}
	if err != nil {
		fmt.Printf("Gagal membuka browser otomatis: %v\n", err)
	}
}
