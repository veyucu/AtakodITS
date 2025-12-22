import { getPTSConnection } from '../config/database.js'

// Default ayarlar (DB boşsa kullanılır)
const DEFAULT_SETTINGS = {
  urunBarkodBilgisi: 'STOK_KODU',
  urunItsBilgisi: "TBLSTSABIT.KOD_5='BESERI'",
  urunUtsBilgisi: "TBLSTSABIT.KOD_5='UTS'",
  cariGlnBilgisi: 'TBLCASABIT.EMAIL',
  cariUtsBilgisi: 'TBLCASABITEK.KULL3S'
}

const settingsService = {
  // Tüm ayarları getir
  async getSettings() {
    try {
      const pool = await getPTSConnection()

      const result = await pool.request()
        .query('SELECT AYAR_ADI, AYAR_DEGERI FROM AKTBLAYAR')

      // DB'den gelen ayarları object'e çevir
      const settings = { ...DEFAULT_SETTINGS }

      for (const row of result.recordset) {
        settings[row.AYAR_ADI] = row.AYAR_DEGERI
      }

      return settings
    } catch (error) {
      console.error('Ayar okuma hatası:', error)
      return DEFAULT_SETTINGS
    }
  },

  // Ayarları kaydet
  async saveSettings(settings) {
    try {
      const pool = await getPTSConnection()

      for (const [key, value] of Object.entries(settings)) {
        // Önce var mı kontrol et
        const exists = await pool.request()
          .input('ayarAdi', key)
          .query('SELECT ID FROM AKTBLAYAR WHERE AYAR_ADI = @ayarAdi')

        if (exists.recordset.length > 0) {
          // Güncelle
          await pool.request()
            .input('ayarAdi', key)
            .input('ayarDegeri', value || '')
            .query(`
              UPDATE AKTBLAYAR 
              SET AYAR_DEGERI = @ayarDegeri, 
                  GUNCELLEME_TARIHI = GETDATE() 
              WHERE AYAR_ADI = @ayarAdi
            `)
        } else {
          // Yeni ekle
          await pool.request()
            .input('ayarAdi', key)
            .input('ayarDegeri', value || '')
            .query(`
              INSERT INTO AKTBLAYAR (AYAR_ADI, AYAR_DEGERI) 
              VALUES (@ayarAdi, @ayarDegeri)
            `)
        }
      }

      return { success: true }
    } catch (error) {
      console.error('Ayar kaydetme hatası:', error)
      throw error
    }
  },

  // Belirli bir ayarı getir
  async getSetting(key) {
    try {
      const pool = await getPTSConnection()

      const result = await pool.request()
        .input('ayarAdi', key)
        .query('SELECT AYAR_DEGERI FROM AKTBLAYAR WHERE AYAR_ADI = @ayarAdi')

      if (result.recordset.length > 0) {
        return result.recordset[0].AYAR_DEGERI
      }

      return DEFAULT_SETTINGS[key] || null
    } catch (error) {
      console.error('Ayar okuma hatası:', error)
      return DEFAULT_SETTINGS[key] || null
    }
  },

  // Cari GLN kolon bilgisini parse et
  parseColumnInfo(columnInfo) {
    if (!columnInfo) return { table: null, column: null }

    const parts = columnInfo.split('.')
    if (parts.length === 2) {
      return { table: parts[0], column: parts[1] }
    } else {
      return { table: 'TBLCASABIT', column: parts[0] }
    }
  }
}

export default settingsService
