// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sepete ekleme fonksiyonu
function sepeteEkle(kitapId, kitapAdi) {
    // Kullanıcı girişi kontrolü
    if (!document.querySelector('[data-kullanici-id]')) {
        alert('Sepete eklemek için giriş yapmalısınız!');
        window.location.href = '/Auth/Login';
        return;
    }

    // Kitap bilgilerini al
    const kitapElement = document.querySelector(`[data-kitap-id="${kitapId}"]`);
    const fiyat = kitapElement ? parseFloat(kitapElement.dataset.fiyat) : 0;
    const yazarAdi = kitapElement ? kitapElement.dataset.yazarAdi : '';
    let resimUrl = kitapElement ? kitapElement.dataset.resimUrl : '';
    
    // Eğer resimUrl http:// veya https:// ile başlamıyorsa ve boş değilse, başına / ekle
    if (resimUrl && !resimUrl.startsWith('http://') && !resimUrl.startsWith('https://')) {
        resimUrl = '/' + resimUrl;
    }

    // Sepet verilerini localStorage'dan al
    let sepet = JSON.parse(localStorage.getItem('sepet') || '[]');
    
    // Kitap zaten sepette var mı kontrol et
    const mevcutUrun = sepet.find(item => item.kitapId === kitapId);
    
    if (mevcutUrun) {
        mevcutUrun.adet += 1;
    } else {
        sepet.push({
            kitapId: kitapId,
            kitapAdi: kitapAdi,
            yazarAdi: yazarAdi,
            fiyat: fiyat,
            resimUrl: resimUrl,
            adet: 1
        });
    }
    
    // Sepeti localStorage'a kaydet
    localStorage.setItem('sepet', JSON.stringify(sepet));
    
    // Başarı mesajı göster
    showNotification(`${kitapAdi} sepete eklendi!`, 'success');
    
    // Sepet sayısını güncelle
    updateSepetSayisi();
}

// Bildirim gösterme fonksiyonu
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // 3 saniye sonra otomatik kaldır
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 3000);
}

// Sepet sayısını güncelleme fonksiyonu
function updateSepetSayisi() {
    const sepet = JSON.parse(localStorage.getItem('sepet') || '[]');
    const toplamAdet = sepet.reduce((total, item) => total + item.adet, 0);
    
    // Sepet ikonuna sayı ekle
    const sepetIcon = document.querySelector('.fa-shopping-cart');
    if (sepetIcon) {
        let badge = sepetIcon.parentNode.querySelector('.badge');
        if (!badge) {
            badge = document.createElement('span');
            badge.className = 'badge bg-danger position-absolute top-0 start-100 translate-middle';
            badge.style.fontSize = '0.6em';
            sepetIcon.parentNode.style.position = 'relative';
            sepetIcon.parentNode.appendChild(badge);
        }
        
        if (toplamAdet > 0) {
            badge.textContent = toplamAdet;
            badge.style.display = 'inline';
        } else {
            badge.style.display = 'none';
        }
    }
}

// Sayfa yüklendiğinde sepet sayısını güncelle
document.addEventListener('DOMContentLoaded', function() {
    updateSepetSayisi();
});
