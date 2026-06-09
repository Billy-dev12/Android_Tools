#!/bin/bash

# Menghentikan script jika terjadi error
set -e

# Warna teks untuk output terminal
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${BLUE}===================================================${NC}"
echo -e "${BLUE}    Android Tools Billy - Avalonia Build Script    ${NC}"
echo -e "${BLUE}===================================================${NC}"
echo -e "Silakan pilih tipe build untuk Windows (win-x64):"
echo -e ""
echo -e "  ${GREEN}1)${NC} ${YELLOW}Portable / Self-Contained${NC}"
echo -e "     • Ukuran besar (~100 MB)"
echo -e "     • Langsung klik jalan tanpa perlu install .NET Runtime di Windows"
echo -e ""
echo -e "  ${GREEN}2)${NC} ${YELLOW}Lightweight / Framework-Dependent${NC}"
echo -e "     • Ukuran kecil (~10 MB)"
echo -e "     • Butuh install .NET Desktop Runtime dulu di komputer target"
echo -e ""
read -p "Masukkan pilihan Anda (1 atau 2): " PILIHAN

# Menghitung path root projek (mendukung symlink)
SCRIPT_DIR="$( cd "$( dirname "$(readlink -f "${BASH_SOURCE[0]}")" )" &> /dev/null && pwd )"
cd "$SCRIPT_DIR/gui_avalonia"

if [ "$PILIHAN" == "1" ]; then
    echo -e "\n${BLUE}Memulai build Portable / Self-Contained...${NC}"
    dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ../app_build/windows_avalonia_portable
    echo -e "\n${GREEN}✔ BERHASIL!${NC} File portable siap di: ${YELLOW}app_build/windows_avalonia_portable/gui_avalonia.exe${NC}"
elif [ "$PILIHAN" == "2" ]; then
    echo -e "\n${BLUE}Memulai build Lightweight / Framework-Dependent...${NC}"
    dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o ../app_build/windows_avalonia_lightweight
    echo -e "\n${GREEN}✔ BERHASIL!${NC} File lightweight siap di: ${YELLOW}app_build/windows_avalonia_lightweight/gui_avalonia.exe${NC}"
else
    echo -e "\n${YELLOW}⚠ Pilihan tidak valid. Silakan jalankan script kembali.${NC}"
    exit 1
fi
