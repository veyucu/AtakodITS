# 🔧 Ayarlar Sistemi - Kullanım Kılavuzu

## 📍 Nereden Erişilir?

**Ana Sayfadan:**
```
Dashboard → Ayarlar Kartı (🔧 turuncu kart)
```

**Doğrudan URL:**
```
http://localhost:3000/settings
```

## 📋 Ayarlar Kategorileri

### 1. 🔐 ITS Ayarları

**Temel Bilgiler:**
- **ITS GLN No**: Kurumunuzun GLN numarası
- **ITS Kullanıcı Adı**: ITS sistemi kullanıcı adı
- **ITS Şifre**: ITS sistemi şifresi (Göster/Gizle butonu ile görülebilir)
- **ITS Web Servis Adresi**: Ana servis URL'i (varsayılan: `https://its2.saglik.gov.tr`)

**Endpoint URL'leri:**
Tüm ITS işlemleri için endpoint path'leri:
- Token alma
- Satış bildirimleri
- Durum kontrol
- Deaktivasyon
- Mal alım/iade
- Eczane satış
- Takas işlemleri
- PTS işlemleri
- vb.

### 2. 🖥️ ERP Ayarları

- **ERP Web Servis Adresi**: Backend API adresi (varsayılan: `http://localhost:5000`)

### 3. 🔗 Alan Eşleştirmeleri

**Ürün Bilgileri:**
- **Ürün Barkod Bilgisi**: TBLSTSABIT'teki barkod kolonu (varsayılan: `STOK_KODU`)
- **Ürün ITS Bilgisi**: ITS ürünlerini belirleyen koşul (varsayılan: `TBLSTSABIT.KOD_5='BESERI'`)
- **Ürün UTS Bilgisi**: UTS ürünlerini belirleyen koşul (varsayılan: `TBLSTSABIT.KOD_5='UTS'`)

**Cari Bilgileri:**
- **Cari GLN Bilgisi**: Cari GLN alanı (varsayılan: `TBLCASABIT.EMAIL`)
- **Cari UTS Bilgisi**: Cari UTS alanı (varsayılan: `TBLCASABITEK.KULL3S`)

## 💾 Kaydetme

### localStorage'da Saklanır
Tüm ayarlar browser'ın localStorage'ında JSON formatında saklanır:

```javascript
{
  "itsGlnNo": "8680001084524",
  "itsUsername": "86800010845240000",
  "itsPassword": "1981aa",
  "itsWebServiceUrl": "https://its2.saglik.gov.tr",
  // ... diğer ayarlar
}
```

### Nasıl Kaydedilir?
1. Ayarlar sayfasında değişiklik yapın
2. Sağ üstteki **"💾 Kaydet"** butonuna tıklayın
3. Başarı mesajı göreceksiniz

### Varsayılana Sıfırlama
**"🔄 Sıfırla"** butonu ile tüm ayarları varsayılan değerlere döndürebilirsiniz.

## 🔨 Kod İçinde Kullanım

### Ayarları Okuma

```javascript
import { getSettings, getSetting, getITSUrl, getITSCredentials } from '@/utils/settingsHelper'

// Tüm ayarları al
const allSettings = getSettings()

// Tek bir ayarı al
const glnNo = getSetting('itsGlnNo')

// ITS URL oluştur
const tokenUrl = getITSUrl('itsTokenUrl')
// Sonuç: "https://its2.saglik.gov.tr/token/app/token"

// ITS credentials al
const creds = getITSCredentials()
// Sonuç: { glnNo, username, password, baseUrl }
```

### Ayarları Kaydetme

```javascript
import { saveSettings } from '@/utils/settingsHelper'

const newSettings = {
  itsGlnNo: '8680001084524',
  itsUsername: 'user123',
  // ...
}

const result = saveSettings(newSettings)
if (result.success) {
  console.log('✅ Kaydedildi')
}
```

### Validasyon

```javascript
import { validateSettings } from '@/utils/settingsHelper'

const validation = validateSettings()
if (!validation.isValid) {
  console.log('❌ Hatalar:', validation.errors)
}
```

## 🎯 Backend'de Kullanım

Backend'de bu ayarları kullanmak için frontend'den API'ye gönderin veya ortam değişkenleri kullanın.

### Örnek: PTS Service Güncelleme

```javascript
// windows-backend/services/ptsService.js
import { getSettings, getITSCredentials } from '../utils/settingsHelper.js'

const PTS_CONFIG = {
  ...getITSCredentials(),
  tokenUrl: getITSUrl('itsTokenUrl'),
  // ...
}
```

## ⚠️ Önemli Notlar

1. **Güvenlik**: Şifreler localStorage'da plain text olarak saklanır. Production ortamında daha güvenli bir yöntem kullanın.

2. **Senkronizasyon**: localStorage browser bazlıdır. Farklı browser'larda ayarlar farklı olacaktır.

3. **Yedekleme**: Ayarları düzenli olarak yedekleyin. Browser cache temizlendiğinde silinebilir.

4. **Backend Restart**: Bazı ayarlar değiştiğinde backend'in yeniden başlatılması gerekebilir.

## 🔍 Ayarları Görüntüleme (Console)

Browser console'da ayarları görmek için:

```javascript
// Tüm ayarları göster
console.log(JSON.parse(localStorage.getItem('appSettings')))

// Belirli bir ayarı göster
const settings = JSON.parse(localStorage.getItem('appSettings'))
console.log('GLN:', settings.itsGlnNo)
```

## 📝 Ayar Şablonu (Örnek)

```json
{
  "itsGlnNo": "8680001084524",
  "itsUsername": "86800010845240000",
  "itsPassword": "1981aa",
  "itsWebServiceUrl": "https://its2.saglik.gov.tr",
  "itsTokenUrl": "/token/app/token",
  "itsDepoSatisUrl": "/wholesale/app/dispatch",
  "itsCheckStatusUrl": "/reference/app/check_status",
  "itsDeaktivasyonUrl": "/common/app/deactivation",
  "itsMalAlimUrl": "/common/app/accept",
  "itsMalIadeUrl": "/common/app/return",
  "itsSatisIptalUrl": "/wholesale/app/dispatchcancel",
  "itsEczaneSatisUrl": "/prescription/app/pharmacysale",
  "itsEczaneSatisIptalUrl": "/prescription/app/pharmacysalecancel",
  "itsTakasDevirUrl": "/common/app/transfer",
  "itsTakasIptalUrl": "/common/app/transfercancel",
  "itsCevapKodUrl": "/reference/app/errorcode",
  "itsPaketSorguUrl": "/pts/app/search",
  "itsPaketIndirUrl": "/pts/app/GetPackage",
  "itsPaketGonderUrl": "/pts/app/SendPackage",
  "itsDogrulamaUrl": "/reference/app/verification",
  "erpWebServiceUrl": "http://localhost:5000",
  "urunBarkodBilgisi": "STOK_KODU",
  "urunItsBilgisi": "TBLSTSABIT.KOD_5='BESERI'",
  "urunUtsBilgisi": "TBLSTSABIT.KOD_5='UTS'",
  "cariGlnBilgisi": "TBLCASABIT.EMAIL",
  "cariUtsBilgisi": "TBLCASABITEK.KULL3S"
}
```

## 🚀 Hızlı Başlangıç

1. Dashboard'dan **Ayarlar** kartına tıklayın
2. **ITS Ayarları** sekmesinde:
   - GLN No
   - Kullanıcı Adı
   - Şifre
   - Web Servis Adresi girin
3. **💾 Kaydet** butonuna basın
4. Diğer sekmelerden gerekli ayarları yapın
5. Test edin!

Artık sistem ayarları hazır! 🎉



