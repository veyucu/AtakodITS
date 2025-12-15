// Decompiled with JetBrains decompiler
// Type: NetProITS.MyUtils
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using NetOpenX50;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NetProITS
{
  public class MyUtils
  {
    public static Kernel kernel = (Kernel) null;
    public static Sirket sirket = (Sirket) null;
    public static FIRMAEntities Firma;
    public static string strConnString = "";
    public static string strDatabase = "";
    public static string strNetsisUser = "";
    public static string strNetsisPassword = "";
    public static string ResmiFaturaYazici = "";
    public static string GayriFaturaYazici = "";
    public static string SiparisYazici = "";
    public static string EtiketYazici = "";

    public static void GetConfigBilgi()
    {
      MyUtils.strConnString = ConfigurationManager.ConnectionStrings["FIRMAEntitiesNew"].ConnectionString;
      MyUtils.strConnString = MyUtils.strConnString.Replace("Initial Catalog=DATABASE;", "Initial Catalog=" + MyUtils.strDatabase + ";");
      MyUtils.strDatabase = ConfigurationManager.AppSettings["Database"];
      MyUtils.strNetsisUser = ConfigurationManager.AppSettings["NetsisUser"];
      MyUtils.strNetsisPassword = ConfigurationManager.AppSettings["NetsisPassword"];
      MyUtils.ResmiFaturaYazici = ConfigurationManager.AppSettings["ResmiFaturaYazici"];
      MyUtils.GayriFaturaYazici = ConfigurationManager.AppSettings["GayriFaturaYazici"];
      MyUtils.SiparisYazici = ConfigurationManager.AppSettings["SiparisYazici"];
      MyUtils.EtiketYazici = ConfigurationManager.AppSettings["EtiketYazici"];
    }

    public static void Refresh()
    {
      MyUtils.Firma = new FIRMAEntities();
      MyUtils.Firma.Database.Connection.ConnectionString = MyUtils.strConnString;
    }

    public static void HareketEkle(
      string BARKOD,
      string EVRAK_SERI,
      string CARI_KOD,
      string DURUM,
      string MIAD,
      string PARTINO,
      string SERI_NO,
      string STOK_KOD,
      int TIP)
    {
      try
      {
        MyUtils.Firma.ITSHAR.Add(new ITSHAR()
        {
          BARKOD = BARKOD,
          EVRAK_SERI = EVRAK_SERI,
          CARI_KOD = CARI_KOD,
          DURUM = DURUM,
          MIAD = MIAD,
          PARTINO = PARTINO,
          SERI_NO = SERI_NO,
          STOK_KOD = STOK_KOD,
          TIP = new int?(TIP)
        });
        MyUtils.Firma.SaveChanges();
        MyUtils.Refresh();
      }
      catch
      {
      }
    }

    public static void HareketEkle(
      string BARKOD,
      string EVRAK_SERI,
      string CARI_KOD,
      string DURUM,
      string MIAD,
      string PARTINO,
      string SERI_NO,
      string STOK_KOD,
      int TIP,
      List<ITSHAR_VIEW> its)
    {
      try
      {
        MyUtils.Firma.ITSHAR.Add(new ITSHAR()
        {
          BARKOD = BARKOD,
          EVRAK_SERI = EVRAK_SERI,
          CARI_KOD = CARI_KOD,
          DURUM = DURUM,
          MIAD = MIAD,
          PARTINO = PARTINO,
          SERI_NO = SERI_NO,
          STOK_KOD = STOK_KOD,
          TIP = new int?(TIP)
        });
        MyUtils.Firma.SaveChanges();
        MyUtils.Refresh();
        its.Add(new ITSHAR_VIEW()
        {
          BARKOD = BARKOD,
          CARI_KOD = CARI_KOD,
          DURUM = DURUM,
          EVRAK_SERI = EVRAK_SERI,
          MIAD = MIAD,
          PARTINO = PARTINO,
          SERI_NO = SERI_NO,
          STOK_KOD = STOK_KOD,
          TIP = new int?(TIP)
        });
      }
      catch
      {
      }
    }

    public static ITSHAR GetItsKalem(ITSHAR_VIEW param)
    {
      return new ITSHAR()
      {
        BARKOD = param.BARKOD,
        CARI_KOD = param.CARI_KOD,
        DURUM = param.DURUM,
        EVRAK_SERI = param.EVRAK_SERI,
        ID = param.ID,
        MIAD = param.MIAD,
        PARTINO = param.PARTINO,
        SERI_NO = param.SERI_NO,
        STOK_KOD = param.STOK_KOD,
        TIP = param.TIP
      };
    }

    public static bool IsIts(string content, string key) => content.Contains(key);

    public static string GetITSHAR_VIEWSQL(string EvrakNo, string CariKod, int tip)
    {
      return "SELECT * FROM ITSHAR_VIEW WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " ORDER BY BARKOD,PARTINO,MIAD";
    }

    public static string GetITSHAR_VIEWSQLPaket(string EvrakNo, string CariKod, int tip)
    {
      return "SELECT * FROM ITSHAR_VIEW WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " AND BARKOD<>'' AND PARTINO<>'' AND SERI_NO<>'' ORDER BY BARKOD,PARTINO,MIAD";
    }

    public static string GetITSHAR_URUN(
      string EvrakNo,
      string CariKod,
      int tip,
      string Barkod,
      string SeriNo,
      string PartiNo,
      string Miad)
    {
      return "SELECT * FROM ITSHAR WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " AND BARKOD='" + Barkod + "' AND SERI_NO='" + SeriNo + "' AND PARTINO='" + PartiNo + "' AND MIAD='" + Miad + "'";
    }

    public static string GetITSHAR_BARKOD(
      string EvrakNo,
      string CariKod,
      int tip,
      string Barkod,
      string SeriNo)
    {
      return "SELECT * FROM ITSHAR WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " AND BARKOD='" + Barkod + "' AND SERI_NO='" + SeriNo + "'";
    }

    public static string GetITSHAR_STOK(string EvrakNo, string CariKod, int tip, string Stok)
    {
      return "SELECT * FROM ITSHAR WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " AND STOK_KOD='" + Stok + "'";
    }

    public static string GetITSHAR_GENEL(string EvrakNo, string CariKod, int tip)
    {
      return "SELECT * FROM ITSHAR WHERE EVRAK_SERI='" + EvrakNo + "' AND CARI_KOD='" + CariKod + "' AND TIP=" + tip.ToString() + " AND BARKOD<>'' AND SERI_NO<>'' AND PARTINO<>''";
    }

    public static string GetBildirimSQL(string EvrakNo, string CariKod, int tip, bool beseri)
    {
      string str = "";
      switch (tip)
      {
        case 1:
          str = "S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and (S.STHAR_HTUR='J' OR S.STHAR_HTUR='I')";
          break;
        case 2:
          str = "S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='J'";
          break;
        case 3:
          str = "S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='L'";
          break;
        case 4:
          str = "S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and S.STHAR_HTUR='L'";
          break;
        case 5:
          str = "1=1";
          break;
      }
      return beseri ? "SELECT *,[MIKTAR]-[KARSILANAN] AS KALAN FROM (SELECT CASE          WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and (S.STHAR_HTUR='J' OR S.STHAR_HTUR='I') THEN 1          WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='J' THEN 2          WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='L' THEN 3          WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and S.STHAR_HTUR='L' THEN 4 END AS [TIP],        ROW_NUMBER()OVER(ORDER BY S.STOK_KODU) as [RECNO],        S.FISNO AS [EVRAK_SERI],        S.STHAR_CARIKOD as [CARI_KOD],        'BESERI' as [REYON_KODU],        S.STOK_KODU as [BARKOD],        S.STOK_KODU as [STOK_KODU],        dbo.TRK(ST.STOK_ADI) as [STOK_ISIM],        SUM(CAST(S.STHAR_GCMIK AS FLOAT)) as [MIKTAR],\t   \tCASE             WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and (S.STHAR_HTUR='J' OR S.STHAR_HTUR='I') THEN dbo.GetItsMiktar(1,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='J' THEN dbo.GetItsMiktar(2,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='L' THEN dbo.GetItsMiktar(3,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and S.STHAR_HTUR='L' THEN dbo.GetItsMiktar(4,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)         END AS [KARSILANAN],         CAST(0 AS float) AS BAKIYE,         dbo.TRK(EK.KULL3S) AS [BOLUM_KODU],         dbo.TRK(S.KOSULKODU) AS KOSULKODU FROM TBLSTHAR S INNER JOIN TBLSTSABIT ST ON S.STOK_KODU=ST.STOK_KODU INNER JOIN TBLSTSABITEK EK ON EK.STOK_KODU = ST.STOK_KODU WHERE S.STHAR_GCKOD in ('G','C') and S.STHAR_FTIRSIP in (1,2) and S.STHAR_HTUR in ('I','J','L') AND S.FISNO='" + EvrakNo + "' AND S.STHAR_CARIKOD='" + CariKod + "' AND " + str + " GROUP BY S.FISNO,S.STOK_KODU,S.STHAR_CARIKOD,ST.STOK_ADI,ST.KOD_5,S.STHAR_GCKOD,S.STHAR_FTIRSIP,S.STHAR_HTUR,EK.KULL3S,S.KOSULKODU  UNION ALL SELECT\t5 AS TIP,         ROW_NUMBER()OVER(ORDER BY S.STOK_KODU) as [RECNO],         FISNO AS [EVRAK_SERI],         STHAR_CARIKOD AS [CARI_KOD],         'BESERI' as [REYON_KODU],         S.STOK_KODU as [BARKOD],         S.STOK_KODU as [STOK_KODU],         dbo.TRK(ST.STOK_ADI) as [STOK_ISIM],         SUM(CAST(S.STHAR_GCMIK AS FLOAT)) as [MIKTAR],         dbo.GetItsMiktar(5,FISNO,S.STHAR_CARIKOD,S.STOK_KODU) AS [KARSILANAN],         0 AS BAKIYE,         dbo.TRK(EK.KULL3S) AS [BOLUM_KODU],         dbo.TRK(S.KOSULKODU) AS KOSULKODU FROM TBLSIPATRA S INNER JOIN TBLSTSABIT ST ON S.STOK_KODU=ST.STOK_KODU INNER JOIN TBLSTSABITEK EK ON EK.STOK_KODU = ST.STOK_KODU WHERE S.STHAR_GCKOD='C' and S.STHAR_FTIRSIP='6' AND S.SUBE_KODU=0 AND S.FISNO='" + EvrakNo + "' AND S.STHAR_CARIKOD='" + CariKod + "' GROUP BY S.FISNO,S.STOK_KODU,S.STHAR_CARIKOD,ST.STOK_ADI,ST.KOD_5,EK.KULL3S,S.KOSULKODU )VERI ORDER BY RECNO" : "SELECT *,[MIKTAR]-[KARSILANAN] AS KALAN FROM (SELECT CASE          WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and (S.STHAR_HTUR='J' OR S.STHAR_HTUR='I') THEN 1          WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='J' THEN 2          WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='L' THEN 3          WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and S.STHAR_HTUR='L' THEN 4 END AS [TIP],        ROW_NUMBER()OVER(ORDER BY S.STOK_KODU) as [RECNO],        S.FISNO AS [EVRAK_SERI],        S.STHAR_CARIKOD as [CARI_KOD],        ST.KOD_5 as [REYON_KODU],        S.STOK_KODU as [BARKOD],        S.STOK_KODU as [STOK_KODU],        dbo.TRK(ST.STOK_ADI) as [STOK_ISIM],        SUM(CAST(S.STHAR_GCMIK AS FLOAT)) as [MIKTAR],\t   \tCASE             WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and (S.STHAR_HTUR='J' OR S.STHAR_HTUR='I') THEN dbo.GetItsMiktar(1,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='J' THEN dbo.GetItsMiktar(2,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='G' AND S.STHAR_FTIRSIP=2 and S.STHAR_HTUR='L' THEN dbo.GetItsMiktar(3,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)            WHEN S.STHAR_GCKOD='C' AND S.STHAR_FTIRSIP=1 and S.STHAR_HTUR='L' THEN dbo.GetItsMiktar(4,FISNO,S.STHAR_CARIKOD,S.STOK_KODU)         END AS [KARSILANAN],         CAST(0 as float) AS BAKIYE,         dbo.TRK(EK.KULL3S) AS [BOLUM_KODU],         dbo.TRK(S.KOSULKODU) AS KOSULKODU FROM TBLSTHAR S INNER JOIN TBLSTSABIT ST ON S.STOK_KODU=ST.STOK_KODU INNER JOIN TBLSTSABITEK EK ON EK.STOK_KODU = ST.STOK_KODU WHERE S.STHAR_GCKOD in ('G','C') and S.STHAR_FTIRSIP in (1,2) and S.STHAR_HTUR in ('I','J','L') AND S.FISNO='" + EvrakNo + "' AND S.STHAR_CARIKOD='" + CariKod + "' AND " + str + " GROUP BY S.FISNO,S.STOK_KODU,S.STHAR_CARIKOD,ST.STOK_ADI,ST.KOD_5,S.STHAR_GCKOD,S.STHAR_FTIRSIP,S.STHAR_HTUR,EK.KULL3S,S.KOSULKODU UNION ALL SELECT\t5 AS TIP,         ROW_NUMBER()OVER(ORDER BY S.STOK_KODU) as [RECNO],         FISNO AS [EVRAK_SERI],         STHAR_CARIKOD AS [CARI_KOD],         ST.KOD_5 as [REYON_KODU],         S.STOK_KODU as [BARKOD],         S.STOK_KODU as [STOK_KODU],         dbo.TRK(ST.STOK_ADI) as [STOK_ISIM],         SUM(CAST(S.STHAR_GCMIK AS FLOAT)) as [MIKTAR],         dbo.GetItsMiktar(5,FISNO,S.STHAR_CARIKOD,S.STOK_KODU) AS [KARSILANAN],         CAST(0 AS float) AS BAKIYE,         dbo.TRK(EK.KULL3S) AS [BOLUM_KODU],         dbo.TRK(S.KOSULKODU) AS KOSULKODU FROM TBLSIPATRA S INNER JOIN TBLSTSABIT ST ON S.STOK_KODU=ST.STOK_KODU INNER JOIN TBLSTSABITEK EK ON EK.STOK_KODU = ST.STOK_KODU WHERE S.STHAR_GCKOD='C' and S.STHAR_FTIRSIP='6' AND S.SUBE_KODU=0 AND S.FISNO='" + EvrakNo + "' AND S.STHAR_CARIKOD='" + CariKod + "' GROUP BY S.FISNO,S.STOK_KODU,S.STHAR_CARIKOD,ST.STOK_ADI,ST.KOD_5,EK.KULL3S,S.KOSULKODU )VERI ORDER BY RECNO";
    }

    public static string GetParamValue(string name)
    {
      TBL_AYARLAR tblAyarlar = new MikroDB_V14Entities().TBL_AYARLAR.Where<TBL_AYARLAR>((Expression<System.Func<TBL_AYARLAR, bool>>) (u => u.PARAM_NAME == name)).FirstOrDefault<TBL_AYARLAR>();
      return tblAyarlar != null ? tblAyarlar.PARAM_VALUE : string.Empty;
    }

    public static void SetParamValue(string name, string value)
    {
      MikroDB_V14Entities mikroDbV14Entities = new MikroDB_V14Entities();
      TBL_AYARLAR entity = mikroDbV14Entities.TBL_AYARLAR.Where<TBL_AYARLAR>((Expression<System.Func<TBL_AYARLAR, bool>>) (u => u.PARAM_NAME == name)).FirstOrDefault<TBL_AYARLAR>();
      if (entity != null)
      {
        entity.PARAM_VALUE = value;
        mikroDbV14Entities.TBL_AYARLAR.Attach(entity);
        mikroDbV14Entities.Entry<TBL_AYARLAR>(entity).State = EntityState.Modified;
      }
      else
        mikroDbV14Entities.TBL_AYARLAR.Add(new TBL_AYARLAR()
        {
          PARAM_NAME = name,
          PARAM_VALUE = value
        });
      mikroDbV14Entities.SaveChanges();
    }
  }
}
