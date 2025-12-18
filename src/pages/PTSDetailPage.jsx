import { useState, useEffect, useMemo } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, Package, ChevronDown, ChevronRight, ArrowUpDown, ArrowUp, ArrowDown } from 'lucide-react'
import {
  useReactTable,
  getCoreRowModel,
  getExpandedRowModel,
  getGroupedRowModel,
  getSortedRowModel,
  flexRender,
} from '@tanstack/react-table'
import apiService from '../services/apiService'
import { getSettings } from '../utils/settingsHelper'

const PTSDetailPage = () => {
  const { transferId } = useParams()
  const navigate = useNavigate()
  
  const [loading, setLoading] = useState(true)
  const [packageData, setPackageData] = useState(null)
  const [products, setProducts] = useState([])
  const [grouping, setGrouping] = useState(['gtin'])
  const [expanded, setExpanded] = useState({})
  const [sorting, setSorting] = useState([])

  useEffect(() => {
    loadPackageDetails()
  }, [transferId])

  // Tüm grupları otomatik aç
  useEffect(() => {
    if (products.length > 0) {
      const expandAll = {}
      const uniqueGtins = [...new Set(products.map(p => p.gtin))]
      uniqueGtins.forEach((gtin, index) => {
        expandAll[`${index}`] = true
      })
      setExpanded(expandAll)
    }
  }, [products])

  const loadPackageDetails = async () => {
    try {
      setLoading(true)
      
      const settings = getSettings()
      const response = await apiService.getPackageFromDB(transferId, settings)
      
      if (response.success && response.data) {
        const data = response.data
        setPackageData(data)
        
        const onlyProducts = (data.products || [])
          .filter(p => p.SERIAL_NUMBER)
          .map(p => ({
            id: p.ID,
            gtin: p.GTIN || '',
            stockName: p.STOK_ADI || '-',
            serialNumber: p.SERIAL_NUMBER,
            lotNumber: p.LOT_NUMBER,
            expirationDate: p.EXPIRATION_DATE ? new Date(p.EXPIRATION_DATE).toLocaleDateString('tr-TR') : '',
            productionDate: p.PRODUCTION_DATE ? new Date(p.PRODUCTION_DATE).toLocaleDateString('tr-TR') : '',
            carrierLabel: p.CARRIER_LABEL,
            containerType: p.CONTAINER_TYPE
          }))
        
        setProducts(onlyProducts)
      }
    } catch (error) {
      console.error('Paket detay yükleme hatası:', error)
    } finally {
      setLoading(false)
    }
  }

  const columns = useMemo(() => [
    {
      accessorKey: 'gtin',
      header: 'GTIN',
      enableSorting: true,
      cell: info => <span className="font-mono font-bold text-blue-900">{info.getValue()}</span>,
      enableGrouping: true,
      aggregatedCell: ({ getValue, row }) => (
        <div className="flex items-center gap-2">
          <button
            onClick={row.getToggleExpandedHandler()}
            className="p-1 hover:bg-blue-200 rounded transition-all duration-200"
          >
            {row.getIsExpanded() ? 
              <ChevronDown className="w-4 h-4 text-blue-700" /> : 
              <ChevronRight className="w-4 h-4 text-blue-700" />
            }
          </button>
          <div className="flex items-center gap-2">
            <span className="font-mono font-bold text-blue-900 text-sm">{getValue()}</span>
            <span className="px-2 py-0.5 bg-gradient-to-r from-blue-500 to-blue-600 text-white rounded-full text-xs font-bold">
              {row.subRows.length} Adet
            </span>
          </div>
        </div>
      ),
    },
    {
      accessorKey: 'stockName',
      header: 'Stok Adı',
      enableSorting: true,
      cell: info => <span className="font-medium text-gray-800">{info.getValue()}</span>,
      aggregatedCell: ({ row }) => (
        <span className="font-semibold text-gray-900 text-sm">{row.subRows[0]?.original.stockName}</span>
      ),
    },
    {
      accessorKey: 'serialNumber',
      header: 'Seri No',
      enableSorting: true,
      cell: info => <span className="font-mono text-red-600 font-bold text-sm">{info.getValue()}</span>,
    },
    {
      accessorKey: 'lotNumber',
      header: 'Lot No',
      enableSorting: true,
      cell: info => <span className="font-mono text-gray-700">{info.getValue() || '-'}</span>,
    },
    {
      accessorKey: 'expirationDate',
      header: 'Son Kullanma Tarihi',
      enableSorting: true,
      cell: info => (
        <span className="px-2 py-1 bg-amber-50 text-amber-800 rounded font-medium text-sm">
          {info.getValue() || '-'}
        </span>
      ),
    },
    {
      accessorKey: 'productionDate',
      header: 'Üretim Tarihi',
      enableSorting: true,
      cell: info => (
        <span className="px-2 py-1 bg-green-50 text-green-800 rounded font-medium text-sm">
          {info.getValue() || '-'}
        </span>
      ),
    },
    {
      accessorKey: 'carrierLabel',
      header: 'Carrier Label',
      enableSorting: true,
      cell: info => (
        <span className="font-mono text-xs text-gray-600 bg-gray-50 px-2 py-1 rounded">
          {info.getValue() || '-'}
        </span>
      ),
    },
  ], [])

  const table = useReactTable({
    data: products,
    columns,
    state: {
      grouping,
      expanded,
      sorting,
    },
    onGroupingChange: setGrouping,
    onExpandedChange: setExpanded,
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getExpandedRowModel: getExpandedRowModel(),
    getGroupedRowModel: getGroupedRowModel(),
    getSortedRowModel: getSortedRowModel(),
    autoResetExpanded: false,
    enableSortingRemoval: false,
  })

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen bg-gray-50">
        <div className="text-center">
          <Package className="w-16 h-16 mx-auto mb-4 text-blue-600 animate-bounce" />
          <p className="text-lg font-medium text-gray-700">Paket detayları yükleniyor...</p>
        </div>
      </div>
    )
  }

  if (!packageData) {
    return (
      <div className="flex items-center justify-center h-screen bg-gray-50">
        <div className="text-center">
          <Package className="w-16 h-16 mx-auto mb-4 text-red-600" />
          <p className="text-lg font-medium text-gray-700">Paket bulunamadı</p>
          <button
            onClick={() => navigate('/pts')}
            className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
          >
            Geri Dön
          </button>
        </div>
      </div>
    )
  }

  return (
    <div className="flex flex-col h-screen bg-gray-50">
      {/* Header - Kompakt */}
      <div className="bg-gradient-to-r from-blue-600 to-cyan-600 text-white shadow-lg">
        <div className="px-6 py-3">
          <div className="flex items-center gap-4">
            <button
              onClick={() => navigate('/pts')}
              className="w-10 h-10 bg-white/20 rounded-lg flex items-center justify-center hover:bg-white/30 transition-colors flex-shrink-0"
            >
              <ArrowLeft className="w-6 h-6" />
            </button>
            <div className="flex-1 flex items-center justify-between">
              <div>
                <h1 className="text-xl font-bold">PTS Paket Detayı</h1>
                <p className="text-blue-100 text-sm">Transfer ID: {transferId}</p>
              </div>
              <div className="flex items-center gap-4 text-sm">
                <div className="bg-white/20 px-3 py-1.5 rounded-lg">
                  <span className="font-semibold">Belge No:</span> {packageData.DOCUMENT_NUMBER || '-'}
                </div>
                <div className="bg-white/20 px-3 py-1.5 rounded-lg">
                  <span className="font-semibold">Belge Tarihi:</span>{' '}
                  {packageData.DOCUMENT_DATE ? new Date(packageData.DOCUMENT_DATE).toLocaleDateString('tr-TR') : '-'}
                </div>
                <div className="bg-white/20 px-3 py-1.5 rounded-lg">
                  <span className="font-semibold">Kaynak GLN:</span>{' '}
                  <span className="font-mono">{packageData.SOURCE_GLN || '-'}</span>
                  {packageData.SOURCE_GLN_NAME && (
                    <span className="ml-2 text-yellow-200">
                      ({packageData.SOURCE_GLN_NAME}
                      {packageData.SOURCE_GLN_ILCE && ` - ${packageData.SOURCE_GLN_ILCE}`}
                      {packageData.SOURCE_GLN_IL && ` / ${packageData.SOURCE_GLN_IL}`})
                    </span>
                  )}
                </div>
                <div className="bg-white/20 px-3 py-1.5 rounded-lg">
                  <span className="font-semibold">Hedef GLN:</span>{' '}
                  <span className="font-mono">{packageData.DESTINATION_GLN || '-'}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* TanStack Table */}
      <div className="flex-1 px-6 py-4 overflow-auto table-container">
        <div className="bg-white rounded-lg shadow-xl border border-gray-200 overflow-hidden">
          <table className="w-full">
            <thead className="bg-gradient-to-r from-blue-600 to-cyan-600 text-white sticky top-0 z-10 shadow-md">
              {table.getHeaderGroups().map(headerGroup => (
                <tr key={headerGroup.id}>
                  {headerGroup.headers.map(header => (
                    <th
                      key={header.id}
                      className="px-4 py-4 text-left text-xs font-bold uppercase tracking-wider"
                    >
                      {header.isPlaceholder ? null : (
                        <div
                          className={`flex items-center gap-2 ${
                            header.column.getCanSort() ? 'cursor-pointer select-none hover:text-yellow-200 transition-colors' : ''
                          }`}
                          onClick={header.column.getToggleSortingHandler()}
                        >
                          {flexRender(header.column.columnDef.header, header.getContext())}
                          {header.column.getCanSort() && (
                            <span className="flex flex-col">
                              {header.column.getIsSorted() === 'asc' ? (
                                <ArrowUp className="w-4 h-4 text-yellow-300" />
                              ) : header.column.getIsSorted() === 'desc' ? (
                                <ArrowDown className="w-4 h-4 text-yellow-300" />
                              ) : (
                                <ArrowUpDown className="w-4 h-4 opacity-50" />
                              )}
                            </span>
                          )}
                        </div>
                      )}
                    </th>
                  ))}
                </tr>
              ))}
            </thead>
            <tbody className="divide-y divide-gray-200">
              {table.getRowModel().rows.map(row => (
                <tr
                  key={row.id}
                  className={`
                    ${row.getIsGrouped() 
                      ? 'tanstack-table-row-group bg-gradient-to-r from-blue-50 to-cyan-50 hover:from-blue-100 hover:to-cyan-100 border-l-4 border-blue-500' 
                      : 'tanstack-table-row-detail hover:bg-gray-50 border-l-4 border-transparent hover:border-blue-300 hover:shadow-sm'
                    }
                    transition-all duration-200
                  `}
                >
                  {row.getVisibleCells().map((cell, index) => (
                    <td
                      key={cell.id}
                      className={`
                        px-4 text-sm
                        ${row.getIsGrouped() ? 'font-semibold py-2' : 'py-2.5'}
                        ${index === 0 && !row.getIsGrouped() ? 'pl-12' : ''}
                      `}
                    >
                      {cell.getIsGrouped() ? (
                        flexRender(cell.column.columnDef.aggregatedCell ?? cell.column.columnDef.cell, cell.getContext())
                      ) : cell.getIsAggregated() ? (
                        flexRender(cell.column.columnDef.aggregatedCell ?? cell.column.columnDef.cell, cell.getContext())
                      ) : cell.getIsPlaceholder() ? null : (
                        flexRender(cell.column.columnDef.cell, cell.getContext())
                      )}
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
            <tfoot className="bg-gradient-to-r from-gray-100 to-gray-200 border-t-2 border-gray-300">
              <tr>
                <td colSpan={table.getVisibleFlatColumns().length} className="px-4 py-3">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-6">
                      <div className="flex items-center gap-2">
                        <span className="text-sm font-semibold text-gray-700">Toplam Satır:</span>
                        <span className="px-3 py-1 bg-gradient-to-r from-blue-500 to-blue-600 text-white rounded-full text-sm font-bold shadow-md">
                          {products.length}
                        </span>
                      </div>
                      <div className="flex items-center gap-2">
                        <span className="text-sm font-semibold text-gray-700">Kalem:</span>
                        <span className="px-3 py-1 bg-gradient-to-r from-green-500 to-green-600 text-white rounded-full text-sm font-bold shadow-md">
                          {new Set(products.map(p => p.gtin)).size}
                        </span>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
    </div>
  )
}

export default PTSDetailPage
