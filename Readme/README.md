# Android Multi Tools

Aplikasi utilitas Android untuk memantau perangkat (Fastboot & MediaTek BROM) serta mengekstrak berkas firmware Xiaomi (`.tar` / `.tgz`). 

Proyek ini menggunakan arsitektur hybrid yang memisahkan antara mesin belakang (**Go Backend CLI**) dan antarmuka visual (**Frontend GUI**).

---

## 📂 Struktur Proyek saat Ini

```text
Android_Tools/
├── main.go               # Entry point aplikasi Go
├── routes/               # Routing perintah CLI & HTTP Server untuk Web UI
├── detector/             # Logika pendeteksian perangkat USB (Fastboot & MTK)
├── extractor/            # Logika ekstraksi berkas firmware (.tar/.tgz)
├── public/               # File aset Web UI (HTML, CSS bergaya retro WinForms, JS)
├── gui_csharp/           # Kode sumber C# Windows Forms (Program.cs, Form1.cs, Designer)
└── README.md             # Dokumentasi proyek (File ini)
```

---

## ⚙️ 1. Backend: Go CLI Engine

Mesin utama ditulis menggunakan Golang untuk memastikan performa tinggi dan portabilitas tanpa dependensi pihak ketiga (*pure Go*).

### Cara Kompilasi untuk Windows (dari Linux):
Anda dapat melakukan kompilasi silang (*cross-compile*) secara langsung dengan perintah berikut:
```bash
GOOS=windows GOARCH=amd64 CGO_ENABLED=0 go build -o android_tools.exe
```
Perintah di atas akan menghasilkan berkas **`android_tools.exe`** (CLI) untuk Windows tanpa memerlukan compiler C.

---

## 🖥️ 2. Pilihan Frontend GUI (Antarmuka)

Kami menyediakan dua jenis pilihan antarmuka yang siap digunakan:

### Pilihan A: C# Windows Forms (Native GUI Windows)
Sangat direkomendasikan jika Anda ingin aplikasi desktop native murni (tampilan kotak dialog Windows asli) yang berjalan sangat ringan. File C# ini akan memanggil `android_tools.exe` di latar belakang dan menulis log-nya di layar.

#### Cara Kompilasi C# langsung di Linux (Menggunakan Mono):
1. Pasang Mono & ImageMagick (untuk konversi ikon) di Linux Anda:
   - **Arch Linux:** `sudo pacman -S mono imagemagick`
   - **Ubuntu/Debian:** `sudo apt install mono-devel imagemagick`
2. Konversi logo Anda (`Logo.jpeg`) menjadi berkas ikon Windows (`Logo.ico`):
   ```bash
   convert Logo.jpeg -define icon:auto-resize=256,128,64,48,32,16 Logo.ico
   ```
3. Masuk ke direktori `gui_csharp` dan jalankan perintah kompilasi (pastikan file `Logo.ico` disalin ke folder `gui_csharp` sebelum dicompile):
   ```bash
   mcs -target:winexe -out:AndroidToolsGUI.exe -win32icon:Logo.ico -reference:System.Windows.Forms -reference:System.Drawing Program.cs Form1.cs Form1.Designer.cs
   ```
   *Parameter `-win32icon:Logo.ico` inilah yang menyisipkan ikon logo Anda ke dalam file `.exe`.*

#### Cara Kompilasi di Windows:
Salin berkas di folder `gui_csharp/` ke dalam proyek **Windows Forms App (.NET Framework)** baru di Visual Studio Windows, lalu klik **Build**.

---

### Pilihan B: Web-Based GUI (Estetika WinForms Klasik)
Antarmuka web lokal yang otomatis membuka browser saat dijalankan. Menggunakan HTML/CSS yang dimodifikasi khusus agar terlihat persis seperti aplikasi Windows Forms jadul.

#### Cara Menjalankan:
```bash
go run main.go ui
```
*Aplikasi akan menyalakan web server lokal di port 8080 dan otomatis membuka browser.*

---

## 📦 3. Cara Distribusi / Kirim ke Windows

Setelah kedua file berhasil di-compile, Anda hanya perlu menyatukan kedua file `.exe` tersebut di dalam satu folder yang sama:

```text
📁 Android_Tools_Windows/
 ├── 📄 AndroidToolsGUI.exe  (Tampilan aplikasi C#)
 └── 📄 android_tools.exe    (Mesin backend Go)
```

Kompres folder tersebut menjadi **`.zip`** dan kirimkan ke pengguna Windows. Ketika mereka mengekstrak `.zip` tersebut dan menjalankan `AndroidToolsGUI.exe`, aplikasi akan bekerja secara utuh!

---

## 🔍 Referensi Fitur: SamFw Tool (Untuk Pengembangan Selanjutnya)

SamFw Tool adalah program All-in-One yang sangat populer untuk servis HP Android (terutama Samsung, Xiaomi, dan LG). Anda bisa menjadikan daftar fitur mereka sebagai ide/roadmap untuk ditambahkan ke backend Go Anda:

### 1. Mode ADB & Fastboot (Android Umum)
*   **Read Info:** Membaca informasi detail perangkat (versi Android, tipe keamanan, IMEI, dll.).
*   **Bypass FRP:** Menghapus kunci Akun Google setelah Factory Reset.
*   **Disable Knox / Factory Mode:** Mematikan sistem keamanan Knox Samsung.
*   **Change CSC:** Mengubah kode regional HP (misal mengubah HP regional luar negeri menjadi regional Indonesia).

### 2. Mode MediaTek (MTK) - *Sudah Mulai Anda Rintis*
*   **Bypass Auth (BROM):** Melewati proteksi bootloader MediaTek secara paksa untuk flashing tanpa akun otorisasi resmi.
*   **Format Data / Factory Reset:** Mereset HP terkunci via kabel data menggunakan protokol preloader MTK.
*   **Read/Write Partition:** Membaca atau menulis partisi Android (`userdata`, `system`, `boot`) langsung via USB.

### 3. Mode Odin / EDL (Flashing)
*   **Qualcomm EDL (9008):** Mengunggah firmware ke HP Qualcomm yang mati total (*hardbrick*).
*   **Odin Flash:** Mengunggah file `.tar` firmware resmi Samsung langsung dari PC.

---

## 🔎 Hasil Analisis Folder SamFwTool (Bagaimana Mereka Bekerja)

Setelah menganalisis berkas di folder `SamFwTool/data/`, terungkap bahwa SamFw Tool menggunakan arsitektur **Wrapper GUI** yang sama dengan yang kita bangun:

1.  **C# Windows Forms (Frontend):** Berfungsi mengontrol tombol-tombol pada UI.
2.  **Helper Binaries (Backend di `data/`):**
    *   `7z.exe` & `7za.exe` -> Mengekstrak arsip firmware.
    *   `adb.exe` & `fastboot.exe` -> Berinteraksi dengan Android.
    *   `LGUP_Cmd.exe` -> Flashing khusus perangkat LG.
    *   `mtk_module` -> Penanganan bypass preloader MediaTek.
    *   `FacRst.apk` & `samfw-protocol.apk` -> APK pembantu yang diinstal ke HP untuk membuka celah FRP.

**Kelebihan Aplikasi Anda:**
Aplikasi Anda jauh lebih efisien karena fungsi seperti pemantauan USB dan ekstraksi tar/tgz langsung digabungkan secara native di dalam mesin Go (`android_tools.exe`), sehingga meminimalisir ketergantungan pada banyak berkas biner eksternal.
