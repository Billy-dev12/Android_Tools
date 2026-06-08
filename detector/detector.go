package detector

// DeviceInfo menyimpan info tentang perangkat USB yang terdeteksi
type DeviceInfo struct {
	VendorID     string
	ProductID    string
	Manufacturer string
	Product      string
	SerialNumber string
	Path         string
}
