-- PTS Tablolarını Hiyerarşik Yapıya Güncelleme
-- Bu script mevcut AKTBLPTSTRA tablosunu yedekleyip yeni yapıya güncelleyecek

USE MUHASEBE2025
GO

PRINT '🔄 PTS Tablolarını güncelleme başlıyor...'
GO

-- 1. Mevcut veri varsa yedekle
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'AKTBLPTSTRA') AND type in (N'U'))
BEGIN
    PRINT '💾 Mevcut AKTBLPTSTRA tablosu yedekleniyor...'
    
    -- Backup tablosu oluştur (yoksa)
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'AKTBLPTSTRA_BACKUP') AND type in (N'U'))
    BEGIN
        SELECT * INTO AKTBLPTSTRA_BACKUP FROM AKTBLPTSTRA
        PRINT '✅ Yedek tablo oluşturuldu: AKTBLPTSTRA_BACKUP'
    END
    
    -- Eski tabloyu sil
    DROP TABLE AKTBLPTSTRA
    PRINT '🗑️ Eski tablo silindi'
END
GO

-- 2. Yeni yapıda tabloyu oluştur
PRINT '📋 Yeni AKTBLPTSTRA tablosu oluşturuluyor...'
GO

CREATE TABLE AKTBLPTSTRA (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TRANSFER_ID BIGINT NOT NULL,
    
    -- Carrier Hiyerarşi Bilgileri
    CARRIER_LABEL NVARCHAR(100) NULL,          -- Bu carrier'ın barkod numarası (20 karakter)
    PARENT_CARRIER_LABEL NVARCHAR(100) NULL,   -- Üst carrier'ın barkodu (NULL ise root level)
    CONTAINER_TYPE NVARCHAR(10) NULL,           -- P:Palet, C:Koli, S:Bağ, B:Koli içi kutu, E:Küçük bağ
    CARRIER_LEVEL INT NULL,                     -- Hiyerarşi seviyesi (1, 2, 3...)
    
    -- Ürün Bilgileri
    GTIN NVARCHAR(50) NULL,                     -- Ürün GTIN kodu
    SERIAL_NUMBER NVARCHAR(100) NULL,           -- Ürün seri numarası
    LOT_NUMBER NVARCHAR(50) NULL,               -- Lot numarası
    EXPIRATION_DATE DATE NULL,                  -- Son kullanma tarihi
    PRODUCTION_DATE DATE NULL,                  -- Üretim tarihi
    PO_NUMBER NVARCHAR(50) NULL,                -- Sipariş numarası
    
    CREATED_DATE DATETIME DEFAULT GETDATE()
)
GO

-- 3. Index'leri oluştur
PRINT '📑 Index''ler oluşturuluyor...'
GO

CREATE INDEX IX_AKTBLPTSTRA_TRANSFER_ID ON AKTBLPTSTRA(TRANSFER_ID)
CREATE INDEX IX_AKTBLPTSTRA_CARRIER_LABEL ON AKTBLPTSTRA(CARRIER_LABEL)
CREATE INDEX IX_AKTBLPTSTRA_PARENT_CARRIER_LABEL ON AKTBLPTSTRA(PARENT_CARRIER_LABEL)
CREATE INDEX IX_AKTBLPTSTRA_GTIN ON AKTBLPTSTRA(GTIN)
CREATE INDEX IX_AKTBLPTSTRA_SERIAL_NUMBER ON AKTBLPTSTRA(SERIAL_NUMBER)
CREATE INDEX IX_AKTBLPTSTRA_EXPIRATION_DATE ON AKTBLPTSTRA(EXPIRATION_DATE)
GO

-- 4. Foreign Key Constraint
PRINT '🔗 Foreign Key oluşturuluyor...'
GO

ALTER TABLE AKTBLPTSTRA
ADD CONSTRAINT FK_AKTBLPTSTRA_TRANSFER_ID 
FOREIGN KEY (TRANSFER_ID) REFERENCES AKTBLPTSMAS(TRANSFER_ID)
GO

PRINT '✅ PTS Tabloları başarıyla güncellendi!'
PRINT '📝 Not: Eski veriler AKTBLPTSTRA_BACKUP tablosunda saklanıyor'
PRINT '🔄 Yeni paket indirmeleri hiyerarşik yapıda kaydedilecek'
GO

-- Tablo yapısını göster
PRINT ''
PRINT '📊 Yeni Tablo Yapısı:'
GO
EXEC sp_help 'AKTBLPTSTRA'
GO



