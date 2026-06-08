package routes

import (
	"android-tools/detector"
	"android-tools/extractor"
	"fmt"
	"os"
	"os/signal"
	"syscall"
	"time"
)

// RouteCommand mengarahkan argumen CLI ke fungsi detektor yang sesuai
func RouteCommand(args []string) {
	if len(args) < 2 {
		showHelp()
		return
	}

	command := args[1]
	switch command {
	case "fastboot":
		runMonitor("Fastboot", detector.DetectFastbootDevices)
	case "mtk":
		runMonitor("MediaTek BROM", detector.DetectMtkDevices)
	case "extract":
		if len(args) < 3 {
			fmt.Println("\033[1;31mError: Path file firmware (.tar/.tgz) harus ditentukan.\033[0m")
			fmt.Println("Penggunaan: go run main.go extract <path/ke/firmware.tar> [folder_tujuan]")
			return
		}
		tarPath := args[2]
		destDir := "extracted_firmware"
		if len(args) > 3 {
			destDir = args[3]
		}
		err := extractor.ExtractFirmware(tarPath, destDir)
		if err != nil {
			fmt.Printf("\033[1;31mError saat mengekstrak firmware: %v\033[0m\n", err)
		}
	case "ui":
		StartWebServer()
	default:
		fmt.Printf("\033[1;31mPerintah '%s' tidak dikenal.\033[0m\n\n", command)
		showHelp()
	}
}

// showHelp menampilkan panduan penggunaan CLI
func showHelp() {
	fmt.Println("\033[1;36m====================================================\033[0m")
	fmt.Println("\033[1;36m       ANDROID MULTI TOOLS - COMMAND ROUTER         \033[0m")
	fmt.Println("\033[1;36m====================================================\033[0m")
	fmt.Println("Penggunaan:")
	fmt.Println("  go run main.go [perintah]")
	fmt.Println()
	fmt.Println("Perintah yang tersedia:")
	fmt.Println("  \033[1;32mfastboot\033[0m   Mulai memantau perangkat dalam mode Fastboot")
	fmt.Println("  \033[1;32mmtk\033[0m        Mulai memantau perangkat MediaTek BROM/Preloader")
	fmt.Println("  \033[1;32mextract\033[0m    Mengekstrak file firmware (.tar/.tgz) Xiaomi")
	fmt.Println("  \033[1;32mui\033[0m         Membuka versi Web GUI (Tampilan Klasik)")
	fmt.Println()
	fmt.Println("Contoh:")
	fmt.Println("  go run main.go ui")
	fmt.Println("  go run main.go extract vayu_global_images_V12.5.3.0.tar")
	fmt.Println("\033[1;36m====================================================\033[0m")
}

// runMonitor menjalankan loop pemantauan perangkat USB secara berkala
func runMonitor(modeName string, detectFunc func() ([]detector.DeviceInfo, error)) {
	fmt.Printf("\033[1;36mMemulai pemantauan Mode: %s...\033[0m\n", modeName)
	fmt.Println("Tekan \033[1;31mCtrl+C\033[0m untuk keluar.")
	fmt.Println()

	sigChan := make(chan os.Signal, 1)
	signal.Notify(sigChan, syscall.SIGINT, syscall.SIGTERM)

	ticker := time.NewTicker(2 * time.Second)
	defer ticker.Stop()

	var lastCount = -1

	for {
		select {
		case <-sigChan:
			fmt.Println("\n\033[1;33mPemantauan dihentikan. Kembali ke menu utama.\033[0m")
			return
		case <-ticker.C:
			devices, err := detectFunc()
			if err != nil {
				fmt.Printf("\033[1;31mError saat deteksi %s: %v\033[0m\n", modeName, err)
				continue
			}

			currentCount := len(devices)

			if currentCount != lastCount {
				lastCount = currentCount
				if currentCount == 0 {
					fmt.Printf("[%s] \033[1;30mStatus: Menunggu perangkat %s terhubung...\033[0m\n", time.Now().Format("15:04:05"), modeName)
				} else {
					fmt.Printf("[%s] \033[1;32mStatus: %d perangkat %s terdeteksi!\033[0m\n", time.Now().Format("15:04:05"), currentCount, modeName)
					for i, dev := range devices {
						fmt.Printf("  Perangkat #%d:\n", i+1)
						fmt.Printf("    - Vendor ID   : \033[1;37m%s\033[0m\n", dev.VendorID)
						fmt.Printf("    - Product ID  : \033[1;37m%s\033[0m\n", dev.ProductID)
						if dev.Manufacturer != "" {
							fmt.Printf("    - Produsen    : \033[1;37m%s\033[0m\n", dev.Manufacturer)
						}
						if dev.Product != "" {
							fmt.Printf("    - Nama Produk : \033[1;37m%s\033[0m\n", dev.Product)
						}
						if dev.SerialNumber != "" {
							fmt.Printf("    - No. Seri    : \033[1;32m%s\033[0m\n", dev.SerialNumber)
						}
					}
					fmt.Println()
				}
			}
		}
	}
}
