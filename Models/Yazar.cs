using System.ComponentModel.DataAnnotations;

namespace KitapSatisSitesi.Models
{
    public class Yazar
    {
        public int YazarID { get; set; }
        
        [Required(ErrorMessage = "Yazar adı zorunludur")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Yazar soyadı zorunludur")]
        [StringLength(100, ErrorMessage = "Yazar soyadı en fazla 100 karakter olabilir")]
        public string Soyad { get; set; } = string.Empty;
        
        public string TamAd => $"{Ad} {Soyad}";
        
        public virtual ICollection<Kitap> Kitaplar { get; set; } = new List<Kitap>();
    }
} 