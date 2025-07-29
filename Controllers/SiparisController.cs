using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class SiparisController : Controller
    {
        private readonly KitapDbContext _context;

        public SiparisController(KitapDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Kullanıcı girişi kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var siparisler = await _context.Siparisler
                .Include(s => s.SiparisDetaylar)
                .ThenInclude(sd => sd.Kitap)
                .ThenInclude(k => k.Yazar)
                .Include(s => s.SiparisDetaylar)
                .ThenInclude(sd => sd.Kitap)
                .ThenInclude(k => k.Kategori)
                .Include(s => s.Kitap) // Eski siparişler için
                .Include(s => s.Kitap.Yazar) // Eski siparişler için yazar bilgisi
                .Include(s => s.Kitap.Kategori) // Eski siparişler için kategori bilgisi
                .Where(s => s.KullaniciID == int.Parse(kullaniciId))
                .OrderByDescending(s => s.SiparisTarihi)
                .ToListAsync();

            return View(siparisler);
        }

        public async Task<IActionResult> Detay(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var siparis = await _context.Siparisler
                .Include(s => s.SiparisDetaylar)
                .ThenInclude(sd => sd.Kitap)
                .ThenInclude(k => k.Yazar)
                .Include(s => s.SiparisDetaylar)
                .ThenInclude(sd => sd.Kitap)
                .ThenInclude(k => k.Kategori)
                .Include(s => s.Kitap) // Eski siparişler için
                .Include(s => s.Kitap.Yazar) // Eski siparişler için yazar bilgisi
                .Include(s => s.Kitap.Kategori) // Eski siparişler için kategori bilgisi
                .Include(s => s.Kullanici)
                .FirstOrDefaultAsync(s => s.SiparisID == id && s.KullaniciID == int.Parse(kullaniciId));

            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        [HttpPost]
        public async Task<IActionResult> SiparisVer([FromBody] List<SepetItem> sepetItems)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return Json(new { success = false, message = "Giriş yapmalısınız!" });
            }

            if (sepetItems == null || sepetItems.Count == 0)
            {
                return Json(new { success = false, message = "Sepetiniz boş!" });
            }

            try
            {
                // Toplam tutarı hesapla
                var toplamTutar = sepetItems.Sum(item => item.ToplamFiyat);

                // Ana sipariş oluştur
                var siparis = new Siparis
                {
                    KullaniciID = int.Parse(kullaniciId),
                    SiparisTarihi = DateTime.Now,
                    ToplamTutar = toplamTutar,
                    Durum = "Beklemede"
                };

                _context.Siparisler.Add(siparis);
                await _context.SaveChangesAsync(); // SiparisID'yi almak için

                // Sipariş detaylarını oluştur ve stokları güncelle
                foreach (var item in sepetItems)
                {
                    // Kitabı bul ve stok kontrolü yap
                    var kitap = await _context.Kitaplar.FindAsync(item.KitapId);
                    if (kitap == null)
                    {
                        return Json(new { success = false, message = $"Kitap bulunamadı: {item.KitapAdi}" });
                    }

                    if (kitap.StokAdedi < item.Adet)
                    {
                        return Json(new { success = false, message = $"Yetersiz stok: {item.KitapAdi} için sadece {kitap.StokAdedi} adet kaldı!" });
                    }

                    // Stoktan düş
                    kitap.StokAdedi -= item.Adet;

                    var siparisDetay = new SiparisDetay
                    {
                        SiparisID = siparis.SiparisID,
                        KitapID = item.KitapId,
                        Adet = item.Adet,
                        BirimFiyat = item.Fiyat,
                        ToplamFiyat = item.ToplamFiyat
                    };

                    _context.SiparisDetaylar.Add(siparisDetay);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Siparişiniz başarıyla oluşturuldu!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sipariş oluşturulurken hata oluştu: " + ex.Message });
            }
        }
    }
} 