// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmYaziciSec
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmYaziciSec : Form
  {
    public int YaziciIndex = -1;
    public int Tip;
    private IContainer components = (IContainer) null;
    private System.Windows.Forms.ComboBox cbYazici;
    private SimpleButton btnTamam;

    public FrmYaziciSec() => this.InitializeComponent();

    private void FrmYaziciSec_Load(object sender, EventArgs e)
    {
      this.cbYazici.Items.Clear();
      foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        this.cbYazici.Items.Add((object) installedPrinter);
      switch (this.Tip)
      {
        case 1:
          this.Text = "Resmi Fatura Yazıcısı";
          break;
        case 2:
          this.Text = "Sipariş Yazıcısı";
          break;
        case 3:
          this.Text = "Gayri Fatura Yazıcısı";
          break;
        case 4:
          this.Text = "Etiket Yazıcısı";
          break;
      }
    }

    private void btnTamam_Click(object sender, EventArgs e)
    {
      this.YaziciIndex = this.cbYazici.SelectedIndex;
      System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      switch (this.Tip)
      {
        case 1:
          configuration.AppSettings.Settings["ResmiFaturaYazici"].Value = this.cbYazici.Text;
          MyUtils.ResmiFaturaYazici = this.cbYazici.Text;
          break;
        case 2:
          configuration.AppSettings.Settings["SiparisYazici"].Value = this.cbYazici.Text;
          MyUtils.SiparisYazici = this.cbYazici.Text;
          break;
        case 3:
          configuration.AppSettings.Settings["GayriFaturaYazici"].Value = this.cbYazici.Text;
          MyUtils.GayriFaturaYazici = this.cbYazici.Text;
          break;
        case 4:
          configuration.AppSettings.Settings["EtiketYazici"].Value = this.cbYazici.Text;
          MyUtils.EtiketYazici = this.cbYazici.Text;
          break;
      }
      configuration.Save(ConfigurationSaveMode.Modified);
      ConfigurationManager.RefreshSection("appSettings");
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
      this.cbYazici = new System.Windows.Forms.ComboBox();
      this.btnTamam = new SimpleButton();
      this.SuspendLayout();
      this.cbYazici.Dock = DockStyle.Top;
      this.cbYazici.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbYazici.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.cbYazici.FormattingEnabled = true;
      this.cbYazici.Location = new Point(0, 0);
      this.cbYazici.Name = "cbYazici";
      this.cbYazici.Size = new Size(455, 32);
      this.cbYazici.TabIndex = 0;
      this.btnTamam.Dock = DockStyle.Fill;
      this.btnTamam.Location = new Point(0, 32);
      this.btnTamam.Name = "btnTamam";
      this.btnTamam.Size = new Size(455, 35);
      this.btnTamam.TabIndex = 1;
      this.btnTamam.Text = "Tamam";
      this.btnTamam.Click += new EventHandler(this.btnTamam_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(455, 67);
      this.Controls.Add((Control) this.btnTamam);
      this.Controls.Add((Control) this.cbYazici);
      this.Name = nameof (FrmYaziciSec);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Yazıcı Seç";
      this.Load += new EventHandler(this.FrmYaziciSec_Load);
      this.ResumeLayout(false);
    }
  }
}
