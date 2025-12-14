import express from 'express'
import documentService from '../services/documentService.js'
import { parseITSBarcode, formatMiad } from '../utils/itsParser.js'

const router = express.Router()

// GET /api/documents - Tüm belgeleri getir (tarih zorunlu)
router.get('/', async (req, res) => {
  try {
    // Tarih parametresi zorunlu
    const date = req.query.date
    
    if (!date) {
      return res.status(400).json({
        success: false,
        message: 'Tarih parametresi zorunludur (date)'
      })
    }
    
    const documents = await documentService.getAllDocuments(date)
    
    res.json({
      success: true,
      documents: documents,
      count: documents.length,
      date: date
    })
  } catch (error) {
    console.error('Belgeler getirme hatası:', error)
    res.status(500).json({
      success: false,
      message: 'Belgeler alınamadı',
      error: error.message
    })
  }
})

// GET /api/documents/:id - Belirli bir belgeyi getir
router.get('/:id', async (req, res) => {
  try {
    const { id } = req.params
    
    // ID formatı: SUBE_KODU-FTIRSIP-FATIRS_NO
    const parts = id.split('-')
    
    if (parts.length !== 3) {
      return res.status(400).json({
        success: false,
        message: 'Geçersiz belge ID formatı'
      })
    }
    
    const [subeKodu, ftirsip, fatirs_no] = parts
    
    const document = await documentService.getDocumentById(subeKodu, ftirsip, fatirs_no)
    
    if (!document) {
      return res.status(404).json({
        success: false,
        message: 'Belge bulunamadı'
      })
    }
    
    res.json({
      success: true,
      data: document
    })
  } catch (error) {
    console.error('Belge detay hatası:', error)
    res.status(500).json({
      success: false,
      message: 'Belge detayı alınamadı',
      error: error.message
    })
  }
})

// GET /api/documents/:documentId/item/:itemId/its-records - ITS Kayıtlarını Getir
router.get('/:documentId/item/:itemId/its-records', async (req, res) => {
  try {
    const { documentId, itemId } = req.params
    
    // Document ID parse et
    const [subeKodu, ftirsip, fatirs_no] = documentId.split('-')
    
    // Kayıt tipi belirle
    const kayitTipi = ftirsip === '6' ? 'M' : 'A'
    
    const records = await documentService.getITSBarcodeRecords(
      subeKodu,
      fatirs_no,
      itemId,
      kayitTipi
    )
    
    res.json({
      success: true,
      data: records,
      count: records.length
    })
    
  } catch (error) {
    console.error('❌ ITS Kayıtları Getirme Hatası:', error)
    res.status(500).json({
      success: false,
      message: 'ITS kayıtları alınamadı',
      error: error.message
    })
  }
})

// DELETE /api/documents/:documentId/item/:itemId/its-records - ITS Kayıtlarını Sil
router.delete('/:documentId/item/:itemId/its-records', async (req, res) => {
  try {
    const { documentId, itemId } = req.params
    const { seriNos } = req.body // Array of seri numbers to delete
    
    if (!seriNos || !Array.isArray(seriNos) || seriNos.length === 0) {
      return res.status(400).json({
        success: false,
        message: 'Silinecek seri numaraları belirtilmeli'
      })
    }
    
    // Document ID parse et
    const [subeKodu, ftirsip, fatirs_no] = documentId.split('-')
    
    const result = await documentService.deleteITSBarcodeRecords(
      seriNos,
      subeKodu,
      fatirs_no,
      itemId
    )
    
    res.json({
      success: true,
      message: `${result.deletedCount} kayıt silindi`,
      deletedCount: result.deletedCount
    })
    
  } catch (error) {
    console.error('❌ ITS Kayıt Silme Hatası:', error)
    res.status(500).json({
      success: false,
      message: 'ITS kayıtları silinemedi',
      error: error.message
    })
  }
})

// POST /api/documents/its-barcode - ITS Karekod Okut ve Kaydet
router.post('/its-barcode', async (req, res) => {
  try {
    const { 
      barcode,      // ITS 2D Karekod
      documentId,   // Belge ID (SUBE_KODU-FTIRSIP-FATIRS_NO)
      itemId,       // INCKEYNO
      stokKodu,
      belgeTip,     // STHAR_HTUR
      gckod,        // STHAR_GCKOD
      belgeNo,
      belgeTarihi,
      docType       // '6' = Sipariş, '1'/'2' = Fatura
    } = req.body
    
    console.log('📱 ITS Karekod İsteği:', { barcode, documentId, itemId })
    
    // 1. Karekodu parse et
    const parseResult = parseITSBarcode(barcode)
    
    if (!parseResult.success) {
      return res.status(400).json({
        success: false,
        message: 'Karekod parse edilemedi: ' + parseResult.error
      })
    }
    
    const parsedData = parseResult.data
    
    // 2. Belge ID'sini parse et
    const [subeKodu, ftirsip, fatirs_no] = documentId.split('-')
    
    // 3. KAYIT_TIPI belirle
    const kayitTipi = docType === '6' ? 'M' : 'A' // Sipariş = M, Fatura = A
    
    // 4. TBLSERITRA'ya kaydet
    const saveResult = await documentService.saveITSBarcode({
      kayitTipi,
      seriNo: parsedData.seriNo,
      stokKodu,
      straInc: itemId,
      tarih: belgeTarihi,
      acik1: parsedData.miad,      // Miad
      acik2: parsedData.lot,        // Lot
      gckod,
      miktar: 1,
      belgeNo,
      belgeTip,
      subeKodu,
      depoKod: '0',
      ilcGtin: parsedData.barkod    // Okutulan Barkod
    })
    
    // Duplicate kontrolü
    if (!saveResult.success) {
      console.log('⚠️ ITS Karekod kaydedilemedi:', saveResult.error, saveResult.message)
      return res.status(400).json(saveResult) // error ve message'ı frontend'e gönder
    }
    
    console.log('✅ ITS Karekod başarıyla kaydedildi!')
    res.json({
      success: true,
      message: 'ITS karekod başarıyla kaydedildi',
      data: {
        barkod: parsedData.barkod,
        seriNo: parsedData.seriNo,
        miad: formatMiad(parsedData.miad),
        lot: parsedData.lot
      }
    })
    
  } catch (error) {
    console.error('❌ ITS Karekod Kaydetme Hatası:', error)
    res.status(500).json({
      success: false,
      message: 'ITS karekod kaydedilemedi',
      error: error.message
    })
  }
})

export default router
