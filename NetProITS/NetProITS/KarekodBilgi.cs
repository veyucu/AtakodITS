// Decompiled with JetBrains decompiler
// Type: NetProITS.KarekodBilgi
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using Newtonsoft.Json;

#nullable disable
namespace NetProITS
{
  public class KarekodBilgi
  {
    public KarekodBilgi()
    {
    }

    public KarekodBilgi(string gtin, string sn, string uc)
    {
      this.Barkod = gtin;
      this.SeriNo = sn;
      this.Sonuc = uc;
    }

    public KarekodBilgi(string gtin, string sn, string parti, string miad)
    {
      this.Barkod = gtin;
      this.SeriNo = sn;
      this.PartiNo = parti;
      this.Miad = miad;
    }

    public KarekodBilgi(string glnno1, string glnno2, string gtin, string sn, string uc)
    {
      this.GlnNo1 = glnno1;
      this.GlnNo2 = glnno2;
      this.Barkod = gtin;
      this.SeriNo = sn;
      this.Sonuc = uc;
    }

    [JsonProperty("gtin")]
    public string Barkod { get; set; }

    [JsonProperty("sn")]
    public string SeriNo { get; set; }

    [JsonProperty("bn")]
    public string PartiNo { get; set; }

    [JsonProperty("xd")]
    public string Miad { get; set; }

    [JsonIgnore]
    public string GlnNo1 { get; set; }

    [JsonIgnore]
    public string GlnNo2 { get; set; }

    [JsonIgnore]
    public string Sonuc { get; set; }
  }
}
