using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class AuthController : Controller
    {
        private readonly KitapDbContext _context;

        public AuthController(KitapDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string parola)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email && k.Parola == parola);

            if (kullanici != null)
            {
                // Basit session yönetimi
                HttpContext.Session.SetString("KullaniciID", kullanici.KullaniciID.ToString());
                HttpContext.Session.SetString("KullaniciAdi", kullanici.TamAd);
                HttpContext.Session.SetString("KullaniciEmail", kullanici.Email);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = "Email veya parola hatalı!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                // Email kontrolü
                var mevcutKullanici = await _context.Kullanicilar
                    .FirstOrDefaultAsync(k => k.Email == kullanici.Email);

                if (mevcutKullanici != null)
                {
                    ViewBag.Hata = "Bu email adresi zaten kullanılıyor!";
                    return View(kullanici);
                }

                _context.Kullanicilar.Add(kullanici);
                await _context.SaveChangesAsync();

                ViewBag.Basari = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            return View(kullanici);
        }
    }
} 