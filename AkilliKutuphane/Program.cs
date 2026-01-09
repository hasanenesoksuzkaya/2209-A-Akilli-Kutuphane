using Microsoft.EntityFrameworkCore;
using AkilliKutuphane.Data;

// Hocam, burada ASP.NET Core uygulamasını ayağa kaldırıyoruz
// WebApplication.CreateBuilder uygulamanın temel yapılandırmasını oluşturuyor
var builder = WebApplication.CreateBuilder(args);

// ==================== SERVİS KAYITLARI ====================
// Hocam, bu bölümde uygulamanın kullanacağı servisleri ekliyoruz

// MVC Controller ve View desteğini ekliyoruz
builder.Services.AddControllersWithViews();

// Hocam, burası çok önemli! SQLite veritabanı bağlantısını burada yapıyoruz
// appsettings.json dosyasından "VarsayilanBaglanti" connection string'ini okuyoruz
// Sonra Entity Framework'ün SQLite kullanmasını söylüyoruz
builder.Services.AddDbContext<KutuphaneDbContext>(options =>
{
    // Connection string'i appsettings.json'dan alıyoruz
    var connectionString = builder.Configuration.GetConnectionString("VarsayilanBaglanti");
    
    // SQLite veritabanını kullanacağımızı belirtiyoruz
    options.UseSqlite(connectionString);
});

// ==================== UYGULAMA YAPILANDIRMASI ====================
var app = builder.Build();

// Hocam, uygulama başladığında veritabanını otomatik oluşturmak için bu bloğu ekledim
// Böylece manuel migration yapmamıza gerek kalmıyor - prototip için daha pratik
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Veritabanı context'ini alıyoruz
        var context = services.GetRequiredService<KutuphaneDbContext>();
        
        // Veritabanını oluştur (eğer yoksa) ve migration'ları uygula
        // EnsureCreated metodu tabloları otomatik oluşturuyor
        context.Database.EnsureCreated();
        
        // Hocam, konsola bilgi mesajı yazdırıyorum - debug için faydalı
        Console.WriteLine(">>> Veritabanı başarıyla oluşturuldu veya zaten mevcut!");
    }
    catch (Exception ex)
    {
        // Hocam, veritabanı bağlantısında hata olursa burası çalışacak
        // Hatayı konsola yazdırıyorum ki nereden kaynaklandığını görebilelim
        Console.WriteLine($">>> Veritabanı oluşturulurken hata: {ex.Message}");
    }
}

// Development (geliştirme) modunda değilsek hata sayfasını göster
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS yönlendirmesi - güvenlik için
app.UseHttpsRedirection();

// wwwroot klasöründeki statik dosyaları (CSS, JS, resimler) sunmak için
app.UseStaticFiles();

// Routing (yönlendirme) sistemini aktif et
app.UseRouting();

// Yetkilendirme middleware'i (şimdilik kullanmıyoruz ama ileride lazım olabilir)
app.UseAuthorization();

// Hocam, varsayılan route ayarını burada yapıyoruz
// Kullanıcı siteye girdiğinde Home controller'ın Index action'ı çalışacak
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Hocam, uygulamayı başlatıyoruz!
Console.WriteLine(">>> Akıllı Kütüphane Sistemi başlatılıyor...");
Console.WriteLine(">>> TÜBİTAK 2209-A Projesi - Hasan Enes Öksüzkaya");
app.Run();
