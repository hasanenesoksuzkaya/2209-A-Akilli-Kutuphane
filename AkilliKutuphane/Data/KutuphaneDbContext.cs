using Microsoft.EntityFrameworkCore;
using AkilliKutuphane.Models;

namespace AkilliKutuphane.Data
{
    // Hocam, bu sınıf veritabanı ile uygulama arasındaki köprü görevi görüyor.
    // Entity Framework Core kullanarak SQLite veritabanına bağlanıyoruz.
    // Tüm tablolar (Ogrenci, Masa, Rezervasyon) burada tanımlanıyor.
    public class KutuphaneDbContext : DbContext
    {
        // Hocam, Constructor'da options parametresi alıyoruz
        // Bu sayede Program.cs'den bağlantı ayarlarını buraya geçirebiliyoruz
        public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options) : base(options)
        {
        }

        // Öğrenciler tablosu - kütüphaneyi kullanan öğrencilerin listesi
        public DbSet<Ogrenci> Ogrenciler { get; set; }

        // Masalar tablosu - kütüphanedeki tüm masaların listesi
        public DbSet<Masa> Masalar { get; set; }

        // Rezervasyonlar tablosu - hangi öğrenci hangi masada oturuyor bilgisi
        public DbSet<Rezervasyon> Rezervasyonlar { get; set; }

        // Hocam, OnModelCreating metodu veritabanı oluşturulurken çağrılıyor
        // Burada tablolar arası ilişkileri ve başlangıç verilerini tanımlıyorum
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Hocam, Ogrenci tablosu için öğrenci numarasının benzersiz olmasını sağlıyorum
            // Aynı öğrenci numarasıyla iki kayıt olmasın diye
            modelBuilder.Entity<Ogrenci>()
                .HasIndex(o => o.OgrenciNo)
                .IsUnique();

            // Masa numarası da benzersiz olmalı - iki tane "A-01" masası olmaz
            modelBuilder.Entity<Masa>()
                .HasIndex(m => m.MasaNo)
                .IsUnique();

            // Hocam, burada başlangıç için birkaç örnek masa ekliyorum
            // Böylece uygulama ilk açıldığında test edebileceğimiz veriler olacak
            modelBuilder.Entity<Masa>().HasData(
                new Masa { Id = 1, MasaNo = "A-01", Konum = "Zemin Kat", Kapasite = 4, DoluMu = false, AktifMi = true },
                new Masa { Id = 2, MasaNo = "A-02", Konum = "Zemin Kat", Kapasite = 4, DoluMu = false, AktifMi = true },
                new Masa { Id = 3, MasaNo = "A-03", Konum = "Zemin Kat", Kapasite = 2, DoluMu = false, AktifMi = true },
                new Masa { Id = 4, MasaNo = "B-01", Konum = "1. Kat", Kapasite = 4, DoluMu = false, AktifMi = true },
                new Masa { Id = 5, MasaNo = "B-02", Konum = "1. Kat", Kapasite = 4, DoluMu = false, AktifMi = true },
                new Masa { Id = 6, MasaNo = "B-03", Konum = "1. Kat - Sessiz Bölüm", Kapasite = 1, DoluMu = false, AktifMi = true },
                new Masa { Id = 7, MasaNo = "B-04", Konum = "1. Kat - Sessiz Bölüm", Kapasite = 1, DoluMu = false, AktifMi = true },
                new Masa { Id = 8, MasaNo = "C-01", Konum = "2. Kat", Kapasite = 6, DoluMu = false, AktifMi = true },
                new Masa { Id = 9, MasaNo = "C-02", Konum = "2. Kat", Kapasite = 6, DoluMu = false, AktifMi = true },
                new Masa { Id = 10, MasaNo = "C-03", Konum = "2. Kat", Kapasite = 4, DoluMu = false, AktifMi = true }
            );

            // Hocam, bir de test için örnek öğrenci ekliyorum - kendim :)
            modelBuilder.Entity<Ogrenci>().HasData(
                new Ogrenci 
                { 
                    Id = 1, 
                    OgrenciNo = "24301071014", 
                    AdSoyad = "Hasan Enes Öksüzkaya", 
                    Eposta = "hasenenes@ogrenci.kayseri.edu.tr",
                    Bolum = "Bilgisayar Programcılığı",
                    KayitTarihi = new DateTime(2024, 9, 15)
                }
            );
        }
    }
}
