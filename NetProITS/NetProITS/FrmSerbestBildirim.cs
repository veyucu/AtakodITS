// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmSerbestBildirim
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace NetProITS
{
  public class FrmSerbestBildirim : Form
  {
    private BildirimClass AktifEvrak;
    private int EvrakID;
    private string EvrakNo;
    private string CariKod;
    private string CariGLNNo;
    private EvrakTipi EvrakTip;
    private IContainer components = (IContainer) null;
    private DefaultLookAndFeel defaultLookAndFeel1;
    private PanelControl panelControl1;
    private XtraTabControl xtraTabControl1;
    private XtraTabPage xtraTabPage2;
    private PanelControl pnlUst;
    private PanelControl panelControl4;
    private SimpleButton btnBildirim;
    private SimpleButton btnKarekod;
    private XtraTabPage xtraTabPage3;
    private PanelControl panelControl6;
    private MemoEdit memSonuc;
    private SplitterControl splitterControl1;
    private SimpleButton btnImport;
    private System.Windows.Forms.ComboBox cbTip;
    private SimpleButton btnExcelAktar;
    private SimpleButton btnSeriAktar;
    private SimpleButton btnEvrak;
    private Label label1;
    private TextBox txtEvrak;
    private Label lblCari;
    private SimpleButton btnGonder;
    private CheckBox chkHizliMod;
    private GridControl grdKalem;
    private GridView gridView2;
    private GridColumn colRECNO1;
    private GridColumn colCARIHR_RECNO;
    private GridColumn gridColumn1;
    private GridColumn colSTOK_KODU;
    private GridColumn colSTOK_ISIM;
    private GridColumn colMIKTAR;
    private GridColumn colKARSILANAN;
    private GridColumn gridColumn2;
    private GridColumn gridColumn3;
    private Panel pnlHizli;
    private MemoEdit txtHizli;
    private Panel panel4;
    private SimpleButton btnGuncelle;
    private GridView gridView1;
    private SimpleButton btnTemizle;
    private Panel panel5;
    private System.Windows.Forms.ProgressBar prgDurum2;
    private PanelControl panelControl7;
    private GridControl grdKalemDetay;
    private GridView gridView3;
    private GridColumn colID;
    private GridColumn colTIP;
    private GridColumn colCARIHR_RECNO1;
    private GridColumn colSTOK_KOD;
    private GridColumn colSTOK_ISIM1;
    private GridColumn colBARKOD;
    private GridColumn colSERI_NO;
    private GridColumn colMIAD;
    private GridColumn colPARTINO;
    private GridColumn colDURUM;
    private SimpleButton btnSorgula;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem exceleGönderToolStripMenuItem;
    private SaveFileDialog saveFileDialog1;
    private SimpleButton btnDogrulama;
    private Panel panel1;
    private System.Windows.Forms.ProgressBar prgDurum;
    private SimpleButton btnTakas;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem mnuDepo1;
    private ToolStripMenuItem mnuDepo2;
    private ToolStripMenuItem mnuDepo3;

    public FrmSerbestBildirim() => this.InitializeComponent();

    private void simpleButton1_Click(object sender, EventArgs e)
    {
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
    }

    private void gridControl1_Click(object sender, EventArgs e)
    {
    }

    private void btnKarekod_Click(object sender, EventArgs e)
    {
      FrmKareKod karekod;
      while (true)
      {
        karekod = new FrmKareKod();
        if (karekod.ShowDialog() == DialogResult.OK)
        {
          if (karekod.chkKolii)
          {
            string koli = karekod.strKoliBarkod;
            StringBuilder stringBuilder = new StringBuilder();
            List<TBLTRANSFER_DETAY> list1 = MyUtils.Firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.CARRIER_LABEL == koli)).ToList<TBLTRANSFER_DETAY>();
            if (list1.Count > 0)
            {
              string barkod = list1[0].GTIN;
              barkod = barkod.Remove(0, 1);
              List<TBLSTSABIT> list2 = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == barkod)).ToList<TBLSTSABIT>();
              if (list2.Count > 0)
              {
                string stokKodu = list2[0].STOK_KODU;
                string stokAdi = list2[0].STOK_ADI;
                foreach (TBLTRANSFER_DETAY tbltransferDetay in list1)
                {
                  TBLTRANSFER_DETAY item = tbltransferDetay;
                  Application.DoEvents();
                  DateTime dateTime;
                  try
                  {
                    dateTime = DateTime.ParseExact(item.DATE, "yyyy-MM-dd", (IFormatProvider) null);
                  }
                  catch
                  {
                    dateTime = DateTime.ParseExact(item.DATE, new string[2]
                    {
                      "yyyy-MM-dd",
                      "yyyy-MM-00"
                    }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
                    dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
                  }
                  string miad = dateTime.ToString("yyMMdd");
                  if (this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == item.GTIN & u.SERI_NO == item.SERIAL_NUMBER & u.PARTINO == item.LOT_NUMBER & u.MIAD == miad)).ToList<ItsHarClass>().Count == 0)
                  {
                    if (this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>().Count == 0)
                      this.AktifEvrak.STOKLAR.Add(new BildirimStokClass()
                      {
                        BARKOD = barkod,
                        CARI_KOD = this.AktifEvrak.CARI_KOD,
                        EVRAK_SERI = this.EvrakNo,
                        STOK_KODU = list2[0].STOK_KODU,
                        STOK_ISIM = stokAdi,
                        TIP = this.EvrakID,
                        REYON_KODU = "BESERI",
                        RECNO = 1,
                        KALAN = 0.0,
                        MIKTAR = 0.0,
                        KARSILANAN = 0
                      });
                    this.AktifEvrak.DETAYLAR.Add(new ItsHarClass()
                    {
                      BARKOD = item.GTIN,
                      EVRAK_SERI = this.EvrakNo,
                      CARI_KOD = this.CariKod,
                      DURUM = "",
                      MIAD = miad,
                      PARTINO = item.LOT_NUMBER,
                      SERI_NO = item.SERIAL_NUMBER,
                      STOK_KOD = list2[0].STOK_KODU,
                      TIP = this.EvrakID,
                      STOK_ISIM = stokAdi
                    });
                    ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].MIKTAR;
                    ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KARSILANAN;
                    this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KALAN = 0.0;
                    this.grdKalem.DataSource = (object) null;
                    this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
                  }
                  else
                    stringBuilder.AppendLine(item.GTIN + " | " + item.SERIAL_NUMBER + " | " + item.LOT_NUMBER + " | " + item.DATE);
                }
              }
            }
            if (stringBuilder.Length > 1)
            {
              FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
              int num = (int) frmKoliSonuc.ShowDialog();
              frmKoliSonuc.Dispose();
            }
          }
          else if (karekod.strGtin.Length > 0 & karekod.strMiad.Length > 0 & karekod.strPartiNo.Length > 0 & karekod.strseriNo.Length > 0 && karekod.strGtin.Length > 0)
          {
            string barkod = karekod.strGtin;
            barkod = barkod.Remove(0, 1);
            karekod.strPartiNo = karekod.strPartiNo.ToUpper();
            karekod.strseriNo = karekod.strseriNo.ToUpper();
            DateTime dateTime;
            try
            {
              dateTime = DateTime.ParseExact(karekod.strMiad, "yyMMdd", (IFormatProvider) null);
            }
            catch
            {
              dateTime = DateTime.ParseExact(karekod.strMiad, new string[2]
              {
                "yyMMdd",
                "yyMM00"
              }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
              dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
            }
            karekod.strMiad = dateTime.ToString("yyMMdd");
            List<TBLSTSABIT> list = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == barkod)).ToList<TBLSTSABIT>();
            if (list.Count > 0)
            {
              string stokKodu = list[0].STOK_KODU;
              string stokAdi = list[0].STOK_ADI;
              if (this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == karekod.strGtin & u.SERI_NO == karekod.strseriNo & u.PARTINO == karekod.strPartiNo & u.MIAD == karekod.strMiad)).ToList<ItsHarClass>().Count == 0)
              {
                if (this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>().Count == 0)
                  this.AktifEvrak.STOKLAR.Add(new BildirimStokClass()
                  {
                    BARKOD = barkod,
                    CARI_KOD = this.AktifEvrak.CARI_KOD,
                    EVRAK_SERI = this.EvrakNo,
                    STOK_KODU = list[0].STOK_KODU,
                    STOK_ISIM = stokAdi,
                    TIP = this.EvrakID,
                    REYON_KODU = "BESERI",
                    RECNO = 1,
                    KALAN = 0.0,
                    MIKTAR = 0.0,
                    KARSILANAN = 0
                  });
                this.AktifEvrak.DETAYLAR.Add(new ItsHarClass()
                {
                  BARKOD = karekod.strGtin,
                  EVRAK_SERI = this.EvrakNo,
                  CARI_KOD = this.CariKod,
                  DURUM = "",
                  MIAD = karekod.strMiad,
                  PARTINO = karekod.strPartiNo,
                  SERI_NO = karekod.strseriNo,
                  STOK_KOD = list[0].STOK_KODU,
                  TIP = this.EvrakID,
                  STOK_ISIM = stokAdi
                });
                ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].MIKTAR;
                ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KARSILANAN;
                this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KALAN = 0.0;
                this.grdKalem.DataSource = (object) null;
                this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
              }
            }
          }
          karekod.Dispose();
        }
        else
          break;
      }
      karekod.Dispose();
    }

    private void btnBitir1_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.AktifEvrak.STOKLAR.Count == 0)
        {
          int num1 = (int) MessageBox.Show("Lütfen ürün okutunuz");
        }
        else
        {
          this.Cursor = Cursors.WaitCursor;
          int num2 = 0;
          int num3 = 0;
          List<FrmSerbestBildirim.HataListe> source = new List<FrmSerbestBildirim.HataListe>();
          foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) MyUtils.Firma.HATA_KODLARI)
            source.Add(new FrmSerbestBildirim.HataListe()
            {
              HataKod = hataKodlari.HATAID,
              Ad = 0,
              Durum = hataKodlari.MESAJ
            });
          List<KarekodBilgi> its = new List<KarekodBilgi>();
          foreach (ItsHarClass itsHarClass in this.AktifEvrak.DETAYLAR)
          {
            string miad = BildirimHelper.ConvertMiadTarih(itsHarClass.MIAD).ToString("yyyy-MM-dd");
            its.Add(new KarekodBilgi(itsHarClass.BARKOD, itsHarClass.SERI_NO, itsHarClass.PARTINO, miad));
          }
          List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              karekodBilgiList = BildirimHelper.DepoSatisBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.AktifEvrak.GLNNO, its);
              break;
            case EvrakTipi.ALIS:
              karekodBilgiList = BildirimHelper.MalAlimBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), its);
              break;
            case EvrakTipi.SATIS_IPTAL:
              karekodBilgiList = BildirimHelper.SatisIptalBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), its);
              break;
            case EvrakTipi.ALIS_IPTAL:
              karekodBilgiList = BildirimHelper.MalIadeBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.AktifEvrak.GLNNO, its);
              break;
            case EvrakTipi.DEAKTIVASYON:
              karekodBilgiList = BildirimHelper.DeakivasyonBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), MyUtils.GetParamValue("GlnNo"), "10", "SİSTEMDEN ÇIKARMA", its);
              break;
          }
          foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
          {
            KarekodBilgi item = karekodBilgi;
            item.Barkod.Remove(0, 1);
            ItsHarClass h = this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo & u.TIP == this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).ToList<ItsHarClass>()[0];
            if (h != null)
            {
              h.DURUM = item.Sonuc;
              if (item.Sonuc == "00000")
                ++num2;
              source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad = source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad + 1;
              h.MESAJ = source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Durum;
            }
            ++num3;
          }
          this.grdKalemDetay.DataSource = (object) null;
          this.grdKalemDetay.DataSource = (object) this.AktifEvrak.DETAYLAR;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("Bildirim Detayları : ");
          foreach (FrmSerbestBildirim.HataListe hataListe in source)
          {
            if (hataListe.Ad > 0)
            {
              stringBuilder.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
              hataListe.Ad = 0;
            }
          }
          stringBuilder.AppendLine("Bildirim Durumu : " + num2.ToString() + " / " + num3.ToString());
          this.memSonuc.Text = stringBuilder.ToString();
          this.Cursor = Cursors.Default;
          this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
          if (this.EvrakTip != EvrakTipi.SATIS || num2 != num3 || MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
          this.btnPaketAl_Click(sender, e);
        }
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
      }
    }

    private void btnPaketAl_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      XmlDocument xmlDocument = new XmlDocument();
      string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><transfer xsi:noNamespaceSchemaLocation=\"its.gov.tr/packageTransfer.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></transfer>";
      xmlDocument.LoadXml(xml);
      XmlNode xmlNode = xmlDocument.SelectSingleNode("transfer");
      XmlElement element1 = xmlDocument.CreateElement("sourceGLN");
      element1.InnerText = MyUtils.GetParamValue("GlnNo");
      xmlNode.AppendChild((XmlNode) element1);
      XmlElement element2 = xmlDocument.CreateElement("destinationGLN");
      element2.InnerText = this.AktifEvrak.GLNNO;
      xmlNode.AppendChild((XmlNode) element2);
      XmlElement element3 = xmlDocument.CreateElement("actionType");
      element3.InnerText = "S";
      xmlNode.AppendChild((XmlNode) element3);
      XmlElement element4 = xmlDocument.CreateElement("shipTo");
      xmlNode.AppendChild((XmlNode) element4);
      XmlElement element5 = xmlDocument.CreateElement("documentNumber");
      element5.InnerText = this.EvrakNo;
      xmlNode.AppendChild((XmlNode) element5);
      XmlElement element6 = xmlDocument.CreateElement("documentDate");
      element6.InnerText = Convert.ToDateTime(this.AktifEvrak.TARIH).ToString("yyyy-MM-dd");
      xmlNode.AppendChild((XmlNode) element6);
      XmlElement element7 = xmlDocument.CreateElement("note");
      xmlNode.AppendChild((XmlNode) element7);
      XmlElement element8 = xmlDocument.CreateElement("version");
      element8.InnerText = "V1";
      xmlNode.AppendChild((XmlNode) element8);
      ITSHAR_VIEW itsharView = new ITSHAR_VIEW();
      int num1 = 10000;
      XmlElement newChild1 = (XmlElement) null;
      XmlElement newChild2 = (XmlElement) null;
      foreach (ItsHarClass itsHarClass in this.AktifEvrak.DETAYLAR)
      {
        if (itsharView.BARKOD == itsHarClass.BARKOD & itsharView.PARTINO == itsHarClass.PARTINO & itsharView.MIAD == itsHarClass.MIAD)
        {
          if (newChild2 != null)
          {
            XmlElement element9 = xmlDocument.CreateElement("serialNumber");
            element9.InnerText = itsHarClass.SERI_NO;
            newChild2.AppendChild((XmlNode) element9);
          }
        }
        else
        {
          if (newChild1 != null)
          {
            newChild1.AppendChild((XmlNode) newChild2);
            xmlNode.AppendChild((XmlNode) newChild1);
          }
          ++num1;
          newChild1 = xmlDocument.CreateElement("carrier");
          XmlAttribute attribute1 = xmlDocument.CreateAttribute("carrierLabel");
          attribute1.InnerText = "00000000000000000000";
          newChild1.Attributes.Append(attribute1);
          XmlAttribute attribute2 = xmlDocument.CreateAttribute("containerType");
          attribute2.InnerText = "C";
          newChild1.Attributes.Append(attribute2);
          newChild2 = xmlDocument.CreateElement("productList");
          XmlAttribute attribute3 = xmlDocument.CreateAttribute("GTIN");
          attribute3.InnerText = itsHarClass.BARKOD;
          newChild2.Attributes.Append(attribute3);
          XmlAttribute attribute4 = xmlDocument.CreateAttribute("lotNumber");
          attribute4.InnerText = itsHarClass.PARTINO;
          newChild2.Attributes.Append(attribute4);
          XmlAttribute attribute5 = xmlDocument.CreateAttribute("expirationDate");
          DateTime dateTime;
          try
          {
            dateTime = DateTime.ParseExact(itsHarClass.MIAD, "yyMMdd", (IFormatProvider) null);
          }
          catch
          {
            dateTime = DateTime.ParseExact(itsHarClass.MIAD, new string[2]
            {
              "yyMMdd",
              "yyMM00"
            }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
          }
          attribute5.InnerText = dateTime.ToString("yyyy-MM-dd");
          newChild2.Attributes.Append(attribute5);
          XmlAttribute attribute6 = xmlDocument.CreateAttribute("PONumber");
          attribute6.InnerText = "";
          newChild2.Attributes.Append(attribute6);
          XmlElement element10 = xmlDocument.CreateElement("serialNumber");
          element10.InnerText = itsHarClass.SERI_NO;
          newChild2.AppendChild((XmlNode) element10);
          itsharView.BARKOD = itsHarClass.BARKOD;
          itsharView.MIAD = itsHarClass.MIAD;
          itsharView.PARTINO = itsHarClass.PARTINO;
        }
      }
      newChild1.AppendChild((XmlNode) newChild2);
      xmlNode.AppendChild((XmlNode) newChild1);
      MemoryStream memoryStream = new MemoryStream();
      if (!Directory.Exists(Application.StartupPath + "\\PTS"))
        Directory.CreateDirectory(Application.StartupPath + "\\PTS");
      File.Delete(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
      File.Delete(Application.StartupPath + "\\PTS\\temp.zip");
      xmlDocument.Save(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
      ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) File.OpenWrite("PTS\\temp.zip"));
      zipOutputStream.PutNextEntry(new ZipEntry("PTS" + this.EvrakNo + ".xml")
      {
        CompressionMethod = CompressionMethod.Deflated
      });
      FileStream fileStream = File.OpenRead(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
      byte[] buffer = new byte[fileStream.Length];
      fileStream.Read(buffer, 0, buffer.Length);
      ((Stream) zipOutputStream).Write(buffer, 0, buffer.Length);
      zipOutputStream.Finish();
      ((Stream) zipOutputStream).Close();
      fileStream.Close();
      try
      {
        int num2 = (int) MessageBox.Show("Paket Gönderilmiştir.\rTransfer ID :" + BildirimHelper.PaketGonder(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.AktifEvrak.GLNNO));
        MyUtils.Firma.SaveChanges();
      }
      catch (Exception ex)
      {
        int num3 = (int) MessageBox.Show("Hata oluştu:" + ex.Message);
      }
      Cursor.Current = Cursors.Default;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      this.prgDurum.Minimum = 0;
      this.prgDurum.Value = 0;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Text Files (*.txt)|*.txt";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      string[] strArray = new StringBuilder(File.ReadAllText(openFileDialog.FileName)).ToString().Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      this.prgDurum.Maximum = strArray.Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length; ++index)
      {
        Application.DoEvents();
        if (strArray[index].Split('|').Length > 2)
        {
          string barkod1 = strArray[index].Split('|')[0];
          string barkod = "0" + strArray[index].Split('|')[0];
          string SeriNo = strArray[index].Split('|')[1];
          string Miad = strArray[index].Split('|')[2];
          string PartiNo = strArray[index].Split('|')[3];
          List<TBLSTSABIT> list = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == barkod1)).ToList<TBLSTSABIT>();
          if (list.Count > 0)
          {
            string stokKodu = list[0].STOK_KODU;
            string stokAdi = list[0].STOK_ADI;
            if (this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == barkod & u.SERI_NO == SeriNo & u.PARTINO == PartiNo & u.MIAD == Miad)).ToList<ItsHarClass>().Count == 0)
            {
              if (this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>().Count == 0)
                this.AktifEvrak.STOKLAR.Add(new BildirimStokClass()
                {
                  BARKOD = barkod1,
                  CARI_KOD = this.AktifEvrak.CARI_KOD,
                  EVRAK_SERI = this.EvrakNo,
                  STOK_KODU = list[0].STOK_KODU,
                  STOK_ISIM = stokAdi,
                  TIP = this.EvrakID,
                  REYON_KODU = "BESERI",
                  RECNO = 1,
                  KALAN = 0.0,
                  MIKTAR = 0.0,
                  KARSILANAN = 0
                });
              this.AktifEvrak.DETAYLAR.Add(new ItsHarClass()
              {
                BARKOD = barkod,
                EVRAK_SERI = this.EvrakNo,
                CARI_KOD = this.CariKod,
                DURUM = "",
                MIAD = Miad,
                PARTINO = PartiNo,
                SERI_NO = SeriNo,
                STOK_KOD = list[0].STOK_KODU,
                TIP = this.EvrakID
              });
              ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].MIKTAR;
              ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].KARSILANAN;
              this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].KALAN = 0.0;
              this.grdKalem.DataSource = (object) null;
              this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
            }
            else
              stringBuilder.AppendLine(strArray[index]);
          }
        }
        this.prgDurum.Value = index + 1;
      }
      int num1 = (int) MessageBox.Show("Aktarım tamamlanmıştır.");
      if (stringBuilder.Length > 1)
      {
        FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
        int num2 = (int) frmKoliSonuc.ShowDialog();
        frmKoliSonuc.Dispose();
      }
    }

    private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cbTip_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cbTip.SelectedIndex)
      {
        case 0:
          this.EvrakTip = EvrakTipi.SATIS;
          break;
        case 1:
          this.EvrakTip = EvrakTipi.ALIS;
          break;
        case 2:
          this.EvrakTip = EvrakTipi.SATIS_IPTAL;
          break;
        case 3:
          this.EvrakTip = EvrakTipi.ALIS_IPTAL;
          break;
        case 4:
          this.EvrakTip = EvrakTipi.DEAKTIVASYON;
          break;
      }
    }

    private void btnExcelAktar_Click(object sender, EventArgs e)
    {
      try
      {
        this.grdKalem.ExportToXls(this.AktifEvrak.EVRAK_NO + ".xls");
        int num = (int) MessageBox.Show("Dosya aktarılmıştır.");
      }
      catch
      {
      }
    }

    private void gridView2_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        string StokKodu = this.gridView2.GetRowCellValue(this.gridView2.FocusedRowHandle, "STOK_KODU").ToString();
        FrmKalemDetay frmKalemDetay = new FrmKalemDetay();
        frmKalemDetay.kalemTipi = KalemTipi.SERBEST;
        frmKalemDetay.Stokkodu = StokKodu;
        frmKalemDetay.serbest = this.AktifEvrak.DETAYLAR;
        int num = (int) frmKalemDetay.ShowDialog();
        this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.STOK_KODU == StokKodu)).First<BildirimStokClass>().MIKTAR = (double) this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.STOK_KOD == StokKodu)).ToList<ItsHarClass>().Count;
        this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.STOK_KODU == StokKodu)).First<BildirimStokClass>().KARSILANAN = this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.STOK_KOD == StokKodu)).ToList<ItsHarClass>().Count;
        frmKalemDetay.Dispose();
        this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnSeriAktar_Click(object sender, EventArgs e)
    {
      FrmExportSettings frmExportSettings = new FrmExportSettings();
      int num1 = (int) frmExportSettings.ShowDialog();
      string barkod = frmExportSettings.Barkod;
      string seriNo = frmExportSettings.SeriNo;
      string miad = frmExportSettings.Miad;
      string partiNo = frmExportSettings.PartiNo;
      frmExportSettings.Dispose();
      try
      {
        TextWriter textWriter = (TextWriter) new StreamWriter("Export\\" + this.AktifEvrak.EVRAK_NO + ".txt");
        List<ItsHarClass> detaylar = this.AktifEvrak.DETAYLAR;
        int count = detaylar.Count;
        for (int index = 0; index < count; ++index)
        {
          string str = detaylar[index].BARKOD;
          if (detaylar[index].SERI_NO.Length > 0)
            str = str.Remove(0, 1);
          textWriter.WriteLine(barkod + str + seriNo + detaylar[index].SERI_NO + miad + detaylar[index].MIAD + partiNo + detaylar[index].PARTINO);
        }
        textWriter.Close();
        int num2 = (int) MessageBox.Show("Dosya aktarılmıştır.");
      }
      catch
      {
      }
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
      if (!(this.gridView2.GetRow(e.RowHandle) is BILDIRIM_STOK row))
        return;
      double? kalan = row.KALAN;
      double num = 0.0;
      if (kalan.GetValueOrDefault() == num && kalan.HasValue)
      {
        e.Appearance.BackColor = Color.Green;
        e.Appearance.ForeColor = Color.White;
      }
    }

    private void btnEvrak_Click(object sender, EventArgs e)
    {
      if (this.cbTip.SelectedIndex >= 0)
      {
        FrmCariSecim frmCariSecim = new FrmCariSecim();
        int num = (int) frmCariSecim.ShowDialog();
        frmCariSecim.Dispose();
        this.CariKod = frmCariSecim.CariKod;
        this.CariGLNNo = frmCariSecim.CariGLNNo;
        switch (this.EvrakTip)
        {
          case EvrakTipi.SATIS:
            this.EvrakID = 1;
            this.EvrakNo = "S000001";
            break;
          case EvrakTipi.ALIS:
            this.EvrakID = 2;
            this.EvrakNo = "A000001";
            break;
          case EvrakTipi.SATIS_IPTAL:
            this.EvrakID = 3;
            this.EvrakNo = "SI000001";
            break;
          case EvrakTipi.ALIS_IPTAL:
            this.EvrakID = 4;
            this.EvrakNo = "AI000001";
            break;
          case EvrakTipi.DEAKTIVASYON:
            this.EvrakID = 5;
            this.EvrakNo = "D000001";
            break;
        }
        this.txtEvrak.Text = this.EvrakNo;
        this.lblCari.Text = frmCariSecim.CariUnvan;
        this.AktifEvrak = new BildirimClass();
        this.AktifEvrak.CARI_KOD = this.CariKod;
        this.AktifEvrak.CARI_UNVAN = frmCariSecim.CariUnvan;
        this.AktifEvrak.DURUM = "";
        this.AktifEvrak.EVRAK_NO = this.EvrakNo;
        this.AktifEvrak.GLNNO = this.CariGLNNo;
        this.AktifEvrak.TARIH = DateTime.Now;
        this.AktifEvrak.TIP = this.EvrakID;
        this.AktifEvrak.STOKLAR = new List<BildirimStokClass>();
        this.AktifEvrak.DETAYLAR = new List<ItsHarClass>();
        this.grdKalem.DataSource = (object) null;
        this.grdKalemDetay.DataSource = (object) null;
        this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
        this.grdKalemDetay.DataSource = (object) this.AktifEvrak.DETAYLAR;
      }
      else
      {
        int num1 = (int) MessageBox.Show("Bildirim tipini seçiniz");
      }
    }

    private void btnGonder_Click(object sender, EventArgs e)
    {
      if (this.EvrakTip != EvrakTipi.SATIS)
      {
        int num1 = (int) MessageBox.Show("Bildirim tipi Satış olmalı!");
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        int recno = this.AktifEvrak.RECNO;
        XmlDocument xmlDocument = new XmlDocument();
        string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><transfer xsi:noNamespaceSchemaLocation=\"its.gov.tr/packageTransfer.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></transfer>";
        xmlDocument.LoadXml(xml);
        XmlNode xmlNode = xmlDocument.SelectSingleNode("transfer");
        XmlElement element1 = xmlDocument.CreateElement("sourceGLN");
        element1.InnerText = MyUtils.GetParamValue("GlnNo");
        xmlNode.AppendChild((XmlNode) element1);
        XmlElement element2 = xmlDocument.CreateElement("destinationGLN");
        element2.InnerText = this.AktifEvrak.GLNNO;
        xmlNode.AppendChild((XmlNode) element2);
        XmlElement element3 = xmlDocument.CreateElement("actionType");
        element3.InnerText = "S";
        xmlNode.AppendChild((XmlNode) element3);
        XmlElement element4 = xmlDocument.CreateElement("shipTo");
        xmlNode.AppendChild((XmlNode) element4);
        XmlElement element5 = xmlDocument.CreateElement("documentNumber");
        element5.InnerText = this.EvrakNo;
        xmlNode.AppendChild((XmlNode) element5);
        XmlElement element6 = xmlDocument.CreateElement("documentDate");
        element6.InnerText = Convert.ToDateTime(this.AktifEvrak.TARIH).ToString("yyyy-MM-dd");
        xmlNode.AppendChild((XmlNode) element6);
        XmlElement element7 = xmlDocument.CreateElement("note");
        xmlNode.AppendChild((XmlNode) element7);
        XmlElement element8 = xmlDocument.CreateElement("version");
        element8.InnerText = "V1";
        xmlNode.AppendChild((XmlNode) element8);
        ITSHAR_VIEW itsharView = new ITSHAR_VIEW();
        int num2 = 10000;
        XmlElement newChild1 = (XmlElement) null;
        XmlElement newChild2 = (XmlElement) null;
        foreach (ItsHarClass itsHarClass in this.AktifEvrak.DETAYLAR)
        {
          if (itsharView.BARKOD == itsHarClass.BARKOD & itsharView.PARTINO == itsHarClass.PARTINO & itsharView.MIAD == itsHarClass.MIAD)
          {
            if (newChild2 != null)
            {
              XmlElement element9 = xmlDocument.CreateElement("serialNumber");
              element9.InnerText = itsHarClass.SERI_NO;
              newChild2.AppendChild((XmlNode) element9);
            }
          }
          else
          {
            if (newChild1 != null)
            {
              newChild1.AppendChild((XmlNode) newChild2);
              xmlNode.AppendChild((XmlNode) newChild1);
            }
            ++num2;
            newChild1 = xmlDocument.CreateElement("carrier");
            XmlAttribute attribute1 = xmlDocument.CreateAttribute("carrierLabel");
            attribute1.InnerText = "00000000000000000000";
            newChild1.Attributes.Append(attribute1);
            XmlAttribute attribute2 = xmlDocument.CreateAttribute("containerType");
            attribute2.InnerText = "C";
            newChild1.Attributes.Append(attribute2);
            newChild2 = xmlDocument.CreateElement("productList");
            XmlAttribute attribute3 = xmlDocument.CreateAttribute("GTIN");
            attribute3.InnerText = itsHarClass.BARKOD;
            newChild2.Attributes.Append(attribute3);
            XmlAttribute attribute4 = xmlDocument.CreateAttribute("lotNumber");
            attribute4.InnerText = itsHarClass.PARTINO;
            newChild2.Attributes.Append(attribute4);
            XmlAttribute attribute5 = xmlDocument.CreateAttribute("expirationDate");
            DateTime dateTime;
            try
            {
              dateTime = DateTime.ParseExact(itsHarClass.MIAD, "yyMMdd", (IFormatProvider) null);
            }
            catch
            {
              dateTime = DateTime.ParseExact(itsHarClass.MIAD, new string[2]
              {
                "yyMMdd",
                "yyMM00"
              }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
              dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
            }
            attribute5.InnerText = dateTime.ToString("yyyy-MM-dd");
            newChild2.Attributes.Append(attribute5);
            XmlAttribute attribute6 = xmlDocument.CreateAttribute("PONumber");
            attribute6.InnerText = "";
            newChild2.Attributes.Append(attribute6);
            XmlElement element10 = xmlDocument.CreateElement("serialNumber");
            element10.InnerText = itsHarClass.SERI_NO;
            newChild2.AppendChild((XmlNode) element10);
            itsharView.BARKOD = itsHarClass.BARKOD;
            itsharView.MIAD = itsHarClass.MIAD;
            itsharView.PARTINO = itsHarClass.PARTINO;
          }
        }
        newChild1.AppendChild((XmlNode) newChild2);
        xmlNode.AppendChild((XmlNode) newChild1);
        if (!Directory.Exists("PTS"))
          Directory.CreateDirectory("PTS");
        MemoryStream memoryStream = new MemoryStream();
        File.Delete(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
        File.Delete(Application.StartupPath + "\\PTS\\temp.zip");
        xmlDocument.Save(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
        ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) File.OpenWrite(Application.StartupPath + "\\PTS\\temp.zip"));
        zipOutputStream.PutNextEntry(new ZipEntry(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml")
        {
          CompressionMethod = CompressionMethod.Deflated
        });
        FileStream fileStream = File.OpenRead(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
        byte[] buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, buffer.Length);
        ((Stream) zipOutputStream).Write(buffer, 0, buffer.Length);
        zipOutputStream.Finish();
        ((Stream) zipOutputStream).Close();
        fileStream.Close();
        try
        {
          int num3 = (int) MessageBox.Show("Paket Gönderilmiştir.\rTransfer ID :" + BildirimHelper.PaketGonder(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.AktifEvrak.GLNNO));
        }
        catch (Exception ex)
        {
          int num4 = (int) MessageBox.Show("Hata oluştu:" + ex.Message);
        }
        Cursor.Current = Cursors.Default;
      }
    }

    private void chkHizliMod_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void chkHizliMod_Click(object sender, EventArgs e)
    {
      this.pnlHizli.Visible = this.chkHizliMod.Checked;
      this.txtHizli.Text = "";
    }

    private void btnGuncelle_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.prgDurum2.Minimum = 0;
      this.prgDurum2.Value = 0;
      string[] strArray = this.txtHizli.Text.Replace(";", "").Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      this.prgDurum2.Maximum = strArray.Length;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length; ++index)
      {
        Application.DoEvents();
        if (strArray[index].Split('|').Length > 2)
        {
          string barkod1 = strArray[index].Split('|')[0];
          string barkod = "0" + strArray[index].Split('|')[0];
          string SeriNo = strArray[index].Split('|')[1];
          string Miad = strArray[index].Split('|')[2];
          string PartiNo = strArray[index].Split('|')[3];
          List<TBLSTSABIT> list = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == barkod1)).ToList<TBLSTSABIT>();
          if (list.Count > 0)
          {
            string stokKodu = list[0].STOK_KODU;
            string stokAdi = list[0].STOK_ADI;
            if (this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == barkod & u.SERI_NO == SeriNo & u.PARTINO == PartiNo & u.MIAD == Miad)).ToList<ItsHarClass>().Count == 0)
            {
              if (this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>().Count == 0)
                this.AktifEvrak.STOKLAR.Add(new BildirimStokClass()
                {
                  BARKOD = barkod1,
                  CARI_KOD = this.AktifEvrak.CARI_KOD,
                  EVRAK_SERI = this.EvrakNo,
                  STOK_KODU = list[0].STOK_KODU,
                  STOK_ISIM = stokAdi,
                  TIP = this.EvrakID,
                  REYON_KODU = "BESERI",
                  RECNO = 1,
                  KALAN = 0.0,
                  MIKTAR = 0.0,
                  KARSILANAN = 0
                });
              this.AktifEvrak.DETAYLAR.Add(new ItsHarClass()
              {
                BARKOD = barkod,
                EVRAK_SERI = this.EvrakNo,
                CARI_KOD = this.CariKod,
                DURUM = "",
                MIAD = Miad,
                PARTINO = PartiNo,
                SERI_NO = SeriNo,
                STOK_KOD = list[0].STOK_KODU,
                TIP = this.EvrakID
              });
              ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].MIKTAR;
              ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].KARSILANAN;
              this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod1)).ToList<BildirimStokClass>()[0].KALAN = 0.0;
              this.grdKalem.DataSource = (object) null;
              this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
            }
            else
              stringBuilder.AppendLine(strArray[index]);
          }
        }
        else if (strArray[index].Length > 10)
        {
          string barkod = "";
          string serino = "";
          string miad = "";
          string parti = "";
          if (strArray[index].Substring(0, 3) == "010")
          {
            barkod = strArray[index].Remove(0, 3);
            barkod = barkod.Substring(0, 13);
            serino = strArray[index].Remove(0, 18);
            int num = serino.IndexOf("17");
            serino.IndexOf("10");
            bool flag = false;
            string str1 = serino.Substring(num + 2, 8);
            while (!flag)
            {
              DateTime dateTime;
              if (str1.Substring(6, 2) == "10")
              {
                try
                {
                  dateTime = DateTime.ParseExact(str1.Substring(0, 6), "yyMMdd", (IFormatProvider) null);
                  flag = true;
                }
                catch
                {
                  try
                  {
                    DateTime exact = DateTime.ParseExact(str1.Substring(0, 6), new string[2]
                    {
                      "yyMMdd",
                      "yyMM00"
                    }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
                    dateTime = new DateTime(exact.Year, exact.Month, DateTime.DaysInMonth(exact.Year, exact.Month));
                    flag = true;
                  }
                  catch
                  {
                  }
                }
              }
              if (!flag)
              {
                num = serino.IndexOf("17", num + 1);
                str1 = serino.Substring(num + 2, 8);
              }
            }
            serino = serino.Substring(0, serino.IndexOf(str1) - 2);
            miad = str1.Substring(0, 6);
            string str2 = strArray[index];
            parti = "010" + barkod + "21" + serino + "17" + miad + "10";
            parti = str2.Replace(parti, "");
            List<TBLSTSABIT> list = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == barkod)).ToList<TBLSTSABIT>();
            if (list.Count > 0)
            {
              string stokKodu = list[0].STOK_KODU;
              string stokAdi = list[0].STOK_ADI;
              if (this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == barkod & u.SERI_NO == serino & u.PARTINO == parti & u.MIAD == miad)).ToList<ItsHarClass>().Count == 0)
              {
                if (this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>().Count == 0)
                  this.AktifEvrak.STOKLAR.Add(new BildirimStokClass()
                  {
                    BARKOD = barkod,
                    CARI_KOD = this.AktifEvrak.CARI_KOD,
                    EVRAK_SERI = this.EvrakNo,
                    STOK_KODU = list[0].STOK_KODU,
                    STOK_ISIM = stokAdi,
                    TIP = this.EvrakID,
                    REYON_KODU = "BESERI",
                    RECNO = 1,
                    KALAN = 0.0,
                    MIKTAR = 0.0,
                    KARSILANAN = 0
                  });
                this.AktifEvrak.DETAYLAR.Add(new ItsHarClass()
                {
                  BARKOD = "0" + barkod,
                  EVRAK_SERI = this.EvrakNo,
                  CARI_KOD = this.CariKod,
                  DURUM = "",
                  MIAD = miad,
                  PARTINO = parti,
                  SERI_NO = serino,
                  STOK_KOD = list[0].STOK_KODU,
                  TIP = this.EvrakID
                });
                ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].MIKTAR;
                ++this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KARSILANAN;
                this.AktifEvrak.STOKLAR.Where<BildirimStokClass>((Func<BildirimStokClass, bool>) (u => u.BARKOD == barkod)).ToList<BildirimStokClass>()[0].KALAN = 0.0;
                this.grdKalem.DataSource = (object) null;
                this.grdKalem.DataSource = (object) this.AktifEvrak.STOKLAR;
              }
              else
                stringBuilder.AppendLine(strArray[index]);
            }
          }
        }
        this.prgDurum2.Value = index + 1;
      }
      int num1 = (int) MessageBox.Show("Aktarım tamamlanmıştır.");
      Cursor.Current = Cursors.Default;
      this.prgDurum2.Value = 0;
      if (stringBuilder.Length <= 1)
        return;
      FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
      frmKoliSonuc.Sonuc = stringBuilder.ToString();
      int num2 = (int) frmKoliSonuc.ShowDialog();
      frmKoliSonuc.Dispose();
    }

    private void btnTemizle_Click(object sender, EventArgs e) => this.txtHizli.Text = "";

    private void btnSorgula_Click(object sender, EventArgs e)
    {
    }

    private void FrmSerbestBildirim_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F3 || this.xtraTabControl1.SelectedTabPage != this.xtraTabPage2)
        return;
      this.btnKarekod_Click(sender, (EventArgs) null);
    }

    private void FrmSerbestBildirim_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
    }

    private void exceleGönderToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.grdKalemDetay.ExportToXls(this.saveFileDialog1.FileName);
    }

    private void btnDogrulama_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      string empty = string.Empty;
      try
      {
        this.Cursor = Cursors.WaitCursor;
        MyUtils.Refresh();
        int num1 = 0;
        int num2 = 0;
        List<FrmSerbestBildirim.HataListe> source1 = new List<FrmSerbestBildirim.HataListe>();
        source1.Add(new FrmSerbestBildirim.HataListe(10007, "Bu Sira Numarasi Zaten Kayitli!", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10008, "Tanimlanmamis Kayit Hatasi.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10201, "Belirtilen urun Sistemimizde Kayitli Degildir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10202, "Ürünün Son Kullanma Tarihi Gecmistir. (Hastaya verilemez.)", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10203, "Ürün Bilgileri Tutarsiz.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10204, "Belirtilen ürün onceden Satilmistir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10205, "Bu ürünün Satisi Yasaklanmistir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10206, "Veritabanı Kayıt Hatası.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10207, "Bu ürün önceden ihrac edilmiştir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10209, "Ürün şu anda başka bir Eczane stoğunda görünüyor.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10210, "Ürün stoğunuzda görünüyor", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10211, "Ürün Stoğunuzda Görünmüyor!", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10214, "Ürün tarafınızdan önceden satılmıstır!", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10219, "Belirtilen ürün tarafınızdan satılmamıştır. Sadece Kendi sattığınız ilacı geri alabilirsiniz", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10220, "Ürün Geri ödeme kurumuna satılmıstır. Satışın Recete Bazlı iptal edilmesi gerekir", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10221, "Ürünün Satisi Iptal Edilemez.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10222, "Ürün Üzerinize Kayitli Degil", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10223, "Ürün Üzerinize Kayitli Görünüyor", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10224, "Ürün Eczane Tarafindan Satilmistir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10225, "Ürün su anda baska bir birimde görünüyor.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10227, "Girilen GLN, eczane GLNsi degildir. GLNnin size ait oldugundan emin olunuz.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10301, "Girilen satici GLNsi yanlistir. Ürünü aldiginiz paydasin GLNsini belirttiginizden emin olunuz.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10302, "Girilen alici GLNsi yanlistir. Ürünü sattiginiz paydasin GLNsini belirttiginizden emin olunuz.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10303, "Belirtilen ürün üzerinize kayitlidir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10304, "Belirtilen ürün üzerinize kayitli degildir.", 0));
        source1.Add(new FrmSerbestBildirim.HataListe(10305, "Belirtilen ürün baska bir paydas üzerine kayitlidir.", 0));
        List<KarekodBilgi> its = new List<KarekodBilgi>();
        foreach (ItsHarClass itsHarClass in this.AktifEvrak.DETAYLAR)
        {
          string miad = BildirimHelper.ConvertMiadTarih(itsHarClass.MIAD).ToString("yyyy-MM-dd");
          its.Add(new KarekodBilgi(itsHarClass.BARKOD, itsHarClass.SERI_NO, itsHarClass.PARTINO, miad));
        }
        List<KarekodBilgi> karekodBilgiList = BildirimHelper.DogrulamaBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), MyUtils.GetParamValue("GlnNo"), its);
        int num3 = 0;
        List<ItsHarClass> source2 = new List<ItsHarClass>();
        foreach (ItsHarClass itsHarClass1 in this.AktifEvrak.DETAYLAR)
        {
          ItsHarClass item = itsHarClass1;
          ItsHarClass itsHarClass2 = new ItsHarClass();
          itsHarClass2.BARKOD = item.BARKOD;
          itsHarClass2.CARI_KOD = item.CARI_KOD;
          itsHarClass2.DURUM = item.DURUM;
          itsHarClass2.EVRAK_SERI = item.EVRAK_SERI;
          itsHarClass2.ID = item.ID;
          itsHarClass2.MIAD = item.MIAD;
          itsHarClass2.PARTINO = item.PARTINO;
          itsHarClass2.SERI_NO = item.SERI_NO;
          TBLSTSABIT tblstsabit = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (o => o.STOK_KODU == item.STOK_KOD)).FirstOrDefault<TBLSTSABIT>();
          if (tblstsabit != null)
            itsHarClass2.STOK_ISIM = tblstsabit.STOK_ADI;
          source2.Add(itsHarClass2);
          ++num3;
        }
        foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
        {
          KarekodBilgi item = karekodBilgi;
          item.Barkod.Remove(0, 1);
          ItsHarClass h = source2.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo)).FirstOrDefault<ItsHarClass>();
          if (h != null)
          {
            h.DURUM = item.Sonuc;
            if (item.Sonuc == "00000")
              ++num1;
            source1.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad = source1.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad + 1;
            h.MESAJ = source1.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Durum;
            source2[source2.IndexOf(h)].DURUM = h.DURUM;
          }
          ++num2;
        }
        MyUtils.Refresh();
        this.grdKalemDetay.DataSource = (object) source2;
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.AppendLine("Bildirim Detayları : ");
        foreach (FrmSerbestBildirim.HataListe hataListe in source1)
        {
          if (hataListe.Ad > 0)
          {
            stringBuilder2.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
            hataListe.Ad = 0;
          }
        }
        stringBuilder2.AppendLine("Bildirim Durumu : " + num1.ToString() + " / " + num2.ToString());
        this.memSonuc.Text = stringBuilder2.ToString();
        this.Cursor = Cursors.Default;
        this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
      }
    }

    private void btnBildirim_Click(object sender, EventArgs e)
    {
    }

    private void btnTakas_1_Click(object sender, EventArgs e)
    {
      if (sender != this.mnuDepo1 && sender != this.mnuDepo2 && sender != this.mnuDepo3)
        return;
      string str = "";
      string paramValue1;
      string paramValue2;
      if (sender == this.mnuDepo1)
      {
        str = MyUtils.GetParamValue("EczGlnNo1");
        paramValue1 = MyUtils.GetParamValue("EczKullanici1");
        paramValue2 = MyUtils.GetParamValue("EczSifre1");
      }
      else if (sender == this.mnuDepo2)
      {
        str = MyUtils.GetParamValue("EczGlnNo2");
        paramValue1 = MyUtils.GetParamValue("EczKullanici2");
        paramValue2 = MyUtils.GetParamValue("EczSifre2");
      }
      else
      {
        str = MyUtils.GetParamValue("EczGlnNo3");
        paramValue1 = MyUtils.GetParamValue("EczKullanici3");
        paramValue2 = MyUtils.GetParamValue("EczSifre3");
      }
      try
      {
        if (this.AktifEvrak.STOKLAR.Count == 0)
        {
          int num1 = (int) MessageBox.Show("Lütfen ürün okutunuz");
        }
        else
        {
          this.Cursor = Cursors.WaitCursor;
          int num2 = 0;
          int num3 = 0;
          List<FrmSerbestBildirim.HataListe> source = new List<FrmSerbestBildirim.HataListe>();
          foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) MyUtils.Firma.HATA_KODLARI)
            source.Add(new FrmSerbestBildirim.HataListe()
            {
              HataKod = hataKodlari.HATAID,
              Ad = 0,
              Durum = hataKodlari.MESAJ
            });
          List<KarekodBilgi> its = new List<KarekodBilgi>();
          foreach (ItsHarClass itsHarClass in this.AktifEvrak.DETAYLAR)
          {
            string miad = BildirimHelper.ConvertMiadTarih(itsHarClass.MIAD).ToString("yyyy-MM-dd");
            its.Add(new KarekodBilgi(itsHarClass.BARKOD, itsHarClass.SERI_NO, itsHarClass.PARTINO, miad));
          }
          List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              karekodBilgiList = BildirimHelper.MalDevirBildirimi(paramValue1, paramValue2, this.AktifEvrak.GLNNO, its);
              break;
            case EvrakTipi.ALIS:
              karekodBilgiList = BildirimHelper.MalAlimBildirimi(paramValue1, paramValue2, its);
              break;
            case EvrakTipi.SATIS_IPTAL:
              karekodBilgiList = BildirimHelper.MalDevirIptalBildirimi(paramValue1, paramValue2, its);
              break;
            case EvrakTipi.ALIS_IPTAL:
              karekodBilgiList = BildirimHelper.MalIadeBildirimi(paramValue1, paramValue2, this.AktifEvrak.GLNNO, its);
              break;
          }
          foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
          {
            KarekodBilgi item = karekodBilgi;
            item.Barkod.Remove(0, 1);
            ItsHarClass h = this.AktifEvrak.DETAYLAR.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo & u.TIP == this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).ToList<ItsHarClass>()[0];
            if (h != null)
            {
              h.DURUM = item.Sonuc;
              if (item.Sonuc == "00000")
                ++num2;
              source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad = source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Ad + 1;
              h.MESAJ = source.Where<FrmSerbestBildirim.HataListe>((Func<FrmSerbestBildirim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSerbestBildirim.HataListe>()[0].Durum;
            }
            ++num3;
          }
          this.grdKalemDetay.DataSource = (object) null;
          this.grdKalemDetay.DataSource = (object) this.AktifEvrak.DETAYLAR;
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("Bildirim Detayları : ");
          foreach (FrmSerbestBildirim.HataListe hataListe in source)
          {
            if (hataListe.Ad > 0)
            {
              stringBuilder.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
              hataListe.Ad = 0;
            }
          }
          stringBuilder.AppendLine("Bildirim Durumu : " + num2.ToString() + " / " + num3.ToString());
          this.memSonuc.Text = stringBuilder.ToString();
          this.Cursor = Cursors.Default;
          this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
          if (this.EvrakTip != EvrakTipi.SATIS || num2 != num3 || MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
          this.btnPaketAl_Click(sender, e);
        }
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
      }
    }

    private void btnTakas_Click(object sender, EventArgs e)
    {
      this.mnuDepo1.Text = MyUtils.GetParamValue("Eczane1");
      this.mnuDepo2.Text = MyUtils.GetParamValue("Eczane2");
      this.mnuDepo3.Text = MyUtils.GetParamValue("Eczane3");
      this.contextMenuStrip2.Show((Control) this.btnTakas, new Point(this.btnTakas.Width, 0));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.defaultLookAndFeel1 = new DefaultLookAndFeel(this.components);
      this.panelControl1 = new PanelControl();
      this.cbTip = new System.Windows.Forms.ComboBox();
      this.xtraTabControl1 = new XtraTabControl();
      this.xtraTabPage2 = new XtraTabPage();
      this.grdKalem = new GridControl();
      this.gridView2 = new GridView();
      this.colRECNO1 = new GridColumn();
      this.colCARIHR_RECNO = new GridColumn();
      this.gridColumn1 = new GridColumn();
      this.colSTOK_KODU = new GridColumn();
      this.colSTOK_ISIM = new GridColumn();
      this.colMIKTAR = new GridColumn();
      this.colKARSILANAN = new GridColumn();
      this.gridColumn2 = new GridColumn();
      this.gridColumn3 = new GridColumn();
      this.pnlHizli = new Panel();
      this.txtHizli = new MemoEdit();
      this.panel4 = new Panel();
      this.panel5 = new Panel();
      this.prgDurum2 = new System.Windows.Forms.ProgressBar();
      this.btnTemizle = new SimpleButton();
      this.btnGuncelle = new SimpleButton();
      this.panelControl4 = new PanelControl();
      this.panel1 = new Panel();
      this.prgDurum = new System.Windows.Forms.ProgressBar();
      this.btnTakas = new SimpleButton();
      this.btnDogrulama = new SimpleButton();
      this.btnGonder = new SimpleButton();
      this.btnSeriAktar = new SimpleButton();
      this.btnExcelAktar = new SimpleButton();
      this.btnImport = new SimpleButton();
      this.btnBildirim = new SimpleButton();
      this.btnKarekod = new SimpleButton();
      this.pnlUst = new PanelControl();
      this.chkHizliMod = new CheckBox();
      this.lblCari = new Label();
      this.label1 = new Label();
      this.txtEvrak = new TextBox();
      this.btnEvrak = new SimpleButton();
      this.xtraTabPage3 = new XtraTabPage();
      this.panelControl7 = new PanelControl();
      this.grdKalemDetay = new GridControl();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.exceleGönderToolStripMenuItem = new ToolStripMenuItem();
      this.gridView3 = new GridView();
      this.colID = new GridColumn();
      this.colTIP = new GridColumn();
      this.colCARIHR_RECNO1 = new GridColumn();
      this.colSTOK_KOD = new GridColumn();
      this.colSTOK_ISIM1 = new GridColumn();
      this.colBARKOD = new GridColumn();
      this.colSERI_NO = new GridColumn();
      this.colMIAD = new GridColumn();
      this.colPARTINO = new GridColumn();
      this.colDURUM = new GridColumn();
      this.btnSorgula = new SimpleButton();
      this.splitterControl1 = new SplitterControl();
      this.panelControl6 = new PanelControl();
      this.memSonuc = new MemoEdit();
      this.gridView1 = new GridView();
      this.saveFileDialog1 = new SaveFileDialog();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.mnuDepo1 = new ToolStripMenuItem();
      this.mnuDepo2 = new ToolStripMenuItem();
      this.mnuDepo3 = new ToolStripMenuItem();
      this.panelControl1.BeginInit();
      this.panelControl1.SuspendLayout();
      this.xtraTabControl1.BeginInit();
      this.xtraTabControl1.SuspendLayout();
      this.xtraTabPage2.SuspendLayout();
      this.grdKalem.BeginInit();
      this.gridView2.BeginInit();
      this.pnlHizli.SuspendLayout();
      this.txtHizli.Properties.BeginInit();
      this.panel4.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panelControl4.BeginInit();
      this.panelControl4.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlUst.BeginInit();
      this.pnlUst.SuspendLayout();
      this.xtraTabPage3.SuspendLayout();
      this.panelControl7.BeginInit();
      this.panelControl7.SuspendLayout();
      this.grdKalemDetay.BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.gridView3.BeginInit();
      this.panelControl6.BeginInit();
      this.panelControl6.SuspendLayout();
      this.memSonuc.Properties.BeginInit();
      this.gridView1.BeginInit();
      this.contextMenuStrip2.SuspendLayout();
      this.SuspendLayout();
      this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Silver";
      this.panelControl1.Controls.Add((Control) this.cbTip);
      this.panelControl1.Dock = DockStyle.Top;
      this.panelControl1.Location = new Point(0, 0);
      this.panelControl1.Name = "panelControl1";
      this.panelControl1.Size = new Size(924, 40);
      this.panelControl1.TabIndex = 0;
      this.cbTip.Dock = DockStyle.Fill;
      this.cbTip.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTip.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.cbTip.FormattingEnabled = true;
      this.cbTip.Items.AddRange(new object[5]
      {
        (object) "Satış Bildirimi",
        (object) "Alış Bildirimi",
        (object) "Satış İptal Bildirimi",
        (object) "Alış İptal Bildirimi",
        (object) "Deaktivasyon"
      });
      this.cbTip.Location = new Point(2, 2);
      this.cbTip.Name = "cbTip";
      this.cbTip.Size = new Size(920, 37);
      this.cbTip.TabIndex = 3;
      this.cbTip.SelectedIndexChanged += new EventHandler(this.cbTip_SelectedIndexChanged);
      this.xtraTabControl1.Dock = DockStyle.Fill;
      this.xtraTabControl1.HeaderAutoFill = DefaultBoolean.False;
      this.xtraTabControl1.Location = new Point(0, 40);
      this.xtraTabControl1.LookAndFeel.SkinName = "VS2010";
      this.xtraTabControl1.Name = "xtraTabControl1";
      this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
      this.xtraTabControl1.Size = new Size(924, 470);
      this.xtraTabControl1.TabIndex = 2;
      this.xtraTabControl1.TabPages.AddRange(new XtraTabPage[2]
      {
        this.xtraTabPage2,
        this.xtraTabPage3
      });
      this.xtraTabPage2.Controls.Add((Control) this.grdKalem);
      this.xtraTabPage2.Controls.Add((Control) this.pnlHizli);
      this.xtraTabPage2.Controls.Add((Control) this.panelControl4);
      this.xtraTabPage2.Controls.Add((Control) this.pnlUst);
      this.xtraTabPage2.Name = "xtraTabPage2";
      this.xtraTabPage2.Size = new Size(918, 442);
      this.xtraTabPage2.Text = "Fatura Kalemleri";
      this.grdKalem.Dock = DockStyle.Fill;
      this.grdKalem.Location = new Point(0, 157);
      this.grdKalem.MainView = (BaseView) this.gridView2;
      this.grdKalem.Name = "grdKalem";
      this.grdKalem.Size = new Size(918, 244);
      this.grdKalem.TabIndex = 12;
      this.grdKalem.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView2
      });
      this.gridView2.Appearance.EvenRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.EvenRow.Options.UseFont = true;
      this.gridView2.Appearance.FocusedRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.FocusedRow.Options.UseFont = true;
      this.gridView2.Appearance.FooterPanel.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.FooterPanel.ForeColor = Color.Red;
      this.gridView2.Appearance.FooterPanel.Options.UseFont = true;
      this.gridView2.Appearance.FooterPanel.Options.UseForeColor = true;
      this.gridView2.Appearance.OddRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.OddRow.Options.UseFont = true;
      this.gridView2.Appearance.Row.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.Row.Options.UseFont = true;
      this.gridView2.Columns.AddRange(new GridColumn[9]
      {
        this.colRECNO1,
        this.colCARIHR_RECNO,
        this.gridColumn1,
        this.colSTOK_KODU,
        this.colSTOK_ISIM,
        this.colMIKTAR,
        this.colKARSILANAN,
        this.gridColumn2,
        this.gridColumn3
      });
      this.gridView2.GridControl = this.grdKalem;
      this.gridView2.GroupSummary.AddRange(new GridSummaryItem[3]
      {
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "MIKTAR", (GridColumn) null, "0"),
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "KARSILANAN", (GridColumn) null, "0"),
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "KALAN", (GridColumn) null, "0")
      });
      this.gridView2.Name = "gridView2";
      this.gridView2.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView2.OptionsBehavior.Editable = false;
      this.gridView2.OptionsView.ShowFooter = true;
      this.gridView2.OptionsView.ShowGroupedColumns = true;
      this.gridView2.OptionsView.ShowGroupPanel = false;
      this.colRECNO1.FieldName = "RECNO";
      this.colRECNO1.Name = "colRECNO1";
      this.colCARIHR_RECNO.FieldName = "CARIHR_RECNO";
      this.colCARIHR_RECNO.Name = "colCARIHR_RECNO";
      this.gridColumn1.Caption = "Barkod";
      this.gridColumn1.FieldName = "BARKOD";
      this.gridColumn1.Name = "gridColumn1";
      this.gridColumn1.Visible = true;
      this.gridColumn1.VisibleIndex = 1;
      this.gridColumn1.Width = 89;
      this.colSTOK_KODU.Caption = "Stok Kodu";
      this.colSTOK_KODU.FieldName = "STOK_KODU";
      this.colSTOK_KODU.Name = "colSTOK_KODU";
      this.colSTOK_KODU.Visible = true;
      this.colSTOK_KODU.VisibleIndex = 2;
      this.colSTOK_KODU.Width = 89;
      this.colSTOK_ISIM.Caption = "Stok İsmi";
      this.colSTOK_ISIM.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM.Name = "colSTOK_ISIM";
      this.colSTOK_ISIM.Visible = true;
      this.colSTOK_ISIM.VisibleIndex = 3;
      this.colSTOK_ISIM.Width = 424;
      this.colMIKTAR.Caption = "Miktar";
      this.colMIKTAR.FieldName = "MIKTAR";
      this.colMIKTAR.Name = "colMIKTAR";
      this.colMIKTAR.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colMIKTAR.Visible = true;
      this.colMIKTAR.VisibleIndex = 4;
      this.colMIKTAR.Width = 70;
      this.colKARSILANAN.Caption = "Karşılanan";
      this.colKARSILANAN.FieldName = "KARSILANAN";
      this.colKARSILANAN.Name = "colKARSILANAN";
      this.colKARSILANAN.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colKARSILANAN.Visible = true;
      this.colKARSILANAN.VisibleIndex = 5;
      this.colKARSILANAN.Width = 60;
      this.gridColumn2.Caption = "Kalan";
      this.gridColumn2.FieldName = "KALAN";
      this.gridColumn2.Name = "gridColumn2";
      this.gridColumn2.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.gridColumn2.Visible = true;
      this.gridColumn2.VisibleIndex = 6;
      this.gridColumn2.Width = 76;
      this.gridColumn3.Caption = "BEŞERİ DURUM";
      this.gridColumn3.FieldName = "REYON_KODU";
      this.gridColumn3.Name = "gridColumn3";
      this.gridColumn3.Visible = true;
      this.gridColumn3.VisibleIndex = 0;
      this.gridColumn3.Width = 88;
      this.pnlHizli.Controls.Add((Control) this.txtHizli);
      this.pnlHizli.Controls.Add((Control) this.panel4);
      this.pnlHizli.Dock = DockStyle.Top;
      this.pnlHizli.Location = new Point(0, 32);
      this.pnlHizli.Name = "pnlHizli";
      this.pnlHizli.Size = new Size(918, 125);
      this.pnlHizli.TabIndex = 11;
      this.pnlHizli.Visible = false;
      this.txtHizli.Dock = DockStyle.Fill;
      this.txtHizli.Location = new Point(0, 0);
      this.txtHizli.Name = "txtHizli";
      this.txtHizli.Size = new Size(918, 97);
      this.txtHizli.TabIndex = 1;
      this.panel4.Controls.Add((Control) this.panel5);
      this.panel4.Controls.Add((Control) this.btnTemizle);
      this.panel4.Controls.Add((Control) this.btnGuncelle);
      this.panel4.Dock = DockStyle.Bottom;
      this.panel4.Location = new Point(0, 97);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(918, 28);
      this.panel4.TabIndex = 0;
      this.panel5.Controls.Add((Control) this.prgDurum2);
      this.panel5.Dock = DockStyle.Fill;
      this.panel5.Location = new Point(190, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(728, 28);
      this.panel5.TabIndex = 18;
      this.prgDurum2.Dock = DockStyle.Fill;
      this.prgDurum2.Location = new Point(0, 0);
      this.prgDurum2.Name = "prgDurum2";
      this.prgDurum2.Size = new Size(728, 28);
      this.prgDurum2.TabIndex = 9;
      this.btnTemizle.Appearance.ForeColor = Color.Black;
      this.btnTemizle.Appearance.Options.UseForeColor = true;
      this.btnTemizle.ButtonStyle = BorderStyles.Office2003;
      this.btnTemizle.Dock = DockStyle.Left;
      this.btnTemizle.Location = new Point(95, 0);
      this.btnTemizle.Name = "btnTemizle";
      this.btnTemizle.Size = new Size(95, 28);
      this.btnTemizle.TabIndex = 17;
      this.btnTemizle.Text = "Temizle";
      this.btnTemizle.Click += new EventHandler(this.btnTemizle_Click);
      this.btnGuncelle.Appearance.ForeColor = Color.Black;
      this.btnGuncelle.Appearance.Options.UseForeColor = true;
      this.btnGuncelle.ButtonStyle = BorderStyles.Office2003;
      this.btnGuncelle.Dock = DockStyle.Left;
      this.btnGuncelle.Location = new Point(0, 0);
      this.btnGuncelle.Name = "btnGuncelle";
      this.btnGuncelle.Size = new Size(95, 28);
      this.btnGuncelle.TabIndex = 15;
      this.btnGuncelle.Text = "Güncelle";
      this.btnGuncelle.Click += new EventHandler(this.btnGuncelle_Click);
      this.panelControl4.Controls.Add((Control) this.panel1);
      this.panelControl4.Controls.Add((Control) this.btnTakas);
      this.panelControl4.Controls.Add((Control) this.btnDogrulama);
      this.panelControl4.Controls.Add((Control) this.btnGonder);
      this.panelControl4.Controls.Add((Control) this.btnSeriAktar);
      this.panelControl4.Controls.Add((Control) this.btnExcelAktar);
      this.panelControl4.Controls.Add((Control) this.btnImport);
      this.panelControl4.Controls.Add((Control) this.btnBildirim);
      this.panelControl4.Controls.Add((Control) this.btnKarekod);
      this.panelControl4.Dock = DockStyle.Bottom;
      this.panelControl4.Location = new Point(0, 401);
      this.panelControl4.Name = "panelControl4";
      this.panelControl4.Size = new Size(918, 41);
      this.panelControl4.TabIndex = 3;
      this.panel1.Controls.Add((Control) this.prgDurum);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(382, 2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(154, 37);
      this.panel1.TabIndex = 25;
      this.prgDurum.Dock = DockStyle.Fill;
      this.prgDurum.Location = new Point(0, 0);
      this.prgDurum.Name = "prgDurum";
      this.prgDurum.Size = new Size(154, 37);
      this.prgDurum.TabIndex = 9;
      this.btnTakas.Appearance.ForeColor = Color.Black;
      this.btnTakas.Appearance.Options.UseForeColor = true;
      this.btnTakas.ButtonStyle = BorderStyles.Office2003;
      this.btnTakas.Dock = DockStyle.Left;
      this.btnTakas.Location = new Point(287, 2);
      this.btnTakas.Name = "btnTakas";
      this.btnTakas.Size = new Size(95, 37);
      this.btnTakas.TabIndex = 24;
      this.btnTakas.Text = "Bildirim 2";
      this.btnTakas.Click += new EventHandler(this.btnTakas_Click);
      this.btnDogrulama.Appearance.ForeColor = Color.Black;
      this.btnDogrulama.Appearance.Options.UseForeColor = true;
      this.btnDogrulama.ButtonStyle = BorderStyles.Office2003;
      this.btnDogrulama.Dock = DockStyle.Left;
      this.btnDogrulama.Location = new Point(192, 2);
      this.btnDogrulama.Name = "btnDogrulama";
      this.btnDogrulama.Size = new Size(95, 37);
      this.btnDogrulama.TabIndex = 16;
      this.btnDogrulama.Text = "Doğrulama";
      this.btnDogrulama.Click += new EventHandler(this.btnDogrulama_Click);
      this.btnGonder.Appearance.ForeColor = Color.Black;
      this.btnGonder.Appearance.Options.UseForeColor = true;
      this.btnGonder.ButtonStyle = BorderStyles.Office2003;
      this.btnGonder.Dock = DockStyle.Left;
      this.btnGonder.Location = new Point(97, 2);
      this.btnGonder.Name = "btnGonder";
      this.btnGonder.Size = new Size(95, 37);
      this.btnGonder.TabIndex = 14;
      this.btnGonder.Text = "Paket Gönder";
      this.btnGonder.Click += new EventHandler(this.btnPaketAl_Click);
      this.btnSeriAktar.Dock = DockStyle.Right;
      this.btnSeriAktar.Location = new Point(536, 2);
      this.btnSeriAktar.Name = "btnSeriAktar";
      this.btnSeriAktar.Size = new Size(95, 37);
      this.btnSeriAktar.TabIndex = 11;
      this.btnSeriAktar.Text = "Seri Aktar";
      this.btnSeriAktar.Click += new EventHandler(this.btnSeriAktar_Click);
      this.btnExcelAktar.Dock = DockStyle.Right;
      this.btnExcelAktar.Location = new Point(631, 2);
      this.btnExcelAktar.Name = "btnExcelAktar";
      this.btnExcelAktar.Size = new Size(95, 37);
      this.btnExcelAktar.TabIndex = 9;
      this.btnExcelAktar.Text = "Excel Aktar";
      this.btnExcelAktar.Click += new EventHandler(this.btnExcelAktar_Click);
      this.btnImport.Dock = DockStyle.Right;
      this.btnImport.Location = new Point(726, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(95, 37);
      this.btnImport.TabIndex = 7;
      this.btnImport.Text = "Dosya Aktar";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.btnBildirim.Appearance.ForeColor = Color.Black;
      this.btnBildirim.Appearance.Options.UseForeColor = true;
      this.btnBildirim.ButtonStyle = BorderStyles.Office2003;
      this.btnBildirim.Dock = DockStyle.Left;
      this.btnBildirim.Location = new Point(2, 2);
      this.btnBildirim.Name = "btnBildirim";
      this.btnBildirim.Size = new Size(95, 37);
      this.btnBildirim.TabIndex = 5;
      this.btnBildirim.Text = "Bildirim Yap";
      this.btnBildirim.Click += new EventHandler(this.btnBitir1_Click);
      this.btnKarekod.Dock = DockStyle.Right;
      this.btnKarekod.Location = new Point(821, 2);
      this.btnKarekod.Name = "btnKarekod";
      this.btnKarekod.Size = new Size(95, 37);
      this.btnKarekod.TabIndex = 3;
      this.btnKarekod.Text = "Karekod Oku (F3)";
      this.btnKarekod.Click += new EventHandler(this.btnKarekod_Click);
      this.pnlUst.Controls.Add((Control) this.chkHizliMod);
      this.pnlUst.Controls.Add((Control) this.lblCari);
      this.pnlUst.Controls.Add((Control) this.label1);
      this.pnlUst.Controls.Add((Control) this.txtEvrak);
      this.pnlUst.Controls.Add((Control) this.btnEvrak);
      this.pnlUst.Dock = DockStyle.Top;
      this.pnlUst.Location = new Point(0, 0);
      this.pnlUst.Name = "pnlUst";
      this.pnlUst.Size = new Size(918, 32);
      this.pnlUst.TabIndex = 0;
      this.chkHizliMod.AutoSize = true;
      this.chkHizliMod.Dock = DockStyle.Right;
      this.chkHizliMod.Location = new Point(733, 2);
      this.chkHizliMod.Name = "chkHizliMod";
      this.chkHizliMod.Size = new Size(67, 28);
      this.chkHizliMod.TabIndex = 5;
      this.chkHizliMod.Text = "Hızlı Mod";
      this.chkHizliMod.UseVisualStyleBackColor = true;
      this.chkHizliMod.CheckedChanged += new EventHandler(this.chkHizliMod_CheckedChanged);
      this.chkHizliMod.Click += new EventHandler(this.chkHizliMod_Click);
      this.lblCari.AutoSize = true;
      this.lblCari.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.lblCari.ForeColor = Color.FromArgb(0, 64, 0);
      this.lblCari.Location = new Point(172, 3);
      this.lblCari.Name = "lblCari";
      this.lblCari.Size = new Size(0, 24);
      this.lblCari.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Dock = DockStyle.Left;
      this.label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.Location = new Point(2, 2);
      this.label1.Name = "label1";
      this.label1.Size = new Size(62, 24);
      this.label1.TabIndex = 2;
      this.label1.Text = "Evrak ";
      this.txtEvrak.Location = new Point(66, 6);
      this.txtEvrak.Name = "txtEvrak";
      this.txtEvrak.Size = new Size(100, 21);
      this.txtEvrak.TabIndex = 1;
      this.btnEvrak.Dock = DockStyle.Right;
      this.btnEvrak.Location = new Point(800, 2);
      this.btnEvrak.Name = "btnEvrak";
      this.btnEvrak.Size = new Size(116, 28);
      this.btnEvrak.TabIndex = 0;
      this.btnEvrak.Text = "Yeni Bildirim";
      this.btnEvrak.Click += new EventHandler(this.btnEvrak_Click);
      this.xtraTabPage3.Controls.Add((Control) this.panelControl7);
      this.xtraTabPage3.Controls.Add((Control) this.btnSorgula);
      this.xtraTabPage3.Controls.Add((Control) this.splitterControl1);
      this.xtraTabPage3.Controls.Add((Control) this.panelControl6);
      this.xtraTabPage3.Name = "xtraTabPage3";
      this.xtraTabPage3.Size = new Size(918, 442);
      this.xtraTabPage3.Text = "Bildirim Sonuç";
      this.panelControl7.Controls.Add((Control) this.grdKalemDetay);
      this.panelControl7.Dock = DockStyle.Fill;
      this.panelControl7.Location = new Point(0, 23);
      this.panelControl7.Name = "panelControl7";
      this.panelControl7.Size = new Size(918, 258);
      this.panelControl7.TabIndex = 10;
      this.grdKalemDetay.ContextMenuStrip = this.contextMenuStrip1;
      this.grdKalemDetay.Dock = DockStyle.Fill;
      this.grdKalemDetay.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdKalemDetay.Location = new Point(2, 2);
      this.grdKalemDetay.MainView = (BaseView) this.gridView3;
      this.grdKalemDetay.Name = "grdKalemDetay";
      this.grdKalemDetay.Size = new Size(914, 254);
      this.grdKalemDetay.TabIndex = 3;
      this.grdKalemDetay.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.exceleGönderToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(150, 26);
      this.exceleGönderToolStripMenuItem.Name = "exceleGönderToolStripMenuItem";
      this.exceleGönderToolStripMenuItem.Size = new Size(149, 22);
      this.exceleGönderToolStripMenuItem.Text = "Excele Gönder";
      this.exceleGönderToolStripMenuItem.Click += new EventHandler(this.exceleGönderToolStripMenuItem_Click);
      this.gridView3.Columns.AddRange(new GridColumn[10]
      {
        this.colID,
        this.colTIP,
        this.colCARIHR_RECNO1,
        this.colSTOK_KOD,
        this.colSTOK_ISIM1,
        this.colBARKOD,
        this.colSERI_NO,
        this.colMIAD,
        this.colPARTINO,
        this.colDURUM
      });
      this.gridView3.GridControl = this.grdKalemDetay;
      this.gridView3.Name = "gridView3";
      this.gridView3.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView3.OptionsBehavior.Editable = false;
      this.gridView3.OptionsView.ShowGroupPanel = false;
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colTIP.FieldName = "TIP";
      this.colTIP.Name = "colTIP";
      this.colCARIHR_RECNO1.FieldName = "CARIHR_RECNO";
      this.colCARIHR_RECNO1.Name = "colCARIHR_RECNO1";
      this.colSTOK_KOD.Caption = "Stok Kodu";
      this.colSTOK_KOD.FieldName = "STOK_KOD";
      this.colSTOK_KOD.Name = "colSTOK_KOD";
      this.colSTOK_KOD.Visible = true;
      this.colSTOK_KOD.VisibleIndex = 0;
      this.colSTOK_KOD.Width = 113;
      this.colSTOK_ISIM1.Caption = "Stok İsmi";
      this.colSTOK_ISIM1.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM1.Name = "colSTOK_ISIM1";
      this.colSTOK_ISIM1.Visible = true;
      this.colSTOK_ISIM1.VisibleIndex = 1;
      this.colSTOK_ISIM1.Width = 130;
      this.colBARKOD.Caption = "Barkod";
      this.colBARKOD.FieldName = "BARKOD";
      this.colBARKOD.Name = "colBARKOD";
      this.colBARKOD.Visible = true;
      this.colBARKOD.VisibleIndex = 2;
      this.colBARKOD.Width = 130;
      this.colSERI_NO.Caption = "Seri No";
      this.colSERI_NO.FieldName = "SERI_NO";
      this.colSERI_NO.Name = "colSERI_NO";
      this.colSERI_NO.Visible = true;
      this.colSERI_NO.VisibleIndex = 3;
      this.colSERI_NO.Width = 130;
      this.colMIAD.Caption = "Miad";
      this.colMIAD.FieldName = "MIAD";
      this.colMIAD.Name = "colMIAD";
      this.colMIAD.Visible = true;
      this.colMIAD.VisibleIndex = 4;
      this.colMIAD.Width = 130;
      this.colPARTINO.Caption = "Parti No";
      this.colPARTINO.FieldName = "PARTINO";
      this.colPARTINO.Name = "colPARTINO";
      this.colPARTINO.Visible = true;
      this.colPARTINO.VisibleIndex = 5;
      this.colPARTINO.Width = 130;
      this.colDURUM.Caption = "Bildirim Durum";
      this.colDURUM.FieldName = "MESAJ";
      this.colDURUM.Name = "colDURUM";
      this.colDURUM.Visible = true;
      this.colDURUM.VisibleIndex = 6;
      this.colDURUM.Width = 137;
      this.btnSorgula.Dock = DockStyle.Top;
      this.btnSorgula.Location = new Point(0, 0);
      this.btnSorgula.Name = "btnSorgula";
      this.btnSorgula.Size = new Size(918, 23);
      this.btnSorgula.TabIndex = 9;
      this.btnSorgula.Text = "Başarısız olan Ürünleri Sorgula";
      this.btnSorgula.Click += new EventHandler(this.btnSorgula_Click);
      this.splitterControl1.Dock = DockStyle.Bottom;
      this.splitterControl1.Location = new Point(0, 281);
      this.splitterControl1.Name = "splitterControl1";
      this.splitterControl1.Size = new Size(918, 6);
      this.splitterControl1.TabIndex = 5;
      this.splitterControl1.TabStop = false;
      this.panelControl6.Controls.Add((Control) this.memSonuc);
      this.panelControl6.Dock = DockStyle.Bottom;
      this.panelControl6.Location = new Point(0, 287);
      this.panelControl6.Name = "panelControl6";
      this.panelControl6.Size = new Size(918, 155);
      this.panelControl6.TabIndex = 3;
      this.memSonuc.Dock = DockStyle.Fill;
      this.memSonuc.Location = new Point(2, 2);
      this.memSonuc.Name = "memSonuc";
      this.memSonuc.Size = new Size(914, 151);
      this.memSonuc.TabIndex = 0;
      this.gridView1.Name = "gridView1";
      this.saveFileDialog1.DefaultExt = "xls";
      this.saveFileDialog1.Filter = "Excel Dosyası|*.xls";
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuDepo1,
        (ToolStripItem) this.mnuDepo2,
        (ToolStripItem) this.mnuDepo3
      });
      this.contextMenuStrip2.Name = "contextMenuStrip1";
      this.contextMenuStrip2.Size = new Size(112, 70);
      this.mnuDepo1.Name = "mnuDepo1";
      this.mnuDepo1.Size = new Size(111, 22);
      this.mnuDepo1.Text = "Depo 1";
      this.mnuDepo1.Click += new EventHandler(this.btnTakas_1_Click);
      this.mnuDepo2.Name = "mnuDepo2";
      this.mnuDepo2.Size = new Size(111, 22);
      this.mnuDepo2.Text = "Depo 2";
      this.mnuDepo2.Click += new EventHandler(this.btnTakas_1_Click);
      this.mnuDepo3.Name = "mnuDepo3";
      this.mnuDepo3.Size = new Size(111, 22);
      this.mnuDepo3.Text = "Depo 3";
      this.mnuDepo3.Click += new EventHandler(this.btnTakas_1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(924, 510);
      this.Controls.Add((Control) this.xtraTabControl1);
      this.Controls.Add((Control) this.panelControl1);
      this.IsMdiContainer = true;
      this.KeyPreview = true;
      this.Name = nameof (FrmSerbestBildirim);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bildirim";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosed += new FormClosedEventHandler(this.FrmSerbestBildirim_FormClosed);
      this.KeyDown += new KeyEventHandler(this.FrmSerbestBildirim_KeyDown);
      this.panelControl1.EndInit();
      this.panelControl1.ResumeLayout(false);
      this.xtraTabControl1.EndInit();
      this.xtraTabControl1.ResumeLayout(false);
      this.xtraTabPage2.ResumeLayout(false);
      this.grdKalem.EndInit();
      this.gridView2.EndInit();
      this.pnlHizli.ResumeLayout(false);
      this.txtHizli.Properties.EndInit();
      this.panel4.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panelControl4.EndInit();
      this.panelControl4.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.pnlUst.EndInit();
      this.pnlUst.ResumeLayout(false);
      this.pnlUst.PerformLayout();
      this.xtraTabPage3.ResumeLayout(false);
      this.panelControl7.EndInit();
      this.panelControl7.ResumeLayout(false);
      this.grdKalemDetay.EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.gridView3.EndInit();
      this.panelControl6.EndInit();
      this.panelControl6.ResumeLayout(false);
      this.memSonuc.Properties.EndInit();
      this.gridView1.EndInit();
      this.contextMenuStrip2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public class HataListe
    {
      private int hataKod;
      private int ad;
      private string durum;

      public int HataKod
      {
        get => this.hataKod;
        set => this.hataKod = value;
      }

      public int Ad
      {
        get => this.ad;
        set => this.ad = value;
      }

      public string Durum
      {
        get => this.durum;
        set => this.durum = value;
      }

      public HataListe()
      {
      }

      public HataListe(int htkod, string drm, int adet)
      {
        this.ad = adet;
        this.durum = drm;
        this.hataKod = htkod;
      }
    }
  }
}
