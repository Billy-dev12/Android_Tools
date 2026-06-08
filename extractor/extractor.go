package extractor

import (
	"archive/tar"
	"compress/gzip"
	"fmt"
	"io"
	"os"
	"path/filepath"
	"strings"
)

// ExtractFirmware mendeteksi format (tar atau tgz) dan mengekstrak isinya ke folder tujuan
func ExtractFirmware(tarPath string, destDir string) error {
	file, err := os.Open(tarPath)
	if err != nil {
		return fmt.Errorf("gagal membuka file firmware: %v", err)
	}
	defer file.Close()

	// Pastikan folder tujuan ada
	if err := os.MkdirAll(destDir, 0755); err != nil {
		return fmt.Errorf("gagal membuat folder tujuan: %v", err)
	}

	var fileReader io.Reader = file

	// Jika file adalah .tgz atau .tar.gz, gunakan pembaca gzip terlebih dahulu
	if strings.HasSuffix(tarPath, ".tgz") || strings.HasSuffix(tarPath, ".tar.gz") {
		gzipReader, err := gzip.NewReader(file)
		if err != nil {
			return fmt.Errorf("gagal menginisialisasi dekompresi gzip: %v", err)
		}
		defer gzipReader.Close()
		fileReader = gzipReader
	}

	tarReader := tar.NewReader(fileReader)

	fmt.Printf("\033[1;36mMengaktifkan Ekstraktor untuk:\033[0m %s\n", filepath.Base(tarPath))
	fmt.Printf("\033[1;36mFolder Tujuan:\033[0m %s\n", destDir)
	fmt.Println("Memulai ekstraksi...")
	fmt.Println()

	var extractedCount int

	for {
		header, err := tarReader.Next()
		if err == io.EOF {
			break // Akhir dari file tar
		}
		if err != nil {
			return fmt.Errorf("gagal membaca entri tar: %v", err)
		}

		// Bersihkan path file tujuan untuk menghindari celah keamanan path traversal (Zip Slip vulnerability)
		cleanPath := filepath.Clean(header.Name)
		if strings.HasPrefix(cleanPath, "..") || strings.HasPrefix(cleanPath, "/") {
			continue // Abaikan file berbahaya
		}

		target := filepath.Join(destDir, cleanPath)

		switch header.Typeflag {
		case tar.TypeDir:
			// Buat direktori jika tipe adalah folder
			if err := os.MkdirAll(target, 0755); err != nil {
				return fmt.Errorf("gagal membuat subdirektori %s: %v", target, err)
			}
		case tar.TypeReg:
			// Buat direktori induk file jika belum ada
			baseDir := filepath.Dir(target)
			if err := os.MkdirAll(baseDir, 0755); err != nil {
				return fmt.Errorf("gagal membuat folder induk untuk file: %v", err)
			}

			// Tampilkan log progress untuk file-file utama (seperti .img, .bin, .sh, .bat)
			isMainFile := strings.HasSuffix(target, ".img") || strings.HasSuffix(target, ".bin") || strings.HasSuffix(target, ".sh") || strings.HasSuffix(target, ".bat")
			if isMainFile {
				sizeMB := float64(header.Size) / (1024 * 1024)
				fmt.Printf("  -> Mengekstrak: \033[1;37m%-35s\033[0m (%.2f MB)... ", filepath.Base(target), sizeMB)
			}

			// Tulis data file
			outFile, err := os.OpenFile(target, os.O_CREATE|os.O_RDWR|os.O_TRUNC, header.FileInfo().Mode())
			if err != nil {
				if isMainFile {
					fmt.Println("\033[1;31m[GAGAL]\033[0m")
				}
				return fmt.Errorf("gagal membuat file %s: %v", target, err)
			}

			_, err = io.Copy(outFile, tarReader)
			outFile.Close()
			if err != nil {
				if isMainFile {
					fmt.Println("\033[1;31m[GAGAL TULIS]\033[0m")
				}
				return fmt.Errorf("gagal menulis data file %s: %v", target, err)
			}

			if isMainFile {
				fmt.Println("\033[1;32m[OK]\033[0m")
			}
			extractedCount++
		}
	}

	fmt.Println()
	fmt.Printf("\033[1;32mEkstraksi selesai! Sukses mengekstrak %d file.\033[0m\n", extractedCount)
	return nil
}
