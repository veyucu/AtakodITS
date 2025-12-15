// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmExportSettings
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmExportSettings : Form
  {
    public string Barkod;
    public string SeriNo;
    public string Miad;
    public string PartiNo;
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox txtBarkod;
    private TextBox txtSeriNo;
    private Label label2;
    private TextBox txtMiad;
    private Label label3;
    private TextBox txtPartiNo;
    private Label label4;
    private SimpleButton btnTamam;

    public FrmExportSettings() => this.InitializeComponent();

    private void btnTamam_Click(object sender, EventArgs e)
    {
      this.Barkod = this.txtBarkod.Text;
      this.SeriNo = this.txtSeriNo.Text;
      this.Miad = this.txtMiad.Text;
      this.PartiNo = this.txtPartiNo.Text;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtBarkod = new TextBox();
      this.txtSeriNo = new TextBox();
      this.label2 = new Label();
      this.txtMiad = new TextBox();
      this.label3 = new Label();
      this.txtPartiNo = new TextBox();
      this.label4 = new Label();
      this.btnTamam = new SimpleButton();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(90, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Barkod Başlangıç";
      this.txtBarkod.Location = new Point(116, 8);
      this.txtBarkod.Name = "txtBarkod";
      this.txtBarkod.Size = new Size(75, 20);
      this.txtBarkod.TabIndex = 1;
      this.txtBarkod.Text = "010";
      this.txtSeriNo.Location = new Point(116, 34);
      this.txtSeriNo.Name = "txtSeriNo";
      this.txtSeriNo.Size = new Size(75, 20);
      this.txtSeriNo.TabIndex = 3;
      this.txtSeriNo.Text = "21";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(91, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Seri No Başlangıç";
      this.txtMiad.Location = new Point(116, 60);
      this.txtMiad.Name = "txtMiad";
      this.txtMiad.Size = new Size(75, 20);
      this.txtMiad.TabIndex = 5;
      this.txtMiad.Text = "17";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 61);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Miad Başlangıç";
      this.txtPartiNo.Location = new Point(116, 86);
      this.txtPartiNo.Name = "txtPartiNo";
      this.txtPartiNo.Size = new Size(75, 20);
      this.txtPartiNo.TabIndex = 7;
      this.txtPartiNo.Text = "10";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 87);
      this.label4.Name = "label4";
      this.label4.Size = new Size(94, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Parti No Başlangıç";
      this.btnTamam.Location = new Point(75, 126);
      this.btnTamam.Name = "btnTamam";
      this.btnTamam.Size = new Size(116, 28);
      this.btnTamam.TabIndex = 8;
      this.btnTamam.Text = "Tamam";
      this.btnTamam.Click += new EventHandler(this.btnTamam_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(210, 166);
      this.Controls.Add((Control) this.btnTamam);
      this.Controls.Add((Control) this.txtPartiNo);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtMiad);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtSeriNo);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtBarkod);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmExportSettings);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Ayıraç Ayarları";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
