package detector

// DeviceInfo menyimpan info tentang perangkat USB yang terdeteksi
type DeviceInfo struct {
	VendorID     string
	ProductID    string
	Manufacturer string
	Product      string
	SerialNumber string
	Path         string
	PortName     string // e.g. "COM5" or "/dev/ttyACM0"
}
