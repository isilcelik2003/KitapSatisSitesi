using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class AdminController : Controller
    {
        private readonly KitapDbContext _context;

        public AdminController(KitapDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Admin kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Dashboard istatistikleri
            ViewBag.ToplamKitap = _context.Kitaplar.Count();
            ViewBag.ToplamSiparis = _context.Siparisler.Count();
            ViewBag.ToplamKullanici = _context.Kullanicilar.Count();
            ViewBag.ToplamYorum = _context.Yorumlar.Count();

            return View();
        }

        public IActionResult KitapEkle()
        {
            // Admin kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Sadece kategorileri al
            ViewBag.Kategoriler = _context.Kategoriler.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KitapEkle(Kitap kitap, string yazarAd, string yazarSoyad, IFormFile resimDosyasi)
        {
            // Debug: Gelen verileri logla
            Console.WriteLine($"Kitap Adı: {kitap.Ad}");
            Console.WriteLine($"Yazar Adı: {yazarAd}");
            Console.WriteLine($"Yazar Soyadı: {yazarSoyad}");
            Console.WriteLine($"Kategori ID: {kitap.KategoriID}");
            Console.WriteLine($"Fiyat: {kitap.Fiyat}");
            Console.WriteLine($"Stok: {kitap.StokAdedi}");
            Console.WriteLine($"Yayın Yılı: {kitap.YayinYili}");

            // Validation kontrolü
            if (string.IsNullOrEmpty(yazarAd) || string.IsNullOrEmpty(yazarSoyad))
            {
                ModelState.AddModelError("", "Yazar adı ve soyadı gereklidir!");
            }

            if (string.IsNullOrEmpty(kitap.Ad))
            {
                ModelState.AddModelError("Ad", "Kitap adı gereklidir!");
            }

            if (kitap.KategoriID == 0)
            {
                ModelState.AddModelError("KategoriID", "Kategori seçimi gereklidir!");
            }

            if (kitap.Fiyat <= 0)
            {
                ModelState.AddModelError("Fiyat", "Fiyat 0'dan büyük olmalıdır!");
            }

            if (kitap.StokAdedi < 0)
            {
                ModelState.AddModelError("StokAdedi", "Stok adedi negatif olamaz!");
            }

            // ModelState'den gereksiz hataları temizle
            ModelState.Remove("YazarID");
            ModelState.Remove("Yazar");
            ModelState.Remove("Kategori");
            
            // ModelState hatalarını kontrol et
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error}");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Önce yazarı kontrol et veya oluştur
                    var yazar = await _context.Yazarlar
                        .FirstOrDefaultAsync(y => y.Ad.ToLower() == yazarAd.ToLower() && y.Soyad.ToLower() == yazarSoyad.ToLower());

                    if (yazar == null)
                    {
                        yazar = new Yazar
                        {
                            Ad = yazarAd.Trim(),
                            Soyad = yazarSoyad.Trim()
                        };
                        _context.Yazarlar.Add(yazar);
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"Yeni yazar oluşturuldu: {yazar.YazarID}");
                    }

                    kitap.YazarID = yazar.YazarID;
                    kitap.Ad = kitap.Ad.Trim();
                    
                    // Dosya yükleme işlemi
                    if (resimDosyasi != null && resimDosyasi.Length > 0)
                    {
                        // Dosya uzantısını al
                        var dosyaUzantisi = Path.GetExtension(resimDosyasi.FileName).ToLowerInvariant();
                        
                        // Güvenli dosya adı oluştur
                        var guvenliDosyaAdi = $"{kitap.Ad.Replace(" ", "_").Replace("/", "_").Replace("\\", "_")}{dosyaUzantisi}";
                        
                        // Dosya yolunu belirle
                        var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "kitapresimleri", guvenliDosyaAdi);
                        
                        // Dosyayı kaydet
                        using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                        {
                            await resimDosyasi.CopyToAsync(stream);
                        }
                        
                        // Resim URL'ini ayarla
                        kitap.ResimUrl = $"/kitapresimleri/{guvenliDosyaAdi}";
                        
                        Console.WriteLine($"Resim dosyası yüklendi: {dosyaYolu}");
                    }
                    else if (string.IsNullOrEmpty(kitap.ResimUrl))
                    {
                        // Resim yüklenmemişse placeholder kullan
                        kitap.ResimUrl = "https://via.placeholder.com/200x300/6c757d/ffffff?text=" + Uri.EscapeDataString(kitap.Ad);
                    }
                    
                    _context.Kitaplar.Add(kitap);
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"Kitap başarıyla eklendi: {kitap.KitapID}");
                    TempData["Mesaj"] = $"Kitap başarıyla eklendi! Yazar: {yazar.TamAd}";
                    return RedirectToAction("KitapListesi");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    ModelState.AddModelError("", $"Kitap eklenirken hata oluştu: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        ModelState.AddModelError("", $"Detay: {ex.InnerException.Message}");
                    }
                }
            }

            // Hata durumunda kategorileri tekrar yükle
            ViewBag.Kategoriler = _context.Kategoriler.ToList();
            
            return View(kitap);
        }

        public async Task<IActionResult> KitapListesi()
        {
            // Admin kontrolü
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .ToListAsync();

            return View(kitaplar);
        }

        // Kitap Düzenleme
        public async Task<IActionResult> KitapDuzenle(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitap = await _context.Kitaplar
                .Include(k => k.Yazar)
                .FirstOrDefaultAsync(k => k.KitapID == id);

            if (kitap == null)
            {
                return NotFound();
            }

            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.YazarAd = kitap.Yazar?.Ad;
            ViewBag.YazarSoyad = kitap.Yazar?.Soyad;

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> KitapDuzenle(int id, Kitap kitap, string yazarAd, string yazarSoyad, IFormFile resimDosyasi)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != kitap.KitapID)
            {
                return NotFound();
            }

            // ModelState'den gereksiz hataları temizle
            ModelState.Remove("YazarID");
            ModelState.Remove("Yazar");
            ModelState.Remove("Kategori");

            // Validation kontrolleri
            if (string.IsNullOrWhiteSpace(kitap.Ad))
            {
                ModelState.AddModelError("Ad", "Kitap adı gereklidir!");
            }

            if (string.IsNullOrWhiteSpace(yazarAd) || string.IsNullOrWhiteSpace(yazarSoyad))
            {
                ModelState.AddModelError("", "Yazar adı ve soyadı gereklidir!");
            }

            if (kitap.KategoriID <= 0)
            {
                ModelState.AddModelError("KategoriID", "Kategori seçimi gereklidir!");
            }

            if (kitap.Fiyat <= 0)
            {
                ModelState.AddModelError("Fiyat", "Fiyat 0'dan büyük olmalıdır!");
            }

            if (kitap.StokAdedi < 0)
            {
                ModelState.AddModelError("StokAdedi", "Stok adedi negatif olamaz!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutKitap = await _context.Kitaplar
                        .Include(k => k.Yazar)
                        .FirstOrDefaultAsync(k => k.KitapID == id);

                    if (mevcutKitap == null)
                    {
                        return NotFound();
                    }

                    // Yazar kontrolü
                    var yazar = await _context.Yazarlar
                        .FirstOrDefaultAsync(y => y.Ad.ToLower() == yazarAd.ToLower() && y.Soyad.ToLower() == yazarSoyad.ToLower());

                    if (yazar == null)
                    {
                        yazar = new Yazar
                        {
                            Ad = yazarAd.Trim(),
                            Soyad = yazarSoyad.Trim()
                        };
                        _context.Yazarlar.Add(yazar);
                        await _context.SaveChangesAsync();
                    }

                    // Dosya yükleme işlemi
                    if (resimDosyasi != null && resimDosyasi.Length > 0)
                    {
                        // Dosya uzantısını al
                        var dosyaUzantisi = Path.GetExtension(resimDosyasi.FileName).ToLowerInvariant();
                        
                        // Güvenli dosya adı oluştur
                        var guvenliDosyaAdi = $"{kitap.Ad.Replace(" ", "_").Replace("/", "_").Replace("\\", "_")}{dosyaUzantisi}";
                        
                        // Dosya yolunu belirle
                        var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "kitapresimleri", guvenliDosyaAdi);
                        
                        // Dosyayı kaydet
                        using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                        {
                            await resimDosyasi.CopyToAsync(stream);
                        }
                        
                        // Resim URL'ini ayarla
                        mevcutKitap.ResimUrl = $"/kitapresimleri/{guvenliDosyaAdi}";
                        
                        Console.WriteLine($"Resim dosyası güncellendi: {dosyaYolu}");
                    }
                    else if (!string.IsNullOrEmpty(kitap.ResimUrl))
                    {
                        // Resim URL'i manuel olarak girilmişse kullan
                        mevcutKitap.ResimUrl = kitap.ResimUrl;
                    }
                    
                    // Kitap bilgilerini güncelle
                    mevcutKitap.Ad = kitap.Ad.Trim();
                    mevcutKitap.YazarID = yazar.YazarID;
                    mevcutKitap.KategoriID = kitap.KategoriID;
                    mevcutKitap.Fiyat = kitap.Fiyat;
                    mevcutKitap.StokAdedi = kitap.StokAdedi;
                    mevcutKitap.YayinYili = kitap.YayinYili;

                    await _context.SaveChangesAsync();
                    TempData["Mesaj"] = "Kitap başarıyla güncellendi!";
                    return RedirectToAction("KitapListesi");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Kitap güncellenirken hata oluştu: {ex.Message}");
                }
            }

            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.YazarAd = yazarAd;
            ViewBag.YazarSoyad = yazarSoyad;

            return View(kitap);
        }

        // Kitap Silme
        [HttpPost]
        public async Task<IActionResult> KitapSil(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }

            try
            {
                _context.Kitaplar.Remove(kitap);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Kitap başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = $"Kitap silinirken hata oluştu: {ex.Message}";
            }

            return RedirectToAction("KitapListesi");
        }

        // Stok Düzenleme
        public async Task<IActionResult> StokDuzenle(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitap = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .FirstOrDefaultAsync(k => k.KitapID == id);

            if (kitap == null)
            {
                return NotFound();
            }

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> StokDuzenle(int id, int yeniStok)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }

            if (yeniStok < 0)
            {
                TempData["Hata"] = "Stok adedi negatif olamaz!";
                return RedirectToAction("StokDuzenle", new { id });
            }

            kitap.StokAdedi = yeniStok;
            await _context.SaveChangesAsync();
            TempData["Mesaj"] = $"Stok başarıyla güncellendi! Yeni stok: {yeniStok}";

            return RedirectToAction("KitapListesi");
        }

        // Yorum Yönetimi
        public async Task<IActionResult> YorumListesi()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var yorumlar = await _context.Yorumlar
                .Include(y => y.Kitap)
                .Include(y => y.Kullanici)
                .OrderByDescending(y => y.Tarih)
                .ToListAsync();

            return View(yorumlar);
        }

        [HttpPost]
        public async Task<IActionResult> YorumSil(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var yorum = await _context.Yorumlar.FindAsync(id);
            if (yorum == null)
            {
                return NotFound();
            }

            try
            {
                _context.Yorumlar.Remove(yorum);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Yorum başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = $"Yorum silinirken hata oluştu: {ex.Message}";
            }

            return RedirectToAction("YorumListesi");
        }

        // Sipariş Yönetimi
        public async Task<IActionResult> SiparisListesi()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var siparisler = await _context.Siparisler
                .Include(s => s.SiparisDetaylar)
                .ThenInclude(sd => sd.Kitap)
                .Include(s => s.Kullanici)
                .Include(s => s.Kitap) // Eski siparişler için Kitap bilgisi
                .OrderByDescending(s => s.SiparisTarihi)
                .ToListAsync();

            return View(siparisler);
        }

        public async Task<IActionResult> SiparisDetay(int id)
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
                .Include(s => s.Kullanici)
                .Include(s => s.Kitap) // Eski siparişler için
                    .ThenInclude(k => k.Yazar)
                .Include(s => s.Kitap) // Eski siparişler için
                    .ThenInclude(k => k.Kategori)
                .FirstOrDefaultAsync(s => s.SiparisID == id);

            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        [HttpPost]
        public async Task<IActionResult> SiparisDurumuGuncelle(int id, string yeniDurum)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }

            siparis.Durum = yeniDurum;
            await _context.SaveChangesAsync();
            TempData["Mesaj"] = $"Sipariş durumu güncellendi: {yeniDurum}";

            return RedirectToAction("SiparisListesi");
        }

        // Kullanıcı Yönetimi
        public async Task<IActionResult> KullaniciListesi()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kullanicilar = await _context.Kullanicilar
                .OrderBy(k => k.Ad)
                .ToListAsync();

            return View(kullanicilar);
        }

        // Kullanıcı Detay Sayfası
        public async Task<IActionResult> KullaniciDetay(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kullanici = await _context.Kullanicilar
                .Include(k => k.Siparisler)
                    .ThenInclude(s => s.SiparisDetaylar)
                    .ThenInclude(sd => sd.Kitap)
                .Include(k => k.Siparisler)
                    .ThenInclude(s => s.Kitap) // Eski siparişler için
                .Include(k => k.Yorumlar)
                    .ThenInclude(y => y.Kitap)
                .FirstOrDefaultAsync(k => k.KullaniciID == id);

            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // Kategori Yönetimi
        public async Task<IActionResult> KategoriListesi()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kategoriler = await _context.Kategoriler
                .Include(k => k.Kitaplar)
                .OrderBy(k => k.Ad)
                .ToListAsync();

            return View(kategoriler);
        }

        public IActionResult KategoriEkle()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KategoriEkle(Kategori kategori)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aynı isimde kategori var mı kontrol et
                    var mevcutKategori = await _context.Kategoriler
                        .FirstOrDefaultAsync(k => k.Ad.ToLower() == kategori.Ad.ToLower());

                    if (mevcutKategori != null)
                    {
                        ModelState.AddModelError("Ad", "Bu isimde bir kategori zaten mevcut!");
                        return View(kategori);
                    }

                    kategori.Ad = kategori.Ad.Trim();
                    _context.Kategoriler.Add(kategori);
                    await _context.SaveChangesAsync();
                    
                    TempData["Mesaj"] = $"Kategori başarıyla eklendi: {kategori.Ad}";
                    return RedirectToAction("KategoriListesi");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Kategori eklenirken hata oluştu: {ex.Message}");
                }
            }

            return View(kategori);
        }

        public async Task<IActionResult> KategoriDuzenle(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kategori = await _context.Kategoriler.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriDuzenle(int id, Kategori kategori)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != kategori.KategoriID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Aynı isimde başka kategori var mı kontrol et
                    var mevcutKategori = await _context.Kategoriler
                        .FirstOrDefaultAsync(k => k.Ad.ToLower() == kategori.Ad.ToLower() && k.KategoriID != id);

                    if (mevcutKategori != null)
                    {
                        ModelState.AddModelError("Ad", "Bu isimde bir kategori zaten mevcut!");
                        return View(kategori);
                    }

                    var kategoriToUpdate = await _context.Kategoriler.FindAsync(id);
                    if (kategoriToUpdate == null)
                    {
                        return NotFound();
                    }

                    kategoriToUpdate.Ad = kategori.Ad.Trim();
                    await _context.SaveChangesAsync();
                    
                    TempData["Mesaj"] = "Kategori başarıyla güncellendi!";
                    return RedirectToAction("KategoriListesi");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Kategori güncellenirken hata oluştu: {ex.Message}");
                }
            }

            return View(kategori);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriSil(int id)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kategori = await _context.Kategoriler
                .Include(k => k.Kitaplar)
                .FirstOrDefaultAsync(k => k.KategoriID == id);

            if (kategori == null)
            {
                return NotFound();
            }

            // Kategoriye ait kitap var mı kontrol et
            if (kategori.Kitaplar.Any())
            {
                TempData["Hata"] = $"Bu kategoriye ait {kategori.Kitaplar.Count} kitap bulunduğu için silinemez!";
                return RedirectToAction("KategoriListesi");
            }

            try
            {
                _context.Kategoriler.Remove(kategori);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = "Kategori başarıyla silindi!";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = $"Kategori silinirken hata oluştu: {ex.Message}";
            }

            return RedirectToAction("KategoriListesi");
        }

        // Toplu Fiyat Güncelleme
        public async Task<IActionResult> TopluFiyatGuncelle()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var kitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .OrderBy(k => k.Ad)
                .ToListAsync();

            return View(kitaplar);
        }

        [HttpPost]
        public async Task<IActionResult> TopluFiyatGuncelle(Dictionary<int, decimal> fiyatlar)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                int guncellenenSayisi = 0;
                foreach (var fiyat in fiyatlar)
                {
                    if (fiyat.Value > 0)
                    {
                        var kitap = await _context.Kitaplar.FindAsync(fiyat.Key);
                        if (kitap != null)
                        {
                            kitap.Fiyat = fiyat.Value;
                            guncellenenSayisi++;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = $"{guncellenenSayisi} kitabın fiyatı başarıyla güncellendi!";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = $"Fiyat güncellenirken hata oluştu: {ex.Message}";
            }

            return RedirectToAction("TopluFiyatGuncelle");
        }

        // Kategori Bazlı Fiyat Güncelleme
        [HttpPost]
        public async Task<IActionResult> KategoriFiyatGuncelle(int kategoriId, decimal yeniFiyat, decimal yuzdeArtis)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var kitaplar = await _context.Kitaplar
                    .Where(k => k.KategoriID == kategoriId)
                    .ToListAsync();

                int guncellenenSayisi = 0;
                foreach (var kitap in kitaplar)
                {
                    if (yeniFiyat > 0)
                    {
                        kitap.Fiyat = yeniFiyat;
                    }
                    else if (yuzdeArtis != 0)
                    {
                        kitap.Fiyat = kitap.Fiyat * (1 + yuzdeArtis / 100);
                    }
                    guncellenenSayisi++;
                }

                await _context.SaveChangesAsync();
                TempData["Mesaj"] = $"{guncellenenSayisi} kitabın fiyatı başarıyla güncellendi!";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = $"Fiyat güncellenirken hata oluştu: {ex.Message}";
            }

            return RedirectToAction("KategoriListesi");
        }
    }
} 