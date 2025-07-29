using Microsoft.AspNetCore.Mvc;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;
using Microsoft.EntityFrameworkCore;

namespace KitapSatisSitesi.Controllers
{
    public class SepetController : Controller
    {
        private readonly KitapDbContext _context;

        public SepetController(KitapDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Kullanıcı girişi kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SepeteEkle(int kitapId, int adet = 1)
        {
            // Kullanıcı girişi kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return Json(new { success = false, message = "Giriş yapmalısınız!" });
            }

            var kitap = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .FirstOrDefaultAsync(k => k.KitapID == kitapId);

            if (kitap == null)
            {
                return Json(new { success = false, message = "Kitap bulunamadı!" });
            }

            if (!kitap.StoktaMi)
            {
                return Json(new { success = false, message = "Kitap stokta yok!" });
            }

            // Sepet verilerini session'da sakla
            var sepet = HttpContext.Session.GetString("Sepet") ?? "[]";
            var sepetListesi = System.Text.Json.JsonSerializer.Deserialize<List<SepetItem>>(sepet) ?? new List<SepetItem>();

            var mevcutUrun = sepetListesi.FirstOrDefault(s => s.KitapId == kitapId);
            if (mevcutUrun != null)
            {
                mevcutUrun.Adet += adet;
            }
            else
            {
                sepetListesi.Add(new SepetItem
                {
                    KitapId = kitap.KitapID,
                    KitapAdi = kitap.Ad,
                    YazarAdi = kitap.Yazar.TamAd,
                    Fiyat = kitap.Fiyat,
                    Adet = adet,
                    ResimUrl = kitap.ResimUrl
                });
            }

            HttpContext.Session.SetString("Sepet", System.Text.Json.JsonSerializer.Serialize(sepetListesi));

            return Json(new { success = true, message = $"{kitap.Ad} sepete eklendi!", sepetAdet = sepetListesi.Count });
        }

        [HttpPost]
        public IActionResult SepettenCikar(int kitapId)
        {
            var sepet = HttpContext.Session.GetString("Sepet") ?? "[]";
            var sepetListesi = System.Text.Json.JsonSerializer.Deserialize<List<SepetItem>>(sepet) ?? new List<SepetItem>();

            var urun = sepetListesi.FirstOrDefault(s => s.KitapId == kitapId);
            if (urun != null)
            {
                sepetListesi.Remove(urun);
                HttpContext.Session.SetString("Sepet", System.Text.Json.JsonSerializer.Serialize(sepetListesi));
            }

            return Json(new { success = true, sepetAdet = sepetListesi.Count });
        }

        [HttpPost]
        public IActionResult AdetGuncelle(int kitapId, int adet)
        {
            var sepet = HttpContext.Session.GetString("Sepet") ?? "[]";
            var sepetListesi = System.Text.Json.JsonSerializer.Deserialize<List<SepetItem>>(sepet) ?? new List<SepetItem>();

            var urun = sepetListesi.FirstOrDefault(s => s.KitapId == kitapId);
            if (urun != null)
            {
                if (adet <= 0)
                {
                    sepetListesi.Remove(urun);
                }
                else
                {
                    urun.Adet = adet;
                }
                HttpContext.Session.SetString("Sepet", System.Text.Json.JsonSerializer.Serialize(sepetListesi));
            }

            var toplamTutar = sepetListesi.Sum(s => s.ToplamFiyat);
            return Json(new { success = true, toplamTutar = toplamTutar.ToString("F2"), sepetAdet = sepetListesi.Count });
        }
    }


} 