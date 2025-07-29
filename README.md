# 📚 Kitap Satış Sitesi

Modern ve kullanıcı dostu bir kitap satış platformu. ASP.NET Core MVC kullanılarak geliştirilmiş tam özellikli e-ticaret uygulaması.

## ✨ Özellikler

### 🛒 Kullanıcı Özellikleri
- **Kitap Arama ve Filtreleme**: Kategoriye göre kitap filtreleme
- **Sepet Yönetimi**: Çoklu kitap ekleme ve sepet yönetimi
- **Sipariş Sistemi**: Tek sipariş altında çoklu kitap siparişi
- **Kullanıcı Profili**: Sipariş geçmişi ve profil yönetimi
- **Yorum Sistemi**: Kitap değerlendirme ve yorum yapma
- **Stok Kontrolü**: Gerçek zamanlı stok kontrolü

### 🔧 Admin Paneli
- **Kitap Yönetimi**: Kitap ekleme, düzenleme, silme
- **Stok Yönetimi**: Stok güncelleme ve takibi
- **Sipariş Yönetimi**: Sipariş durumu güncelleme ve detay görüntüleme
- **Kullanıcı Yönetimi**: Kullanıcı detayları ve sipariş geçmişi
- **Kategori Yönetimi**: Kategori ekleme, düzenleme, silme
- **Yorum Yönetimi**: Yorum onaylama ve silme
- **Toplu Fiyat Güncelleme**: Kategori bazında fiyat güncelleme

## 🛠️ Teknolojiler

- **Backend**: ASP.NET Core 9.0 MVC
- **Veritabanı**: SQLite
- **ORM**: Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **İkonlar**: Font Awesome
- **Tablolar**: DataTables.js

## 📋 Gereksinimler

- .NET 9.0 SDK
- Visual Studio 2022 veya Visual Studio Code
- Git

## 🚀 Kurulum

1. **Repository'yi klonlayın:**
   ```bash
   git clone https://github.com/isilcelik2003/KitapSatisSitesi.git
   cd KitapSatisSitesi
   ```

2. **Bağımlılıkları yükleyin:**
   ```bash
   dotnet restore
   ```

3. **Veritabanını oluşturun:**
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```

5. **Tarayıcıda açın:**
   ```
   http://localhost:5100
   ```

## 👤 Varsayılan Kullanıcılar

### Admin Kullanıcısı
- **Email**: admin@example.com
- **Şifre**: 12345

### Test Kullanıcısı
- **Email**: zeynep@example.com
- **Şifre**: qwerty

### Test Kullanıcısı 2 
- **Email**: mehmet@example.com
- **Şifre**: abcde

## 📊 Veritabanı Yapısı

### Ana Tablolar
- **Kitaplar**: Kitap bilgileri, fiyat, stok
- **Kategoriler**: Kitap kategorileri
- **Yazarlar**: Yazar bilgileri
- **Kullanıcılar**: Kullanıcı hesapları
- **Siparişler**: Ana sipariş bilgileri
- **SiparisDetaylar**: Sipariş içindeki ürünler
- **Yorumlar**: Kitap değerlendirmeleri
- **Ödemeler**: Ödeme bilgileri

## 🎯 Öne Çıkan Özellikler

### 🔄 Çoklu Sipariş Sistemi
- Tek sipariş altında birden fazla kitap
- Detaylı sipariş özeti
- Stok otomatik düşürme

### 📱 Responsive Tasarım
- Mobil uyumlu arayüz
- Bootstrap 5 ile modern tasarım
- Kullanıcı dostu navigasyon

### 🔐 Güvenlik
- Session tabanlı kimlik doğrulama
- Admin paneli koruması
- Güvenli form işleme

## 📸 Ekran Görüntüleri

### Ana Sayfa
- Öne çıkan kitaplar
- Kategori filtreleme
- Arama fonksiyonu

### Admin Paneli
- Dashboard istatistikleri
- Kitap yönetimi
- Sipariş takibi

### Kullanıcı Paneli
- Profil yönetimi
- Sipariş geçmişi
- Sepet yönetimi

## 🔧 Geliştirme

### Proje Yapısı
```
KitapSatisSitesi/
├── Controllers/          # MVC Controllers
├── Models/              # Entity Models
├── Views/               # Razor Views
├── Data/                # Database Context
├── Migrations/          # EF Core Migrations
├── wwwroot/             # Static Files
│   ├── css/
│   ├── js/
│   └── kitapresimleri/
└── Properties/          # Project Properties
```

### Önemli Dosyalar
- `Program.cs`: Uygulama başlangıç noktası
- `KitapDbContext.cs`: Veritabanı context
- `DbInitializer.cs`: Başlangıç verileri
- `_Layout.cshtml`: Ana layout şablonu

## 🚀 Deployment

### Production Ortamı
1. `appsettings.Production.json` dosyasını yapılandırın
2. Veritabanı bağlantı stringini güncelleyin
3. `dotnet publish` ile yayınlayın

### Docker (Opsiyonel)
```bash
docker build -t kitapsatis .
docker run -p 8080:80 kitapsatis
```

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 📞 İletişim

- **Proje Sahibi**: İsil Çelik
- **Email**: isilcelik2003@gmail.com
- **GitHub**: [github.com/isilcelik2003]

## 🙏 Teşekkürler

- ASP.NET Core ekibine
- Bootstrap ekibine
- Font Awesome ekibine
- Tüm katkıda bulunanlara

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!
