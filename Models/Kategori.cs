using System.ComponentModel.DataAnnotations;

namespace KitapSatisSitesi.Models
{
    public class Kategori
    {
        public int KategoriID { get; set; }
        
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;
        
        public virtual ICollection<Kitap> Kitaplar { get; set; } = new List<Kitap>();
    }
} 