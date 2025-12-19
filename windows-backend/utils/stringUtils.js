import iconv from 'iconv-lite'

/**
 * Türkçe karakter düzeltme fonksiyonu - SQL Server CP1254 to UTF-8
 * @param {string} str - Düzeltilecek metin
 * @returns {string} - Düzeltilmiş metin
 */
export const fixTurkishChars = (str) => {
  if (!str || typeof str !== 'string') return str
  
  try {
    let fixed = str
    
    // SQL Server'dan gelen yanlış encoded metni düzelt
    // CP1254 (Turkish) -> UTF-8 dönüşümü
    try {
      // Önce latin1 olarak encode edip cp1254 olarak decode et
      const buf = Buffer.from(fixed, 'latin1')
      fixed = iconv.decode(buf, 'cp1254')
    } catch (e) {
      // Sessizce devam et
    }
    
    // Hala ? veya bozuk karakterler varsa manuel düzelt
    if (fixed.includes('?') || fixed.match(/[\u0080-\u00FF]/)) {
      // Karakter karakter düzeltme - SQL Server'dan gelen bozuk karakterler
      const charMap = {
        // UTF-8 çift byte sorunları
        'Ä°': 'İ', 'Ä±': 'ı',
        'ÅŸ': 'ş', 'Åž': 'Ş',
        'Ã§': 'ç', 'Ã‡': 'Ç',
        'ÄŸ': 'ğ', 'Äž': 'Ğ',
        'Ã¼': 'ü', 'Ãœ': 'Ü',
        'Ã¶': 'ö', 'Ã–': 'Ö',
        'Â': '',
        '�': '',
        // Single character replacements from CP1254
        '\u00DD': 'İ', // İ
        '\u00FD': 'ı', // ı  
        '\u00DE': 'Ş', // Ş
        '\u00FE': 'ş', // ş
        '\u00D0': 'Ğ', // Ğ
        '\u00F0': 'ğ', // ğ
      }
      
      for (const [wrong, correct] of Object.entries(charMap)) {
        fixed = fixed.split(wrong).join(correct)
      }
    }
    
    // ? karakteri context'e göre düzelt
    // Türkçe kelimelerde ? genelde şu karakterlerdir: ğ, ı, ş, ç, ö, ü, İ
    fixed = fixed
      .replace(/\?([AEIOU])/g, 'İ$1') // ?A, ?E -> İA, İE (ISTANBUL -> İSTANBUL)
      .replace(/([BCDFGHJKLMNPQRSTVWXYZ])\?/g, '$1İ') // Y? -> Yİ (KAYSER? -> KAYSERİ)
      .replace(/\?([bcdfghjklmnpqrstvwxyz])/g, 'ı$1') // ?n -> ın
      .replace(/([bcdfghjklmnpqrstvwxyz])\?([aeiou])/g, '$1ı$2') // n?a -> nıa
    
    // Başındaki nokta ve gereksiz boşlukları temizle
    fixed = fixed.replace(/^\.+/, '').trim()
    
    return fixed
  } catch (error) {
    console.error('Türkçe karakter düzeltme hatası:', error)
    return str
  }
}

/**
 * Objedeki tüm string alanları düzelt
 * @param {Object} obj - Düzeltilecek obje
 * @returns {Object} - Düzeltilmiş obje
 */
export const fixObjectStrings = (obj) => {
  if (!obj) return obj
  
  const fixed = {}
  for (const [key, value] of Object.entries(obj)) {
    if (typeof value === 'string') {
      fixed[key] = fixTurkishChars(value)
    } else if (Array.isArray(value)) {
      fixed[key] = value.map(item => 
        typeof item === 'object' ? fixObjectStrings(item) : 
        typeof item === 'string' ? fixTurkishChars(item) : item
      )
    } else if (typeof value === 'object' && value !== null) {
      fixed[key] = fixObjectStrings(value)
    } else {
      fixed[key] = value
    }
  }
  return fixed
}

/**
 * GTIN'den baştaki sıfırları kaldır
 * @param {string} gtin - GTIN kodu
 * @returns {string} - Temizlenmiş GTIN
 */
export const trimGtinLeadingZeros = (gtin) => {
  if (!gtin) return gtin
  return gtin.replace(/^0+/, '')
}

export default {
  fixTurkishChars,
  fixObjectStrings,
  trimGtinLeadingZeros
}


