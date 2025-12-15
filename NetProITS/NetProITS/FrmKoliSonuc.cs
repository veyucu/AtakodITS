// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmKoliSonuc
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmKoliSonuc : Form
  {
    public string Sonuc = (string) null;
    private IContainer components = (IContainer) null;
    private TextBox textBox1;

    public FrmKoliSonuc() => this.InitializeComponent();

    private void FrmKoliSonuc_Shown(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.Sonuc))
        return;
      this.textBox1.Text = this.Sonuc;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.SuspendLayout();
      this.textBox1.Dock = DockStyle.Fill;
      this.textBox1.Location = new Point(0, 0);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(616, 444);
      this.textBox1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(616, 444);
      this.Controls.Add((Control) this.textBox1);
      this.Name = nameof (FrmKoliSonuc);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "İşlem Sonucu";
      this.Shown += new EventHandler(this.FrmKoliSonuc_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
