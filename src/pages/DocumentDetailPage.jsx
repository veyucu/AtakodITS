import { useState, useMemo, useEffect, useRef, useCallback } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { AgGridReact } from 'ag-grid-react'
import 'ag-grid-community/styles/ag-grid.css'
import 'ag-grid-community/styles/ag-theme-alpine.css'
import { 
  ArrowLeft, Package, CheckCircle, XCircle, Barcode, 
  AlertTriangle, User, MapPin, Calendar, Hash, FileText
} from 'lucide-react'
import apiService from '../services/apiService'

const DocumentDetailPage = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const barcodeInputRef = useRef(null)
  const utsGridRef = useRef(null)
  
  const [order, setOrder] = useState(null)
  const [items, setItems] = useState([])
  const [barcodeInput, setBarcodeInput] = useState('')
  const [message, setMessage] = useState(null)
  const [stats, setStats] = useState({ total: 0, prepared: 0, remaining: 0 })
  const [loading, setLoading] = useState(true)
  const [showITSModal, setShowITSModal] = useState(false)
  const [selectedItem, setSelectedItem] = useState(null)
  const [itsRecords, setItsRecords] = useState([])
  const [selectedRecords, setSelectedRecords] = useState([])
  const [itsLoading, setItsLoading] = useState(false)
  const [deleteMode, setDeleteMode] = useState(false) // Silme modu
  const [koliMode, setKoliMode] = useState(false) // Koli barkodu modu
  const [itsModalView, setItsModalView] = useState('grid') // 'grid' veya 'text'
  
  // UTS Popup State'leri
  
  // UTS Modal State'leri (Grid görünümü için)
  const [showUTSModal, setShowUTSModal] = useState(false)
  const [selectedUTSItem, setSelectedUTSItem] = useState(null)
  const [utsRecords, setUtsRecords] = useState([])
  const [originalUtsRecords, setOriginalUtsRecords] = useState([]) // DB'den gelen orijinal kayıtlar
  const [selectedUTSRecords, setSelectedUTSRecords] = useState([])
  const [utsLoading, setUtsLoading] = useState(false)
  const [utsModalMessage, setUtsModalMessage] = useState(null) // Modal içi mesajlar için
  const [utsHasChanges, setUtsHasChanges] = useState(false) // Grid'de değişiklik var mı?

  // Toplu Okutma Modal State'leri
  const [showBulkScanModal, setShowBulkScanModal] = useState(false)
  const [bulkBarcodeText, setBulkBarcodeText] = useState('')
  const [bulkScanLoading, setBulkScanLoading] = useState(false)
  const [bulkScanResults, setBulkScanResults] = useState(null)
  const bulkTextareaRef = useRef(null)
  const bulkLineNumbersRef = useRef(null)

  // Belge tipini belirle
  const getDocumentTypeName = (docType, tipi) => {
    // docType: FTIRSIP değeri ('1', '2', '6')
    // tipi: TIPI değeri (Alış/Satış bilgisi)
    if (docType === '6') {
      return 'Sipariş'
    } else if (docType === '1' || docType === '2') {
      // TIPI alanına göre Alış veya Satış faturası
      const tipiStr = tipi ? String(tipi).toLowerCase() : ''
      if (tipiStr.includes('aliş') || tipiStr.includes('alis')) {
        return 'Alış Faturası'
      } else if (tipiStr.includes('satiş') || tipiStr.includes('satis')) {
        return 'Satış Faturası'
      }
      // Eğer TIPI bilgisi yoksa, FTIRSIP'e göre varsayılan
      // FTIRSIP: '1' = Satış Faturası, '2' = Alış Faturası
      return docType === '1' ? 'Satış Faturası' : 'Alış Faturası'
    }
    return 'Belge'
  }

  // Update statistics
  const updateStats = useCallback((currentItems) => {
    const total = currentItems.length
    const prepared = currentItems.filter(item => item.isPrepared).length
    const remaining = total - prepared
    setStats({ total, prepared, remaining })
  }, [])

  // Fetch document function - reusable
  const fetchDocument = useCallback(async () => {
    try {
      setLoading(true)
      console.log('Fetching document with ID:', id)
      const response = await apiService.getDocumentById(id)
      console.log('API Response:', response)
      
      if (response.success && response.data) {
        const doc = response.data
        console.log('Document data:', doc)
        setOrder(doc)
        setItems(doc.items || [])
        updateStats(doc.items || [])
      } else {
        console.error('API response unsuccessful or no data:', response)
      }
    } catch (error) {
      console.error('Belge yükleme hatası:', error)
    } finally {
      setLoading(false)
    }
  }, [id, updateStats])

  // Load order and items from API
  useEffect(() => {
    fetchDocument()
  }, [fetchDocument])

  // Auto focus barcode input - sayfa yüklendiğinde ve her state değiştiğinde
  useEffect(() => {
    const timer = setTimeout(() => {
      if (barcodeInputRef.current) {
        barcodeInputRef.current.focus()
      }
    }, 100)
    return () => clearTimeout(timer)
  }, [items, message])

  // Otomatik barkod okutma: Herhangi bir tuşa basıldığında barkod input'una focus et
  useEffect(() => {
    const handleKeyDown = (e) => {
      // Modal açıksa iptal et
      if (showITSModal || showUTSModal) return
      
      // Input/textarea aktifse iptal et (zaten bir yerde yazıyoruz)
      const activeElement = document.activeElement
      if (activeElement.tagName === 'INPUT' || 
          activeElement.tagName === 'TEXTAREA' ||
          activeElement.isContentEditable) {
        return
      }
      
      // Özel tuşlar için iptal et (Ctrl, Alt, F1-F12, Arrow keys, vb.)
      if (e.ctrlKey || e.altKey || e.metaKey || 
          e.key === 'Escape' || e.key === 'Tab' || 
          e.key.startsWith('F') || e.key.startsWith('Arrow')) {
        return
      }
      
      // Barkod input'una focus et (karakter girişi yapılacak)
      if (barcodeInputRef.current && !barcodeInputRef.current.contains(activeElement)) {
        barcodeInputRef.current.focus()
        // Tuşu barkod input'una iletmek için event'i yeniden tetiklemiyoruz,
        // tarayıcı otomatik olarak focused element'e yazacak
      }
    }
    
    window.addEventListener('keydown', handleKeyDown)
    return () => window.removeEventListener('keydown', handleKeyDown)
  }, [showITSModal, showUTSModal])

  // ITS Modal ESC tuşu desteği
  useEffect(() => {
    if (!showITSModal) return

    const handleEscape = (e) => {
      if (e.key === 'Escape') {
        handleCloseITSModal()
      }
    }

    window.addEventListener('keydown', handleEscape)
    return () => window.removeEventListener('keydown', handleEscape)
  }, [showITSModal])

  // Calculate totals for footer
  const totals = useMemo(() => {
    const totalQuantity = items.reduce((sum, item) => sum + (item.quantity || 0), 0)
    const totalOkutulan = items.reduce((sum, item) => sum + (item.okutulan || 0), 0)
    const totalKalan = totalQuantity - totalOkutulan
    
    return {
      rowNumber: items.length,
      turu: null, // Footer'da türü boş olacak
      barcode: '',
      productName: 'Toplam',
      quantity: totalQuantity,
      okutulan: totalOkutulan,
      kalan: totalKalan
    }
  }, [items])

  // Column Definitions
  const columnDefs = useMemo(() => [
    {
      headerName: '#',
      valueGetter: (params) => {
        // Footer için toplam satır sayısını göster
        if (params.node.rowPinned === 'bottom') {
          return items.length
        }
        return params.node.rowIndex + 1
      },
      width: 60,
      cellClass: 'text-center font-semibold text-gray-600',
      pinned: 'left',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { borderLeft: 'none', backgroundColor: '#f9fafb' }
        }
        return { borderLeft: 'none' }
      },
      cellClassRules: {
        'font-bold text-gray-900': (params) => params.node.rowPinned === 'bottom'
      }
    },
    {
      headerName: 'Türü',
      field: 'turu',
      width: 90,
      cellClass: 'text-center',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      },
      cellRenderer: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return '' // Footer'da türü boş
        }
        if (params.value === 'ITS') {
          return <span className="px-2 py-0.5 rounded text-xs font-semibold bg-blue-100 text-blue-700">ITS</span>
        }
        if (params.value === 'UTS') {
          return <span className="px-2 py-0.5 rounded text-xs font-semibold bg-red-100 text-red-700">UTS</span>
        }
        return <span className="px-2 py-0.5 rounded text-xs font-semibold bg-gray-100 text-gray-700">{params.value}</span>
      }
    },
    {
      headerName: 'Stok Kodu',
      field: 'barcode',
      width: 150,
      cellClass: 'font-mono',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      }
    },
    {
      headerName: 'Ürün Adı',
      field: 'productName',
      flex: 1,
      minWidth: 300,
      cellRenderer: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return (
            <div style={{ 
              width: '100%', 
              textAlign: 'right', 
              fontWeight: 'bold', 
              paddingRight: '12px',
              display: 'flex',
              justifyContent: 'flex-end',
              alignItems: 'center'
            }}>
              {params.value}
            </div>
          )
        }
        return <div style={{ fontWeight: 'bold' }}>{params.value}</div>
      },
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      }
    },
    {
      headerName: 'Miktar',
      field: 'quantity',
      width: 110,
      cellClass: 'text-center',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      },
      cellRenderer: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return (
            <span className="px-3 py-1 rounded text-sm font-bold bg-gray-100 text-gray-800">
              {params.value}
            </span>
          )
        }
        return (
          <span className="px-3 py-1 rounded text-sm font-bold bg-gray-100 text-gray-800">
            {params.value}
          </span>
        )
      }
    },
    {
      headerName: 'Okutulan',
      field: 'okutulan',
      width: 110,
      cellClass: 'text-center',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      },
      cellRenderer: (params) => {
        if (params.node.rowPinned === 'bottom') {
          const val = params.value || 0
          if (val > 0) {
            return (
              <span className="px-3 py-1 rounded text-sm font-bold bg-green-100 text-green-700">
                {val}
              </span>
            )
          }
          return (
            <span className="px-3 py-1 rounded text-sm font-bold bg-gray-100 text-gray-400">
              {val}
            </span>
          )
        }
        const okutulan = params.value || 0
        const item = params.data
        
        // ITS ürünleri için tıklanabilir badge (0'dan büyükse)
        if (item.turu === 'ITS' && okutulan > 0) {
          return (
            <button
              onClick={() => handleOpenITSModal(item)}
              className="px-3 py-1 rounded text-sm font-bold bg-green-100 text-green-700 hover:bg-green-200 transition-colors cursor-pointer"
              title="ITS karekod detaylarını görüntüle"
            >
              {okutulan} 🔍
            </button>
          )
        }
        
        // UTS ürünleri için tıklanabilir badge (0 da olsa tıklanabilir!)
        if (item.turu === 'UTS') {
          return (
            <button
              onClick={() => handleOpenUTSModal(item)}
              className={`px-3 py-1 rounded text-sm font-bold transition-colors cursor-pointer ${
                okutulan > 0 
                  ? 'bg-red-100 text-red-700 hover:bg-red-200' 
                  : 'bg-gray-100 text-gray-500 hover:bg-gray-200'
              }`}
              title="UTS kayıtlarını görüntüle / Manuel kayıt ekle"
            >
              {okutulan} {okutulan > 0 ? '🔍' : '➕'}
            </button>
          )
        }
        
        // Diğer ürünler için normal badge
        if (okutulan > 0) {
          return (
            <span className="px-3 py-1 rounded text-sm font-bold bg-green-100 text-green-700">
              {okutulan}
            </span>
          )
        }
        return (
          <span className="px-3 py-1 rounded text-sm font-bold bg-gray-100 text-gray-400">
            {okutulan}
          </span>
        )
      }
    },
    {
      headerName: 'Kalan',
      field: 'kalan',
      width: 110,
      valueGetter: (params) => {
        if (params.node.rowPinned === 'bottom') return params.data.kalan
        return (params.data.quantity || 0) - (params.data.okutulan || 0)
      },
      cellClass: 'text-center',
      cellStyle: (params) => {
        if (params.node.rowPinned === 'bottom') {
          return { backgroundColor: '#f9fafb' }
        }
        return {}
      },
      cellRenderer: (params) => {
        if (params.node.rowPinned === 'bottom') {
          const val = params.value || 0
          if (val > 0) {
            return (
              <span className="px-3 py-1 rounded text-sm font-bold bg-orange-100 text-orange-700">
                {val}
              </span>
            )
          }
          return (
            <span className="px-3 py-1 rounded text-sm font-bold bg-green-100 text-green-700">
              ✓
            </span>
          )
        }
        const kalan = params.value || 0
        if (kalan > 0) {
          return (
            <span className="px-3 py-1 rounded text-sm font-bold bg-orange-100 text-orange-700">
              {kalan}
            </span>
          )
        }
        return (
          <span className="px-3 py-1 rounded text-sm font-bold bg-green-100 text-green-700">
            ✓
          </span>
        )
      }
    }
  ], [items])

  const defaultColDef = useMemo(() => ({
    sortable: true,
    resizable: true,
    filter: false
  }), [])

  // UTS Modal Grid Column Definitions - EDITABLE
  const utsModalColumnDefs = useMemo(() => [
    {
      headerName: '',
      checkboxSelection: true,
      headerCheckboxSelection: true,
      width: 50,
      pinned: 'left',
      suppressMenu: true,
      editable: false,
      cellStyle: { display: 'flex', alignItems: 'center', justifyContent: 'center' },
      headerClass: 'ag-header-cell-center'
    },
    {
      headerName: '#',
      valueGetter: 'node.rowIndex + 1',
      width: 60,
      cellClass: 'text-center font-semibold text-gray-600',
      editable: false
    },
    {
      headerName: 'Seri No',
      field: 'seriNo',
      flex: 1,
      minWidth: 150,
      cellClass: 'font-mono font-bold text-red-600',
      editable: true,
      valueSetter: (params) => {
        const newValue = params.newValue ? params.newValue.trim() : ''
        params.data.seriNo = newValue
        
        // Seri No girildiğinde miktar otomatik 1 olmalı
        if (newValue) {
          params.data.miktar = 1
        }
        
        // Grid'i güncelle
        return true
      }
    },
    {
      headerName: 'Lot No',
      field: 'lot',
      width: 150,
      cellClass: 'font-mono',
      editable: true
    },
    {
      headerName: 'Üretim Tarihi',
      field: 'uretimTarihi',
      width: 150,
      cellClass: 'text-center font-semibold',
      editable: true,
      cellEditor: 'agDateStringCellEditor',
      cellEditorParams: {
        min: '2000-01-01',
        max: '2099-12-31'
      },
      valueGetter: (params) => {
        // Grid'e YYYY-MM-DD formatında göster (edit için)
        const data = params.data
        if (!data) return ''
        
        // Eğer uretimTarihiDisplay varsa onu kullan
        if (data.uretimTarihiDisplay) {
          return data.uretimTarihiDisplay
        }
        
        // YYMMDD formatını YYYY-MM-DD'ye çevir
        if (data.uretimTarihi && data.uretimTarihi.length === 6) {
          const yy = data.uretimTarihi.substring(0, 2)
          const mm = data.uretimTarihi.substring(2, 4)
          const dd = data.uretimTarihi.substring(4, 6)
          const yyyy = parseInt(yy) > 50 ? `19${yy}` : `20${yy}`
          return `${yyyy}-${mm}-${dd}`
        }
        return ''
      },
      valueSetter: (params) => {
        // Edit sonrası değeri kaydet
        const newValue = params.newValue
        if (!newValue) return false
        
        params.data.uretimTarihiDisplay = newValue
        
        // YYYY-MM-DD -> YYMMDD formatına çevir
        if (newValue.includes('-')) {
          const [yyyy, mm, dd] = newValue.split('-')
          const yy = yyyy.substring(2, 4)
          params.data.uretimTarihi = `${yy}${mm}${dd}`
        }
        return true
      },
      valueFormatter: (params) => {
        // Görüntüleme için DD.MM.YYYY formatı
        if (!params.value) return ''
        
        if (params.value.includes('-')) {
          const [yyyy, mm, dd] = params.value.split('-')
          return `${dd}.${mm}.${yyyy}`
        }
        return params.value
      }
    },
    {
      headerName: 'Miktar',
      field: 'miktar',
      width: 120,
      cellClass: (params) => {
        // Seri No varsa disabled görünümü
        if (params.data && params.data.seriNo) {
          return 'text-center font-bold bg-gray-100 text-gray-500'
        }
        return 'text-center font-bold'
      },
      editable: (params) => {
        // Sadece Seri No yoksa miktar düzenlenebilir
        return !params.data.seriNo
      },
      cellEditor: 'agNumberCellEditor',
      cellEditorParams: {
        min: 1,
        max: 9999,
        precision: 0
      },
      valueGetter: (params) => {
        // Seri No varsa miktar her zaman 1
        if (params.data && params.data.seriNo) {
          return 1
        }
        // Seri No yoksa miktar değerini döndür (boş olabilir)
        return params.data ? params.data.miktar : ''
      },
      valueSetter: (params) => {
        // Seri No yoksa miktarı güncelle
        if (!params.data.seriNo) {
          const val = Number(params.newValue)
          params.data.miktar = val > 0 ? val : ''
        } else {
          // Seri No varsa her zaman 1
          params.data.miktar = 1
        }
        return true
      }
    }
  ], [])

  const utsModalDefaultColDef = useMemo(() => ({
    sortable: true,
    resizable: true,
    filter: true
  }), [])

  // ITS Modal Grid Column Definitions
  const itsModalColumnDefs = useMemo(() => [
    {
      headerName: '',
      checkboxSelection: true,
      headerCheckboxSelection: true,
      width: 50,
      pinned: 'left',
      suppressMenu: true,
      cellStyle: { display: 'flex', alignItems: 'center', justifyContent: 'center' },
      headerClass: 'ag-header-cell-center'
    },
    {
      headerName: '#',
      valueGetter: 'node.rowIndex + 1',
      width: 60,
      cellClass: 'text-center font-semibold text-gray-600'
    },
    {
      headerName: 'Barkod',
      field: 'barkod',
      width: 150,
      cellClass: 'font-mono'
    },
    {
      headerName: 'Seri No',
      field: 'seriNo',
      flex: 1,
      minWidth: 250,
      cellClass: 'font-mono font-bold text-primary-600'
    },
    {
      headerName: 'Miad',
      field: 'miad',
      width: 120,
      cellClass: 'text-center font-semibold',
      valueFormatter: (params) => {
        // YYMMDD -> DD.MM.YYYY
        if (!params.value) return ''
        if (params.value.length === 6) {
          const yy = params.value.substring(0, 2)
          const mm = params.value.substring(2, 4)
          const dd = params.value.substring(4, 6)
          const yyyy = parseInt(yy) > 50 ? `19${yy}` : `20${yy}`
          return `${dd}.${mm}.${yyyy}`
        }
        return params.value
      }
    },
    {
      headerName: 'Lot',
      field: 'lot',
      width: 150,
      cellClass: 'font-mono'
    },
    {
      headerName: 'Koli Barkodu',
      field: 'carrierLabel',
      width: 180,
      cellClass: 'font-mono text-blue-600'
    }
  ], [])

  const itsModalDefaultColDef = useMemo(() => ({
    sortable: true,
    resizable: true,
    filter: true
  }), [])

  // Handle Barcode Scan
  const handleBarcodeScan = async (e) => {
    e.preventDefault()
    
    if (!barcodeInput.trim()) {
      showMessage('⚠️ Barkod giriniz', 'warning')
      playWarningSound()
      return
    }

    const scannedBarcode = barcodeInput.trim()
    
    // Hem Sil hem Koli modu aktifse - Koli barkoduna göre sil
    if (deleteMode && koliMode) {
      await handleDeleteCarrierBarcode(scannedBarcode)
      setBarcodeInput('')
      return
    }
    
    // Sadece Koli modu aktifse - Koli barkodunu kaydet
    if (koliMode) {
      await handleCarrierBarcode(scannedBarcode)
      setBarcodeInput('')
      return
    }
    
    // ITS Karekod kontrolü (01 ile başlıyorsa ITS karekodudur)
    const isITSBarcode = scannedBarcode.startsWith('01') && scannedBarcode.length > 30
    
    if (isITSBarcode) {
      // ITS Karekod İşlemi
      if (deleteMode) {
        await handleDeleteITSBarcode(scannedBarcode)
      } else {
        await handleITSBarcode(scannedBarcode)
      }
    } else {
      // Normal barkod işlemi (DGR/UTS)
      if (deleteMode) {
        await handleDeleteDGRBarcode(scannedBarcode)
      } else {
        await handleNormalBarcode(scannedBarcode)
      }
    }
    
    setBarcodeInput('')
    barcodeInputRef.current?.focus()
  }

  // Toplu Okutma - Scroll Senkronizasyonu
  const handleBulkTextareaScroll = () => {
    if (bulkTextareaRef.current && bulkLineNumbersRef.current) {
      bulkLineNumbersRef.current.scrollTop = bulkTextareaRef.current.scrollTop
    }
  }

  // ITS Karekod Parse Fonksiyonu (Backend ile aynı)
  const parseITSBarcode = (barcode) => {
    try {
      let position = 0
      const result = {
        gtin: '',
        serialNumber: '',
        expiryDate: '',
        lotNumber: '',
        raw: barcode
      }

      // Boşluk ve özel karakterleri temizle
      barcode = barcode.trim().replace(/\s+/g, '')

      // 1. GTIN (01) - İlk 2 karakter
      if (!barcode.startsWith('01')) {
        console.error('Geçersiz ITS karekod formatı - 01 ile başlamalı')
        return null
      }
      position += 2

      // 2. GTIN - Sonraki 14 karakter
      const gtinFull = barcode.substring(position, position + 14)
      if (gtinFull.length < 14) {
        console.error('Geçersiz GTIN uzunluğu')
        return null
      }
      result.gtin = gtinFull
      position += 14

      // 3. Serial Number AI (21)
      if (barcode.substring(position, position + 2) !== '21') {
        console.error('Serial Number AI (21) bulunamadı')
        return null
      }
      position += 2

      // 4. Serial Number - 17 AI'sına kadar (tarih kontrolü ile)
      const serialStartPos = position
      const maxSerialLength = 20
      const searchEndPos = Math.min(serialStartPos + maxSerialLength, barcode.length - 8)
      
      let expiryAIPos = -1
      
      for (let i = serialStartPos; i <= searchEndPos; i++) {
        if (barcode.substring(i, i + 2) === '17') {
          // 17'den sonraki 6 karakter var mı ve rakam mı?
          const dateStr = barcode.substring(i + 2, i + 8)
          if (dateStr.length === 6 && /^\d{6}$/.test(dateStr)) {
            // Tarih formatı doğru mu kontrol et (YYMMDD)
            const mm = parseInt(dateStr.substring(2, 4))
            const dd = parseInt(dateStr.substring(4, 6))
            
            // Ay 01-12, gün 01-31 arası olmalı
            if (mm >= 1 && mm <= 12 && dd >= 1 && dd <= 31) {
              // Seri no min 4 karakter olmalı
              if (i - serialStartPos >= 4) {
                expiryAIPos = i
                break
              }
            }
          }
        }
      }
      
      if (expiryAIPos === -1) {
        console.error('Expiry Date AI (17) bulunamadı')
        return null
      }
      
      result.serialNumber = barcode.substring(serialStartPos, expiryAIPos)
      position = expiryAIPos + 2 // 17'yi atla

      // 5. Expiry Date (YYMMDD) - 6 karakter
      result.expiryDate = barcode.substring(position, position + 6)
      if (result.expiryDate.length !== 6) {
        console.error('Geçersiz miad formatı (YYMMDD)')
        return null
      }
      position += 6

      // 6. Lot/Batch AI (10)
      if (barcode.substring(position, position + 2) !== '10') {
        console.error('Lot/Batch AI (10) bulunamadı')
        return null
      }
      position += 2

      // 7. Lot/Batch - String sonuna kadar (lot son alandır)
      result.lotNumber = barcode.substring(position).replace(/\x1D/g, '').trim()

      // Validasyon
      if (!result.gtin || !result.serialNumber || !result.expiryDate || !result.lotNumber) {
        console.error('Eksik karekod bilgisi')
        return null
      }

      console.log('✅ ITS Karekod Parse Başarılı:', result)
      return result

    } catch (error) {
      console.error('ITS karekod parse hatası:', error)
      return null
    }
  }

  // Toplu ITS Karekod Okutma İşlemi
  const handleBulkScan = async () => {
    if (!bulkBarcodeText.trim()) {
      setMessage({ type: 'warning', text: '⚠️ Lütfen karekod girin' })
      return
    }

    setBulkScanLoading(true)
    setBulkScanResults(null)

    // Satırlara ayır ve boş satırları temizle
    const lines = bulkBarcodeText
      .split('\n')
      .map(line => line.trim())
      .filter(line => line.length > 0)

    if (lines.length === 0) {
      setMessage({ type: 'warning', text: '⚠️ Geçerli karekod bulunamadı' })
      setBulkScanLoading(false)
      return
    }

    const results = {
      total: lines.length,
      success: 0,
      failed: 0,
      errors: []
    }

    // Her satır için işlem yap (Sadece ITS Karekod)
    for (let i = 0; i < lines.length; i++) {
      const barcode = lines[i]
      
      try {
        // ITS Karekod kontrolü (Sadece ITS desteklenir)
        const isITSBarcode = barcode.startsWith('01') && barcode.length > 30

        if (!isITSBarcode) {
          throw new Error('Sadece ITS karekod (2D barkod) desteklenir!')
        }

        // ITS işlemi
        await handleITSBarcodeProcess(barcode)
        
        results.success++
      } catch (error) {
        results.failed++
        results.errors.push(`${i + 1}. satır: ${error.message || 'Bilinmeyen hata'}`)
      }
    }

    setBulkScanResults(results)
    setBulkScanLoading(false)

    // Belgeyi yenile
    const response = await apiService.getDocumentById(order.id)
    if (response.success && response.data) {
      setOrder(response.data)
      setItems(response.data.items || [])
      updateStats(response.data.items || [])
    }

    // Başarı/hata mesajı
    if (results.failed === 0) {
      setMessage({ type: 'success', text: `✅ ${results.success} barkod başarıyla işlendi!` })
      playSuccessSound()
      // Modal'ı kapat
      setTimeout(() => {
        setShowBulkScanModal(false)
        setBulkBarcodeText('')
        setBulkScanResults(null)
      }, 2000)
    } else {
      setMessage({ type: 'warning', text: `⚠️ ${results.success} başarılı, ${results.failed} başarısız` })
      playWarningSound()
    }
  }

  // ITS barkod işlemi (toplu okutma için)
  const handleITSBarcodeProcess = async (itsBarcode) => {
    const parsedData = parseITSBarcode(itsBarcode)
    if (!parsedData) {
      throw new Error('Geçersiz ITS karekod formatı')
    }

    const itemIndex = items.findIndex(item => {
      const normalizedGtin = item.barcode?.replace(/^0+/, '')
      const normalizedParsedGtin = parsedData.gtin?.replace(/^0+/, '')
      return normalizedGtin === normalizedParsedGtin || item.stokKodu === parsedData.gtin
    })

    if (itemIndex === -1) {
      throw new Error(`Ürün bulunamadı: ${parsedData.gtin}`)
    }

    const item = items[itemIndex]

    if (item.turu !== 'ITS') {
      throw new Error(`${item.productName} - ITS ürünü değil!`)
    }

    let belgeTarihiFormatted
    if (order.orderDate) {
      const date = new Date(order.orderDate)
      const year = date.getFullYear()
      const month = String(date.getMonth() + 1).padStart(2, '0')
      const day = String(date.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    } else {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      const day = String(today.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    }

    const result = await apiService.saveITSBarcode({
      barcode: itsBarcode,
      documentId: order.id,
      itemId: item.itemId,
      stokKodu: item.stokKodu,
      belgeTip: item.stharHtur,
      gckod: item.stharGckod || '',
      belgeNo: order.orderNo,
      belgeTarihi: belgeTarihiFormatted,
      docType: order.docType,
      expectedQuantity: item.quantity,
      cariKodu: order.customerCode,
      kullanici: JSON.parse(localStorage.getItem('user') || '{}').username || 'USER'
    })

    if (!result.success) {
      // Backend'den gelen detaylı hata mesajını kullan
      const errorMessage = result.message || result.error || 'Kayıt başarısız!'
      throw new Error(errorMessage)
    }
  }

  // Normal barkod işlemi (toplu okutma için)
  const handleNormalBarcodeProcess = async (scannedBarcode) => {
    let quantity = 1
    let actualBarcode = scannedBarcode

    if (scannedBarcode.includes('*')) {
      const parts = scannedBarcode.split('*')
      if (parts.length === 2 && !isNaN(parts[0])) {
        quantity = parseInt(parts[0])
        actualBarcode = parts[1]
      }
    }

    const itemIndex = items.findIndex(item => item.barcode === actualBarcode || item.stokKodu === actualBarcode)

    if (itemIndex === -1) {
      throw new Error(`Ürün bulunamadı: ${actualBarcode}`)
    }

    const item = items[itemIndex]

    if (item.turu === 'ITS') {
      throw new Error(`${item.productName} - ITS ürünüdür! Karekod gerekli!`)
    }

    if (item.turu === 'UTS') {
      throw new Error(`${item.productName} - UTS ürünü için manuel giriş gerekli!`)
    }

    let belgeTarihiFormatted
    if (order.orderDate) {
      const date = new Date(order.orderDate)
      const year = date.getFullYear()
      const month = String(date.getMonth() + 1).padStart(2, '0')
      const day = String(date.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    } else {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      const day = String(today.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    }

    for (let i = 0; i < quantity; i++) {
      const result = await apiService.saveDGRBarcode({
        barcode: actualBarcode,
        documentId: order.id,
        itemId: item.itemId,
        stokKodu: item.stokKodu,
        belgeTip: item.stharHtur,
        gckod: item.stharGckod || '',
        belgeNo: order.orderNo,
        belgeTarihi: belgeTarihiFormatted,
        docType: order.docType,
        expectedQuantity: item.quantity,
        cariKodu: order.customerCode,
        kullanici: JSON.parse(localStorage.getItem('user') || '{}').username || 'USER'
      })

      if (!result.success) {
        // Backend'den gelen detaylı hata mesajını kullan
        const errorMessage = result.message || result.error || 'Kayıt başarısız!'
        throw new Error(errorMessage)
      }
    }
  }

  // Koli Barkodu İşlemi (ITS için)
  const handleCarrierBarcode = async (carrierLabel) => {
    try {
      console.log('📦 Koli barkodu okutuldu:', carrierLabel)
      showMessage('📦 Koli işleniyor...', 'info')
      
      const user = JSON.parse(localStorage.getItem('user') || '{}')
      const username = user.username || 'USER'
      
      const result = await apiService.saveCarrierBarcode({
        carrierLabel,
        docId: id, // Belge KAYITNO
        ftirsip: order.docType,
        cariKodu: order.customerCode,
        kullanici: username
      })
      
      if (result.success) {
        playSuccessSound()
        showMessage(`✅ ${result.message}`, 'success')
        
        // Local state'i güncelle (ekranı yenileme)
        const updatedItems = [...items]
        let hasChanges = false
        
        // Backend'den dönen GTIN'lere göre okutulan miktarlarını artır
        if (result.affectedGtins && result.affectedGtins.length > 0) {
          result.affectedGtins.forEach(gtin => {
            // Her GTIN için kaç adet ürün eklendi?
            const addedCount = result.savedCount ? Math.floor(result.savedCount / result.affectedGtins.length) : 1
            
            // GTIN veya STOK_KODU ile eşleşen item'ı bul
            const itemIndex = updatedItems.findIndex(item => 
              item.gtin === gtin || 
              item.stokKodu === gtin || 
              item.barcode === gtin
            )
            
            if (itemIndex !== -1) {
              updatedItems[itemIndex].okutulan = (updatedItems[itemIndex].okutulan || 0) + addedCount
              updatedItems[itemIndex].isPrepared = updatedItems[itemIndex].okutulan >= updatedItems[itemIndex].quantity
              hasChanges = true
            }
          })
        }
        
        if (hasChanges) {
          setItems(updatedItems)
          updateStats(updatedItems)
        }
      } else {
        playErrorSound()
        showMessage(`❌ ${result.message}`, 'error')
      }
    } catch (error) {
      console.error('❌ Koli barkodu işleme hatası:', error)
      playErrorSound()
      showMessage(`❌ ${error.response?.data?.message || error.message || 'Koli barkodu işlenemedi'}`, 'error')
    }
  }

  // Koli Barkodu Silme İşlemi (ITS için)
  const handleDeleteCarrierBarcode = async (carrierLabel) => {
    try {
      console.log('🗑️ Koli barkodu siliniyor:', carrierLabel)
      showMessage('🗑️ Koli siliniyor...', 'info')
      
      const result = await apiService.deleteCarrierBarcode({
        carrierLabel,
        docId: id // Belge KAYITNO
      })
      
      if (result.success) {
        playSuccessSound()
        showMessage(`✅ ${result.message || `${result.deletedCount} ürün koliden silindi`}`, 'success')
        
        // Local state'i güncelle (ekranı yenileme)
        const updatedItems = [...items]
        let hasChanges = false
        
        // Backend'den dönen GTIN'lere göre okutulan miktarlarını azalt
        if (result.affectedGtins && result.affectedGtins.length > 0) {
          result.affectedGtins.forEach(gtin => {
            // Her GTIN için kaç adet ürün silindi?
            const deletedCount = result.gtinCounts ? result.gtinCounts[gtin] : 0
            
            if (deletedCount > 0) {
              // GTIN veya STOK_KODU ile eşleşen item'ı bul
              const itemIndex = updatedItems.findIndex(item => 
                item.gtin === gtin || 
                item.stokKodu === gtin || 
                item.barcode === gtin
              )
              
              if (itemIndex !== -1) {
                // Okutulan miktarı azalt (negatif olmasın)
                updatedItems[itemIndex].okutulan = Math.max(0, (updatedItems[itemIndex].okutulan || 0) - deletedCount)
                updatedItems[itemIndex].isPrepared = updatedItems[itemIndex].okutulan >= updatedItems[itemIndex].quantity
                hasChanges = true
              }
            }
          })
        }
        
        if (hasChanges) {
          setItems(updatedItems)
          updateStats(updatedItems)
        }
      } else {
        playErrorSound()
        showMessage(`❌ ${result.message}`, 'error')
      }
    } catch (error) {
      console.error('❌ Koli barkodu silme hatası:', error)
      playErrorSound()
      showMessage(`❌ ${error.response?.data?.message || error.message || 'Koli barkodu silinemedi'}`, 'error')
    }
  }

  // Normal Barkod İşlemi (DGR/UTS Ürünleri - ITS DEĞİL!)
  const handleNormalBarcode = async (scannedBarcode) => {
    // Toplu okutma kontrolü: 100*Barkod formatı
    let quantity = 1
    let actualBarcode = scannedBarcode
    
    if (scannedBarcode.includes('*')) {
      const parts = scannedBarcode.split('*')
      if (parts.length === 2 && !isNaN(parts[0])) {
        quantity = parseInt(parts[0])
        actualBarcode = parts[1]
        console.log(`📦 Toplu okutma: ${quantity} adet - Barkod: ${actualBarcode}`)
      }
    }
    
    // Find item by barcode
    const itemIndex = items.findIndex(item => item.barcode === actualBarcode || item.stokKodu === actualBarcode)
    
    if (itemIndex === -1) {
      showMessage(`❌ Bulunamadı: ${actualBarcode}`, 'error')
      playErrorSound()
      return
    }
    
    const item = items[itemIndex]
    
    // ITS ürünü kontrolü - ITS ürünlerinde normal barkod kabul edilmez!
    if (item.turu === 'ITS') {
      showMessage(`❌ ${item.productName} - ITS ürünüdür! Karekod (2D) okutmalısınız!`, 'error')
      playErrorSound()
      return
    }
    
    // UTS ürünü kontrolü - UTS ürünlerinde direkt modal aç!
    if (item.turu === 'UTS') {
      handleOpenUTSModal(item)
      return
    }
    
    // Belge tarihini saat bilgisi olmadan formatla (YYYY-MM-DD) - Local time
    let belgeTarihiFormatted
    if (order.orderDate) {
      const date = new Date(order.orderDate)
      const year = date.getFullYear()
      const month = String(date.getMonth() + 1).padStart(2, '0')
      const day = String(date.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    } else {
      const today = new Date()
      const year = today.getFullYear()
      const month = String(today.getMonth() + 1).padStart(2, '0')
      const day = String(today.getDate()).padStart(2, '0')
      belgeTarihiFormatted = `${year}-${month}-${day}`
    }
    
    // Toplu okutma için döngü
    for (let i = 0; i < quantity; i++) {
      // Backend'e DGR barkod gönder (TBLSERITRA'ya kayıt)
      const result = await apiService.saveDGRBarcode({
        barcode: actualBarcode,
        documentId: order.id,
        itemId: item.itemId,
        stokKodu: item.stokKodu,
        belgeTip: item.stharHtur,     // STHAR_HTUR
        gckod: item.stharGckod || '', // STHAR_GCKOD
        belgeNo: order.orderNo,
        belgeTarihi: belgeTarihiFormatted, // Belge tarihi (saat yok)
        docType: order.docType,
        expectedQuantity: item.quantity, // Miktar kontrolü için
        cariKodu: order.customerCode,    // Belgedeki CARI_KODU
        kullanici: JSON.parse(localStorage.getItem('user') || '{}').username || 'USER' // Sisteme giriş yapan kullanıcı
      })
      
      if (!result.success) {
        // Hata varsa döngüyü kır
        if (result.error === 'QUANTITY_EXCEEDED') {
          console.error('⚠️⚠️⚠️ MİKTAR AŞIMI! Bu üründen daha fazla okutulamaz!')
          console.error('Ürün:', item.productName)
          console.error('Miktar:', item.quantity)
          
          showMessage(`❌ MİKTAR AŞIMI! ${item.productName} - ${result.message}`, 'error')
          playErrorSound()
        } else {
          showMessage(`❌ ${item.productName} - ${result.message}`, 'error')
          playErrorSound()
        }
        break
      }
    }
    
    // Tüm döngü başarılıysa, son güncellemeyi göster
    // Backend'den son durumu al
    const docResponse = await apiService.getDocumentById(order.id)
    if (docResponse.success && docResponse.data) {
      setItems(docResponse.data.items || [])
      updateStats(docResponse.data.items || [])
      
      const updatedItem = docResponse.data.items.find(i => i.itemId === item.itemId)
      if (updatedItem) {
        if (quantity > 1) {
          showMessage(`✅ ${item.productName} - ${quantity} adet eklendi (${updatedItem.okutulan}/${item.quantity})`, 'success')
        } else {
          showMessage(`✅ ${item.productName} (${updatedItem.okutulan}/${item.quantity})`, 'success')
        }
        playSuccessSound()
        
        // Check if all items are prepared
        if (docResponse.data.items.every(item => item.okutulan >= item.quantity)) {
          setTimeout(() => {
            showMessage('🎉 Tüm ürünler tamamlandı!', 'success')
            playSuccessSound()
          }, 1000)
        }
      }
    }
  }

  // ITS Barkod Silme İşlemi
  const handleDeleteITSBarcode = async (itsBarcode) => {
    try {
      console.log('🗑️ ITS Barkod siliniyor:', itsBarcode.substring(0, 50) + '...')
      showMessage('🗑️ Siliniyor...', 'info')
      
      // Karekodu parse et (aynı fonksiyonu kullan!)
      const parsedData = parseITSBarcode(itsBarcode)
      
      if (!parsedData || !parsedData.serialNumber) {
        showMessage(`❌ Seri numarası okunamadı!`, 'error')
        playErrorSound()
        return
      }
      
      console.log('✅ Parse edildi:', parsedData)
      
      // Ürünü bul
      const itemIndex = items.findIndex(item => {
        const normalizedGtin = item.barcode?.replace(/^0+/, '')
        const normalizedParsedGtin = parsedData.gtin?.replace(/^0+/, '')
        return normalizedGtin === normalizedParsedGtin || item.stokKodu === parsedData.gtin || item.barcode === parsedData.gtin.substring(1)
      })
      
      if (itemIndex === -1) {
        showMessage(`❌ Ürün bulunamadı: ${parsedData.gtin}`, 'error')
        playErrorSound()
        return
      }
      
      const item = items[itemIndex]
      
      // Sadece ITS ürünleri için karekod silinebilir
      if (item.turu !== 'ITS') {
        showMessage(`❌ ${item.productName} - ITS ürünü değil!`, 'error')
        playErrorSound()
        return
      }
      
      const seriNo = parsedData.serialNumber
      
      // Backend'e silme isteği gönder
      const result = await apiService.deleteITSBarcodeRecords(
        order.id,
        item.itemId,
        [seriNo]
      )
      
      if (result.success) {
        console.log('✅ ITS Barkod silindi!')
        
        // Grid'i yenile
        const docResponse = await apiService.getDocumentById(order.id)
        if (docResponse.success && docResponse.data) {
          setItems(docResponse.data.items || [])
          updateStats(docResponse.data.items || [])
          
          const updatedItem = docResponse.data.items.find(i => i.itemId === item.itemId)
          if (updatedItem) {
            showMessage(`🗑️ ${item.productName} - Silindi (${updatedItem.okutulan}/${item.quantity})`, 'success')
            playSuccessSound()
          }
        }
      } else {
        showMessage(`❌ ${item.productName} - ${result.message}`, 'error')
        playErrorSound()
      }
      
    } catch (error) {
      console.error('ITS Barkod Silme Hatası:', error)
      showMessage(`❌ Hata: ${error.message}`, 'error')
      playErrorSound()
    }
  }

  // DGR/UTS Barkod Silme İşlemi (ITS DEĞİL!)
  const handleDeleteDGRBarcode = async (scannedBarcode) => {
    try {
      console.log('🗑️ DGR/UTS Barkod siliniyor:', scannedBarcode)
      showMessage('🗑️ Siliniyor...', 'info')
      
      // Ürünü bul
      const itemIndex = items.findIndex(item => item.barcode === scannedBarcode || item.stokKodu === scannedBarcode)
      
      if (itemIndex === -1) {
        showMessage(`❌ Bulunamadı: ${scannedBarcode}`, 'error')
        playErrorSound()
        return
      }
      
      const item = items[itemIndex]
      
      // ITS ürünü kontrolü - ITS ürünlerinde normal barkod ile silme yapılamaz!
      if (item.turu === 'ITS') {
        showMessage(`❌ ${item.productName} - ITS ürünüdür! Silmek için karekod (2D) okutmalısınız!`, 'error')
        playErrorSound()
        return
      }
      
      // Backend'e silme isteği gönder (DGR için seri_no = stok_kodu)
      const result = await apiService.deleteITSBarcodeRecords(
        order.id,
        item.itemId,
        [item.stokKodu]  // DGR için SERI_NO = STOK_KODU
      )
      
      if (result.success) {
        console.log('✅ DGR Barkod silindi!')
        
        // Grid'i yenile
        const docResponse = await apiService.getDocumentById(order.id)
        if (docResponse.success && docResponse.data) {
          setItems(docResponse.data.items || [])
          updateStats(docResponse.data.items || [])
          
          const updatedItem = docResponse.data.items.find(i => i.itemId === item.itemId)
          if (updatedItem) {
            showMessage(`🗑️ ${item.productName} - Silindi (${updatedItem.okutulan}/${item.quantity})`, 'success')
            playSuccessSound()
          }
        }
      } else {
        showMessage(`❌ ${item.productName} - ${result.message}`, 'error')
        playErrorSound()
      }
      
    } catch (error) {
      console.error('DGR Barkod Silme Hatası:', error)
      showMessage(`❌ Hata: ${error.message}`, 'error')
      playErrorSound()
    }
  }

  // ITS Karekod İşlemi
  const handleITSBarcode = async (itsBarcode) => {
    try {
      console.log('🔍 ITS Karekod okutuldu:', itsBarcode.substring(0, 50) + '...')
      showMessage('📱 İşleniyor...', 'info')
      
      // ITS karekoddan barkodu parse et (basit parse - ilk 01'den sonraki 14 karakter)
      const barkodPart = itsBarcode.substring(3, 16) // 13 digit barkod
      console.log('📦 Barkod parse edildi:', barkodPart)
      
      // Ürünü bul
      const itemIndex = items.findIndex(item => item.barcode === barkodPart || item.stokKodu === barkodPart)
      
      if (itemIndex === -1) {
        showMessage(`❌ Ürün bulunamadı: ${barkodPart}`, 'error')
        playErrorSound()
        return
      }
      
      const item = items[itemIndex]
      
      // Sadece ITS ürünleri için karekod okutulabilir
      if (item.turu !== 'ITS') {
        showMessage(`❌ ${item.productName} - ITS ürünü değil!`, 'error')
        playErrorSound()
        return
      }
      
      // Belge tarihini saat bilgisi olmadan formatla (YYYY-MM-DD) - Local time
      let belgeTarihiFormatted
      if (order.orderDate) {
        const date = new Date(order.orderDate)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, '0')
        const day = String(date.getDate()).padStart(2, '0')
        belgeTarihiFormatted = `${year}-${month}-${day}`
      } else {
        const today = new Date()
        const year = today.getFullYear()
        const month = String(today.getMonth() + 1).padStart(2, '0')
        const day = String(today.getDate()).padStart(2, '0')
        belgeTarihiFormatted = `${year}-${month}-${day}`
      }
      
      // Backend'e ITS karekod gönder
      const result = await apiService.saveITSBarcode({
        barcode: itsBarcode,
        documentId: order.id,
        itemId: item.itemId,
        stokKodu: item.stokKodu,
        belgeTip: item.stharHtur, // STHAR_HTUR
        gckod: item.stharGckod || '', // STHAR_GCKOD
        belgeNo: order.orderNo,
        belgeTarihi: belgeTarihiFormatted, // Belge tarihi (saat yok)
        docType: order.docType,
        expectedQuantity: item.quantity, // Miktar kontrolü için
        cariKodu: order.customerCode,    // Belgedeki CARI_KODU
        kullanici: JSON.parse(localStorage.getItem('user') || '{}').username || 'USER' // Sisteme giriş yapan kullanıcı
      })
      
      if (result.success) {
        console.log('✅ ITS Karekod başarıyla kaydedildi!')
        console.log('Ürün:', item.productName)
        console.log('Seri No:', result.data.seriNo)
        console.log('Miad:', result.data.miad)
        console.log('Lot:', result.data.lot)
        
        // Ürünü hazırlandı olarak işaretle
        const updatedItems = [...items]
        updatedItems[itemIndex].okutulan = (updatedItems[itemIndex].okutulan || 0) + 1
        updatedItems[itemIndex].isPrepared = updatedItems[itemIndex].okutulan >= updatedItems[itemIndex].quantity
        setItems(updatedItems)
        updateStats(updatedItems)
        
        showMessage(
          `✅ ${item.productName} - Seri: ${result.data.seriNo} (${updatedItems[itemIndex].okutulan}/${item.quantity})`, 
          'success'
        )
        playSuccessSound()
        
        // Check if all items are prepared
        if (updatedItems.every(item => item.okutulan >= item.quantity)) {
          setTimeout(() => {
            showMessage('🎉 Tüm ürünler tamamlandı!', 'success')
            playSuccessSound()
          }, 1000)
        }
      } else if (result.error === 'DUPLICATE') {
        // Duplicate karekod uyarısı - HATA!
        console.error('⚠️⚠️⚠️ DUPLICATE KAREKOD! Bu seri numarası daha önce okutulmuş!')
        console.error('Ürün:', item.productName)
        console.error('Stok Kodu:', item.stokKodu)
        
        // Seri numarasını karekoddan çıkar (21 ile başlayan kısım)
        const seriMatch = itsBarcode.match(/21([^\x1D]+)/)
        const seriKisa = seriMatch ? seriMatch[1].substring(0, 12) : 'N/A'
        
        showMessage(`❌ DUPLICATE! ${item.productName} - Seri: ${seriKisa}... - Bu karekod zaten okutulmuş!`, 'error')
        playErrorSound() // Warning yerine error sesi çal
      } else if (result.error === 'QUANTITY_EXCEEDED') {
        // Miktar aşımı uyarısı
        console.error('⚠️⚠️⚠️ MİKTAR AŞIMI! Bu üründen daha fazla okutulamaz!')
        console.error('Ürün:', item.productName)
        console.error('Miktar:', item.quantity)
        
        showMessage(`❌ MİKTAR AŞIMI! ${item.productName} - ${result.message}`, 'error')
        playErrorSound()
      } else {
        showMessage(`❌ ${item.productName} - ${result.message}`, 'error')
        playErrorSound()
      }
      
    } catch (error) {
      console.error('ITS Karekod Hatası:', error)
      showMessage(`❌ Hata: ${error.message}`, 'error')
      playErrorSound()
    }
  }

  // Show message
  const showMessage = (text, type) => {
    setMessage({ text, type })
    setTimeout(() => setMessage(null), 3000)
  }

  // UTS Modal içi mesajlar için özel fonksiyon
  const showUTSMessage = (text, type) => {
    setUtsModalMessage({ text, type })
    setTimeout(() => setUtsModalMessage(null), 4000)
  }

  // UTS Modal Aç
  const handleOpenUTSModal = async (item) => {
    try {
      setSelectedUTSItem(item)
      setShowUTSModal(true)
      setUtsLoading(true)
      setUtsHasChanges(false) // Temiz başlangıç
      
      // UTS kayıtlarını getir
      const response = await apiService.getUTSBarcodeRecords(order.id, item.itemId)
      
      if (response.success) {
        // Kayıtlara uretimTarihiDisplay ve benzersiz id ekle (YYMMDD -> YYYY-MM-DD)
        const enrichedRecords = (response.data || []).map((record, index) => {
          let uretimTarihiDisplay = ''
          if (record.uretimTarihi && record.uretimTarihi.length === 6) {
            const yy = record.uretimTarihi.substring(0, 2)
            const mm = record.uretimTarihi.substring(2, 4)
            const dd = record.uretimTarihi.substring(4, 6)
            const yyyy = parseInt(yy) > 50 ? `19${yy}` : `20${yy}`
            uretimTarihiDisplay = `${yyyy}-${mm}-${dd}`
          }
          return {
            ...record,
            id: record.siraNo || `existing-${Date.now()}-${index}`, // Benzersiz ID ekle
            uretimTarihiDisplay
          }
        })
        setUtsRecords(enrichedRecords)
        setOriginalUtsRecords(JSON.parse(JSON.stringify(enrichedRecords))) // Deep copy
      } else {
        console.error('UTS kayıtları yüklenemedi')
      }
    } catch (error) {
      console.error('UTS kayıtları yükleme hatası:', error)
    } finally {
      setUtsLoading(false)
    }
  }

  // UTS Modal Kapat
  const handleCloseUTSModal = (skipWarning = false) => {
    // Eğer skipWarning bir event ise (onClick'ten geliyorsa), false olarak ayarla
    const shouldSkipWarning = typeof skipWarning === 'boolean' ? skipWarning : false;
    
    // Kaydedilmemiş değişiklik varsa uyar (ama kaydet butonundan geliyorsa uyarma)
    if (!shouldSkipWarning && utsHasChanges) {
      const confirmClose = confirm('⚠️ Ekrandaki veriler kaydedilmemiştir. Modal kapatılsın mı?\n\nEmin misiniz?');
      if (!confirmClose) {
        return; // Modal'ı kapatma
      }
    }
    
    setShowUTSModal(false);
    setSelectedUTSItem(null);
    setUtsRecords([]);
    setSelectedUTSRecords([]);
    setUtsModalMessage(null); // Modal mesajını temizle
    setUtsHasChanges(false); // Değişiklik flag'ini temizle
  };

  // UTS Kayıtlarını Sil
  const handleDeleteUTSRecords = () => {
    if (selectedUTSRecords.length === 0) {
      showUTSMessage('⚠️ Lütfen silinecek kayıtları seçin', 'warning')
      playErrorSound()
      return
    }

    if (!confirm(`${selectedUTSRecords.length} kayıt grid'den kaldırılacak. "Kaydet" butonuna basınca veri tabanından silinecek. Emin misiniz?`)) {
      return
    }

    // Seçili kayıtların ID'lerini al
    const selectedIds = selectedUTSRecords.map(r => r.id)
    
    // Sadece grid'den kaldır - ID'ye göre filtrele
    const filteredRecords = utsRecords.filter(record => !selectedIds.includes(record.id))
    
    setUtsRecords(filteredRecords)
    setSelectedUTSRecords([])
    setUtsHasChanges(true) // Değişiklik yapıldı
    showUTSMessage(`✅ ${selectedUTSRecords.length} kayıt grid'den kaldırıldı. "Kaydet" butonuna basın.`, 'success')
    playSuccessSound()
  }

  // UTS Grid'e Yeni Boş Satır Ekle
  const handleAddNewUTSRow = () => {
    const newRow = {
      id: `new-${Date.now()}`,
      seriNo: '',
      lot: '',
      uretimTarihi: '',
      uretimTarihiDisplay: '',
      miktar: '', // Boş başlasın, kullanıcı girecek (seri no girilirse otomatik 1 olur)
      isNew: true
    }
    setUtsRecords([...utsRecords, newRow])
    setUtsHasChanges(true) // Değişiklik yapıldı
    
    // Grid'i scroll et yeni satıra
    setTimeout(() => {
      if (utsGridRef.current) {
        utsGridRef.current.api.ensureIndexVisible(utsRecords.length, 'bottom')
      }
    }, 100)
  }

  // Tüm UTS Kayıtlarını Kaydet
  const handleSaveAllUTSRecords = async () => {
    try {
      // Grid'den tüm satırları al
      const allRows = []
      utsGridRef.current.api.forEachNode(node => allRows.push(node.data))

      // Boş satırları filtrele
      const validRows = allRows.filter(row => row.seriNo || row.lot)

      // Eğer grid boşsa ama originalRecords varsa, silme işlemi yapılacak
      if (validRows.length === 0 && originalUtsRecords.length === 0) {
        showUTSMessage('❌ Kaydedilecek satır yok!', 'error')
        playErrorSound()
        return
      }

      // Eğer sadece silme işlemi yapılacaksa (grid boş, orijinalde kayıt var)
      if (validRows.length === 0 && originalUtsRecords.length > 0) {
        if (!confirm(`Tüm kayıtlar silinecek (${originalUtsRecords.length} kayıt). Emin misiniz?`)) {
          return
        }
      }

      // Validasyonlar (sadece kayıt varsa)
      if (validRows.length > 0) {
        for (let i = 0; i < validRows.length; i++) {
        const row = validRows[i]
        const rowNum = i + 1

        // Seri No veya Lot No zorunlu
        if (!row.seriNo && !row.lot) {
          showUTSMessage(`❌ Satır ${rowNum}: Seri No veya Lot No girilmeli!`, 'error')
          playErrorSound()
          return
        }

        // Seri No varsa Lot No da zorunlu
        if (row.seriNo && !row.lot) {
          showUTSMessage(`❌ Satır ${rowNum}: Seri No girildiğinde Lot No da girilmelidir!`, 'error')
          playErrorSound()
          return
        }

        // Üretim Tarihi her zaman zorunlu
        if (!row.uretimTarihi && !row.uretimTarihiDisplay) {
          showUTSMessage(`❌ Satır ${rowNum}: Üretim Tarihi zorunludur!`, 'error')
          playErrorSound()
          return
        }

        // Tarih formatı kontrolü (YYMMDD veya YYYY-MM-DD)
        let uretimTarihiYYMMDD = row.uretimTarihi
        if (row.uretimTarihiDisplay && row.uretimTarihiDisplay.includes('-')) {
          // YYYY-MM-DD -> YYMMDD
          const [yyyy, mm, dd] = row.uretimTarihiDisplay.split('-')
          uretimTarihiYYMMDD = `${yyyy.substring(2, 4)}${mm}${dd}`
        }

        if (uretimTarihiYYMMDD.length !== 6) {
          showUTSMessage(`❌ Satır ${rowNum}: Üretim Tarihi geçersiz format!`, 'error')
          playErrorSound()
          return
        }

        // Miktar kontrolü
        if (!row.miktar || row.miktar <= 0) {
          showUTSMessage(`❌ Satır ${rowNum}: Miktar boş olamaz ve 0'dan büyük olmalı!`, 'error')
          playErrorSound()
          return
        }

        // Seri no varsa miktar 1 olmalı
        if (row.seriNo && row.miktar !== 1) {
          showUTSMessage(`❌ Satır ${rowNum}: Seri No girildiğinde miktar 1 olmalı!`, 'error')
          playErrorSound()
          return
        }
      }

        // Seri No teklik kontrolü
        const serialNumbers = validRows.filter(r => r.seriNo).map(r => r.seriNo.trim().toLowerCase())
        const serialCounts = {}
        serialNumbers.forEach(sn => {
          serialCounts[sn] = (serialCounts[sn] || 0) + 1
        })
        const duplicateSerials = Object.keys(serialCounts).filter(sn => serialCounts[sn] > 1)
        if (duplicateSerials.length > 0) {
          showUTSMessage(`❌ Aynı Seri No birden fazla satırda kullanılamaz: ${duplicateSerials.join(', ')}`, 'error')
          playErrorSound()
          return
        }

        // Lot No teklik kontrolü
        const lotNumbers = validRows.filter(r => r.lot).map(r => r.lot.trim().toLowerCase())
        const lotCounts = {}
        lotNumbers.forEach(lot => {
          lotCounts[lot] = (lotCounts[lot] || 0) + 1
        })
        const duplicateLots = Object.keys(lotCounts).filter(lot => lotCounts[lot] > 1)
        if (duplicateLots.length > 0) {
          showUTSMessage(`❌ Aynı Lot numarası birden fazla satırda kullanılamaz: ${duplicateLots.join(', ')}`, 'error')
          playErrorSound()
          return
        }

        // Toplam miktar kontrolü
        const totalMiktar = validRows.reduce((sum, row) => sum + (row.miktar || 0), 0)
        if (totalMiktar > selectedUTSItem.quantity) {
          showUTSMessage(`❌ Toplam miktar (${totalMiktar}) belge kalemindeki miktarı (${selectedUTSItem.quantity}) geçemez!`, 'error')
          playErrorSound()
          return
        }
      } // Validasyonlar sonu

      // Belge tarihini formatla
      let belgeTarihiFormatted
      if (order.orderDate) {
        const date = new Date(order.orderDate)
        const year = date.getFullYear()
        const month = String(date.getMonth() + 1).padStart(2, '0')
        const day = String(date.getDate()).padStart(2, '0')
        belgeTarihiFormatted = `${year}-${month}-${day}`
      } else {
        const today = new Date()
        const year = today.getFullYear()
        const month = String(today.getMonth() + 1).padStart(2, '0')
        const day = String(today.getDate()).padStart(2, '0')
        belgeTarihiFormatted = `${year}-${month}-${day}`
      }

      // Bulk save API'yi çağır (toplu kaydet/güncelle/sil)
      const result = await apiService.saveUTSRecords({
        records: validRows,
        originalRecords: originalUtsRecords,
        documentId: order.id,
        itemId: selectedUTSItem.itemId,
        stokKodu: selectedUTSItem.stokKodu,
        belgeTip: selectedUTSItem.stharHtur,
        gckod: selectedUTSItem.stharGckod || '',
        belgeNo: order.orderNo,
        belgeTarihi: belgeTarihiFormatted,
        docType: order.docType,
        expectedQuantity: selectedUTSItem.quantity,
        barcode: selectedUTSItem.barcode || selectedUTSItem.stokKodu,
        cariKodu: order.customerCode,    // Belgedeki CARI_KODU
        kullanici: JSON.parse(localStorage.getItem('user') || '{}').username || 'USER' // Sisteme giriş yapan kullanıcı
      })

      if (result.success) {
        showUTSMessage(`✅ ${result.message}`, 'success')
        playSuccessSound()
        setUtsHasChanges(false) // Değişiklikler kaydedildi
      } else {
        showUTSMessage(`❌ ${result.message}`, 'error')
        playErrorSound()
        return
      }

      // Grid'i yenile
      const response = await apiService.getUTSBarcodeRecords(order.id, selectedUTSItem.itemId)
      if (response.success) {
        // Kayıtlara uretimTarihiDisplay ekle (YYMMDD -> YYYY-MM-DD)
        const enrichedRecords = (response.data || []).map(record => {
          let uretimTarihiDisplay = ''
          if (record.uretimTarihi && record.uretimTarihi.length === 6) {
            const yy = record.uretimTarihi.substring(0, 2)
            const mm = record.uretimTarihi.substring(2, 4)
            const dd = record.uretimTarihi.substring(4, 6)
            const yyyy = parseInt(yy) > 50 ? `19${yy}` : `20${yy}`
            uretimTarihiDisplay = `${yyyy}-${mm}-${dd}`
          }
          return {
            ...record,
            uretimTarihiDisplay
          }
        })
        setUtsRecords(enrichedRecords)
        setOriginalUtsRecords(JSON.parse(JSON.stringify(enrichedRecords))) // Yeni orijinal
      }

      // Ana grid'i güncelle
      const docResponse = await apiService.getDocumentById(order.id)
      if (docResponse.success && docResponse.data) {
        setItems(docResponse.data.items || [])
        updateStats(docResponse.data.items || [])
      }

      // Başarılı kayıt sonrası modal'ı kapat (uyarı gösterme)
      setTimeout(() => {
        handleCloseUTSModal(true) // skipWarning = true
      }, 1000) // 1 saniye sonra kapat (başarı mesajını göster)
      
    } catch (error) {
      console.error('UTS toplu kayıt hatası:', error)
      showUTSMessage('❌ Kayıt sırasında hata oluştu', 'error')
      playErrorSound()
    }
  }


  // ITS Modal Aç
  const handleOpenITSModal = async (item) => {
    try {
      setSelectedItem(item)
      setShowITSModal(true)
      setItsLoading(true)
      
      // ITS kayıtlarını getir
      const response = await apiService.getITSBarcodeRecords(order.id, item.itemId)
      
      if (response.success) {
        setItsRecords(response.data || [])
      } else {
        console.error('ITS kayıtları yüklenemedi')
      }
    } catch (error) {
      console.error('ITS kayıtları yükleme hatası:', error)
    } finally {
      setItsLoading(false)
    }
  }

  // ITS Modal Kapat
  const handleCloseITSModal = () => {
    setShowITSModal(false)
    setSelectedItem(null)
    setItsRecords([])
    setSelectedRecords([])
    setItsModalView('grid') // View'i sıfırla
  }
  
  // ITS Karekodları Text Formatında Oluştur
  const generateITSBarcodeTexts = () => {
    return itsRecords.map(record => {
      // Format: 010 + ILC_GTIN + 21 + SERI_NO + 17 + ACIK1 + 10 + ACIK2
      // + işaretleri olmadan, değerler direkt birleştirilir
      const parts = [
        '010',
        record.barkod || '',
        '21',
        record.seriNo || '',
        '17',
        record.miad || '',
        '10',
        record.lot || ''
      ]
      return parts.join('')
    }).join('\n')
  }
  
  // Tüm Karekodları Kopyala
  const handleCopyAllBarcodes = () => {
    const text = generateITSBarcodeTexts()
    navigator.clipboard.writeText(text).then(() => {
      console.log('✅ Karekodlar kopyalandı!')
      playSuccessSound()
      alert('✅ Karekodlar panoya kopyalandı!')
    }).catch(err => {
      console.error('Kopyalama hatası:', err)
      playErrorSound()
      alert('❌ Kopyalama başarısız!')
    })
  }


  // ITS Kayıtlarını Sil
  const handleDeleteITSRecords = async () => {
    if (selectedRecords.length === 0) {
      alert('⚠️ Lütfen silinecek kayıtları seçin')
      return
    }

    // Seçili kayıtlarda koli barkodu var mı kontrol et
    const recordsWithCarrier = selectedRecords.filter(record => {
      const fullRecord = itsRecords.find(r => r.seriNo === record)
      return fullRecord && fullRecord.carrierLabel
    })

    // Koli barkodu varsa ve tüm kayıtlar seçili değilse uyar
    if (recordsWithCarrier.length > 0) {
      // Her bir koli barkodu için o koliden kaç kayıt olduğunu ve kaçının seçildiğini kontrol et
      const carrierLabels = new Set()
      recordsWithCarrier.forEach(record => {
        const fullRecord = itsRecords.find(r => r.seriNo === record)
        if (fullRecord && fullRecord.carrierLabel) {
          carrierLabels.add(fullRecord.carrierLabel)
        }
      })

      // Her koli için kontrol yap
      let hasPartialSelection = false
      for (const carrierLabel of carrierLabels) {
        const totalWithCarrier = itsRecords.filter(r => r.carrierLabel === carrierLabel).length
        const selectedWithCarrier = recordsWithCarrier.filter(record => {
          const fullRecord = itsRecords.find(r => r.seriNo === record)
          return fullRecord && fullRecord.carrierLabel === carrierLabel
        }).length
        
        if (selectedWithCarrier < totalWithCarrier) {
          hasPartialSelection = true
          break
        }
      }

      // Kullanıcıya uyarı göster
      let confirmMessage = hasPartialSelection 
        ? `⚠️ UYARI: Seçili kayıtlardan bazıları koli ile okutulmuştur.\n\nBu satırları silerseniz koli bütünlüğü bozulacak ve aynı koli barkoduna sahip diğer kayıtların da koli bilgisi silinecektir.\n\n${selectedRecords.length} kayıt silinecek. Emin misiniz?`
        : `${selectedRecords.length} kayıt silinecek (koli bilgileri de silinecek). Emin misiniz?`
      
      if (!confirm(confirmMessage)) {
        return
      }
    } else {
      if (!confirm(`${selectedRecords.length} kayıt silinecek. Emin misiniz?`)) {
        return
      }
    }

    try {
      const result = await apiService.deleteITSBarcodeRecords(
        order.id,
        selectedItem.itemId,
        selectedRecords
      )

      if (result.success) {
        console.log('✅ ITS kayıtlar silindi:', result.deletedCount)
        // Kayıtları yeniden yükle
        const response = await apiService.getITSBarcodeRecords(order.id, selectedItem.itemId)
        if (response.success) {
          setItsRecords(response.data || [])
          setSelectedRecords([])
        }
        
        // Ana grid'i yenile
        const docResponse = await apiService.getDocumentById(order.id)
        if (docResponse.success && docResponse.data) {
          setItems(docResponse.data.items || [])
        }
      } else {
        alert('❌ Kayıtlar silinemedi: ' + result.message)
      }
    } catch (error) {
      console.error('ITS kayıt silme hatası:', error)
      alert('❌ Kayıtlar silinemedi')
    }
  }

  // Sound effects - Web Audio API ile gerçek ses
  const playSuccessSound = () => {
    try {
      const audioContext = new (window.AudioContext || window.webkitAudioContext)()
      const oscillator = audioContext.createOscillator()
      const gainNode = audioContext.createGain()
      
      oscillator.connect(gainNode)
      gainNode.connect(audioContext.destination)
      
      oscillator.frequency.value = 800 // Başarı için yüksek ton
      oscillator.type = 'sine'
      
      gainNode.gain.setValueAtTime(0.3, audioContext.currentTime)
      gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.15)
      
      oscillator.start(audioContext.currentTime)
      oscillator.stop(audioContext.currentTime + 0.15)
    } catch (error) {
      console.log('Success beep!')
    }
  }

  const playErrorSound = () => {
    try {
      const audioContext = new (window.AudioContext || window.webkitAudioContext)()
      const oscillator = audioContext.createOscillator()
      const gainNode = audioContext.createGain()
      
      oscillator.connect(gainNode)
      gainNode.connect(audioContext.destination)
      
      oscillator.frequency.value = 200 // Hata için düşük ton
      oscillator.type = 'sawtooth'
      
      gainNode.gain.setValueAtTime(0.3, audioContext.currentTime)
      gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.3)
      
      oscillator.start(audioContext.currentTime)
      oscillator.stop(audioContext.currentTime + 0.3)
    } catch (error) {
      console.log('Error beep!')
    }
  }

  const playWarningSound = () => {
    try {
      const audioContext = new (window.AudioContext || window.webkitAudioContext)()
      const oscillator = audioContext.createOscillator()
      const gainNode = audioContext.createGain()
      
      oscillator.connect(gainNode)
      gainNode.connect(audioContext.destination)
      
      oscillator.frequency.value = 500 // Uyarı için orta ton
      oscillator.type = 'square'
      
      gainNode.gain.setValueAtTime(0.2, audioContext.currentTime)
      gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.2)
      
      oscillator.start(audioContext.currentTime)
      oscillator.stop(audioContext.currentTime + 0.2)
    } catch (error) {
      console.log('Warning beep!')
    }
  }

  // Tamamlanma yüzdesini miktar bazında hesapla (Hook'lar early return'den ÖNCE olmalı)
  const completionPercentage = useMemo(() => {
    const totalQuantity = items.reduce((sum, item) => sum + (item.quantity || 0), 0)
    const totalOkutulan = items.reduce((sum, item) => sum + (item.okutulan || 0), 0)
    
    if (totalQuantity === 0) return 0
    return Math.round((totalOkutulan / totalQuantity) * 100)
  }, [items])

  // Row Style - Satır renklerine göre
  const getRowStyle = (params) => {
    // Footer satırı için stil - header ile aynı renk
    if (params.node.rowPinned === 'bottom') {
      return { 
        backgroundColor: '#f9fafb',
        fontWeight: 'bold',
        borderTop: '2px solid #e5e7eb'
      }
    }
    
    const quantity = params.data.quantity || 0
    const okutulan = params.data.okutulan || 0
    
    // Tamamı okutulan → Yeşil
    if (okutulan > 0 && okutulan >= quantity) {
      return { 
        backgroundColor: '#f0fdf4'
      }
    }
    
    // Kısmen okutulan → Sarı
    if (okutulan > 0 && okutulan < quantity) {
      return { 
        backgroundColor: '#fef9e7'
      }
    }
    
    // Hiç okutulmayan → Normal (beyaz)
    return { 
      backgroundColor: '#ffffff'
    }
  }

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <div className="animate-spin w-12 h-12 border-4 border-gray-200 border-t-primary-600 rounded-full mx-auto mb-4" />
          <p className="text-gray-600">Belge yükleniyor...</p>
        </div>
      </div>
    )
  }

  if (!order) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <Package className="w-16 h-16 text-gray-400 mx-auto mb-4" />
          <p className="text-gray-600">Belge bulunamadı</p>
          <button
            onClick={() => navigate('/documents')}
            className="mt-4 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
          >
            Belgelere Dön
          </button>
        </div>
      </div>
    )
  }

  return (
    <div className="flex flex-col h-screen bg-gray-50">
      {/* Header - Ultra Compact */}
      <div className="bg-gradient-to-r from-white to-gray-50 border-b border-primary-100 shadow-sm">
        <div className="px-4 py-1.5">
          <div className="flex items-center justify-between">
            {/* Left - Back Button & Document Info */}
            <div className="flex items-center gap-2">
              <button
                onClick={() => navigate('/documents')}
                className="w-8 h-8 flex items-center justify-center rounded bg-gray-600 hover:bg-gray-700 transition-all shadow-lg hover:shadow-xl"
                title="Geri Dön"
              >
                <ArrowLeft className="w-5 h-5 text-white" />
              </button>
              <div className={`px-3 py-1 rounded-lg border shadow-sm ${
                order.docType === '6' 
                  ? 'bg-purple-100 border-purple-300' 
                  : order.docType === '1' 
                  ? 'bg-green-100 border-green-300' 
                  : 'bg-orange-100 border-orange-300'
              }`}>
                <p className={`text-[9px] font-medium leading-tight ${
                  order.docType === '6' 
                    ? 'text-purple-700' 
                    : order.docType === '1' 
                    ? 'text-green-700' 
                    : 'text-orange-700'
                }`}>
                  {getDocumentTypeName(order.docType, order.tipi)}
                </p>
                <h1 className={`text-sm font-bold leading-tight ${
                  order.docType === '6' 
                    ? 'text-purple-900' 
                    : order.docType === '1' 
                    ? 'text-green-900' 
                    : 'text-orange-900'
                }`}>{order.orderNo}</h1>
              </div>
            </div>
            
            {/* Center - Customer Info Cards - Ultra Compact */}
            <div className="flex items-center gap-1.5">
              {/* 1. Tarih */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <Calendar className="w-3 h-3 text-orange-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">Tarih</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">
                      {order.orderDate ? new Date(order.orderDate).toLocaleDateString('tr-TR') : '-'}
                    </p>
                  </div>
                </div>
              </div>

              {/* 2. Cari Kodu */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <Hash className="w-3 h-3 text-purple-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">Cari Kodu</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">{order.customerCode}</p>
                  </div>
                </div>
              </div>

              {/* 3. Cari İsim */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <User className="w-3 h-3 text-blue-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">Cari İsim</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">{order.customerName}</p>
                  </div>
                </div>
              </div>

              {/* 4. İlçe/Şehir */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <MapPin className="w-3 h-3 text-green-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">İlçe / Şehir</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">
                      {order.district ? `${order.district} / ${order.city}` : order.city}
                    </p>
                  </div>
                </div>
              </div>

              {/* 5. GLN No */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <FileText className="w-3 h-3 text-teal-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">GLN No</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">
                      {order.email || '-'}
                    </p>
                  </div>
                </div>
              </div>

              {/* 6. UTS No */}
              <div className="bg-white px-2.5 py-1 rounded-lg border border-gray-200 shadow-sm">
                <div className="flex items-center gap-1.5">
                  <FileText className="w-3 h-3 text-indigo-600" />
                  <div>
                    <p className="text-[9px] text-gray-500 leading-tight">UTS No</p>
                    <p className="text-sm font-bold text-gray-900 leading-tight">
                      {order.utsNo || '-'}
                    </p>
                  </div>
                </div>
              </div>
            </div>
            
            {/* Right - Completion - Ultra Compact */}
            <div className="bg-gradient-to-br from-primary-500 to-primary-600 px-3 py-1 rounded-lg shadow-md">
              <div className="flex items-center gap-2">
                <div className="text-xl font-bold text-white leading-tight">{completionPercentage}%</div>
                <div className="w-12 bg-white/20 rounded-full h-1.5 overflow-hidden">
                  <div 
                    className="h-full bg-white transition-all duration-500"
                    style={{ width: `${completionPercentage}%` }}
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Barcode Scanner - Enhanced & Prominent */}
      <div className={`transition-all duration-300 ${
        deleteMode && koliMode
          ? 'bg-gradient-to-r from-orange-500 via-amber-500 to-orange-600'
          : deleteMode
          ? 'bg-gradient-to-r from-red-500 via-red-600 to-red-700'
          : koliMode
          ? 'bg-gradient-to-r from-emerald-500 via-teal-500 to-cyan-600'
          : 'bg-gradient-to-r from-primary-500 to-primary-600'
      }`}>
        <div className="px-6 py-3">
          <form onSubmit={handleBarcodeScan}>
            <div className="flex gap-2 items-center">
              {/* Silme Modu Checkbox */}
              <div className="flex items-center">
                <label className={`flex items-center gap-2 cursor-pointer backdrop-blur-sm px-3 py-1.5 rounded shadow-lg hover:shadow-xl transition-all ${
                  deleteMode 
                    ? 'bg-white text-red-600 ring-2 ring-white/50' 
                    : 'bg-white/20 text-white hover:bg-white/30'
                }`}>
                  <input
                    type="checkbox"
                    checked={deleteMode}
                    onChange={(e) => {
                      setDeleteMode(e.target.checked)
                      // Checkbox değiştikten sonra barkod input'una focus et
                      setTimeout(() => {
                        barcodeInputRef.current?.focus()
                      }, 0)
                    }}
                    className="w-4 h-4 cursor-pointer accent-red-600"
                  />
                  <span className="font-semibold text-sm">🗑️ Sil</span>
                </label>
              </div>
              
              {/* Koli Modu Checkbox */}
              <div className="flex items-center">
                <label className={`flex items-center gap-2 cursor-pointer backdrop-blur-sm px-3 py-1.5 rounded shadow-lg hover:shadow-xl transition-all ${
                  koliMode 
                    ? 'bg-white text-emerald-600 ring-2 ring-white/50' 
                    : 'bg-white/20 text-white hover:bg-white/30'
                }`}>
                  <input
                    type="checkbox"
                    checked={koliMode}
                    onChange={(e) => {
                      setKoliMode(e.target.checked)
                      // Checkbox değiştikten sonra barkod input'una focus et
                      setTimeout(() => {
                        barcodeInputRef.current?.focus()
                      }, 0)
                    }}
                    className="w-4 h-4 cursor-pointer accent-emerald-600"
                  />
                  <span className="font-semibold text-sm">📦 Koli</span>
                </label>
              </div>
              
              <div className="flex-1 relative">
                <Barcode className={`absolute left-4 top-1/2 transform -translate-y-1/2 w-6 h-6 transition-all ${
                  deleteMode && koliMode
                    ? 'text-orange-600'
                    : deleteMode
                    ? 'text-red-600'
                    : koliMode
                    ? 'text-emerald-600'
                    : 'text-gray-400'
                }`} />
                <input
                  ref={barcodeInputRef}
                  type="text"
                  value={barcodeInput}
                  onChange={(e) => setBarcodeInput(e.target.value)}
                  placeholder={
                    deleteMode && koliMode
                      ? "⚠️ KOLİ SİLME MODU - Koli barkodu okutun..."
                      : deleteMode 
                      ? "🗑️ Silmek için barkod okutun (ITS için karekod gerekli)..." 
                      : koliMode
                      ? "📦 Koli barkodu okutun..."
                      : "📱 Barkod okutun (ITS: karekod, DGR/UTS: normal barkod veya 100*Barkod)"
                  }
                  className={`w-full pl-14 pr-4 py-2 text-xl font-mono font-bold rounded-lg shadow-2xl focus:outline-none transition-all ${
                    deleteMode && koliMode
                      ? 'bg-white text-orange-700 border-4 border-orange-500 placeholder-orange-400 focus:ring-4 focus:ring-orange-300'
                      : deleteMode 
                      ? 'bg-white text-red-700 border-4 border-red-500 placeholder-red-400 focus:ring-4 focus:ring-red-300'
                      : koliMode
                      ? 'bg-white text-emerald-700 border-4 border-emerald-500 placeholder-emerald-400 focus:ring-4 focus:ring-emerald-300'
                      : 'bg-white text-gray-900 border-4 border-white placeholder-gray-400 focus:ring-4 focus:ring-blue-300'
                  }`}
                  autoComplete="off"
                />
              </div>
              {/* Hidden submit button for Enter key to work */}
              <button type="submit" className="hidden" aria-hidden="true" />
              
              {deleteMode ? (
                <button
                  type="button"
                  onClick={handleBarcodeScan}
                  className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-red-600 text-white hover:bg-red-700"
                >
                  Sil
                </button>
              ) : (
                <>
                  <button
                    type="button"
                    onClick={() => setShowBulkScanModal(true)}
                    className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-white/90 text-primary-600 hover:bg-white border-2 border-white/50"
                    title="Toplu ITS karekod okutma"
                  >
                    📋 Toplu Karekod
                  </button>
                  <button
                    type="button"
                    onClick={fetchDocument}
                    className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-white/90 text-primary-600 hover:bg-white border-2 border-white/50"
                    title="Grid'i yenile"
                  >
                    🔄 Yenile
                  </button>
                </>
              )}
            </div>
          </form>
        </div>
        
        {/* Message Strip - Enhanced with Mode Colors */}
        <div className={`px-6 py-2 transition-all duration-300 shadow-inner ${
          message 
            ? message.type === 'success' 
              ? 'bg-green-600' 
              : message.type === 'error' 
              ? 'bg-red-600' 
              : message.type === 'info'
              ? 'bg-blue-600'
              : 'bg-yellow-600'
            : deleteMode && koliMode
            ? 'bg-orange-600'
            : deleteMode 
            ? 'bg-red-700'
            : koliMode
            ? 'bg-emerald-700'
            : 'bg-primary-700'
        }`}>
          <p className="text-white font-bold text-center text-base h-6 leading-6 overflow-hidden text-ellipsis whitespace-nowrap">
            {message 
              ? message.text 
              : deleteMode && koliMode
              ? '⚠️ KOLİ SİLME MODU AKTİF - Koli barkodu okutarak tüm içeriği silebilirsiniz!'
              : deleteMode 
              ? '🗑️ SİLME MODU AKTİF - ITS: Karekod okutun | DGR/UTS: Normal barkod okutun'
              : koliMode
              ? '📦 KOLİ MODU AKTİF - Koli barkodu okutun'
              : '📱 ITS ürünler için KAREKOD (2D) zorunlu | DGR/UTS için normal barkod (Toplu: 100*Barkod)'}
          </p>
        </div>
      </div>

      {/* AG Grid */}
      <div className="flex-1 px-6 py-4">
        <div className="ag-theme-alpine h-full rounded-lg shadow-lg overflow-hidden border border-gray-200">
          <AgGridReact
            rowData={items}
            columnDefs={columnDefs}
            defaultColDef={defaultColDef}
            animateRows={true}
            getRowStyle={getRowStyle}
            enableCellTextSelection={true}
            suppressCellFocus={true}
            pinnedBottomRowData={[totals]}
            suppressRowHoverHighlight={false}
            getRowClass={(params) => {
              if (params.node.rowPinned === 'bottom') {
                return 'footer-row-no-hover'
              }
              return ''
            }}
          />
        </div>
      </div>

      {/* UTS Kayıtları Modal */}
      {showUTSModal && selectedUTSItem && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50" onClick={handleCloseUTSModal}>
          <div className="bg-white rounded-xl shadow-2xl w-[90%] max-w-5xl max-h-[80vh] overflow-hidden" onClick={(e) => e.stopPropagation()}>
            {/* Modal Header */}
            <div className="bg-gradient-to-r from-red-500 to-red-600 px-6 py-4 text-white">
              <div className="flex items-center justify-between">
                <div>
                  <h2 className="text-xl font-bold">UTS Kayıtları</h2>
                  <p className="text-sm text-red-100">{selectedUTSItem.productName}</p>
                </div>
                <div className="flex items-center gap-4">
                  <div className="text-right">
                    <p className="text-xs text-red-100">Beklenen / Okutulan / Kalan</p>
                    <p className="text-2xl font-bold">
                      <span className="text-red-100">{selectedUTSItem.quantity}</span>
                      {' / '}
                      <span>{utsRecords.reduce((sum, r) => sum + (r.miktar || 0), 0)}</span>
                      {' / '}
                      <span className={utsRecords.reduce((sum, r) => sum + (r.miktar || 0), 0) >= selectedUTSItem.quantity ? 'text-green-300' : 'text-yellow-300'}>
                        {selectedUTSItem.quantity - utsRecords.reduce((sum, r) => sum + (r.miktar || 0), 0)}
                      </span>
                    </p>
                  </div>
                  <button
                    onClick={handleCloseUTSModal}
                    className="w-8 h-8 flex items-center justify-center rounded-lg hover:bg-white/20 transition-colors"
                  >
                    <XCircle className="w-5 h-5" />
                  </button>
                </div>
              </div>
            </div>

            {/* Modal Body */}
            <div className="p-6 flex flex-col" style={{ height: 'calc(80vh - 100px)' }}>
              {/* UTS Modal Toast Message */}
              {utsModalMessage && (
                <div className={`mb-4 px-4 py-3 rounded-lg shadow-lg border-l-4 animate-pulse ${
                  utsModalMessage.type === 'success' 
                    ? 'bg-green-50 border-green-500 text-green-800' 
                    : utsModalMessage.type === 'error' 
                    ? 'bg-red-50 border-red-500 text-red-800'
                    : 'bg-yellow-50 border-yellow-500 text-yellow-800'
                }`}>
                  <p className="font-semibold">{utsModalMessage.text}</p>
                </div>
              )}
              
              {/* UTS Records Grid */}
              <div className="ag-theme-alpine flex-1 mb-4">
                {utsLoading ? (
                  <div className="flex items-center justify-center h-full">
                    <div className="text-center">
                      <div className="animate-spin w-8 h-8 border-3 border-gray-200 border-t-red-600 rounded-full mx-auto mb-2" />
                      <p className="text-gray-600 text-sm">Yükleniyor...</p>
                    </div>
                  </div>
                ) : (
                  <AgGridReact
                    ref={utsGridRef}
                    rowData={utsRecords}
                    columnDefs={utsModalColumnDefs}
                    defaultColDef={utsModalDefaultColDef}
                    getRowId={(params) => params.data.id}
                    rowSelection="multiple"
                    suppressRowClickSelection={true}
                    onSelectionChanged={(event) => {
                      const selected = event.api.getSelectedRows()
                      // Benzersiz ID, Seri No, Lot No ve Sira No kombinasyonunu sakla
                      setSelectedUTSRecords(selected.map(r => ({
                        id: r.id,
                        siraNo: r.siraNo,
                        seriNo: r.seriNo,
                        lot: r.lot
                      })))
                    }}
                    onCellValueChanged={(event) => {
                      // Hücre değiştiğinde state'i güncelle (toplamlar için)
                      const allRows = []
                      event.api.forEachNode(node => allRows.push(node.data))
                      setUtsRecords([...allRows])
                      setUtsHasChanges(true) // Değişiklik yapıldı
                    }}
                    animateRows={true}
                    enableCellTextSelection={true}
                    singleClickEdit={true}
                    stopEditingWhenCellsLoseFocus={true}
                  />
                )}
              </div>

              {/* Action Bar - Fixed at Bottom */}
              <div className="flex items-center gap-3 border-t border-gray-200 pt-4">
                <button
                  onClick={handleAddNewUTSRow}
                  className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-green-600 text-white hover:bg-green-700"
                >
                  ➕ Yeni Satır Ekle
                </button>
                <button
                  onClick={handleSaveAllUTSRecords}
                  className={`flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-blue-600 text-white hover:bg-blue-700 ${utsHasChanges ? 'animate-pulse-save' : ''}`}
                >
                  💾 Kaydet
                </button>
                <button
                  onClick={handleDeleteUTSRecords}
                  disabled={selectedUTSRecords.length === 0}
                  className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-red-600 text-white hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  🗑️ Seçilenleri Sil
                </button>
                <div className="flex-1" />
                {selectedUTSRecords.length > 0 && (
                  <span className="text-sm text-gray-600 font-semibold">
                    {selectedUTSRecords.length} kayıt seçildi
                  </span>
                )}
                <div className="text-right">
                  <p className="text-xs text-gray-500">Toplam Miktar</p>
                  <p className="text-lg font-bold text-blue-600">
                    {utsRecords.reduce((sum, r) => sum + (r.miktar || 0), 0)} / {selectedUTSItem.quantity}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* ITS Karekod Detay Modal */}
      {showITSModal && selectedItem && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50" onClick={handleCloseITSModal}>
          <div className="bg-white rounded-xl shadow-2xl w-[90%] max-w-5xl max-h-[80vh] overflow-hidden" onClick={(e) => e.stopPropagation()}>
            {/* Modal Header */}
            <div className="bg-gradient-to-r from-primary-500 to-primary-600 px-6 py-4 text-white">
              <div className="flex items-center justify-between">
                <div>
                  <h2 className="text-xl font-bold">ITS Karekod Detayları</h2>
                  <p className="text-sm text-primary-100">{selectedItem.productName}</p>
                </div>
                <div className="flex items-center gap-4">
                  <div className="text-right">
                    <p className="text-xs text-primary-100">Toplam Okutulan</p>
                    <p className="text-2xl font-bold">{itsRecords.length}</p>
                  </div>
                  <button
                    onClick={handleCloseITSModal}
                    className="w-8 h-8 flex items-center justify-center rounded-lg hover:bg-white/20 transition-colors"
                  >
                    <XCircle className="w-5 h-5" />
                  </button>
                </div>
              </div>
            </div>

            {/* Modal Body */}
            <div className="p-6 flex flex-col" style={{ height: 'calc(80vh - 100px)' }}>
              {itsModalView === 'grid' ? (
                <>
                  {/* ITS Records Grid */}
                  <div className="ag-theme-alpine flex-1 mb-4">
                    {itsLoading ? (
                      <div className="flex items-center justify-center h-full">
                        <div className="text-center">
                          <div className="animate-spin w-8 h-8 border-3 border-gray-200 border-t-primary-600 rounded-full mx-auto mb-2" />
                          <p className="text-gray-600 text-sm">Yükleniyor...</p>
                        </div>
                      </div>
                    ) : (
                      <AgGridReact
                        rowData={itsRecords}
                        columnDefs={itsModalColumnDefs}
                        defaultColDef={itsModalDefaultColDef}
                        rowSelection="multiple"
                        suppressRowClickSelection={true}
                        onSelectionChanged={(event) => {
                          const selected = event.api.getSelectedRows()
                          setSelectedRecords(selected.map(r => r.seriNo))
                        }}
                        animateRows={true}
                        enableCellTextSelection={true}
                      />
                    )}
                  </div>

                  {/* Action Bar - Fixed at Bottom */}
                  <div className="flex items-center gap-3 border-t border-gray-200 pt-4">
                    <button
                      onClick={handleDeleteITSRecords}
                      disabled={selectedRecords.length === 0}
                      className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-red-600 text-white hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      Seçilenleri Sil
                    </button>
                    <button
                      onClick={() => setItsModalView('text')}
                      className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-blue-600 text-white hover:bg-blue-700"
                    >
                      📄 Karekodları Göster
                    </button>
                    {selectedRecords.length > 0 && (
                      <span className="text-sm text-gray-600 font-semibold">
                        {selectedRecords.length} kayıt seçildi
                      </span>
                    )}
                  </div>
                </>
              ) : (
                <>
                  {/* ITS Karekod Text View */}
                  <div className="flex-1 mb-4 flex flex-col">
                    <div className="mb-3 flex items-center justify-between">
                      <h3 className="text-lg font-bold text-gray-900">
                        Karekod Text Formatı
                      </h3>
                      <span className="text-sm text-gray-600">
                        {itsRecords.length} kayıt
                      </span>
                    </div>
                    <textarea
                      value={generateITSBarcodeTexts()}
                      readOnly
                      className="flex-1 w-full p-4 font-mono text-sm border border-gray-300 rounded-lg bg-gray-50 resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                      style={{ minHeight: '400px' }}
                    />
                  </div>

                  {/* Action Bar - Fixed at Bottom */}
                  <div className="flex items-center gap-3 border-t border-gray-200 pt-4">
                    <button
                      onClick={() => setItsModalView('grid')}
                      className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-gray-600 text-white hover:bg-gray-700"
                    >
                      ← Tabloya Dön
                    </button>
                    <button
                      onClick={handleCopyAllBarcodes}
                      className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-green-600 text-white hover:bg-green-700"
                    >
                      📋 Tümünü Kopyala
                    </button>
                    <span className="text-sm text-gray-600">
                      Format: 010BARKOD21SERİNO17MİAD10LOT
                    </span>
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Toplu Okutma Modal */}
      {showBulkScanModal && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-[9999] p-4">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-2xl max-h-[90vh] flex flex-col">
            {/* Modal Header */}
            <div className="px-6 py-4 border-b border-gray-200 flex items-center justify-between bg-gradient-to-r from-primary-600 to-primary-700 rounded-t-2xl">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 bg-white/20 rounded-lg flex items-center justify-center">
                  <Barcode className="w-6 h-6 text-white" />
                </div>
                <div>
                  <h2 className="text-xl font-bold text-white">Toplu ITS Karekod Okutma</h2>
                  <p className="text-xs text-white/80">Her satıra bir ITS karekod (2D) yazın</p>
                </div>
              </div>
              <button
                onClick={() => {
                  setShowBulkScanModal(false)
                  setBulkBarcodeText('')
                  setBulkScanResults(null)
                }}
                className="w-8 h-8 flex items-center justify-center rounded-lg bg-white/10 hover:bg-white/20 transition-colors"
                disabled={bulkScanLoading}
              >
                <XCircle className="w-5 h-5 text-white" />
              </button>
            </div>

            {/* Modal Body */}
            <div className="p-6 flex-1 flex flex-col gap-4 overflow-y-auto">
              {/* Textarea with Line Numbers */}
              <div className="flex-1">
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  ITS Karekod Listesi
                  <span className="text-gray-500 font-normal ml-2">(Her satıra bir ITS karekod)</span>
                </label>
                <div className="flex border-2 border-gray-300 rounded-lg focus-within:border-primary-500 overflow-hidden" style={{ height: '256px' }}>
                  {/* Line Numbers */}
                  <div 
                    ref={bulkLineNumbersRef}
                    className="bg-gray-100 px-3 py-3 font-mono text-sm text-gray-500 text-right select-none border-r border-gray-300 overflow-hidden" 
                    style={{ minWidth: '50px', maxHeight: '256px', overflowY: 'hidden' }}
                  >
                    {bulkBarcodeText.split('\n').map((_, index) => (
                      <div key={index} style={{ lineHeight: '24px', height: '24px' }}>
                        {index + 1}
                      </div>
                    ))}
                    {bulkBarcodeText === '' && <div style={{ lineHeight: '24px', height: '24px' }}>1</div>}
                  </div>
                  {/* Textarea */}
                  <textarea
                    ref={bulkTextareaRef}
                    value={bulkBarcodeText}
                    onChange={(e) => setBulkBarcodeText(e.target.value)}
                    onScroll={handleBulkTextareaScroll}
                    className="flex-1 px-4 py-3 border-0 focus:outline-none font-mono text-sm resize-none"
                    placeholder="010867978996572117081600001234&#10;010867978996572117081600005678&#10;010867978996572117081600009999"
                    disabled={bulkScanLoading}
                    autoFocus
                    style={{ height: '256px', lineHeight: '24px' }}
                  />
                </div>
                <p className="text-xs text-gray-500 mt-1">
                  💡 Sadece ITS karekod (2D barkod, 01... ile başlayan) desteklenir
                </p>
              </div>

              {/* Results */}
              {bulkScanResults && (
                <div className="bg-gray-50 rounded-lg p-4 border border-gray-200">
                  <h3 className="font-semibold text-gray-900 mb-3">İşlem Sonuçları</h3>
                  <div className="grid grid-cols-3 gap-3 mb-3">
                    <div className="bg-white rounded-lg p-3 text-center border border-gray-200">
                      <p className="text-2xl font-bold text-gray-900">{bulkScanResults.total}</p>
                      <p className="text-xs text-gray-600">Toplam</p>
                    </div>
                    <div className="bg-green-50 rounded-lg p-3 text-center border border-green-200">
                      <p className="text-2xl font-bold text-green-700">{bulkScanResults.success}</p>
                      <p className="text-xs text-green-600">Başarılı</p>
                    </div>
                    <div className="bg-red-50 rounded-lg p-3 text-center border border-red-200">
                      <p className="text-2xl font-bold text-red-700">{bulkScanResults.failed}</p>
                      <p className="text-xs text-red-600">Başarısız</p>
                    </div>
                  </div>
                  
                  {bulkScanResults.errors.length > 0 && (
                    <div className="max-h-32 overflow-y-auto">
                      <p className="text-xs font-semibold text-red-700 mb-2">Hatalar:</p>
                      {bulkScanResults.errors.map((error, index) => (
                        <p key={index} className="text-xs text-red-600 mb-1">• {error}</p>
                      ))}
                    </div>
                  )}
                </div>
              )}
            </div>

            {/* Modal Footer */}
            <div className="px-6 py-4 border-t border-gray-200 flex items-center justify-end gap-3">
              <button
                onClick={() => {
                  setShowBulkScanModal(false)
                  setBulkBarcodeText('')
                  setBulkScanResults(null)
                }}
                className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all border-2 border-gray-300 text-gray-700 hover:bg-gray-50"
                disabled={bulkScanLoading}
              >
                İptal
              </button>
              <button
                onClick={handleBulkScan}
                className="flex items-center gap-1 px-3 py-1.5 text-sm font-semibold rounded shadow-lg hover:shadow-xl transition-all bg-primary-600 text-white hover:bg-primary-700 disabled:opacity-50 disabled:cursor-not-allowed"
                disabled={bulkScanLoading || !bulkBarcodeText.trim()}
              >
                {bulkScanLoading ? (
                  <>
                    <div className="animate-spin w-4 h-4 border-2 border-white border-t-transparent rounded-full" />
                    <span>Kaydediliyor...</span>
                  </>
                ) : (
                  <>
                    <CheckCircle className="w-5 h-5" />
                    <span>Kaydet</span>
                  </>
                )}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default DocumentDetailPage












