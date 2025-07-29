using KitapSatisSitesi.Models;

namespace KitapSatisSitesi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(KitapDbContext context)
        {
            // Veritabanı zaten oluşturulmuş mu kontrol et
            context.Database.EnsureCreated();

            // Kategoriler zaten var mı kontrol et
            if (context.Kategoriler.Any())
            {
                return; // Veriler zaten mevcut
            }

            // Kategorileri ekle
            var kategoriler = new Kategori[]
            {
                new Kategori { Ad = "Roman" },
                new Kategori { Ad = "Bilim Kurgu" },
                new Kategori { Ad = "Tarih" },
                new Kategori { Ad = "Çocuk" },
                new Kategori { Ad = "Kişisel Gelişim" },
                new Kategori { Ad = "Manga" },
                new Kategori { Ad = "Şiir" },
                new Kategori { Ad = "Felsefe" },
                new Kategori { Ad = "Fantastik" }
            };

            context.Kategoriler.AddRange(kategoriler);
            context.SaveChanges();

            // Yazarları ekle
            var yazarlar = new Yazar[]
            {
                new Yazar { Ad = "J.K.", Soyad = "Rowling" },
                new Yazar { Ad = "Nazım", Soyad = "Hikmet" },
                new Yazar { Ad = "Tsugumi", Soyad = "Ohba" },
                new Yazar { Ad = "Robert", Soyad = "Kiyosaki" },
                new Yazar { Ad = "Zülfü", Soyad = "Livaneli" },
                new Yazar { Ad = "Sevim", Soyad = "Ak" },
                new Yazar { Ad = "Niccolò", Soyad = "Machiavelli" },
                new Yazar { Ad = "James", Soyad = "Clear" },
                new Yazar { Ad = "İmam", Soyad = "Gazali" }
            };

            context.Yazarlar.AddRange(yazarlar);
            context.SaveChanges();

            // Kitapları ekle
            var kitaplar = new Kitap[]
            {
                new Kitap { 
                    Ad = "Serenad", 
                    KategoriID = kategoriler[0].KategoriID, // Roman
                    YazarID = yazarlar[4].YazarID, // Zülfü Livaneli
                    YayinYili = 2011, 
                    Fiyat = 244.90m, 
                    StokAdedi = 10,
                    ResimUrl = "kitapresimleri/Serenad.jpg"
                },
                new Kitap { 
                    Ad = "Vanilya Kokulu Mektuplar", 
                    KategoriID = kategoriler[0].KategoriID, // Roman
                    YazarID = yazarlar[5].YazarID, // Sevim Ak
                    YayinYili = 2008, 
                    Fiyat = 84.50m, 
                    StokAdedi = 20,
                    ResimUrl = "kitapresimleri/Vanilya Kokulu Mektuplar.jpg"
                },
                new Kitap { 
                    Ad = "Zengin Baba Yoksul Baba", 
                    KategoriID = kategoriler[4].KategoriID, // Kişisel Gelişim
                    YazarID = yazarlar[3].YazarID, // Robert Kiyosaki
                    YayinYili = 1997, 
                    Fiyat = 299.60m, 
                    StokAdedi = 15,
                    ResimUrl = "kitapresimleri/Zengin Baba Yoksul Baba.jpg"
                },
                new Kitap { 
                    Ad = "Büyük Saat - Bütün Şiirleri", 
                    KategoriID = kategoriler[6].KategoriID, // Şiir
                    YazarID = yazarlar[1].YazarID, // Nazım Hikmet
                    YayinYili = 1984, 
                    Fiyat = 252.10m, 
                    StokAdedi = 30,
                    ResimUrl = "kitapresimleri/Büyük Saat - Bütün Şiirleri.jpg"
                },
                new Kitap { 
                    Ad = "Prens", 
                    KategoriID = kategoriler[7].KategoriID, // Felsefe
                    YazarID = yazarlar[6].YazarID, // Niccolò Machiavelli
                    YayinYili = 1532, 
                    Fiyat = 89.90m, 
                    StokAdedi = 12,
                    ResimUrl = "kitapresimleri/Prens.jpg"
                },
                new Kitap { 
                    Ad = "Atomik Alışkanlıklar", 
                    KategoriID = kategoriler[4].KategoriID, // Kişisel Gelişim
                    YazarID = yazarlar[7].YazarID, // James Clear
                    YayinYili = 2018, 
                    Fiyat = 420.00m, 
                    StokAdedi = 12,
                    ResimUrl = "kitapresimleri/Atomik Alışkanlıklar.jpg"
                },
                new Kitap { 
                    Ad = "Deathnote", 
                    KategoriID = kategoriler[5].KategoriID, // Manga
                    YazarID = yazarlar[2].YazarID, // Tsugumi Ohba
                    YayinYili = 2006, 
                    Fiyat = 131.40m, 
                    StokAdedi = 12,
                    ResimUrl = "kitapresimleri/Deathnote.jpg"
                },
                new Kitap { 
                    Ad = "Kalplerin Keşfi", 
                    KategoriID = kategoriler[7].KategoriID, // Felsefe
                    YazarID = yazarlar[8].YazarID, // İmam Gazali
                    YayinYili = 2018, 
                    Fiyat = 101.90m, 
                    StokAdedi = 12,
                    ResimUrl = "kitapresimleri/Kalplerin Keşfi.jpg"
                },
                new Kitap { 
                    Ad = "Harry Potter and the Philisophers Stone", 
                    KategoriID = kategoriler[8].KategoriID, // Fantastik
                    YazarID = yazarlar[0].YazarID, // J.K. Rowling
                    YayinYili = 1997, 
                    Fiyat = 146.70m, 
                    StokAdedi = 12,
                    ResimUrl = "kitapresimleri/Harry Potter and the Philisophers Stone.jpg"
                }
            };

            context.Kitaplar.AddRange(kitaplar);
            context.SaveChanges();

            // Kullanıcıları ekle
            var kullanicilar = new Kullanici[]
            {
                new Kullanici { 
                    Ad = "Admin", 
                    Soyad = "Admin", 
                    Email = "admin@example.com", 
                    Parola = "12345", 
                    Telefon = "055555555555" 
                },
                new Kullanici { 
                    Ad = "Mehmet", 
                    Soyad = "Demir", 
                    Email = "mehmet@example.com", 
                    Parola = "abcde", 
                    Telefon = "05555555554" 
                },
                new Kullanici { 
                    Ad = "Zeynep", 
                    Soyad = "Kaya", 
                    Email = "zeynep@example.com", 
                    Parola = "qwerty", 
                    Telefon = "05555555553" 
                }
            };

            context.Kullanicilar.AddRange(kullanicilar);
            context.SaveChanges();

            // Siparişleri ekle
            var siparisler = new Siparis[]
            {
                new Siparis { 
                    KullaniciID = kullanicilar[1].KullaniciID, // Mehmet
                    SiparisTarihi = new DateTime(2025, 7, 20),
                    ToplamTutar = 146.70m,
                    Durum = "Kargoya verildi."
                },
                new Siparis { 
                    KullaniciID = kullanicilar[2].KullaniciID, // Zeynep
                    SiparisTarihi = new DateTime(2025, 7, 21),
                    ToplamTutar = 420.00m,
                    Durum = "Hazirlaniyor."
                }
            };

            context.Siparisler.AddRange(siparisler);
            context.SaveChanges();

            // Sipariş detaylarını ekle
            var siparisDetaylar = new SiparisDetay[]
            {
                new SiparisDetay {
                    SiparisID = siparisler[0].SiparisID,
                    KitapID = kitaplar[8].KitapID, // Harry Potter
                    Adet = 1,
                    BirimFiyat = 146.70m,
                    ToplamFiyat = 146.70m
                },
                new SiparisDetay {
                    SiparisID = siparisler[1].SiparisID,
                    KitapID = kitaplar[5].KitapID, // Atomik Alışkanlıklar
                    Adet = 1,
                    BirimFiyat = 420.00m,
                    ToplamFiyat = 420.00m
                }
            };

            context.SiparisDetaylar.AddRange(siparisDetaylar);
            context.SaveChanges();

            // Ödemeleri ekle
            var odemeler = new Odeme[]
            {
                new Odeme { 
                    SiparisID = siparisler[0].SiparisID,
                    OdemeTipi = "Kredi Kartı",
                    Tutar = 149.90m,
                    OdemeTarihi = new DateTime(2025, 7, 20),
                    OdemeDurumu = "Başarılı"
                },
                new Odeme { 
                    SiparisID = siparisler[1].SiparisID,
                    OdemeTipi = "Havale",
                    Tutar = 89.50m,
                    OdemeTarihi = new DateTime(2025, 7, 21),
                    OdemeDurumu = "Başarılı"
                }
            };

            context.Odemeler.AddRange(odemeler);
            context.SaveChanges();

            // Yorumları ekle
            var yorumlar = new Yorum[]
            {
                new Yorum { 
                    KitapID = kitaplar[0].KitapID, // Serenad
                    KullaniciID = kullanicilar[0].KullaniciID, // Admin
                    Metin = "Çok sürükleyici bir romandı, severek okudum.",
                    Puan = 5,
                    Tarih = new DateTime(2025, 7, 21)
                },
                new Yorum { 
                    KitapID = kitaplar[2].KitapID, // Zengin Baba Yoksul Baba
                    KullaniciID = kullanicilar[1].KullaniciID, // Mehmet
                    Metin = "Tarihî bilgilerle dolu güzel bir kaynak.",
                    Puan = 4,
                    Tarih = new DateTime(2025, 7, 22)
                },
                new Yorum { 
                    KitapID = kitaplar[1].KitapID, // Vanilya Kokulu Mektuplar
                    KullaniciID = kullanicilar[0].KullaniciID, // Admin
                    Metin = "Görsellerle desteklenmiş, çocuklar için çok faydalı.",
                    Puan = 5,
                    Tarih = new DateTime(2025, 7, 23)
                }
            };

            context.Yorumlar.AddRange(yorumlar);
            context.SaveChanges();
        }
    }
} 