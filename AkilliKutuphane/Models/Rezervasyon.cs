using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkilliKutuphane.Models
{
    // Hocam, bu sınıf en önemli tablo aslında!
    // Öğrenci ile Masa arasındaki ilişkiyi tutuyor.
    // Yani kim, hangi masada, ne kadar süre oturuyor - hepsini burada tutuyoruz.
    public class Rezervasyon
    {
        [Key]
        public int Id { get; set; }

        // Hangi öğrenci bu rezervasyonu yaptı?
        // Foreign Key olarak Ogrenci tablosuna bağlıyoruz
        [Required]
        public int OgrenciId { get; set; }

        // Hangi masayı rezerve etti?
        // Foreign Key olarak Masa tablosuna bağlıyoruz
        [Required]
        public int MasaId { get; set; }

        // Öğrenci masaya ne zaman oturdu?
        // QR kodu okuttuğu an bu tarih yazılacak
        public DateTime BaslangicZamani { get; set; } = DateTime.Now;

        // Öğrenci kaç saat oturacağını seçecek (1-6 saat arası)
        // Hocam, bu süre dolunca sistem otomatik olarak masayı boşaltabilir
        [Required]
        [Range(1, 6, ErrorMessage = "1 ile 6 saat arası seçiniz!")]
        public int PlanlananSure { get; set; } = 1; // Varsayılan 1 saat

        // Tahmini bitiş zamanı - başlangıç + planlanan süre
        public DateTime TahminibitisZamani { get; set; }

        // Öğrenci gerçekte ne zaman çıktı?
        // Çıkış yapmadıysa bu alan null olacak
        public DateTime? GercekCikisZamani { get; set; }

        // Hocam, rezervasyonun durumunu burada tutuyorum
        // Aktif = öğrenci masada oturuyor
        // Tamamlandi = öğrenci çıkış yaptı
        // IptalEdildi = öğrenci gelmedi veya iptal etti
        // SuresiDoldu = planlanan süre doldu ama çıkış yapmadı
        [StringLength(20)]
        public string Durum { get; set; } = "Aktif";

        // Navigation Property - bu rezervasyon hangi öğrenciye ait?
        // Hocam, Entity Framework bu alanları kullanarak tabloları birbirine bağlıyor
        [ForeignKey("OgrenciId")]
        public virtual Ogrenci? Ogrenci { get; set; }

        // Navigation Property - bu rezervasyon hangi masaya ait?
        [ForeignKey("MasaId")]
        public virtual Masa? Masa { get; set; }
    }
}
