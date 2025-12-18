-- SERIAL_NUMBER kolonunu NULL kabul edecek şekilde güncelle
-- Carrier kayıtları için SERIAL_NUMBER NULL olmalı

USE MUHASEBE2025
GO

PRINT '🔧 SERIAL_NUMBER kolonu güncelleniyor...'
GO

-- Kolon tanımını değiştir - NULL kabul etsin
ALTER TABLE AKTBLPTSTRA
ALTER COLUMN SERIAL_NUMBER NVARCHAR(100) NULL  -- NOT NULL'dan NULL'a çevir
GO

PRINT '✅ SERIAL_NUMBER artık NULL kabul ediyor'
PRINT '📝 Artık carrier kayıtları (SERIAL_NUMBER=NULL) eklenebilir'
GO

-- Kontrol
PRINT ''
PRINT '📊 Kolon Bilgisi:'
GO

SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'AKTBLPTSTRA'
  AND COLUMN_NAME = 'SERIAL_NUMBER'
GO



