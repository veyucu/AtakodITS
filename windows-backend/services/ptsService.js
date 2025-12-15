import axios from 'axios'
import AdmZip from 'adm-zip'
import xml2js from 'xml2js'

// PTS Web Servis Bilgileri (NetProITS'den alındı)
const PTS_CONFIG = {
  username: '86800010845240000',
  password: '1981aa',
  glnNo: '8680001084524',
  baseUrl: process.env.PTS_BASE_URL || 'https://itsws.saglik.gov.tr', // ITS Production URL
  tokenUrl: '/token/app/token',
  searchUrl: '/pts/app/search',
  getPackageUrl: '/pts/app/GetPackage'
}


/**
 * PTS Token Alma
 * @returns {Promise<string|null>}
 */
async function getAccessToken() {
  try {
    const response = await axios.post(
      `${PTS_CONFIG.baseUrl}${PTS_CONFIG.tokenUrl}`,
      {
        username: PTS_CONFIG.username,
        password: PTS_CONFIG.password
      },
      {
        headers: {
          'Content-Type': 'application/json'
        },
        timeout: 10000
      }
    )

    return response.data?.token || null
  } catch (error) {
    console.error('❌ Token alma hatası:', error.message)
    return null
  }
}

/**
 * PTS'den paket listesi sorgula (tarih aralığında)
 * @param {Date} startDate - Başlangıç tarihi
 * @param {Date} endDate - Bitiş tarihi
 * @returns {Promise<Object>}
 */
async function searchPackages(startDate, endDate) {
  try {
    const token = await getAccessToken()
    if (!token) {
      return {
        success: false,
        message: 'Token alınamadı'
      }
    }

    const formatDate = (date) => {
      const d = new Date(date)
      return d.toISOString().split('T')[0] // YYYY-MM-DD
    }

    const response = await axios.post(
      `${PTS_CONFIG.baseUrl}${PTS_CONFIG.searchUrl}`,
      {
        sourceGln: '',
        destinationGln: PTS_CONFIG.glnNo,
        bringNotReceivedTransferInfo: 0,
        startDate: formatDate(startDate),
        endDate: formatDate(endDate)
      },
      {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        timeout: 30000
      }
    )

    const transferIds = response.data?.transferDetails?.map(t => t.transferId) || []
    
    return {
      success: true,
      data: transferIds,
      message: `${transferIds.length} paket bulundu`
    }

  } catch (error) {
    console.error('❌ Paket sorgulama hatası:', error.message)
    return {
      success: false,
      message: error.response?.data?.message || error.message || 'Paket sorgulanamadı'
    }
  }
}

/**
 * PTS'den paket indir (Transfer ID ile)
 * @param {string} transferId - Transfer ID
 * @returns {Promise<Object>}
 */
async function downloadPackage(transferId) {
  try {
    console.log(`📥 Paket indiriliyor: ${transferId}`)

    const token = await getAccessToken()
    if (!token) {
      return {
        success: false,
        message: 'Token alınamadı'
      }
    }

    const response = await axios.post(
      `${PTS_CONFIG.baseUrl}${PTS_CONFIG.getPackageUrl}`,
      {
        transferId: transferId
      },
      {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        timeout: 30000
      }
    )

    const base64Data = response.data?.fileStream
    if (!base64Data) {
      return {
        success: false,
        message: 'Paket verisi alınamadı'
      }
    }

    // Base64'ten ZIP'e çevir
    const zipBuffer = Buffer.from(base64Data, 'base64')
    
    // ZIP'i aç
    const zip = new AdmZip(zipBuffer)
    const zipEntries = zip.getEntries()
    
    if (zipEntries.length === 0) {
      return {
        success: false,
        message: 'ZIP dosyası boş'
      }
    }

    // İlk XML dosyasını al
    const xmlContent = zipEntries[0].getData().toString('utf8')
    
    // XML'i parse et
    const parser = new xml2js.Parser()
    const xmlData = await parser.parseStringPromise(xmlContent)

    // XML'den bilgileri çıkar
    const root = xmlData.shipmentNotification || xmlData
    const packageInfo = {
      transferId,
      documentNumber: root.documentNumber?.[0] || '',
      documentDate: root.documentDate?.[0] || '',
      sourceGLN: root.sourceGLN?.[0] || '',
      destinationGLN: root.destinationGLN?.[0] || '',
      products: []
    }

    // Carrier ve productList'i parse et
    if (root.carrier) {
      for (const carrier of root.carrier) {
        const carrierLabel = carrier.$.carrierLabel || ''
        
        if (carrier.productList) {
          for (const product of carrier.productList) {
            const gtin = product.$.GTIN || ''
            const expirationDate = product.$.expirationDate || product.$.ExpirationDate || ''
            const lotNumber = product.$.lotNumber || product.$.LotNumber || ''
            
            // Her serial number için ürün ekle
            const serialNumbers = product._ ? [product._] : (Array.isArray(product) ? product : [])
            
            for (const sn of serialNumbers) {
              packageInfo.products.push({
                carrierLabel,
                gtin,
                expirationDate,
                lotNumber,
                serialNumber: sn
              })
            }
          }
        }
      }
    }

    return {
      success: true,
      data: packageInfo,
      message: `${packageInfo.products.length} ürün bulundu`
    }

  } catch (error) {
    console.error('❌ Paket indirme hatası:', error.message)
    return {
      success: false,
      message: error.response?.data?.message || error.message || 'Paket indirilemedi'
    }
  }
}

/**
 * Transfer ID ile paket detayı sorgula
 * @param {string} transferId - Transfer ID
 * @returns {Promise<Object>}
 */
async function queryPackage(transferId) {
  try {
    console.log(`🔍 Paket sorgulanıyor: ${transferId}`)
    
    // Paketi indir ve detaylarını döndür
    return await downloadPackage(transferId)

  } catch (error) {
    console.error('❌ Paket sorgulama hatası:', error)
    
    return {
      success: false,
      message: 'Paket sorgulanamadı',
      error: error.message
    }
  }
}

export {
  getAccessToken,
  searchPackages,
  downloadPackage,
  queryPackage,
  PTS_CONFIG
}

