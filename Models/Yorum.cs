using System.ComponentModel.DataAnnotations;

namespace KitapSatisSitesi.Models
{
    public class Yorum
    {
        public int YorumID { get; set; }
        
        [Required(ErrorMessage = "Yorum metni zorunludur")]
        [StringLength(500, ErrorMessage = "Yorum en fazla 500 karakter olabilir")]
        public string Metin { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Puan zorunludur")]
        [Range(1, 5, ErrorMessage = "Puan 1-5 arasında olmalıdır")]
        public int Puan { get; set; }
        
        public DateTime Tarih { get; set; } = DateTime.Now;
        
        // Foreign Keys
        public int KitapID { get; set; }
        public int KullaniciID { get; set; }
        
        // Navigation Properties
        public virtual Kitap Kitap { get; set; } = null!;
        public virtual Kullanici Kullanici { get; set; } = null!;
    }
} 