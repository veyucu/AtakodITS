// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmEvrakNo
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
  public class FrmEvrakNo : Form
  {
    public string EvrakNo = string.Empty;
    public string EvrakSeri = string.Empty;
    public string EvrakSira = string.Empty;
    private IContainer components = (IContainer) null;
    private TextBox txtEvrakSeri;
    private Button btnOk;
    private TextBox txtFaturaNo;
    private Label label1;

    public FrmEvrakNo() => this.InitializeComponent();

    public FrmEvrakNo(string evrakseri, string evraksira)
    {
      this.InitializeComponent();
      this.txtEvrakSeri.Text = evrakseri;
      this.txtFaturaNo.Text = evraksira;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.txtEvrakSeri.Text.Trim() == "" || this.txtFaturaNo.Text.Trim() == "")
      {
        int num = (int) MessageBox.Show("Evrak Numarasını Giriniz!");
      }
      else
      {
        Convert.ToInt32(this.txtFaturaNo.Text.Trim());
        string str = this.txtEvrakSeri.Text.Trim();
        this.EvrakNo = this.txtEvrakSeri.Text.Trim() + str + this.txtFaturaNo.Text.Trim();
        this.EvrakSeri = this.txtEvrakSeri.Text.Trim();
        this.EvrakSira = this.txtFaturaNo.Text.Trim();
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtEvrakSeri = new TextBox();
      this.btnOk = new Button();
      this.txtFaturaNo = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.txtEvrakSeri.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.txtEvrakSeri.Location = new Point(104, 14);
      this.txtEvrakSeri.Name = "txtEvrakSeri";
      this.txtEvrakSeri.ReadOnly = true;
      this.txtEvrakSeri.Size = new Size(39, 29);
      this.txtEvrakSeri.TabIndex = 4;
      this.btnOk.Dock = DockStyle.Bottom;
      this.btnOk.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnOk.Location = new Point(0, 57);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(338, 38);
      this.btnOk.TabIndex = 5;
      this.btnOk.Text = "Tamam";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.txtFaturaNo.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.txtFaturaNo.Location = new Point(146, 14);
      this.txtFaturaNo.Name = "txtFaturaNo";
      this.txtFaturaNo.Size = new Size(176, 29);
      this.txtFaturaNo.TabIndex = 6;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.Location = new Point(8, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(89, 20);
      this.label1.TabIndex = 3;
      this.label1.Text = "Fatura No";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(338, 95);
      this.Controls.Add((Control) this.txtEvrakSeri);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtFaturaNo);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmEvrakNo);
      this.Text = "Fatura No";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
