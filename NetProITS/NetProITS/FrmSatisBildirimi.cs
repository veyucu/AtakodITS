// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmSatisBildirimi
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
using FastReport;
using ICSharpCode.SharpZipLib.Zip;
using NetOpenX50;
using NetProITS.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace NetProITS
{
  public class FrmSatisBildirimi : Form
  {
    private BILDIRIM AktifEvrak;
    private List<BILDIRIM_STOK> AktifKalemler;
    private int EvrakID;
    private string EvrakNo;
    private string CariKod;
    private EvrakTipi EvrakTip;
    private bool strBeseri = false;
    private string EvrakSeri = string.Empty;
    private string EvrakSira = string.Empty;
    private string DizayNo = string.Empty;
    private IContainer components = (IContainer) null;
    private DefaultLookAndFeel defaultLookAndFeel1;
    private PanelControl panelControl1;
    private XtraTabControl xtraTabControl1;
    private XtraTabPage xtraTabPage1;
    private XtraTabPage xtraTabPage2;
    private PanelControl panelControl2;
    private SimpleButton simpleButton1;
    private DateEdit txtTarih;
    private LabelControl labelControl2;
    private PanelControl panelControl3;
    private GridControl grdSiparis;
    private GridView gridView1;
    private GridColumn colRECNO;
    private GridColumn colTARIH;
    private GridColumn colEVRAK_SERI;
    private GridColumn colCARI_KOD;
    private GridColumn colCARI_UNVAN;
    private PanelControl pnlUst;
    private PanelControl panelControl5;
    private PanelControl panelControl4;
    private SimpleButton btnBildirim;
    private SimpleButton btnKarekod;
    private Label lblDurum;
    private XtraTabPage xtraTabPage3;
    private PanelControl panelControl6;
    private MemoEdit memSonuc;
    private SplitterControl splitterControl1;
    private CheckBox chkBildirim;
    private Button button1;
    private SimpleButton btnImport;
    private TextEdit txtEvrakSeri;
    private LabelControl labelControl3;
    private System.Windows.Forms.ComboBox cbTip;
    private SimpleButton btnExcelAktar;
    private SimpleButton btnSeriAktar;
    private SimpleButton btnGonder;
    private CheckBox chkHizliMod;
    private Panel panel3;
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
    private Panel panel4;
    private SimpleButton btnGuncelle;
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
    private CheckBox chkTekrar;
    private GridColumn colCARI_IL;
    private CheckBox chkBasari;
    private SimpleButton btnYazdir;
    private GridColumn gridColumn4;
    private GridColumn gridColumn5;
    private SimpleButton btnFoy;
    private SimpleButton btnOperasyon;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem exceleGönderToolStripMenuItem;
    private SaveFileDialog saveFileDialog1;
    private SimpleButton btnDogrulama;
    private SimpleButton btnTakas;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem mnuDepo1;
    private ToolStripMenuItem mnuDepo2;
    private ContextMenuStrip contextMenuStrip3;
    private ToolStripMenuItem mnuFirma1;
    private ToolStripMenuItem mnuFirma2;
    private ToolStripMenuItem mnuFirma3;
    private GridColumn colACIKLAMA;
    private SimpleButton btnFaturaBas;
    private ToolStripMenuItem mnuDepo3;
    private GridColumn colCARI_ILCE;
    private GridColumn colSAAT;
    private GridColumn colTUTAR;
    private MemoEdit txtHizli;
    private Panel panel2;
    private Button button2;
    private TextBox txtBarkod;
    private CheckBox chkKoli2;
    private Panel panel1;
    private System.Windows.Forms.ProgressBar prgDurum;
    private SimpleButton btnEtiketYazdir;
    private GridColumn colREYON_KODU;
    private SimpleButton simpleButton2;

    public FrmSatisBildirimi() => this.InitializeComponent();

    private void simpleButton1_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.cbTip.SelectedIndex > -1)
      {
        List<BILDIRIM> list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == "")).ToList<BILDIRIM>();
        this.strBeseri = !string.IsNullOrEmpty(MyUtils.GetParamValue("IsBeseri")) && Convert.ToBoolean(MyUtils.GetParamValue("IsBeseri"));
        string strDeak = MyUtils.GetParamValue("Deaktivasyon");
        DateTime dateTime = this.txtTarih.DateTime;
        if (true)
        {
          if (this.txtEvrakSeri.Text.Length > 0)
          {
            switch (this.EvrakTip)
            {
              case EvrakTipi.SATIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != strDeak)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SATIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SIPARIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 5)).ToList<BILDIRIM>();
                break;
            }
          }
          else if (this.chkBildirim.Checked)
          {
            switch (this.EvrakTip)
            {
              case EvrakTipi.SATIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != strDeak)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SATIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SIPARIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 5)).ToList<BILDIRIM>();
                break;
            }
          }
          else
          {
            switch (this.EvrakTip)
            {
              case EvrakTipi.SATIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != strDeak)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SATIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.ALIS_IPTAL:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
                break;
              case EvrakTipi.SIPARIS:
                list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.txtTarih.DateTime & u.TIP == (int?) 5)).ToList<BILDIRIM>();
                break;
            }
          }
          this.grdSiparis.DataSource = (object) list;
          this.gridView1.BestFitColumns();
        }
      }
      Cursor.Current = Cursors.Default;
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.AktifEvrak = this.gridView1.GetRow(this.gridView1.FocusedRowHandle) as BILDIRIM;
      this.btnOperasyon.Visible = this.EvrakTip == EvrakTipi.SATIS;
      this.EvrakNo = this.AktifEvrak.EVRAK_NO;
      this.CariKod = this.AktifEvrak.CARI_KOD;
      this.EvrakID = this.AktifEvrak.TIP.Value;
      MyUtils.Refresh();
      bool flag = true;
      string str1 = "";
      if (this.EvrakTip == EvrakTipi.SIPARIS)
      {
        List<string> list1 = MyUtils.Firma.Database.SqlQuery<string>("SELECT FISNO FROM TBLSTHAR WHERE STHAR_SIPNUM='" + this.EvrakNo + "'").ToList<string>();
        if (list1.Count > 0)
        {
          flag = false;
          str1 = list1[0];
        }
        string sql = "SELECT STOK_KODU FROM (SELECT STOK_KODU, COUNT(*) AS SAYI FROM TBLSIPATRA WHERE FISNO = '" + this.EvrakNo + "' GROUP BY FISNO, STOK_KODU)A WHERE A.SAYI>1";
        List<string> list2 = MyUtils.Firma.Database.SqlQuery<string>(sql).ToList<string>();
        if (list2 != null)
        {
          foreach (string str2 in list2)
            this.MesajGoster(str2 + " kodlu ürün birden fazla girilmiştir.Lütfen düzeltiniz!!!!");
        }
      }
      if (flag)
      {
        this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
        this.grdKalem.DataSource = (object) this.AktifKalemler;
        this.grdKalemDetay.DataSource = (object) MyUtils.Firma.Database.SqlQuery<ITSHAR_VIEW>(MyUtils.GetITSHAR_VIEWSQL(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR_VIEW>();
        this.lblDurum.Text = "Evrak : " + this.EvrakNo + " " + this.AktifEvrak.CARI_UNVAN;
        Cursor.Current = Cursors.Default;
        this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
      }
      else
      {
        int num = (int) MessageBox.Show("Bu sipariş evrağı faturalaşmıştır.\rFatura No:" + str1);
      }
    }

    private void YeniEvrak(object sender, EventArgs e)
    {
      this.grdKalemDetay.DataSource = (object) null;
      this.grdKalem.DataSource = (object) null;
      this.AktifEvrak = (BILDIRIM) null;
      this.AktifKalemler = (List<BILDIRIM_STOK>) null;
      this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
      this.simpleButton1_Click(sender, e);
    }

    private void gridControl1_Click(object sender, EventArgs e)
    {
    }

    public void MesajGoster(string mesaj)
    {
      FrmMesaj frmMesaj = new FrmMesaj(mesaj);
      int num = (int) frmMesaj.ShowDialog();
      frmMesaj.Dispose();
    }

    private void btnKarekod_Click(object sender, EventArgs e)
    {
      FrmKareKod karekod;
      while (true)
      {
        karekod = new FrmKareKod();
        if (karekod.ShowDialog() == DialogResult.OK)
        {
          this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
          double? nullable1;
          if (karekod.chkKolii)
          {
            this.prgDurum.Minimum = 0;
            this.prgDurum.Value = 0;
            int num1 = 0;
            string koli = karekod.strKoliBarkod;
            StringBuilder stringBuilder = new StringBuilder();
            List<TBLTRANSFER_DETAY> list = MyUtils.Firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.CARRIER_LABEL == koli)).ToList<TBLTRANSFER_DETAY>();
            if (list.Count > 0)
            {
              this.prgDurum.Maximum = list.Count;
              foreach (TBLTRANSFER_DETAY tbltransferDetay in list)
              {
                Application.DoEvents();
                string barkod = tbltransferDetay.GTIN;
                barkod = barkod.Remove(0, 1);
                DateTime dateTime;
                try
                {
                  dateTime = DateTime.ParseExact(tbltransferDetay.DATE, "yyyy-MM-dd", (IFormatProvider) null);
                }
                catch
                {
                  dateTime = DateTime.ParseExact(tbltransferDetay.DATE, new string[2]
                  {
                    "yyyy-MM-dd",
                    "yyyy-MM-00"
                  }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
                  dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
                }
                string str = dateTime.ToString("yyMMdd");
                if (!this.chkHizliMod.Checked)
                {
                  BILDIRIM_STOK bildirimStok1 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).FirstOrDefault<BILDIRIM_STOK>();
                  if (bildirimStok1 != null)
                  {
                    double? nullable2 = bildirimStok1.MIKTAR;
                    int? karsilanan = bildirimStok1.KARSILANAN;
                    double? nullable3 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
                    if (nullable2.GetValueOrDefault() > nullable3.GetValueOrDefault() && nullable2.HasValue & nullable3.HasValue)
                    {
                      if (MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, tbltransferDetay.GTIN, tbltransferDetay.SERIAL_NUMBER, tbltransferDetay.LOT_NUMBER, str)).ToList<ITSHAR>().Count == 0)
                      {
                        MyUtils.HareketEkle(tbltransferDetay.GTIN, this.EvrakNo, this.CariKod, "", str, tbltransferDetay.LOT_NUMBER, tbltransferDetay.SERIAL_NUMBER, bildirimStok1.STOK_KODU, this.EvrakID);
                        BILDIRIM_STOK bildirimStok2 = bildirimStok1;
                        karsilanan = bildirimStok1.KARSILANAN;
                        int? nullable4 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() + 1) : new int?();
                        bildirimStok2.KARSILANAN = nullable4;
                        BILDIRIM_STOK bildirimStok3 = bildirimStok1;
                        nullable3 = bildirimStok1.KALAN;
                        double num2 = 1.0;
                        double? nullable5;
                        if (!nullable3.HasValue)
                        {
                          nullable2 = new double?();
                          nullable5 = nullable2;
                        }
                        else
                          nullable5 = new double?(nullable3.GetValueOrDefault() - num2);
                        bildirimStok3.KALAN = nullable5;
                        this.grdKalem.DataSource = (object) null;
                        this.grdKalem.DataSource = (object) this.AktifKalemler;
                      }
                      else
                        stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
                    }
                    else
                      stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
                  }
                  else
                    stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
                }
                else
                {
                  MemoEdit txtHizli1 = this.txtHizli;
                  txtHizli1.Text = txtHizli1.Text + tbltransferDetay.GTIN + "|" + tbltransferDetay.SERIAL_NUMBER + "|" + tbltransferDetay.LOT_NUMBER + "|" + str + "\r\n";
                  if (this.txtHizli.Text.IndexOf(tbltransferDetay.GTIN + "|" + tbltransferDetay.SERIAL_NUMBER + "|" + tbltransferDetay.LOT_NUMBER + "|" + str) >= 0)
                  {
                    if (this.chkTekrar.Checked && MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                      this.txtHizli.Text = this.txtHizli.Text.Replace(tbltransferDetay.GTIN + "|" + tbltransferDetay.SERIAL_NUMBER + "|" + tbltransferDetay.LOT_NUMBER + "|" + str + "\r\n", "");
                  }
                  else
                  {
                    MemoEdit txtHizli2 = this.txtHizli;
                    txtHizli2.Text = txtHizli2.Text + tbltransferDetay.GTIN + "|" + tbltransferDetay.SERIAL_NUMBER + "|" + tbltransferDetay.LOT_NUMBER + "|" + str + "\r\n";
                  }
                }
                this.prgDurum.Value = num1 + 1;
              }
              this.prgDurum.Value = 0;
              this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
              this.grdKalem.DataSource = (object) this.AktifKalemler;
            }
            if (stringBuilder.Length > 1)
            {
              FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
              frmKoliSonuc.Sonuc = stringBuilder.ToString();
              int num3 = (int) frmKoliSonuc.ShowDialog();
              frmKoliSonuc.Dispose();
            }
          }
          else if (karekod.strGtin.Length > 0 & karekod.strMiad.Length > 0 & karekod.strPartiNo.Length > 0 & karekod.strseriNo.Length > 0)
          {
            if (karekod.strGtin.Length > 0)
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
              if (!this.chkHizliMod.Checked)
              {
                BILDIRIM_STOK bildirimStok4 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).FirstOrDefault<BILDIRIM_STOK>();
                if (bildirimStok4 != null)
                {
                  double? nullable6 = bildirimStok4.MIKTAR;
                  int? karsilanan = bildirimStok4.KARSILANAN;
                  nullable1 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
                  if (nullable6.GetValueOrDefault() > nullable1.GetValueOrDefault() && nullable6.HasValue & nullable1.HasValue)
                  {
                    List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, karekod.strGtin, karekod.strseriNo, karekod.strPartiNo, karekod.strMiad)).ToList<ITSHAR>();
                    if (list.Count == 0)
                    {
                      MyUtils.HareketEkle(karekod.strGtin, this.EvrakNo, this.CariKod, "", karekod.strMiad, karekod.strPartiNo, karekod.strseriNo, bildirimStok4.STOK_KODU, this.EvrakID);
                      BILDIRIM_STOK bildirimStok5 = bildirimStok4;
                      karsilanan = bildirimStok4.KARSILANAN;
                      int? nullable7 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() + 1) : new int?();
                      bildirimStok5.KARSILANAN = nullable7;
                      BILDIRIM_STOK bildirimStok6 = bildirimStok4;
                      nullable1 = bildirimStok4.KALAN;
                      double num4 = 1.0;
                      double? nullable8;
                      if (!nullable1.HasValue)
                      {
                        nullable6 = new double?();
                        nullable8 = nullable6;
                      }
                      else
                        nullable8 = new double?(nullable1.GetValueOrDefault() - num4);
                      bildirimStok6.KALAN = nullable8;
                      this.grdKalem.DataSource = (object) null;
                      this.grdKalem.DataSource = (object) this.AktifKalemler;
                      int num5 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
                      if (num5 != int.MinValue)
                        this.gridView2.FocusedRowHandle = num5;
                    }
                    else if (this.chkTekrar.Checked)
                    {
                      Console.Beep(1000, 1000);
                      if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                      {
                        MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM ITSHAR WHERE ID=" + list[0].ID.ToString());
                        MyUtils.Refresh();
                        BILDIRIM_STOK bildirimStok7 = bildirimStok4;
                        karsilanan = bildirimStok4.KARSILANAN;
                        int? nullable9 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() - 1) : new int?();
                        bildirimStok7.KARSILANAN = nullable9;
                        BILDIRIM_STOK bildirimStok8 = bildirimStok4;
                        nullable1 = bildirimStok4.KALAN;
                        double num6 = 1.0;
                        double? nullable10;
                        if (!nullable1.HasValue)
                        {
                          nullable6 = new double?();
                          nullable10 = nullable6;
                        }
                        else
                          nullable10 = new double?(nullable1.GetValueOrDefault() + num6);
                        bildirimStok8.KALAN = nullable10;
                        this.grdKalem.DataSource = (object) null;
                        this.grdKalem.DataSource = (object) this.AktifKalemler;
                        int num7 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
                        if (num7 != int.MinValue)
                          this.gridView2.FocusedRowHandle = num7;
                      }
                    }
                  }
                  else
                  {
                    List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, karekod.strGtin, karekod.strseriNo, karekod.strPartiNo, karekod.strMiad)).ToList<ITSHAR>();
                    if (list.Count > 0)
                    {
                      if (this.chkTekrar.Checked)
                      {
                        Console.Beep(1000, 1000);
                        if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                          MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM ITSHAR WHERE ID=" + list[0].ID.ToString());
                          MyUtils.Refresh();
                          BILDIRIM_STOK bildirimStok9 = bildirimStok4;
                          karsilanan = bildirimStok4.KARSILANAN;
                          int? nullable11 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() - 1) : new int?();
                          bildirimStok9.KARSILANAN = nullable11;
                          BILDIRIM_STOK bildirimStok10 = bildirimStok4;
                          nullable1 = bildirimStok4.KALAN;
                          double num8 = 1.0;
                          double? nullable12;
                          if (!nullable1.HasValue)
                          {
                            nullable6 = new double?();
                            nullable12 = nullable6;
                          }
                          else
                            nullable12 = new double?(nullable1.GetValueOrDefault() + num8);
                          bildirimStok10.KALAN = nullable12;
                          this.grdKalem.DataSource = (object) null;
                          this.grdKalem.DataSource = (object) this.AktifKalemler;
                          int num9 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
                          if (num9 != int.MinValue)
                            this.gridView2.FocusedRowHandle = num9;
                        }
                      }
                    }
                    else
                    {
                      Console.Beep(1000, 1000);
                      this.MesajGoster("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
                    }
                  }
                }
                else
                {
                  Console.Beep(1000, 1000);
                  this.MesajGoster("Bu ürün bu siparişde yoktur...");
                }
              }
              else if (this.txtHizli.Text.IndexOf(karekod.strGtin + "|" + karekod.strseriNo + "|" + karekod.strPartiNo + "|" + karekod.strMiad) >= 0)
              {
                if (this.chkTekrar.Checked && MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                  this.txtHizli.Text = this.txtHizli.Text.Replace(karekod.strGtin + "|" + karekod.strseriNo + "|" + karekod.strPartiNo + "|" + karekod.strMiad + "\r\n", "");
              }
              else
              {
                MemoEdit txtHizli = this.txtHizli;
                txtHizli.Text = txtHizli.Text + karekod.strGtin + "|" + karekod.strseriNo + "|" + karekod.strPartiNo + "|" + karekod.strMiad + "\r\n";
              }
            }
          }
          else if (karekod.strBarkod.Length > 0)
          {
            if (!this.chkHizliMod.Checked)
            {
              List<BILDIRIM_STOK> list = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == karekod.strBarkod)).ToList<BILDIRIM_STOK>();
              if (list.Count > 0)
              {
                if (list[0].REYON_KODU != "BESERI")
                {
                  FrmAdet frmAdet = new FrmAdet();
                  int num10 = (int) frmAdet.ShowDialog();
                  int adet = frmAdet.Adet;
                  frmAdet.Dispose();
                  nullable1 = list[0].KALAN;
                  double num11 = nullable1.Value;
                  double num12 = num11;
                  if ((double) adet > num12 && num12 > 0.0)
                  {
                    Console.Beep(1000, 1000);
                    this.MesajGoster("Fazla giriş yaptınız Lütfen kontrol ediniz...");
                  }
                  else
                  {
                    nullable1 = list[0].KALAN;
                    double num13 = 0.0;
                    if (nullable1.GetValueOrDefault() > num13 && nullable1.HasValue)
                    {
                      for (int index = 0; index < adet; ++index)
                      {
                        if (num11 > 0.0)
                        {
                          MyUtils.HareketEkle("0" + karekod.strBarkod, this.EvrakNo, this.CariKod, "", "", "", "", list[0].STOK_KODU, this.EvrakID);
                          --num11;
                        }
                      }
                      this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
                      this.grdKalem.DataSource = (object) this.AktifKalemler;
                      int num14 = this.gridView2.LocateByValue("BARKOD", (object) karekod.strBarkod);
                      if (num14 != int.MinValue)
                        this.gridView2.FocusedRowHandle = num14;
                    }
                    else
                    {
                      Console.Beep(1000, 1000);
                      this.MesajGoster("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
                    }
                  }
                }
              }
              else
              {
                Console.Beep(1000, 1000);
                this.MesajGoster("Bu ürün bu siparişde yoktur...");
              }
            }
            else
            {
              MemoEdit txtHizli = this.txtHizli;
              txtHizli.Text = txtHizli.Text + karekod.strBarkod + "\r\n";
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
      if (this.EvrakTip == EvrakTipi.SIPARIS)
      {
        string sql = "SELECT STOK_KODU FROM (SELECT STOK_KODU, COUNT(*) AS SAYI FROM TBLSIPATRA WHERE FISNO = '" + this.EvrakNo + "' GROUP BY FISNO, STOK_KODU)A WHERE A.SAYI>1";
        List<string> list = MyUtils.Firma.Database.SqlQuery<string>(sql).ToList<string>();
        if (list != null && list.Count > 0)
        {
          using (List<string>.Enumerator enumerator = list.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.MesajGoster(enumerator.Current + " kodlu ürün birden fazla girilmiştir.Lütfen düzeltiniz!!!!");
            return;
          }
        }
      }
      this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
      foreach (BILDIRIM_STOK bildirimStok in this.AktifKalemler)
      {
        double? kalan = bildirimStok.KALAN;
        double num1 = 0.0;
        int num2;
        if ((kalan.GetValueOrDefault() > num1 ? (kalan.HasValue ? 1 : 0) : 0) != 0)
        {
          int? karsilanan = bildirimStok.KARSILANAN;
          int num3 = 0;
          if ((karsilanan.GetValueOrDefault() > num3 ? (karsilanan.HasValue ? 1 : 0) : 0) != 0)
          {
            num2 = bildirimStok.KOSULKODU.IndexOf("-MF") > -1 ? 1 : 0;
            goto label_13;
          }
        }
        num2 = 0;
label_13:
        if (num2 != 0)
        {
          int num4 = (int) MessageBox.Show(bildirimStok.STOK_ISIM + " isimli ürün MF'lidir lütfen tamamlayınız!!!!");
          return;
        }
      }
      int num5 = 0;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      StringBuilder stringBuilder1 = new StringBuilder();
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.ResmiFaturaYazici)
        {
          num6 = num5 + 1;
          str1 = MyUtils.ResmiFaturaYazici;
          break;
        }
        ++num5;
      }
      int num9 = 0;
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.GayriFaturaYazici)
        {
          num7 = num9 + 1;
          str2 = MyUtils.GayriFaturaYazici;
          break;
        }
        ++num9;
      }
      if (this.EvrakTip == EvrakTipi.SIPARIS && (num6 == -1 || num7 == -1))
      {
        int num10 = (int) MessageBox.Show("Lütfen fatura yazıcı ayarını yapınız!");
      }
      else
      {
        if (this.EvrakTip == EvrakTipi.SIPARIS)
        {
          if (string.IsNullOrEmpty(this.EvrakSira))
            this.EvrakSira = "1";
          if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "" && MyUtils.GetParamValue("FaturaDizaynNo") != "")
          {
            FrmEvrakTip frmEvrakTip = new FrmEvrakTip();
            if (frmEvrakTip.ShowDialog() == DialogResult.OK)
            {
              this.EvrakSeri = frmEvrakTip.EvrakSeri;
              this.DizayNo = frmEvrakTip.DizaynNo;
              if (frmEvrakTip.Tip == 1)
              {
                num8 = num6;
                str3 = str1;
              }
              else
              {
                num8 = num7;
                str3 = str2;
              }
            }
            frmEvrakTip.Dispose();
          }
          else if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "")
          {
            this.EvrakSeri = MyUtils.GetParamValue("GayriEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("GayriFaturaDizaynNo");
            num8 = num7;
            str3 = str2;
          }
          else
          {
            this.EvrakSeri = MyUtils.GetParamValue("ResmiEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("FaturaDizaynNo");
            num8 = num6;
            str3 = str1;
          }
          if (string.IsNullOrEmpty(this.EvrakSeri))
          {
            int num11 = (int) MessageBox.Show("Lütfen Evrak Seri giriniz!!!");
            return;
          }
        }
        string glnno1 = this.AktifEvrak.GLNNO;
        string paramValue1 = MyUtils.GetParamValue("KullaniciAdi");
        string paramValue2 = MyUtils.GetParamValue("Sifre");
        string glnno2 = this.AktifEvrak.GLNNO;
        try
        {
          if (Convert.ToInt32(this.AktifKalemler.Sum<BILDIRIM_STOK>((Func<BILDIRIM_STOK, double?>) (u => u.KALAN)).Value) > 0)
          {
            if (this.EvrakTip == EvrakTipi.SIPARIS)
            {
              int num12 = (int) MessageBox.Show("Okutulmayan kalemler mevcut\r Lütfen siparişinizi tekrar gözden geçiriniz!");
            }
            else if (MessageBox.Show("Okutulmayan kalemler mevcut\r Bildirim yapmak istediğinize eminmisiniz?", "Bildirim Uyarı", MessageBoxButtons.YesNo) == DialogResult.No)
              return;
          }
          this.Cursor = Cursors.WaitCursor;
          MyUtils.Refresh();
          int num13 = 0;
          int num14 = 0;
          List<FrmSatisBildirimi.HataListe> source = new List<FrmSatisBildirimi.HataListe>();
          foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) MyUtils.Firma.HATA_KODLARI)
            source.Add(new FrmSatisBildirimi.HataListe()
            {
              HataKod = hataKodlari.HATAID,
              Ad = 0,
              Durum = hataKodlari.MESAJ
            });
          List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_GENEL(this.EvrakNo, this.CariKod, this.EvrakID)).Where<ITSHAR>((Func<ITSHAR, bool>) (u => u.SERI_NO != "")).ToList<ITSHAR>();
          List<KarekodBilgi> its = new List<KarekodBilgi>();
          foreach (ITSHAR itshar in list)
          {
            string miad = BildirimHelper.ConvertMiadTarih(itshar.MIAD).ToString("yyyy-MM-dd");
            its.Add(new KarekodBilgi(itshar.BARKOD, itshar.SERI_NO, itshar.PARTINO, miad));
          }
          List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              karekodBilgiList = BildirimHelper.DepoSatisBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), glnno1, its);
              break;
            case EvrakTipi.ALIS:
              karekodBilgiList = BildirimHelper.MalAlimBildirimi(paramValue1, paramValue2, its);
              break;
            case EvrakTipi.SATIS_IPTAL:
              karekodBilgiList = BildirimHelper.SatisIptalBildirimi(paramValue1, paramValue2, its);
              break;
            case EvrakTipi.ALIS_IPTAL:
              karekodBilgiList = BildirimHelper.MalIadeBildirimi(paramValue1, paramValue2, glnno2, its);
              break;
            case EvrakTipi.SIPARIS:
              karekodBilgiList = BildirimHelper.DepoSatisBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), glnno1, its);
              TBLSIPAMAS tblsipamas = MyUtils.Firma.TBLSIPAMAS.Where<TBLSIPAMAS>((Expression<Func<TBLSIPAMAS, bool>>) (u => (int) u.SUBE_KODU == 0 & u.FTIRSIP == "6" & u.FATIRS_NO == this.AktifEvrak.EVRAK_NO & u.CARI_KODU == this.AktifEvrak.CARI_KOD)).FirstOrDefault<TBLSIPAMAS>();
              if (tblsipamas != null)
                tblsipamas.S_YEDEK1 = "OK";
              MyUtils.Firma.SaveChanges();
              break;
          }
          string fsip = this.AktifEvrak.FTIRSIP;
          string fatno = this.AktifEvrak.EVRAK_NO;
          TBLFATUIRS tblfatuirs = MyUtils.Firma.TBLFATUIRS.Where<TBLFATUIRS>((Expression<Func<TBLFATUIRS, bool>>) (u => u.FTIRSIP == fsip && u.FATIRS_NO == fatno)).FirstOrDefault<TBLFATUIRS>();
          if (tblfatuirs != null)
          {
            tblfatuirs.S_YEDEK1 = "OK";
            MyUtils.Firma.SaveChanges();
          }
          if (karekodBilgiList != null)
          {
            foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
            {
              KarekodBilgi item = karekodBilgi;
              item.Barkod.Remove(0, 1);
              ITSHAR h = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).ToList<ITSHAR>()[0];
              if (h != null)
              {
                h.DURUM = item.Sonuc;
                if (item.Sonuc == "00000")
                  ++num13;
                source.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad = source.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad + 1;
                MyUtils.Firma.SaveChanges();
              }
              ++num14;
            }
          }
          MyUtils.Refresh();
          this.grdKalemDetay.DataSource = (object) MyUtils.Firma.Database.SqlQuery<ITSHAR_VIEW>(MyUtils.GetITSHAR_VIEWSQL(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR_VIEW>();
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.AppendLine("Bildirim Detayları : ");
          foreach (FrmSatisBildirimi.HataListe hataListe in source)
          {
            if (hataListe.Ad > 0)
            {
              stringBuilder2.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
              hataListe.Ad = 0;
            }
          }
          stringBuilder2.AppendLine("Bildirim Durumu : " + num13.ToString() + " / " + num14.ToString());
          this.memSonuc.Text = stringBuilder2.ToString();
          this.Cursor = Cursors.Default;
          this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
          if (this.EvrakTip == EvrakTipi.SATIS)
          {
            if (num13 != num14 || MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) != DialogResult.Yes)
              return;
            this.btnPaketAl_Click(sender, e);
          }
          else if (this.EvrakTip == EvrakTipi.SIPARIS && num13 == num14)
          {
            if (MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) == DialogResult.Yes)
              this.btnPaketAl_Click(sender, e);
            int num15 = (int) MessageBox.Show("Sipariş Faturalaştırılacak.Lütfen bekleyiniz!....");
            // ISSUE: variable of a compiler-generated type
            Fatura o1 = (Fatura) null;
            // ISSUE: variable of a compiler-generated type
            Fatura o2 = (Fatura) null;
            // ISSUE: variable of a compiler-generated type
            FatUst o3 = (FatUst) null;
            // ISSUE: variable of a compiler-generated type
            FatKalem o4 = (FatKalem) null;
            // ISSUE: variable of a compiler-generated type
            FatKalem sipKalem = (FatKalem) null;
            // ISSUE: variable of a compiler-generated type
            Basim o5 = (Basim) null;
            // ISSUE: variable of a compiler-generated type
            NetRS o6 = (NetRS) null;
            stringBuilder1 = new StringBuilder();
            string str4 = string.Empty;
            string empty = string.Empty;
            try
            {
              if (MyUtils.sirket != null)
              {
                // ISSUE: reference to a compiler-generated method
                o1 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSSip);
                // ISSUE: reference to a compiler-generated method
                o1.OkuUst(this.AktifEvrak.EVRAK_NO, this.AktifEvrak.CARI_KOD);
                // ISSUE: reference to a compiler-generated method
                o1.OkuKalem();
                // ISSUE: reference to a compiler-generated method
                // ISSUE: variable of a compiler-generated type
                FatUst fatUst = o1.Ust();
                // ISSUE: reference to a compiler-generated method
                o2 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSFat);
                o2.KosulluHesapla = false;
                // ISSUE: reference to a compiler-generated method
                o3 = o2.Ust();
                // ISSUE: reference to a compiler-generated method
                string evraksira = o2.YeniNumara(this.EvrakSeri).Replace(this.EvrakSeri, "");
                FrmEvrakNo frmEvrakNo = new FrmEvrakNo(this.EvrakSeri, evraksira);
                if (frmEvrakNo.ShowDialog() == DialogResult.OK)
                {
                  str4 = frmEvrakNo.EvrakSeri;
                  evraksira = frmEvrakNo.EvrakSira;
                }
                frmEvrakNo.Dispose();
                if (string.IsNullOrEmpty(evraksira))
                  throw new Exception("Lütfen Evrak Seri giriniz!!!");
                // ISSUE: reference to a compiler-generated method
                o5 = MyUtils.kernel.yeniBasim(MyUtils.sirket);
                string projeKodu = fatUst.Proje_Kodu;
                o3.CariKod = this.AktifEvrak.CARI_KOD;
                o3.SIPARIS_TEST = DateTime.Now;
                o3.FiiliTarih = DateTime.Now;
                o3.Tarih = DateTime.Now;
                o3.FATIRS_NO = str4 + evraksira;
                o3.TIPI = TFaturaTipi.ft_Acik;
                o3.PLA_KODU = fatUst.PLA_KODU;
                o3.Proje_Kodu = projeKodu;
                o3.KOD1 = fatUst.KOD1;
                o3.KOSULKODU = fatUst.KOSULKODU;
                try
                {
                  o3.KOSULTARIHI = fatUst.KOSULTARIHI;
                }
                catch
                {
                }
                try
                {
                  o3.FIYATTARIHI = fatUst.FIYATTARIHI;
                }
                catch
                {
                }
                o3.KDV_DAHILMI = fatUst.KDV_DAHILMI;
                o3.GENISK1TIP = fatUst.GENISK1TIP;
                o3.GENISK2TIP = fatUst.GENISK2TIP;
                o3.GENISK3TIP = fatUst.GENISK3TIP;
                o3.GEN_ISK1O = fatUst.GEN_ISK1O;
                o3.GEN_ISK2O = fatUst.GEN_ISK2O;
                o3.GEN_ISK3O = fatUst.GEN_ISK3O;
                o3.GEN_ISK1T = fatUst.GEN_ISK1T;
                o3.GEN_ISK2T = fatUst.GEN_ISK2T;
                o3.GEN_ISK3T = fatUst.GEN_ISK3T;
                this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
                for (int Index = 0; Index < o1.KalemAdedi; ++Index)
                {
                  // ISSUE: reference to a compiler-generated method
                  sipKalem = o1.get_Kalem(Index);
                  BILDIRIM_STOK bildirimStok = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.STOK_KODU == sipKalem.StokKodu)).First<BILDIRIM_STOK>();
                  int? karsilanan = bildirimStok.KARSILANAN;
                  int num16 = 0;
                  if (karsilanan.GetValueOrDefault() > num16 && karsilanan.HasValue)
                  {
                    // ISSUE: reference to a compiler-generated method
                    o4 = o2.kalemYeni(bildirimStok.STOK_KODU);
                    o4.StokKodu = bildirimStok.STOK_KODU;
                    o4.STra_GCMIK = Convert.ToDouble((object) bildirimStok.KARSILANAN) - sipKalem.STra_MALFISK;
                    o4.STra_MALFISK = sipKalem.STra_MALFISK;
                    o4.STra_BF = sipKalem.STra_BF;
                    o4.STra_NF = sipKalem.STra_NF;
                    o4.STra_SatIsk = sipKalem.STra_SatIsk;
                    o4.STra_SatIsk2 = sipKalem.STra_SatIsk2;
                    o4.STra_SatIsk3 = sipKalem.STra_SatIsk3;
                    o4.STra_KDV = sipKalem.STra_KDV;
                    o4.STra_SIPNUM = this.AktifEvrak.EVRAK_NO;
                    o4.STra_SIPKONT = sipKalem.STra_SIPKONT;
                    o4.Stra_KosulK = sipKalem.Stra_KosulK;
                    o4.STra_ODEGUN = sipKalem.STra_ODEGUN;
                    o4.ProjeKodu = fatUst.Proje_Kodu;
                    o4.Plasiyer_Kodu = fatUst.PLA_KODU;
                    o4.Olcubr = sipKalem.Olcubr;
                    o4.STra_KOD1 = sipKalem.STra_KOD1;
                  }
                }
                // ISSUE: reference to a compiler-generated method
                o2.kayitYeni();
                // ISSUE: reference to a compiler-generated method
                o6 = MyUtils.kernel.yeniNetRS(MyUtils.sirket);
                // ISSUE: reference to a compiler-generated method
                o6.Calistir("UPDATE TBLSIPAMAS SET SIRA_NO=-1 WHERE FATIRS_NO='" + o3.FATIRS_NO + "' AND FTIRSIP='6'");
                // ISSUE: reference to a compiler-generated method
                o6.Calistir("UPDATE ITSHAR SET TIP=1,EVRAK_SERI='" + o3.FATIRS_NO + "' WHERE TIP=5 AND EVRAK_SERI='" + this.AktifEvrak.EVRAK_NO + "' AND CARI_KOD='" + this.AktifEvrak.CARI_KOD + "'");
                // ISSUE: reference to a compiler-generated method
                o6.Calistir("UPDATE TBLFATUIRS SET S_YEDEK1='OK' WHERE FTIRSIP='1' AND FATIRS_NO='" + o3.FATIRS_NO + "' AND TIPI=2 AND CARI_KODU='" + this.AktifEvrak.CARI_KOD + "'");
                // ISSUE: reference to a compiler-generated method
                o6.Calistir("UPDATE TBLDIZAYNGENEL SET YEDEK5=" + num8.ToString() + " WHERE GENELID=" + this.DizayNo);
                // ISSUE: reference to a compiler-generated method
                o5.FaturaBasimDizaynNo(TFaturaTip.ftSFat, o3.FATIRS_NO, this.AktifEvrak.CARI_KOD, Convert.ToInt32(this.DizayNo));
                this.YeniEvrak(sender, e);
              }
            }
            catch (Exception ex)
            {
              this.Cursor = Cursors.Default;
              int num17 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
            }
            finally
            {
              if (o6 != null)
                Marshal.ReleaseComObject((object) o6);
              if (o5 != null)
                Marshal.ReleaseComObject((object) o5);
              if (o4 != null)
                Marshal.ReleaseComObject((object) o4);
              if (o3 != null)
                Marshal.ReleaseComObject((object) o3);
              if (o2 != null)
                Marshal.ReleaseComObject((object) o2);
              if (o1 != null)
                Marshal.ReleaseComObject((object) o1);
            }
          }
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num18 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
        }
      }
    }

    private void btnPaketAl_Click(object sender, EventArgs e)
    {
      if (this.EvrakTip != EvrakTipi.SATIS && this.EvrakTip != EvrakTipi.SIPARIS)
      {
        int num1 = (int) MessageBox.Show("Bildirim tipi Satış olmalı!");
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        List<ITSHAR_VIEW> list = MyUtils.Firma.Database.SqlQuery<ITSHAR_VIEW>(MyUtils.GetITSHAR_VIEWSQLPaket(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR_VIEW>();
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
        ITSHAR_VIEW itsharView1 = new ITSHAR_VIEW();
        int num2 = 10000;
        XmlElement newChild1 = (XmlElement) null;
        XmlElement newChild2 = (XmlElement) null;
        foreach (ITSHAR_VIEW itsharView2 in list)
        {
          if (itsharView1.BARKOD == itsharView2.BARKOD & itsharView1.PARTINO == itsharView2.PARTINO & itsharView1.MIAD == itsharView2.MIAD)
          {
            if (newChild2 != null)
            {
              XmlElement element9 = xmlDocument.CreateElement("serialNumber");
              element9.InnerText = itsharView2.SERI_NO;
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
            attribute3.InnerText = itsharView2.BARKOD;
            newChild2.Attributes.Append(attribute3);
            XmlAttribute attribute4 = xmlDocument.CreateAttribute("lotNumber");
            attribute4.InnerText = itsharView2.PARTINO;
            newChild2.Attributes.Append(attribute4);
            XmlAttribute attribute5 = xmlDocument.CreateAttribute("expirationDate");
            DateTime dateTime;
            try
            {
              dateTime = DateTime.ParseExact(itsharView2.MIAD, "yyMMdd", (IFormatProvider) null);
            }
            catch
            {
              dateTime = DateTime.ParseExact(itsharView2.MIAD, new string[2]
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
            element10.InnerText = itsharView2.SERI_NO;
            newChild2.AppendChild((XmlNode) element10);
            itsharView1.BARKOD = itsharView2.BARKOD;
            itsharView1.MIAD = itsharView2.MIAD;
            itsharView1.PARTINO = itsharView2.PARTINO;
          }
        }
        newChild1.AppendChild((XmlNode) newChild2);
        xmlNode.AppendChild((XmlNode) newChild1);
        if (!Directory.Exists(Application.StartupPath + "\\PTS"))
          Directory.CreateDirectory(Application.StartupPath + "\\PTS");
        MemoryStream memoryStream = new MemoryStream();
        File.Delete(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
        File.Delete(Application.StartupPath + "\\PTS\\temp.zip");
        xmlDocument.Save(Application.StartupPath + "\\PTS\\PTS" + this.EvrakNo + ".xml");
        ZipOutputStream zipOutputStream = new ZipOutputStream((Stream) File.OpenWrite(Application.StartupPath + "\\PTS\\temp.zip"));
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
          string str = BildirimHelper.PaketGonder(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.AktifEvrak.GLNNO);
          int? tip = this.AktifEvrak.TIP;
          int num3 = 5;
          if (tip.GetValueOrDefault() == num3 && tip.HasValue)
          {
            TBLSIPAMAS tblsipamas = MyUtils.Firma.TBLSIPAMAS.Where<TBLSIPAMAS>((Expression<Func<TBLSIPAMAS, bool>>) (u => (int) u.SUBE_KODU == 0 & u.FTIRSIP == "6" & u.FATIRS_NO == this.AktifEvrak.EVRAK_NO & u.CARI_KODU == this.AktifEvrak.CARI_KOD)).First<TBLSIPAMAS>();
            tblsipamas.EXGUMRUKNO = str;
            MyUtils.Firma.SaveChanges();
            int num4 = (int) MessageBox.Show("Paket Gönderilmiştir.\rTransfer ID :" + tblsipamas.EXGUMRUKNO);
          }
          else
          {
            string fsip3 = this.AktifEvrak.FTIRSIP;
            string fatno3 = this.AktifEvrak.EVRAK_NO;
            TBLFATUIRS tblfatuirs = MyUtils.Firma.TBLFATUIRS.Where<TBLFATUIRS>((Expression<Func<TBLFATUIRS, bool>>) (u => u.FTIRSIP == fsip3 && u.FATIRS_NO == fatno3)).First<TBLFATUIRS>();
            tblfatuirs.EXGUMRUKNO = str;
            int num5 = (int) MessageBox.Show("Paket Gönderilmiştir.\rTransfer ID :" + tblfatuirs.EXGUMRUKNO);
          }
          MyUtils.Firma.SaveChanges();
        }
        catch (Exception ex)
        {
          int num6 = (int) MessageBox.Show("Hata oluştu:" + ex.Message);
        }
        Cursor.Current = Cursors.Default;
      }
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
          string str1 = "0" + strArray[index].Split('|')[0];
          string str2 = strArray[index].Split('|')[1];
          string str3 = strArray[index].Split('|')[2];
          string str4 = strArray[index].Split('|')[3];
          List<BILDIRIM_STOK> list = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod1)).ToList<BILDIRIM_STOK>();
          if (list.Count > 0)
          {
            double? miktar = list[0].MIKTAR;
            int? karsilanan = list[0].KARSILANAN;
            double? nullable = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
            if (miktar.GetValueOrDefault() > nullable.GetValueOrDefault() && miktar.HasValue & nullable.HasValue)
            {
              if (MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, str1, str2, str4, str3)).ToList<ITSHAR>().Count == 0)
              {
                MyUtils.HareketEkle(str1, this.EvrakNo, this.CariKod, "", str3, str4, str2, list[0].STOK_KODU, this.EvrakID);
                this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
                this.grdKalem.DataSource = (object) this.AktifKalemler;
              }
              else
                stringBuilder.AppendLine(strArray[index]);
            }
            else
              stringBuilder.AppendLine(strArray[index]);
          }
          else
            stringBuilder.AppendLine(strArray[index]);
        }
        this.prgDurum.Value = index + 1;
      }
      int num1 = (int) MessageBox.Show("Aktarım tamamlanmıştır.");
      if (stringBuilder.Length > 1)
      {
        FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
        frmKoliSonuc.Sonuc = stringBuilder.ToString();
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
          this.EvrakTip = EvrakTipi.SIPARIS;
          break;
      }
    }

    private void btnExcelAktar_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Directory.Exists("Export"))
          Directory.CreateDirectory("Export");
        this.grdKalem.ExportToXls("Export\\" + this.AktifEvrak.EVRAK_NO + ".xls");
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
        List<ITSHAR> list = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.TIP == (int?) this.EvrakID & u.STOK_KOD == StokKodu & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>();
        FrmKalemDetay frmKalemDetay = new FrmKalemDetay();
        frmKalemDetay.kalemTipi = KalemTipi.ITS;
        frmKalemDetay.itsHar = (IEnumerable<ITSHAR>) list;
        int num = (int) frmKalemDetay.ShowDialog();
        frmKalemDetay.Dispose();
        MyUtils.Refresh();
        this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
        this.grdKalem.DataSource = (object) this.AktifKalemler;
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
        if (!Directory.Exists(Application.StartupPath + "\\Export"))
          Directory.CreateDirectory(Application.StartupPath + "\\Export");
        TextWriter textWriter = (TextWriter) new StreamWriter(Application.StartupPath + "\\Export\\" + this.AktifEvrak.EVRAK_NO + ".txt");
        List<ITSHAR_VIEW> list = MyUtils.Firma.Database.SqlQuery<ITSHAR_VIEW>(MyUtils.GetITSHAR_VIEWSQL(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR_VIEW>();
        int count = list.Count;
        for (int index = 0; index < count; ++index)
        {
          string str = list[index].BARKOD;
          if (list[index].SERI_NO.Length > 0)
            str = str.Remove(0, 1);
          textWriter.WriteLine(barkod + str + seriNo + list[index].SERI_NO + miad + list[index].MIAD + partiNo + list[index].PARTINO);
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
      double num1 = 0.0;
      if (kalan.GetValueOrDefault() == num1 && kalan.HasValue)
      {
        e.Appearance.BackColor = Color.Green;
        e.Appearance.ForeColor = Color.White;
      }
      else
      {
        int? karsilanan = row.KARSILANAN;
        int num2 = 0;
        if (karsilanan.GetValueOrDefault() > num2 && karsilanan.HasValue)
        {
          e.Appearance.BackColor = Color.Red;
          e.Appearance.ForeColor = Color.White;
        }
      }
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
        double? nullable1;
        int? karsilanan;
        double? nullable2;
        if (strArray[index].Split('|').Length > 2)
        {
          string barkod1 = strArray[index].Split('|')[0].Remove(0, 1);
          string str1 = strArray[index].Split('|')[0];
          string str2 = strArray[index].Split('|')[1];
          string str3 = strArray[index].Split('|')[3];
          string str4 = strArray[index].Split('|')[2];
          List<BILDIRIM_STOK> list = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod1)).ToList<BILDIRIM_STOK>();
          if (list.Count > 0)
          {
            nullable1 = list[0].MIKTAR;
            karsilanan = list[0].KARSILANAN;
            nullable2 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
            if (nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault() && nullable1.HasValue & nullable2.HasValue)
            {
              if (MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, str1, str2, str4, str3)).ToList<ITSHAR>().Count == 0)
                MyUtils.HareketEkle(str1, this.EvrakNo, this.CariKod, "", str3, str4, str2, list[0].STOK_KODU, this.EvrakID);
              else
                stringBuilder.AppendLine(strArray[index] + " - Ürün daha önce okutulmuştur");
            }
            else
              stringBuilder.AppendLine(strArray[index] + " - Ürün fazla okutulmuştur");
          }
          else
            stringBuilder.AppendLine(strArray[index] + " - Ürün siparişde bulunmamaktadır");
        }
        else if (strArray[index].Length > 10)
        {
          string barkod = "";
          if (strArray[index].Substring(0, 3) == "010")
          {
            barkod = strArray[index].Remove(0, 3);
            barkod = barkod.Substring(0, 13);
            string str5 = strArray[index].Remove(0, 18);
            int num = str5.IndexOf("17");
            str5.IndexOf("10");
            bool flag = false;
            string str6 = str5.Substring(num + 2, 8);
            while (!flag)
            {
              DateTime dateTime;
              if (str6.Substring(6, 2) == "10")
              {
                try
                {
                  dateTime = DateTime.ParseExact(str6.Substring(0, 6), "yyMMdd", (IFormatProvider) null);
                  flag = true;
                }
                catch
                {
                  try
                  {
                    DateTime exact = DateTime.ParseExact(str6.Substring(0, 6), new string[2]
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
                num = str5.IndexOf("17", num + 1);
                str6 = str5.Substring(num + 2, 8);
              }
            }
            string str7 = str5.Substring(0, str5.IndexOf(str6) - 2);
            string str8 = str6.Substring(0, 6);
            string str9 = strArray[index].Replace("010" + barkod + "21" + str7 + "17" + str8 + "10", "");
            List<BILDIRIM_STOK> list = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).ToList<BILDIRIM_STOK>();
            if (list.Count > 0)
            {
              nullable2 = list[0].MIKTAR;
              karsilanan = list[0].KARSILANAN;
              nullable1 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
              if (nullable2.GetValueOrDefault() > nullable1.GetValueOrDefault() && nullable2.HasValue & nullable1.HasValue)
              {
                if (MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, "0" + barkod, str7, str9, str8)).ToList<ITSHAR>().Count == 0)
                  MyUtils.HareketEkle("0" + barkod, this.EvrakNo, this.CariKod, "", str8, str9, str7, list[0].STOK_KODU, this.EvrakID);
                else
                  stringBuilder.AppendLine(strArray[index] + " - Ürün daha önce okutulmuştur");
              }
              else
                stringBuilder.AppendLine(strArray[index] + " - Ürün fazla okutulmuştur");
            }
            else
              stringBuilder.AppendLine(strArray[index] + " - Ürün siparişde bulunmamaktadır");
          }
          else
          {
            barkod = strArray[index].Trim();
            List<BILDIRIM_STOK> list = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).ToList<BILDIRIM_STOK>();
            if (list.Count > 0)
            {
              nullable1 = list[0].KALAN;
              double num = 0.0;
              if (nullable1.GetValueOrDefault() > num && nullable1.HasValue)
                MyUtils.HareketEkle("0" + barkod, this.EvrakNo, this.CariKod, "", "", "", "", list[0].STOK_KODU, this.EvrakID);
              else
                stringBuilder.AppendLine(strArray[index] + " - Ürün fazla okutulmuştur");
            }
            else
              stringBuilder.AppendLine(strArray[index] + " - Ürün siparişde bulunmamaktadır");
          }
        }
        this.prgDurum2.Value = index + 1;
      }
      this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
      this.grdKalem.DataSource = (object) this.AktifKalemler;
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

    private void FrmSatisBildirimi_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F5 && this.xtraTabControl1.SelectedTabPage == this.xtraTabPage1)
        this.simpleButton1_Click(sender, new EventArgs());
      if (e.KeyCode == Keys.F3 && this.xtraTabControl1.SelectedTabPage == this.xtraTabPage2)
        this.btnKarekod_Click(sender, new EventArgs());
      if (!e.Alt || e.KeyCode != Keys.F8 || this.xtraTabControl1.SelectedTabPage != this.xtraTabPage2)
        return;
      try
      {
        string sql = "SELECT CARI_TEL,dbo.TRK(CARI_IL) AS CARI_IL,dbo.TRK(CARI_ISIM) AS CARI_ISIM,dbo.TRK(CARI_ADRES)AS CARI_ADRES,dbo.TRK(CARI_ILCE) AS CARI_ILCE, dbo.TRK(VERGI_DAIRESI) AS VERGI_DAIRESI, VERGI_NUMARASI, dbo.TRK(ACIK1) AS ACIK1 FROM TBLCASABIT WHERE CARI_KOD='" + this.AktifEvrak.CARI_KOD + "'";
        List<CariKart> list = MyUtils.Firma.Database.SqlQuery<CariKart>(sql).ToList<CariKart>();
        Report report = new Report();
        report.Load("Etiket.frx");
        report.RegisterData((IEnumerable) list, "Etiket");
        report.Design();
        report.Dispose();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Hata oluştu:" + ex.Message);
      }
    }

    private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
    {
      if (this.xtraTabControl1.SelectedTabPage != this.xtraTabPage2)
        return;
      this.btnKarekod_Click(sender, (EventArgs) null);
    }

    private void FrmSatisBildirimi_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Dispose();
    }

    private void btnYazdir_Click(object sender, EventArgs e)
    {
      if (this.EvrakTip != EvrakTipi.SIPARIS)
      {
        int num1 = (int) MessageBox.Show("Evrak tipi sipariş olmalı...");
      }
      else if (this.EvrakTip == EvrakTipi.SIPARIS && MyUtils.SiparisYazici == "")
      {
        int num2 = (int) MessageBox.Show("Lütfen sipariş yazıcı ayarını yapınız!");
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        Basim o1 = (Basim) null;
        // ISSUE: variable of a compiler-generated type
        NetRS o2 = (NetRS) null;
        try
        {
          if (MyUtils.sirket == null)
            return;
          int num3 = 0;
          int num4 = -1;
          foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
          {
            if (installedPrinter == MyUtils.SiparisYazici)
            {
              num4 = num3 + 1;
              break;
            }
            ++num3;
          }
          if (num4 > -1)
          {
            // ISSUE: reference to a compiler-generated method
            o2 = MyUtils.kernel.yeniNetRS(MyUtils.sirket);
            // ISSUE: reference to a compiler-generated method
            o2.Calistir("UPDATE TBLDIZAYNGENEL SET YEDEK5=" + num4.ToString() + " WHERE GENELID=" + MyUtils.GetParamValue("SiparisDizaynNo"));
            // ISSUE: reference to a compiler-generated method
            o1 = MyUtils.kernel.yeniBasim(MyUtils.sirket);
            // ISSUE: reference to a compiler-generated method
            o1.FaturaBasimDizaynNo(TFaturaTip.ftSSip, this.AktifEvrak.EVRAK_NO, this.AktifEvrak.CARI_KOD, Convert.ToInt32(MyUtils.GetParamValue("SiparisDizaynNo")));
          }
        }
        catch (Exception ex)
        {
          int num5 = (int) MessageBox.Show("Hata oluştu : " + ex.Message);
        }
        finally
        {
          if (o2 != null)
            Marshal.ReleaseComObject((object) o2);
          if (o1 != null)
            Marshal.ReleaseComObject((object) o1);
        }
      }
    }

    private void printControl1_Load(object sender, EventArgs e)
    {
    }

    private void btnFoy_Click(object sender, EventArgs e)
    {
      FrmKareKod frmKareKod = new FrmKareKod();
      if (frmKareKod.ShowDialog() == DialogResult.OK && frmKareKod.strGtin.Length > 0 & frmKareKod.strMiad.Length > 0 & frmKareKod.strPartiNo.Length > 0 & frmKareKod.strseriNo.Length > 0)
      {
        frmKareKod.strPartiNo = frmKareKod.strPartiNo.ToUpper();
        frmKareKod.strseriNo = frmKareKod.strseriNo.ToUpper();
        DateTime dateTime;
        try
        {
          dateTime = DateTime.ParseExact(frmKareKod.strMiad, "yyMMdd", (IFormatProvider) null);
        }
        catch
        {
          dateTime = DateTime.ParseExact(frmKareKod.strMiad, new string[2]
          {
            "yyMMdd",
            "yyMM00"
          }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
          dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }
        frmKareKod.strMiad = dateTime.ToString("yyMMdd");
        FrmFoy frmFoy = new FrmFoy();
        frmFoy.GTIN = frmKareKod.strGtin;
        frmFoy.SERIAL_NUMBER = frmKareKod.strseriNo;
        frmFoy.LOT_NUMBER = frmKareKod.strPartiNo;
        frmFoy.DATE = frmKareKod.strMiad;
        int num = (int) frmFoy.ShowDialog();
        frmFoy.Dispose();
      }
      frmKareKod.Dispose();
    }

    private void btnOperasyon_Click(object sender, EventArgs e)
    {
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int num4 = 0;
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.ResmiFaturaYazici)
        {
          num1 = num4;
          break;
        }
        ++num4;
      }
      int num5 = 0;
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.GayriFaturaYazici)
        {
          num2 = num5;
          break;
        }
        ++num5;
      }
      if (num1 == -1 || num2 == -1)
      {
        int num6 = (int) MessageBox.Show("Lütfen fatura yazıcı ayarını yapınız!");
      }
      else
      {
        FrmEvrakTip frmEvrakTip = new FrmEvrakTip();
        if (frmEvrakTip.ShowDialog() == DialogResult.OK)
        {
          this.DizayNo = frmEvrakTip.DizaynNo;
          num3 = frmEvrakTip.Tip != 1 ? num2 : num1;
        }
        frmEvrakTip.Dispose();
        // ISSUE: variable of a compiler-generated type
        Fatura o1 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        FatUst o2 = (FatUst) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem o3 = (FatKalem) null;
        // ISSUE: variable of a compiler-generated type
        Basim o4 = (Basim) null;
        // ISSUE: variable of a compiler-generated type
        NetRS o5 = (NetRS) null;
        try
        {
          if (MyUtils.sirket == null)
            return;
          // ISSUE: reference to a compiler-generated method
          o1 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSFat);
          // ISSUE: reference to a compiler-generated method
          o1.OkuUst(this.AktifEvrak.EVRAK_NO, this.AktifEvrak.CARI_KOD);
          // ISSUE: reference to a compiler-generated method
          o1.OkuKalem();
          // ISSUE: reference to a compiler-generated method
          o2 = o1.Ust();
          // ISSUE: reference to a compiler-generated method
          o4 = MyUtils.kernel.yeniBasim(MyUtils.sirket);
          // ISSUE: reference to a compiler-generated method
          o5 = MyUtils.kernel.yeniNetRS(MyUtils.sirket);
          // ISSUE: reference to a compiler-generated method
          o5.Calistir("UPDATE TBLDIZAYNGENEL SET YEDEK5=" + num3.ToString() + " WHERE GENELID=" + this.DizayNo);
          // ISSUE: reference to a compiler-generated method
          o4.FaturaBasimDizaynNo(TFaturaTip.ftSFat, o2.FATIRS_NO, this.AktifEvrak.CARI_KOD, Convert.ToInt32(this.DizayNo));
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num7 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
        }
        finally
        {
          if (o5 != null)
            Marshal.ReleaseComObject((object) o5);
          if (o4 != null)
            Marshal.ReleaseComObject((object) o4);
          if (o3 != null)
            Marshal.ReleaseComObject((object) o3);
          if (o2 != null)
            Marshal.ReleaseComObject((object) o2);
          if (o1 != null)
            Marshal.ReleaseComObject((object) o1);
        }
      }
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
        List<FrmSatisBildirimi.HataListe> source1 = new List<FrmSatisBildirimi.HataListe>();
        source1.Add(new FrmSatisBildirimi.HataListe(10007, "Bu Sira Numarasi Zaten Kayitli!", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10008, "Tanimlanmamis Kayit Hatasi.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10201, "Belirtilen urun Sistemimizde Kayitli Degildir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10202, "Ürünün Son Kullanma Tarihi Gecmistir. (Hastaya verilemez.)", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10203, "Ürün Bilgileri Tutarsiz.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10204, "Belirtilen ürün onceden Satilmistir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10205, "Bu ürünün Satisi Yasaklanmistir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10206, "Veritabanı Kayıt Hatası.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10207, "Bu ürün önceden ihrac edilmiştir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10209, "Ürün şu anda başka bir Eczane stoğunda görünüyor.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10210, "Ürün stoğunuzda görünüyor", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10211, "Ürün Stoğunuzda Görünmüyor!", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10214, "Ürün tarafınızdan önceden satılmıstır!", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10219, "Belirtilen ürün tarafınızdan satılmamıştır. Sadece Kendi sattığınız ilacı geri alabilirsiniz", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10220, "Ürün Geri ödeme kurumuna satılmıstır. Satışın Recete Bazlı iptal edilmesi gerekir", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10222, "Ürün Üzerinize Kayitli Degil", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10223, "Ürün Üzerinize Kayitli Görünüyor", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10224, "Ürün Eczane Tarafindan Satilmistir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10225, "Ürün su anda baska bir birimde görünüyor.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10227, "Girilen GLN, eczane GLNsi degildir. GLNnin size ait oldugundan emin olunuz.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10301, "Girilen satici GLNsi yanlistir. Ürünü aldiginiz paydasin GLNsini belirttiginizden emin olunuz.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10302, "Girilen alici GLNsi yanlistir. Ürünü sattiginiz paydasin GLNsini belirttiginizden emin olunuz.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10303, "Belirtilen ürün üzerinize kayitlidir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10304, "Belirtilen ürün üzerinize kayitli degildir.", 0));
        source1.Add(new FrmSatisBildirimi.HataListe(10305, "Belirtilen ürün baska bir paydas üzerine kayitlidir.", 0));
        List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_GENEL(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR>();
        List<KarekodBilgi> its = new List<KarekodBilgi>();
        foreach (ITSHAR itshar in list)
        {
          string miad = BildirimHelper.ConvertMiadTarih(itshar.MIAD).ToString("yyyy-MM-dd");
          its.Add(new KarekodBilgi(itshar.BARKOD, itshar.SERI_NO, itshar.PARTINO, miad));
        }
        List<KarekodBilgi> karekodBilgiList = BildirimHelper.DogrulamaBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), MyUtils.GetParamValue("GlnNo"), its);
        int num3 = 0;
        List<ItsHarClass> source2 = new List<ItsHarClass>();
        foreach (ITSHAR itshar in list)
        {
          ITSHAR item = itshar;
          ItsHarClass itsHarClass = new ItsHarClass();
          itsHarClass.BARKOD = item.BARKOD;
          itsHarClass.CARI_KOD = item.CARI_KOD;
          itsHarClass.DURUM = item.DURUM;
          itsHarClass.EVRAK_SERI = item.EVRAK_SERI;
          itsHarClass.ID = item.ID;
          itsHarClass.MIAD = item.MIAD;
          itsHarClass.PARTINO = item.PARTINO;
          itsHarClass.SERI_NO = item.SERI_NO;
          TBLSTSABIT tblstsabit = MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (o => o.STOK_KODU == item.STOK_KOD)).FirstOrDefault<TBLSTSABIT>();
          if (tblstsabit != null)
            itsHarClass.STOK_ISIM = tblstsabit.STOK_ADI;
          source2.Add(itsHarClass);
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
            source1.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad = source1.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad + 1;
            h.MESAJ = source1.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Durum;
            source2[source2.IndexOf(h)].DURUM = h.DURUM;
          }
          ++num2;
        }
        MyUtils.Refresh();
        this.grdKalemDetay.DataSource = (object) source2;
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.AppendLine("Bildirim Detayları : ");
        foreach (FrmSatisBildirimi.HataListe hataListe in source1)
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

    private void btnTakas_1_Click(object sender, EventArgs e)
    {
      if (this.EvrakTip == EvrakTipi.SIPARIS)
      {
        string sql = "SELECT STOK_KODU FROM (SELECT STOK_KODU, COUNT(*) AS SAYI FROM TBLSIPATRA WHERE FISNO = '" + this.EvrakNo + "' GROUP BY FISNO, STOK_KODU)A WHERE A.SAYI>1";
        List<string> list = MyUtils.Firma.Database.SqlQuery<string>(sql).ToList<string>();
        if (list != null && list.Count > 0)
        {
          using (List<string>.Enumerator enumerator = list.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.MesajGoster(enumerator.Current + " kodlu ürün birden fazla girilmiştir.Lütfen düzeltiniz!!!!");
            return;
          }
        }
      }
      this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
      foreach (BILDIRIM_STOK bildirimStok in this.AktifKalemler)
      {
        double? kalan = bildirimStok.KALAN;
        double num1 = 0.0;
        int num2;
        if ((kalan.GetValueOrDefault() > num1 ? (kalan.HasValue ? 1 : 0) : 0) != 0)
        {
          int? karsilanan = bildirimStok.KARSILANAN;
          int num3 = 0;
          if ((karsilanan.GetValueOrDefault() > num3 ? (karsilanan.HasValue ? 1 : 0) : 0) != 0)
          {
            num2 = bildirimStok.KOSULKODU.IndexOf("-MF") > -1 ? 1 : 0;
            goto label_13;
          }
        }
        num2 = 0;
label_13:
        if (num2 != 0)
        {
          int num4 = (int) MessageBox.Show(bildirimStok.STOK_ISIM + " isimli ürün MF'lidir lütfen tamamlayınız!!!!");
          return;
        }
      }
      if (sender != this.mnuDepo1 && sender != this.mnuDepo2 && sender != this.mnuDepo3)
        return;
      int num5 = -1;
      int num6 = -1;
      int num7 = -1;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      int num8 = 0;
      StringBuilder stringBuilder1 = new StringBuilder();
      if (this.cbTip.SelectedIndex == 4)
      {
        foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        {
          if (installedPrinter == MyUtils.ResmiFaturaYazici)
          {
            num5 = num8;
            str1 = MyUtils.ResmiFaturaYazici;
            break;
          }
          ++num8;
        }
        int num9 = 0;
        foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        {
          if (installedPrinter == MyUtils.GayriFaturaYazici)
          {
            num6 = num9;
            str3 = MyUtils.GayriFaturaYazici;
            break;
          }
          ++num9;
        }
        if (this.EvrakTip == EvrakTipi.SIPARIS && (num5 == -1 || num6 == -1))
        {
          int num10 = (int) MessageBox.Show("Lütfen fatura yazıcı ayarını yapınız!");
          return;
        }
        if (this.EvrakTip == EvrakTipi.SIPARIS)
        {
          if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "" && MyUtils.GetParamValue("FaturaDizaynNo") != "")
          {
            FrmEvrakTip frmEvrakTip = new FrmEvrakTip();
            if (frmEvrakTip.ShowDialog() == DialogResult.OK)
            {
              this.EvrakSeri = frmEvrakTip.EvrakSeri;
              this.DizayNo = frmEvrakTip.DizaynNo;
              if (frmEvrakTip.Tip == 1)
              {
                num7 = num5;
                str3 = str1;
              }
              else
              {
                num7 = num6;
                str3 = str2;
              }
            }
            frmEvrakTip.Dispose();
          }
          else if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "")
          {
            this.EvrakSeri = MyUtils.GetParamValue("GayriEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("GayriFaturaDizaynNo");
            num7 = num6;
            str3 = str2;
          }
          else
          {
            this.EvrakSeri = MyUtils.GetParamValue("ResmiEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("FaturaDizaynNo");
            num7 = num5;
            str3 = str1;
          }
          if (string.IsNullOrEmpty(this.EvrakSeri))
          {
            int num11 = (int) MessageBox.Show("Lütfen Evrak Seri giriniz!!!");
            return;
          }
        }
      }
      string str4 = "";
      string paramValue1;
      string paramValue2;
      if (sender == this.mnuDepo1)
      {
        str4 = MyUtils.GetParamValue("EczGlnNo1");
        paramValue1 = MyUtils.GetParamValue("EczKullanici1");
        paramValue2 = MyUtils.GetParamValue("EczSifre1");
      }
      else if (sender == this.mnuDepo2)
      {
        str4 = MyUtils.GetParamValue("EczGlnNo2");
        paramValue1 = MyUtils.GetParamValue("EczKullanici2");
        paramValue2 = MyUtils.GetParamValue("EczSifre2");
      }
      else
      {
        str4 = MyUtils.GetParamValue("EczGlnNo3");
        paramValue1 = MyUtils.GetParamValue("EczKullanici3");
        paramValue2 = MyUtils.GetParamValue("EczSifre3");
      }
      try
      {
        if (this.AktifKalemler.Sum<BILDIRIM_STOK>((Func<BILDIRIM_STOK, double?>) (u => u.KALAN)).Value > 0.0 && MessageBox.Show("Okutulmayan kalemler mevcut\r Bildirim yapmak istediğinize eminmisiniz?", "Bildirim Uyarı", MessageBoxButtons.YesNo) == DialogResult.No)
          return;
        this.Cursor = Cursors.WaitCursor;
        MyUtils.Refresh();
        int num12 = 0;
        int num13 = 0;
        List<FrmSatisBildirimi.HataListe> source = new List<FrmSatisBildirimi.HataListe>();
        foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) MyUtils.Firma.HATA_KODLARI)
          source.Add(new FrmSatisBildirimi.HataListe()
          {
            HataKod = hataKodlari.HATAID,
            Ad = 0,
            Durum = hataKodlari.MESAJ
          });
        MyUtils.Refresh();
        List<ITSHAR_VIEW> list1 = MyUtils.Firma.Database.SqlQuery<ITSHAR_VIEW>(MyUtils.GetITSHAR_VIEWSQL(this.EvrakNo, this.CariKod, this.EvrakID)).ToList<ITSHAR_VIEW>();
        this.grdKalemDetay.DataSource = (object) null;
        this.grdKalemDetay.DataSource = (object) list1;
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.AppendLine("Bildirim Detayları : ");
        foreach (FrmSatisBildirimi.HataListe hataListe in source)
        {
          if (hataListe.Ad > 0)
          {
            stringBuilder2.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
            hataListe.Ad = 0;
          }
        }
        List<ITSHAR> list2 = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_GENEL(this.EvrakNo, this.CariKod, this.EvrakID)).Where<ITSHAR>((Func<ITSHAR, bool>) (u => u.SERI_NO != "")).ToList<ITSHAR>();
        List<KarekodBilgi> its = new List<KarekodBilgi>();
        foreach (ITSHAR itshar in list2)
        {
          string miad = BildirimHelper.ConvertMiadTarih(itshar.MIAD).ToString("yyyy-MM-dd");
          its.Add(new KarekodBilgi(itshar.BARKOD, itshar.SERI_NO, itshar.PARTINO, miad));
        }
        List<KarekodBilgi> karekodBilgiList = new List<KarekodBilgi>();
        switch (this.cbTip.SelectedIndex)
        {
          case 0:
          case 4:
            karekodBilgiList = BildirimHelper.MalDevirBildirimi(paramValue1, paramValue2, this.AktifEvrak.GLNNO, its);
            if (this.cbTip.SelectedIndex == 0)
            {
              string fsip = this.AktifEvrak.FTIRSIP;
              string fatno = this.AktifEvrak.EVRAK_NO;
              TBLFATUIRS tblfatuirs = MyUtils.Firma.TBLFATUIRS.Where<TBLFATUIRS>((Expression<Func<TBLFATUIRS, bool>>) (u => u.FTIRSIP == fsip && u.FATIRS_NO == fatno)).FirstOrDefault<TBLFATUIRS>();
              if (tblfatuirs != null)
                tblfatuirs.S_YEDEK1 = "OK";
              MyUtils.Firma.SaveChanges();
              break;
            }
            TBLSIPAMAS tblsipamas = MyUtils.Firma.TBLSIPAMAS.Where<TBLSIPAMAS>((Expression<Func<TBLSIPAMAS, bool>>) (u => (int) u.SUBE_KODU == 0 & u.FTIRSIP == "6" & u.FATIRS_NO == this.AktifEvrak.EVRAK_NO & u.CARI_KODU == this.AktifEvrak.CARI_KOD)).FirstOrDefault<TBLSIPAMAS>();
            if (tblsipamas != null)
              tblsipamas.S_YEDEK1 = "OK";
            MyUtils.Firma.SaveChanges();
            break;
          case 1:
            karekodBilgiList = BildirimHelper.MalAlimBildirimi(paramValue1, paramValue2, its);
            break;
          case 2:
            karekodBilgiList = BildirimHelper.MalDevirIptalBildirimi(paramValue1, paramValue2, its);
            break;
          case 3:
            karekodBilgiList = BildirimHelper.MalIadeBildirimi(paramValue1, paramValue2, this.AktifEvrak.GLNNO, its);
            break;
        }
        foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
        {
          KarekodBilgi item = karekodBilgi;
          item.Barkod.Remove(0, 1);
          ITSHAR h = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).ToList<ITSHAR>()[0];
          if (h != null)
          {
            h.DURUM = item.Sonuc;
            if (item.Sonuc == "00000")
              ++num12;
            source.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad = source.Where<FrmSatisBildirimi.HataListe>((Func<FrmSatisBildirimi.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmSatisBildirimi.HataListe>()[0].Ad + 1;
            MyUtils.Firma.SaveChanges();
          }
          ++num13;
        }
        stringBuilder2.AppendLine("Bildirim Durumu : " + num12.ToString() + " / " + num13.ToString());
        this.memSonuc.Text = stringBuilder2.ToString();
        this.Cursor = Cursors.Default;
        this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
        if (this.EvrakTip != EvrakTipi.SIPARIS || num12 != num13)
          return;
        if (MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) == DialogResult.Yes)
          this.btnPaketAl_Click(sender, e);
        int num14 = (int) MessageBox.Show("Sipariş Faturalaştırılacak.Lütfen bekleyiniz!....");
        // ISSUE: variable of a compiler-generated type
        Fatura o1 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        Fatura o2 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        FatUst o3 = (FatUst) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem o4 = (FatKalem) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem sipKalem = (FatKalem) null;
        // ISSUE: variable of a compiler-generated type
        Basim o5 = (Basim) null;
        // ISSUE: variable of a compiler-generated type
        NetRS o6 = (NetRS) null;
        stringBuilder1 = new StringBuilder();
        try
        {
          if (MyUtils.sirket != null)
          {
            // ISSUE: reference to a compiler-generated method
            o1 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSSip);
            // ISSUE: reference to a compiler-generated method
            o1.OkuUst(this.AktifEvrak.EVRAK_NO, this.AktifEvrak.CARI_KOD);
            // ISSUE: reference to a compiler-generated method
            o1.OkuKalem();
            // ISSUE: reference to a compiler-generated method
            // ISSUE: variable of a compiler-generated type
            FatUst fatUst = o1.Ust();
            // ISSUE: reference to a compiler-generated method
            o2 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSFat);
            o2.KosulluHesapla = false;
            // ISSUE: reference to a compiler-generated method
            o3 = o2.Ust();
            // ISSUE: reference to a compiler-generated method
            o5 = MyUtils.kernel.yeniBasim(MyUtils.sirket);
            string projeKodu = fatUst.Proje_Kodu;
            o3.CariKod = this.AktifEvrak.CARI_KOD;
            o3.SIPARIS_TEST = DateTime.Now;
            o3.FiiliTarih = DateTime.Now;
            o3.Tarih = DateTime.Now;
            // ISSUE: reference to a compiler-generated method
            o3.FATIRS_NO = o2.YeniNumara(this.EvrakSeri);
            o3.TIPI = TFaturaTipi.ft_Acik;
            o3.PLA_KODU = fatUst.PLA_KODU;
            o3.Proje_Kodu = projeKodu;
            o3.KOD1 = fatUst.KOD1;
            o3.KOSULKODU = fatUst.KOSULKODU;
            try
            {
              o3.KOSULTARIHI = fatUst.KOSULTARIHI;
            }
            catch
            {
            }
            try
            {
              o3.FIYATTARIHI = fatUst.FIYATTARIHI;
            }
            catch
            {
            }
            o3.KDV_DAHILMI = fatUst.KDV_DAHILMI;
            o3.GENISK1TIP = fatUst.GENISK1TIP;
            o3.GENISK2TIP = fatUst.GENISK2TIP;
            o3.GENISK3TIP = fatUst.GENISK3TIP;
            o3.GEN_ISK1O = fatUst.GEN_ISK1O;
            o3.GEN_ISK2O = fatUst.GEN_ISK2O;
            o3.GEN_ISK3O = fatUst.GEN_ISK3O;
            o3.GEN_ISK1T = fatUst.GEN_ISK1T;
            o3.GEN_ISK2T = fatUst.GEN_ISK2T;
            o3.GEN_ISK3T = fatUst.GEN_ISK3T;
            this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
            for (int Index = 0; Index < o1.KalemAdedi; ++Index)
            {
              // ISSUE: reference to a compiler-generated method
              sipKalem = o1.get_Kalem(Index);
              BILDIRIM_STOK bildirimStok = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.STOK_KODU == sipKalem.StokKodu)).First<BILDIRIM_STOK>();
              int? karsilanan = bildirimStok.KARSILANAN;
              int num15 = 0;
              if (karsilanan.GetValueOrDefault() > num15 && karsilanan.HasValue)
              {
                // ISSUE: reference to a compiler-generated method
                o4 = o2.kalemYeni(bildirimStok.STOK_KODU);
                o4.StokKodu = bildirimStok.STOK_KODU;
                o4.STra_GCMIK = Convert.ToDouble((object) bildirimStok.KARSILANAN) - sipKalem.STra_MALFISK;
                o4.STra_MALFISK = sipKalem.STra_MALFISK;
                o4.STra_BF = sipKalem.STra_BF;
                o4.STra_NF = sipKalem.STra_NF;
                o4.STra_SatIsk = sipKalem.STra_SatIsk;
                o4.STra_SatIsk2 = sipKalem.STra_SatIsk2;
                o4.STra_SatIsk3 = sipKalem.STra_SatIsk3;
                o4.STra_KDV = sipKalem.STra_KDV;
                o4.STra_SIPNUM = this.AktifEvrak.EVRAK_NO;
                o4.STra_SIPKONT = sipKalem.STra_SIPKONT;
                o4.Stra_KosulK = sipKalem.Stra_KosulK;
                o4.STra_ODEGUN = sipKalem.STra_ODEGUN;
                o4.ProjeKodu = fatUst.Proje_Kodu;
                o4.Plasiyer_Kodu = fatUst.PLA_KODU;
                o4.Olcubr = sipKalem.Olcubr;
                o4.STra_KOD1 = sipKalem.STra_KOD1;
              }
            }
            // ISSUE: reference to a compiler-generated method
            o2.kayitYeni();
            // ISSUE: reference to a compiler-generated method
            o6 = MyUtils.kernel.yeniNetRS(MyUtils.sirket);
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLSIPAMAS SET SIRA_NO=-1 WHERE FATIRS_NO='" + o3.FATIRS_NO + "' AND FTIRSIP='6'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE ITSHAR SET TIP=1,EVRAK_SERI='" + o3.FATIRS_NO + "' WHERE TIP=5 AND EVRAK_SERI='" + this.AktifEvrak.EVRAK_NO + "' AND CARI_KOD='" + this.AktifEvrak.CARI_KOD + "'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLFATUIRS SET S_YEDEK1='OK' WHERE FTIRSIP='1' AND FATIRS_NO='" + o3.FATIRS_NO + "' AND TIPI=2 AND CARI_KODU='" + this.AktifEvrak.CARI_KOD + "'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLDIZAYNGENEL SET YEDEK5=" + num7.ToString() + " WHERE GENELID=" + this.DizayNo);
            // ISSUE: reference to a compiler-generated method
            o5.FaturaBasimDizaynNo(TFaturaTip.ftSFat, o3.FATIRS_NO, this.AktifEvrak.CARI_KOD, Convert.ToInt32(this.DizayNo));
            this.YeniEvrak(sender, e);
          }
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num16 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
        }
        finally
        {
          if (o6 != null)
            Marshal.ReleaseComObject((object) o6);
          if (o5 != null)
            Marshal.ReleaseComObject((object) o5);
          if (o4 != null)
            Marshal.ReleaseComObject((object) o4);
          if (o3 != null)
            Marshal.ReleaseComObject((object) o3);
          if (o2 != null)
            Marshal.ReleaseComObject((object) o2);
          if (o1 != null)
            Marshal.ReleaseComObject((object) o1);
        }
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num17 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
      }
    }

    private void btnTakas_Click(object sender, EventArgs e)
    {
      this.mnuDepo1.Text = MyUtils.GetParamValue("Eczane1");
      this.mnuDepo2.Text = MyUtils.GetParamValue("Eczane2");
      this.mnuDepo3.Text = MyUtils.GetParamValue("Eczane3");
      this.contextMenuStrip1.Show((Control) this.btnTakas, new Point(this.btnTakas.Width, 0));
    }

    private void btnBildirim_Click(object sender, EventArgs e)
    {
      this.mnuFirma1.Text = this.AktifEvrak.CARI_UNVAN;
      this.mnuFirma2.Text = MyUtils.GetParamValue("Eczane1");
      this.mnuFirma3.Text = MyUtils.GetParamValue("Eczane2");
      this.contextMenuStrip3.Show((Control) this.btnBildirim, new Point(this.btnBildirim.Width, 0));
    }

    private void btnFaturaBas_Click(object sender, EventArgs e)
    {
      if (this.EvrakTip == EvrakTipi.SIPARIS)
      {
        string sql = "SELECT STOK_KODU FROM (SELECT STOK_KODU, COUNT(*) AS SAYI FROM TBLSIPATRA WHERE FISNO = '" + this.EvrakNo + "' GROUP BY FISNO, STOK_KODU)A WHERE A.SAYI>1";
        List<string> list = MyUtils.Firma.Database.SqlQuery<string>(sql).ToList<string>();
        if (list != null && list.Count > 0)
        {
          using (List<string>.Enumerator enumerator = list.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.MesajGoster(enumerator.Current + " kodlu ürün birden fazla girilmiştir.Lütfen düzeltiniz!!!!");
            return;
          }
        }
      }
      this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
      foreach (BILDIRIM_STOK bildirimStok in this.AktifKalemler)
      {
        double? kalan = bildirimStok.KALAN;
        double num1 = 0.0;
        int num2;
        if ((kalan.GetValueOrDefault() > num1 ? (kalan.HasValue ? 1 : 0) : 0) != 0)
        {
          int? karsilanan = bildirimStok.KARSILANAN;
          int num3 = 0;
          if ((karsilanan.GetValueOrDefault() > num3 ? (karsilanan.HasValue ? 1 : 0) : 0) != 0)
          {
            num2 = bildirimStok.KOSULKODU.IndexOf("-MF") > -1 ? 1 : 0;
            goto label_13;
          }
        }
        num2 = 0;
label_13:
        if (num2 != 0)
        {
          int num4 = (int) MessageBox.Show(bildirimStok.STOK_ISIM + " isimli ürün MF'lidir lütfen tamamlayınız!!!!");
          return;
        }
      }
      int num5 = 0;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      string str1 = "";
      string str2 = "";
      string str3 = "";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.ResmiFaturaYazici)
        {
          num6 = num5 + 1;
          str3 = MyUtils.ResmiFaturaYazici;
          break;
        }
        ++num5;
      }
      int num9 = 0;
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
      {
        if (installedPrinter == MyUtils.GayriFaturaYazici)
        {
          num7 = num9 + 1;
          str2 = MyUtils.GayriFaturaYazici;
          break;
        }
        ++num9;
      }
      if (this.EvrakTip == EvrakTipi.SIPARIS && (num6 == -1 || num7 == -1))
      {
        int num10 = (int) MessageBox.Show("Lütfen fatura yazıcı ayarını yapınız!");
      }
      else
      {
        if (this.EvrakTip == EvrakTipi.SIPARIS)
        {
          if (string.IsNullOrEmpty(this.EvrakSira))
            this.EvrakSira = "1";
          if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "" && MyUtils.GetParamValue("FaturaDizaynNo") != "")
          {
            FrmEvrakTip frmEvrakTip = new FrmEvrakTip();
            if (frmEvrakTip.ShowDialog() == DialogResult.OK)
            {
              this.EvrakSeri = frmEvrakTip.EvrakSeri;
              this.DizayNo = frmEvrakTip.DizaynNo;
              if (frmEvrakTip.Tip == 1)
              {
                num8 = num6;
                str1 = str3;
              }
              else
              {
                num8 = num7;
                str1 = str2;
              }
            }
            frmEvrakTip.Dispose();
          }
          else if (MyUtils.GetParamValue("GayriFaturaDizaynNo") != "")
          {
            this.EvrakSeri = MyUtils.GetParamValue("GayriEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("GayriFaturaDizaynNo");
            num8 = num7;
            str1 = str2;
          }
          else
          {
            this.EvrakSeri = MyUtils.GetParamValue("ResmiEvrakSeri");
            this.DizayNo = MyUtils.GetParamValue("FaturaDizaynNo");
            num8 = num6;
            str1 = str3;
          }
          if (string.IsNullOrEmpty(this.EvrakSeri))
          {
            int num11 = (int) MessageBox.Show("Lütfen Evrak Seri giriniz!!!");
            return;
          }
        }
        string str4 = string.Empty;
        string empty = string.Empty;
        if (this.EvrakTip != EvrakTipi.SIPARIS)
          return;
        int num12 = (int) MessageBox.Show("Sipariş Faturalaştırılacak.Lütfen bekleyiniz!....");
        // ISSUE: variable of a compiler-generated type
        Fatura o1 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        Fatura o2 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        FatUst o3 = (FatUst) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem o4 = (FatKalem) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem sipKalem = (FatKalem) null;
        // ISSUE: variable of a compiler-generated type
        Basim o5 = (Basim) null;
        // ISSUE: variable of a compiler-generated type
        NetRS o6 = (NetRS) null;
        stringBuilder = new StringBuilder();
        try
        {
          if (MyUtils.sirket != null)
          {
            // ISSUE: reference to a compiler-generated method
            o1 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSSip);
            // ISSUE: reference to a compiler-generated method
            o1.OkuUst(this.AktifEvrak.EVRAK_NO, this.AktifEvrak.CARI_KOD);
            // ISSUE: reference to a compiler-generated method
            o1.OkuKalem();
            // ISSUE: reference to a compiler-generated method
            // ISSUE: variable of a compiler-generated type
            FatUst fatUst = o1.Ust();
            // ISSUE: reference to a compiler-generated method
            o2 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftSFat);
            o2.KosulluHesapla = false;
            // ISSUE: reference to a compiler-generated method
            o3 = o2.Ust();
            // ISSUE: reference to a compiler-generated method
            string evraksira = o2.YeniNumara(this.EvrakSeri).Replace(this.EvrakSeri, "");
            FrmEvrakNo frmEvrakNo = new FrmEvrakNo(this.EvrakSeri, evraksira);
            if (frmEvrakNo.ShowDialog() == DialogResult.OK)
            {
              str4 = frmEvrakNo.EvrakSeri;
              evraksira = frmEvrakNo.EvrakSira;
            }
            frmEvrakNo.Dispose();
            if (string.IsNullOrEmpty(evraksira))
              throw new Exception("Lütfen Evrak Seri giriniz!!!");
            // ISSUE: reference to a compiler-generated method
            o5 = MyUtils.kernel.yeniBasim(MyUtils.sirket);
            string projeKodu = fatUst.Proje_Kodu;
            o3.CariKod = this.AktifEvrak.CARI_KOD;
            o3.SIPARIS_TEST = DateTime.Now;
            o3.FiiliTarih = DateTime.Now;
            o3.Tarih = DateTime.Now;
            o3.FATIRS_NO = str4 + evraksira;
            o3.TIPI = TFaturaTipi.ft_Acik;
            o3.PLA_KODU = fatUst.PLA_KODU;
            o3.Proje_Kodu = projeKodu;
            o3.KOD1 = fatUst.KOD1;
            o3.KOSULKODU = fatUst.KOSULKODU;
            try
            {
              o3.KOSULTARIHI = fatUst.KOSULTARIHI;
            }
            catch
            {
            }
            try
            {
              o3.FIYATTARIHI = fatUst.FIYATTARIHI;
            }
            catch
            {
            }
            o3.KDV_DAHILMI = fatUst.KDV_DAHILMI;
            o3.GENISK1TIP = fatUst.GENISK1TIP;
            o3.GENISK2TIP = fatUst.GENISK2TIP;
            o3.GENISK3TIP = fatUst.GENISK3TIP;
            o3.GEN_ISK1O = fatUst.GEN_ISK1O;
            o3.GEN_ISK2O = fatUst.GEN_ISK2O;
            o3.GEN_ISK3O = fatUst.GEN_ISK3O;
            o3.GEN_ISK1T = fatUst.GEN_ISK1T;
            o3.GEN_ISK2T = fatUst.GEN_ISK2T;
            o3.GEN_ISK3T = fatUst.GEN_ISK3T;
            this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
            for (int Index = 0; Index < o1.KalemAdedi; ++Index)
            {
              // ISSUE: reference to a compiler-generated method
              sipKalem = o1.get_Kalem(Index);
              BILDIRIM_STOK bildirimStok = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.STOK_KODU == sipKalem.StokKodu)).First<BILDIRIM_STOK>();
              int? karsilanan = bildirimStok.KARSILANAN;
              int num13 = 0;
              if (karsilanan.GetValueOrDefault() > num13 && karsilanan.HasValue)
              {
                // ISSUE: reference to a compiler-generated method
                o4 = o2.kalemYeni(bildirimStok.STOK_KODU);
                o4.StokKodu = bildirimStok.STOK_KODU;
                o4.STra_GCMIK = Convert.ToDouble((object) bildirimStok.KARSILANAN) - sipKalem.STra_MALFISK;
                o4.STra_MALFISK = sipKalem.STra_MALFISK;
                o4.STra_BF = sipKalem.STra_BF;
                o4.STra_NF = sipKalem.STra_NF;
                o4.STra_SatIsk = sipKalem.STra_SatIsk;
                o4.STra_SatIsk2 = sipKalem.STra_SatIsk2;
                o4.STra_SatIsk3 = sipKalem.STra_SatIsk3;
                o4.STra_KDV = sipKalem.STra_KDV;
                o4.STra_SIPNUM = this.AktifEvrak.EVRAK_NO;
                o4.STra_SIPKONT = sipKalem.STra_SIPKONT;
                o4.Stra_KosulK = sipKalem.Stra_KosulK;
                o4.STra_ODEGUN = sipKalem.STra_ODEGUN;
                o4.ProjeKodu = fatUst.Proje_Kodu;
                o4.Plasiyer_Kodu = fatUst.PLA_KODU;
                o4.Olcubr = sipKalem.Olcubr;
                o4.STra_KOD1 = sipKalem.STra_KOD1;
              }
            }
            // ISSUE: reference to a compiler-generated method
            o2.kayitYeni();
            // ISSUE: reference to a compiler-generated method
            o6 = MyUtils.kernel.yeniNetRS(MyUtils.sirket);
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLSIPAMAS SET SIRA_NO=-1 WHERE FATIRS_NO='" + o3.FATIRS_NO + "' AND FTIRSIP='6'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE ITSHAR SET TIP=1,EVRAK_SERI='" + o3.FATIRS_NO + "' WHERE TIP=5 AND EVRAK_SERI='" + this.AktifEvrak.EVRAK_NO + "' AND CARI_KOD='" + this.AktifEvrak.CARI_KOD + "'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLFATUIRS SET S_YEDEK1='OK' WHERE FTIRSIP='1' AND FATIRS_NO='" + o3.FATIRS_NO + "' AND TIPI=2 AND CARI_KODU='" + this.AktifEvrak.CARI_KOD + "'");
            // ISSUE: reference to a compiler-generated method
            o6.Calistir("UPDATE TBLDIZAYNGENEL SET YEDEK5=" + num8.ToString() + " WHERE GENELID=" + this.DizayNo);
            // ISSUE: reference to a compiler-generated method
            o5.FaturaBasimDizaynNo(TFaturaTip.ftSFat, o3.FATIRS_NO, this.AktifEvrak.CARI_KOD, Convert.ToInt32(this.DizayNo));
            this.YeniEvrak(sender, e);
          }
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num14 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
        }
        finally
        {
          if (o6 != null)
            Marshal.ReleaseComObject((object) o6);
          if (o5 != null)
            Marshal.ReleaseComObject((object) o5);
          if (o4 != null)
            Marshal.ReleaseComObject((object) o4);
          if (o3 != null)
            Marshal.ReleaseComObject((object) o3);
          if (o2 != null)
            Marshal.ReleaseComObject((object) o2);
          if (o1 != null)
            Marshal.ReleaseComObject((object) o1);
        }
      }
    }

    private void gridView2_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.C)
        return;
      GridControl gridControl = sender as GridControl;
      Clipboard.SetText((this.grdKalem.FocusedView as GridView).GetFocusedDisplayText());
      e.Handled = true;
    }

    private void txtBarkod_Validated(object sender, EventArgs e)
    {
    }

    private void button2_Click(object sender, EventArgs e) => this.txtBarkod.Clear();

    private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (new KeyEventArgs((Keys) e.KeyChar).KeyCode != Keys.Return)
        return;
      if (this.chkKoli2.Checked)
      {
        this.prgDurum.Minimum = 0;
        this.prgDurum.Value = 0;
        int num1 = 0;
        string koli = this.txtBarkod.Text;
        StringBuilder stringBuilder = new StringBuilder();
        List<TBLTRANSFER_DETAY> list = MyUtils.Firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.CARRIER_LABEL == koli)).ToList<TBLTRANSFER_DETAY>();
        if (list.Count > 0)
        {
          this.prgDurum.Maximum = list.Count;
          foreach (TBLTRANSFER_DETAY tbltransferDetay in list)
          {
            Application.DoEvents();
            string barkod = tbltransferDetay.GTIN;
            barkod = barkod.Remove(0, 1);
            DateTime dateTime;
            try
            {
              dateTime = DateTime.ParseExact(tbltransferDetay.DATE, "yyyy-MM-dd", (IFormatProvider) null);
            }
            catch
            {
              dateTime = DateTime.ParseExact(tbltransferDetay.DATE, new string[2]
              {
                "yyyy-MM-dd",
                "yyyy-MM-00"
              }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
              dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
            }
            string str = dateTime.ToString("yyMMdd");
            BILDIRIM_STOK bildirimStok1 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).FirstOrDefault<BILDIRIM_STOK>();
            if (bildirimStok1 != null)
            {
              double? nullable1 = bildirimStok1.MIKTAR;
              int? karsilanan = bildirimStok1.KARSILANAN;
              double? nullable2 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
              if (nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault() && nullable1.HasValue & nullable2.HasValue)
              {
                if (MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, tbltransferDetay.GTIN, tbltransferDetay.SERIAL_NUMBER, tbltransferDetay.LOT_NUMBER, str)).ToList<ITSHAR>().Count == 0)
                {
                  MyUtils.HareketEkle(tbltransferDetay.GTIN, this.EvrakNo, this.CariKod, "", str, tbltransferDetay.LOT_NUMBER, tbltransferDetay.SERIAL_NUMBER, bildirimStok1.STOK_KODU, this.EvrakID);
                  BILDIRIM_STOK bildirimStok2 = bildirimStok1;
                  karsilanan = bildirimStok1.KARSILANAN;
                  int? nullable3 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() + 1) : new int?();
                  bildirimStok2.KARSILANAN = nullable3;
                  BILDIRIM_STOK bildirimStok3 = bildirimStok1;
                  double? kalan = bildirimStok1.KALAN;
                  double num2 = 1.0;
                  double? nullable4;
                  if (!kalan.HasValue)
                  {
                    nullable1 = new double?();
                    nullable4 = nullable1;
                  }
                  else
                    nullable4 = new double?(kalan.GetValueOrDefault() - num2);
                  bildirimStok3.KALAN = nullable4;
                  this.grdKalem.DataSource = (object) null;
                  this.grdKalem.DataSource = (object) this.AktifKalemler;
                }
                else
                  stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
              }
              else
                stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
            }
            else
              stringBuilder.AppendLine(tbltransferDetay.GTIN + " | " + tbltransferDetay.SERIAL_NUMBER + " | " + tbltransferDetay.LOT_NUMBER + " | " + tbltransferDetay.DATE);
            this.prgDurum.Value = num1 + 1;
          }
          this.prgDurum.Value = 0;
          this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
          this.grdKalem.DataSource = (object) this.AktifKalemler;
        }
        if (stringBuilder.Length > 1)
        {
          FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
          frmKoliSonuc.Sonuc = stringBuilder.ToString();
          int num3 = (int) frmKoliSonuc.ShowDialog();
          frmKoliSonuc.Dispose();
        }
      }
      else if (this.txtBarkod.Text.Substring(0, 3) == "010")
      {
        string barkod = this.txtBarkod.Text.Remove(0, 3);
        barkod = barkod.Substring(0, 13);
        string str1 = this.txtBarkod.Text.Remove(0, 18);
        int num4 = str1.IndexOf("17");
        str1.IndexOf("10");
        bool flag = false;
        string str2 = str1.Substring(num4 + 2, 8);
        while (!flag)
        {
          if (str2.Substring(6, 2) == "10")
          {
            try
            {
              DateTime.ParseExact(str2.Substring(0, 6), "yyMMdd", (IFormatProvider) null);
              flag = true;
            }
            catch
            {
              try
              {
                DateTime dateTime = DateTime.ParseExact(str2.Substring(0, 6), new string[2]
                {
                  "yyMMdd",
                  "yyMM00"
                }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
                dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
                flag = true;
              }
              catch
              {
              }
            }
          }
          if (!flag)
          {
            num4 = str1.IndexOf("17", num4 + 1);
            str2 = str1.Substring(num4 + 2, 8);
          }
        }
        string str3 = str1.Substring(0, str1.IndexOf(str2) - 2);
        string str4 = str2.Substring(0, 6);
        string str5 = this.txtBarkod.Text.Replace("010" + barkod + "21" + str3 + "17" + str4 + "10", "");
        BILDIRIM_STOK bildirimStok4 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).FirstOrDefault<BILDIRIM_STOK>();
        if (bildirimStok4 != null)
        {
          double? nullable5 = bildirimStok4.MIKTAR;
          int? karsilanan = bildirimStok4.KARSILANAN;
          double? nullable6 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
          if (nullable5.GetValueOrDefault() > nullable6.GetValueOrDefault() && nullable5.HasValue & nullable6.HasValue)
          {
            List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, "0" + barkod, str3, str5, str4)).ToList<ITSHAR>();
            if (list.Count == 0)
            {
              MyUtils.HareketEkle("0" + barkod, this.EvrakNo, this.CariKod, "", str4, str5, str3, bildirimStok4.STOK_KODU, this.EvrakID);
              BILDIRIM_STOK bildirimStok5 = bildirimStok4;
              karsilanan = bildirimStok4.KARSILANAN;
              int? nullable7 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() + 1) : new int?();
              bildirimStok5.KARSILANAN = nullable7;
              BILDIRIM_STOK bildirimStok6 = bildirimStok4;
              double? kalan = bildirimStok4.KALAN;
              double num5 = 1.0;
              double? nullable8;
              if (!kalan.HasValue)
              {
                nullable5 = new double?();
                nullable8 = nullable5;
              }
              else
                nullable8 = new double?(kalan.GetValueOrDefault() - num5);
              bildirimStok6.KALAN = nullable8;
              this.grdKalem.DataSource = (object) null;
              this.grdKalem.DataSource = (object) this.AktifKalemler;
              int num6 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
              if (num6 != int.MinValue)
                this.gridView2.FocusedRowHandle = num6;
            }
            else if (this.chkTekrar.Checked)
            {
              Console.Beep(1000, 1000);
              if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
              {
                MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM ITSHAR WHERE ID=" + list[0].ID.ToString());
                MyUtils.Refresh();
                BILDIRIM_STOK bildirimStok7 = bildirimStok4;
                karsilanan = bildirimStok4.KARSILANAN;
                int? nullable9 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() - 1) : new int?();
                bildirimStok7.KARSILANAN = nullable9;
                BILDIRIM_STOK bildirimStok8 = bildirimStok4;
                double? kalan = bildirimStok4.KALAN;
                double num7 = 1.0;
                double? nullable10;
                if (!kalan.HasValue)
                {
                  nullable5 = new double?();
                  nullable10 = nullable5;
                }
                else
                  nullable10 = new double?(kalan.GetValueOrDefault() + num7);
                bildirimStok8.KALAN = nullable10;
                this.grdKalem.DataSource = (object) null;
                this.grdKalem.DataSource = (object) this.AktifKalemler;
                int num8 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
                if (num8 != int.MinValue)
                  this.gridView2.FocusedRowHandle = num8;
              }
            }
          }
          else
          {
            List<ITSHAR> list = MyUtils.Firma.Database.SqlQuery<ITSHAR>(MyUtils.GetITSHAR_URUN(this.EvrakNo, this.CariKod, this.EvrakID, "0" + barkod, str3, str5, str4)).ToList<ITSHAR>();
            if (list.Count > 0)
            {
              if (this.chkTekrar.Checked)
              {
                Console.Beep(1000, 1000);
                if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                  MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM ITSHAR WHERE ID=" + list[0].ID.ToString());
                  MyUtils.Refresh();
                  BILDIRIM_STOK bildirimStok9 = bildirimStok4;
                  karsilanan = bildirimStok4.KARSILANAN;
                  int? nullable11 = karsilanan.HasValue ? new int?(karsilanan.GetValueOrDefault() - 1) : new int?();
                  bildirimStok9.KARSILANAN = nullable11;
                  BILDIRIM_STOK bildirimStok10 = bildirimStok4;
                  double? kalan = bildirimStok4.KALAN;
                  double num9 = 1.0;
                  double? nullable12;
                  if (!kalan.HasValue)
                  {
                    nullable5 = new double?();
                    nullable12 = nullable5;
                  }
                  else
                    nullable12 = new double?(kalan.GetValueOrDefault() + num9);
                  bildirimStok10.KALAN = nullable12;
                  this.grdKalem.DataSource = (object) null;
                  this.grdKalem.DataSource = (object) this.AktifKalemler;
                  int num10 = this.gridView2.LocateByValue("BARKOD", (object) barkod);
                  if (num10 != int.MinValue)
                    this.gridView2.FocusedRowHandle = num10;
                }
              }
            }
            else
            {
              Console.Beep(1000, 1000);
              this.MesajGoster("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
            }
          }
        }
        else
        {
          Console.Beep(1000, 1000);
          this.MesajGoster("Bu ürün bu siparişde yoktur...");
        }
      }
      else if (this.txtBarkod.Text.Length > 0)
      {
        List<BILDIRIM_STOK> list = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == this.txtBarkod.Text)).ToList<BILDIRIM_STOK>();
        if (list.Count > 0)
        {
          if (list[0].REYON_KODU != "BESERI")
          {
            double num11 = list[0].KALAN.Value;
            double? kalan = list[0].KALAN;
            double num12 = 0.0;
            if (kalan.GetValueOrDefault() > num12 && kalan.HasValue)
            {
              MyUtils.HareketEkle("0" + this.txtBarkod.Text, this.EvrakNo, this.CariKod, "", "", "", "", list[0].STOK_KODU, this.EvrakID);
              this.AktifKalemler = MyUtils.Firma.Database.SqlQuery<BILDIRIM_STOK>(MyUtils.GetBildirimSQL(this.EvrakNo, this.CariKod, this.EvrakID, this.strBeseri)).ToList<BILDIRIM_STOK>();
              this.grdKalem.DataSource = (object) this.AktifKalemler;
              int num13 = this.gridView2.LocateByValue("BARKOD", (object) this.txtBarkod.Text);
              if (num13 != int.MinValue)
                this.gridView2.FocusedRowHandle = num13;
            }
            else
            {
              Console.Beep(1000, 1000);
              this.MesajGoster("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
            }
          }
        }
        else
        {
          Console.Beep(1000, 1000);
          this.MesajGoster("Bu ürün bu siparişde yoktur...");
        }
      }
      this.txtBarkod.Clear();
      this.txtBarkod.Focus();
    }

    private void btnEtiketYazdir_Click(object sender, EventArgs e)
    {
      try
      {
        FrmAdet frmAdet = new FrmAdet();
        int num = (int) frmAdet.ShowDialog();
        int adet = frmAdet.Adet;
        frmAdet.Dispose();
        string sql = "SELECT CARI_TEL,dbo.TRK(CARI_IL) AS CARI_IL,dbo.TRK(CARI_ISIM) AS CARI_ISIM,dbo.TRK(CARI_ADRES)AS CARI_ADRES,dbo.TRK(CARI_ILCE) AS CARI_ILCE, dbo.TRK(VERGI_DAIRESI) AS VERGI_DAIRESI, VERGI_NUMARASI, dbo.TRK(ACIK1) AS ACIK1 FROM TBLCASABIT WHERE CARI_KOD='" + this.AktifEvrak.CARI_KOD + "'";
        List<CariKart> list = MyUtils.Firma.Database.SqlQuery<CariKart>(sql).ToList<CariKart>();
        Report report = new Report();
        report.Load("Etiket.frx");
        report.RegisterData((IEnumerable) list, "Etiket");
        report.PrintSettings.ShowDialog = false;
        report.PrintSettings.Copies = adet;
        report.PrintSettings.Printer = MyUtils.EtiketYazici;
        report.Print();
        report.Dispose();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Hata oluştu:" + ex.Message);
      }
    }

    private void FrmSatisBildirimi_Load(object sender, EventArgs e)
    {
      this.txtTarih.DateTime = DateTime.Now.Date;
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
      this.xtraTabPage1 = new XtraTabPage();
      this.panelControl3 = new PanelControl();
      this.grdSiparis = new GridControl();
      this.gridView1 = new GridView();
      this.colRECNO = new GridColumn();
      this.colTARIH = new GridColumn();
      this.colEVRAK_SERI = new GridColumn();
      this.colCARI_KOD = new GridColumn();
      this.colCARI_UNVAN = new GridColumn();
      this.colCARI_IL = new GridColumn();
      this.colCARI_ILCE = new GridColumn();
      this.gridColumn4 = new GridColumn();
      this.gridColumn5 = new GridColumn();
      this.colACIKLAMA = new GridColumn();
      this.colSAAT = new GridColumn();
      this.colTUTAR = new GridColumn();
      this.panelControl2 = new PanelControl();
      this.txtEvrakSeri = new TextEdit();
      this.labelControl3 = new LabelControl();
      this.chkBildirim = new CheckBox();
      this.simpleButton1 = new SimpleButton();
      this.txtTarih = new DateEdit();
      this.labelControl2 = new LabelControl();
      this.xtraTabPage2 = new XtraTabPage();
      this.panelControl5 = new PanelControl();
      this.panel3 = new Panel();
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
      this.colREYON_KODU = new GridColumn();
      this.pnlHizli = new Panel();
      this.txtHizli = new MemoEdit();
      this.panel2 = new Panel();
      this.txtBarkod = new TextBox();
      this.chkKoli2 = new CheckBox();
      this.button2 = new Button();
      this.panel4 = new Panel();
      this.panel5 = new Panel();
      this.prgDurum2 = new System.Windows.Forms.ProgressBar();
      this.btnTemizle = new SimpleButton();
      this.btnGuncelle = new SimpleButton();
      this.button1 = new Button();
      this.panelControl4 = new PanelControl();
      this.panel1 = new Panel();
      this.prgDurum = new System.Windows.Forms.ProgressBar();
      this.btnEtiketYazdir = new SimpleButton();
      this.btnTakas = new SimpleButton();
      this.btnDogrulama = new SimpleButton();
      this.btnOperasyon = new SimpleButton();
      this.btnFoy = new SimpleButton();
      this.btnYazdir = new SimpleButton();
      this.btnGonder = new SimpleButton();
      this.btnSeriAktar = new SimpleButton();
      this.btnExcelAktar = new SimpleButton();
      this.btnImport = new SimpleButton();
      this.btnBildirim = new SimpleButton();
      this.btnKarekod = new SimpleButton();
      this.pnlUst = new PanelControl();
      this.btnFaturaBas = new SimpleButton();
      this.chkBasari = new CheckBox();
      this.chkTekrar = new CheckBox();
      this.chkHizliMod = new CheckBox();
      this.lblDurum = new Label();
      this.xtraTabPage3 = new XtraTabPage();
      this.panelControl7 = new PanelControl();
      this.grdKalemDetay = new GridControl();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
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
      this.saveFileDialog1 = new SaveFileDialog();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.mnuDepo1 = new ToolStripMenuItem();
      this.mnuDepo2 = new ToolStripMenuItem();
      this.mnuDepo3 = new ToolStripMenuItem();
      this.contextMenuStrip3 = new ContextMenuStrip(this.components);
      this.mnuFirma1 = new ToolStripMenuItem();
      this.mnuFirma2 = new ToolStripMenuItem();
      this.mnuFirma3 = new ToolStripMenuItem();
      this.simpleButton2 = new SimpleButton();
      this.panelControl1.BeginInit();
      this.panelControl1.SuspendLayout();
      this.xtraTabControl1.BeginInit();
      this.xtraTabControl1.SuspendLayout();
      this.xtraTabPage1.SuspendLayout();
      this.panelControl3.BeginInit();
      this.panelControl3.SuspendLayout();
      this.grdSiparis.BeginInit();
      this.gridView1.BeginInit();
      this.panelControl2.BeginInit();
      this.panelControl2.SuspendLayout();
      this.txtEvrakSeri.Properties.BeginInit();
      this.txtTarih.Properties.CalendarTimeProperties.BeginInit();
      this.txtTarih.Properties.BeginInit();
      this.xtraTabPage2.SuspendLayout();
      this.panelControl5.BeginInit();
      this.panelControl5.SuspendLayout();
      this.panel3.SuspendLayout();
      this.grdKalem.BeginInit();
      this.gridView2.BeginInit();
      this.pnlHizli.SuspendLayout();
      this.txtHizli.Properties.BeginInit();
      this.panel2.SuspendLayout();
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
      this.contextMenuStrip2.SuspendLayout();
      this.gridView3.BeginInit();
      this.panelControl6.BeginInit();
      this.panelControl6.SuspendLayout();
      this.memSonuc.Properties.BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.contextMenuStrip3.SuspendLayout();
      this.SuspendLayout();
      this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Silver";
      this.panelControl1.Controls.Add((Control) this.cbTip);
      this.panelControl1.Dock = DockStyle.Top;
      this.panelControl1.Location = new Point(0, 0);
      this.panelControl1.Name = "panelControl1";
      this.panelControl1.Size = new Size(1117, 40);
      this.panelControl1.TabIndex = 0;
      this.cbTip.Dock = DockStyle.Fill;
      this.cbTip.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTip.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.cbTip.FormattingEnabled = true;
      this.cbTip.Items.AddRange(new object[5]
      {
        (object) "Satış Bildirimi(Fatura)",
        (object) "Alış Bildirimi",
        (object) "Satış İptal Bildirimi",
        (object) "Alış İptal Bildirimi",
        (object) "Satış Bildirimi(Sipariş)"
      });
      this.cbTip.Location = new Point(2, 2);
      this.cbTip.Name = "cbTip";
      this.cbTip.Size = new Size(1113, 37);
      this.cbTip.TabIndex = 3;
      this.cbTip.SelectedIndexChanged += new EventHandler(this.cbTip_SelectedIndexChanged);
      this.xtraTabControl1.Dock = DockStyle.Fill;
      this.xtraTabControl1.HeaderAutoFill = DefaultBoolean.False;
      this.xtraTabControl1.Location = new Point(0, 40);
      this.xtraTabControl1.LookAndFeel.SkinName = "VS2010";
      this.xtraTabControl1.Name = "xtraTabControl1";
      this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
      this.xtraTabControl1.Size = new Size(1117, 470);
      this.xtraTabControl1.TabIndex = 2;
      this.xtraTabControl1.TabPages.AddRange(new XtraTabPage[3]
      {
        this.xtraTabPage1,
        this.xtraTabPage2,
        this.xtraTabPage3
      });
      this.xtraTabControl1.SelectedPageChanged += new TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
      this.xtraTabPage1.Controls.Add((Control) this.panelControl3);
      this.xtraTabPage1.Controls.Add((Control) this.panelControl2);
      this.xtraTabPage1.Name = "xtraTabPage1";
      this.xtraTabPage1.Size = new Size(1111, 442);
      this.xtraTabPage1.Text = "Faturalar";
      this.panelControl3.Controls.Add((Control) this.grdSiparis);
      this.panelControl3.Dock = DockStyle.Fill;
      this.panelControl3.Location = new Point(0, 33);
      this.panelControl3.Name = "panelControl3";
      this.panelControl3.Size = new Size(1111, 409);
      this.panelControl3.TabIndex = 2;
      this.grdSiparis.Dock = DockStyle.Fill;
      this.grdSiparis.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdSiparis.Location = new Point(2, 2);
      this.grdSiparis.MainView = (BaseView) this.gridView1;
      this.grdSiparis.Name = "grdSiparis";
      this.grdSiparis.Size = new Size(1107, 405);
      this.grdSiparis.TabIndex = 1;
      this.grdSiparis.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView1
      });
      this.gridView1.Appearance.EvenRow.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.EvenRow.Options.UseFont = true;
      this.gridView1.Appearance.FocusedRow.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.FocusedRow.Options.UseFont = true;
      this.gridView1.Appearance.OddRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.OddRow.Options.UseFont = true;
      this.gridView1.Appearance.Row.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.Row.Options.UseFont = true;
      this.gridView1.Appearance.SelectedRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.SelectedRow.Options.UseFont = true;
      this.gridView1.Columns.AddRange(new GridColumn[12]
      {
        this.colRECNO,
        this.colTARIH,
        this.colEVRAK_SERI,
        this.colCARI_KOD,
        this.colCARI_UNVAN,
        this.colCARI_IL,
        this.colCARI_ILCE,
        this.gridColumn4,
        this.gridColumn5,
        this.colACIKLAMA,
        this.colSAAT,
        this.colTUTAR
      });
      this.gridView1.GridControl = this.grdSiparis;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView1.OptionsBehavior.Editable = false;
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      this.colRECNO.FieldName = "RECNO";
      this.colRECNO.Name = "colRECNO";
      this.colTARIH.Caption = "Tarih";
      this.colTARIH.FieldName = "TARIH";
      this.colTARIH.Name = "colTARIH";
      this.colTARIH.OptionsColumn.AllowEdit = false;
      this.colTARIH.Width = 113;
      this.colEVRAK_SERI.Caption = "Evrak No";
      this.colEVRAK_SERI.FieldName = "EVRAK_NO";
      this.colEVRAK_SERI.Name = "colEVRAK_SERI";
      this.colEVRAK_SERI.OptionsColumn.AllowEdit = false;
      this.colEVRAK_SERI.Visible = true;
      this.colEVRAK_SERI.VisibleIndex = 0;
      this.colEVRAK_SERI.Width = 196;
      this.colCARI_KOD.Caption = "Cari Kodu";
      this.colCARI_KOD.FieldName = "CARI_KOD";
      this.colCARI_KOD.Name = "colCARI_KOD";
      this.colCARI_KOD.OptionsColumn.AllowEdit = false;
      this.colCARI_KOD.Visible = true;
      this.colCARI_KOD.VisibleIndex = 1;
      this.colCARI_KOD.Width = 112;
      this.colCARI_UNVAN.Caption = "Cari Ünvan";
      this.colCARI_UNVAN.FieldName = "CARI_UNVAN";
      this.colCARI_UNVAN.Name = "colCARI_UNVAN";
      this.colCARI_UNVAN.OptionsColumn.AllowEdit = false;
      this.colCARI_UNVAN.Visible = true;
      this.colCARI_UNVAN.VisibleIndex = 2;
      this.colCARI_UNVAN.Width = 447;
      this.colCARI_IL.Caption = "İl";
      this.colCARI_IL.FieldName = "CARI_IL";
      this.colCARI_IL.Name = "colCARI_IL";
      this.colCARI_IL.OptionsColumn.AllowEdit = false;
      this.colCARI_IL.Visible = true;
      this.colCARI_IL.VisibleIndex = 4;
      this.colCARI_IL.Width = 53;
      this.colCARI_ILCE.Caption = "İlçe";
      this.colCARI_ILCE.FieldName = "CARI_ILCE";
      this.colCARI_ILCE.Name = "colCARI_ILCE";
      this.colCARI_ILCE.Visible = true;
      this.colCARI_ILCE.VisibleIndex = 5;
      this.gridColumn4.Caption = "Sıra No";
      this.gridColumn4.FieldName = "SIRANO";
      this.gridColumn4.Name = "gridColumn4";
      this.gridColumn4.Width = 89;
      this.gridColumn5.Caption = "Sip.Durum";
      this.gridColumn5.FieldName = "SIPDURUM";
      this.gridColumn5.Name = "gridColumn5";
      this.gridColumn5.Width = 95;
      this.colACIKLAMA.Caption = "Açıklama";
      this.colACIKLAMA.FieldName = "ACIKLAMA";
      this.colACIKLAMA.Name = "colACIKLAMA";
      this.colACIKLAMA.Visible = true;
      this.colACIKLAMA.VisibleIndex = 3;
      this.colACIKLAMA.Width = 168;
      this.colSAAT.Caption = "Saat";
      this.colSAAT.FieldName = "SAAT";
      this.colSAAT.Name = "colSAAT";
      this.colSAAT.Visible = true;
      this.colSAAT.VisibleIndex = 6;
      this.colTUTAR.Caption = "Tutar";
      this.colTUTAR.FieldName = "TUTAR";
      this.colTUTAR.Name = "colTUTAR";
      this.colTUTAR.Visible = true;
      this.colTUTAR.VisibleIndex = 7;
      this.panelControl2.Controls.Add((Control) this.txtEvrakSeri);
      this.panelControl2.Controls.Add((Control) this.labelControl3);
      this.panelControl2.Controls.Add((Control) this.chkBildirim);
      this.panelControl2.Controls.Add((Control) this.simpleButton1);
      this.panelControl2.Controls.Add((Control) this.txtTarih);
      this.panelControl2.Controls.Add((Control) this.labelControl2);
      this.panelControl2.Dock = DockStyle.Top;
      this.panelControl2.Location = new Point(0, 0);
      this.panelControl2.Name = "panelControl2";
      this.panelControl2.Size = new Size(1111, 33);
      this.panelControl2.TabIndex = 1;
      this.txtEvrakSeri.Location = new Point(311, 8);
      this.txtEvrakSeri.Name = "txtEvrakSeri";
      this.txtEvrakSeri.Size = new Size(100, 20);
      this.txtEvrakSeri.TabIndex = 5;
      this.labelControl3.Appearance.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.labelControl3.Appearance.Options.UseFont = true;
      this.labelControl3.Location = new Point(217, 8);
      this.labelControl3.Name = "labelControl3";
      this.labelControl3.Size = new Size(83, 19);
      this.labelControl3.TabIndex = 4;
      this.labelControl3.Text = "Evrak Seri";
      this.chkBildirim.AutoSize = true;
      this.chkBildirim.Location = new Point(596, 8);
      this.chkBildirim.Name = "chkBildirim";
      this.chkBildirim.Size = new Size(145, 17);
      this.chkBildirim.TabIndex = 3;
      this.chkBildirim.Text = "Bildirimi yapılanları göster";
      this.chkBildirim.UseVisualStyleBackColor = true;
      this.simpleButton1.Dock = DockStyle.Right;
      this.simpleButton1.Location = new Point(1034, 2);
      this.simpleButton1.Name = "simpleButton1";
      this.simpleButton1.Size = new Size(75, 29);
      this.simpleButton1.TabIndex = 2;
      this.simpleButton1.Text = "Yenile (F5)";
      this.simpleButton1.Click += new EventHandler(this.simpleButton1_Click);
      this.txtTarih.EditValue = (object) null;
      this.txtTarih.Location = new Point(63, 8);
      this.txtTarih.Name = "txtTarih";
      this.txtTarih.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.txtTarih.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.txtTarih.Size = new Size(139, 20);
      this.txtTarih.TabIndex = 1;
      this.labelControl2.Appearance.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.labelControl2.Appearance.Options.UseFont = true;
      this.labelControl2.Location = new Point(9, 7);
      this.labelControl2.Name = "labelControl2";
      this.labelControl2.Size = new Size(43, 19);
      this.labelControl2.TabIndex = 0;
      this.labelControl2.Text = "Tarih";
      this.xtraTabPage2.Controls.Add((Control) this.panelControl5);
      this.xtraTabPage2.Controls.Add((Control) this.panelControl4);
      this.xtraTabPage2.Controls.Add((Control) this.pnlUst);
      this.xtraTabPage2.Name = "xtraTabPage2";
      this.xtraTabPage2.Size = new Size(1111, 442);
      this.xtraTabPage2.Text = "Fatura Kalemleri";
      this.panelControl5.Controls.Add((Control) this.panel3);
      this.panelControl5.Controls.Add((Control) this.pnlHizli);
      this.panelControl5.Controls.Add((Control) this.button1);
      this.panelControl5.Dock = DockStyle.Fill;
      this.panelControl5.Location = new Point(0, 31);
      this.panelControl5.Name = "panelControl5";
      this.panelControl5.Size = new Size(1111, 348);
      this.panelControl5.TabIndex = 4;
      this.panel3.Controls.Add((Control) this.grdKalem);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(2, (int) sbyte.MaxValue);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(1107, 219);
      this.panel3.TabIndex = 9;
      this.grdKalem.Dock = DockStyle.Fill;
      this.grdKalem.Location = new Point(0, 0);
      this.grdKalem.MainView = (BaseView) this.gridView2;
      this.grdKalem.Name = "grdKalem";
      this.grdKalem.Size = new Size(1107, 219);
      this.grdKalem.TabIndex = 4;
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
      this.gridView2.Columns.AddRange(new GridColumn[10]
      {
        this.colRECNO1,
        this.colCARIHR_RECNO,
        this.gridColumn1,
        this.colSTOK_KODU,
        this.colSTOK_ISIM,
        this.colMIKTAR,
        this.colKARSILANAN,
        this.gridColumn2,
        this.gridColumn3,
        this.colREYON_KODU
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
      this.gridView2.OptionsClipboard.AllowCopy = DefaultBoolean.False;
      this.gridView2.OptionsPrint.PrintFooter = false;
      this.gridView2.OptionsPrint.PrintGroupFooter = false;
      this.gridView2.OptionsPrint.PrintHorzLines = false;
      this.gridView2.OptionsPrint.PrintVertLines = false;
      this.gridView2.OptionsView.ShowFooter = true;
      this.gridView2.OptionsView.ShowGroupedColumns = true;
      this.gridView2.OptionsView.ShowGroupPanel = false;
      this.gridView2.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridView2_CustomDrawCell);
      this.gridView2.KeyDown += new KeyEventHandler(this.gridView2_KeyDown);
      this.gridView2.DoubleClick += new EventHandler(this.gridView2_DoubleClick);
      this.colRECNO1.FieldName = "RECNO";
      this.colRECNO1.Name = "colRECNO1";
      this.colCARIHR_RECNO.FieldName = "CARIHR_RECNO";
      this.colCARIHR_RECNO.Name = "colCARIHR_RECNO";
      this.gridColumn1.Caption = "Barkod";
      this.gridColumn1.FieldName = "BARKOD";
      this.gridColumn1.Name = "gridColumn1";
      this.gridColumn1.Visible = true;
      this.gridColumn1.VisibleIndex = 2;
      this.gridColumn1.Width = 168;
      this.colSTOK_KODU.Caption = "Stok Kodu";
      this.colSTOK_KODU.FieldName = "STOK_KODU";
      this.colSTOK_KODU.Name = "colSTOK_KODU";
      this.colSTOK_KODU.Visible = true;
      this.colSTOK_KODU.VisibleIndex = 3;
      this.colSTOK_KODU.Width = 207;
      this.colSTOK_ISIM.Caption = "Stok İsmi";
      this.colSTOK_ISIM.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM.Name = "colSTOK_ISIM";
      this.colSTOK_ISIM.Visible = true;
      this.colSTOK_ISIM.VisibleIndex = 4;
      this.colSTOK_ISIM.Width = 354;
      this.colMIKTAR.Caption = "Miktar";
      this.colMIKTAR.FieldName = "MIKTAR";
      this.colMIKTAR.Name = "colMIKTAR";
      this.colMIKTAR.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colMIKTAR.Visible = true;
      this.colMIKTAR.VisibleIndex = 5;
      this.colMIKTAR.Width = 58;
      this.colKARSILANAN.Caption = "Karşılanan";
      this.colKARSILANAN.FieldName = "KARSILANAN";
      this.colKARSILANAN.Name = "colKARSILANAN";
      this.colKARSILANAN.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colKARSILANAN.Visible = true;
      this.colKARSILANAN.VisibleIndex = 6;
      this.colKARSILANAN.Width = 49;
      this.gridColumn2.Caption = "Kalan";
      this.gridColumn2.FieldName = "KALAN";
      this.gridColumn2.Name = "gridColumn2";
      this.gridColumn2.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.gridColumn2.Visible = true;
      this.gridColumn2.VisibleIndex = 7;
      this.gridColumn2.Width = 71;
      this.gridColumn3.Caption = "BEŞERİ DURUM";
      this.gridColumn3.FieldName = "REYON_KODU";
      this.gridColumn3.Name = "gridColumn3";
      this.gridColumn3.Visible = true;
      this.gridColumn3.VisibleIndex = 0;
      this.gridColumn3.Width = 98;
      this.colREYON_KODU.Caption = "Reyon Kodu";
      this.colREYON_KODU.FieldName = "BOLUM_KODU";
      this.colREYON_KODU.Name = "colREYON_KODU";
      this.colREYON_KODU.Visible = true;
      this.colREYON_KODU.VisibleIndex = 1;
      this.colREYON_KODU.Width = 84;
      this.pnlHizli.Controls.Add((Control) this.txtHizli);
      this.pnlHizli.Controls.Add((Control) this.panel2);
      this.pnlHizli.Controls.Add((Control) this.panel4);
      this.pnlHizli.Dock = DockStyle.Top;
      this.pnlHizli.Location = new Point(2, 2);
      this.pnlHizli.Name = "pnlHizli";
      this.pnlHizli.Size = new Size(1107, 125);
      this.pnlHizli.TabIndex = 8;
      this.pnlHizli.Visible = false;
      this.txtHizli.Dock = DockStyle.Fill;
      this.txtHizli.Location = new Point(0, 29);
      this.txtHizli.Name = "txtHizli";
      this.txtHizli.Size = new Size(1107, 68);
      this.txtHizli.TabIndex = 6;
      this.panel2.Controls.Add((Control) this.txtBarkod);
      this.panel2.Controls.Add((Control) this.chkKoli2);
      this.panel2.Controls.Add((Control) this.button2);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1107, 29);
      this.panel2.TabIndex = 5;
      this.txtBarkod.Dock = DockStyle.Fill;
      this.txtBarkod.Font = new Font("Tahoma", 12.25f);
      this.txtBarkod.Location = new Point(0, 0);
      this.txtBarkod.Name = "txtBarkod";
      this.txtBarkod.Size = new Size(990, 27);
      this.txtBarkod.TabIndex = 4;
      this.txtBarkod.Enter += new EventHandler(this.txtBarkod_Validated);
      this.txtBarkod.KeyPress += new KeyPressEventHandler(this.txtBarkod_KeyPress);
      this.chkKoli2.AutoSize = true;
      this.chkKoli2.Dock = DockStyle.Right;
      this.chkKoli2.Location = new Point(990, 0);
      this.chkKoli2.Name = "chkKoli2";
      this.chkKoli2.Size = new Size(42, 29);
      this.chkKoli2.TabIndex = 3;
      this.chkKoli2.Text = "Koli";
      this.chkKoli2.UseVisualStyleBackColor = true;
      this.button2.Dock = DockStyle.Right;
      this.button2.Location = new Point(1032, 0);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 29);
      this.button2.TabIndex = 0;
      this.button2.Text = "Temizle";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.panel4.Controls.Add((Control) this.panel5);
      this.panel4.Controls.Add((Control) this.btnTemizle);
      this.panel4.Controls.Add((Control) this.btnGuncelle);
      this.panel4.Dock = DockStyle.Bottom;
      this.panel4.Location = new Point(0, 97);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(1107, 28);
      this.panel4.TabIndex = 0;
      this.panel5.Controls.Add((Control) this.prgDurum2);
      this.panel5.Dock = DockStyle.Fill;
      this.panel5.Location = new Point(190, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(917, 28);
      this.panel5.TabIndex = 18;
      this.prgDurum2.Dock = DockStyle.Fill;
      this.prgDurum2.Location = new Point(0, 0);
      this.prgDurum2.Name = "prgDurum2";
      this.prgDurum2.Size = new Size(917, 28);
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
      this.button1.Location = new Point(479, 264);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 6;
      this.button1.Text = "paket";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Visible = false;
      this.button1.Click += new EventHandler(this.btnPaketAl_Click);
      this.panelControl4.Controls.Add((Control) this.simpleButton2);
      this.panelControl4.Controls.Add((Control) this.panel1);
      this.panelControl4.Controls.Add((Control) this.btnEtiketYazdir);
      this.panelControl4.Controls.Add((Control) this.btnTakas);
      this.panelControl4.Controls.Add((Control) this.btnDogrulama);
      this.panelControl4.Controls.Add((Control) this.btnOperasyon);
      this.panelControl4.Controls.Add((Control) this.btnFoy);
      this.panelControl4.Controls.Add((Control) this.btnYazdir);
      this.panelControl4.Controls.Add((Control) this.btnGonder);
      this.panelControl4.Controls.Add((Control) this.btnSeriAktar);
      this.panelControl4.Controls.Add((Control) this.btnExcelAktar);
      this.panelControl4.Controls.Add((Control) this.btnImport);
      this.panelControl4.Controls.Add((Control) this.btnBildirim);
      this.panelControl4.Controls.Add((Control) this.btnKarekod);
      this.panelControl4.Dock = DockStyle.Bottom;
      this.panelControl4.Location = new Point(0, 379);
      this.panelControl4.Name = "panelControl4";
      this.panelControl4.Size = new Size(1111, 63);
      this.panelControl4.TabIndex = 3;
      this.panel1.Controls.Add((Control) this.prgDurum);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(311, 2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(160, 59);
      this.panel1.TabIndex = 26;
      this.prgDurum.Dock = DockStyle.Fill;
      this.prgDurum.Location = new Point(0, 0);
      this.prgDurum.Name = "prgDurum";
      this.prgDurum.Size = new Size(160, 59);
      this.prgDurum.TabIndex = 9;
      this.btnEtiketYazdir.Dock = DockStyle.Right;
      this.btnEtiketYazdir.ImageOptions.Image = (Image) Resources.if_print_48713;
      this.btnEtiketYazdir.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnEtiketYazdir.Location = new Point(471, 2);
      this.btnEtiketYazdir.Name = "btnEtiketYazdir";
      this.btnEtiketYazdir.Size = new Size(78, 59);
      this.btnEtiketYazdir.TabIndex = 25;
      this.btnEtiketYazdir.Text = "Etiket Yazdır";
      this.btnEtiketYazdir.Click += new EventHandler(this.btnEtiketYazdir_Click);
      this.btnTakas.Appearance.ForeColor = Color.Black;
      this.btnTakas.Appearance.Options.UseForeColor = true;
      this.btnTakas.Dock = DockStyle.Left;
      this.btnTakas.ImageOptions.Image = (Image) Resources.if_Arrows_swap_direction_orientation_switch_1886297;
      this.btnTakas.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnTakas.Location = new Point(237, 2);
      this.btnTakas.Name = "btnTakas";
      this.btnTakas.Size = new Size(74, 59);
      this.btnTakas.TabIndex = 23;
      this.btnTakas.Text = "Takas yap";
      this.btnTakas.Click += new EventHandler(this.btnTakas_Click);
      this.btnDogrulama.Appearance.ForeColor = Color.Black;
      this.btnDogrulama.Appearance.Options.UseForeColor = true;
      this.btnDogrulama.Appearance.Options.UseTextOptions = true;
      this.btnDogrulama.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
      this.btnDogrulama.Dock = DockStyle.Left;
      this.btnDogrulama.ImageOptions.Image = (Image) Resources.if_Artboard_9svg_1579798;
      this.btnDogrulama.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnDogrulama.Location = new Point(150, 2);
      this.btnDogrulama.Name = "btnDogrulama";
      this.btnDogrulama.Size = new Size(87, 59);
      this.btnDogrulama.TabIndex = 21;
      this.btnDogrulama.Text = "Doğrulama yap";
      this.btnDogrulama.Click += new EventHandler(this.btnDogrulama_Click);
      this.btnOperasyon.Dock = DockStyle.Right;
      this.btnOperasyon.ImageOptions.Image = (Image) Resources.if_print_48713;
      this.btnOperasyon.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnOperasyon.Location = new Point(549, 2);
      this.btnOperasyon.Name = "btnOperasyon";
      this.btnOperasyon.Size = new Size(78, 59);
      this.btnOperasyon.TabIndex = 19;
      this.btnOperasyon.Text = "Fatura Yazdır";
      this.btnOperasyon.Visible = false;
      this.btnOperasyon.Click += new EventHandler(this.btnOperasyon_Click);
      this.btnFoy.Dock = DockStyle.Right;
      this.btnFoy.ImageOptions.Image = (Image) Resources.if_files_folders_05_808579;
      this.btnFoy.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnFoy.Location = new Point(627, 2);
      this.btnFoy.Name = "btnFoy";
      this.btnFoy.Size = new Size(74, 59);
      this.btnFoy.TabIndex = 17;
      this.btnFoy.Text = "Karekod Föy";
      this.btnFoy.Click += new EventHandler(this.btnFoy_Click);
      this.btnYazdir.Dock = DockStyle.Right;
      this.btnYazdir.ImageOptions.Image = (Image) Resources.if_print_48713;
      this.btnYazdir.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnYazdir.Location = new Point(701, 2);
      this.btnYazdir.Name = "btnYazdir";
      this.btnYazdir.Size = new Size(79, 59);
      this.btnYazdir.TabIndex = 15;
      this.btnYazdir.Text = "Sip.Yazdır";
      this.btnYazdir.Click += new EventHandler(this.btnYazdir_Click);
      this.btnGonder.Appearance.ForeColor = Color.Black;
      this.btnGonder.Appearance.Options.UseForeColor = true;
      this.btnGonder.Dock = DockStyle.Left;
      this.btnGonder.ImageOptions.Image = (Image) Resources.if_Package_Accept_49598;
      this.btnGonder.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnGonder.Location = new Point(73, 2);
      this.btnGonder.Name = "btnGonder";
      this.btnGonder.Size = new Size(77, 59);
      this.btnGonder.TabIndex = 13;
      this.btnGonder.Text = "Paket Gönder";
      this.btnGonder.Click += new EventHandler(this.btnPaketAl_Click);
      this.btnSeriAktar.Dock = DockStyle.Right;
      this.btnSeriAktar.ImageOptions.Image = (Image) Resources.if_export_share_upload_2931143;
      this.btnSeriAktar.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnSeriAktar.Location = new Point(780, 2);
      this.btnSeriAktar.Name = "btnSeriAktar";
      this.btnSeriAktar.Size = new Size(72, 59);
      this.btnSeriAktar.TabIndex = 11;
      this.btnSeriAktar.Text = "Seri Aktar";
      this.btnSeriAktar.Click += new EventHandler(this.btnSeriAktar_Click);
      this.btnExcelAktar.Dock = DockStyle.Right;
      this.btnExcelAktar.ImageOptions.Image = (Image) Resources.if_excel_272709;
      this.btnExcelAktar.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnExcelAktar.Location = new Point(852, 2);
      this.btnExcelAktar.Name = "btnExcelAktar";
      this.btnExcelAktar.Size = new Size(89, 59);
      this.btnExcelAktar.TabIndex = 9;
      this.btnExcelAktar.Text = "Excel Aktar";
      this.btnExcelAktar.Click += new EventHandler(this.btnExcelAktar_Click);
      this.btnImport.Dock = DockStyle.Right;
      this.btnImport.ImageOptions.Image = (Image) Resources.if_Download_2202271;
      this.btnImport.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnImport.Location = new Point(941, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(73, 59);
      this.btnImport.TabIndex = 7;
      this.btnImport.Text = "Dosya Aktar";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.btnBildirim.Appearance.ForeColor = Color.Black;
      this.btnBildirim.Appearance.Options.UseForeColor = true;
      this.btnBildirim.Dock = DockStyle.Left;
      this.btnBildirim.ImageOptions.Image = (Image) Resources.if_circle_content_send_1495053;
      this.btnBildirim.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnBildirim.Location = new Point(2, 2);
      this.btnBildirim.Name = "btnBildirim";
      this.btnBildirim.Size = new Size(71, 59);
      this.btnBildirim.TabIndex = 5;
      this.btnBildirim.Text = "Bildirim Yap";
      this.btnBildirim.Click += new EventHandler(this.btnBitir1_Click);
      this.btnKarekod.Dock = DockStyle.Right;
      this.btnKarekod.ImageOptions.Image = (Image) Resources.if_finance_10_808669;
      this.btnKarekod.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.btnKarekod.Location = new Point(1014, 2);
      this.btnKarekod.Name = "btnKarekod";
      this.btnKarekod.Size = new Size(95, 59);
      this.btnKarekod.TabIndex = 3;
      this.btnKarekod.Text = "Karekod Oku (F3)";
      this.btnKarekod.Click += new EventHandler(this.btnKarekod_Click);
      this.pnlUst.Controls.Add((Control) this.btnFaturaBas);
      this.pnlUst.Controls.Add((Control) this.chkBasari);
      this.pnlUst.Controls.Add((Control) this.chkTekrar);
      this.pnlUst.Controls.Add((Control) this.chkHizliMod);
      this.pnlUst.Controls.Add((Control) this.lblDurum);
      this.pnlUst.Dock = DockStyle.Top;
      this.pnlUst.Location = new Point(0, 0);
      this.pnlUst.Name = "pnlUst";
      this.pnlUst.Size = new Size(1111, 31);
      this.pnlUst.TabIndex = 0;
      this.btnFaturaBas.Appearance.ForeColor = Color.Black;
      this.btnFaturaBas.Appearance.Options.UseForeColor = true;
      this.btnFaturaBas.ButtonStyle = BorderStyles.Office2003;
      this.btnFaturaBas.Dock = DockStyle.Right;
      this.btnFaturaBas.Location = new Point(636, 2);
      this.btnFaturaBas.Name = "btnFaturaBas";
      this.btnFaturaBas.Size = new Size(95, 27);
      this.btnFaturaBas.TabIndex = 22;
      this.btnFaturaBas.Text = "Fatura bas";
      this.btnFaturaBas.Click += new EventHandler(this.btnFaturaBas_Click);
      this.chkBasari.AutoSize = true;
      this.chkBasari.Checked = true;
      this.chkBasari.CheckState = CheckState.Checked;
      this.chkBasari.Dock = DockStyle.Right;
      this.chkBasari.Location = new Point(731, 2);
      this.chkBasari.Name = "chkBasari";
      this.chkBasari.Size = new Size(184, 27);
      this.chkBasari.TabIndex = 6;
      this.chkBasari.Text = "Başarılı ürünlerin bildirimini yapma";
      this.chkBasari.UseVisualStyleBackColor = true;
      this.chkTekrar.AutoSize = true;
      this.chkTekrar.Checked = true;
      this.chkTekrar.CheckState = CheckState.Checked;
      this.chkTekrar.Dock = DockStyle.Right;
      this.chkTekrar.Location = new Point(915, 2);
      this.chkTekrar.Name = "chkTekrar";
      this.chkTekrar.Size = new Size((int) sbyte.MaxValue, 27);
      this.chkTekrar.TabIndex = 5;
      this.chkTekrar.Text = "Ürün Tekrarını göster";
      this.chkTekrar.UseVisualStyleBackColor = true;
      this.chkHizliMod.AutoSize = true;
      this.chkHizliMod.Dock = DockStyle.Right;
      this.chkHizliMod.Location = new Point(1042, 2);
      this.chkHizliMod.Name = "chkHizliMod";
      this.chkHizliMod.Size = new Size(67, 27);
      this.chkHizliMod.TabIndex = 4;
      this.chkHizliMod.Text = "Hızlı Mod";
      this.chkHizliMod.UseVisualStyleBackColor = true;
      this.chkHizliMod.Click += new EventHandler(this.chkHizliMod_Click);
      this.lblDurum.AutoSize = true;
      this.lblDurum.Dock = DockStyle.Left;
      this.lblDurum.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.lblDurum.ForeColor = Color.FromArgb(0, 64, 0);
      this.lblDurum.Location = new Point(2, 2);
      this.lblDurum.Name = "lblDurum";
      this.lblDurum.Size = new Size(0, 29);
      this.lblDurum.TabIndex = 3;
      this.xtraTabPage3.Controls.Add((Control) this.panelControl7);
      this.xtraTabPage3.Controls.Add((Control) this.btnSorgula);
      this.xtraTabPage3.Controls.Add((Control) this.splitterControl1);
      this.xtraTabPage3.Controls.Add((Control) this.panelControl6);
      this.xtraTabPage3.Name = "xtraTabPage3";
      this.xtraTabPage3.Size = new Size(1111, 442);
      this.xtraTabPage3.Text = "Bildirim Sonuç";
      this.panelControl7.Controls.Add((Control) this.grdKalemDetay);
      this.panelControl7.Dock = DockStyle.Fill;
      this.panelControl7.Location = new Point(0, 23);
      this.panelControl7.Name = "panelControl7";
      this.panelControl7.Size = new Size(1111, 258);
      this.panelControl7.TabIndex = 9;
      this.grdKalemDetay.ContextMenuStrip = this.contextMenuStrip2;
      this.grdKalemDetay.Dock = DockStyle.Fill;
      this.grdKalemDetay.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdKalemDetay.Location = new Point(2, 2);
      this.grdKalemDetay.MainView = (BaseView) this.gridView3;
      this.grdKalemDetay.Name = "grdKalemDetay";
      this.grdKalemDetay.Size = new Size(1107, 254);
      this.grdKalemDetay.TabIndex = 3;
      this.grdKalemDetay.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.exceleGönderToolStripMenuItem
      });
      this.contextMenuStrip2.Name = "contextMenuStrip1";
      this.contextMenuStrip2.Size = new Size(150, 26);
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
      this.btnSorgula.Size = new Size(1111, 23);
      this.btnSorgula.TabIndex = 8;
      this.btnSorgula.Text = "Başarısız olan Ürünleri Sorgula";
      this.btnSorgula.Click += new EventHandler(this.btnSorgula_Click);
      this.splitterControl1.Dock = DockStyle.Bottom;
      this.splitterControl1.Location = new Point(0, 281);
      this.splitterControl1.Name = "splitterControl1";
      this.splitterControl1.Size = new Size(1111, 6);
      this.splitterControl1.TabIndex = 5;
      this.splitterControl1.TabStop = false;
      this.panelControl6.Controls.Add((Control) this.memSonuc);
      this.panelControl6.Dock = DockStyle.Bottom;
      this.panelControl6.Location = new Point(0, 287);
      this.panelControl6.Name = "panelControl6";
      this.panelControl6.Size = new Size(1111, 155);
      this.panelControl6.TabIndex = 3;
      this.memSonuc.Dock = DockStyle.Fill;
      this.memSonuc.Location = new Point(2, 2);
      this.memSonuc.Name = "memSonuc";
      this.memSonuc.Size = new Size(1107, 151);
      this.memSonuc.TabIndex = 0;
      this.saveFileDialog1.DefaultExt = "xls";
      this.saveFileDialog1.Filter = "Excel Dosyası|*.xls";
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuDepo1,
        (ToolStripItem) this.mnuDepo2,
        (ToolStripItem) this.mnuDepo3
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(112, 70);
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
      this.contextMenuStrip3.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuFirma1,
        (ToolStripItem) this.mnuFirma2,
        (ToolStripItem) this.mnuFirma3
      });
      this.contextMenuStrip3.Name = "contextMenuStrip1";
      this.contextMenuStrip3.Size = new Size(112, 70);
      this.mnuFirma1.Name = "mnuFirma1";
      this.mnuFirma1.Size = new Size(111, 22);
      this.mnuFirma1.Text = "Depo 1";
      this.mnuFirma1.Click += new EventHandler(this.btnBitir1_Click);
      this.mnuFirma2.Name = "mnuFirma2";
      this.mnuFirma2.Size = new Size(111, 22);
      this.mnuFirma2.Text = "Depo 2";
      this.mnuFirma2.Click += new EventHandler(this.btnBitir1_Click);
      this.mnuFirma3.Name = "mnuFirma3";
      this.mnuFirma3.Size = new Size(111, 22);
      this.mnuFirma3.Text = "Depo 3";
      this.mnuFirma3.Click += new EventHandler(this.btnBitir1_Click);
      this.simpleButton2.Dock = DockStyle.Right;
      this.simpleButton2.ImageOptions.Image = (Image) Resources.if_print_48713;
      this.simpleButton2.ImageOptions.ImageToTextAlignment = ImageAlignToText.TopCenter;
      this.simpleButton2.Location = new Point(393, 2);
      this.simpleButton2.Name = "simpleButton2";
      this.simpleButton2.Size = new Size(78, 59);
      this.simpleButton2.TabIndex = 27;
      this.simpleButton2.Text = "Etiket Yazdır";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1117, 510);
      this.Controls.Add((Control) this.xtraTabControl1);
      this.Controls.Add((Control) this.panelControl1);
      this.IsMdiContainer = true;
      this.KeyPreview = true;
      this.Name = nameof (FrmSatisBildirimi);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bildirim";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosed += new FormClosedEventHandler(this.FrmSatisBildirimi_FormClosed);
      this.Load += new EventHandler(this.FrmSatisBildirimi_Load);
      this.KeyDown += new KeyEventHandler(this.FrmSatisBildirimi_KeyDown);
      this.panelControl1.EndInit();
      this.panelControl1.ResumeLayout(false);
      this.xtraTabControl1.EndInit();
      this.xtraTabControl1.ResumeLayout(false);
      this.xtraTabPage1.ResumeLayout(false);
      this.panelControl3.EndInit();
      this.panelControl3.ResumeLayout(false);
      this.grdSiparis.EndInit();
      this.gridView1.EndInit();
      this.panelControl2.EndInit();
      this.panelControl2.ResumeLayout(false);
      this.panelControl2.PerformLayout();
      this.txtEvrakSeri.Properties.EndInit();
      this.txtTarih.Properties.CalendarTimeProperties.EndInit();
      this.txtTarih.Properties.EndInit();
      this.xtraTabPage2.ResumeLayout(false);
      this.panelControl5.EndInit();
      this.panelControl5.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.grdKalem.EndInit();
      this.gridView2.EndInit();
      this.pnlHizli.ResumeLayout(false);
      this.txtHizli.Properties.EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
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
      this.contextMenuStrip2.ResumeLayout(false);
      this.gridView3.EndInit();
      this.panelControl6.EndInit();
      this.panelControl6.ResumeLayout(false);
      this.memSonuc.Properties.EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.contextMenuStrip3.ResumeLayout(false);
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
