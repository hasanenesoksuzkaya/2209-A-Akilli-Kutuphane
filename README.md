# Akıllı Kütüphane - Kurulum ve Kullanım Kılavuzu

## 🎯 Proje Hakkında

Bu proje, TÜBİTAK 2209-A kapsamında Kayseri Üniversitesi öğrencisi Hasan Enes Öksüzkaya tarafından geliştirilmiştir. Üniversite kütüphanelerinde masa doluluk takibi yapılmasını sağlayan mobil uyumlu bir web sistemidir.

## 🚀 Hızlı Başlangıç

### Gereksinimler
- .NET 8.0 SDK
- Python 3.x (QR kod üretimi için)
- Modern bir web tarayıcısı

### Web Uygulamasını Çalıştırma

```powershell
# 1. Proje klasörüne git
cd AkilliKutuphane

# 2. Paketleri yükle
dotnet restore

# 3. Uygulamayı başlat
dotnet run
```

Tarayıcıda açın: **http://localhost:5000**

### QR Kodları Üretme

```powershell
# 1. Python klasörüne git
cd QR_Kod_Uretici

# 2. Gerekli kütüphaneleri yükle
pip install -r requirements.txt

# 3. QR kodları üret
python qr_uretici.py
```

QR kodlar `QR_Kodlar` klasörüne kaydedilecektir.

## 📱 Kullanım

### Öğrenci olarak:
1. Kütüphanedeki masanın QR kodunu okutun
2. Öğrenci numaranızı girin
3. Kaç saat oturacağınızı seçin
4. Çıkarken "Çıkış Yap" butonuna basın

### Test için:
- Örnek öğrenci no: `24301071014`
- Mevcut masalar: A-01, A-02, A-03, B-01, B-02, B-03, B-04, C-01, C-02, C-03

## 📂 Proje Yapısı

```
├── AkilliKutuphane/        # ASP.NET Core Web Uygulaması
├── QR_Kod_Uretici/         # Python QR Script
├── Proje_Raporu.md         # Detaylı proje raporu
└── README.md               # Bu dosya
```

## 👨‍💻 Geliştirici Bilgileri

- **Proje Sahibi:** Hasan Enes Öksüzkaya
- **Danışman:** Öğr. Gör. Nurullah Öztürk
- **Üniversite:** Kayseri Üniversitesi
- **Bölüm:** Bilgisayar Programcılığı

---
*TÜBİTAK 2209-A Üniversite Öğrencileri Araştırma Projeleri Destekleme Programı*
