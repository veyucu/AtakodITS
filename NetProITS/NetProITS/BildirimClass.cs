// Decompiled with JetBrains decompiler
// Type: NetProITS.BildirimClass
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace NetProITS
{
  public class BildirimClass
  {
    public List<BildirimStokClass> STOKLAR;
    public List<ItsHarClass> DETAYLAR;

    public int RECNO { get; set; }

    public int TIP { get; set; }

    public string DURUM { get; set; }

    public DateTime TARIH { get; set; }

    public string EVRAK_NO { get; set; }

    public string CARI_KOD { get; set; }

    public string CARI_UNVAN { get; set; }

    public string GLNNO { get; set; }
  }
}
