// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmKareKod
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
  public class FrmKareKod : Form
  {
    private bool keypad_alt_0 = false;
    private bool keypad_alt_2 = false;
    private bool keypad_alt_9 = false;
    private string currenttoken = "";
    private int nextConsume = 0;
    private bool consumingGtin = false;
    private bool consumingSerial = false;
    private bool consumingExpDate = false;
    private bool consumingBatch = false;
    private bool consumingAI = true;
    private bool consumingOther = false;
    private int WAIT_FOR_FNC1 = 10000;
    public string strGtin = "";
    public string strseriNo = "";
    public string strPartiNo = "";
    public string strMiad = "";
    public string strKoliBarkod = "";
    public bool chkKolii = false;
    public string strBarkod = "";
    private IContainer components = (IContainer) null;
    private TextBox textBox1;
    private Label label1;
    private TextBox expdate;
    private TextBox batch;
    private TextBox serial;
    private TextBox gtin;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Button btnBitir;
    private CheckBox chkKoli;

    public FrmKareKod() => this.InitializeComponent();

    public void processCurrentToken(Keys keynum)
    {
      string str = ((char) new KeyEventArgs(keynum).KeyValue).ToString();
      this.currenttoken += str;
      if (!this.consumingAI && this.nextConsume == 0)
      {
        this.consumingGtin = false;
        this.consumingSerial = false;
        this.consumingExpDate = false;
        this.consumingBatch = false;
        this.consumingOther = false;
        this.consumingAI = true;
        this.currenttoken = str;
      }
      if (keynum == Keys.IMENonconvert)
      {
        this.currenttoken = "";
        this.nextConsume = 0;
      }
      else if (this.currenttoken == "01" && this.consumingAI)
      {
        this.currenttoken = "";
        this.nextConsume = 14;
        this.consumingGtin = true;
        this.consumingAI = false;
      }
      else if (this.currenttoken == "21" && this.consumingAI)
      {
        this.currenttoken = "";
        this.nextConsume = this.WAIT_FOR_FNC1;
        this.consumingSerial = true;
        this.consumingAI = false;
      }
      else if (this.currenttoken == "10" && this.consumingAI)
      {
        this.currenttoken = "";
        this.nextConsume = this.WAIT_FOR_FNC1;
        this.consumingBatch = true;
        this.consumingAI = false;
      }
      else if (this.currenttoken == "17" && this.consumingAI)
      {
        this.currenttoken = "";
        this.nextConsume = 6;
        this.consumingExpDate = true;
        this.consumingAI = false;
      }
      else
      {
        if (this.consumingGtin)
        {
          this.gtin.Text = this.currenttoken;
          this.strGtin = this.gtin.Text.Replace("\r", "");
          --this.nextConsume;
        }
        if (this.consumingSerial)
        {
          this.serial.Text = this.currenttoken;
          this.strseriNo = this.serial.Text.Replace("\r", "");
          --this.nextConsume;
        }
        if (this.consumingBatch)
        {
          this.batch.Text = this.currenttoken;
          this.strPartiNo = this.batch.Text.Replace("\r", "");
          --this.nextConsume;
        }
        if (!this.consumingExpDate)
          return;
        this.expdate.Text = this.currenttoken;
        this.strMiad = this.expdate.Text.Replace("\r", "");
        --this.nextConsume;
      }
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.chkKoli.Checked)
        return;
      Keys keynum = e.KeyCode;
      if (Keys.F1 <= keynum && keynum <= Keys.F12)
        this.processCurrentToken(Keys.IMENonconvert);
      else if (e.Alt)
      {
        if (keynum == Keys.Menu)
        {
          this.keypad_alt_0 = false;
          this.keypad_alt_2 = false;
          this.keypad_alt_9 = false;
        }
        if (keynum == Keys.Insert || Convert.ToInt32(keynum.ToString()) == 96 || Convert.ToInt32(keynum.ToString()) == 48)
          this.keypad_alt_0 = true;
        if (keynum == Keys.Down || Convert.ToInt32(keynum.ToString()) == 98 || Convert.ToInt32(keynum.ToString()) == 50)
          this.keypad_alt_2 = true;
        if (keynum == Keys.Prior || keynum == Keys.NumPad9 || keynum == Keys.D9)
          this.keypad_alt_9 = true;
        if (this.keypad_alt_0 && this.keypad_alt_2 && this.keypad_alt_9)
        {
          keynum = Keys.IMENonconvert;
          this.processCurrentToken(keynum);
          this.keypad_alt_0 = false;
          this.keypad_alt_2 = false;
          this.keypad_alt_9 = false;
        }
      }
    }

    private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
      char keyChar = e.KeyChar;
      KeyEventArgs keyEventArgs = new KeyEventArgs((Keys) e.KeyChar);
      if (keyEventArgs.Alt)
        return;
      if (keyEventArgs.KeyCode == Keys.Return)
      {
        this.chkKolii = this.chkKoli.Checked;
        this.strBarkod = this.textBox1.Text;
        if (this.chkKolii)
          this.strKoliBarkod = this.textBox1.Text;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
      if (!this.chkKoli.Checked)
        this.processCurrentToken((Keys) e.KeyChar);
    }

    public void resetConsume()
    {
      this.consumingGtin = false;
      this.consumingSerial = false;
      this.consumingExpDate = false;
      this.consumingBatch = false;
      this.consumingOther = false;
      this.consumingAI = true;
      this.currenttoken = "";
      this.nextConsume = 0;
    }

    private void btnBitir_Click(object sender, EventArgs e)
    {
      this.chkKolii = this.chkKoli.Checked;
      this.strBarkod = this.textBox1.Text;
      if (this.chkKolii)
      {
        this.strKoliBarkod = this.textBox1.Text;
      }
      else
      {
        this.strGtin = this.gtin.Text;
        this.strMiad = this.expdate.Text;
        this.strPartiNo = this.batch.Text;
        this.strseriNo = this.serial.Text;
      }
      this.DialogResult = DialogResult.OK;
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
      this.textBox1 = new TextBox();
      this.label1 = new Label();
      this.expdate = new TextBox();
      this.batch = new TextBox();
      this.serial = new TextBox();
      this.gtin = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.btnBitir = new Button();
      this.chkKoli = new CheckBox();
      this.SuspendLayout();
      this.textBox1.Location = new Point(94, 5);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(404, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.KeyDown += new KeyEventHandler(this.textBox1_KeyDown);
      this.textBox1.KeyPress += new KeyPressEventHandler(this.textBox1_KeyPress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(47, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Karekod";
      this.expdate.Location = new Point(94, 110);
      this.expdate.Name = "expdate";
      this.expdate.Size = new Size(180, 20);
      this.expdate.TabIndex = 8;
      this.batch.Location = new Point(94, 84);
      this.batch.Name = "batch";
      this.batch.Size = new Size(180, 20);
      this.batch.TabIndex = 7;
      this.serial.Location = new Point(94, 58);
      this.serial.Name = "serial";
      this.serial.Size = new Size(180, 20);
      this.serial.TabIndex = 6;
      this.gtin.Location = new Point(94, 32);
      this.gtin.Name = "gtin";
      this.gtin.Size = new Size(180, 20);
      this.gtin.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "(01) GTIN";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 61);
      this.label3.Name = "label3";
      this.label3.Size = new Size(72, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "(21) SERI NO";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 87);
      this.label4.Name = "label4";
      this.label4.Size = new Size(79, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "(10) PARTI NO";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 113);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "(17) MİAD";
      this.btnBitir.DialogResult = DialogResult.OK;
      this.btnBitir.Location = new Point(417, 107);
      this.btnBitir.Name = "btnBitir";
      this.btnBitir.Size = new Size(75, 23);
      this.btnBitir.TabIndex = 13;
      this.btnBitir.Text = "Bitir";
      this.btnBitir.UseVisualStyleBackColor = true;
      this.btnBitir.Click += new EventHandler(this.btnBitir_Click);
      this.chkKoli.AutoSize = true;
      this.chkKoli.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.chkKoli.Location = new Point(327, 35);
      this.chkKoli.Name = "chkKoli";
      this.chkKoli.Size = new Size(165, 29);
      this.chkKoli.TabIndex = 14;
      this.chkKoli.Text = "Koli Barkodu";
      this.chkKoli.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(520, 145);
      this.Controls.Add((Control) this.chkKoli);
      this.Controls.Add((Control) this.btnBitir);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.expdate);
      this.Controls.Add((Control) this.batch);
      this.Controls.Add((Control) this.serial);
      this.Controls.Add((Control) this.gtin);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox1);
      this.Name = nameof (FrmKareKod);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Karekod Oku";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
