//go:build windows

package detector

import (
	"encoding/json"
	"fmt"
	"os/exec"
	"regexp"
	"strings"
)

// PnpDevice mewakili data output dari PowerShell Get-PnpDevice
type PnpDevice struct {
	FriendlyName string   `json:"FriendlyName"`
	InstanceId   string   `json:"InstanceId"`
	Manufacturer string   `json:"Manufacturer"`
	HardwareID   []string `json:"HardwareID"`
	Status       string   `json:"Status"`
}

// DetectFastbootDevices memindai port USB di Windows menggunakan PowerShell
func DetectFastbootDevices() ([]DeviceInfo, error) {
	psCommand := `Get-PnpDevice -PresentOnly | Where-Object { ($_.HardwareID -like "*Class_ff&SubClass_42&Prot_03*") -or ($_.FriendlyName -like "*Android Bootloader*") -or ($_.FriendlyName -like "*Fastboot*") } | Select-Object FriendlyName, InstanceId, Manufacturer, HardwareID, Status | ConvertTo-Json -Compress`

	cmd := exec.Command("powershell", "-Command", psCommand)
	outputBytes, err := cmd.Output()
	if err != nil {
		return nil, fmt.Errorf("gagal menjalankan query USB di Windows: %v", err)
	}

	output := strings.TrimSpace(string(outputBytes))
	if output == "" || output == "null" {
		return []DeviceInfo{}, nil
	}

	if !strings.HasPrefix(output, "[") {
		output = "[" + output + "]"
	}

	var pnpDevs []PnpDevice
	err = json.Unmarshal([]byte(output), &pnpDevs)
	if err != nil {
		return nil, fmt.Errorf("gagal memproses data USB Windows: %v", err)
	}

	var devices []DeviceInfo
	for _, dev := range pnpDevs {
		vid, pid, serial := parseInstanceId(dev.InstanceId)

		devices = append(devices, DeviceInfo{
			VendorID:     vid,
			ProductID:    pid,
			Manufacturer: dev.Manufacturer,
			Product:      dev.FriendlyName,
			SerialNumber: serial,
			Path:         dev.InstanceId,
		})
	}

	return devices, nil
}

// DetectMtkDevices memindai port USB di Windows untuk mencari MediaTek BROM/Preloader
func DetectMtkDevices() ([]DeviceInfo, error) {
	// Query PnP devices dengan VID 0E8D (MediaTek) dan PID 0003 (BROM) / 2000 (Preloader) atau nama MediaTek
	psCommand := `Get-PnpDevice -PresentOnly | Where-Object { ($_.HardwareID -like "*VID_0E8D&PID_0003*") -or ($_.HardwareID -like "*VID_0E8D&PID_2000*") -or ($_.FriendlyName -like "*MediaTek USB Port*") -or ($_.FriendlyName -like "*MediaTek Preloader*") } | Select-Object FriendlyName, InstanceId, Manufacturer, HardwareID, Status | ConvertTo-Json -Compress`

	cmd := exec.Command("powershell", "-Command", psCommand)
	outputBytes, err := cmd.Output()
	if err != nil {
		return nil, fmt.Errorf("gagal menjalankan query USB MediaTek di Windows: %v", err)
	}

	output := strings.TrimSpace(string(outputBytes))
	if output == "" || output == "null" {
		return []DeviceInfo{}, nil
	}

	if !strings.HasPrefix(output, "[") {
		output = "[" + output + "]"
	}

	var pnpDevs []PnpDevice
	err = json.Unmarshal([]byte(output), &pnpDevs)
	if err != nil {
		return nil, fmt.Errorf("gagal memproses data USB MediaTek Windows: %v", err)
	}

	var devices []DeviceInfo
	comRegex := regexp.MustCompile(`\((COM\d+)\)`)
	for _, dev := range pnpDevs {
		vid, pid, serial := parseInstanceId(dev.InstanceId)

		portName := ""
		matches := comRegex.FindStringSubmatch(dev.FriendlyName)
		if len(matches) > 1 {
			portName = matches[1]
		}

		devices = append(devices, DeviceInfo{
			VendorID:     vid,
			ProductID:    pid,
			Manufacturer: dev.Manufacturer,
			Product:      dev.FriendlyName,
			SerialNumber: serial,
			Path:         dev.InstanceId,
			PortName:     portName,
		})
	}

	return devices, nil
}

func parseInstanceId(instanceId string) (string, string, string) {
	parts := strings.Split(instanceId, "\\")
	if len(parts) < 2 {
		return "", "", ""
	}

	vidPidPart := parts[1]
	serial := ""
	if len(parts) > 2 {
		serial = parts[2]
	}

	vid := ""
	pid := ""
	vpSubparts := strings.Split(vidPidPart, "&")
	for _, sub := range vpSubparts {
		if strings.HasPrefix(sub, "VID_") {
			vid = strings.TrimPrefix(sub, "VID_")
		} else if strings.HasPrefix(sub, "PID_") {
			pid = strings.TrimPrefix(sub, "PID_")
		}
	}

	return vid, pid, serial
}
