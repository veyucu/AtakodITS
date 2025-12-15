// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmCariSecim
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
  public class FrmCariSecim : Form
  {
    public string CariKod;
    public string CariGLNNo;
    public string CariUnvan;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Label label1;
    private Panel panel2;
    private Button btnBul;
    private TextBox txtCariUnvan;
    private Label label2;
    private TextBox txtCariKodu;
    private GridControl grdSiparis;
    private GridView gridView1;
    private GridColumn colRECNO;
    private GridColumn colCARI_KOD;
    private GridColumn colCARI_UNVAN;
    private GridColumn gridColumn1;
    private GridColumn colCARI_IL;

    public FrmCariSecim() => this.InitializeComponent();

    private void btnBul_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      string sql = "SELECT ROW_NUMBER()OVER(ORDER BY CARI_KOD) as cari_RECno,CARI_KOD as cari_kod,EMAIL as cari_sicil_no,dbo.TRK(CARI_ISIM) as cari_unvan1,dbo.TRK(CARI_IL) as cari_il FROM TBLCASABIT WHERE EMAIL like '%" + this.txtCariKodu.Text + "%' AND CARI_ISIM like '%" + this.txtCariUnvan.Text + "%'";
      List<FrmCariSecim.CariTablo> list = MyUtils.Firma.Database.SqlQuery<FrmCariSecim.CariTablo>(sql).ToList<FrmCariSecim.CariTablo>();
      Cursor.Current = Cursors.Default;
      this.grdSiparis.DataSource = (object) list;
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      FrmCariSecim.CariTablo row = this.gridView1.GetRow(this.gridView1.FocusedRowHandle) as FrmCariSecim.CariTablo;
      this.CariKod = row.cari_kod;
      this.CariUnvan = row.cari_unvan1;
      this.CariGLNNo = row.cari_sicil_no;
      this.DialogResult = DialogResult.OK;
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
      this.btnBul = new Button();
      this.txtCariUnvan = new TextBox();
      this.label2 = new Label();
      this.txtCariKodu = new TextBox();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.grdSiparis = new GridControl();
      this.gridView1 = new GridView();
      this.colRECNO = new GridColumn();
      this.colCARI_KOD = new GridColumn();
      this.gridColumn1 = new GridColumn();
      this.colCARI_UNVAN = new GridColumn();
      this.colCARI_IL = new GridColumn();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grdSiparis.BeginInit();
      this.gridView1.BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnBul);
      this.panel1.Controls.Add((Control) this.txtCariUnvan);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.txtCariKodu);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(721, 58);
      this.panel1.TabIndex = 0;
      this.btnBul.Location = new Point(528, 23);
      this.btnBul.Name = "btnBul";
      this.btnBul.Size = new Size(75, 20);
      this.btnBul.TabIndex = 4;
      this.btnBul.Text = "BUL";
      this.btnBul.UseVisualStyleBackColor = true;
      this.btnBul.Click += new EventHandler(this.btnBul_Click);
      this.txtCariUnvan.Location = new Point(169, 23);
      this.txtCariUnvan.Name = "txtCariUnvan";
      this.txtCariUnvan.Size = new Size(355, 20);
      this.txtCariUnvan.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(169, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Cari Ünvan";
      this.txtCariKodu.Location = new Point(7, 23);
      this.txtCariKodu.Name = "txtCariKodu";
      this.txtCariKodu.Size = new Size(160, 20);
      this.txtCariKodu.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "GLN No";
      this.panel2.Controls.Add((Control) this.grdSiparis);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 58);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(721, 406);
      this.panel2.TabIndex = 1;
      this.grdSiparis.Dock = DockStyle.Fill;
      this.grdSiparis.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdSiparis.Location = new Point(0, 0);
      this.grdSiparis.MainView = (BaseView) this.gridView1;
      this.grdSiparis.Name = "grdSiparis";
      this.grdSiparis.Size = new Size(721, 406);
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
        this.colRECNO,
        this.colCARI_KOD,
        this.gridColumn1,
        this.colCARI_UNVAN,
        this.colCARI_IL
      });
      this.gridView1.GridControl = this.grdSiparis;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView1.OptionsBehavior.Editable = false;
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      this.colRECNO.FieldName = "cari_RECno";
      this.colRECNO.Name = "colRECNO";
      this.colCARI_KOD.Caption = "Cari Kodu";
      this.colCARI_KOD.FieldName = "cari_kod";
      this.colCARI_KOD.Name = "colCARI_KOD";
      this.colCARI_KOD.OptionsColumn.AllowEdit = false;
      this.colCARI_KOD.Visible = true;
      this.colCARI_KOD.VisibleIndex = 1;
      this.colCARI_KOD.Width = 144;
      this.gridColumn1.Caption = "GLN No";
      this.gridColumn1.FieldName = "cari_sicil_no";
      this.gridColumn1.Name = "gridColumn1";
      this.gridColumn1.OptionsColumn.AllowEdit = false;
      this.gridColumn1.Visible = true;
      this.gridColumn1.VisibleIndex = 0;
      this.gridColumn1.Width = 97;
      this.colCARI_UNVAN.Caption = "Cari Ünvan";
      this.colCARI_UNVAN.FieldName = "cari_unvan1";
      this.colCARI_UNVAN.Name = "colCARI_UNVAN";
      this.colCARI_UNVAN.OptionsColumn.AllowEdit = false;
      this.colCARI_UNVAN.Visible = true;
      this.colCARI_UNVAN.VisibleIndex = 2;
      this.colCARI_UNVAN.Width = 345;
      this.colCARI_IL.Caption = "İl";
      this.colCARI_IL.FieldName = "cari_il";
      this.colCARI_IL.Name = "colCARI_IL";
      this.colCARI_IL.OptionsColumn.AllowEdit = false;
      this.colCARI_IL.Visible = true;
      this.colCARI_IL.VisibleIndex = 3;
      this.colCARI_IL.Width = 117;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(721, 464);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (FrmCariSecim);
      this.Text = "Cari Seçim";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.grdSiparis.EndInit();
      this.gridView1.EndInit();
      this.ResumeLayout(false);
    }

    public class CariTablo
    {
      public long cari_RECno { get; set; }

      public string cari_kod { get; set; }

      public string cari_sicil_no { get; set; }

      public string cari_unvan1 { get; set; }

      public string cari_il { get; set; }
    }
  }
}
