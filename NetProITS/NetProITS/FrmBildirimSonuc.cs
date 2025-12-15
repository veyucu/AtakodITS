// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmBildirimSonuc
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmBildirimSonuc : Form
  {
    public List<PTSHAR_VIEW> BildirimSonuc;
    public string Sonuc = (string) null;
    private IContainer components = (IContainer) null;
    private PanelControl panelControl6;
    private MemoEdit memSonuc;
    private PanelControl panelControl7;
    private GridControl gridControl2;
    private GridView gridView3;
    private GridColumn colID;
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
    private GridColumn colDURUM;
    private GridColumn colSTOK_ISIM;
    private GridColumn colMESAJ;

    public FrmBildirimSonuc() => this.InitializeComponent();

    private void FrmBildirimSonuc_Shown(object sender, EventArgs e)
    {
      this.gridControl2.DataSource = (object) this.BildirimSonuc;
      if (string.IsNullOrEmpty(this.Sonuc))
        return;
      this.memSonuc.Text = this.Sonuc;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelControl6 = new PanelControl();
      this.memSonuc = new MemoEdit();
      this.panelControl7 = new PanelControl();
      this.gridControl2 = new GridControl();
      this.gridView3 = new GridView();
      this.colID = new GridColumn();
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
      this.colDURUM = new GridColumn();
      this.colSTOK_ISIM = new GridColumn();
      this.colMESAJ = new GridColumn();
      this.panelControl6.BeginInit();
      this.panelControl6.SuspendLayout();
      this.memSonuc.Properties.BeginInit();
      this.panelControl7.BeginInit();
      this.panelControl7.SuspendLayout();
      this.gridControl2.BeginInit();
      this.gridView3.BeginInit();
      this.SuspendLayout();
      this.panelControl6.Controls.Add((Control) this.memSonuc);
      this.panelControl6.Dock = DockStyle.Bottom;
      this.panelControl6.Location = new Point(0, 326);
      this.panelControl6.Name = "panelControl6";
      this.panelControl6.Size = new Size(919, 155);
      this.panelControl6.TabIndex = 4;
      this.memSonuc.Dock = DockStyle.Fill;
      this.memSonuc.Location = new Point(2, 2);
      this.memSonuc.Name = "memSonuc";
      this.memSonuc.Size = new Size(915, 151);
      this.memSonuc.TabIndex = 0;
      this.panelControl7.Controls.Add((Control) this.gridControl2);
      this.panelControl7.Dock = DockStyle.Fill;
      this.panelControl7.Location = new Point(0, 0);
      this.panelControl7.Name = "panelControl7";
      this.panelControl7.Size = new Size(919, 326);
      this.panelControl7.TabIndex = 5;
      this.gridControl2.Dock = DockStyle.Fill;
      this.gridControl2.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.gridControl2.Location = new Point(2, 2);
      this.gridControl2.MainView = (BaseView) this.gridView3;
      this.gridControl2.Name = "gridControl2";
      this.gridControl2.Size = new Size(915, 322);
      this.gridControl2.TabIndex = 3;
      this.gridControl2.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.gridView3.Columns.AddRange(new GridColumn[14]
      {
        this.colID,
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
        this.colDURUM,
        this.colSTOK_ISIM,
        this.colMESAJ
      });
      this.gridView3.GridControl = this.gridControl2;
      this.gridView3.Name = "gridView3";
      this.gridView3.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView3.OptionsBehavior.Editable = false;
      this.gridView3.OptionsView.ShowGroupPanel = false;
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colID.Visible = true;
      this.colID.VisibleIndex = 0;
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
      this.colGTIN.Visible = true;
      this.colGTIN.VisibleIndex = 1;
      this.colSERIAL_NUMBER.FieldName = "SERIAL_NUMBER";
      this.colSERIAL_NUMBER.Name = "colSERIAL_NUMBER";
      this.colSERIAL_NUMBER.Visible = true;
      this.colSERIAL_NUMBER.VisibleIndex = 2;
      this.colLOT_NUMBER.FieldName = "LOT_NUMBER";
      this.colLOT_NUMBER.Name = "colLOT_NUMBER";
      this.colLOT_NUMBER.Visible = true;
      this.colLOT_NUMBER.VisibleIndex = 3;
      this.colDATE.FieldName = "DATE";
      this.colDATE.Name = "colDATE";
      this.colDATE.Visible = true;
      this.colDATE.VisibleIndex = 4;
      this.colKOLI_BARKOD.FieldName = "KOLI_BARKOD";
      this.colKOLI_BARKOD.Name = "colKOLI_BARKOD";
      this.colTRANSFER_ID.FieldName = "TRANSFER_ID";
      this.colTRANSFER_ID.Name = "colTRANSFER_ID";
      this.colDURUM.FieldName = "DURUM";
      this.colDURUM.Name = "colDURUM";
      this.colSTOK_ISIM.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM.Name = "colSTOK_ISIM";
      this.colSTOK_ISIM.Visible = true;
      this.colSTOK_ISIM.VisibleIndex = 5;
      this.colMESAJ.FieldName = "MESAJ";
      this.colMESAJ.Name = "colMESAJ";
      this.colMESAJ.Visible = true;
      this.colMESAJ.VisibleIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(919, 481);
      this.Controls.Add((Control) this.panelControl7);
      this.Controls.Add((Control) this.panelControl6);
      this.Name = nameof (FrmBildirimSonuc);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bildirim Sonuç";
      this.Shown += new EventHandler(this.FrmBildirimSonuc_Shown);
      this.panelControl6.EndInit();
      this.panelControl6.ResumeLayout(false);
      this.memSonuc.Properties.EndInit();
      this.panelControl7.EndInit();
      this.panelControl7.ResumeLayout(false);
      this.gridControl2.EndInit();
      this.gridView3.EndInit();
      this.ResumeLayout(false);
    }
  }
}
