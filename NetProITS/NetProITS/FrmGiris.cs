// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmGiris
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using NetOpenX50;
using NetProITS.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmGiris : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private ComboBox cbSirket;
    private Button btnGiris;
    private PictureBox pictureBox1;
    private TextBox txtUser;
    private Label label2;
    private Label label3;
    private TextBox txtPass;

    public FrmGiris() => this.InitializeComponent();

    private void cbSirket_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.SelectNextControl((Control) sender, true, true, true, true);
    }

    private void FrmGiris_Load(object sender, EventArgs e)
    {
      this.cbSirket.Items.Clear();
      foreach (Sirketler sirketler in new MikroDB_V14Entities().Database.SqlQuery<Sirketler>("SELECT SIRKET as DB_kod FROM SIRKETLER30").ToList<Sirketler>())
        this.cbSirket.Items.Add((object) sirketler.DB_kod);
      MyUtils.GetConfigBilgi();
      this.txtUser.Text = MyUtils.strNetsisUser;
      this.txtPass.Text = MyUtils.strNetsisPassword;
      this.cbSirket.SelectedIndex = this.cbSirket.Items.IndexOf((object) MyUtils.strDatabase);
    }

    private void btnGiris_Click(object sender, EventArgs e)
    {
      if (this.cbSirket.Text.Trim().Length <= 0)
      {
        int num1 = (int) MessageBox.Show("Lütfen şirket seçiniz!");
      }
      else if (this.txtUser.Text.Trim() == "" || this.txtPass.Text.Trim() == "")
      {
        int num2 = (int) MessageBox.Show("Lütfen Kullanıcı adı veya şifre giriniz!");
      }
      else
      {
        MyUtils.strDatabase = this.cbSirket.Text.Trim();
        MyUtils.GetConfigBilgi();
        MyUtils.Refresh();
        MyUtils.kernel = (Kernel) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("65EB3876-89FF-459F-BF24-02E8DD7F2DB2")));
        MyUtils.sirket = (Sirket) null;
        try
        {
          // ISSUE: reference to a compiler-generated method
          MyUtils.sirket = MyUtils.kernel.yeniSirket(TVTTipi.vtMSSQL, MyUtils.strDatabase, "TEMELSET", "", MyUtils.strNetsisUser, MyUtils.strNetsisPassword, 0);
        }
        catch (Exception ex)
        {
          if (MyUtils.sirket != null)
            Marshal.ReleaseComObject((object) MyUtils.sirket);
          // ISSUE: reference to a compiler-generated method
          MyUtils.kernel.FreeNetsisLibrary();
          Marshal.ReleaseComObject((object) MyUtils.kernel);
          int num3 = (int) MessageBox.Show("Hata oluştu : " + ex.Message);
          return;
        }
        MyUtils.strNetsisUser = this.txtUser.Text;
        MyUtils.strNetsisPassword = this.txtPass.Text;
        System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        configuration.AppSettings.Settings["NetsisUser"].Value = this.txtUser.Text;
        configuration.AppSettings.Settings["NetsisPassword"].Value = this.txtPass.Text;
        configuration.AppSettings.Settings["Database"].Value = MyUtils.strDatabase;
        configuration.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
        FrmBildirim frmBildirim = new FrmBildirim();
        int num4 = (int) frmBildirim.ShowDialog();
        frmBildirim.Dispose();
      }
    }

    private void FrmGiris_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (MyUtils.sirket != null)
      {
        // ISSUE: reference to a compiler-generated method
        MyUtils.sirket.LogOff();
        Marshal.ReleaseComObject((object) MyUtils.sirket);
      }
      if (MyUtils.kernel == null)
        return;
      // ISSUE: reference to a compiler-generated method
      MyUtils.kernel.FreeNetsisLibrary();
      Marshal.ReleaseComObject((object) MyUtils.kernel);
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
      this.cbSirket = new ComboBox();
      this.pictureBox1 = new PictureBox();
      this.btnGiris = new Button();
      this.txtUser = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtPass = new TextBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label1.ForeColor = Color.Green;
      this.label1.Location = new Point(160, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 25);
      this.label1.TabIndex = 0;
      this.label1.Text = "Şirket";
      this.cbSirket.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSirket.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.cbSirket.FormattingEnabled = true;
      this.cbSirket.Location = new Point(268, 12);
      this.cbSirket.Name = "cbSirket";
      this.cbSirket.Size = new Size(198, 33);
      this.cbSirket.TabIndex = 1;
      this.cbSirket.KeyPress += new KeyPressEventHandler(this.cbSirket_KeyPress);
      this.pictureBox1.BackgroundImage = (Image) Resources._1306351836_gnome_keyring_manager;
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Center;
      this.pictureBox1.Dock = DockStyle.Left;
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(147, 197);
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      this.btnGiris.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.btnGiris.Image = (Image) Resources._1311968555_button_ok;
      this.btnGiris.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnGiris.Location = new Point(304, 135);
      this.btnGiris.Name = "btnGiris";
      this.btnGiris.Size = new Size(162, 50);
      this.btnGiris.TabIndex = 2;
      this.btnGiris.Text = "Giriş Yap";
      this.btnGiris.TextAlign = ContentAlignment.MiddleRight;
      this.btnGiris.UseVisualStyleBackColor = true;
      this.btnGiris.Click += new EventHandler(this.btnGiris_Click);
      this.txtUser.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.txtUser.Location = new Point(268, 48);
      this.txtUser.Name = "txtUser";
      this.txtUser.Size = new Size(198, 31);
      this.txtUser.TabIndex = 4;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label2.ForeColor = Color.Green;
      this.label2.Location = new Point(160, 51);
      this.label2.Name = "label2";
      this.label2.Size = new Size(102, 25);
      this.label2.TabIndex = 5;
      this.label2.Text = "Kullanıcı";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.label3.ForeColor = Color.Green;
      this.label3.Location = new Point(160, 85);
      this.label3.Name = "label3";
      this.label3.Size = new Size(61, 25);
      this.label3.TabIndex = 7;
      this.label3.Text = "Şifre";
      this.txtPass.Font = new Font("Microsoft Sans Serif", 15.75f);
      this.txtPass.Location = new Point(268, 82);
      this.txtPass.Name = "txtPass";
      this.txtPass.PasswordChar = '*';
      this.txtPass.Size = new Size(198, 31);
      this.txtPass.TabIndex = 6;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(478, 197);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtPass);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtUser);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnGiris);
      this.Controls.Add((Control) this.cbSirket);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (FrmGiris);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Giriş";
      this.FormClosed += new FormClosedEventHandler(this.FrmGiris_FormClosed);
      this.Load += new EventHandler(this.FrmGiris_Load);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
