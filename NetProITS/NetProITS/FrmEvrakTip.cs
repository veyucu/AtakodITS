// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmEvrakTip
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
  public class FrmEvrakTip : Form
  {
    public int Tip = -1;
    public string EvrakSeri = string.Empty;
    public string DizaynNo = string.Empty;
    private IContainer components = (IContainer) null;
    private Label label1;
    private Button btnOk;
    private ComboBox cbTip;

    public FrmEvrakTip()
    {
      this.InitializeComponent();
      this.cbTip.SelectedIndex = 0;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.cbTip.SelectedIndex == -1)
      {
        int num = (int) MessageBox.Show("Fatura Tipini seçiniz!");
      }
      else
      {
        switch (this.cbTip.SelectedIndex)
        {
          case 0:
            this.EvrakSeri = MyUtils.GetParamValue("GayriEvrakSeri");
            this.DizaynNo = MyUtils.GetParamValue("GayriFaturaDizaynNo");
            this.Tip = 0;
            break;
          case 1:
            this.EvrakSeri = MyUtils.GetParamValue("ResmiEvrakSeri");
            this.DizaynNo = MyUtils.GetParamValue("FaturaDizaynNo");
            this.Tip = 1;
            break;
        }
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
      this.label1 = new Label();
      this.btnOk = new Button();
      this.cbTip = new ComboBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.Location = new Point(8, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(95, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Fatura Tipi";
      this.btnOk.Dock = DockStyle.Bottom;
      this.btnOk.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.btnOk.Location = new Point(0, 59);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(324, 38);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "Tamam";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.cbTip.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTip.Font = new Font("Tahoma", 15.25f);
      this.cbTip.FormattingEnabled = true;
      this.cbTip.Items.AddRange(new object[2]
      {
        (object) "Gayri",
        (object) "Resmi"
      });
      this.cbTip.Location = new Point(109, 14);
      this.cbTip.Name = "cbTip";
      this.cbTip.Size = new Size(193, 32);
      this.cbTip.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(324, 97);
      this.Controls.Add((Control) this.cbTip);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmEvrakTip);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Fatura No";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
