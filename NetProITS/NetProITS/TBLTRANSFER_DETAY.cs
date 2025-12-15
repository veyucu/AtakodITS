// Decompiled with JetBrains decompiler
// Type: NetProITS.TBLTRANSFER_DETAY
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System;

#nullable disable
namespace NetProITS
{
  public class TBLTRANSFER_DETAY
  {
    public int ID { get; set; }

    public string KAYNAK_GLNNO { get; set; }

    public string DOCUMENT_NUMBER { get; set; }

    public DateTime? DOCUMENT_DATE { get; set; }

    public string CARRIER_LABEL { get; set; }

    public string GTIN { get; set; }

    public string SERIAL_NUMBER { get; set; }

    public string LOT_NUMBER { get; set; }

    public string DATE { get; set; }

    public string KOLI_BARKOD { get; set; }

    public long? TRANSFER_ID { get; set; }

    public string DURUM { get; set; }
  }
}
