using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AkilliKutuphane.Data;
using AkilliKutuphane.Models;

namespace AkilliKutuphane.Controllers
{
    // Hocam, bu controller kütüphanedeki masaları yöneten ana controller
    // Masa listeleme, detay görme, QR okutma, rezervasyon işlemleri hep burada
    public class MasaController : Controller
    {
        // Veritabanı bağlantısı için context
        private readonly KutuphaneDbContext _context;

        // Hocam, constructor'da dependency injection ile context'i alıyoruz
        public MasaController(KutuphaneDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // TÜM MASALARI LİSTELEME
        // Hocam, bu action kütüphanedeki tüm masaları listeler
        // Öğrenciler buradan hangi masaların boş olduğunu görecek
        // =====================================================
        public async Task<IActionResult> Index()
        {
            // Veritabanından tüm masaları çekiyoruz
            var masalar = await _context.Masalar
                .Where(m => m.AktifMi == true) // Sadece aktif masaları getir
                .OrderBy(m => m.Konum) // Konuma göre sırala
                .ThenBy(m => m.MasaNo) // Sonra masa numarasına göre
                .ToListAsync();

            return View(masalar);
        }

        // =====================================================
        // KATA GÖRE MASALARI FİLTRELEME
        // Hocam, öğrenci belirli bir kattaki masaları görmek isteyebilir
        // =====================================================
        public async Task<IActionResult> KataGore(string konum)
        {
            var masalar = await _context.Masalar
                .Where(m => m.AktifMi == true && m.Konum.Contains(konum))
                .OrderBy(m => m.MasaNo)
                .ToListAsync();

            ViewBag.SeciliKonum = konum;
            return View("Index", masalar);
        }

        // =====================================================
        // QR KOD TARAMA SİMÜLASYONU
        // Hocam, gerçek QR okuyucu yerine masa numarasını manuel giriyoruz
        // Prototip aşamasında bu yeterli olacak
        // =====================================================
        public IActionResult QRTara()
        {
            return View();
        }

        // QR kod tarandıktan sonra (masa numarası girilince) burası çalışır
        [HttpPost]
        public async Task<IActionResult> QRTara(string masaNo)
        {
            // Hocam, girilen masa numarasıyla veritabanında masa arıyoruz
            if (string.IsNullOrEmpty(masaNo))
            {
                ViewBag.Hata = "Lütfen masa numarası giriniz!";
                return View();
            }

            var masa = await _context.Masalar
                .FirstOrDefaultAsync(m => m.MasaNo == masaNo.ToUpper());

            if (masa == null)
            {
                ViewBag.Hata = "Bu numarada bir masa bulunamadı!";
                return View();
            }

            // Masa bulunduysa rezervasyon sayfasına yönlendir
            return RedirectToAction("Rezervasyon", new { masaId = masa.Id });
        }

        // =====================================================
        // REZERVASYON SAYFASI
        // Hocam, öğrenci QR okuttuktan sonra bu sayfaya gelecek
        // Burada kaç saat oturacağını seçip rezervasyon yapacak
        // =====================================================
        public async Task<IActionResult> Rezervasyon(int masaId)
        {
            var masa = await _context.Masalar.FindAsync(masaId);

            if (masa == null)
            {
                return NotFound("Masa bulunamadı!");
            }

            // Eğer masa doluysa, aktif rezervasyonu da çekelim
            if (masa.DoluMu)
            {
                var aktifRezervasyon = await _context.Rezervasyonlar
                    .Include(r => r.Ogrenci)
                    .FirstOrDefaultAsync(r => r.MasaId == masaId && r.Durum == "Aktif");

                ViewBag.AktifRezervasyon = aktifRezervasyon;
            }

            return View(masa);
        }

        // =====================================================
        // REZERVASYON YAPMA (POST)
        // Hocam, öğrenci formu doldurup gönderince burası çalışır
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> RezervasyonYap(int masaId, string ogrenciNo, int sure)
        {
            // Hocam, önce masayı kontrol ediyoruz
            var masa = await _context.Masalar.FindAsync(masaId);
            if (masa == null)
            {
                return NotFound("Masa bulunamadı!");
            }

            // Masa zaten doluysa hata ver
            if (masa.DoluMu)
            {
                TempData["Hata"] = "Bu masa şu an dolu! Başka bir masa seçiniz.";
                return RedirectToAction("Rezervasyon", new { masaId = masaId });
            }

            // Öğrenciyi kontrol et veya yeni kayıt oluştur
            var ogrenci = await _context.Ogrenciler
                .FirstOrDefaultAsync(o => o.OgrenciNo == ogrenciNo);

            // Hocam, eğer öğrenci sistemde yoksa basit bir kayıt oluşturuyoruz
            // Gerçek projede öğrenci kartından bilgiler otomatik gelecek
            if (ogrenci == null)
            {
                ogrenci = new Ogrenci
                {
                    OgrenciNo = ogrenciNo,
                    AdSoyad = "Öğrenci " + ogrenciNo, // Geçici isim
                    KayitTarihi = DateTime.Now
                };
                _context.Ogrenciler.Add(ogrenci);
                await _context.SaveChangesAsync();
            }

            // Hocam, bu öğrencinin başka aktif rezervasyonu var mı kontrol ediyorum
            // Bir öğrenci aynı anda iki masada oturamaz
            var mevcutRezervasyon = await _context.Rezervasyonlar
                .FirstOrDefaultAsync(r => r.OgrenciId == ogrenci.Id && r.Durum == "Aktif");

            if (mevcutRezervasyon != null)
            {
                TempData["Hata"] = "Zaten aktif bir rezervasyonunuz var! Önce çıkış yapmalısınız.";
                return RedirectToAction("Rezervasyon", new { masaId = masaId });
            }

            // Yeni rezervasyon oluştur
            var rezervasyon = new Rezervasyon
            {
                OgrenciId = ogrenci.Id,
                MasaId = masaId,
                BaslangicZamani = DateTime.Now,
                PlanlananSure = sure,
                TahminibitisZamani = DateTime.Now.AddHours(sure),
                Durum = "Aktif"
            };

            // Masayı dolu olarak işaretle
            masa.DoluMu = true;

            // Veritabanına kaydet
            _context.Rezervasyonlar.Add(rezervasyon);
            await _context.SaveChangesAsync();

            TempData["Basari"] = $"Rezervasyon başarılı! {masa.MasaNo} numaralı masa {sure} saat için sizin.";
            return RedirectToAction("RezervasyonDetay", new { id = rezervasyon.Id });
        }

        // =====================================================
        // REZERVASYON DETAY SAYFASI
        // Hocam, öğrenci rezervasyon yaptıktan sonra bu sayfayı görür
        // Kalan süre ve çıkış butonu burada olacak
        // =====================================================
        public async Task<IActionResult> RezervasyonDetay(int id)
        {
            var rezervasyon = await _context.Rezervasyonlar
                .Include(r => r.Ogrenci)
                .Include(r => r.Masa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervasyon == null)
            {
                return NotFound("Rezervasyon bulunamadı!");
            }

            return View(rezervasyon);
        }

        // =====================================================
        // ÇIKIŞ YAPMA
        // Hocam, öğrenci kütüphaneden çıkarken bu butona basacak
        // Masa tekrar boş olarak işaretlenecek
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> CikisYap(int rezervasyonId)
        {
            var rezervasyon = await _context.Rezervasyonlar
                .Include(r => r.Masa)
                .FirstOrDefaultAsync(r => r.Id == rezervasyonId);

            if (rezervasyon == null)
            {
                return NotFound("Rezervasyon bulunamadı!");
            }

            // Hocam, rezervasyonu tamamlandı olarak işaretliyoruz
            rezervasyon.Durum = "Tamamlandi";
            rezervasyon.GercekCikisZamani = DateTime.Now;

            // Masayı boş olarak işaretle
            if (rezervasyon.Masa != null)
            {
                rezervasyon.Masa.DoluMu = false;
            }

            await _context.SaveChangesAsync();

            TempData["Basari"] = "Çıkış yapıldı! İyi günler dileriz.";
            return RedirectToAction("Index");
        }

        // =====================================================
        // AKTİF REZERVASYONLARIM
        // Hocam, öğrenci kendi aktif rezervasyonunu görmek isteyebilir
        // =====================================================
        public async Task<IActionResult> Rezervasyonlarim(string ogrenciNo)
        {
            if (string.IsNullOrEmpty(ogrenciNo))
            {
                return View(new List<Rezervasyon>());
            }

            var rezervasyonlar = await _context.Rezervasyonlar
                .Include(r => r.Masa)
                .Include(r => r.Ogrenci)
                .Where(r => r.Ogrenci.OgrenciNo == ogrenciNo)
                .OrderByDescending(r => r.BaslangicZamani)
                .ToListAsync();

            ViewBag.OgrenciNo = ogrenciNo;
            return View(rezervasyonlar);
        }

        // =====================================================
        // MASA DETAY
        // Hocam, tek bir masanın detaylarını gösterir
        // =====================================================
        public async Task<IActionResult> Detay(int id)
        {
            var masa = await _context.Masalar.FindAsync(id);

            if (masa == null)
            {
                return NotFound("Masa bulunamadı!");
            }

            // Eğer dolu ise aktif rezervasyonu da getir
            if (masa.DoluMu)
            {
                var aktifRezervasyon = await _context.Rezervasyonlar
                    .Include(r => r.Ogrenci)
                    .FirstOrDefaultAsync(r => r.MasaId == id && r.Durum == "Aktif");

                ViewBag.AktifRezervasyon = aktifRezervasyon;
            }

            return View(masa);
        }
    }
}
