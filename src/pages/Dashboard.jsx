import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { 
  Package, 
  LogOut, 
  User, 
  ArrowRight,
  Truck,
  Settings
} from 'lucide-react'

const Dashboard = () => {
  const navigate = useNavigate()
  const { user, logout } = useAuth()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-4">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 bg-primary-600 rounded-lg flex items-center justify-center">
                <Package className="w-6 h-6 text-white" />
              </div>
              <div>
                <h1 className="text-2xl font-bold text-gray-900">AtakodITS</h1>
                <p className="text-sm text-gray-500">Ürün Hazırlama Sistemi</p>
              </div>
            </div>

            <div className="flex items-center gap-4">
              <div className="flex items-center gap-2 px-4 py-2 bg-gray-50 rounded-lg">
                <User className="w-5 h-5 text-gray-600" />
                <div>
                  <p className="text-sm font-medium text-gray-900">{user?.name}</p>
                  <p className="text-xs text-gray-500">{user?.role}</p>
                </div>
              </div>
              
              <button
                onClick={handleLogout}
                className="flex items-center gap-2 px-4 py-2 text-sm font-medium text-red-600 hover:bg-red-50 rounded-lg transition-colors"
              >
                <LogOut className="w-4 h-4" />
                Çıkış
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        {/* Quick Actions */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <button
            onClick={() => navigate('/documents')}
            className="bg-white rounded-xl shadow-md hover:shadow-xl transition-all p-8 text-left group border-2 border-transparent hover:border-purple-200"
          >
            <div className="flex flex-col">
              <div className="flex items-center justify-between mb-4">
                <div className="w-16 h-16 bg-purple-100 rounded-xl flex items-center justify-center group-hover:bg-purple-600 transition-colors">
                  <Package className="w-8 h-8 text-purple-600 group-hover:text-white transition-colors" />
                </div>
                <ArrowRight className="w-6 h-6 text-gray-400 group-hover:text-purple-600 transition-colors" />
                </div>
                <div>
                  <h3 className="text-xl font-bold text-gray-900 mb-1">Ürün Hazırlama</h3>
                  <p className="text-sm text-gray-500">Ürün hazırlama işlemlerini başlat</p>
                </div>
            </div>
          </button>

          <button
            onClick={() => navigate('/pts')}
            className="bg-white rounded-xl shadow-md hover:shadow-xl transition-all p-8 text-left group border-2 border-transparent hover:border-blue-200"
          >
            <div className="flex flex-col">
              <div className="flex items-center justify-between mb-4">
                <div className="w-16 h-16 bg-blue-100 rounded-xl flex items-center justify-center group-hover:bg-blue-600 transition-colors">
                  <Truck className="w-8 h-8 text-blue-600 group-hover:text-white transition-colors" />
                </div>
                <ArrowRight className="w-6 h-6 text-gray-400 group-hover:text-blue-600 transition-colors" />
                </div>
                <div>
                  <h3 className="text-xl font-bold text-gray-900 mb-1">PTS</h3>
                  <p className="text-sm text-gray-500">Paket Transfer Sistemi</p>
                </div>
              </div>
          </button>

          <button
            onClick={() => navigate('/settings')}
            className="bg-white rounded-xl shadow-md hover:shadow-xl transition-all p-8 text-left group border-2 border-transparent hover:border-orange-200"
          >
            <div className="flex flex-col">
              <div className="flex items-center justify-between mb-4">
                <div className="w-16 h-16 bg-orange-100 rounded-xl flex items-center justify-center group-hover:bg-orange-600 transition-colors">
                  <Settings className="w-8 h-8 text-orange-600 group-hover:text-white transition-colors" />
                </div>
                <ArrowRight className="w-6 h-6 text-gray-400 group-hover:text-orange-600 transition-colors" />
              </div>
              <div>
                <h3 className="text-xl font-bold text-gray-900 mb-1">Ayarlar</h3>
                <p className="text-sm text-gray-500">ITS ve ERP entegrasyon ayarları</p>
              </div>
            </div>
          </button>
        </div>
      </main>
    </div>
  )
}

export default Dashboard
