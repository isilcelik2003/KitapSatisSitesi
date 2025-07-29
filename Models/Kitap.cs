using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitapSatisSitesi.Models
{
    public class Kitap
    {
        public int KitapID { get; set; }
        
        [Required(ErrorMessage = "Kitap adı zorunludur")]
        [StringLength(200, ErrorMessage = "Kitap adı en fazla 200 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kategori zorunludur")]
        public int KategoriID { get; set; }
        
        [Required(ErrorMessage = "Yazar zorunludur")]
        public int YazarID { get; set; }
        
        [Required(ErrorMessage = "Yayın yılı zorunludur")]
        public int YayinYili { get; set; }
        
        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fiyat { get; set; }
        
        [Required(ErrorMessage = "Stok adedi zorunludur")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok adedi 0'dan büyük olmalıdır")]
        public int StokAdedi { get; set; }
        
        // Navigation Properties
        public virtual Kategori Kategori { get; set; } = null!;
        public virtual Yazar Yazar { get; set; } = null!;
        public virtual ICollection<Yorum> Yorumlar { get; set; } = new List<Yorum>();
        
        // Computed Properties
        public bool StoktaMi => StokAdedi > 0;
        
        public double OrtalamaPuan => Yorumlar.Any() ? Yorumlar.Average(y => y.Puan) : 0;
        
        public int YorumSayisi => Yorumlar.Count;
        
        [StringLength(200)]
        public string? ResimUrl { get; set; }
    }
} 