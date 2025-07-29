using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class KitapController : Controller
    {
        private readonly KitapDbContext _context;

        public KitapController(KitapDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .ToListAsync();

            // Kategorileri de al
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = kategoriler;

            return View(kitaplar);
        }

        public async Task<IActionResult> Detay(int id)
        {
            var kitap = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .Include(k => k.Yorumlar)
                    .ThenInclude(y => y.Kullanici)
                .FirstOrDefaultAsync(k => k.KitapID == id);

            if (kitap == null)
            {
                return NotFound();
            }

            return View(kitap);
        }

        public async Task<IActionResult> Kategori(string kategori)
        {
            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .Where(k => k.Kategori.Ad == kategori)
                .ToListAsync();

            // TÜM KATEGORİLERİ DE AL
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = kategoriler;

            ViewBag.Kategori = kategori;
            return View(kitaplar);
        }

        public async Task<IActionResult> Arama(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index");
            }

            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .Where(k => k.Ad.Contains(q) || 
                           k.Yazar.Ad.Contains(q) || 
                           k.Yazar.Soyad.Contains(q) || 
                           k.Kategori.Ad.Contains(q))
                .ToListAsync();

            // TÜM KATEGORİLERİ DE AL
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = kategoriler;

            ViewBag.AramaTerimi = q;
            return View(kitaplar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YorumEkle(int kitapId, string metin, int puan)
        {
            // Kullanıcı girişi kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return Json(new { success = false, message = "Yorum yapmak için giriş yapmalısınız!" });
            }

            if (string.IsNullOrEmpty(metin) || puan < 1 || puan > 5)
            {
                return Json(new { success = false, message = "Geçersiz yorum bilgileri!" });
            }

            try
            {
                var yorum = new Yorum
                {
                    KitapID = kitapId,
                    KullaniciID = int.Parse(kullaniciId),
                    Metin = metin.Trim(),
                    Puan = puan,
                    Tarih = DateTime.Now
                };

                _context.Yorumlar.Add(yorum);
                await _context.SaveChangesAsync();

                // Kullanıcı bilgisini al
                var kullanici = await _context.Kullanicilar.FindAsync(int.Parse(kullaniciId));

                return Json(new { 
                    success = true, 
                    message = "Yorum başarıyla eklendi!",
                    yorum = new {
                        kullaniciAdi = kullanici?.TamAd,
                        metin = yorum.Metin,
                        puan = yorum.Puan,
                        tarih = yorum.Tarih.ToString("dd.MM.yyyy HH:mm")
                    }
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Yorum eklenirken hata oluştu!" });
            }
        }
    }
} 