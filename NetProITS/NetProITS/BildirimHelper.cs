// Decompiled with JetBrains decompiler
// Type: NetProITS.BildirimHelper
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class BildirimHelper
  {
    private static string itsHost;
    public static string TokenUrl = "/token/app/token";
    public static string DepoSatisUrl = "/wholesale/app/dispatch";
    public static string CheckStatusUrl = "/reference/app/check_status";
    public static string DeaktivasyonUrl = "/common/app/deactivation";
    public static string MalAlimUrl = "/common/app/accept";
    public static string MalIadeUrl = "/common/app/return";
    public static string SatisIptalUrl = "/wholesale/app/dispatchcancel";
    public static string EczaneSatisUrl = "/prescription/app/pharmacysale";
    public static string EczaneSatisIptalUrl = "/prescription/app/pharmacysalecancel";
    public static string TakasDevirUrl = "/common/app/transfer";
    public static string TakasIptalUrl = "/common/app/transfercancel";
    public static string CevapKodUrl = "/reference/app/errorcode";
    public static string PaketSorguUrl = "/pts/app/search";
    public static string PaketIndirUrl = "/pts/app/GetPackage";
    public static string PaketGonderUrl = "/pts/app/SendPackage";
    public static string DogrulamaUrl = "/reference/app/verification";

    public static string ItsHost
    {
      get
      {
        if (string.IsNullOrEmpty(BildirimHelper.itsHost))
          BildirimHelper.itsHost = ConfigurationManager.AppSettings[nameof (ItsHost)];
        return BildirimHelper.itsHost;
      }
    }

    public static DateTime ConvertMiadTarih(string Tarih)
    {
      try
      {
        return DateTime.ParseExact(Tarih, "yyMMdd", (IFormatProvider) null);
      }
      catch
      {
        DateTime dateTime = DateTime.ParseExact(Tarih, new string[2]
        {
          "yyMMdd",
          "yyMM00"
        }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
        dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        return dateTime;
      }
    }

    public static string GetAccessToken(string userName, string password)
    {
      try
      {
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.TokenUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody((object) ("{\"username\" : \"" + userName + "\",\"password\":\"" + password + "\" }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        return jobject["item"].SelectToken("token") != null ? jobject["item"][(object) "token"].Value<string>() : (string) null;
      }
      catch
      {
        return (string) null;
      }
    }

    public static List<KarekodBilgi> DepoSatisBildirimi(
      string userName,
      string password,
      string KarsiGlnNo,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.DepoSatisUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"togln\" : \"" + KarsiGlnNo + "\",\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> DurumSorgula(
      string userName,
      string password,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.CheckStatusUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "responseObjectList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gln1"].Value<string>(), jtoken[(object) "gln2"].Value<string>(), jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> DeakivasyonBildirimi(
      string userName,
      string password,
      string glnno,
      string deakivasyonkodu,
      string aciklama,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.DeaktivasyonUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"dt\" : \"D\",\"frGln\":\"" + glnno + "\",\"ds\":\"" + deakivasyonkodu + "\",\"description\":\"" + aciklama + "\",\"document\":{\"dd\":\"\",\"dn\":\"\"},\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> MalAlimBildirimi(
      string userName,
      string password,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.MalAlimUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> MalIadeBildirimi(
      string userName,
      string password,
      string KarsiGlnNo,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.MalIadeUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"togln\" : \"" + KarsiGlnNo + "\",\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> SatisIptalBildirimi(
      string userName,
      string password,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.SatisIptalUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> EczaneSatisBildirimi(
      string userName,
      string password,
      string kaynakglnno,
      string hedefglnno,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.EczaneSatisUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"dt\":\"S\",\"fr\":\"" + kaynakglnno + "\",\"to\":\"" + hedefglnno + "\",\"document\":{\"dd\":\"\",\"dn\":\"\",\"dr\":\"\",\"cp\":\"\",\"eid\":\"\",\"rkn\":\"\"},\"productList\":" + str + " } }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> EczaneSatisIptalBildirimi(
      string userName,
      string password,
      string kaynakglnno,
      string hedefglnno,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.EczaneSatisIptalUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"dt\":\"C\",\"fr\":\"" + kaynakglnno + "\",\"to\":\"" + hedefglnno + "\",\"document\":{\"dd\":\"\",\"dn\":\"\",\"dr\":\"\",\"cp\":\"\",\"eid\":\"\",\"rkn\":\"\"},\"productList\":" + str + " } }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> MalDevirBildirimi(
      string userName,
      string password,
      string KarsiGlnNo,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.TakasDevirUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"togln\" : \"" + KarsiGlnNo + "\",\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> MalDevirIptalBildirimi(
      string userName,
      string password,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.TakasIptalUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static List<KarekodBilgi> DogrulamaBildirimi(
      string userName,
      string password,
      string glnno,
      List<KarekodBilgi> its)
    {
      try
      {
        if (its == null || its.Count <= 0)
          return (List<KarekodBilgi>) null;
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.DogrulamaUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string str = JsonConvert.SerializeObject((object) its);
        request.AddJsonBody((object) ("{\"dt\":\"V\",\"fr\":\"" + glnno + "\",\"productList\":" + str + " }"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "productList"])
          karekodBilgiList.Add(new KarekodBilgi(jtoken[(object) "gtin"].Value<string>(), jtoken[(object) "sn"].Value<string>(), jtoken[(object) "uc"].Value<string>()));
        return karekodBilgiList;
      }
      catch
      {
        return (List<KarekodBilgi>) null;
      }
    }

    public static void GetCevapKodlari(string userName, string password)
    {
      try
      {
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.CevapKodUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddJsonBody((object) "{}");
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "errorCodeList"])
        {
          int iii = Convert.ToInt32(jtoken[(object) "code"].Value<string>());
          if (MyUtils.Firma.HATA_KODLARI.Where<HATA_KODLARI>((Expression<Func<HATA_KODLARI, bool>>) (u => u.HATAID == iii)).FirstOrDefault<HATA_KODLARI>() == null)
          {
            MyUtils.Firma.HATA_KODLARI.Add(new HATA_KODLARI()
            {
              AD = new int?(0),
              HATAID = iii,
              MESAJ = jtoken[(object) "message"].Value<string>()
            });
            MyUtils.Firma.SaveChanges();
          }
        }
      }
      catch
      {
      }
    }

    public static List<string> PaketSorgulama(
      string userName,
      string password,
      string senderglnno,
      string glnno,
      DateTime ilktarih,
      DateTime sontarih)
    {
      try
      {
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.PaketSorguUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddJsonBody((object) ("{\"sourceGln\":\"\",\"destinationGln\":\"" + glnno + "\",\"bringNotReceivedTransferInfo\":0,\"startDate\":\"" + ilktarih.ToString("yyyy-MM-dd") + "\",\"endDate\":\"" + sontarih.ToString("yyyy-MM-dd") + "\"}"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        List<string> stringList = new List<string>();
        foreach (JToken jtoken in (IEnumerable<JToken>) jobject["item"][(object) "transferDetails"])
          stringList.Add(jtoken[(object) "transferId"].Value<string>());
        return stringList;
      }
      catch
      {
        return (List<string>) null;
      }
    }

    public static string PaketIndir(string userName, string password, string transferno)
    {
      try
      {
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.PaketIndirUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddJsonBody((object) ("{\"transferId\":\"" + transferno + "\"}"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        return jobject["item"].SelectToken("fileStream") != null ? jobject["item"][(object) "fileStream"].Value<string>() : (string) null;
      }
      catch
      {
        return (string) null;
      }
    }

    public static string PaketGonder(string userName, string password, string karsiglnno)
    {
      try
      {
        byte[] inArray = File.ReadAllBytes(Application.StartupPath + "\\PTS\\temp.zip");
        string accessToken = BildirimHelper.GetAccessToken(userName, password);
        RestClient restClient = new RestClient(BildirimHelper.ItsHost);
        RestRequest request = new RestRequest(BildirimHelper.PaketGonderUrl, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + accessToken);
        string base64String = Convert.ToBase64String(inArray);
        request.AddJsonBody((object) ("{\"receiver\":\"" + karsiglnno + "\",\"file\":\"" + base64String + "\"}"));
        JObject jobject = JObject.Parse("{'item':" + restClient.Execute((IRestRequest) request).Content + "}");
        return jobject["item"].SelectToken("transferId") != null ? jobject["item"][(object) "transferId"].Value<string>() : string.Empty;
      }
      catch
      {
        return string.Empty;
      }
    }
  }
}
