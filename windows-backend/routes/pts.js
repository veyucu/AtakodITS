import express from 'express'
import * as ptsService from '../services/ptsService.js'

const router = express.Router()

/**
 * POST /api/pts/search
 * Tarih aralığında paket listesi sorgula
 */
router.post('/search', async (req, res) => {
  try {
    const { startDate, endDate } = req.body

    if (!startDate || !endDate) {
      return res.status(400).json({
        success: false,
        message: 'Başlangıç ve bitiş tarihi gerekli'
      })
    }

    const result = await ptsService.searchPackages(startDate, endDate)
    res.json(result)

  } catch (error) {
    console.error('PTS search route error:', error)
    res.status(500).json({
      success: false,
      message: 'Sunucu hatası',
      error: error.message
    })
  }
})

/**
 * GET /api/pts/query/:transferId
 * Transfer ID ile paket detayı sorgula
 */
router.get('/query/:transferId', async (req, res) => {
  try {
    const { transferId } = req.params

    if (!transferId) {
      return res.status(400).json({
        success: false,
        message: 'Transfer ID gerekli'
      })
    }

    const result = await ptsService.queryPackage(transferId)
    res.json(result)

  } catch (error) {
    console.error('PTS query route error:', error)
    res.status(500).json({
      success: false,
      message: 'Sunucu hatası',
      error: error.message
    })
  }
})

/**
 * POST /api/pts/download
 * Transfer ID ile paket indir ve parse et
 */
router.post('/download', async (req, res) => {
  try {
    const { transferId } = req.body

    if (!transferId) {
      return res.status(400).json({
        success: false,
        message: 'Transfer ID gerekli'
      })
    }

    const result = await ptsService.downloadPackage(transferId)
    res.json(result)

  } catch (error) {
    console.error('PTS download route error:', error)
    res.status(500).json({
      success: false,
      message: 'Sunucu hatası',
      error: error.message
    })
  }
})

/**
 * GET /api/pts/config
 * PTS konfigürasyon bilgileri (güvenlik için password hariç)
 */
router.get('/config', async (req, res) => {
  try {
    res.json({
      success: true,
      data: {
        glnNo: ptsService.PTS_CONFIG.glnNo,
        username: ptsService.PTS_CONFIG.username,
        baseUrl: ptsService.PTS_CONFIG.baseUrl
      }
    })
  } catch (error) {
    console.error('PTS config route error:', error)
    res.status(500).json({
      success: false,
      message: 'Sunucu hatası'
    })
  }
})

export default router

