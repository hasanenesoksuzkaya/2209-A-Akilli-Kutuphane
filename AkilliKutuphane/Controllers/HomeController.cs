using Microsoft.AspNetCore.Mvc;
using AkilliKutuphane.Data;
using Microsoft.EntityFrameworkCore;

namespace AkilliKutuphane.Controllers
{
    // Hocam, bu controller ana sayfa ve genel sayfaları yönetiyor
    // Kullanıcı siteye ilk girdiğinde burası çalışıyor
    public class HomeController : Controller
    {
        // Veritabanı bağlantısı için context'i tutuyoruz
        private readonly KutuphaneDbContext _context;

        // Hocam, Constructor Injection ile DbContext'i alıyoruz
        // Bu sayede veritabanına erişebiliyoruz
        public HomeController(KutuphaneDbContext context)
        {
            _context = context;
        }

        // Ana sayfa - kütüphanenin genel durumunu gösteriyor
        public async Task<IActionResult> Index()
        {
            // Hocam, toplam masa sayısını ve dolu/boş durumunu hesaplıyorum
            var toplamMasa = await _context.Masalar.CountAsync();
            var doluMasaSayisi = await _context.Masalar.CountAsync(m => m.DoluMu == true);
            var bosMasaSayisi = toplamMasa - doluMasaSayisi;

            // ViewBag ile bu bilgileri View'a gönderiyorum
            ViewBag.ToplamMasa = toplamMasa;
            ViewBag.DoluMasa = doluMasaSayisi;
            ViewBag.BosMasa = bosMasaSayisi;

            // Hocam, doluluk oranını yüzde olarak hesaplıyorum
            if (toplamMasa > 0)
            {
                ViewBag.DolulukOrani = (doluMasaSayisi * 100) / toplamMasa;
            }
            else
            {
                ViewBag.DolulukOrani = 0;
            }

            return View();
        }

        // Hakkında sayfası - proje hakkında bilgi
        public IActionResult Hakkinda()
        {
            return View();
        }

        // Hata sayfası
        public IActionResult Error()
        {
            return View();
        }
    }
}
