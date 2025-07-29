using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class ProfilController : Controller
    {
        private readonly KitapDbContext _context;

        public ProfilController(KitapDbContext context)
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

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciID == int.Parse(kullaniciId));

            if (kullanici == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(kullanici);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Kullanici kullanici)
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciID");
            if (string.IsNullOrEmpty(kullaniciId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var mevcutKullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciID == int.Parse(kullaniciId));

            if (mevcutKullanici == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ModelState.IsValid)
            {
                mevcutKullanici.Ad = kullanici.Ad;
                mevcutKullanici.Soyad = kullanici.Soyad;
                mevcutKullanici.Email = kullanici.Email;
                mevcutKullanici.Telefon = kullanici.Telefon;

                if (!string.IsNullOrEmpty(kullanici.Parola))
                {
                    mevcutKullanici.Parola = kullanici.Parola;
                }

                await _context.SaveChangesAsync();

                // Session'ı güncelle
                HttpContext.Session.SetString("KullaniciAdi", mevcutKullanici.TamAd);
                HttpContext.Session.SetString("KullaniciEmail", mevcutKullanici.Email);

                TempData["Mesaj"] = "Profil başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            return View("Index", mevcutKullanici);
        }
    }
} 