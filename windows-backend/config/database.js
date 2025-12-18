import sql from 'mssql'

// Ana veritabanı config (MUHASEBE2025) - ITS, UTS, Belgeler için
const mainConfig = {
  server: 'NB2',
  database: 'MUHASEBE2025',
  user: 'sa',
  password: 'sapass1*',
  options: {
    encrypt: false,
    trustServerCertificate: true,
    enableArithAbort: true,
    useUTC: false
  },
  pool: {
    max: 10,
    min: 0,
    idleTimeoutMillis: 30000
  },
  connectionTimeout: 30000,
  requestTimeout: 60000,
  beforeConnect: (conn) => {
    conn.on('connect', (err) => {
      if (!err) {
        console.log('SQL Server bağlantısı kuruldu - Türkçe karakter desteği aktif')
      }
    })
  }
}

// PTS veritabanı config (NETSIS) - Sadece PTS işlemleri için
const ptsConfig = {
  server: 'NB2',
  database: 'NETSIS',
  user: 'sa',
  password: 'sapass1*',
  options: {
    encrypt: false,
    trustServerCertificate: true,
    enableArithAbort: true,
    useUTC: false
  },
  pool: {
    max: 10,
    min: 0,
    idleTimeoutMillis: 30000
  },
  connectionTimeout: 30000,
  requestTimeout: 60000,
  beforeConnect: (conn) => {
    conn.on('connect', (err) => {
      if (!err) {
        console.log('PTS SQL Server bağlantısı kuruldu - Türkçe karakter desteği aktif')
      }
    })
  }
}

let mainPool = null
let ptsPool = null

// Ana veritabanı bağlantısı (MUHASEBE2025)
export const getConnection = async () => {
  try {
    if (!mainPool) {
      mainPool = await sql.connect(mainConfig)
      console.log(`✅ SQL Server bağlantısı başarılı (${mainConfig.database})`)
    }
    return mainPool
  } catch (error) {
    console.error(`❌ SQL Server bağlantı hatası (${mainConfig.database}):`, error)
    throw error
  }
}

// PTS veritabanı bağlantısı (NETSIS)
export const getPTSConnection = async () => {
  try {
    if (!ptsPool) {
      ptsPool = await new sql.ConnectionPool(ptsConfig).connect()
      console.log(`✅ PTS SQL Server bağlantısı başarılı (${ptsConfig.database})`)
    }
    return ptsPool
  } catch (error) {
    console.error(`❌ PTS SQL Server bağlantı hatası (${ptsConfig.database}):`, error)
    throw error
  }
}

export const closeConnection = async () => {
  try {
    if (mainPool) {
      await mainPool.close()
      mainPool = null
      console.log(`${mainConfig.database} bağlantısı kapatıldı`)
    }
    if (ptsPool) {
      await ptsPool.close()
      ptsPool = null
      console.log(`${ptsConfig.database} bağlantısı kapatıldı`)
    }
  } catch (error) {
    console.error('Bağlantı kapatma hatası:', error)
  }
}

export default { getConnection, getPTSConnection, closeConnection }

