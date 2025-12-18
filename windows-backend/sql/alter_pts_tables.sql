-- PTS Tablolarını Güncelle - Yeni Kolonları Ekle
-- Mevcut verileri koruyarak sadece yeni kolonları ekler

USE MUHASEBE2025
GO

PRINT '🔄 AKTBLPTSTRA tablosuna yeni kolonlar ekleniyor...'
GO

-- PARENT_CARRIER_LABEL kolonu ekle
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLPTSTRA') AND name = 'PARENT_CARRIER_LABEL')
BEGIN
    ALTER TABLE AKTBLPTSTRA ADD PARENT_CARRIER_LABEL NVARCHAR(100) NULL
    PRINT '✅ PARENT_CARRIER_LABEL kolonu eklendi'
END
ELSE
BEGIN
    PRINT '⚠️ PARENT_CARRIER_LABEL kolonu zaten mevcut'
END
GO

-- CARRIER_LEVEL kolonu ekle
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLPTSTRA') AND name = 'CARRIER_LEVEL')
BEGIN
    ALTER TABLE AKTBLPTSTRA ADD CARRIER_LEVEL INT NULL
    PRINT '✅ CARRIER_LEVEL kolonu eklendi'
END
ELSE
BEGIN
    PRINT '⚠️ CARRIER_LEVEL kolonu zaten mevcut'
END
GO

-- Index ekle - PARENT_CARRIER_LABEL
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLPTSTRA_PARENT_CARRIER_LABEL')
BEGIN
    CREATE INDEX IX_AKTBLPTSTRA_PARENT_CARRIER_LABEL ON AKTBLPTSTRA(PARENT_CARRIER_LABEL)
    PRINT '✅ PARENT_CARRIER_LABEL index''i oluşturuldu'
END
ELSE
BEGIN
    PRINT '⚠️ PARENT_CARRIER_LABEL index''i zaten mevcut'
END
GO

-- Index ekle - CARRIER_LABEL (yoksa)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLPTSTRA_CARRIER_LABEL')
BEGIN
    CREATE INDEX IX_AKTBLPTSTRA_CARRIER_LABEL ON AKTBLPTSTRA(CARRIER_LABEL)
    PRINT '✅ CARRIER_LABEL index''i oluşturuldu'
END
ELSE
BEGIN
    PRINT '⚠️ CARRIER_LABEL index''i zaten mevcut'
END
GO

PRINT ''
PRINT '✅ Tablo güncelleme tamamlandı!'
PRINT '📝 Yeni kolonlar:'
PRINT '   - PARENT_CARRIER_LABEL: Üst carrier barkodu'
PRINT '   - CARRIER_LEVEL: Hiyerarşi seviyesi'
PRINT ''
PRINT '🔄 Artık yeni paketler hiyerarşik yapıda kaydedilecek'
GO

-- Tablo yapısını göster
EXEC sp_help 'AKTBLPTSTRA'
GO



