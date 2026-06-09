package detector

import (
	"fmt"
	"time"

	"go.bug.st/serial"
)

// HandshakeMtkDevice melakukan handshake BROM untuk mengunci HP agar koneksi tidak putus
func HandshakeMtkDevice(portName string) (bool, error) {
	if portName == "" {
		return false, fmt.Errorf("port name kosong")
	}

	mode := &serial.Mode{
		BaudRate: 115200,
		DataBits: 8,
		Parity:   serial.NoParity,
		StopBits: serial.OneStopBit,
	}

	// Coba buka port serial
	port, err := serial.Open(portName, mode)
	if err != nil {
		return false, fmt.Errorf("gagal membuka port serial: %v", err)
	}
	defer port.Close()

	// Pasang timeout pembacaan 1 detik
	port.SetReadTimeout(1 * time.Second)

	buffer := make([]byte, 1)
	success := false

	// Loop kirim byte 0xa0 hingga mendapat respon balik 0x5f dari HP
	// Mencoba hingga 150 kali
	for i := 0; i < 150; i++ {
		_, err := port.Write([]byte{0xa0})
		if err != nil {
			return false, fmt.Errorf("gagal menulis ke port: %v", err)
		}

		n, err := port.Read(buffer)
		if err == nil && n > 0 {
			if buffer[0] == 0x5f {
				success = true
				break
			}
		}
		time.Sleep(10 * time.Millisecond)
	}

	if !success {
		return false, fmt.Errorf("handshake timeout (HP tidak membalas 0x5f)")
	}

	// Sinyal konfirmasi kelanjutan protokol BROM
	confirmBytes := []byte{0x0a, 0x50, 0x05, 0x30}
	for _, b := range confirmBytes {
		_, err = port.Write([]byte{b})
		if err != nil {
			return false, fmt.Errorf("gagal mengirim byte konfirmasi: %v", err)
		}

		// Baca respon balik (echo) dari HP
		n, err := port.Read(buffer)
		if err != nil || n == 0 {
			return false, fmt.Errorf("HP gagal membalas echo byte 0x%02x", b)
		}
	}

	// Setelah handshake sukses, kita biarkan port terbuka selama beberapa detik
	// agar pengguna bisa melepaskan tombol volume tanpa khawatir HP terputus.
	// BROM akan membeku (freeze) selama port ini di-hold terbuka.
	time.Sleep(5 * time.Second)

	return true, nil
}
