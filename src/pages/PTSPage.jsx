import { useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import { Package, Home, Truck, Search } from 'lucide-react'
import apiService from '../services/apiService'

const PTSPage = () => {
  const navigate = useNavigate()
  const barcodeInputRef = useRef(null)
  
  const [barcode, setBarcode] = useState('')
  const [packages, setPackages] = useState([])
  const [message, setMessage] = useState(null)
  const [loading, setLoading] = useState(false)
  const [startDate, setStartDate] = useState(new Date().toISOString().split('T')[0])
  const [endDate, setEndDate] = useState(new Date().toISOString().split('T')[0])

  // Mesaj göster
  const showMessage = (text, type = 'info') => {
    setMessage({ text, type })
    setTimeout(() => setMessage(null), 5000)
  }

  // Transfer ID ile paket sorgula
  const handleBarcodeScan = async (e) => {
    e.preventDefault()
    
    if (!barcode || barcode.length < 5) {
      showMessage('❌ Geçersiz Transfer ID!', 'error')
      return
    }

    try {
      setLoading(true)
      showMessage('🔍 Paket sorgulanıyor...', 'info')
      
      // PTS'den paket bilgisini sorgula ve indir
      const response = await apiService.queryPackage(barcode)
      
      if (response.success && response.data) {
        // Paketi listeye ekle
        const packageData = response.data
        const newPackage = {
          id: Date.now(),
          transferId: barcode,
          timestamp: new Date().toLocaleString('tr-TR'),
          documentNumber: packageData.documentNumber || '',
          documentDate: packageData.documentDate || '',
          sourceGLN: packageData.sourceGLN || '',
          destinationGLN: packageData.destinationGLN || '',
          productCount: packageData.products?.length || 0,
          products: packageData.products || []
        }
        
        setPackages([newPackage, ...packages])
        setBarcode('')
        showMessage(`✅ Paket bilgisi alındı - ${newPackage.productCount} ürün bulundu`, 'success')
      } else {
        showMessage(`❌ ${response.message || 'Paket sorgulanamadı'}`, 'error')
      }
      
      // Input'a focus geri dön
      barcodeInputRef.current?.focus()
      
    } catch (error) {
      console.error('Paket sorgulama hatası:', error)
      showMessage('❌ Paket sorgulanamadı', 'error')
    } finally {
      setLoading(false)
    }
  }

  // Tarih aralığına göre paket listesi çek
  const handleSearchByDate = async () => {
    if (!startDate || !endDate) {
      showMessage('❌ Başlangıç ve bitiş tarihi seçin', 'error')
      return
    }

    if (new Date(startDate) > new Date(endDate)) {
      showMessage('❌ Başlangıç tarihi bitiş tarihinden büyük olamaz', 'error')
      return
    }

    try {
      setLoading(true)
      showMessage('🔍 Paketler aranıyor...', 'info')
      
      // PTS'den transfer ID listesi al
      const searchResponse = await apiService.searchPackages(startDate, endDate)
      
      if (!searchResponse.success) {
        showMessage(`❌ ${searchResponse.message || 'Paket listesi alınamadı'}`, 'error')
        return
      }

      const transferIds = searchResponse.data || []
      
      if (transferIds.length === 0) {
        showMessage('⚠️ Belirtilen tarih aralığında paket bulunamadı', 'warning')
        return
      }

      showMessage(`📦 ${transferIds.length} paket bulundu, indiriliyor...`, 'info')

      // Her transfer ID için paket detayını indir
      const newPackages = []
      for (let i = 0; i < transferIds.length; i++) {
        try {
          const transferId = transferIds[i]
          showMessage(`📥 Paket indiriliyor: ${i + 1}/${transferIds.length}`, 'info')
          
          const response = await apiService.queryPackage(transferId)
          
          if (response.success && response.data) {
            const packageData = response.data
            newPackages.push({
              id: Date.now() + i,
              transferId: transferId,
              timestamp: new Date().toLocaleString('tr-TR'),
              documentNumber: packageData.documentNumber || '',
              documentDate: packageData.documentDate || '',
              sourceGLN: packageData.sourceGLN || '',
              destinationGLN: packageData.destinationGLN || '',
              productCount: packageData.products?.length || 0,
              products: packageData.products || []
            })
          }
        } catch (error) {
          console.error(`Transfer ${transferIds[i]} indirme hatası:`, error)
        }
      }

      setPackages([...newPackages, ...packages])
      showMessage(`✅ ${newPackages.length} paket başarıyla indirildi`, 'success')
      
    } catch (error) {
      console.error('Tarih aralığı sorgulama hatası:', error)
      showMessage('❌ Paket listesi alınamadı', 'error')
    } finally {
      setLoading(false)
    }
  }

  // Paketi sil
  const handleDeletePackage = (id) => {
    setPackages(packages.filter(p => p.id !== id))
    showMessage('🗑️ Paket silindi', 'info')
  }

  // Tümünü temizle
  const handleClearAll = () => {
    if (packages.length === 0) return
    
    if (confirm('Tüm paketler silinecek. Emin misiniz?')) {
      setPackages([])
      showMessage('🗑️ Tüm paketler temizlendi', 'info')
    }
  }

  // İstatistikler
  const stats = {
    total: packages.length
  }

  return (
    <div className="flex flex-col h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-gradient-to-r from-blue-50 to-cyan-50 border-b border-gray-200">
        <div className="px-6 py-3">
          <div className="flex items-center justify-between gap-4">
            {/* Sol - Başlık */}
            <div className="flex items-center gap-2">
              <button
                onClick={() => navigate('/')}
                className="w-8 h-8 bg-gray-600 rounded flex items-center justify-center hover:bg-gray-700 transition-colors shadow-lg hover:shadow-xl"
                title="Ana Menü"
              >
                <Home className="w-5 h-5 text-white" />
              </button>
              <div className="w-8 h-8 bg-blue-600 rounded flex items-center justify-center">
                <Truck className="w-5 h-5 text-white" />
              </div>
              <h1 className="text-lg font-bold text-gray-900">PTS - Paket Transfer Sistemi</h1>
            </div>

            {/* Orta - İstatistikler */}
            <div className="flex items-center gap-2">
              <div className="bg-gradient-to-br from-blue-500 to-blue-600 rounded px-3 py-1.5 text-white shadow-sm">
                <div className="flex items-center gap-2">
                  <Package className="w-3.5 h-3.5" />
                  <div className="flex items-baseline gap-1">
                    <span className="text-xs font-medium opacity-90">Sorgulanan:</span>
                    <span className="text-base font-bold">{stats.total}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Mesaj Alanı */}
      {message && (
        <div className="px-6 py-2">
          <div className={`p-3 rounded-lg text-sm font-medium ${
            message.type === 'success' ? 'bg-green-100 text-green-800' :
            message.type === 'error' ? 'bg-red-100 text-red-800' :
            message.type === 'warning' ? 'bg-yellow-100 text-yellow-800' :
            'bg-blue-100 text-blue-800'
          }`}>
            {message.text}
          </div>
        </div>
      )}

      {/* Sorgulama Alanı */}
      <div className="px-6 py-4 bg-white border-b border-gray-200">
        {/* Tarih Aralığı Sorgulama */}
        <div className="mb-4 p-4 bg-gray-50 rounded-lg">
          <h3 className="text-sm font-semibold text-gray-700 mb-3">Tarih Aralığına Göre Paket Listesi</h3>
          <div className="flex gap-3 items-end">
            <div className="flex-1">
              <label className="block text-xs font-medium text-gray-600 mb-1">Başlangıç Tarihi</label>
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                disabled={loading}
              />
            </div>
            
            <div className="flex-1">
              <label className="block text-xs font-medium text-gray-600 mb-1">Bitiş Tarihi</label>
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                disabled={loading}
              />
            </div>
            
            <button
              type="button"
              onClick={handleSearchByDate}
              disabled={loading}
              className="px-6 py-2 bg-green-600 text-white rounded-lg font-semibold hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-lg hover:shadow-xl flex items-center gap-2"
            >
              <Search className="w-5 h-5" />
              Paketleri Listele
            </button>
          </div>
        </div>

        {/* Transfer ID ile Tekil Sorgulama */}
        <form onSubmit={handleBarcodeScan} className="flex gap-3 items-center">
          <div className="flex-1">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
              <input
                ref={barcodeInputRef}
                type="text"
                value={barcode}
                onChange={(e) => setBarcode(e.target.value)}
                placeholder="Transfer ID ile tekil sorgulama..."
                className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-lg font-mono"
                disabled={loading}
              />
            </div>
          </div>
          
          <button
            type="submit"
            disabled={loading || !barcode}
            className="px-6 py-3 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-lg hover:shadow-xl flex items-center gap-2"
          >
            <Search className="w-5 h-5" />
            Sorgula
          </button>

          <button
            type="button"
            onClick={handleClearAll}
            disabled={loading || packages.length === 0}
            className="px-6 py-3 bg-red-600 text-white rounded-lg font-semibold hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors shadow-lg hover:shadow-xl"
          >
            Tümünü Temizle
          </button>
        </form>
      </div>

      {/* Paket Listesi */}
      <div className="flex-1 px-6 py-4 overflow-auto">
        {packages.length === 0 ? (
          <div className="flex flex-col items-center justify-center h-full text-gray-400">
            <Package className="w-16 h-16 mb-4" />
            <p className="text-lg font-medium">Henüz paket sorgulanmadı</p>
            <p className="text-sm">Barkod okutarak paket sorgulayın</p>
          </div>
        ) : (
          <div className="space-y-3">
            {packages.map((pkg) => (
              <div
                key={pkg.id}
                className="bg-white rounded-lg border border-gray-200 p-4 hover:shadow-md transition-shadow"
              >
                <div className="flex items-start justify-between">
                  <div className="flex items-start gap-4 flex-1">
                    <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center flex-shrink-0">
                      <Package className="w-7 h-7 text-blue-600" />
                    </div>
                    <div className="flex-1 min-w-0">
                      <div className="flex items-center gap-2 mb-1">
                        <span className="text-xs font-medium text-gray-500">Transfer ID:</span>
                        <p className="text-lg font-mono font-bold text-blue-600">{pkg.transferId}</p>
                      </div>
                      <p className="text-xs text-gray-500 mb-2">{pkg.timestamp}</p>
                      
                      {/* PTS'den gelen bilgiler */}
                      <div className="mt-2 space-y-1 text-sm">
                        {pkg.documentNumber && (
                          <div className="flex items-center gap-2">
                            <span className="text-xs font-medium text-gray-500">Belge No:</span>
                            <span className="text-sm text-gray-700">{pkg.documentNumber}</span>
                          </div>
                        )}
                        {pkg.documentDate && (
                          <div className="flex items-center gap-2">
                            <span className="text-xs font-medium text-gray-500">Belge Tarihi:</span>
                            <span className="text-sm text-gray-700">{pkg.documentDate}</span>
                          </div>
                        )}
                        {pkg.sourceGLN && (
                          <div className="flex items-center gap-2">
                            <span className="text-xs font-medium text-gray-500">Kaynak GLN:</span>
                            <span className="text-sm text-gray-700 font-mono">{pkg.sourceGLN}</span>
                          </div>
                        )}
                        <div className="flex items-center gap-2">
                          <span className="text-xs font-medium text-gray-500">Ürün Sayısı:</span>
                          <span className="text-sm font-bold text-green-600">{pkg.productCount}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                  
                  <button
                    onClick={() => handleDeletePackage(pkg.id)}
                    className="px-3 py-1.5 text-sm text-red-600 hover:bg-red-50 rounded transition-colors flex-shrink-0"
                  >
                    Sil
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

export default PTSPage

