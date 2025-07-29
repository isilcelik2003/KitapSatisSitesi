using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapSatisSitesi.Models
{
    public class SiparisDetay
    {
        public int SiparisDetayID { get; set; }
        
        [Required]
        public int SiparisID { get; set; }
        
        [Required]
        public int KitapID { get; set; }
        
        [Required]
        public int Adet { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal BirimFiyat { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ToplamFiyat { get; set; }
        
        // Navigation Properties
        public virtual Siparis Siparis { get; set; } = null!;
        public virtual Kitap Kitap { get; set; } = null!;
    }
} 