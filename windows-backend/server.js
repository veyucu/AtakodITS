import express from 'express';
import cors from 'cors';
import dotenv from 'dotenv';
import documentsRouter from './routes/documents.js';
import ptsRouter from './routes/pts.js';
import itsRouter from './routes/its.js';
import * as ptsDbService from './services/ptsDbService.js';
import * as itsDbService from './services/itsDbService.js';

dotenv.config();

const app = express();
const PORT = process.env.PORT || 5000;

// Middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Health check endpoint
app.get('/api/health', (req, res) => {
  res.json({ 
    status: 'OK', 
    message: 'AtakodITS Backend is running',
    timestamp: new Date().toISOString()
  });
});

// Test endpoint
app.get('/api/test', (req, res) => {
  res.json({ 
    message: 'Test endpoint working!',
    data: {
      version: '1.0.0',
      environment: process.env.NODE_ENV || 'development'
    }
  });
});

// Routes
app.use('/api/documents', documentsRouter);
app.use('/api/pts', ptsRouter);
app.use('/api/its', itsRouter);

// 404 handler
app.use((req, res) => {
  res.status(404).json({ 
    error: 'Not Found',
    message: `Cannot ${req.method} ${req.path}` 
  });
});

// Error handler
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).json({ 
    error: 'Internal Server Error',
    message: err.message 
  });
});

// Server başlatma ve tabloları hazırlama
async function startServer() {
  try {
    // PTS tablolarını oluştur (varsa kontrol et)
    console.log('📋 PTS tabloları kontrol ediliyor...');
    const ptsTablesResult = await ptsDbService.createTablesIfNotExists();
    
    if (ptsTablesResult.success) {
      console.log('✅ PTS tabloları hazır');
    } else {
      console.error('⚠️ PTS tabloları oluşturulamadı:', ptsTablesResult.error);
    }
    
    // ITS tablolarını oluştur (varsa kontrol et)
    console.log('📋 ITS tabloları kontrol ediliyor...');
    const itsTablesResult = await itsDbService.createTablesIfNotExists();
    
    if (itsTablesResult.success) {
      console.log('✅ ITS tabloları hazır');
    } else {
      console.error('⚠️ ITS tabloları oluşturulamadı:', itsTablesResult.error);
    }
    
    // Server'ı başlat
app.listen(PORT, () => {
  console.log(`🚀 Server is running on http://localhost:${PORT}`);
  console.log(`📍 Health check: http://localhost:${PORT}/api/health`);
});
  } catch (error) {
    console.error('❌ Server başlatma hatası:', error);
    process.exit(1);
  }
}

startServer();

