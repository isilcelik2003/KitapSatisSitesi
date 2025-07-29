using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapSatisSitesi.Models
{
    public class Siparis
    {
        public int SiparisID { get; set; }
        
        [Required]
        public int KullaniciID { get; set; }
        
        [Required]
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ToplamTutar { get; set; }
        
        [StringLength(100)]
        public string? Durum { get; set; }
        
        // Navigation Properties
        public virtual Kullanici Kullanici { get; set; } = null!;
        public virtual ICollection<SiparisDetay> SiparisDetaylar { get; set; } = new List<SiparisDetay>();
        
        // Geriye dönük uyumluluk için (eski siparişler için)
        public int? KitapID { get; set; }
        public virtual Kitap? Kitap { get; set; }
    }
} 