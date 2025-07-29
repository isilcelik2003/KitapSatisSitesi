using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapSatisSitesi.Models
{
    public class Odeme
    {
        public int OdemeID { get; set; }
        
        [Required]
        public int SiparisID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OdemeTipi { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Tutar { get; set; }
        
        [Required]
        public DateTime OdemeTarihi { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(50)]
        public string OdemeDurumu { get; set; } = string.Empty;
        
        // Navigation Properties
        public virtual Siparis Siparis { get; set; } = null!;
    }
} 