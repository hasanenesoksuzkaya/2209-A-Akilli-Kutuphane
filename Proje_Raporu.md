# TÜBİTAK 2209-A ÜNİVERSİTE ÖĞRENCİLERİ ARAŞTIRMA PROJELERİ DESTEKLEME PROGRAMI

## PROJE RAPORU

---

### PROJE BİLGİLERİ

| Alan | Bilgi |
|------|-------|
| **Proje Adı** | Akıllı Kütüphane: QR Kod ve Öğrenci Kartı Entegrasyonuna Dayalı Masa Takip Sistemi |
| **Proje Sahibi** | Hasan Enes Öksüzkaya |
| **Öğrenci Numarası** | 24301071014 |
| **Bölüm** | Bilgisayar Programcılığı (2. Sınıf) |
| **Üniversite** | Kayseri Üniversitesi |
| **Danışman** | Öğr. Gör. Nurullah Öztürk |
| **Tarih** | Ocak 2026 |

---

## 1. ÖZET

Bu proje, üniversite kütüphanesinde öğrencilerin boş masa arayarak zaman kaybetmesini önlemek amacıyla geliştirilen mobil uyumlu bir web sistemidir. Öğrenciler, masaların üzerindeki QR kodları telefonlarıyla okutarak rezervasyon yapabilmekte ve kütüphanedeki doluluk durumunu anlık olarak takip edebilmektedir.

Proje, ASP.NET Core MVC framework'ü ile C# programlama dili kullanılarak geliştirilmiştir. Veritabanı olarak SQLite tercih edilmiş, QR kod üretimi için Python kullanılmıştır.

---

## 2. GİRİŞ

### 2.1. Problemin Tanımı

Üniversite kütüphaneleri, özellikle sınav dönemlerinde yoğun bir şekilde kullanılmaktadır. Öğrenciler kütüphaneye geldiklerinde boş masa bulmak için kat kat dolaşmak zorunda kalmaktadır. Bu durum:

- Öğrencilerin zaman kaybetmesine,
- Kütüphane içinde gereksiz hareket ve gürültüye,
- Doluluk durumunun bilinmemesi nedeniyle kütüphaneye boşuna gelmeye

neden olmaktadır.

### 2.2. Projenin Amacı

Bu projenin temel amacı:

1. Kütüphanedeki masaların doluluk durumunu anlık olarak takip etmek
2. Öğrencilerin QR kod ile hızlıca rezervasyon yapmasını sağlamak
3. Kütüphane kaynaklarının daha verimli kullanılmasını sağlamak
4. Öğrencilere mobil uyumlu, kullanımı kolay bir arayüz sunmak

---

## 3. MATERYAL VE YÖNTEM

### 3.1. Kullanılan Teknolojiler

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| **C# / ASP.NET Core MVC** | Backend geliştirme, iş mantığı |
| **Entity Framework Core** | Veritabanı işlemleri (ORM) |
| **SQLite** | Veritabanı (prototip aşaması) |
| **HTML5 / CSS3 / JavaScript** | Frontend geliştirme |
| **Bootstrap 5** | Responsive (mobil uyumlu) tasarım |
| **Python** | QR kod üretim scripti |
| **Font Awesome** | İkon kütüphanesi |

### 3.2. Sistem Mimarisi

Proje, **Monolitik MVC (Model-View-Controller)** mimarisi ile geliştirilmiştir. Bu mimari, bir öğrenci projesi için anlaşılır ve yönetilebilir bir yapı sunmaktadır.

```
AkilliKutuphane/
├── Controllers/          # İş mantığı (Controller katmanı)
│   ├── HomeController.cs
│   └── MasaController.cs
├── Models/               # Veri modelleri (Model katmanı)
│   ├── Ogrenci.cs
│   ├── Masa.cs
│   └── Rezervasyon.cs
├── Views/                # Kullanıcı arayüzü (View katmanı)
│   ├── Home/
│   ├── Masa/
│   └── Shared/
├── Data/                 # Veritabanı bağlantısı
│   └── KutuphaneDbContext.cs
├── Program.cs            # Uygulama başlangıç noktası
└── appsettings.json      # Yapılandırma ayarları
```

### 3.3. Veritabanı Tasarımı

Projede üç ana tablo bulunmaktadır:

#### 3.3.1. Ogrenciler Tablosu

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | INTEGER (PK) | Benzersiz kimlik |
| OgrenciNo | TEXT | Öğrenci numarası (benzersiz) |
| AdSoyad | TEXT | Öğrencinin adı soyadı |
| Eposta | TEXT | E-posta adresi |
| Bolum | TEXT | Bölüm adı |
| KayitTarihi | DATETIME | Sisteme kayıt tarihi |

#### 3.3.2. Masalar Tablosu

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | INTEGER (PK) | Benzersiz kimlik |
| MasaNo | TEXT | Masa numarası (örn: A-01) |
| Konum | TEXT | Masanın bulunduğu kat/bölüm |
| Kapasite | INTEGER | Kaç kişilik masa |
| DoluMu | BOOLEAN | Masa dolu mu? |
| AktifMi | BOOLEAN | Masa kullanılabilir mi? |

#### 3.3.3. Rezervasyonlar Tablosu

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | INTEGER (PK) | Benzersiz kimlik |
| OgrenciId | INTEGER (FK) | Hangi öğrenci |
| MasaId | INTEGER (FK) | Hangi masa |
| BaslangicZamani | DATETIME | Rezervasyon başlangıcı |
| PlanlananSure | INTEGER | Planlanan süre (saat) |
| TahminibitisZamani | DATETIME | Tahmini bitiş |
| GercekCikisZamani | DATETIME | Gerçek çıkış zamanı |
| Durum | TEXT | Aktif/Tamamlandi/IptalEdildi |

---

## 4. UYGULAMA

### 4.1. Sistemin Çalışma Mantığı

1. **Öğrenci kütüphaneye gelir** ve oturmak istediği masadaki QR kodu telefonuyla tarar.

2. **QR kod tarandığında** sistem açılır ve ilgili masanın sayfasına yönlendirilir.

3. **Masa boşsa**, öğrenci:
   - Öğrenci numarasını girer
   - Kaç saat oturacağını seçer (1-6 saat)
   - "Rezervasyon Yap" butonuna basar

4. **Sistem masayı "DOLU" olarak işaretler** ve diğer öğrenciler bunu anlık görebilir.

5. **Öğrenci çıkarken** "Çıkış Yap" butonuna basar ve masa tekrar "BOŞ" olur.

### 4.2. Ekran Görüntüleri ve Özellikler

#### Ana Sayfa
- Toplam masa sayısı
- Boş/dolu masa sayısı
- Doluluk oranı (yüzde)
- Hızlı erişim butonları

#### Masalar Sayfası
- Tüm masaların kart görünümü
- Kata göre filtreleme
- Boş masalar yeşil, dolu masalar kırmızı
- Her masanın konum ve kapasite bilgisi

#### QR Tarama Sayfası
- QR kod okutma simülasyonu
- Manuel masa numarası girişi
- Mevcut masa numaralarının listesi

#### Rezervasyon Sayfası
- Masa detayları
- Öğrenci numarası girişi
- Süre seçimi (1-6 saat)
- Onay butonu

#### Rezervasyon Detay Sayfası
- Masa ve öğrenci bilgileri
- Başlangıç/bitiş saatleri
- Kalan süre (canlı güncelleme)
- Çıkış yapma butonu

### 4.3. QR Kod Üretimi

QR kodlar, Python programlama dili ve `qrcode` kütüphanesi kullanılarak üretilmiştir. Her QR kod, ilgili masanın rezervasyon sayfasına yönlendiren bir URL içermektedir.

```python
# Örnek QR içeriği
"http://localhost:5000/Masa/QRTara?masaNo=A-01"
```

---

## 5. BULGULAR VE TARTIŞMA

### 5.1. Elde Edilen Sonuçlar

Prototip aşamasında geliştirilen sistem başarıyla çalışmaktadır:

- ✅ 10 adet masa tanımlanmış ve yönetilebilir durumda
- ✅ Öğrenci kaydı ve takibi yapılabiliyor
- ✅ Rezervasyon oluşturma ve sonlandırma çalışıyor
- ✅ Anlık doluluk bilgisi görüntülenebiliyor
- ✅ Mobil uyumlu tasarım tüm cihazlarda çalışıyor
- ✅ QR kodlar üretilebiliyor

### 5.2. Karşılaşılan Zorluklar

1. **Entity Framework ilişkileri**: Tablolar arası Foreign Key ilişkileri kurarken biraz zorlandım. Include() metodu ile ilişkili verileri çekmek için araştırma yaptım.

2. **Responsive tasarım**: Mobil cihazlarda kartların düzgün görünmesi için Bootstrap grid sistemini öğrenmem gerekti.

3. **DateTime işlemleri**: Kalan süre hesaplama ve gösterimi için JavaScript ile backend senkronizasyonu gerekti.

### 5.3. Öğrenilen Konular

Bu proje sürecinde:

- ASP.NET Core MVC mimarisi
- Entity Framework Core ile veritabanı işlemleri
- SQLite veritabanı kullanımı
- Bootstrap ile responsive tasarım
- C# async/await programlama
- Python ile QR kod üretimi

konularında pratik deneyim kazandım.

---

## 6. SONUÇ VE ÖNERİLER

### 6.1. Sonuç

Akıllı Kütüphane projesi, belirlenen hedefler doğrultusunda başarıyla tamamlanmıştır. Sistem, kütüphane masalarının anlık takibini ve QR kod ile hızlı rezervasyon yapılmasını sağlamaktadır.

### 6.2. Gelecek Çalışmalar

Projenin ileride geliştirilmesi için öneriler:

1. **Gerçek QR kamera entegrasyonu**: JavaScript ile kameradan QR okuma
2. **Öğrenci kartı NFC entegrasyonu**: Kart okuyucu ile otomatik giriş/çıkış
3. **Bildirim sistemi**: Süre dolmadan önce uyarı bildirimi
4. **İstatistik paneli**: Yöneticiler için detaylı raporlar
5. **Mobil uygulama**: Native Android/iOS uygulaması
6. **Randevu sistemi**: İleri tarihli masa rezervasyonu

---

## 7. KAYNAKLAR

1. Microsoft Docs - ASP.NET Core Documentation
2. Entity Framework Core Documentation
3. Bootstrap 5 Documentation
4. Python qrcode Library Documentation
5. SQLite Documentation

---

## 8. EKLER

### Ek-1: Proje Dosya Yapısı

```
2209-A_projesi_Hasan_Enes_Öksüzkaya/
├── AkilliKutuphane/              # Ana web uygulaması
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── Data/
│   ├── wwwroot/
│   ├── Program.cs
│   ├── appsettings.json
│   └── AkilliKutuphane.csproj
├── QR_Kod_Uretici/               # Python QR scripti
│   ├── qr_uretici.py
│   └── requirements.txt
├── Proje_Raporu.md               # Bu rapor
└── Rules.txt                     # Kodlama kuralları
```

### Ek-2: Projeyi Çalıştırma

#### Web Uygulaması için:
```bash
cd AkilliKutuphane
dotnet restore
dotnet run
```
Tarayıcıda: http://localhost:5000

#### QR Kod Üretici için:
```bash
cd QR_Kod_Uretici
pip install -r requirements.txt
python qr_uretici.py
```

---

**Hazırlayan:** Hasan Enes Öksüzkaya  
**Danışman:** Öğr. Gör. Nurullah Öztürk  
**Kayseri Üniversitesi - Bilgisayar Programcılığı**  
**TÜBİTAK 2209-A Projesi - Ocak 2026**
