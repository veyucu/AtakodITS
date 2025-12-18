-- PTS Carrier Hiyerarşi Test Sorguları

USE MUHASEBE2025
GO

-- =====================================================
-- 1. TÜM CARRIER HİYERARŞİSİNİ GÖRÜNTÜLE
-- =====================================================
PRINT '📦 Tüm Carrier Hiyerarşisi:'
PRINT '============================'
GO

SELECT 
    TRANSFER_ID,
    REPLICATE('  ', ISNULL(CARRIER_LEVEL, 1) - 1) + 
    ISNULL(CARRIER_LABEL, 'NULL') AS HIERARCHY_TREE,
    CONTAINER_TYPE,
    CARRIER_LEVEL,
    PARENT_CARRIER_LABEL,
    COUNT(CASE WHEN SERIAL_NUMBER IS NOT NULL THEN 1 END) AS PRODUCT_COUNT
FROM AKTBLPTSTRA
GROUP BY 
    TRANSFER_ID,
    CARRIER_LABEL, 
    PARENT_CARRIER_LABEL, 
    CONTAINER_TYPE, 
    CARRIER_LEVEL
ORDER BY TRANSFER_ID, CARRIER_LEVEL, CARRIER_LABEL
GO

-- =====================================================
-- 2. BELİRLİ BİR KOLİNİN İÇİNDEKİ TÜM ÜRÜNLERİ GETİR
-- =====================================================
PRINT ''
PRINT '🔍 Belirli Bir Kolinin İçindeki Ürünler (Recursive):'
PRINT '===================================================='
GO

-- Örnek carrier label (ilk carrier'ı bul ve kullan)
DECLARE @TestCarrierLabel NVARCHAR(100)
SELECT TOP 1 @TestCarrierLabel = CARRIER_LABEL 
FROM AKTBLPTSTRA 
WHERE CARRIER_LABEL IS NOT NULL 
  AND CONTAINER_TYPE IN ('P', 'C')
ORDER BY CREATED_DATE DESC

PRINT 'Test Carrier Label: ' + ISNULL(@TestCarrierLabel, 'NULL')
PRINT ''

IF @TestCarrierLabel IS NOT NULL
BEGIN
    ;WITH CarrierHierarchy AS (
        -- Root: Aranan carrier
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
            0 AS DEPTH
        FROM AKTBLPTSTRA
        WHERE CARRIER_LABEL = @TestCarrierLabel
        
        UNION ALL
        
        -- Recursive: Alt carrier'lar
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
            ch.DEPTH + 1
        FROM AKTBLPTSTRA t
        INNER JOIN CarrierHierarchy ch ON t.PARENT_CARRIER_LABEL = ch.CARRIER_LABEL
    )
    SELECT 
        DEPTH,
        REPLICATE('  ', DEPTH) + ISNULL(CARRIER_LABEL, '[Ürün]') AS HIERARCHY,
        CONTAINER_TYPE,
        CARRIER_LEVEL,
        SERIAL_NUMBER,
        GTIN,
        LOT_NUMBER,
        EXPIRATION_DATE
    FROM CarrierHierarchy
    ORDER BY DEPTH, CARRIER_LEVEL, ID
END
ELSE
BEGIN
    PRINT '⚠️ Test için carrier bulunamadı. Önce paket indirin.'
END
GO

-- =====================================================
-- 3. TRANSFER İSTATİSTİKLERİ
-- =====================================================
PRINT ''
PRINT '📊 Transfer İstatistikleri:'
PRINT '=========================='
GO

SELECT 
    M.TRANSFER_ID,
    M.DOCUMENT_NUMBER,
    M.DOCUMENT_DATE,
    M.SOURCE_GLN,
    M.DESTINATION_GLN,
    COUNT(DISTINCT T.CARRIER_LABEL) AS TOTAL_CARRIERS,
    COUNT(DISTINCT CASE WHEN T.CONTAINER_TYPE = 'P' THEN T.CARRIER_LABEL END) AS PALET_COUNT,
    COUNT(DISTINCT CASE WHEN T.CONTAINER_TYPE = 'C' THEN T.CARRIER_LABEL END) AS KOLI_COUNT,
    COUNT(CASE WHEN T.SERIAL_NUMBER IS NOT NULL THEN 1 END) AS PRODUCT_COUNT,
    M.CREATED_DATE
FROM AKTBLPTSMAS M
LEFT JOIN AKTBLPTSTRA T ON M.TRANSFER_ID = T.TRANSFER_ID
GROUP BY 
    M.TRANSFER_ID,
    M.DOCUMENT_NUMBER,
    M.DOCUMENT_DATE,
    M.SOURCE_GLN,
    M.DESTINATION_GLN,
    M.CREATED_DATE
ORDER BY M.CREATED_DATE DESC
GO

-- =====================================================
-- 4. BİR PALETİN İÇİNDEKİ TÜM KOLİLERİ LİSTELE
-- =====================================================
PRINT ''
PRINT '📦 Palet İçindeki Koliler:'
PRINT '========================='
GO

SELECT 
    p.TRANSFER_ID,
    p.CARRIER_LABEL AS PALET_BARCODE,
    p.CONTAINER_TYPE AS PALET_TYPE,
    c.CARRIER_LABEL AS KOLI_BARCODE,
    c.CONTAINER_TYPE AS KOLI_TYPE,
    c.CARRIER_LEVEL,
    COUNT(CASE WHEN c.SERIAL_NUMBER IS NOT NULL THEN 1 END) AS KOLI_PRODUCT_COUNT
FROM AKTBLPTSTRA p
LEFT JOIN AKTBLPTSTRA c ON c.PARENT_CARRIER_LABEL = p.CARRIER_LABEL
WHERE p.CONTAINER_TYPE = 'P'
GROUP BY 
    p.TRANSFER_ID,
    p.CARRIER_LABEL,
    p.CONTAINER_TYPE,
    c.CARRIER_LABEL,
    c.CONTAINER_TYPE,
    c.CARRIER_LEVEL
ORDER BY p.TRANSFER_ID, p.CARRIER_LABEL, c.CARRIER_LABEL
GO

-- =====================================================
-- 5. BİR ÜRÜNÜn HANGİ KOLİDE OLDUĞUNU BUL
-- =====================================================
PRINT ''
PRINT '🔍 Ürünün Hangi Kolide Olduğunu Bulma:'
PRINT '======================================'
GO

-- Test için bir seri numarası bul
DECLARE @TestSerial NVARCHAR(100)
SELECT TOP 1 @TestSerial = SERIAL_NUMBER 
FROM AKTBLPTSTRA 
WHERE SERIAL_NUMBER IS NOT NULL
ORDER BY CREATED_DATE DESC

PRINT 'Test Serial Number: ' + ISNULL(@TestSerial, 'NULL')
PRINT ''

IF @TestSerial IS NOT NULL
BEGIN
    ;WITH CarrierPath AS (
        -- Root: Ürünün olduğu carrier
        SELECT 
            CARRIER_LABEL,
            PARENT_CARRIER_LABEL,
            CONTAINER_TYPE,
            CARRIER_LEVEL,
            SERIAL_NUMBER,
            GTIN,
            CAST(CARRIER_LABEL AS NVARCHAR(500)) AS PATH,
            0 AS LEVEL_UP
        FROM AKTBLPTSTRA
        WHERE SERIAL_NUMBER = @TestSerial
        
        UNION ALL
        
        -- Parent'a çık
        SELECT 
            p.CARRIER_LABEL,
            p.PARENT_CARRIER_LABEL,
            p.CONTAINER_TYPE,
            p.CARRIER_LEVEL,
            cp.SERIAL_NUMBER,
            cp.GTIN,
            CAST(p.CARRIER_LABEL + ' <- ' + cp.PATH AS NVARCHAR(500)),
            cp.LEVEL_UP + 1
        FROM AKTBLPTSTRA p
        INNER JOIN CarrierPath cp ON p.CARRIER_LABEL = cp.PARENT_CARRIER_LABEL
    )
    SELECT 
        SERIAL_NUMBER,
        GTIN,
        CARRIER_LABEL AS TOP_CARRIER,
        CONTAINER_TYPE AS TOP_CONTAINER_TYPE,
        PATH AS FULL_PATH,
        LEVEL_UP
    FROM CarrierPath
    ORDER BY LEVEL_UP DESC
END
ELSE
BEGIN
    PRINT '⚠️ Test için ürün bulunamadı. Önce paket indirin.'
END
GO

PRINT ''
PRINT '✅ Test sorguları tamamlandı!'
GO



