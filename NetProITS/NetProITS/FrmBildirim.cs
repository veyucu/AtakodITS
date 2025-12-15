// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmBildirim
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmBildirim : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnSatisBildir;
    private Button btnPTSGuncelle;
    private Button btnYonetim;
    private Button btnDeak;
    private Button btnSerbestBildirim;
    private Button btnAyarlar;

    public FrmBildirim() => this.InitializeComponent();

    private void btnSatisBildir_Click(object sender, EventArgs e) => new FrmSatisBildirimi().Show();

    private void button1_Click_1(object sender, EventArgs e)
    {
      FrmPaketTransfer frmPaketTransfer = new FrmPaketTransfer();
      int num = (int) frmPaketTransfer.ShowDialog();
      frmPaketTransfer.Dispose();
    }

    private void btnYonetim_Click(object sender, EventArgs e)
    {
      FrmPaketYonetim frmPaketYonetim = new FrmPaketYonetim();
      int num = (int) frmPaketYonetim.ShowDialog();
      frmPaketYonetim.Dispose();
    }

    private void FrmBildirim_Shown(object sender, EventArgs e)
    {
    }

    private void btnDeak_Click(object sender, EventArgs e)
    {
      FrmDeaktivasyon frmDeaktivasyon = new FrmDeaktivasyon();
      int num = (int) frmDeaktivasyon.ShowDialog();
      frmDeaktivasyon.Dispose();
    }

    private void button2_Click(object sender, EventArgs e) => new FrmSerbestBildirim().Show();

    private void FrmBildirim_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F12)
      {
        FrmYaziciSec frmYaziciSec = new FrmYaziciSec();
        frmYaziciSec.Tip = 1;
        int num = (int) frmYaziciSec.ShowDialog();
        frmYaziciSec.Dispose();
      }
      if (e.KeyCode == Keys.F11)
      {
        FrmYaziciSec frmYaziciSec = new FrmYaziciSec();
        frmYaziciSec.Tip = 2;
        int num = (int) frmYaziciSec.ShowDialog();
        frmYaziciSec.Dispose();
      }
      if (e.KeyCode == Keys.F10)
      {
        FrmYaziciSec frmYaziciSec = new FrmYaziciSec();
        frmYaziciSec.Tip = 3;
        int num = (int) frmYaziciSec.ShowDialog();
        frmYaziciSec.Dispose();
      }
      if (e.KeyCode != Keys.F9)
        return;
      FrmYaziciSec frmYaziciSec1 = new FrmYaziciSec();
      frmYaziciSec1.Tip = 4;
      int num1 = (int) frmYaziciSec1.ShowDialog();
      frmYaziciSec1.Dispose();
    }

    private void btnAyarlar_Click(object sender, EventArgs e)
    {
      FrmAyarlar frmAyarlar = new FrmAyarlar();
      int num = (int) frmAyarlar.ShowDialog();
      frmAyarlar.Dispose();
    }

    private void FrmBildirim_Load(object sender, EventArgs e)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmBildirim));
      this.btnSerbestBildirim = new Button();
      this.btnDeak = new Button();
      this.btnYonetim = new Button();
      this.btnPTSGuncelle = new Button();
      this.btnSatisBildir = new Button();
      this.btnAyarlar = new Button();
      this.SuspendLayout();
      this.btnSerbestBildirim.FlatStyle = FlatStyle.Flat;
      this.btnSerbestBildirim.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnSerbestBildirim.ForeColor = Color.Green;
      this.btnSerbestBildirim.Image = (Image) componentResourceManager.GetObject("btnSerbestBildirim.Image");
      this.btnSerbestBildirim.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSerbestBildirim.Location = new Point(260, 7);
      this.btnSerbestBildirim.Name = "btnSerbestBildirim";
      this.btnSerbestBildirim.Size = new Size(249, 75);
      this.btnSerbestBildirim.TabIndex = 8;
      this.btnSerbestBildirim.Text = "Serbest Bildirim";
      this.btnSerbestBildirim.TextAlign = ContentAlignment.MiddleRight;
      this.btnSerbestBildirim.UseVisualStyleBackColor = true;
      this.btnSerbestBildirim.Click += new EventHandler(this.button2_Click);
      this.btnDeak.FlatStyle = FlatStyle.Flat;
      this.btnDeak.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnDeak.ForeColor = Color.Green;
      this.btnDeak.Image = (Image) componentResourceManager.GetObject("btnDeak.Image");
      this.btnDeak.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnDeak.Location = new Point(5, 170);
      this.btnDeak.Name = "btnDeak";
      this.btnDeak.Size = new Size(249, 75);
      this.btnDeak.TabIndex = 7;
      this.btnDeak.Text = "     Deaktivasyon";
      this.btnDeak.UseVisualStyleBackColor = true;
      this.btnDeak.Click += new EventHandler(this.btnDeak_Click);
      this.btnYonetim.FlatStyle = FlatStyle.Flat;
      this.btnYonetim.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnYonetim.ForeColor = Color.Green;
      this.btnYonetim.Image = (Image) componentResourceManager.GetObject("btnYonetim.Image");
      this.btnYonetim.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnYonetim.Location = new Point(260, 89);
      this.btnYonetim.Name = "btnYonetim";
      this.btnYonetim.Size = new Size(249, 75);
      this.btnYonetim.TabIndex = 6;
      this.btnYonetim.Text = "     PTS Yönetim";
      this.btnYonetim.UseVisualStyleBackColor = true;
      this.btnYonetim.Click += new EventHandler(this.btnYonetim_Click);
      this.btnPTSGuncelle.FlatStyle = FlatStyle.Flat;
      this.btnPTSGuncelle.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnPTSGuncelle.ForeColor = Color.Green;
      this.btnPTSGuncelle.Image = (Image) componentResourceManager.GetObject("btnPTSGuncelle.Image");
      this.btnPTSGuncelle.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPTSGuncelle.Location = new Point(5, 89);
      this.btnPTSGuncelle.Name = "btnPTSGuncelle";
      this.btnPTSGuncelle.Size = new Size(249, 75);
      this.btnPTSGuncelle.TabIndex = 5;
      this.btnPTSGuncelle.Text = "     PTS Güncelle";
      this.btnPTSGuncelle.UseVisualStyleBackColor = true;
      this.btnPTSGuncelle.Click += new EventHandler(this.button1_Click_1);
      this.btnSatisBildir.FlatStyle = FlatStyle.Flat;
      this.btnSatisBildir.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnSatisBildir.ForeColor = Color.Green;
      this.btnSatisBildir.Image = (Image) componentResourceManager.GetObject("btnSatisBildir.Image");
      this.btnSatisBildir.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSatisBildir.Location = new Point(5, 7);
      this.btnSatisBildir.Name = "btnSatisBildir";
      this.btnSatisBildir.Size = new Size(249, 75);
      this.btnSatisBildir.TabIndex = 0;
      this.btnSatisBildir.Text = "Bildirim Ekranı";
      this.btnSatisBildir.TextAlign = ContentAlignment.MiddleRight;
      this.btnSatisBildir.UseVisualStyleBackColor = true;
      this.btnSatisBildir.Click += new EventHandler(this.btnSatisBildir_Click);
      this.btnAyarlar.FlatStyle = FlatStyle.Flat;
      this.btnAyarlar.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnAyarlar.ForeColor = Color.Green;
      this.btnAyarlar.Image = (Image) componentResourceManager.GetObject("btnAyarlar.Image");
      this.btnAyarlar.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAyarlar.Location = new Point(260, 170);
      this.btnAyarlar.Name = "btnAyarlar";
      this.btnAyarlar.Size = new Size(249, 75);
      this.btnAyarlar.TabIndex = 11;
      this.btnAyarlar.Text = "     Ayarlar";
      this.btnAyarlar.UseVisualStyleBackColor = true;
      this.btnAyarlar.Click += new EventHandler(this.btnAyarlar_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(513, 256);
      this.Controls.Add((Control) this.btnAyarlar);
      this.Controls.Add((Control) this.btnSerbestBildirim);
      this.Controls.Add((Control) this.btnDeak);
      this.Controls.Add((Control) this.btnYonetim);
      this.Controls.Add((Control) this.btnPTSGuncelle);
      this.Controls.Add((Control) this.btnSatisBildir);
      this.KeyPreview = true;
      this.Name = nameof (FrmBildirim);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bildirim Ekranı";
      this.Load += new EventHandler(this.FrmBildirim_Load);
      this.Shown += new EventHandler(this.FrmBildirim_Shown);
      this.KeyDown += new KeyEventHandler(this.FrmBildirim_KeyDown);
      this.ResumeLayout(false);
    }
  }
}
