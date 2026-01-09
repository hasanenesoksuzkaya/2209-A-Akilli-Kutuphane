# -*- coding: utf-8 -*-
"""
QR Kod Uretici - Akilli Kutuphane Projesi
=========================================
Bu Python scripti kutuphanedeki masalar icin QR kod uretiyor.
Her QR kod tarandiginda ilgili masanin rezervasyon sayfasina yonlendiriyor.

TUBITAK 2209-A Projesi
Gelistirici: Hasan Enes Oksuzxkaya
Danisman: Ogr. Gor. Nurullah Ozturk
"""

import qrcode
import os

# Masalarin listesi ve bilgileri
# Bu bilgiler veritabanindaki Masalar tablosuyla ayni olmali
masalar = [
    {"no": "A-01", "konum": "Zemin Kat", "kapasite": 4},
    {"no": "A-02", "konum": "Zemin Kat", "kapasite": 4},
    {"no": "A-03", "konum": "Zemin Kat", "kapasite": 2},
    {"no": "B-01", "konum": "1. Kat", "kapasite": 4},
    {"no": "B-02", "konum": "1. Kat", "kapasite": 4},
    {"no": "B-03", "konum": "1. Kat - Sessiz Bolum", "kapasite": 1},
    {"no": "B-04", "konum": "1. Kat - Sessiz Bolum", "kapasite": 1},
    {"no": "C-01", "konum": "2. Kat", "kapasite": 6},
    {"no": "C-02", "konum": "2. Kat", "kapasite": 6},
    {"no": "C-03", "konum": "2. Kat", "kapasite": 4},
]

# Web uygulamasinin calistigi adres
# Localhost olarak ayarlandi, sunucuya yuklenince degisecek
BASE_URL = "http://localhost:5000"

def qr_kodlari_olustur():
    """
    Bu fonksiyon tum masalar icin QR kod olusturuyor.
    Her QR kod bir PNG dosyasi olarak kaydediliyor.
    """
    
    # QR kodlari kaydedeceginiz klasoru olusturun
    qr_klasoru = "QR_Kodlar"
    if not os.path.exists(qr_klasoru):
        os.makedirs(qr_klasoru)
        print(f"'{qr_klasoru}' klasoru olusturuldu.")
    
    print("\n" + "=" * 50)
    print("QR KOD URETICI - Akilli Kutuphane")
    print("=" * 50)
    
    # Her masa icin dongu ile QR kod uretiliyor
    for masa in masalar:
        masa_no = masa["no"]
        konum = masa["konum"]
        
        # QR kod icerigi - bu URL tarandiginda acilacak
        # QR tarandiginda direkt rezervasyon sayfasi acilsin diye
        # masa numarasini parametre olarak gonderiliyor
        qr_icerik = f"{BASE_URL}/Masa/QRTara?masaNo={masa_no}"
        
        # QR kod olusturma
        # qrcode kutuphanesinin ayarlari burada yapiliyor
        qr = qrcode.QRCode(
            version=1,  # QR kodun boyutu (1-40 arasi)
            error_correction=qrcode.constants.ERROR_CORRECT_M,  # Hata duzeltme seviyesi
            box_size=10,  # Her kutucugun piksel boyutu
            border=4,  # Kenarlik kalinligi
        )
        
        qr.add_data(qr_icerik)
        qr.make(fit=True)
        
        # QR kodu resme cevir
        qr_resim = qr.make_image(fill_color="black", back_color="white")
        
        # Dosya adi - masa numarasindaki tire'yi alt cizgiye ceviriliyor
        dosya_adi = f"{qr_klasoru}/Masa_{masa_no.replace('-', '_')}.png"
        
        # Kaydet
        qr_resim.save(dosya_adi)
        
        print(f"+ {masa_no} - {konum} --> {dosya_adi}")
    
    print("\n" + "=" * 50)
    print(f"Toplam {len(masalar)} adet QR kod olusturuldu!")
    print(f"QR kodlar '{qr_klasoru}' klasorune kaydedildi.")
    print("=" * 50)


def tek_qr_olustur(masa_no):
    """
    Bu fonksiyon tek bir masa icin QR kod olusturuyor.
    Yeni masa eklendiginde bu fonksiyonu kullanabilirsiniz.
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
    
    print(f"QR kod olusturuldu: {dosya_adi}")
    return dosya_adi


# Script direkt calistirilirsa bu kisim devreye giriyor
if __name__ == "__main__":
    print("\n*** Kayseri Universitesi - Akilli Kutuphane Projesi")
    print("*** QR Kod Uretici v1.0")
    print("*** Gelistirici: Hasan Enes Oksuzxkaya\n")
    
    # Tum masalar icin QR kodlari olustur
    qr_kodlari_olustur()
    
    print("\n*** Ipucu: Bu QR kodlari yazdirip masalarin uzerine yapistirin!")
    print("*** Ogrenciler telefonlariyla QR'i okutunca rezervasyon sayfasi acilacak.\n")
