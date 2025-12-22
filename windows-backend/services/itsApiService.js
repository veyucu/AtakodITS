/**
 * ITS API Servisi - İlaç Takip Sistemi Web Servisi İşlemleri
 * Bu dosya ITS web servisleriyle iletişim kurar (Satış Bildirimi, İptal, Doğrulama vb.)
 */

import axios from 'axios'
import { getConnection, getPTSConnection } from '../config/database.js'
import * as settingsHelper from '../utils/settingsHelper.js'
import { log } from '../utils/logger.js'
import { toSqlTurkishChars, fixTurkishChars } from '../utils/stringUtils.js'

/**
 * Ayarları yükle ve config oluştur
 */
function loadITSConfig(frontendSettings = null) {
    if (frontendSettings) {
        settingsHelper.updateSettings(frontendSettings)
    }

    const creds = settingsHelper.getITSCredentials()

    return {
        username: creds.username,
        password: creds.password,
        glnNo: creds.glnNo,
        baseUrl: creds.baseUrl,
        tokenUrl: settingsHelper.getSetting('itsTokenUrl', '/token/app/token'),
        depoSatisUrl: settingsHelper.getSetting('itsDepoSatisUrl', '/wholesale/app/dispatch'),
        satisIptalUrl: settingsHelper.getSetting('itsSatisIptalUrl', '/wholesale/app/dispatchcancel'),
        dogrulamaUrl: settingsHelper.getSetting('itsDogrulamaUrl', '/reference/app/verification'),
        checkStatusUrl: settingsHelper.getSetting('itsCheckStatusUrl', '/reference/app/check_status'),
        cevapKodUrl: settingsHelper.getSetting('itsCevapKodUrl', '/reference/app/errorcode')
    }
}

/**
 * GTIN'i 14 karaktere tamamla (başına 0 ekle)
 */
function formatGtin(gtin) {
    if (!gtin) return gtin
    const gtinStr = String(gtin).trim()
    return gtinStr.padStart(14, '0')
}

/**
 * Miad verisini yyyy-MM-dd formatına çevir
 * Gelen format: YYMMDD, YYYYMMDD veya Date objesi olabilir
 */
function formatMiad(miad) {
    if (!miad) return miad

    try {
        // Eğer Date objesi ise
        if (miad instanceof Date) {
            return miad.toISOString().split('T')[0]
        }

        const miadStr = String(miad).trim()

        // Eğer zaten yyyy-MM-dd formatında ise
        if (/^\d{4}-\d{2}-\d{2}$/.test(miadStr)) {
            return miadStr
        }

        // YYMMDD formatı (6 karakter)
        if (miadStr.length === 6) {
            const yy = miadStr.substring(0, 2)
            const mm = miadStr.substring(2, 4)
            const dd = miadStr.substring(4, 6)
            // 2000'li yıllar varsayılıyor
            const yyyy = parseInt(yy) > 50 ? `19${yy}` : `20${yy}`
            return `${yyyy}-${mm}-${dd}`
        }

        // YYYYMMDD formatı (8 karakter)
        if (miadStr.length === 8 && !miadStr.includes('-')) {
            const yyyy = miadStr.substring(0, 4)
            const mm = miadStr.substring(4, 6)
            const dd = miadStr.substring(6, 8)
            return `${yyyy}-${mm}-${dd}`
        }

        // Diğer durumlarda olduğu gibi döndür
        return miadStr
    } catch (error) {
        console.error('Miad formatlama hatası:', error)
        return miad
    }
}
/**
 * Access Token Al
 */
const getAccessToken = async (config) => {
    try {
        log('🔑 ITS Token alınıyor...')
        log('URL:', `${config.baseUrl}${config.tokenUrl}`)

        const requestBody = `{"username":"${config.username}","password":"${config.password}"}`

        const response = await axios.post(
            `${config.baseUrl}${config.tokenUrl}`,
            requestBody,
            {
                headers: { 'Content-Type': 'application/json' },
                timeout: 10000
            }
        )

        log('✅ ITS Token alındı')

        const token = response.data?.token || null

        if (!token) {
            console.error('❌ Token response\'da bulunamadı:', response.data)
            throw new Error('Token alınamadı')
        }

        return token
    } catch (error) {
        console.error('❌ ITS Token Hatası:', error.message)
        throw error
    }
}

/**
 * Depo Satış Bildirimi
 * Satış yapılan ürünlerin ITS'ye bildirilmesi
 * 
 * @param {string} karsiGlnNo - Alıcı GLN numarası
 * @param {Array} products - Ürün listesi [{gtin, seriNo/sn, miad/xd, lotNo/bn}]
 * @param {Object} frontendSettings - Frontend'den gelen ayarlar (opsiyonel)
 * @returns {Object} - { success, message, data }
 */
export const depoSatisBildirimi = async (karsiGlnNo, products, frontendSettings = null) => {
    try {
        if (!products || products.length === 0) {
            return { success: false, message: 'Bildirilecek ürün bulunamadı', data: [] }
        }

        const config = loadITSConfig(frontendSettings)

        if (!config.username || !config.password) {
            return { success: false, message: 'ITS kullanıcı adı veya şifre tanımlı değil' }
        }

        // Access Token al
        const token = await getAccessToken(config)

        // Ürün listesini hazırla
        const productList = products.map(p => ({
            gtin: formatGtin(p.gtin),
            sn: p.seriNo || p.sn,
            xd: formatMiad(p.miad || p.xd),   // Son kullanma tarihi (yyyy-MM-dd)
            bn: p.lotNo || p.bn   // Lot numarası
        }))

        log('📤 ITS Satış Bildirimi gönderiliyor:', { karsiGlnNo, productCount: productList.length })

        // API isteği
        const response = await axios.post(
            `${config.baseUrl}${config.depoSatisUrl}`,
            {
                togln: karsiGlnNo,
                productList: productList
            },
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                timeout: 30000
            }
        )

        log('✅ ITS Satış Bildirimi yanıtı:', response.data)

        // Sonuçları işle
        const results = (response.data?.productList || []).map(item => ({
            gtin: item.gtin,
            seriNo: item.sn,
            durum: item.uc  // uc = durum kodu (1 = başarılı vb.)
        }))

        const successCount = results.filter(r => r.durum == 1).length
        const errorCount = results.length - successCount

        return {
            success: true,
            message: `${successCount} ürün başarılı, ${errorCount} ürün hatalı`,
            data: results
        }

    } catch (error) {
        console.error('❌ ITS Satış Bildirimi Hatası:', error.message)
        return {
            success: false,
            message: error.response?.data?.message || error.message || 'Satış bildirimi başarısız',
            data: []
        }
    }
}

/**
 * Depo Satış İptal Bildirimi
 * Hatalı satış bildirimlerinin iptali
 */
export const depoSatisIptalBildirimi = async (karsiGlnNo, products, frontendSettings = null) => {
    try {
        if (!products || products.length === 0) {
            return { success: false, message: 'İptal edilecek ürün bulunamadı', data: [] }
        }

        const config = loadITSConfig(frontendSettings)

        if (!config.username || !config.password) {
            return { success: false, message: 'ITS kullanıcı adı veya şifre tanımlı değil' }
        }

        const token = await getAccessToken(config)

        const productList = products.map(p => ({
            gtin: formatGtin(p.gtin),
            sn: p.seriNo || p.sn,
            xd: formatMiad(p.miad || p.xd),
            bn: p.lotNo || p.bn
        }))

        log('🔴 ITS Satış İptal gönderiliyor:', { karsiGlnNo, productCount: productList.length })

        const response = await axios.post(
            `${config.baseUrl}${config.satisIptalUrl}`,
            {
                togln: karsiGlnNo,
                productList: productList
            },
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                timeout: 30000
            }
        )

        log('✅ ITS Satış İptal yanıtı:', response.data)

        const results = (response.data?.productList || []).map(item => ({
            gtin: item.gtin,
            seriNo: item.sn,
            durum: item.uc
        }))

        const successCount = results.filter(r => r.durum == 1).length
        const errorCount = results.length - successCount

        return {
            success: true,
            message: `${successCount} ürün başarıyla iptal edildi, ${errorCount} ürün hatalı`,
            data: results
        }

    } catch (error) {
        console.error('❌ ITS Satış İptal Hatası:', error.message)
        return {
            success: false,
            message: error.response?.data?.message || error.message || 'Satış iptal bildirimi başarısız',
            data: []
        }
    }
}

/**
 * Doğrulama İşlemi
 * Ürünlerin ITS'deki durumlarını doğrulama
 */
export const dogrulamaYap = async (products, frontendSettings = null) => {
    try {
        if (!products || products.length === 0) {
            return { success: false, message: 'Doğrulanacak ürün bulunamadı', data: [] }
        }

        const config = loadITSConfig(frontendSettings)

        if (!config.username || !config.password) {
            return { success: false, message: 'ITS kullanıcı adı veya şifre tanımlı değil' }
        }

        if (!config.glnNo) {
            return { success: false, message: 'GLN numarası tanımlı değil' }
        }

        const token = await getAccessToken(config)

        const productList = products.map(p => ({
            gtin: formatGtin(p.gtin),
            sn: p.seriNo || p.sn
        }))

        log('🔍 ITS Doğrulama gönderiliyor:', { glnNo: config.glnNo, productCount: productList.length })

        const response = await axios.post(
            `${config.baseUrl}${config.dogrulamaUrl}`,
            {
                dt: 'V',                    // V = Verification (Doğrulama)
                fr: config.glnNo,           // Gönderen GLN numarası
                productList: productList
            },
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                timeout: 30000
            }
        )

        log('✅ ITS Doğrulama yanıtı:', response.data)

        const results = (response.data?.productList || []).map(item => ({
            gtin: item.gtin,
            seriNo: item.sn,
            durum: item.uc,
            statu: item.status
        }))

        return {
            success: true,
            message: `${results.length} ürün doğrulandı`,
            data: results
        }

    } catch (error) {
        console.error('❌ ITS Doğrulama Hatası:', error.message)
        return {
            success: false,
            message: error.response?.data?.message || error.message || 'Doğrulama başarısız',
            data: []
        }
    }
}

/**
 * Başarısız Ürünleri Sorgula (Check Status)
 * Daha önce yapılan bildirimlerde başarısız olan ürünleri sorgulama
 */
export const basarisizlariSorgula = async (products, frontendSettings = null) => {
    try {
        if (!products || products.length === 0) {
            return { success: false, message: 'Sorgulanacak ürün bulunamadı', data: [] }
        }

        const config = loadITSConfig(frontendSettings)

        if (!config.username || !config.password) {
            return { success: false, message: 'ITS kullanıcı adı veya şifre tanımlı değil' }
        }

        const token = await getAccessToken(config)

        const productList = products.map(p => ({
            gtin: formatGtin(p.gtin),
            sn: p.seriNo || p.sn
        }))

        log('❓ ITS Başarısız Sorgulama gönderiliyor:', { productCount: productList.length })

        const response = await axios.post(
            `${config.baseUrl}${config.checkStatusUrl}`,
            {
                productList: productList
            },
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                timeout: 30000
            }
        )

        log('✅ ITS Başarısız Sorgulama yanıtı:', response.data)

        const results = (response.data?.productList || []).map(item => ({
            gtin: item.gtin,
            seriNo: item.sn,
            durum: item.uc,
            hataKodu: item.errorCode || item.ec,
            hataMesaji: item.errorMessage || item.em
        }))

        const failedCount = results.filter(r => r.durum != 1).length

        return {
            success: true,
            message: `${results.length} ürün sorgulandı, ${failedCount} adet başarısız`,
            data: results
        }

    } catch (error) {
        console.error('❌ Başarısız Sorgulama Hatası:', error.message)
        return {
            success: false,
            message: error.response?.data?.message || error.message || 'Sorgulama başarısız',
            data: []
        }
    }
}

/**
 * Bildirim Sonuçlarını Veritabanına Kaydet
 * AKTBLITSUTS tablosundaki ilgili kayıtların durumunu güncelle
 */
export const updateBildirimDurum = async (results) => {
    try {
        const pool = await getConnection()
        let updatedCount = 0

        for (const item of results) {
            if (!item.recNo) continue

            const query = `
        UPDATE AKTBLITSUTS
        SET DURUM = @durum,
            BILDIRIM_TARIHI = GETDATE()
        WHERE RECNO = @recNo
      `

            const request = pool.request()
            request.input('durum', item.durum || 'B')  // B = Bildirildi
            request.input('recNo', item.recNo)

            const result = await request.query(query)
            if (result.rowsAffected[0] > 0) {
                updatedCount++
            }
        }

        log('✅ Bildirim durumları güncellendi:', updatedCount)
        return { success: true, updatedCount }
    } catch (error) {
        console.error('❌ Bildirim Durum Güncelleme Hatası:', error.message)
        throw error
    }
}

/**
 * ITS'den Cevap Kodlarını Çek ve Veritabanına Kaydet
 * AKTBLITSMESAJ tablosuna ID ve MESAJ olarak kaydeder
 */
export const getCevapKodlari = async (frontendSettings = null) => {
    try {
        const config = loadITSConfig(frontendSettings)

        if (!config.username || !config.password) {
            return { success: false, message: 'ITS kullanıcı adı veya şifre tanımlı değil' }
        }

        const token = await getAccessToken(config)

        log('📋 ITS Cevap Kodları çekiliyor...')

        const response = await axios.post(
            `${config.baseUrl}${config.cevapKodUrl}`,
            {},
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                timeout: 30000
            }
        )

        log('✅ ITS Cevap Kodları yanıtı alındı')

        const errorCodeList = response.data?.errorCodeList || []

        if (errorCodeList.length === 0) {
            return { success: false, message: 'Cevap kodu bulunamadı', data: [] }
        }

        // Veritabanına kaydet (NETSIS DB)
        const pool = await getPTSConnection()
        let insertedCount = 0
        let updatedCount = 0

        for (const item of errorCodeList) {
            const code = parseInt(item.code)
            const message = toSqlTurkishChars(item.message || '')

            // Önce var mı kontrol et
            const checkQuery = `SELECT COUNT(*) as count FROM AKTBLITSMESAJ WHERE ID = @code`
            const checkRequest = pool.request()
            checkRequest.input('code', code)
            const checkResult = await checkRequest.query(checkQuery)

            if (checkResult.recordset[0].count === 0) {
                // Yeni kayıt ekle
                const insertQuery = `INSERT INTO AKTBLITSMESAJ (ID, MESAJ) VALUES (@code, @message)`
                const insertRequest = pool.request()
                insertRequest.input('code', code)
                insertRequest.input('message', message)
                await insertRequest.query(insertQuery)
                insertedCount++
            } else {
                // Güncelle
                const updateQuery = `UPDATE AKTBLITSMESAJ SET MESAJ = @message WHERE ID = @code`
                const updateRequest = pool.request()
                updateRequest.input('code', code)
                updateRequest.input('message', message)
                await updateRequest.query(updateQuery)
                updatedCount++
            }
        }

        log(`✅ Mesaj kodları güncellendi: ${insertedCount} yeni, ${updatedCount} güncellendi`)

        return {
            success: true,
            message: `${insertedCount} yeni mesaj eklendi, ${updatedCount} mesaj güncellendi`,
            data: errorCodeList.map(item => ({
                id: parseInt(item.code),
                mesaj: item.message
            }))
        }

    } catch (error) {
        console.error('❌ Cevap Kodları Hatası:', error.message)
        return {
            success: false,
            message: error.response?.data?.message || error.message || 'Cevap kodları alınamadı',
            data: []
        }
    }
}

/**
 * Tüm Mesaj Kodlarını Getir
 * AKTBLITSMESAJ tablosundan okur
 */
export const getAllMesajKodlari = async () => {
    try {
        const pool = await getPTSConnection()

        const query = `SELECT ID, MESAJ FROM AKTBLITSMESAJ ORDER BY ID`
        const result = await pool.request().query(query)

        const records = result.recordset.map(row => ({
            id: row.ID,
            mesaj: fixTurkishChars(row.MESAJ)
        }))

        return {
            success: true,
            data: records,
            count: records.length
        }
    } catch (error) {
        console.error('❌ Mesaj Kodları Getirme Hatası:', error.message)
        return {
            success: false,
            message: error.message,
            data: []
        }
    }
}

/**
 * Mesaj Kodunu ID'ye Göre Getir
 */
export const getMesajByCode = async (code) => {
    try {
        const pool = await getPTSConnection()

        const query = `SELECT MESAJ FROM AKTBLITSMESAJ WHERE ID = @code`
        const request = pool.request()
        request.input('code', code)
        const result = await request.query(query)

        if (result.recordset.length > 0) {
            return result.recordset[0].MESAJ
        }
        return null
    } catch (error) {
        console.error('❌ Mesaj Kodu Getirme Hatası:', error.message)
        return null
    }
}

export default {
    loadITSConfig,
    depoSatisBildirimi,
    depoSatisIptalBildirimi,
    dogrulamaYap,
    basarisizlariSorgula,
    updateBildirimDurum,
    getCevapKodlari,
    getAllMesajKodlari,
    getMesajByCode
}
