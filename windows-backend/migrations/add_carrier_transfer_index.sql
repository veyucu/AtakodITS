-- Koli barkodu sorguları için optimize edilmiş index
-- Tarih: 2024-12-19
-- Sorun: CARRIER_LABEL ile MAX(TRANSFER_ID) sorgusu yavaş

USE NETSIS
GO

-- Mevcut index'i kaldır
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AKTBLPTSTRA_CARRIER_LABEL' AND object_id = OBJECT_ID('AKTBLPTSTRA'))
BEGIN
    DROP INDEX IX_AKTBLPTSTRA_CARRIER_LABEL ON AKTBLPTSTRA
    PRINT 'Eski IX_AKTBLPTSTRA_CARRIER_LABEL kaldırıldı'
END
GO

-- Yeni index: CARRIER_LABEL + TRANSFER_ID (composite) + covering columns
CREATE NONCLUSTERED INDEX IX_AKTBLPTSTRA_CARRIER_LABEL ON AKTBLPTSTRA(CARRIER_LABEL, TRANSFER_ID DESC) 
    INCLUDE (PARENT_CARRIER_LABEL, GTIN, SERIAL_NUMBER, LOT_NUMBER, EXPIRATION_DATE, CONTAINER_TYPE, CARRIER_LEVEL)
GO

PRINT 'Yeni IX_AKTBLPTSTRA_CARRIER_LABEL oluşturuldu (TRANSFER_ID DESC ile)'

-- İstatistikleri güncelle
UPDATE STATISTICS AKTBLPTSTRA WITH FULLSCAN
PRINT 'İstatistikler güncellendi'
GO

-- Kontrol
SELECT 
    i.name AS IndexName,
    STUFF((
        SELECT ', ' + c.name + CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE '' END
        FROM sys.index_columns ic
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS KeyColumns,
    STUFF((
        SELECT ', ' + c.name
        FROM sys.index_columns ic
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
        ORDER BY ic.key_ordinal
        FOR XML PATH('')
    ), 1, 2, '') AS IncludedColumns
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID('AKTBLPTSTRA') AND i.name LIKE '%CARRIER%'

PRINT '
=====================================================
Index güncellendi!

Yeni yapı:
- Key: CARRIER_LABEL, TRANSFER_ID DESC
- Include: PARENT_CARRIER_LABEL, GTIN, SERIAL_NUMBER, LOT_NUMBER, EXPIRATION_DATE, CONTAINER_TYPE, CARRIER_LEVEL

Bu sayede:
1. MAX(TRANSFER_ID) WHERE CARRIER_LABEL = @x çok hızlı olacak
2. Recursive CTE için covering index olacak
====================================================='

