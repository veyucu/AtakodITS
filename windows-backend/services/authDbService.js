import { getPTSConnection } from '../config/database.js'

// Tabloları oluştur
export async function createTablesIfNotExists() {
  try {
    const pool = await getPTSConnection()

    // AKTBLKULLANICI tablosu (yetki kolonları ile)
    await pool.request().query(`
      IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AKTBLKULLANICI' AND xtype='U')
      BEGIN
        CREATE TABLE AKTBLKULLANICI (
          ID INT IDENTITY(1,1) PRIMARY KEY,
          KULLANICI_ADI NVARCHAR(50) NOT NULL UNIQUE,
          SIFRE NVARCHAR(255) NOT NULL,
          AD_SOYAD NVARCHAR(100),
          EMAIL NVARCHAR(100),
          ROL NVARCHAR(20) DEFAULT 'user',
          DEPARTMAN NVARCHAR(50),
          AKTIF BIT DEFAULT 1,
          YETKI_URUN_HAZIRLAMA BIT DEFAULT 1,
          YETKI_PTS BIT DEFAULT 1,
          YETKI_MESAJ_KODLARI BIT DEFAULT 0,
          YETKI_AYARLAR BIT DEFAULT 0,
          YETKI_KULLANICILAR BIT DEFAULT 0,
          SON_GIRIS DATETIME,
          OLUSTURMA_TARIHI DATETIME DEFAULT GETDATE()
        );
      END
    `)

    // Yetki kolonlarını ekle (mevcut tabloya)
    const columns = [
      'YETKI_URUN_HAZIRLAMA',
      'YETKI_PTS',
      'YETKI_MESAJ_KODLARI',
      'YETKI_AYARLAR',
      'YETKI_KULLANICILAR'
    ]

    for (const col of columns) {
      await pool.request().query(`
        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLKULLANICI') AND name = '${col}')
        BEGIN
          ALTER TABLE AKTBLKULLANICI ADD ${col} BIT DEFAULT ${col === 'YETKI_URUN_HAZIRLAMA' || col === 'YETKI_PTS' ? '1' : '0'};
        END
      `)
    }

    // Varsayılan admin kullanıcısı (tüm yetkiler)
    await pool.request().query(`
      IF NOT EXISTS (SELECT * FROM AKTBLKULLANICI WHERE KULLANICI_ADI = 'admin')
      BEGIN
        INSERT INTO AKTBLKULLANICI (KULLANICI_ADI, SIFRE, AD_SOYAD, EMAIL, ROL, DEPARTMAN, AKTIF,
          YETKI_URUN_HAZIRLAMA, YETKI_PTS, YETKI_MESAJ_KODLARI, YETKI_AYARLAR, YETKI_KULLANICILAR)
        VALUES ('admin', 'admin123', 'Admin Kullanıcı', 'admin@atakodits.com', 'admin', 'Yönetim', 1,
          1, 1, 1, 1, 1);
      END
      ELSE
      BEGIN
        UPDATE AKTBLKULLANICI SET 
          YETKI_URUN_HAZIRLAMA = 1, YETKI_PTS = 1, YETKI_MESAJ_KODLARI = 1, 
          YETKI_AYARLAR = 1, YETKI_KULLANICILAR = 1
        WHERE KULLANICI_ADI = 'admin';
      END
    `)

    // Demo kullanıcı
    await pool.request().query(`
      IF NOT EXISTS (SELECT * FROM AKTBLKULLANICI WHERE KULLANICI_ADI = 'demo')
      BEGIN
        INSERT INTO AKTBLKULLANICI (KULLANICI_ADI, SIFRE, AD_SOYAD, EMAIL, ROL, DEPARTMAN, AKTIF,
          YETKI_URUN_HAZIRLAMA, YETKI_PTS, YETKI_MESAJ_KODLARI, YETKI_AYARLAR, YETKI_KULLANICILAR)
        VALUES ('demo', 'demo123', 'Demo Kullanıcı', 'demo@atakodits.com', 'user', 'Satış', 1,
          1, 1, 0, 0, 0);
      END
    `)

    // AKTBLAYAR tablosu
    await pool.request().query(`
      IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AKTBLAYAR' AND xtype='U')
      BEGIN
        CREATE TABLE AKTBLAYAR (
          ID INT IDENTITY(1,1) PRIMARY KEY,
          AYAR_ADI NVARCHAR(100) NOT NULL UNIQUE,
          AYAR_DEGERI NVARCHAR(500),
          ACIKLAMA NVARCHAR(200),
          GUNCELLEME_TARIHI DATETIME DEFAULT GETDATE()
        );
      END
    `)

    // TÜM AYARLAR - SettingsPage'deki DEFAULT_SETTINGS ile uyumlu
    const allSettings = [
      // ITS Temel Ayarları
      { name: 'itsGlnNo', value: '', desc: 'ITS GLN No' },
      { name: 'itsUsername', value: '', desc: 'ITS Kullanıcı Adı' },
      { name: 'itsPassword', value: '', desc: 'ITS Şifre' },
      { name: 'itsWebServiceUrl', value: 'https://its2.saglik.gov.tr', desc: 'ITS Web Servis Adresi' },

      // ITS Endpoint URL'leri
      { name: 'itsTokenUrl', value: '/token/app/token', desc: 'Token URL' },
      { name: 'itsDepoSatisUrl', value: '/wholesale/app/dispatch', desc: 'Depo Satış URL' },
      { name: 'itsCheckStatusUrl', value: '/reference/app/check_status', desc: 'Durum Kontrol URL' },
      { name: 'itsDeaktivasyonUrl', value: '/common/app/deactivation', desc: 'Deaktivasyon URL' },
      { name: 'itsMalAlimUrl', value: '/common/app/accept', desc: 'Mal Alım URL' },
      { name: 'itsMalIadeUrl', value: '/common/app/return', desc: 'Mal İade URL' },
      { name: 'itsSatisIptalUrl', value: '/wholesale/app/dispatchcancel', desc: 'Satış İptal URL' },
      { name: 'itsEczaneSatisUrl', value: '/prescription/app/pharmacysale', desc: 'Eczane Satış URL' },
      { name: 'itsEczaneSatisIptalUrl', value: '/prescription/app/pharmacysalecancel', desc: 'Eczane Satış İptal URL' },
      { name: 'itsTakasDevirUrl', value: '/common/app/transfer', desc: 'Takas Devir URL' },
      { name: 'itsTakasIptalUrl', value: '/common/app/transfercancel', desc: 'Takas İptal URL' },
      { name: 'itsCevapKodUrl', value: '/reference/app/errorcode', desc: 'Cevap Kod URL' },
      { name: 'itsPaketSorguUrl', value: '/pts/app/search', desc: 'Paket Sorgu URL' },
      { name: 'itsPaketIndirUrl', value: '/pts/app/GetPackage', desc: 'Paket İndir URL' },
      { name: 'itsPaketGonderUrl', value: '/pts/app/SendPackage', desc: 'Paket Gönder URL' },
      { name: 'itsDogrulamaUrl', value: '/reference/app/verification', desc: 'Doğrulama URL' },

      // UTS Ayarları
      { name: 'utsId', value: '', desc: 'UTS ID (40 karakter)' },
      { name: 'utsWebServiceUrl', value: 'https://utsuygulama.saglik.gov.tr', desc: 'UTS Web Servis Adresi' },
      { name: 'utsVermeBildirimiUrl', value: '/UTS/uh/rest/bildirim/verme/ekle', desc: 'Verme Bildirimi URL' },
      { name: 'utsVermeIptalBildirimiUrl', value: '/UTS/uh/rest/bildirim/verme/iptal', desc: 'Verme İptal Bildirimi URL' },
      { name: 'utsAlmaBildirimiUrl', value: '/UTS/uh/rest/bildirim/alma/ekle', desc: 'Alma Bildirimi URL' },
      { name: 'utsFirmaSorgulaUrl', value: '/UTS/rest/kurum/firmaSorgula', desc: 'Firma Sorgula URL' },
      { name: 'utsUrunSorgulaUrl', value: '/UTS/rest/tibbiCihaz/urunSorgula', desc: 'Ürün Sorgula URL' },
      { name: 'utsBekleyenleriSorgulaUrl', value: '/UTS/uh/rest/bildirim/alma/bekleyenler/sorgula', desc: 'Bekleyenler Sorgula URL' },
      { name: 'utsBildirimSorgulaUrl', value: '/UTS/uh/rest/bildirim/sorgula/offset', desc: 'Bildirim Sorgula URL' },
      { name: 'utsStokYapilabilirTekilUrunSorgulaUrl', value: '/UTS/uh/rest/stokYapilabilirTekilUrun/sorgula', desc: 'Stok Yapılabilir Tekil Ürün URL' },

      // ERP Ayarları
      { name: 'erpWebServiceUrl', value: 'http://localhost:5000', desc: 'ERP Web Servis Adresi' },

      // Ürün Ayarları
      { name: 'urunBarkodBilgisi', value: 'STOK_KODU', desc: 'Ürün barkod bilgisi alanı' },
      { name: 'urunItsBilgisi', value: "TBLSTSABIT.KOD_5='BESERI'", desc: 'ITS ürün filtresi' },
      { name: 'urunUtsBilgisi', value: "TBLSTSABIT.KOD_5='UTS'", desc: 'UTS ürün filtresi' },

      // Cari Ayarları
      { name: 'cariGlnBilgisi', value: 'TBLCASABIT.EMAIL', desc: 'Cari GLN bilgisi alanı' },
      { name: 'cariUtsBilgisi', value: 'TBLCASABITEK.KULL3S', desc: 'Cari UTS bilgisi alanı' }
    ]

    for (const setting of allSettings) {
      await pool.request()
        .input('ayarAdi', setting.name)
        .input('ayarDegeri', setting.value)
        .input('aciklama', setting.desc)
        .query(`
          IF NOT EXISTS (SELECT * FROM AKTBLAYAR WHERE AYAR_ADI = @ayarAdi)
          BEGIN
            INSERT INTO AKTBLAYAR (AYAR_ADI, AYAR_DEGERI, ACIKLAMA) 
            VALUES (@ayarAdi, @ayarDegeri, @aciklama);
          END
        `)
    }

    return { success: true }
  } catch (error) {
    console.error('Auth tabloları oluşturma hatası:', error)
    return { success: false, error: error.message }
  }
}
