-- =====================================================
-- PTS CARRIER (KOLİ/PALET/BAĞ) SORGULAMA ÖRNEKLERİ
-- Kullanıcı herhangi bir barkod okuttuğunda kullanılacak sorgular
-- =====================================================

USE MUHASEBE2025
GO

-- =====================================================
-- 1. OKUTULAN BARKODU BUL (Koli/Palet/Bağ olabilir)
-- =====================================================
PRINT '🔍 Barkod Arama (Örnek):'
PRINT '======================='
GO

DECLARE @OkutulanBarkod NVARCHAR(100)

-- Test için bir barkod al
SELECT TOP 1 @OkutulanBarkod = CARRIER_LABEL 
FROM AKTBLPTSTRA 
WHERE CARRIER_LABEL IS NOT NULL 
  AND SERIAL_NUMBER IS NULL  -- Carrier kaydı (ürün değil)
ORDER BY CREATED_DATE DESC

PRINT 'Okutulan Barkod: ' + ISNULL(@OkutulanBarkod, 'YOK')
PRINT ''

IF @OkutulanBarkod IS NOT NULL
BEGIN
    -- Barkod bilgisini getir
    SELECT 
        CARRIER_LABEL AS BARKOD,
        CONTAINER_TYPE AS TIP,
        CARRIER_LEVEL AS SEVIYE,
        PARENT_CARRIER_LABEL AS UST_CARRIER,
        CASE 
            WHEN SERIAL_NUMBER IS NULL THEN 'CARRIER (Koli/Palet/Bağ)'
            ELSE 'URUN'
        END AS KAYIT_TIPI
    FROM AKTBLPTSTRA
    WHERE CARRIER_LABEL = @OkutulanBarkod
      AND SERIAL_NUMBER IS NULL  -- Carrier'ın kendisi
    
    PRINT ''
    PRINT '✅ Barkod bulundu!'
    PRINT ''
END
ELSE
BEGIN
    PRINT '⚠️ Test için carrier bulunamadı. Önce paket indirin.'
END
GO

-- =====================================================
-- 2. OKUTULAN BARKODUN İÇİNDEKİ TÜM ÜRÜNLERİ GETİR
-- Recursive: Alt koliler, bağlar ve tüm ürünler
-- =====================================================
PRINT '📦 Barkod İçindeki Tüm Ürünler (Recursive):'
PRINT '==========================================='
GO

DECLARE @OkutulanBarkod2 NVARCHAR(100)

SELECT TOP 1 @OkutulanBarkod2 = CARRIER_LABEL 
FROM AKTBLPTSTRA 
WHERE CARRIER_LABEL IS NOT NULL 
  AND SERIAL_NUMBER IS NULL
ORDER BY CREATED_DATE DESC

IF @OkutulanBarkod2 IS NOT NULL
BEGIN
    ;WITH CarrierHierarchy AS (
        -- Root: Okutulan barkod
        SELECT 
            ID,
            TRANSFER_ID,
            CARRIER_LABEL,
            PARENT_CARRIER_LABEL,
            CONTAINER_TYPE,
            CARRIER_LEVEL,
            GTIN,
            SERIAL_NUMBER,
            LOT_NUMBER,
            EXPIRATION_DATE,
            0 AS DEPTH,
            CAST(CARRIER_LABEL AS NVARCHAR(500)) AS PATH
        FROM AKTBLPTSTRA
        WHERE CARRIER_LABEL = @OkutulanBarkod2
        
        UNION ALL
        
        -- Recursive: Alt carrier'lar ve ürünler
        SELECT 
            t.ID,
            t.TRANSFER_ID,
            t.CARRIER_LABEL,
            t.PARENT_CARRIER_LABEL,
            t.CONTAINER_TYPE,
            t.CARRIER_LEVEL,
            t.GTIN,
            t.SERIAL_NUMBER,
            t.LOT_NUMBER,
            t.EXPIRATION_DATE,
            ch.DEPTH + 1,
            CAST(ch.PATH + ' -> ' + ISNULL(t.CARRIER_LABEL, '[Ürün]') AS NVARCHAR(500))
        FROM AKTBLPTSTRA t
        INNER JOIN CarrierHierarchy ch ON t.PARENT_CARRIER_LABEL = ch.CARRIER_LABEL
    )
    SELECT 
        DEPTH AS SEVIYE,
        CASE 
            WHEN SERIAL_NUMBER IS NULL THEN 'CARRIER'
            ELSE 'URUN'
        END AS TIP,
        CONTAINER_TYPE,
        CARRIER_LABEL AS BARKOD,
        GTIN,
        SERIAL_NUMBER AS SERI_NO,
        LOT_NUMBER AS LOT,
        EXPIRATION_DATE AS SKT,
        PATH AS HIYERARSI_YOLU
    FROM CarrierHierarchy
    ORDER BY DEPTH, CARRIER_LEVEL, ID
    
    PRINT ''
    PRINT '--- ÖZET ---'
    
    -- Toplam ürün sayısı
    SELECT 
        @OkutulanBarkod2 AS OKUTULAN_BARKOD,
        COUNT(*) AS TOPLAM_KAYIT,
        SUM(CASE WHEN SERIAL_NUMBER IS NULL THEN 1 ELSE 0 END) AS CARRIER_SAYISI,
        SUM(CASE WHEN SERIAL_NUMBER IS NOT NULL THEN 1 ELSE 0 END) AS URUN_SAYISI
    FROM (
        SELECT * FROM CarrierHierarchy
    ) AS Results
    
END
GO

-- =====================================================
-- 3. SADECE ÜRÜNLERİ GETİR (Satış için)
-- =====================================================
PRINT ''
PRINT '🛒 Satış İçin Ürün Listesi:'
PRINT '=========================='
GO

DECLARE @OkutulanBarkod3 NVARCHAR(100)

SELECT TOP 1 @OkutulanBarkod3 = CARRIER_LABEL 
FROM AKTBLPTSTRA 
WHERE CARRIER_LABEL IS NOT NULL 
  AND SERIAL_NUMBER IS NULL
ORDER BY CREATED_DATE DESC

IF @OkutulanBarkod3 IS NOT NULL
BEGIN
    ;WITH CarrierHierarchy AS (
        SELECT 
            ID, TRANSFER_ID, CARRIER_LABEL, PARENT_CARRIER_LABEL,
            GTIN, SERIAL_NUMBER, LOT_NUMBER, EXPIRATION_DATE, PRODUCTION_DATE
        FROM AKTBLPTSTRA
        WHERE CARRIER_LABEL = @OkutulanBarkod3
        
        UNION ALL
        
        SELECT 
            t.ID, t.TRANSFER_ID, t.CARRIER_LABEL, t.PARENT_CARRIER_LABEL,
            t.GTIN, t.SERIAL_NUMBER, t.LOT_NUMBER, t.EXPIRATION_DATE, t.PRODUCTION_DATE
        FROM AKTBLPTSTRA t
        INNER JOIN CarrierHierarchy ch ON t.PARENT_CARRIER_LABEL = ch.CARRIER_LABEL
    )
    SELECT 
        ROW_NUMBER() OVER (ORDER BY ID) AS SIRA,
        GTIN,
        SERIAL_NUMBER AS SERI_NO,
        LOT_NUMBER AS LOT,
        EXPIRATION_DATE AS SKT,
        PRODUCTION_DATE AS URETIM_TARIHI,
        CARRIER_LABEL AS BULUNDUGU_CARRIER
    FROM CarrierHierarchy
    WHERE SERIAL_NUMBER IS NOT NULL  -- Sadece ürünler
    ORDER BY ID
END
GO

-- =====================================================
-- 4. TABLO YAPISINI GÖSTER
-- Carrier kayıtları vs Ürün kayıtları
-- =====================================================
PRINT ''
PRINT '📊 Tablo Yapısı Özeti:'
PRINT '===================='
GO

SELECT 
    'TOPLAM KAYIT' AS KATEGORI,
    COUNT(*) AS ADET
FROM AKTBLPTSTRA

UNION ALL

SELECT 
    'CARRIER KAYITLARI (Koli/Palet/Bağ)',
    COUNT(*)
FROM AKTBLPTSTRA
WHERE SERIAL_NUMBER IS NULL

UNION ALL

SELECT 
    'URUN KAYITLARI (Seri No var)',
    COUNT(*)
FROM AKTBLPTSTRA
WHERE SERIAL_NUMBER IS NOT NULL

UNION ALL

SELECT 
    'Palet (P)',
    COUNT(DISTINCT CARRIER_LABEL)
FROM AKTBLPTSTRA
WHERE CONTAINER_TYPE = 'P' AND SERIAL_NUMBER IS NULL

UNION ALL

SELECT 
    'Koli (C)',
    COUNT(DISTINCT CARRIER_LABEL)
FROM AKTBLPTSTRA
WHERE CONTAINER_TYPE = 'C' AND SERIAL_NUMBER IS NULL

UNION ALL

SELECT 
    'Bağ (S)',
    COUNT(DISTINCT CARRIER_LABEL)
FROM AKTBLPTSTRA
WHERE CONTAINER_TYPE = 'S' AND SERIAL_NUMBER IS NULL
GO

-- =====================================================
-- 5. ÖRNEK CARRIER HİYERARŞİSİ
-- =====================================================
PRINT ''
PRINT '🌳 Örnek Carrier Hiyerarşisi:'
PRINT '============================'
GO

SELECT TOP 20
    TRANSFER_ID,
    REPLICATE('  ', ISNULL(CARRIER_LEVEL, 1) - 1) + 
    ISNULL(CARRIER_LABEL, 'ROOT') + 
    ' [' + ISNULL(CONTAINER_TYPE, '?') + ']' +
    CASE 
        WHEN SERIAL_NUMBER IS NULL THEN ' (CARRIER)'
        ELSE ' -> Ürün: ' + SERIAL_NUMBER
    END AS HIYERARSI,
    CARRIER_LEVEL AS LVL,
    PARENT_CARRIER_LABEL AS PARENT
FROM AKTBLPTSTRA
ORDER BY TRANSFER_ID DESC, CARRIER_LEVEL, CARRIER_LABEL, ID
GO

PRINT ''
PRINT '✅ Sorgular tamamlandı!'
PRINT ''
PRINT '💡 ÖNEMLİ NOTLAR:'
PRINT '   - Her carrier (koli/palet/bağ) için SERIAL_NUMBER=NULL olan ayrı kayıt var'
PRINT '   - Her ürün için SERIAL_NUMBER dolu olan kayıt var'
PRINT '   - Kullanıcı hangi barkodu okuttuğunu bilmiyoruz, sistem otomatik buluyor'
PRINT '   - Recursive CTE ile tüm alt carrier ve ürünler getiriliyor'
GO



