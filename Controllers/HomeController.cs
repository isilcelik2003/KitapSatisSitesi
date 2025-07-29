using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;
using KitapSatisSitesi.Data;

namespace KitapSatisSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KitapDbContext _context;

        public HomeController(ILogger<HomeController> logger, KitapDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Öne çıkan kitapları al (son eklenen 4 kitap)
            var oneCikanKitaplar = await _context.Kitaplar
                .Include(k => k.Kategori)
                .Include(k => k.Yazar)
                .OrderByDescending(k => k.KitapID)
                .Take(4)
                .ToListAsync();

            // Kategorileri al
            var kategoriler = await _context.Kategoriler
                .Include(k => k.Kitaplar)
                .ToListAsync();

            ViewBag.OneCikanKitaplar = oneCikanKitaplar;
            ViewBag.Kategoriler = kategoriler;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
