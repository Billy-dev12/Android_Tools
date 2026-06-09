# Jurnal Perjalanan Pengembangan: Android Multi Tools
*Dokumentasi Pengembangan Sistem - Juni 2026*

Dokumen ini merangkum kronologi perjalanan pengembangan, tantangan, dan solusi yang telah kita buat hari ini untuk mewujudkan aplikasi servis Android yang handal dan profesional.

---

## 🚀 Peta Perjalanan & Evolusi Proyek

### 📍 Fase 1: Perancangan Awal & Pembuatan Web GUI (Wails-Style)
*   **Tantangan:** Membuat tampilan GUI yang sederhana, bersih, namun fungsional seperti SamFw Tool.
*   **Solusi:** 
    *   Membuat struktur antarmuka web lokal di dalam direktori `public/` (HTML, CSS, dan JS).
    *   Menghubungkan frontend web ke backend Go menggunakan **Server-Sent Events (SSE)** untuk mengalirkan log pendeteksian USB secara *real-time*.
    *   Mendesain UI dengan tema **Windows Forms Klasik (Win32)** menggunakan warna abu-abu khas kontrol Windows, tombol 3D bevel, dan konsol output berlatar putih seperti multiline TextBox.
    *   Mengintegrasikan tombol **Browse...** yang memanggil File Explorer asli Windows (melalui skrip PowerShell tersembunyi) dan Linux (`zenity`).

---

### 📍 Fase 2: Eksperimen Fyne GUI vs Pembersihan CGO
*   **Tantangan:** Pengguna ingin membandingkan antarmuka berbasis Web dengan antarmuka native Go menggunakan framework **Fyne**.
*   **Solusi:**
    *   Mengimpor pustaka `fyne.io/fyne/v2` dan membuat file kontrol GUI native di `routes/fyne_gui.go`.
    *   **Hasil Evaluasi:** Fyne berjalan secara native, tetapi tampilannya terlalu menyesuaikan dengan sistem operasi host (Linux) sehingga terlihat kurang seragam jika dijalankan lintas sistem operasi. Selain itu, Fyne mewajibkan penggunaan **CGO** (compiler C) sehingga menyulitkan proses kompilasi silang (*cross-compile*) dari Linux ke Windows.
    *   **Keputusan:** Kode Fyne dihapus sepenuhnya demi menjaga basis kode tetap *pure Go* tanpa CGO, sehingga bisa dikompilasi ke Windows secara instan.

---

### 📍 Fase 3: Pemisahan Arsitektur (C# WinForms + Go CLI Backend)
*   **Tantangan:** Menginginkan aplikasi desktop native Windows murni (seperti file `.exe` biasa) yang tidak bergantung pada browser web sama sekali.
*   **Solusi:**
    *   Menerapkan arsitektur hybrid standar industri alat servis HP: **C# Windows Forms di depan (Frontend) dan Go CLI di belakang (Backend)**.
    *   Membuat file sumber C# lengkap di folder `gui_csharp/` (`Program.cs`, `Form1.cs`, `Form1.Designer.cs`).
    *   C# bertindak sebagai penampil tombol dan teks, lalu memanggil `android_tools.exe` (Go) di latar belakang secara asinkron (menggunakan `Process`), menyaring teks ANSI warna, dan menulisnya ke UI log C#.
    *   Menambahkan instruksi pembuatan ikon Windows (`Logo.ico`) menggunakan ImageMagick (`convert`) dan cara mengompilasinya langsung di Linux menggunakan compiler **Mono** (`mcs -win32icon:Logo.ico`).

---

### 📍 Fase 4: Mengatasi Masalah Koneksi MTK BROM (Handshake)
*   **Tantangan:** HP MediaTek yang dicolok dalam mode BROM (sambil menekan tombol volume) mengalami masalah koneksi putus-nyambung karena mengalami batas waktu (*timeout*) dan booting ulang.
*   **Solusi:**
    *   Memperbarui pendeteksi USB agar mendeteksi nama port serial virtual secara dinamis (`COMx` di Windows via Regex, `/dev/ttyACM0` di Linux via pencarian sysfs).
    *   Menggunakan pustaka `go.bug.st/serial` untuk membuka jalur serial port secara langsung pada Baud Rate 115200.
    *   Mengirimkan byte jabat tangan `0xa0` secara terus menerus sampai boot ROM HP membalas dengan byte `0x5f`, kemudian mengirimkan byte konfirmasi kelanjutan (`0x0a`, `0x50`, `0x05`, `0x30`).
    *   Koneksi port ditahan agar chip BROM "membeku" (*freeze*), memungkinkan pengguna melepaskan tombol volume tanpa khawatir koneksi USB terputus.

---

## 📦 Hasil Akhir yang Siap Didistribusikan

Setelah proses build selesai, Anda kini memiliki paket distribusi Windows yang sangat bersih:

```text
📁 Android_Tools_Package/
 ├── 📄 AndroidToolsGUI.exe  (Aplikasi UI C# - berikon logo Anda)
 ├── 📄 android_tools.exe    (Mesin backend Go - menangani USB & Handshake)
 ├── 🖼️ Logo.jpeg            (Gambar logo UI)
 └── 🖼️ Logo.ico             (Ikon aplikasi)
```

Perjalanan hari ini membuktikan bahwa kombinasi **Go + C#** menghasilkan aplikasi servis yang sangat kuat, aman dari deteksi salah antivirus, dan sangat mudah dirakit dari Linux! 🚀
