using Microsoft.EntityFrameworkCore;
using KitapSatisSitesi.Models;

namespace KitapSatisSitesi.Data
{
    public class KitapDbContext : DbContext
    {
        public KitapDbContext(DbContextOptions<KitapDbContext> options) : base(options)
        {
        }

        public DbSet<Kitap> Kitaplar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Yazar> Yazarlar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
        public DbSet<Odeme> Odemeler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kategori - Kitap ilişkisi
            modelBuilder.Entity<Kitap>()
                .HasOne(k => k.Kategori)
                .WithMany(k => k.Kitaplar)
                .HasForeignKey(k => k.KategoriID);

            // Yazar - Kitap ilişkisi
            modelBuilder.Entity<Kitap>()
                .HasOne(k => k.Yazar)
                .WithMany(y => y.Kitaplar)
                .HasForeignKey(k => k.YazarID);

            // Kullanıcı - Sipariş ilişkisi
            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.Kullanici)
                .WithMany(k => k.Siparisler)
                .HasForeignKey(s => s.KullaniciID);

            // Sipariş - Kitap ilişkisi (geriye dönük uyumluluk için)
            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.Kitap)
                .WithMany()
                .HasForeignKey(s => s.KitapID)
                .IsRequired(false);

            // Sipariş - SiparişDetay ilişkisi
            modelBuilder.Entity<SiparisDetay>()
                .HasOne(sd => sd.Siparis)
                .WithMany(s => s.SiparisDetaylar)
                .HasForeignKey(sd => sd.SiparisID);

            // Kitap - SiparişDetay ilişkisi
            modelBuilder.Entity<SiparisDetay>()
                .HasOne(sd => sd.Kitap)
                .WithMany()
                .HasForeignKey(sd => sd.KitapID);

            // Sipariş - Ödeme ilişkisi
            modelBuilder.Entity<Odeme>()
                .HasOne(o => o.Siparis)
                .WithMany()
                .HasForeignKey(o => o.SiparisID);

            // Kitap - Yorum ilişkisi
            modelBuilder.Entity<Yorum>()
                .HasOne(y => y.Kitap)
                .WithMany(k => k.Yorumlar)
                .HasForeignKey(y => y.KitapID);

            // Kullanıcı - Yorum ilişkisi
            modelBuilder.Entity<Yorum>()
                .HasOne(y => y.Kullanici)
                .WithMany(k => k.Yorumlar)
                .HasForeignKey(y => y.KullaniciID);
        }
    }
} 