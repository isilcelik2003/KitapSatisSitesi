using System.ComponentModel.DataAnnotations;

namespace KitapSatisSitesi.Models
{
    public class Kullanici
    {
        public int KullaniciID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Ad { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Soyad { get; set; } = string.Empty;
        
        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Parola { get; set; } = string.Empty;
        
        [Required]
        [StringLength(15)]
        public string Telefon { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<Siparis> Siparisler { get; set; } = new List<Siparis>();
        public virtual ICollection<Yorum> Yorumlar { get; set; } = new List<Yorum>();

        // Computed property
        public string TamAd => $"{Ad} {Soyad}";
    }
} 