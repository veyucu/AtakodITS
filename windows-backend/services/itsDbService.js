import { getConnection } from '../config/database.js'
import sql from 'mssql'
import fs from 'fs'
import path from 'path'
import { fileURLToPath } from 'url'
import iconv from 'iconv-lite'
import { log } from '../utils/logger.js'

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)

/**
 * Türkçe karakter düzeltme fonksiyonu
 */
function fixTurkishChars(str) {
  if (!str || typeof str !== 'string') return str
  
  try {
    // CP1254 (Turkish) encoding'den UTF-8'e dönüştür
    const buffer = Buffer.from(str, 'binary')
    return iconv.decode(buffer, 'cp1254')
  } catch (error) {
    return str
  }
}

/**
 * Object içindeki string değerleri düzeltme
 */
function fixObjectStrings(obj) {
  if (!obj || typeof obj !== 'object') return obj
  
  const fixed = {}
  for (const [key, value] of Object.entries(obj)) {
    if (typeof value === 'string') {
      fixed[key] = fixTurkishChars(value)
    } else if (value && typeof value === 'object') {
      fixed[key] = fixObjectStrings(value)
    } else {
      fixed[key] = value
    }
  }
  return fixed
}

/**
 * ITS tablosunu ve indexlerini oluşturur
 */
export async function createTablesIfNotExists() {
  try {
    log('📋 ITS tabloları kontrol ediliyor...')
    
    const pool = await getConnection()
    
    // SQL scriptini oku ve çalıştır
    const sqlScriptPath = path.join(__dirname, '../sql/create-its-tables.sql')
    const sqlScript = fs.readFileSync(sqlScriptPath, 'utf8')
    
    // SQL scriptini batch'lere ayır (GO statement'larına göre)
    const batches = sqlScript.split(/\bGO\b/gi).filter(batch => batch.trim())
    
    for (const batch of batches) {
      if (batch.trim()) {
        await pool.request().query(batch)
      }
    }
    
    log('✅ ITS tabloları hazır')
    return { success: true }
  } catch (error) {
    console.error('❌ ITS tabloları oluşturma hatası:', error)
    throw error
  }
}

/**
 * ITS kaydı ekler
 */
export async function addITSRecord(data) {
  try {
    const pool = await getConnection()
    
    const result = await pool.request()
      .input('HAR_RECNO', sql.Int, data.HAR_RECNO || null)
      .input('TURU', sql.VarChar(3), data.TURU)
      .input('FTIRSIP', sql.Char(1), data.FTIRSIP)
      .input('FATIRS_NO', sql.VarChar(15), data.FATIRS_NO)
      .input('CARI_KODU', sql.VarChar(35), data.CARI_KODU)
      .input('STOK_KODU', sql.VarChar(35), data.STOK_KODU)
      .input('GTIN', sql.VarChar(15), data.GTIN)
      .input('SERI_NO', sql.VarChar(35), data.SERI_NO)
      .input('MIAD', sql.VarChar(25), data.MIAD)
      .input('LOT_NO', sql.VarChar(35), data.LOT_NO)
      .input('URETIM_TARIHI', sql.VarChar(25), data.URETIM_TARIHI)
      .input('CARRIER_LABEL', sql.VarChar(100), data.CARRIER_LABEL)
      .input('DURUM', sql.VarChar(20), data.DURUM || 'BEKLEMEDE')
      .input('KULLANICI', sql.VarChar(35), data.KULLANICI)
      .query(`
        INSERT INTO AKTBLITSUTS (
          HAR_RECNO, TURU, FTIRSIP, FATIRS_NO, CARI_KODU, STOK_KODU, GTIN, SERI_NO,
          MIAD, LOT_NO, URETIM_TARIHI, CARRIER_LABEL, DURUM, KULLANICI, KAYIT_TARIHI
        ) VALUES (
          @HAR_RECNO, @TURU, @FTIRSIP, @FATIRS_NO, @CARI_KODU, @STOK_KODU, @GTIN, @SERI_NO,
          @MIAD, @LOT_NO, @URETIM_TARIHI, @CARRIER_LABEL, @DURUM, @KULLANICI, GETDATE()
        );
        SELECT SCOPE_IDENTITY() AS RECNO;
      `)
    
    return { 
      success: true, 
      recno: result.recordset[0].RECNO 
    }
  } catch (error) {
    console.error('❌ ITS kaydı ekleme hatası:', error)
    throw error
  }
}

/**
 * ITS kayıtlarını listeler
 */
export async function listITSRecords(filters = {}) {
  try {
    const pool = await getConnection()
    const request = pool.request()
    
    let query = `
      SELECT 
        RECNO,
        HAR_RECNO,
        TURU,
        FTIRSIP,
        FATIRS_NO,
        CARI_KODU,
        STOK_KODU,
        GTIN,
        SERI_NO,
        MIAD,
        LOT_NO,
        URETIM_TARIHI,
        CARRIER_LABEL,
        DURUM,
        BILDIRIM_ID,
        BILDIRIM_TARIHI,
        KAYIT_TARIHI,
        KULLANICI
      FROM AKTBLITSUTS WITH (NOLOCK)
      WHERE 1=1
    `
    
    // Filtreleme
    if (filters.startDate && filters.endDate) {
      request.input('startDate', sql.DateTime, new Date(filters.startDate))
      request.input('endDate', sql.DateTime, new Date(filters.endDate))
      query += ` AND KAYIT_TARIHI BETWEEN @startDate AND @endDate`
    }
    
    if (filters.durum) {
      request.input('durum', sql.VarChar(20), filters.durum)
      query += ` AND DURUM = @durum`
    }
    
    if (filters.fatirsNo) {
      request.input('fatirsNo', sql.VarChar(15), filters.fatirsNo)
      query += ` AND FATIRS_NO = @fatirsNo`
    }
    
    if (filters.gtin) {
      request.input('gtin', sql.VarChar(15), filters.gtin)
      query += ` AND GTIN = @gtin`
    }
    
    if (filters.seriNo) {
      request.input('seriNo', sql.VarChar(35), filters.seriNo)
      query += ` AND SERI_NO = @seriNo`
    }
    
    if (filters.cariKodu) {
      request.input('cariKodu', sql.VarChar(35), filters.cariKodu)
      query += ` AND CARI_KODU = @cariKodu`
    }
    
    if (filters.kullanici) {
      request.input('kullanici', sql.VarChar(35), filters.kullanici)
      query += ` AND KULLANICI = @kullanici`
    }
    
    query += ` ORDER BY KAYIT_TARIHI DESC`
    
    const result = await request.query(query)
    
    // Türkçe karakter düzeltmesi
    const records = result.recordset.map(record => fixObjectStrings(record))
    
    return {
      success: true,
      data: records,
      count: records.length
    }
  } catch (error) {
    console.error('❌ ITS kayıtları listeleme hatası:', error)
    throw error
  }
}

/**
 * ITS kaydını RECNO'ya göre getirir
 */
export async function getITSRecord(recno) {
  try {
    const pool = await getConnection()
    
    const result = await pool.request()
      .input('recno', sql.Int, recno)
      .query(`
        SELECT 
          RECNO,
          HAR_RECNO,
          TURU,
          FTIRSIP,
          FATIRS_NO,
          CARI_KODU,
          STOK_KODU,
          GTIN,
          SERI_NO,
          MIAD,
          LOT_NO,
          URETIM_TARIHI,
          CARRIER_LABEL,
          DURUM,
          BILDIRIM_ID,
          BILDIRIM_TARIHI,
          KAYIT_TARIHI,
          KULLANICI
        FROM AKTBLITSUTS WITH (NOLOCK)
        WHERE RECNO = @recno
      `)
    
    if (result.recordset.length === 0) {
      return { success: false, message: 'Kayıt bulunamadı' }
    }
    
    const record = fixObjectStrings(result.recordset[0])
    
    return {
      success: true,
      data: record
    }
  } catch (error) {
    console.error('❌ ITS kaydı getirme hatası:', error)
    throw error
  }
}

/**
 * ITS kaydını günceller
 */
export async function updateITSRecord(recno, data) {
  try {
    const pool = await getConnection()
    const request = pool.request().input('recno', sql.Int, recno)
    
    const updateFields = []
    
    if (data.HAR_RECNO !== undefined) {
      request.input('HAR_RECNO', sql.Int, data.HAR_RECNO)
      updateFields.push('HAR_RECNO = @HAR_RECNO')
    }
    if (data.TURU !== undefined) {
      request.input('TURU', sql.VarChar(3), data.TURU)
      updateFields.push('TURU = @TURU')
    }
    if (data.FTIRSIP !== undefined) {
      request.input('FTIRSIP', sql.Char(1), data.FTIRSIP)
      updateFields.push('FTIRSIP = @FTIRSIP')
    }
    if (data.FATIRS_NO !== undefined) {
      request.input('FATIRS_NO', sql.VarChar(15), data.FATIRS_NO)
      updateFields.push('FATIRS_NO = @FATIRS_NO')
    }
    if (data.CARI_KODU !== undefined) {
      request.input('CARI_KODU', sql.VarChar(35), data.CARI_KODU)
      updateFields.push('CARI_KODU = @CARI_KODU')
    }
    if (data.STOK_KODU !== undefined) {
      request.input('STOK_KODU', sql.VarChar(35), data.STOK_KODU)
      updateFields.push('STOK_KODU = @STOK_KODU')
    }
    if (data.GTIN !== undefined) {
      request.input('GTIN', sql.VarChar(15), data.GTIN)
      updateFields.push('GTIN = @GTIN')
    }
    if (data.SERI_NO !== undefined) {
      request.input('SERI_NO', sql.VarChar(35), data.SERI_NO)
      updateFields.push('SERI_NO = @SERI_NO')
    }
    if (data.MIAD !== undefined) {
      request.input('MIAD', sql.VarChar(25), data.MIAD)
      updateFields.push('MIAD = @MIAD')
    }
    if (data.LOT_NO !== undefined) {
      request.input('LOT_NO', sql.VarChar(35), data.LOT_NO)
      updateFields.push('LOT_NO = @LOT_NO')
    }
    if (data.URETIM_TARIHI !== undefined) {
      request.input('URETIM_TARIHI', sql.VarChar(25), data.URETIM_TARIHI)
      updateFields.push('URETIM_TARIHI = @URETIM_TARIHI')
    }
    if (data.CARRIER_LABEL !== undefined) {
      request.input('CARRIER_LABEL', sql.VarChar(100), data.CARRIER_LABEL)
      updateFields.push('CARRIER_LABEL = @CARRIER_LABEL')
    }
    if (data.DURUM !== undefined) {
      request.input('DURUM', sql.VarChar(20), data.DURUM)
      updateFields.push('DURUM = @DURUM')
    }
    if (data.BILDIRIM_ID !== undefined) {
      request.input('BILDIRIM_ID', sql.NVarChar(50), data.BILDIRIM_ID)
      updateFields.push('BILDIRIM_ID = @BILDIRIM_ID')
    }
    if (data.BILDIRIM_TARIHI !== undefined) {
      request.input('BILDIRIM_TARIHI', sql.DateTime, data.BILDIRIM_TARIHI)
      updateFields.push('BILDIRIM_TARIHI = @BILDIRIM_TARIHI')
    }
    if (data.KULLANICI !== undefined) {
      request.input('KULLANICI', sql.VarChar(35), data.KULLANICI)
      updateFields.push('KULLANICI = @KULLANICI')
    }
    
    if (updateFields.length === 0) {
      return { success: false, message: 'Güncellenecek alan yok' }
    }
    
    // KAYIT_TARIHI her zaman güncellenir
    updateFields.push('KAYIT_TARIHI = GETDATE()')
    
    const query = `
      UPDATE AKTBLITSUTS 
      SET ${updateFields.join(', ')}
      WHERE RECNO = @recno
    `
    
    await request.query(query)
    
    return { success: true, message: 'Kayıt güncellendi' }
  } catch (error) {
    console.error('❌ ITS kaydı güncelleme hatası:', error)
    throw error
  }
}

/**
 * ITS kaydını siler
 */
export async function deleteITSRecord(recno) {
  try {
    const pool = await getConnection()
    
    await pool.request()
      .input('recno', sql.Int, recno)
      .query(`DELETE FROM AKTBLITSUTS WHERE RECNO = @recno`)
    
    return { success: true, message: 'Kayıt silindi' }
  } catch (error) {
    console.error('❌ ITS kaydı silme hatası:', error)
    throw error
  }
}

/**
 * Toplu bildirim durumu güncelleme
 */
export async function updateBulkNotificationStatus(recnos, bildirimId, bildirimTarihi, durum) {
  try {
    const pool = await getConnection()
    
    await pool.request()
      .input('recnos', sql.VarChar(sql.MAX), recnos.join(','))
      .input('bildirimId', sql.NVarChar(50), bildirimId)
      .input('bildirimTarihi', sql.DateTime, bildirimTarihi)
      .input('durum', sql.VarChar(20), durum)
      .query(`
        UPDATE AKTBLITSUTS 
        SET 
          BILDIRIM_ID = @bildirimId,
          BILDIRIM_TARIHI = @bildirimTarihi,
          DURUM = @durum,
          KAYIT_TARIHI = GETDATE()
        WHERE RECNO IN (SELECT value FROM STRING_SPLIT(@recnos, ','))
      `)
    
    return { success: true, message: 'Toplu güncelleme tamamlandı' }
  } catch (error) {
    console.error('❌ Toplu güncelleme hatası:', error)
    throw error
  }
}

/**
 * İstatistikler
 */
export async function getITSStatistics(filters = {}) {
  try {
    const pool = await getConnection()
    const request = pool.request()
    
    let whereClause = 'WHERE 1=1'
    
    if (filters.startDate && filters.endDate) {
      request.input('startDate', sql.DateTime, new Date(filters.startDate))
      request.input('endDate', sql.DateTime, new Date(filters.endDate))
      whereClause += ` AND KAYIT_TARIHI BETWEEN @startDate AND @endDate`
    }
    
    const result = await request.query(`
      SELECT 
        COUNT(*) as TOPLAM,
        COUNT(DISTINCT GTIN) as FARKLI_URUN,
        COUNT(DISTINCT FATIRS_NO) as FARKLI_FATURA,
        COUNT(DISTINCT CARI_KODU) as FARKLI_CARI,
        SUM(CASE WHEN DURUM = 'BEKLEMEDE' THEN 1 ELSE 0 END) as BEKLEMEDE,
        SUM(CASE WHEN DURUM = 'BILDIRILDI' THEN 1 ELSE 0 END) as BILDIRILDI,
        SUM(CASE WHEN DURUM = 'HATA' THEN 1 ELSE 0 END) as HATA,
        SUM(CASE WHEN BILDIRIM_ID IS NOT NULL THEN 1 ELSE 0 END) as BILDIRIM_YAPILAN
      FROM AKTBLITSUTS WITH (NOLOCK)
      ${whereClause}
    `)
    
    return {
      success: true,
      data: result.recordset[0]
    }
  } catch (error) {
    console.error('❌ İstatistik getirme hatası:', error)
    throw error
  }
}

export default {
  createTablesIfNotExists,
  addITSRecord,
  listITSRecords,
  getITSRecord,
  updateITSRecord,
  deleteITSRecord,
  updateBulkNotificationStatus,
  getITSStatistics
}

