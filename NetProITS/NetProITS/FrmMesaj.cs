// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmMesaj
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmMesaj : Form
  {
    private IContainer components = (IContainer) null;
    private Label lblUyari;

    public FrmMesaj(string Mesaj)
    {
      this.InitializeComponent();
      this.lblUyari.Text = Mesaj;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblUyari = new Label();
      this.SuspendLayout();
      this.lblUyari.AutoSize = true;
      this.lblUyari.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.lblUyari.Location = new Point(12, 9);
      this.lblUyari.Name = "lblUyari";
      this.lblUyari.Size = new Size(52, 19);
      this.lblUyari.TabIndex = 0;
      this.lblUyari.Text = "Uyarı";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(547, 77);
      this.Controls.Add((Control) this.lblUyari);
      this.Name = nameof (FrmMesaj);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Uyarı";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
