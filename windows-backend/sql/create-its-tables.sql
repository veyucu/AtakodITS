-- AKTBLITSUTS Tablosu
-- ITS Ürün Takip Sistemi için ana tablo

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AKTBLITSUTS]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AKTBLITSUTS] (
        [RECNO] INT IDENTITY(1,1) PRIMARY KEY,
        [HAR_RECNO] INT,
        [TURU] VARCHAR(3),
        [FTIRSIP] CHAR(1) NOT NULL,
        [FATIRS_NO] VARCHAR(15),
        [CARI_KODU] VARCHAR(35),
        [STOK_KODU] VARCHAR(35),
        [GTIN] VARCHAR(15),
        [SERI_NO] VARCHAR(35),
        [MIAD] VARCHAR(25),
        [LOT_NO] VARCHAR(35),
        [URETIM_TARIHI] VARCHAR(25),
        [CARRIER_LABEL] VARCHAR(100),
        [DURUM] VARCHAR(20),
        [BILDIRIM_ID] NVARCHAR(50),
        [BILDIRIM_TARIHI] DATETIME,
        [KAYIT_TARIHI] DATETIME DEFAULT GETDATE(),
        [KULLANICI] VARCHAR(35),
        [MIKTAR] FLOAT
    );

    PRINT '✅ AKTBLITSUTS tablosu oluşturuldu';
END
ELSE
BEGIN
    PRINT '⚠️ AKTBLITSUTS tablosu zaten mevcut';
    
    -- Eğer ID kolonu varsa RECNO'ya dönüştür
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLITSUTS') AND name = 'ID')
    BEGIN
        EXEC sp_rename 'AKTBLITSUTS.ID', 'RECNO', 'COLUMN';
        PRINT '✅ ID kolonu RECNO olarak değiştirildi';
    END
    
    -- HAR_RECNO kolonu yoksa ekle
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLITSUTS') AND name = 'HAR_RECNO')
    BEGIN
        ALTER TABLE AKTBLITSUTS ADD HAR_RECNO INT;
        PRINT '✅ HAR_RECNO kolonu eklendi';
    END
    
    -- MIKTAR kolonu yoksa ekle
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AKTBLITSUTS') AND name = 'MIKTAR')
    BEGIN
        ALTER TABLE AKTBLITSUTS ADD MIKTAR FLOAT;
        PRINT '✅ MIKTAR kolonu eklendi';
    END
END
GO

-- INDEX 1: BILDIRIM_ID için index (bildirime göre sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_BILDIRIM_ID' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_BILDIRIM_ID] 
    ON [dbo].[AKTBLITSUTS] ([BILDIRIM_ID])
    INCLUDE ([DURUM], [BILDIRIM_TARIHI]);
    
    PRINT '✅ BILDIRIM_ID index oluşturuldu';
END
GO

-- INDEX 2: GTIN için index (ürün bazlı sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_GTIN' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_GTIN] 
    ON [dbo].[AKTBLITSUTS] ([GTIN])
    INCLUDE ([SERI_NO], [LOT_NO], [MIAD]);
    
    PRINT '✅ GTIN index oluşturuldu';
END
GO

-- INDEX 3: SERI_NO için index (seri no bazlı arama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_SERI_NO' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_SERI_NO] 
    ON [dbo].[AKTBLITSUTS] ([SERI_NO])
    INCLUDE ([GTIN], [DURUM]);
    
    PRINT '✅ SERI_NO index oluşturuldu';
END
GO

-- INDEX 4: FATIRS_NO için index (fatura bazlı sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_FATIRS_NO' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_FATIRS_NO] 
    ON [dbo].[AKTBLITSUTS] ([FATIRS_NO], [FTIRSIP])
    INCLUDE ([CARI_KODU], [KAYIT_TARIHI]);
    
    PRINT '✅ FATIRS_NO index oluşturuldu';
END
GO

-- INDEX 5: CARI_KODU + STOK_KODU için composite index (müşteri-ürün bazlı sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_CARI_STOK' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_CARI_STOK] 
    ON [dbo].[AKTBLITSUTS] ([CARI_KODU], [STOK_KODU])
    INCLUDE ([GTIN], [SERI_NO], [DURUM]);
    
    PRINT '✅ CARI_KODU + STOK_KODU index oluşturuldu';
END
GO

-- INDEX 6: KAYIT_TARIHI için index (tarih bazlı sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_KAYIT_TARIHI' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_KAYIT_TARIHI] 
    ON [dbo].[AKTBLITSUTS] ([KAYIT_TARIHI] DESC)
    INCLUDE ([DURUM], [KULLANICI]);
    
    PRINT '✅ KAYIT_TARIHI index oluşturuldu';
END
GO

-- INDEX 7: DURUM için index (durum bazlı filtreleme)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_DURUM' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_DURUM] 
    ON [dbo].[AKTBLITSUTS] ([DURUM])
    INCLUDE ([BILDIRIM_ID], [BILDIRIM_TARIHI], [KAYIT_TARIHI]);
    
    PRINT '✅ DURUM index oluşturuldu';
END
GO

-- INDEX 8: KULLANICI için index (kullanıcı bazlı sorgulama)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLITSUTS_KULLANICI' AND object_id = OBJECT_ID('AKTBLITSUTS'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AKTBLITSUTS_KULLANICI] 
    ON [dbo].[AKTBLITSUTS] ([KULLANICI])
    INCLUDE ([KAYIT_TARIHI], [DURUM]);
    
    PRINT '✅ KULLANICI index oluşturuldu';
END
GO

PRINT '✅✅✅ AKTBLITSUTS tablosu ve tüm indexler hazır!';

