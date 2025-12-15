// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmFoy
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmFoy : Form
  {
    public string GTIN;
    public string SERIAL_NUMBER;
    public string LOT_NUMBER;
    public string DATE;
    private IContainer components = (IContainer) null;
    private GridControl grdSiparis;
    private GridView gridView1;
    private GridColumn colTIP;
    private GridColumn colSTOK_KOD;
    private GridColumn colSTOK_ADI;
    private GridColumn colEVRAK_SERI;
    private GridColumn colCARI_ISIM;

    public FrmFoy() => this.InitializeComponent();

    public void FoyGetir()
    {
      string sql = "SELECT 'PTS ALIS' AS TIP,STOK_KODU,STOK_ADI,DOCUMENT_NUMBER AS EVRAK_SERI,(SELECT TOP 1 CARI_ISIM FROM TBLCASABIT WHERE EMAIL=KAYNAK_GLNNO)AS CARI_ISIM FROM TBLTRANSFER_DETAY T INNER JOIN TBLSTSABIT S ON T.GTIN='0'+S.STOK_KODU WHERE GTIN='" + this.GTIN + "' AND SERIAL_NUMBER='" + this.SERIAL_NUMBER + "' AND LOT_NUMBER='" + this.LOT_NUMBER + "'" + " UNION ALL SELECT CASE TIP WHEN 1 THEN 'SATIŞ' WHEN 2 THEN 'ALIŞ' WHEN 3 THEN 'SATIŞ İPTAL' WHEN 4 THEN 'ALIŞ İPTAL' WHEN 5 THEN 'SIPARIS' END AS TIP,STOK_KOD,STOK_ADI,EVRAK_SERI,CARI_ISIM FROM ITSHAR I INNER JOIN TBLSTSABIT S ON I.STOK_KOD=S.STOK_KODU INNER JOIN TBLCASABIT C ON I.CARI_KOD=C.CARI_KOD WHERE BARKOD='" + this.GTIN + "' AND SERI_NO='" + this.SERIAL_NUMBER + "' AND PARTINO='" + this.LOT_NUMBER + "' AND MIAD='" + this.DATE + "'";
      List<FrmFoy.Hareket> list = MyUtils.Firma.Database.SqlQuery<FrmFoy.Hareket>(sql).ToList<FrmFoy.Hareket>();
      this.grdSiparis.DataSource = (object) null;
      this.grdSiparis.DataSource = (object) list;
    }

    private void FrmFoy_Load(object sender, EventArgs e) => this.FoyGetir();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grdSiparis = new GridControl();
      this.gridView1 = new GridView();
      this.colTIP = new GridColumn();
      this.colSTOK_KOD = new GridColumn();
      this.colSTOK_ADI = new GridColumn();
      this.colEVRAK_SERI = new GridColumn();
      this.colCARI_ISIM = new GridColumn();
      this.grdSiparis.BeginInit();
      this.gridView1.BeginInit();
      this.SuspendLayout();
      this.grdSiparis.Dock = DockStyle.Fill;
      this.grdSiparis.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdSiparis.Location = new Point(0, 0);
      this.grdSiparis.MainView = (BaseView) this.gridView1;
      this.grdSiparis.Name = "grdSiparis";
      this.grdSiparis.Size = new Size(1184, 419);
      this.grdSiparis.TabIndex = 2;
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
      this.gridView1.Columns.AddRange(new GridColumn[5]
      {
        this.colTIP,
        this.colSTOK_KOD,
        this.colSTOK_ADI,
        this.colEVRAK_SERI,
        this.colCARI_ISIM
      });
      this.gridView1.GridControl = this.grdSiparis;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView1.OptionsBehavior.Editable = false;
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.colTIP.Caption = "Tipi";
      this.colTIP.FieldName = "TIP";
      this.colTIP.Name = "colTIP";
      this.colTIP.OptionsColumn.AllowEdit = false;
      this.colTIP.Visible = true;
      this.colTIP.VisibleIndex = 0;
      this.colTIP.Width = 130;
      this.colSTOK_KOD.Caption = "Stok Kodu";
      this.colSTOK_KOD.FieldName = "STOK_KOD";
      this.colSTOK_KOD.Name = "colSTOK_KOD";
      this.colSTOK_KOD.OptionsColumn.AllowEdit = false;
      this.colSTOK_KOD.Visible = true;
      this.colSTOK_KOD.VisibleIndex = 1;
      this.colSTOK_KOD.Width = 161;
      this.colSTOK_ADI.Caption = "Stok Adı";
      this.colSTOK_ADI.FieldName = "STOK_ADI";
      this.colSTOK_ADI.Name = "colSTOK_ADI";
      this.colSTOK_ADI.OptionsColumn.AllowEdit = false;
      this.colSTOK_ADI.Visible = true;
      this.colSTOK_ADI.VisibleIndex = 2;
      this.colSTOK_ADI.Width = 380;
      this.colEVRAK_SERI.Caption = "Fiş No";
      this.colEVRAK_SERI.FieldName = "EVRAK_SERI";
      this.colEVRAK_SERI.Name = "colEVRAK_SERI";
      this.colEVRAK_SERI.OptionsColumn.AllowEdit = false;
      this.colEVRAK_SERI.Visible = true;
      this.colEVRAK_SERI.VisibleIndex = 3;
      this.colEVRAK_SERI.Width = 160;
      this.colCARI_ISIM.Caption = "Cari isim";
      this.colCARI_ISIM.FieldName = "CARI_ISIM";
      this.colCARI_ISIM.Name = "colCARI_ISIM";
      this.colCARI_ISIM.OptionsColumn.AllowEdit = false;
      this.colCARI_ISIM.Visible = true;
      this.colCARI_ISIM.VisibleIndex = 4;
      this.colCARI_ISIM.Width = 335;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1184, 419);
      this.Controls.Add((Control) this.grdSiparis);
      this.Name = nameof (FrmFoy);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Karekod Föy";
      this.Load += new EventHandler(this.FrmFoy_Load);
      this.grdSiparis.EndInit();
      this.gridView1.EndInit();
      this.ResumeLayout(false);
    }

    public class Hareket
    {
      public string TIP { get; set; }

      public string STOK_KOD { get; set; }

      public string STOK_ADI { get; set; }

      public string EVRAK_SERI { get; set; }

      public string CARI_ISIM { get; set; }
    }
  }
}
