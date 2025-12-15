// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmKalemDetay
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmKalemDetay : Form
  {
    public KalemTipi kalemTipi;
    public IEnumerable<ITSHAR> itsHar;
    public IEnumerable<TBLTRANSFER_DETAY> transFer;
    public string Stokkodu;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private SimpleButton btnKaydet;
    private Panel panel2;
    private GridControl grdITSHAR;
    private GridView gridView1;
    private GridControl grdTRANSFER;
    private GridView gridView2;
    private BindingSource tBLTRANSFERDETAYBindingSource;
    private GridColumn colID1;
    private GridColumn colKAYNAK_GLNNO;
    private GridColumn colDOCUMENT_NUMBER;
    private GridColumn colDOCUMENT_DATE;
    private GridColumn colCARRIER_LABEL;
    private GridColumn colGTIN;
    private GridColumn colSERIAL_NUMBER;
    private GridColumn colLOT_NUMBER;
    private GridColumn colDATE;
    private GridColumn colKOLI_BARKOD;
    private GridColumn colTRANSFER_ID;
    private GridColumn colDURUM1;
    private BindingSource ıTSHARBindingSource;
    private GridColumn colID;
    private GridColumn colTIP;
    private GridColumn colEVRAK_SERI;
    private GridColumn colSTOK_KOD;
    private GridColumn colBARKOD;
    private GridColumn colSERI_NO;
    private GridColumn colMIAD;
    private GridColumn colPARTINO;
    private GridColumn colDURUM;
    private GridColumn colCARI_KOD;
    private SimpleButton btnSil;
    private SimpleButton btnBuyuk;
    private SimpleButton btnTumSil;
    private GridControl grdSERBEST;
    private GridView gridView3;
    private GridColumn gridColumn1;
    private GridColumn gridColumn6;
    private GridColumn gridColumn7;
    private GridColumn gridColumn8;
    private GridColumn gridColumn9;
    private BindingSource ıtsHarClassBindingSource;
    private SimpleButton btnTextGoster;

    public FrmKalemDetay() => this.InitializeComponent();

    public List<ItsHarClass> serbest { get; set; }

    private void FrmKalemDetay_Load(object sender, EventArgs e)
    {
    }

    private void simpleButton1_Click(object sender, EventArgs e)
    {
      MyUtils.Firma.SaveChanges();
      int num = (int) MessageBox.Show("Kayıt güncellenmiştir...");
    }

    private void FrmKalemDetay_Shown(object sender, EventArgs e)
    {
      if (this.kalemTipi == KalemTipi.ITS)
      {
        this.grdITSHAR.DataSource = (object) this.itsHar;
        this.grdITSHAR.Dock = DockStyle.Fill;
        this.grdITSHAR.Visible = true;
      }
      else if (this.kalemTipi == KalemTipi.TRANSFER)
      {
        this.grdTRANSFER.DataSource = (object) this.transFer;
        this.grdTRANSFER.Dock = DockStyle.Fill;
        this.grdTRANSFER.Visible = true;
      }
      else
      {
        this.grdSERBEST.DataSource = (object) this.serbest.Where<ItsHarClass>((Func<ItsHarClass, bool>) (u => u.STOK_KOD == this.Stokkodu)).ToList<ItsHarClass>();
        this.grdSERBEST.Dock = DockStyle.Fill;
        this.grdSERBEST.Visible = true;
      }
    }

    private void btnSil_Click(object sender, EventArgs e)
    {
      if (this.kalemTipi == KalemTipi.ITS)
      {
        if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        try
        {
          int focusedRowHandle = this.gridView1.FocusedRowHandle;
          ITSHAR row = this.gridView1.GetRow(focusedRowHandle) as ITSHAR;
          MyUtils.Firma.ITSHAR.Remove(row);
          MyUtils.Firma.SaveChanges();
          this.gridView1.DeleteRow(focusedRowHandle);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
        }
      }
      else if (this.kalemTipi == KalemTipi.TRANSFER)
      {
        if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          try
          {
            int focusedRowHandle = this.gridView2.FocusedRowHandle;
            TBLTRANSFER_DETAY row = this.gridView2.GetRow(focusedRowHandle) as TBLTRANSFER_DETAY;
            MyUtils.Firma.TBLTRANSFER_DETAY.Remove(row);
            MyUtils.Firma.SaveChanges();
            this.gridView2.DeleteRow(focusedRowHandle);
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
          }
        }
      }
      else if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
      {
        try
        {
          int focusedRowHandle = this.gridView3.FocusedRowHandle;
          this.serbest.Remove(this.gridView3.GetRow(focusedRowHandle) as ItsHarClass);
          this.gridView3.DeleteRow(focusedRowHandle);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
        }
      }
    }

    private void btnBuyuk_Click(object sender, EventArgs e)
    {
      if (this.kalemTipi == KalemTipi.ITS)
      {
        foreach (ITSHAR itshar in this.itsHar)
        {
          itshar.PARTINO = itshar.PARTINO.ToUpper(new CultureInfo("en-us"));
          itshar.SERI_NO = itshar.SERI_NO.ToUpper(new CultureInfo("en-us"));
        }
        MyUtils.Firma.SaveChanges();
        this.grdITSHAR.DataSource = (object) this.itsHar;
        this.Close();
      }
      else if (this.kalemTipi == KalemTipi.TRANSFER)
      {
        foreach (TBLTRANSFER_DETAY tbltransferDetay in this.transFer)
        {
          tbltransferDetay.LOT_NUMBER = tbltransferDetay.LOT_NUMBER.ToUpper(new CultureInfo("en-us"));
          tbltransferDetay.SERIAL_NUMBER = tbltransferDetay.SERIAL_NUMBER.ToUpper(new CultureInfo("en-us"));
        }
        MyUtils.Firma.SaveChanges();
        this.grdTRANSFER.DataSource = (object) this.transFer;
        this.Close();
      }
      else
      {
        foreach (ItsHarClass itsHarClass in this.serbest)
        {
          itsHarClass.PARTINO = itsHarClass.PARTINO.ToUpper(new CultureInfo("en-us"));
          itsHarClass.SERI_NO = itsHarClass.SERI_NO.ToUpper(new CultureInfo("en-us"));
        }
        this.grdSERBEST.DataSource = (object) this.serbest;
        this.Close();
      }
    }

    private void btnTumSil_Click(object sender, EventArgs e)
    {
      if (this.kalemTipi == KalemTipi.ITS)
      {
        if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        try
        {
          while (this.gridView1.RowCount > 0)
          {
            ITSHAR row = this.gridView1.GetRow(0) as ITSHAR;
            MyUtils.Firma.ITSHAR.Remove(row);
            MyUtils.Firma.SaveChanges();
            this.gridView1.DeleteRow(0);
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
        }
      }
      else if (this.kalemTipi == KalemTipi.TRANSFER)
      {
        if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          try
          {
            while (this.gridView2.RowCount > 0)
            {
              TBLTRANSFER_DETAY row = this.gridView2.GetRow(0) as TBLTRANSFER_DETAY;
              MyUtils.Firma.TBLTRANSFER_DETAY.Remove(row);
              MyUtils.Firma.SaveChanges();
              this.gridView2.DeleteRow(0);
            }
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
          }
        }
      }
      else if (MessageBox.Show("Silmek istediğinize eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
      {
        try
        {
          while (this.gridView3.RowCount > 0)
          {
            this.serbest.Remove(this.gridView3.GetRow(0) as ItsHarClass);
            this.gridView3.DeleteRow(0);
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Hata Oluştu :" + ex.Message);
        }
      }
    }

    private void gridView1_Click(object sender, EventArgs e)
    {
    }

    private void btnTextGoster_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.kalemTipi == KalemTipi.ITS)
      {
        foreach (ITSHAR itshar in this.itsHar)
        {
          string str = itshar.BARKOD.Remove(0, 1);
          stringBuilder.AppendLine("010" + str + "21" + itshar.SERI_NO + "17" + itshar.MIAD + "10" + itshar.PARTINO);
        }
      }
      else if (this.kalemTipi == KalemTipi.TRANSFER)
      {
        foreach (TBLTRANSFER_DETAY tbltransferDetay in this.transFer)
          stringBuilder.AppendLine("010" + tbltransferDetay.GTIN + "21" + tbltransferDetay.SERIAL_NUMBER + "17" + tbltransferDetay.DATE + "10" + tbltransferDetay.LOT_NUMBER);
      }
      else
      {
        foreach (ItsHarClass itsHarClass in this.serbest)
        {
          string str = itsHarClass.BARKOD.Remove(0, 1);
          stringBuilder.AppendLine("010" + str + "21" + itsHarClass.SERI_NO + "17" + itsHarClass.MIAD + "10" + itsHarClass.PARTINO);
        }
      }
      FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
      frmKoliSonuc.Sonuc = stringBuilder.ToString();
      int num = (int) frmKoliSonuc.ShowDialog();
      frmKoliSonuc.Dispose();
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
      this.panel1 = new Panel();
      this.btnTextGoster = new SimpleButton();
      this.btnTumSil = new SimpleButton();
      this.btnBuyuk = new SimpleButton();
      this.btnSil = new SimpleButton();
      this.btnKaydet = new SimpleButton();
      this.panel2 = new Panel();
      this.grdSERBEST = new GridControl();
      this.ıtsHarClassBindingSource = new BindingSource(this.components);
      this.gridView3 = new GridView();
      this.gridColumn1 = new GridColumn();
      this.gridColumn6 = new GridColumn();
      this.gridColumn7 = new GridColumn();
      this.gridColumn8 = new GridColumn();
      this.gridColumn9 = new GridColumn();
      this.grdTRANSFER = new GridControl();
      this.tBLTRANSFERDETAYBindingSource = new BindingSource(this.components);
      this.gridView2 = new GridView();
      this.colID1 = new GridColumn();
      this.colKAYNAK_GLNNO = new GridColumn();
      this.colDOCUMENT_NUMBER = new GridColumn();
      this.colDOCUMENT_DATE = new GridColumn();
      this.colCARRIER_LABEL = new GridColumn();
      this.colGTIN = new GridColumn();
      this.colSERIAL_NUMBER = new GridColumn();
      this.colLOT_NUMBER = new GridColumn();
      this.colDATE = new GridColumn();
      this.colKOLI_BARKOD = new GridColumn();
      this.colTRANSFER_ID = new GridColumn();
      this.colDURUM1 = new GridColumn();
      this.grdITSHAR = new GridControl();
      this.ıTSHARBindingSource = new BindingSource(this.components);
      this.gridView1 = new GridView();
      this.colID = new GridColumn();
      this.colTIP = new GridColumn();
      this.colEVRAK_SERI = new GridColumn();
      this.colSTOK_KOD = new GridColumn();
      this.colBARKOD = new GridColumn();
      this.colSERI_NO = new GridColumn();
      this.colMIAD = new GridColumn();
      this.colPARTINO = new GridColumn();
      this.colDURUM = new GridColumn();
      this.colCARI_KOD = new GridColumn();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grdSERBEST.BeginInit();
      ((ISupportInitialize) this.ıtsHarClassBindingSource).BeginInit();
      this.gridView3.BeginInit();
      this.grdTRANSFER.BeginInit();
      ((ISupportInitialize) this.tBLTRANSFERDETAYBindingSource).BeginInit();
      this.gridView2.BeginInit();
      this.grdITSHAR.BeginInit();
      ((ISupportInitialize) this.ıTSHARBindingSource).BeginInit();
      this.gridView1.BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnTextGoster);
      this.panel1.Controls.Add((Control) this.btnTumSil);
      this.panel1.Controls.Add((Control) this.btnBuyuk);
      this.panel1.Controls.Add((Control) this.btnSil);
      this.panel1.Controls.Add((Control) this.btnKaydet);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 376);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(642, 35);
      this.panel1.TabIndex = 1;
      this.btnTextGoster.ButtonStyle = BorderStyles.Office2003;
      this.btnTextGoster.Dock = DockStyle.Right;
      this.btnTextGoster.Location = new Point(271, 0);
      this.btnTextGoster.Name = "btnTextGoster";
      this.btnTextGoster.Size = new Size(101, 35);
      this.btnTextGoster.TabIndex = 4;
      this.btnTextGoster.Text = "Text Göster";
      this.btnTextGoster.Click += new EventHandler(this.btnTextGoster_Click);
      this.btnTumSil.ButtonStyle = BorderStyles.Office2003;
      this.btnTumSil.Dock = DockStyle.Left;
      this.btnTumSil.Location = new Point(112, 0);
      this.btnTumSil.Name = "btnTumSil";
      this.btnTumSil.Size = new Size(112, 35);
      this.btnTumSil.TabIndex = 3;
      this.btnTumSil.Text = "Tümünü Sil";
      this.btnTumSil.Click += new EventHandler(this.btnTumSil_Click);
      this.btnBuyuk.ButtonStyle = BorderStyles.Office2003;
      this.btnBuyuk.Dock = DockStyle.Right;
      this.btnBuyuk.Location = new Point(372, 0);
      this.btnBuyuk.Name = "btnBuyuk";
      this.btnBuyuk.Size = new Size(158, 35);
      this.btnBuyuk.TabIndex = 2;
      this.btnBuyuk.Text = "Büyük / Küçük Harf Kontrol";
      this.btnBuyuk.Click += new EventHandler(this.btnBuyuk_Click);
      this.btnSil.ButtonStyle = BorderStyles.Office2003;
      this.btnSil.Dock = DockStyle.Left;
      this.btnSil.Location = new Point(0, 0);
      this.btnSil.Name = "btnSil";
      this.btnSil.Size = new Size(112, 35);
      this.btnSil.TabIndex = 1;
      this.btnSil.Text = "Seçili Kalemi Sil";
      this.btnSil.Click += new EventHandler(this.btnSil_Click);
      this.btnKaydet.ButtonStyle = BorderStyles.Office2003;
      this.btnKaydet.Dock = DockStyle.Right;
      this.btnKaydet.Location = new Point(530, 0);
      this.btnKaydet.Name = "btnKaydet";
      this.btnKaydet.Size = new Size(112, 35);
      this.btnKaydet.TabIndex = 0;
      this.btnKaydet.Text = "Kaydet";
      this.btnKaydet.Click += new EventHandler(this.simpleButton1_Click);
      this.panel2.Controls.Add((Control) this.grdSERBEST);
      this.panel2.Controls.Add((Control) this.grdTRANSFER);
      this.panel2.Controls.Add((Control) this.grdITSHAR);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(642, 376);
      this.panel2.TabIndex = 2;
      this.grdSERBEST.DataSource = (object) this.ıtsHarClassBindingSource;
      this.grdSERBEST.Location = new Point(75, 132);
      this.grdSERBEST.MainView = (BaseView) this.gridView3;
      this.grdSERBEST.Name = "grdSERBEST";
      this.grdSERBEST.Size = new Size(400, 200);
      this.grdSERBEST.TabIndex = 3;
      this.grdSERBEST.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.grdSERBEST.Visible = false;
      this.ıtsHarClassBindingSource.DataSource = (object) typeof (ItsHarClass);
      this.gridView3.Columns.AddRange(new GridColumn[5]
      {
        this.gridColumn1,
        this.gridColumn6,
        this.gridColumn7,
        this.gridColumn8,
        this.gridColumn9
      });
      this.gridView3.GridControl = this.grdSERBEST;
      this.gridView3.Name = "gridView3";
      this.gridView3.OptionsView.ShowGroupPanel = false;
      this.gridColumn1.FieldName = "ID";
      this.gridColumn1.Name = "gridColumn1";
      this.gridColumn6.FieldName = "BARKOD";
      this.gridColumn6.Name = "gridColumn6";
      this.gridColumn6.OptionsColumn.AllowEdit = false;
      this.gridColumn6.Visible = true;
      this.gridColumn6.VisibleIndex = 0;
      this.gridColumn7.FieldName = "SERI_NO";
      this.gridColumn7.Name = "gridColumn7";
      this.gridColumn7.Visible = true;
      this.gridColumn7.VisibleIndex = 1;
      this.gridColumn8.FieldName = "PARTINO";
      this.gridColumn8.Name = "gridColumn8";
      this.gridColumn8.Visible = true;
      this.gridColumn8.VisibleIndex = 2;
      this.gridColumn9.FieldName = "MIAD";
      this.gridColumn9.Name = "gridColumn9";
      this.gridColumn9.Visible = true;
      this.gridColumn9.VisibleIndex = 3;
      this.grdTRANSFER.DataSource = (object) this.tBLTRANSFERDETAYBindingSource;
      this.grdTRANSFER.Location = new Point(242, 176);
      this.grdTRANSFER.MainView = (BaseView) this.gridView2;
      this.grdTRANSFER.Name = "grdTRANSFER";
      this.grdTRANSFER.Size = new Size(400, 200);
      this.grdTRANSFER.TabIndex = 2;
      this.grdTRANSFER.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView2
      });
      this.grdTRANSFER.Visible = false;
      this.tBLTRANSFERDETAYBindingSource.DataSource = (object) typeof (TBLTRANSFER_DETAY);
      this.gridView2.Columns.AddRange(new GridColumn[12]
      {
        this.colID1,
        this.colKAYNAK_GLNNO,
        this.colDOCUMENT_NUMBER,
        this.colDOCUMENT_DATE,
        this.colCARRIER_LABEL,
        this.colGTIN,
        this.colSERIAL_NUMBER,
        this.colLOT_NUMBER,
        this.colDATE,
        this.colKOLI_BARKOD,
        this.colTRANSFER_ID,
        this.colDURUM1
      });
      this.gridView2.GridControl = this.grdTRANSFER;
      this.gridView2.Name = "gridView2";
      this.gridView2.OptionsView.ShowGroupPanel = false;
      this.colID1.FieldName = "ID";
      this.colID1.Name = "colID1";
      this.colKAYNAK_GLNNO.FieldName = "KAYNAK_GLNNO";
      this.colKAYNAK_GLNNO.Name = "colKAYNAK_GLNNO";
      this.colDOCUMENT_NUMBER.FieldName = "DOCUMENT_NUMBER";
      this.colDOCUMENT_NUMBER.Name = "colDOCUMENT_NUMBER";
      this.colDOCUMENT_DATE.FieldName = "DOCUMENT_DATE";
      this.colDOCUMENT_DATE.Name = "colDOCUMENT_DATE";
      this.colCARRIER_LABEL.FieldName = "CARRIER_LABEL";
      this.colCARRIER_LABEL.Name = "colCARRIER_LABEL";
      this.colGTIN.FieldName = "GTIN";
      this.colGTIN.Name = "colGTIN";
      this.colGTIN.OptionsColumn.AllowEdit = false;
      this.colGTIN.Visible = true;
      this.colGTIN.VisibleIndex = 0;
      this.colSERIAL_NUMBER.FieldName = "SERIAL_NUMBER";
      this.colSERIAL_NUMBER.Name = "colSERIAL_NUMBER";
      this.colSERIAL_NUMBER.Visible = true;
      this.colSERIAL_NUMBER.VisibleIndex = 1;
      this.colLOT_NUMBER.FieldName = "LOT_NUMBER";
      this.colLOT_NUMBER.Name = "colLOT_NUMBER";
      this.colLOT_NUMBER.Visible = true;
      this.colLOT_NUMBER.VisibleIndex = 2;
      this.colDATE.FieldName = "DATE";
      this.colDATE.Name = "colDATE";
      this.colDATE.Visible = true;
      this.colDATE.VisibleIndex = 3;
      this.colKOLI_BARKOD.FieldName = "KOLI_BARKOD";
      this.colKOLI_BARKOD.Name = "colKOLI_BARKOD";
      this.colTRANSFER_ID.FieldName = "TRANSFER_ID";
      this.colTRANSFER_ID.Name = "colTRANSFER_ID";
      this.colDURUM1.FieldName = "DURUM";
      this.colDURUM1.Name = "colDURUM1";
      this.grdITSHAR.DataSource = (object) this.ıTSHARBindingSource;
      this.grdITSHAR.Location = new Point(12, 12);
      this.grdITSHAR.MainView = (BaseView) this.gridView1;
      this.grdITSHAR.Name = "grdITSHAR";
      this.grdITSHAR.Size = new Size(400, 200);
      this.grdITSHAR.TabIndex = 1;
      this.grdITSHAR.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView1
      });
      this.grdITSHAR.Visible = false;
      this.ıTSHARBindingSource.DataSource = (object) typeof (ITSHAR);
      this.gridView1.Columns.AddRange(new GridColumn[10]
      {
        this.colID,
        this.colTIP,
        this.colEVRAK_SERI,
        this.colSTOK_KOD,
        this.colBARKOD,
        this.colSERI_NO,
        this.colMIAD,
        this.colPARTINO,
        this.colDURUM,
        this.colCARI_KOD
      });
      this.gridView1.GridControl = this.grdITSHAR;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.gridView1.Click += new EventHandler(this.gridView1_Click);
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colTIP.FieldName = "TIP";
      this.colTIP.Name = "colTIP";
      this.colEVRAK_SERI.FieldName = "EVRAK_SERI";
      this.colEVRAK_SERI.Name = "colEVRAK_SERI";
      this.colSTOK_KOD.FieldName = "STOK_KOD";
      this.colSTOK_KOD.Name = "colSTOK_KOD";
      this.colBARKOD.FieldName = "BARKOD";
      this.colBARKOD.Name = "colBARKOD";
      this.colBARKOD.OptionsColumn.AllowEdit = false;
      this.colBARKOD.Visible = true;
      this.colBARKOD.VisibleIndex = 0;
      this.colSERI_NO.FieldName = "SERI_NO";
      this.colSERI_NO.Name = "colSERI_NO";
      this.colSERI_NO.Visible = true;
      this.colSERI_NO.VisibleIndex = 1;
      this.colMIAD.FieldName = "MIAD";
      this.colMIAD.Name = "colMIAD";
      this.colMIAD.Visible = true;
      this.colMIAD.VisibleIndex = 2;
      this.colPARTINO.FieldName = "PARTINO";
      this.colPARTINO.Name = "colPARTINO";
      this.colPARTINO.Visible = true;
      this.colPARTINO.VisibleIndex = 3;
      this.colDURUM.FieldName = "DURUM";
      this.colDURUM.Name = "colDURUM";
      this.colCARI_KOD.FieldName = "CARI_KOD";
      this.colCARI_KOD.Name = "colCARI_KOD";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(642, 411);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (FrmKalemDetay);
      this.Text = "Kalem Detayı";
      this.Load += new EventHandler(this.FrmKalemDetay_Load);
      this.Shown += new EventHandler(this.FrmKalemDetay_Shown);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.grdSERBEST.EndInit();
      ((ISupportInitialize) this.ıtsHarClassBindingSource).EndInit();
      this.gridView3.EndInit();
      this.grdTRANSFER.EndInit();
      ((ISupportInitialize) this.tBLTRANSFERDETAYBindingSource).EndInit();
      this.gridView2.EndInit();
      this.grdITSHAR.EndInit();
      ((ISupportInitialize) this.ıTSHARBindingSource).EndInit();
      this.gridView1.EndInit();
      this.ResumeLayout(false);
    }
  }
}
