using System.ComponentModel.DataAnnotations;

namespace AkilliKutuphane.Models
{
    // Hocam, bu sınıf kütüphaneyi kullanan öğrencilerin bilgilerini tutuyor.
    // Her öğrencinin bir numarası ve adı var, kartıyla giriş yapınca bu bilgiler kullanılacak.
    public class Ogrenci
    {
        // Hocam, bu Id alanı veritabanında otomatik artan birincil anahtar olacak.
        [Key]
        public int Id { get; set; }

        // Öğrenci numarası - mesela benim numaram 2210101025 gibi
        // Required koydum çünkü öğrenci numarası olmadan kayıt olmasın
        [Required(ErrorMessage = "Öğrenci numarası zorunludur!")]
        [StringLength(20)]
        public string OgrenciNo { get; set; } = string.Empty;

        // Öğrencinin adı soyadı
        [Required(ErrorMessage = "Ad Soyad zorunludur!")]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        // Öğrencinin e-posta adresi (opsiyonel tuttum ama ileride bildirim için lazım olabilir)
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz!")]
        public string? Eposta { get; set; }

        // Hocam, bu alan öğrencinin hangi bölümde okuduğunu tutuyor
        // Benim gibi Bilgisayar Programcılığı öğrencileri olabilir mesela
        [StringLength(100)]
        public string? Bolum { get; set; }

        // Öğrenci ne zaman sisteme kayıt oldu, bunu da tutuyoruz
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        // Hocam, bir öğrenci birden fazla rezervasyon yapabilir
        // Bu yüzden burada liste olarak tuttum (1-N ilişki)
        public virtual ICollection<Rezervasyon>? Rezervasyonlar { get; set; }
    }
}
