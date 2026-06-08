//go:build linux

package detector

import (
	"fmt"
	"io/ioutil"
	"os"
	"path/filepath"
	"strings"
)

// DetectFastbootDevices memindai port USB di Linux untuk mencari Fastboot interface
func DetectFastbootDevices() ([]DeviceInfo, error) {
	var devices []DeviceInfo

	basePath := "/sys/bus/usb/devices"
	files, err := ioutil.ReadDir(basePath)
	if err != nil {
		if os.IsNotExist(err) {
			return nil, fmt.Errorf("direktori %s tidak ditemukan. Apakah sistem Anda mendukung USB?", basePath)
		}
		return nil, err
	}

	for _, file := range files {
		name := file.Name()
		if !strings.Contains(name, ":") {
			continue
		}

		ifPath := filepath.Join(basePath, name)

		classBytes, err := ioutil.ReadFile(filepath.Join(ifPath, "bInterfaceClass"))
		if err != nil {
			continue
		}
		subclassBytes, err := ioutil.ReadFile(filepath.Join(ifPath, "bInterfaceSubClass"))
		if err != nil {
			continue
		}
		protoBytes, err := ioutil.ReadFile(filepath.Join(ifPath, "bInterfaceProtocol"))
		if err != nil {
			continue
		}

		class := strings.TrimSpace(string(classBytes))
		subclass := strings.TrimSpace(string(subclassBytes))
		proto := strings.TrimSpace(string(protoBytes))

		if class == "ff" && subclass == "42" && proto == "03" {
			parentDirName := strings.Split(name, ":")[0]
			parentPath := filepath.Join(basePath, parentDirName)

			vidBytes, _ := ioutil.ReadFile(filepath.Join(parentPath, "idVendor"))
			pidBytes, _ := ioutil.ReadFile(filepath.Join(parentPath, "idProduct"))
			mfgBytes, _ := ioutil.ReadFile(filepath.Join(parentPath, "manufacturer"))
			prodBytes, _ := ioutil.ReadFile(filepath.Join(parentPath, "product"))
			serBytes, _ := ioutil.ReadFile(filepath.Join(parentPath, "serial"))

			devices = append(devices, DeviceInfo{
				VendorID:     strings.TrimSpace(string(vidBytes)),
				ProductID:    strings.TrimSpace(string(pidBytes)),
				Manufacturer: strings.TrimSpace(string(mfgBytes)),
				Product:      strings.TrimSpace(string(prodBytes)),
				SerialNumber: strings.TrimSpace(string(serBytes)),
				Path:         parentPath,
			})
		}
	}

	return devices, nil
}

// DetectMtkDevices memindai port USB di Linux untuk mencari MediaTek BROM/Preloader
func DetectMtkDevices() ([]DeviceInfo, error) {
	var devices []DeviceInfo

	basePath := "/sys/bus/usb/devices"
	files, err := ioutil.ReadDir(basePath)
	if err != nil {
		if os.IsNotExist(err) {
			return nil, fmt.Errorf("direktori %s tidak ditemukan. Apakah sistem Anda mendukung USB?", basePath)
		}
		return nil, err
	}

	for _, file := range files {
		name := file.Name()
		// Cari device induk saja (tanpa titik dua ":")
		if strings.Contains(name, ":") {
			continue
		}

		devPath := filepath.Join(basePath, name)

		vidBytes, err := ioutil.ReadFile(filepath.Join(devPath, "idVendor"))
		if err != nil {
			continue
		}
		pidBytes, err := ioutil.ReadFile(filepath.Join(devPath, "idProduct"))
		if err != nil {
			continue
		}

		vid := strings.TrimSpace(strings.ToLower(string(vidBytes)))
		pid := strings.TrimSpace(strings.ToLower(string(pidBytes)))

		// MediaTek VID = 0e8d, PID BROM = 0003, Preloader = 2000
		if vid == "0e8d" && (pid == "0003" || pid == "2000") {
			mfgBytes, _ := ioutil.ReadFile(filepath.Join(devPath, "manufacturer"))
			prodBytes, _ := ioutil.ReadFile(filepath.Join(devPath, "product"))
			serBytes, _ := ioutil.ReadFile(filepath.Join(devPath, "serial"))

			productName := strings.TrimSpace(string(prodBytes))
			if productName == "" {
				if pid == "0003" {
					productName = "MediaTek USB Port (BROM Mode)"
				} else {
					productName = "MediaTek Preloader Device"
				}
			}

			devices = append(devices, DeviceInfo{
				VendorID:     vid,
				ProductID:    pid,
				Manufacturer: strings.TrimSpace(string(mfgBytes)),
				Product:      productName,
				SerialNumber: strings.TrimSpace(string(serBytes)),
				Path:         devPath,
			})
		}
	}

	return devices, nil
}
