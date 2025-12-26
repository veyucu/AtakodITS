import { useState, useRef, useEffect } from 'react'
import { XCircle } from 'lucide-react'
import apiService from '../../services/apiService'

/**
 * Toplu ITS Karekod Okutma Modal Componenti
 */
const BulkScanModal = ({
  isOpen,
  onClose,
  documentId,
  documentNo,
  docType,
  onSuccess,
  playSuccessSound,
  playErrorSound
}) => {
  const textareaRef = useRef(null)
  const lineNumbersRef = useRef(null)
  const [barcodeText, setBarcodeText] = useState('')
  const [loading, setLoading] = useState(false)
  const [results, setResults] = useState(null)

  // Modal açıldığında textarea'ya focus
  useEffect(() => {
    if (isOpen && textareaRef.current) {
      setTimeout(() => textareaRef.current?.focus(), 100)
    }
  }, [isOpen])

  // Satır numaralarını senkronize et
  useEffect(() => {
    if (lineNumbersRef.current && textareaRef.current) {
      const lineCount = barcodeText.split('\n').length
      const numbers = Array.from({ length: Math.max(lineCount, 10) }, (_, i) => i + 1).join('\n')
      lineNumbersRef.current.textContent = numbers
    }
  }, [barcodeText])

  // Scroll senkronizasyonu
  const handleScroll = () => {
    if (lineNumbersRef.current && textareaRef.current) {
      lineNumbersRef.current.scrollTop = textareaRef.current.scrollTop
    }
  }

  // Toplu okutma işlemi
  const handleBulkScan = async () => {
    const lines = barcodeText.split('\n').filter(line => line.trim())

    if (lines.length === 0) {
      alert('⚠️ Lütfen en az bir karekod girin')
      return
    }

    setLoading(true)
    setResults(null)

    try {
      const response = await apiService.bulkScanITSBarcodes({
        documentId: documentId,
        barcodes: lines,
        belgeNo: documentNo,
        docType: docType
      })

      if (response.success) {
        setResults(response.data)

        if (response.data.successCount > 0) {
          playSuccessSound?.()
        }
        if (response.data.errorCount > 0) {
          playErrorSound?.()
        }

        // Başarılı işlem sonrası callback
        if (response.data.successCount > 0) {
          onSuccess?.()
        }
      } else {
        alert('❌ ' + response.message)
        playErrorSound?.()
      }
    } catch (error) {
      console.error('Toplu okutma hatası:', error)
      alert('❌ Toplu okutma sırasında hata oluştu')
      playErrorSound?.()
    } finally {
      setLoading(false)
    }
  }

  // Temizle
  const handleClear = () => {
    setBarcodeText('')
    setResults(null)
    textareaRef.current?.focus()
  }

  // Modal kapat
  const handleClose = () => {
    setBarcodeText('')
    setResults(null)
    onClose()
  }

  if (!isOpen) return null

  return (
    <div
      className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      onClick={handleClose}
      onKeyDown={(e) => e.key === 'Escape' && handleClose()}
    >
      <div
        className="bg-white rounded-xl shadow-2xl w-[90%] max-w-3xl max-h-[80vh] overflow-hidden"
        onClick={(e) => e.stopPropagation()}
      >
        {/* Modal Header */}
        <div className="bg-gradient-to-r from-indigo-500 to-indigo-600 px-6 py-4 text-white">
          <div className="flex items-center justify-between">
            <div>
              <h2 className="text-xl font-bold">📋 Toplu ITS Karekod Okutma</h2>
              <p className="text-sm text-indigo-100">Her satıra bir karekod yapıştırın</p>
            </div>
            <button
              onClick={handleClose}
              className="w-8 h-8 flex items-center justify-center rounded-lg hover:bg-white/20 transition-colors"
            >
              <XCircle className="w-5 h-5" />
            </button>
          </div>
        </div>

        {/* Modal Body */}
        <div className="p-6">
          {/* Textarea with Line Numbers */}
          <div className="flex border border-gray-300 rounded-lg overflow-hidden mb-4" style={{ height: '300px' }}>
            {/* Line Numbers */}
            <pre
              ref={lineNumbersRef}
              className="bg-gray-100 text-gray-500 text-right px-3 py-2 font-mono text-sm overflow-hidden select-none"
              style={{ minWidth: '50px', lineHeight: '1.5' }}
            >
              {Array.from({ length: 10 }, (_, i) => i + 1).join('\n')}
            </pre>
            {/* Textarea */}
            <textarea
              ref={textareaRef}
              value={barcodeText}
              onChange={(e) => setBarcodeText(e.target.value)}
              onScroll={handleScroll}
              placeholder="Her satıra bir ITS karekod yapıştırın...&#10;&#10;Örnek:&#10;01086992937002582110020832004322217280831102509178&#10;01086992937002582110020832004322217280831102509179"
              className="flex-1 p-2 font-mono text-sm resize-none focus:outline-none"
              style={{ lineHeight: '1.5' }}
              disabled={loading}
            />
          </div>

          {/* Results */}
          {results && (
            <div className="mb-4 p-4 rounded-lg bg-gray-50 border border-gray-200">
              <h3 className="font-bold mb-2">📊 Sonuçlar</h3>
              <div className="grid grid-cols-3 gap-4 text-center">
                <div className="p-2 bg-blue-100 rounded">
                  <p className="text-2xl font-bold text-blue-600">{results.totalCount}</p>
                  <p className="text-xs text-blue-600">Toplam</p>
                </div>
                <div className="p-2 bg-green-100 rounded">
                  <p className="text-2xl font-bold text-green-600">{results.successCount}</p>
                  <p className="text-xs text-green-600">Başarılı</p>
                </div>
                <div className="p-2 bg-red-100 rounded">
                  <p className="text-2xl font-bold text-red-600">{results.errorCount}</p>
                  <p className="text-xs text-red-600">Hatalı</p>
                </div>
              </div>

              {/* Error Details */}
              {results.errors && results.errors.length > 0 && (
                <div className="mt-4">
                  <h4 className="text-sm font-semibold text-red-600 mb-2">❌ Hatalar:</h4>
                  <div className="max-h-32 overflow-y-auto bg-red-50 rounded p-2">
                    {results.errors.map((error, index) => (
                      <p key={index} className="text-xs text-red-700 font-mono py-0.5">
                        Satır {error.line}: {error.message}
                      </p>
                    ))}
                  </div>
                </div>
              )}
            </div>
          )}

          {/* Action Buttons */}
          <div className="flex items-center gap-3">
            <button
              onClick={handleBulkScan}
              disabled={loading || !barcodeText.trim()}
              className="flex-1 flex items-center justify-center gap-2 px-4 py-3 text-lg font-semibold rounded-lg shadow-lg hover:shadow-xl transition-all bg-indigo-600 text-white hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? (
                <>
                  <div className="animate-spin w-5 h-5 border-2 border-white border-t-transparent rounded-full" />
                  İşleniyor...
                </>
              ) : (
                <>📤 Toplu Okut ({barcodeText.split('\n').filter(l => l.trim()).length} satır)</>
              )}
            </button>
            <button
              onClick={handleClear}
              disabled={loading}
              className="px-4 py-3 text-lg font-semibold rounded-lg shadow-lg hover:shadow-xl transition-all bg-gray-200 text-gray-700 hover:bg-gray-300 disabled:opacity-50"
            >
              🗑️ Temizle
            </button>
          </div>
        </div>
      </div>
    </div>
  )
}

export default BulkScanModal


