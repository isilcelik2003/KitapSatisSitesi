namespace KitapSatisSitesi.Models
{
    public class SepetItem
    {
        public int KitapId { get; set; }
        public string KitapAdi { get; set; } = "";
        public string YazarAdi { get; set; } = "";
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
        public string? ResimUrl { get; set; }
        public decimal ToplamFiyat => Fiyat * Adet;
    }
} 