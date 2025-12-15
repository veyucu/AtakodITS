// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmPaketYonetim
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NetOpenX50;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmPaketYonetim : Form
  {
    private FIRMAEntities firma;
    private TRANSFER_CARI AktifTransfer;
    private List<VIEWTRANSFER_DETAY> AktifKalemler;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private SplitterControl splitterControl1;
    private Panel panel2;
    private Panel panel3;
    private GridControl gridControl1;
    private GridView gridView1;
    private Panel panel5;
    private GridControl gridControl2;
    private GridView gridView3;
    private Panel panel4;
    private TextBox txtTransferID;
    private Label label1;
    private SimpleButton btnListe;
    private SimpleButton btnAlimBildirim;
    private GridColumn colID;
    private GridColumn colTARIH;
    private GridColumn colTRANSFER_ID;
    private GridColumn colKAYNAK_GLNNO;
    private GridColumn colCARI_UNVAN;
    private CheckBox chkBildirim;
    private GridColumn colID1;
    private GridColumn colKAYNAK_GLNNO1;
    private GridColumn colDOCUMENT_NUMBER;
    private GridColumn colDOCUMENT_DATE;
    private GridColumn colCARRIER_LABEL;
    private GridColumn colGTIN;
    private GridColumn colSERIAL_NUMBER;
    private GridColumn colLOT_NUMBER;
    private GridColumn colDATE;
    private GridColumn colKOLI_BARKOD;
    private GridColumn colTRANSFER_ID1;
    private GridColumn colDURUM;
    private GridColumn colSTOK_ISIM;
    private SimpleButton btnSeriAktar;
    private SimpleButton btnAlisIrsaliye;

    public FrmPaketYonetim() => this.InitializeComponent();

    private void btnListe_Click(object sender, EventArgs e)
    {
      this.firma = new FIRMAEntities();
      this.firma.Database.Connection.ConnectionString = MyUtils.strConnString;
      List<TRANSFER_CARI> list = this.firma.TRANSFER_CARI.Where<TRANSFER_CARI>((Expression<Func<TRANSFER_CARI, bool>>) (u => u.DURUM != "OK" || u.DURUM == default (string))).OrderByDescending<TRANSFER_CARI, int>((Expression<Func<TRANSFER_CARI, int>>) (u => u.ID)).ToList<TRANSFER_CARI>();
      if (this.chkBildirim.Checked)
        list = this.firma.TRANSFER_CARI.Where<TRANSFER_CARI>((Expression<Func<TRANSFER_CARI, bool>>) (u => u.DURUM == "OK")).OrderByDescending<TRANSFER_CARI, int>((Expression<Func<TRANSFER_CARI, int>>) (u => u.ID)).ToList<TRANSFER_CARI>();
      if (this.txtTransferID.Text.Trim().Length > 0)
      {
        long r = Convert.ToInt64(this.txtTransferID.Text);
        if (this.chkBildirim.Checked)
          list = this.firma.TRANSFER_CARI.Where<TRANSFER_CARI>((Expression<Func<TRANSFER_CARI, bool>>) (u => u.DURUM == "OK" & u.TRANSFER_ID == r)).OrderByDescending<TRANSFER_CARI, int>((Expression<Func<TRANSFER_CARI, int>>) (u => u.ID)).ToList<TRANSFER_CARI>();
        else
          list = this.firma.TRANSFER_CARI.Where<TRANSFER_CARI>((Expression<Func<TRANSFER_CARI, bool>>) (u => u.TRANSFER_ID == r)).OrderByDescending<TRANSFER_CARI, int>((Expression<Func<TRANSFER_CARI, int>>) (u => u.ID)).ToList<TRANSFER_CARI>();
      }
      this.gridControl2.DataSource = (object) list;
    }

    private void gridView3_DoubleClick(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      long recno = Convert.ToInt64(this.gridView3.GetRowCellValue(this.gridView3.FocusedRowHandle, "TRANSFER_ID").ToString());
      string KaynakGln = this.gridView3.GetRowCellValue(this.gridView3.FocusedRowHandle, "KAYNAK_GLNNO").ToString();
      this.AktifTransfer = MyUtils.Firma.TRANSFER_CARI.Where<TRANSFER_CARI>((Expression<Func<TRANSFER_CARI, bool>>) (u => u.TRANSFER_ID == recno & u.KAYNAK_GLNNO == KaynakGln)).First<TRANSFER_CARI>();
      this.firma = new FIRMAEntities();
      this.firma.Database.Connection.ConnectionString = MyUtils.strConnString;
      this.AktifKalemler = this.firma.VIEWTRANSFER_DETAY.Where<VIEWTRANSFER_DETAY>((Expression<Func<VIEWTRANSFER_DETAY, bool>>) (u => u.TRANSFER_ID == (long?) recno)).ToList<VIEWTRANSFER_DETAY>();
      this.gridControl1.DataSource = (object) this.AktifKalemler;
      Cursor.Current = Cursors.Default;
    }

    private void btnAlimBildirim_Click(object sender, EventArgs e)
    {
      if (this.AktifKalemler.Count <= 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.firma = new FIRMAEntities();
      this.firma.Database.Connection.ConnectionString = MyUtils.strConnString;
      List<PTSHAR_VIEW> list1 = this.firma.PTSHAR_VIEW.Where<PTSHAR_VIEW>((Expression<Func<PTSHAR_VIEW, bool>>) (u => u.TRANSFER_ID == (long?) this.AktifTransfer.TRANSFER_ID & u.KAYNAK_GLNNO == this.AktifTransfer.KAYNAK_GLNNO)).ToList<PTSHAR_VIEW>();
      List<KarekodBilgi> its = new List<KarekodBilgi>();
      foreach (PTSHAR_VIEW ptsharView in list1)
        its.Add(new KarekodBilgi(ptsharView.GTIN, ptsharView.SERIAL_NUMBER, ptsharView.LOT_NUMBER, ptsharView.DATE));
      List<KarekodBilgi> karekodBilgiList = BildirimHelper.MalAlimBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), its);
      int recno = this.AktifTransfer.ID;
      this.firma.TBLTRANSFER.Where<TBLTRANSFER>((Expression<Func<TBLTRANSFER, bool>>) (u => u.ID == recno)).First<TBLTRANSFER>().DURUM = "OK";
      this.firma.SaveChanges();
      List<FrmPaketYonetim.HataListe> source = new List<FrmPaketYonetim.HataListe>();
      foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) this.firma.HATA_KODLARI)
        source.Add(new FrmPaketYonetim.HataListe()
        {
          HataKod = hataKodlari.HATAID,
          Ad = 0,
          Durum = hataKodlari.MESAJ
        });
      int num1 = 0;
      int num2 = 0;
      foreach (KarekodBilgi karekodBilgi in karekodBilgiList)
      {
        KarekodBilgi item = karekodBilgi;
        string Barkod = item.Barkod;
        TBLTRANSFER_DETAY h = this.firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.GTIN == Barkod & u.TRANSFER_ID == (long?) this.AktifTransfer.TRANSFER_ID & u.SERIAL_NUMBER == item.SeriNo & u.KAYNAK_GLNNO == this.AktifTransfer.KAYNAK_GLNNO)).ToList<TBLTRANSFER_DETAY>()[0];
        if (h != null)
        {
          h.DURUM = item.Sonuc;
          if (item.Sonuc == "00000")
            ++num1;
          source.Where<FrmPaketYonetim.HataListe>((Func<FrmPaketYonetim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmPaketYonetim.HataListe>()[0].Ad = source.Where<FrmPaketYonetim.HataListe>((Func<FrmPaketYonetim.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmPaketYonetim.HataListe>()[0].Ad + 1;
          this.firma.SaveChanges();
        }
        ++num2;
      }
      this.firma = new FIRMAEntities();
      this.firma.Database.Connection.ConnectionString = MyUtils.strConnString;
      List<PTSHAR_VIEW> list2 = this.firma.PTSHAR_VIEW.Where<PTSHAR_VIEW>((Expression<Func<PTSHAR_VIEW, bool>>) (u => u.TRANSFER_ID == (long?) this.AktifTransfer.TRANSFER_ID & u.KAYNAK_GLNNO == this.AktifTransfer.KAYNAK_GLNNO)).ToList<PTSHAR_VIEW>();
      FrmBildirimSonuc frmBildirimSonuc = new FrmBildirimSonuc();
      frmBildirimSonuc.BildirimSonuc = list2;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Bildirim Detayları : ");
      foreach (FrmPaketYonetim.HataListe hataListe in source)
      {
        if (hataListe.Ad > 0)
        {
          stringBuilder.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
          hataListe.Ad = 0;
        }
      }
      stringBuilder.AppendLine("Bildirim Durumu : " + num1.ToString() + " / " + num2.ToString());
      frmBildirimSonuc.Sonuc = stringBuilder.ToString();
      this.Cursor = Cursors.Default;
      int num3 = (int) frmBildirimSonuc.ShowDialog();
      frmBildirimSonuc.Dispose();
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        long id = Convert.ToInt64(this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "ID").ToString());
        List<TBLTRANSFER_DETAY> list = MyUtils.Firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => (long) u.ID == id)).ToList<TBLTRANSFER_DETAY>();
        FrmKalemDetay frmKalemDetay = new FrmKalemDetay();
        frmKalemDetay.kalemTipi = KalemTipi.TRANSFER;
        frmKalemDetay.transFer = (IEnumerable<TBLTRANSFER_DETAY>) list;
        int num = (int) frmKalemDetay.ShowDialog();
        frmKalemDetay.Dispose();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnSeriAktar_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Directory.Exists(Application.StartupPath + "\\Export"))
          Directory.CreateDirectory(Application.StartupPath + "\\Export");
        TextWriter textWriter = (TextWriter) new StreamWriter(Application.StartupPath + "\\Export\\TR" + this.AktifTransfer.TRANSFER_ID.ToString() + ".txt");
        int count = this.AktifKalemler.Count;
        for (int index = 0; index < count; ++index)
        {
          string str = this.AktifKalemler[index].GTIN.Remove(0, 1);
          DateTime dateTime;
          try
          {
            dateTime = DateTime.ParseExact(this.AktifKalemler[index].DATE, "yyyy-MM-dd", (IFormatProvider) null);
          }
          catch
          {
            dateTime = DateTime.ParseExact(this.AktifKalemler[index].DATE, new string[2]
            {
              "yyyy-MM-dd",
              "yyyy-MM-00"
            }, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
          }
          textWriter.WriteLine(str + "|" + this.AktifKalemler[index].SERIAL_NUMBER + "|" + dateTime.ToString("yyMMdd") + "|" + this.AktifKalemler[index].LOT_NUMBER + "|");
        }
        textWriter.Close();
        int num = (int) MessageBox.Show("Dosya aktarılmıştır.");
      }
      catch
      {
      }
    }

    private void btnAlisIrsaliye_Click(object sender, EventArgs e)
    {
      if (this.AktifKalemler.Count <= 0)
        return;
      string str = MyUtils.Firma.Database.SqlQuery<string>("SELECT CARI_KOD FROM TBLCASABIT WHERE EMAIL='" + this.AktifTransfer.KAYNAK_GLNNO + "'").FirstOrDefault<string>();
      if (string.IsNullOrEmpty(str))
      {
        int num1 = (int) MessageBox.Show("Cari bulunamadı");
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        List<\u003C\u003Ef__AnonymousType0<string, int>> list = this.AktifKalemler.GroupBy((Func<VIEWTRANSFER_DETAY, string>) (u => u.GTIN), (key, g) => new
        {
          Barkod = key.Remove(0, 1),
          Miktar = g.Count<VIEWTRANSFER_DETAY>()
        }).ToList();
        string paramValue = MyUtils.GetParamValue("AlisIrsaliyeSeri");
        // ISSUE: variable of a compiler-generated type
        Fatura o1 = (Fatura) null;
        // ISSUE: variable of a compiler-generated type
        FatUst o2 = (FatUst) null;
        // ISSUE: variable of a compiler-generated type
        FatKalem o3 = (FatKalem) null;
        try
        {
          // ISSUE: reference to a compiler-generated method
          o1 = MyUtils.kernel.yeniFatura(MyUtils.sirket, TFaturaTip.ftAIrs);
          // ISSUE: reference to a compiler-generated method
          o2 = o1.Ust();
          // ISSUE: reference to a compiler-generated method
          o2.FATIRS_NO = o1.YeniNumara(paramValue);
          o2.CariKod = str;
          o2.Tarih = DateTime.Now;
          o2.FiiliTarih = DateTime.Now;
          o2.SIPARIS_TEST = DateTime.Now;
          o2.FIYATTARIHI = DateTime.Now;
          o2.TIPI = TFaturaTipi.ft_YurtIci;
          o2.KDV_DAHILMI = true;
          foreach (var data in list)
          {
            var item = data;
            if (MyUtils.Firma.TBLSTSABIT.Where<TBLSTSABIT>((Expression<Func<TBLSTSABIT, bool>>) (u => u.STOK_KODU == item.Barkod)).FirstOrDefault<TBLSTSABIT>() != null)
            {
              // ISSUE: reference to a compiler-generated method
              o3 = o1.kalemYeni(item.Barkod);
              o3.STra_GCMIK = Convert.ToDouble(item.Miktar);
            }
          }
          // ISSUE: reference to a compiler-generated method
          o1.kayitYeni();
          int num2 = (int) MessageBox.Show(o2.FATIRS_NO + " nolu irsaliye oluşmuştur");
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num3 = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
        }
        finally
        {
          this.Cursor = Cursors.Default;
          Marshal.ReleaseComObject((object) o3);
          Marshal.ReleaseComObject((object) o2);
          Marshal.ReleaseComObject((object) o1);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.panel5 = new Panel();
      this.gridControl2 = new GridControl();
      this.gridView3 = new GridView();
      this.colID = new GridColumn();
      this.colTARIH = new GridColumn();
      this.colTRANSFER_ID = new GridColumn();
      this.colKAYNAK_GLNNO = new GridColumn();
      this.colCARI_UNVAN = new GridColumn();
      this.panel4 = new Panel();
      this.chkBildirim = new CheckBox();
      this.btnListe = new SimpleButton();
      this.txtTransferID = new TextBox();
      this.label1 = new Label();
      this.splitterControl1 = new SplitterControl();
      this.panel2 = new Panel();
      this.btnSeriAktar = new SimpleButton();
      this.btnAlimBildirim = new SimpleButton();
      this.panel3 = new Panel();
      this.gridControl1 = new GridControl();
      this.gridView1 = new GridView();
      this.colID1 = new GridColumn();
      this.colKAYNAK_GLNNO1 = new GridColumn();
      this.colDOCUMENT_NUMBER = new GridColumn();
      this.colDOCUMENT_DATE = new GridColumn();
      this.colCARRIER_LABEL = new GridColumn();
      this.colGTIN = new GridColumn();
      this.colSERIAL_NUMBER = new GridColumn();
      this.colLOT_NUMBER = new GridColumn();
      this.colDATE = new GridColumn();
      this.colKOLI_BARKOD = new GridColumn();
      this.colTRANSFER_ID1 = new GridColumn();
      this.colDURUM = new GridColumn();
      this.colSTOK_ISIM = new GridColumn();
      this.btnAlisIrsaliye = new SimpleButton();
      this.panel1.SuspendLayout();
      this.panel5.SuspendLayout();
      this.gridControl2.BeginInit();
      this.gridView3.BeginInit();
      this.panel4.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.gridControl1.BeginInit();
      this.gridView1.BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.panel5);
      this.panel1.Controls.Add((Control) this.panel4);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(836, 196);
      this.panel1.TabIndex = 0;
      this.panel5.Controls.Add((Control) this.gridControl2);
      this.panel5.Dock = DockStyle.Fill;
      this.panel5.Location = new Point(0, 35);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(836, 161);
      this.panel5.TabIndex = 7;
      this.gridControl2.Dock = DockStyle.Fill;
      this.gridControl2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.gridControl2.Location = new Point(0, 0);
      this.gridControl2.MainView = (BaseView) this.gridView3;
      this.gridControl2.Name = "gridControl2";
      this.gridControl2.Size = new Size(836, 161);
      this.gridControl2.TabIndex = 5;
      this.gridControl2.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.gridView3.Columns.AddRange(new GridColumn[5]
      {
        this.colID,
        this.colTARIH,
        this.colTRANSFER_ID,
        this.colKAYNAK_GLNNO,
        this.colCARI_UNVAN
      });
      this.gridView3.GridControl = this.gridControl2;
      this.gridView3.Name = "gridView3";
      this.gridView3.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView3.OptionsBehavior.Editable = false;
      this.gridView3.OptionsView.ShowGroupPanel = false;
      this.gridView3.DoubleClick += new EventHandler(this.gridView3_DoubleClick);
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colTARIH.Caption = "Tarih";
      this.colTARIH.FieldName = "TARIH";
      this.colTARIH.Name = "colTARIH";
      this.colTARIH.Visible = true;
      this.colTARIH.VisibleIndex = 0;
      this.colTARIH.Width = 91;
      this.colTRANSFER_ID.Caption = "Transfer ID";
      this.colTRANSFER_ID.FieldName = "TRANSFER_ID";
      this.colTRANSFER_ID.Name = "colTRANSFER_ID";
      this.colTRANSFER_ID.Visible = true;
      this.colTRANSFER_ID.VisibleIndex = 1;
      this.colTRANSFER_ID.Width = 109;
      this.colKAYNAK_GLNNO.Caption = "GLN no";
      this.colKAYNAK_GLNNO.FieldName = "KAYNAK_GLNNO";
      this.colKAYNAK_GLNNO.Name = "colKAYNAK_GLNNO";
      this.colKAYNAK_GLNNO.Visible = true;
      this.colKAYNAK_GLNNO.VisibleIndex = 2;
      this.colKAYNAK_GLNNO.Width = 134;
      this.colCARI_UNVAN.Caption = "Cari Ünvan";
      this.colCARI_UNVAN.FieldName = "CARI_UNVAN";
      this.colCARI_UNVAN.Name = "colCARI_UNVAN";
      this.colCARI_UNVAN.Visible = true;
      this.colCARI_UNVAN.VisibleIndex = 3;
      this.colCARI_UNVAN.Width = 484;
      this.panel4.Controls.Add((Control) this.chkBildirim);
      this.panel4.Controls.Add((Control) this.btnListe);
      this.panel4.Controls.Add((Control) this.txtTransferID);
      this.panel4.Controls.Add((Control) this.label1);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(836, 35);
      this.panel4.TabIndex = 6;
      this.chkBildirim.AutoSize = true;
      this.chkBildirim.Location = new Point(215, 8);
      this.chkBildirim.Name = "chkBildirim";
      this.chkBildirim.Size = new Size(141, 17);
      this.chkBildirim.TabIndex = 5;
      this.chkBildirim.Text = "Bildirimi yapılanları göster";
      this.chkBildirim.UseVisualStyleBackColor = true;
      this.btnListe.Dock = DockStyle.Right;
      this.btnListe.Location = new Point(741, 0);
      this.btnListe.LookAndFeel.SkinName = "Seven Classic";
      this.btnListe.LookAndFeel.UseDefaultLookAndFeel = false;
      this.btnListe.Name = "btnListe";
      this.btnListe.Size = new Size(95, 35);
      this.btnListe.TabIndex = 4;
      this.btnListe.Text = "Listele";
      this.btnListe.Click += new EventHandler(this.btnListe_Click);
      this.txtTransferID.Location = new Point(78, 6);
      this.txtTransferID.Name = "txtTransferID";
      this.txtTransferID.Size = new Size(100, 20);
      this.txtTransferID.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Transfer ID";
      this.splitterControl1.Dock = DockStyle.Top;
      this.splitterControl1.Location = new Point(0, 196);
      this.splitterControl1.Name = "splitterControl1";
      this.splitterControl1.Size = new Size(836, 6);
      this.splitterControl1.TabIndex = 1;
      this.splitterControl1.TabStop = false;
      this.panel2.Controls.Add((Control) this.btnAlisIrsaliye);
      this.panel2.Controls.Add((Control) this.btnSeriAktar);
      this.panel2.Controls.Add((Control) this.btnAlimBildirim);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 423);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(836, 41);
      this.panel2.TabIndex = 2;
      this.btnSeriAktar.Dock = DockStyle.Left;
      this.btnSeriAktar.Location = new Point(0, 0);
      this.btnSeriAktar.LookAndFeel.SkinName = "Seven Classic";
      this.btnSeriAktar.LookAndFeel.UseDefaultLookAndFeel = false;
      this.btnSeriAktar.Name = "btnSeriAktar";
      this.btnSeriAktar.Size = new Size(95, 41);
      this.btnSeriAktar.TabIndex = 6;
      this.btnSeriAktar.Text = "Seri Aktar";
      this.btnSeriAktar.Click += new EventHandler(this.btnSeriAktar_Click);
      this.btnAlimBildirim.Dock = DockStyle.Right;
      this.btnAlimBildirim.Location = new Point(741, 0);
      this.btnAlimBildirim.LookAndFeel.SkinName = "Seven Classic";
      this.btnAlimBildirim.LookAndFeel.UseDefaultLookAndFeel = false;
      this.btnAlimBildirim.Name = "btnAlimBildirim";
      this.btnAlimBildirim.Size = new Size(95, 41);
      this.btnAlimBildirim.TabIndex = 5;
      this.btnAlimBildirim.Text = "Alım Bildirimi Yap";
      this.btnAlimBildirim.Click += new EventHandler(this.btnAlimBildirim_Click);
      this.panel3.Controls.Add((Control) this.gridControl1);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 202);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(836, 221);
      this.panel3.TabIndex = 3;
      this.gridControl1.Dock = DockStyle.Fill;
      this.gridControl1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.gridControl1.Location = new Point(0, 0);
      this.gridControl1.MainView = (BaseView) this.gridView1;
      this.gridControl1.Name = "gridControl1";
      this.gridControl1.Size = new Size(836, 221);
      this.gridControl1.TabIndex = 5;
      this.gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView1
      });
      this.gridView1.Columns.AddRange(new GridColumn[13]
      {
        this.colID1,
        this.colKAYNAK_GLNNO1,
        this.colDOCUMENT_NUMBER,
        this.colDOCUMENT_DATE,
        this.colCARRIER_LABEL,
        this.colGTIN,
        this.colSERIAL_NUMBER,
        this.colLOT_NUMBER,
        this.colDATE,
        this.colKOLI_BARKOD,
        this.colTRANSFER_ID1,
        this.colDURUM,
        this.colSTOK_ISIM
      });
      this.gridView1.GridControl = this.gridControl1;
      this.gridView1.GroupCount = 1;
      this.gridView1.GroupSummary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Count, "SERIAL_NUMBER", (GridColumn) null, "(Kalem Adet : {0})", (object) "1")
      });
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView1.OptionsBehavior.Editable = false;
      this.gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
      this.gridView1.OptionsView.ShowFooter = true;
      this.gridView1.SortInfo.AddRange(new GridColumnSortInfo[1]
      {
        new GridColumnSortInfo(this.colSTOK_ISIM, ColumnSortOrder.Ascending)
      });
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      this.colID1.FieldName = "ID";
      this.colID1.Name = "colID1";
      this.colKAYNAK_GLNNO1.FieldName = "KAYNAK_GLNNO";
      this.colKAYNAK_GLNNO1.Name = "colKAYNAK_GLNNO1";
      this.colDOCUMENT_NUMBER.FieldName = "DOCUMENT_NUMBER";
      this.colDOCUMENT_NUMBER.Name = "colDOCUMENT_NUMBER";
      this.colDOCUMENT_DATE.FieldName = "DOCUMENT_DATE";
      this.colDOCUMENT_DATE.Name = "colDOCUMENT_DATE";
      this.colCARRIER_LABEL.FieldName = "CARRIER_LABEL";
      this.colCARRIER_LABEL.Name = "colCARRIER_LABEL";
      this.colGTIN.FieldName = "GTIN";
      this.colGTIN.Name = "colGTIN";
      this.colGTIN.Visible = true;
      this.colGTIN.VisibleIndex = 0;
      this.colGTIN.Width = 123;
      this.colSERIAL_NUMBER.FieldName = "SERIAL_NUMBER";
      this.colSERIAL_NUMBER.Name = "colSERIAL_NUMBER";
      this.colSERIAL_NUMBER.Visible = true;
      this.colSERIAL_NUMBER.VisibleIndex = 1;
      this.colSERIAL_NUMBER.Width = 135;
      this.colLOT_NUMBER.FieldName = "LOT_NUMBER";
      this.colLOT_NUMBER.Name = "colLOT_NUMBER";
      this.colLOT_NUMBER.Visible = true;
      this.colLOT_NUMBER.VisibleIndex = 2;
      this.colLOT_NUMBER.Width = 135;
      this.colDATE.FieldName = "DATE";
      this.colDATE.Name = "colDATE";
      this.colDATE.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Count, "DATE", "Toplam ({0})", (object) "1")
      });
      this.colDATE.Visible = true;
      this.colDATE.VisibleIndex = 3;
      this.colDATE.Width = 143;
      this.colKOLI_BARKOD.FieldName = "KOLI_BARKOD";
      this.colKOLI_BARKOD.Name = "colKOLI_BARKOD";
      this.colTRANSFER_ID1.FieldName = "TRANSFER_ID";
      this.colTRANSFER_ID1.Name = "colTRANSFER_ID1";
      this.colDURUM.FieldName = "DURUM";
      this.colDURUM.Name = "colDURUM";
      this.colSTOK_ISIM.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM.Name = "colSTOK_ISIM";
      this.colSTOK_ISIM.Visible = true;
      this.colSTOK_ISIM.VisibleIndex = 1;
      this.colSTOK_ISIM.Width = 282;
      this.btnAlisIrsaliye.Dock = DockStyle.Right;
      this.btnAlisIrsaliye.Location = new Point(646, 0);
      this.btnAlisIrsaliye.LookAndFeel.SkinName = "Seven Classic";
      this.btnAlisIrsaliye.LookAndFeel.UseDefaultLookAndFeel = false;
      this.btnAlisIrsaliye.Name = "btnAlisIrsaliye";
      this.btnAlisIrsaliye.Size = new Size(95, 41);
      this.btnAlisIrsaliye.TabIndex = 7;
      this.btnAlisIrsaliye.Text = "İrsaliye Oluştur";
      this.btnAlisIrsaliye.Click += new EventHandler(this.btnAlisIrsaliye_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(836, 464);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.splitterControl1);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (FrmPaketYonetim);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Paket Yönetimi";
      this.panel1.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.gridControl2.EndInit();
      this.gridView3.EndInit();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.gridControl1.EndInit();
      this.gridView1.EndInit();
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
    }
  }
}
