// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmAdet
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
  public class FrmAdet : Form
  {
    public int Adet = 1;
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox txtAdet;
    private SimpleButton btnTamam;

    public FrmAdet() => this.InitializeComponent();

    private void btnTamam_Click(object sender, EventArgs e)
    {
      try
      {
        this.Adet = Convert.ToInt32(this.txtAdet.Text);
      }
      catch
      {
        this.Adet = 1;
      }
      this.Close();
    }

    private void txtAdet_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.btnTamam_Click((object) null, (EventArgs) null);
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
      this.txtAdet = new TextBox();
      this.btnTamam = new SimpleButton();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(118, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Adet Giriniz";
      this.txtAdet.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.txtAdet.Location = new Point(152, 9);
      this.txtAdet.Name = "txtAdet";
      this.txtAdet.Size = new Size(100, 29);
      this.txtAdet.TabIndex = 1;
      this.txtAdet.KeyPress += new KeyPressEventHandler(this.txtAdet_KeyPress);
      this.btnTamam.Location = new Point(12, 53);
      this.btnTamam.Name = "btnTamam";
      this.btnTamam.Size = new Size(240, 34);
      this.btnTamam.TabIndex = 2;
      this.btnTamam.Text = "Tamam";
      this.btnTamam.Click += new EventHandler(this.btnTamam_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(268, 99);
      this.Controls.Add((Control) this.btnTamam);
      this.Controls.Add((Control) this.txtAdet);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmAdet);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Adet Giriniz";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
