// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmPaketTransfer
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace NetProITS
{
  public class FrmPaketTransfer : Form
  {
    private DateTime dDate;
    private string dNumber;
    private string dGlnNo;
    private long dTransferID;
    private string drm = "";
    private IContainer components = (IContainer) null;
    private ProgressBar detay;
    private ProgressBar master;
    private Panel panel2;
    private DateTimePicker dateSon;
    private Label label2;
    private DateTimePicker dateIlk;
    private Label label1;
    private Panel panel1;
    private Button btnDosyaImport;
    private Button btnBasla;
    private ImageList imgList;
    private Button btnListe;
    private TextBox txtPTSNo;
    private Label label3;

    public FrmPaketTransfer() => this.InitializeComponent();

    public void NodeProcess(XmlNode node, string cLabel)
    {
      if (node.ChildNodes.Count <= 0)
        return;
      for (int i1 = 0; i1 < node.ChildNodes.Count; ++i1)
      {
        if (node.ChildNodes[i1].Name == "carrier")
        {
          string cLabel1;
          try
          {
            cLabel1 = node.ChildNodes[i1].Attributes["carrierLabel"].Value;
          }
          catch
          {
            cLabel1 = "0000000000000000000";
          }
          this.drm = "3";
          this.NodeProcess(node.ChildNodes[i1], cLabel1);
        }
        else if (node.ChildNodes[i1].Name == "productList" && node.ChildNodes[i1].ChildNodes.Count > 0)
        {
          string str1 = node.ChildNodes[i1].Attributes["GTIN"].Value;
          string str2;
          try
          {
            str2 = node.ChildNodes[i1].Attributes["expirationDate"].Value;
          }
          catch
          {
            str2 = node.ChildNodes[i1].Attributes["ExpirationDate"].Value;
          }
          string str3;
          try
          {
            str3 = node.ChildNodes[i1].Attributes["lotNumber"].Value;
          }
          catch
          {
            str3 = node.ChildNodes[i1].Attributes["LotNumber"].Value;
          }
          this.drm = "4";
          this.detay.Minimum = 0;
          this.detay.Maximum = node.ChildNodes[i1].ChildNodes.Count - 1;
          for (int i2 = 0; i2 < node.ChildNodes[i1].ChildNodes.Count; ++i2)
          {
            this.drm = "5";
            TBLTRANSFER_DETAY entity = new TBLTRANSFER_DETAY();
            this.drm = cLabel + "-" + str2 + "-" + this.dNumber + "-" + str1 + "-" + str3 + "-" + node.ChildNodes[i1].ChildNodes[i2].InnerText + "-" + this.dTransferID.ToString();
            entity.ID = 0;
            entity.CARRIER_LABEL = cLabel;
            entity.DATE = str2;
            entity.DOCUMENT_DATE = new DateTime?(this.dDate);
            entity.DOCUMENT_NUMBER = this.dNumber.Length <= 20 ? this.dNumber : this.dNumber.Substring(0, 20);
            entity.GTIN = str1;
            entity.KAYNAK_GLNNO = this.dGlnNo;
            entity.LOT_NUMBER = str3;
            entity.SERIAL_NUMBER = node.ChildNodes[i1].ChildNodes[i2].InnerText;
            entity.TRANSFER_ID = new long?(this.dTransferID);
            MyUtils.Firma.TBLTRANSFER_DETAY.Add(entity);
            MyUtils.Firma.SaveChanges();
            this.detay.Value = i2;
          }
        }
      }
    }

    private void btnBasla_Click(object sender, EventArgs e)
    {
      string paramValue1 = MyUtils.GetParamValue("GlnNo");
      string paramValue2 = MyUtils.GetParamValue("KullaniciAdi");
      string paramValue3 = MyUtils.GetParamValue("Sifre");
      Cursor.Current = Cursors.WaitCursor;
      List<string> stringList = BildirimHelper.PaketSorgulama(paramValue2, paramValue3, "", paramValue1, this.dateIlk.Value, this.dateSon.Value.AddDays(1.0));
      StringBuilder stringBuilder = new StringBuilder();
      if (stringList != null && stringList.Count > 0)
      {
        foreach (string str1 in stringList)
        {
          string item = str1;
          long ptsid = Convert.ToInt64(item);
          if (MyUtils.Firma.TBLTRANSFER.Where<TBLTRANSFER>((Expression<Func<TBLTRANSFER, bool>>) (u => u.TRANSFER_ID == ptsid)).FirstOrDefault<TBLTRANSFER>() == null)
          {
            string s = BildirimHelper.PaketIndir(paramValue2, paramValue3, item);
            if (s != null && s.Length > 0)
            {
              MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(s));
              MemoryStream inStream = new MemoryStream();
              using (ZipInputStream zipInputStream = new ZipInputStream((Stream) memoryStream))
              {
                while (zipInputStream.GetNextEntry() != null)
                {
                  byte[] buffer = new byte[memoryStream.ToArray().Length];
                  for (int count = ((Stream) zipInputStream).Read(buffer, 0, buffer.Length); count > 0; count = ((Stream) zipInputStream).Read(buffer, 0, buffer.Length))
                    inStream.Write(buffer, 0, count);
                }
              }
              XmlDocument xmlDocument = new XmlDocument();
              try
              {
                Application.DoEvents();
                inStream.Position = 0L;
                xmlDocument.Load((Stream) inStream);
                memoryStream.Close();
                XmlElement documentElement = xmlDocument.DocumentElement;
                this.dNumber = documentElement.SelectSingleNode("documentNumber").InnerText;
                this.dDate = DateTime.Now;
                this.dGlnNo = documentElement.SelectSingleNode("sourceGLN").InnerText;
                try
                {
                  this.dDate = DateTime.ParseExact(documentElement.SelectSingleNode("documentDate").InnerText, "yyyy-MM-dd", (IFormatProvider) null);
                }
                catch
                {
                  this.dDate = DateTime.Now;
                }
                if (!Directory.Exists("Gelen Paketler"))
                  Directory.CreateDirectory("Gelen Paketler");
                TBLTRANSFER entity = new TBLTRANSFER();
                entity.KAYNAK_GLNNO = this.dGlnNo;
                this.dTransferID = Convert.ToInt64(item);
                TBLTRANSFER tbltransfer = entity;
                DateTime dateTime = DateTime.Now;
                dateTime = dateTime.Date;
                string str2 = dateTime.ToString("yyyy-MM-dd");
                tbltransfer.TARIH = str2;
                entity.TRANSFER_ID = Convert.ToInt64(item);
                xmlDocument.Save(Application.StartupPath + "\\Gelen Paketler\\" + this.dGlnNo + "-" + item + ".xml");
                this.NodeProcess((XmlNode) documentElement, "");
                MyUtils.Firma.TBLTRANSFER.Add(entity);
                MyUtils.Firma.SaveChanges();
              }
              catch
              {
                DbSet<TBLTRANSFER_DETAY> tbltransferDetay1 = MyUtils.Firma.TBLTRANSFER_DETAY;
                Expression<Func<TBLTRANSFER_DETAY, bool>> predicate = (Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.TRANSFER_ID == (long?) Convert.ToInt64(item));
                foreach (TBLTRANSFER_DETAY tbltransferDetay2 in tbltransferDetay1.Where<TBLTRANSFER_DETAY>(predicate).ToList<TBLTRANSFER_DETAY>())
                {
                  MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM TBLTRANSFER_DETAY WHERE ID=" + tbltransferDetay2.ID.ToString());
                  MyUtils.Firma.SaveChanges();
                }
                if (!Directory.Exists("Bozuk Paketler"))
                  Directory.CreateDirectory("Bozuk Paketler");
                xmlDocument.Save(Application.StartupPath + "\\Bozuk Paketler\\" + item + ".xml");
                stringBuilder.AppendLine("Hata Oluştu : Transfer ID :" + item + " Dosya :" + item + ".xml");
              }
            }
          }
        }
      }
      Cursor.Current = Cursors.Default;
      int num1 = (int) MessageBox.Show("Güncelleme tamamlanmıştır.");
      if (stringBuilder.Length <= 0)
        return;
      int num2 = (int) new FrmKoliSonuc()
      {
        Sonuc = stringBuilder.ToString()
      }.ShowDialog();
    }

    private void btnDosyaImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Paket Dosyası (*.xml)|*.xml";
      StringBuilder stringBuilder = new StringBuilder();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      string xml = File.ReadAllText(openFileDialog.FileName);
      XmlDocument xmlDocument = new XmlDocument();
      TBLTRANSFER entity1 = new TBLTRANSFER();
      try
      {
        Application.DoEvents();
        xmlDocument.LoadXml(xml);
        XmlElement documentElement = xmlDocument.DocumentElement;
        this.dNumber = documentElement.SelectSingleNode("documentNumber").InnerText;
        this.dGlnNo = documentElement.SelectSingleNode("sourceGLN").InnerText;
        this.dTransferID = Convert.ToInt64(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
        this.dDate = DateTime.Now;
        try
        {
          this.dDate = Convert.ToDateTime(documentElement.SelectSingleNode("documentDate").InnerText);
        }
        catch
        {
          this.dDate = DateTime.Now;
        }
        entity1.KAYNAK_GLNNO = this.dGlnNo;
        entity1.TARIH = DateTime.Now.ToShortDateString();
        entity1.TRANSFER_ID = this.dTransferID;
        this.NodeProcess((XmlNode) documentElement, "");
        MyUtils.Firma.TBLTRANSFER.Add(entity1);
        MyUtils.Firma.SaveChanges();
        if (!Directory.Exists(Application.StartupPath + "\\Gelen Paketler"))
          Directory.CreateDirectory(Application.StartupPath + "\\Gelen Paketler");
        xmlDocument.Save(Application.StartupPath + "\\Gelen Paketler\\" + this.dGlnNo + "-" + this.dTransferID.ToString() + ".xml");
        int num = (int) MessageBox.Show("Güncelleme tamamlanmıştır.");
      }
      catch (Exception ex)
      {
        DbSet<TBLTRANSFER_DETAY> tbltransferDetay = MyUtils.Firma.TBLTRANSFER_DETAY;
        Expression<Func<TBLTRANSFER_DETAY, bool>> predicate = (Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.TRANSFER_ID == (long?) this.dTransferID & u.KAYNAK_GLNNO == this.dGlnNo);
        foreach (TBLTRANSFER_DETAY entity2 in tbltransferDetay.Where<TBLTRANSFER_DETAY>(predicate).ToList<TBLTRANSFER_DETAY>())
        {
          MyUtils.Firma.TBLTRANSFER_DETAY.Remove(entity2);
          MyUtils.Firma.SaveChanges();
        }
        if (!Directory.Exists(Application.StartupPath + "\\Bozuk Paketler"))
          Directory.CreateDirectory(Application.StartupPath + "\\Bozuk Paketler");
        xmlDocument.Save(Application.StartupPath + "\\Bozuk Paketler\\" + this.dGlnNo + "-" + this.dTransferID.ToString() + ".xml");
        stringBuilder.AppendLine("Hata Oluştu : GLNNO : " + this.dGlnNo + " Transfer ID :" + this.dTransferID.ToString() + " Dosya :" + this.dGlnNo + "-" + this.dTransferID.ToString() + ".xml (" + ex.Message + ")");
      }
      if (stringBuilder.Length > 0)
      {
        int num1 = (int) new FrmKoliSonuc()
        {
          Sonuc = stringBuilder.ToString()
        }.ShowDialog();
      }
    }

    private void FrmPaketTransfer_Load(object sender, EventArgs e)
    {
      this.dateIlk.Value = DateTime.Now.AddYears(-1);
      this.dateSon.Value = DateTime.Now;
    }

    private void btnListe_Click(object sender, EventArgs e)
    {
    }

    private void btnListe_Click_1(object sender, EventArgs e)
    {
      if (this.txtPTSNo.Text == "")
      {
        int num1 = (int) MessageBox.Show("Lütfen Pts No giriniz!");
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        StringBuilder stringBuilder = new StringBuilder();
        long ptsid = Convert.ToInt64(this.txtPTSNo.Text);
        if (MyUtils.Firma.TBLTRANSFER.Where<TBLTRANSFER>((Expression<Func<TBLTRANSFER, bool>>) (u => u.TRANSFER_ID == ptsid)).FirstOrDefault<TBLTRANSFER>() == null)
        {
          string s = BildirimHelper.PaketIndir(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), this.txtPTSNo.Text);
          if (s != null && s.Length > 0)
          {
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(s));
            MemoryStream inStream = new MemoryStream();
            using (ZipInputStream zipInputStream = new ZipInputStream((Stream) memoryStream))
            {
              while (zipInputStream.GetNextEntry() != null)
              {
                byte[] buffer = new byte[memoryStream.ToArray().Length];
                for (int count = ((Stream) zipInputStream).Read(buffer, 0, buffer.Length); count > 0; count = ((Stream) zipInputStream).Read(buffer, 0, buffer.Length))
                  inStream.Write(buffer, 0, count);
              }
            }
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
              Application.DoEvents();
              inStream.Position = 0L;
              xmlDocument.Load((Stream) inStream);
              memoryStream.Close();
              XmlElement documentElement = xmlDocument.DocumentElement;
              this.dNumber = documentElement.SelectSingleNode("documentNumber").InnerText;
              this.dGlnNo = documentElement.SelectSingleNode("sourceGLN").InnerText;
              this.dTransferID = Convert.ToInt64(this.txtPTSNo.Text);
              DateTime dateTime = DateTime.Now;
              try
              {
                dateTime = DateTime.ParseExact(documentElement.SelectSingleNode("documentDate").InnerText, "yyyy-MM-dd", (IFormatProvider) null);
              }
              catch
              {
                dateTime = DateTime.Now;
              }
              if (!Directory.Exists("Gelen Paketler"))
                Directory.CreateDirectory("Gelen Paketler");
              TBLTRANSFER entity = new TBLTRANSFER();
              entity.KAYNAK_GLNNO = this.dGlnNo;
              entity.TARIH = DateTime.Now.ToShortDateString();
              entity.TRANSFER_ID = Convert.ToInt64(this.txtPTSNo.Text);
              this.NodeProcess((XmlNode) documentElement, "");
              MyUtils.Firma.TBLTRANSFER.Add(entity);
              MyUtils.Firma.SaveChanges();
              xmlDocument.Save(Application.StartupPath + "\\Gelen Paketler\\" + this.dGlnNo + "-" + this.txtPTSNo.Text + ".xml");
            }
            catch
            {
              DbSet<TBLTRANSFER_DETAY> tbltransferDetay1 = MyUtils.Firma.TBLTRANSFER_DETAY;
              Expression<Func<TBLTRANSFER_DETAY, bool>> predicate = (Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.TRANSFER_ID == (long?) Convert.ToInt64(this.txtPTSNo.Text));
              foreach (TBLTRANSFER_DETAY tbltransferDetay2 in tbltransferDetay1.Where<TBLTRANSFER_DETAY>(predicate).ToList<TBLTRANSFER_DETAY>())
              {
                MyUtils.Firma.Database.ExecuteSqlCommand("DELETE FROM TBLTRANSFER_DETAY WHERE ID=" + tbltransferDetay2.ID.ToString());
                MyUtils.Firma.SaveChanges();
              }
              if (!Directory.Exists("Bozuk Paketler"))
                Directory.CreateDirectory("Bozuk Paketler");
              xmlDocument.Save(Application.StartupPath + "\\Bozuk Paketler\\" + this.txtPTSNo.Text + ".xml");
              stringBuilder.AppendLine("Hata Oluştu : GLNNO : " + this.txtPTSNo.Text + " Transfer ID :" + this.txtPTSNo.Text + " Dosya :" + this.txtPTSNo.Text + ".xml");
            }
          }
        }
        Cursor.Current = Cursors.Default;
        int num2 = (int) MessageBox.Show("Güncelleme tamamlanmıştır.");
        if (stringBuilder.Length <= 0)
          return;
        int num3 = (int) new FrmKoliSonuc()
        {
          Sonuc = stringBuilder.ToString()
        }.ShowDialog();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPaketTransfer));
      this.detay = new ProgressBar();
      this.master = new ProgressBar();
      this.panel2 = new Panel();
      this.dateSon = new DateTimePicker();
      this.label2 = new Label();
      this.dateIlk = new DateTimePicker();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.btnDosyaImport = new Button();
      this.btnBasla = new Button();
      this.imgList = new ImageList(this.components);
      this.btnListe = new Button();
      this.txtPTSNo = new TextBox();
      this.label3 = new Label();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.detay.Dock = DockStyle.Top;
      this.detay.Location = new Point(0, 70);
      this.detay.Name = "detay";
      this.detay.Size = new Size(604, 37);
      this.detay.TabIndex = 9;
      this.master.Dock = DockStyle.Top;
      this.master.Location = new Point(0, 35);
      this.master.Name = "master";
      this.master.Size = new Size(604, 35);
      this.master.TabIndex = 8;
      this.panel2.Controls.Add((Control) this.btnListe);
      this.panel2.Controls.Add((Control) this.txtPTSNo);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.dateSon);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.dateIlk);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(604, 35);
      this.panel2.TabIndex = 7;
      this.dateSon.Format = DateTimePickerFormat.Short;
      this.dateSon.Location = new Point(216, 6);
      this.dateSon.Name = "dateSon";
      this.dateSon.Size = new Size(102, 20);
      this.dateSon.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(161, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(53, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Son Tarih";
      this.dateIlk.Format = DateTimePickerFormat.Short;
      this.dateIlk.Location = new Point(54, 6);
      this.dateIlk.Name = "dateIlk";
      this.dateIlk.Size = new Size(102, 20);
      this.dateIlk.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "İlk Tarih";
      this.panel1.Controls.Add((Control) this.btnDosyaImport);
      this.panel1.Controls.Add((Control) this.btnBasla);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 107);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(604, 34);
      this.panel1.TabIndex = 10;
      this.btnDosyaImport.Dock = DockStyle.Fill;
      this.btnDosyaImport.Location = new Point(0, 0);
      this.btnDosyaImport.Name = "btnDosyaImport";
      this.btnDosyaImport.Size = new Size(248, 34);
      this.btnDosyaImport.TabIndex = 5;
      this.btnDosyaImport.Text = "Paket Dosyası Aç";
      this.btnDosyaImport.UseVisualStyleBackColor = true;
      this.btnDosyaImport.Click += new EventHandler(this.btnDosyaImport_Click);
      this.btnBasla.Dock = DockStyle.Right;
      this.btnBasla.Location = new Point(248, 0);
      this.btnBasla.Name = "btnBasla";
      this.btnBasla.Size = new Size(356, 34);
      this.btnBasla.TabIndex = 1;
      this.btnBasla.Text = "Transfer Et";
      this.btnBasla.UseVisualStyleBackColor = true;
      this.btnBasla.Click += new EventHandler(this.btnBasla_Click);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "button_ok16.png");
      this.imgList.Images.SetKeyName(1, "button_cancel2.png");
      this.btnListe.Location = new Point(533, 6);
      this.btnListe.Name = "btnListe";
      this.btnListe.Size = new Size(47, 22);
      this.btnListe.TabIndex = 21;
      this.btnListe.Text = "İndir";
      this.btnListe.UseVisualStyleBackColor = true;
      this.btnListe.Click += new EventHandler(this.btnListe_Click_1);
      this.txtPTSNo.Location = new Point(383, 6);
      this.txtPTSNo.Name = "txtPTSNo";
      this.txtPTSNo.Size = new Size(144, 20);
      this.txtPTSNo.TabIndex = 20;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(332, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(45, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "PTS No";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(604, 140);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.detay);
      this.Controls.Add((Control) this.master);
      this.Controls.Add((Control) this.panel2);
      this.Name = nameof (FrmPaketTransfer);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Paket Transfer";
      this.Load += new EventHandler(this.FrmPaketTransfer_Load);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public class Ptsler
    {
      public string GlnNo { get; set; }

      public List<string> Transferler { get; set; }
    }
  }
}
