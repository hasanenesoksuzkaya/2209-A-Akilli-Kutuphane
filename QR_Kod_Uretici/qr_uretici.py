"""
QR Kod Üretici - Akıllı Kütüphane Projesi
=========================================
Hocam, bu Python scripti kütüphanedeki masalar için QR kod üretiyor.
Her QR kod tarandığında ilgili masanın rezervasyon sayfasına yönlendiriyor.

TÜBİTAK 2209-A Projesi
Geliştirici: Hasan Enes Öksüzkaya
Danışman: Öğr. Gör. Nurullah Öztürk
"""

import qrcode  # Hocam, QR kod oluşturmak için bu kütüphaneyi kullanıyorum
import os

# Hocam, masaların listesi ve bilgileri
# Bu bilgiler veritabanındaki Masalar tablosuyla aynı olmalı
masalar = [
    {"no": "A-01", "konum": "Zemin Kat", "kapasite": 4},
    {"no": "A-02", "konum": "Zemin Kat", "kapasite": 4},
    {"no": "A-03", "konum": "Zemin Kat", "kapasite": 2},
    {"no": "B-01", "konum": "1. Kat", "kapasite": 4},
    {"no": "B-02", "konum": "1. Kat", "kapasite": 4},
    {"no": "B-03", "konum": "1. Kat - Sessiz Bölüm", "kapasite": 1},
    {"no": "B-04", "konum": "1. Kat - Sessiz Bölüm", "kapasite": 1},
    {"no": "C-01", "konum": "2. Kat", "kapasite": 6},
    {"no": "C-02", "konum": "2. Kat", "kapasite": 6},
    {"no": "C-03", "konum": "2. Kat", "kapasite": 4},
]

# Hocam, web uygulamasının çalıştığı adres
# Localhost olarak ayarladım, sunucuya yüklenince değişecek
BASE_URL = "http://localhost:5000"

def qr_kodlari_olustur():
    """
    Hocam, bu fonksiyon tüm masalar için QR kod oluşturuyor.
    Her QR kod bir PNG dosyası olarak kaydediliyor.
    """
    
    # Hocam, QR kodları kaydedeceğimiz klasörü oluşturuyorum
    qr_klasoru = "QR_Kodlar"
    if not os.path.exists(qr_klasoru):
        os.makedirs(qr_klasoru)
        print(f"'{qr_klasoru}' klasörü oluşturuldu.")
    
    print("\n" + "=" * 50)
    print("QR KOD ÜRETİCİ - Akıllı Kütüphane")
    print("=" * 50)
    
    # Hocam, her masa için döngü ile QR kod üretiyorum
    for masa in masalar:
        masa_no = masa["no"]
        konum = masa["konum"]
        
        # QR kod içeriği - bu URL tarandığında açılacak
        # Hocam, QR tarandığında direkt rezervasyon sayfası açılsın diye
        # masa numarasını parametre olarak gönderiyorum
        qr_icerik = f"{BASE_URL}/Masa/QRTara?masaNo={masa_no}"
        
        # QR kod oluşturma
        # Hocam, qrcode kütüphanesinin ayarlarını burada yapıyorum
        qr = qrcode.QRCode(
            version=1,  # QR kodun boyutu (1-40 arası)
            error_correction=qrcode.constants.ERROR_CORRECT_M,  # Hata düzeltme seviyesi
            box_size=10,  # Her kutucuğun piksel boyutu
            border=4,  # Kenarlık kalınlığı
        )
        
        qr.add_data(qr_icerik)
        qr.make(fit=True)
        
        # QR kodu resme çevir
        qr_resim = qr.make_image(fill_color="black", back_color="white")
        
        # Dosya adı - masa numarasındaki tire'yi alt çizgiye çeviriyorum
        dosya_adi = f"{qr_klasoru}/Masa_{masa_no.replace('-', '_')}.png"
        
        # Kaydet
        qr_resim.save(dosya_adi)
        
        print(f"✓ {masa_no} - {konum} --> {dosya_adi}")
    
    print("\n" + "=" * 50)
    print(f"Toplam {len(masalar)} adet QR kod oluşturuldu!")
    print(f"QR kodlar '{qr_klasoru}' klasörüne kaydedildi.")
    print("=" * 50)
 

def tek_qr_olustur(masa_no):
    """
    Hocam, bu fonksiyon tek bir masa için QR kod oluşturuyor.
    Yeni masa eklendiğinde bu fonksiyonu kullanabiliriz.
    """
    
    qr_klasoru = "QR_Kodlar"
    if not os.path.exists(qr_klasoru):
        os.makedirs(qr_klasoru)
    
    qr_icerik = f"{BASE_URL}/Masa/QRTara?masaNo={masa_no}"
    
    qr = qrcode.QRCode(
        version=1,
        error_correction=qrcode.constants.ERROR_CORRECT_M,
        box_size=10,
        border=4,
    )
    
    qr.add_data(qr_icerik)
    qr.make(fit=True)
    
    qr_resim = qr.make_image(fill_color="black", back_color="white")
    
    dosya_adi = f"{qr_klasoru}/Masa_{masa_no.replace('-', '_')}.png"
    qr_resim.save(dosya_adi)
    
    print(f"QR kod oluşturuldu: {dosya_adi}")
    return dosya_adi


# Hocam, script direkt çalıştırılırsa bu kısım devreye giriyor
if __name__ == "__main__":
    print("\n🏫 Kayseri Üniversitesi - Akıllı Kütüphane Projesi")
    print("📱 QR Kod Üretici v1.0")
    print("👨‍💻 Geliştirici: Hasan Enes Öksüzkaya\n")
    
    # Tüm masalar için QR kodları oluştur
    qr_kodlari_olustur()
    
    print("\n💡 İpucu: Bu QR kodları yazdırıp masaların üzerine yapıştırın!")
    print("📱 Öğrenciler telefonlarıyla QR'ı okutunca rezervasyon sayfası açılacak.\n")
