using System.ComponentModel.DataAnnotations;

namespace AkilliKutuphane.Models
{
    // Hocam, bu sınıf kütüphanedeki masaları temsil ediyor.
    // Her masanın bir numarası var ve QR kodu bu numaraya göre oluşturulacak.
    public class Masa
    {
        // Masanın veritabanındaki benzersiz Id'si
        [Key]
        public int Id { get; set; }

        // Masanın numarası - kütüphanedeki fiziksel masa numarası
        // Mesela "A-01", "A-02" veya sadece "1", "2" olabilir
        [Required(ErrorMessage = "Masa numarası zorunludur!")]
        [StringLength(20)]
        public string MasaNo { get; set; } = string.Empty;

        // Hocam, masanın hangi katta veya bölümde olduğunu tutuyorum
        // Mesela "Zemin Kat", "1. Kat", "Sessiz Çalışma Bölümü" gibi
        [StringLength(50)]
        public string? Konum { get; set; }

        // Masanın kaç kişilik olduğu - bazı masalar 4 kişilik, bazıları 2 kişilik olabilir
        public int Kapasite { get; set; } = 1;

        // ÖNEMLİ: Masanın şu an dolu mu boş mu olduğunu gösteren alan
        // true = masa dolu, false = masa boş
        // Hocam, projenin en kritik alanı burası aslında
        public bool DoluMu { get; set; } = false;

        // Masa aktif mi? Mesela tamir için kapatılmış olabilir
        public bool AktifMi { get; set; } = true;

        // Hocam, her masaya ait birden fazla rezervasyon olabilir (farklı zamanlarda)
        public virtual ICollection<Rezervasyon>? Rezervasyonlar { get; set; }
    }
}
