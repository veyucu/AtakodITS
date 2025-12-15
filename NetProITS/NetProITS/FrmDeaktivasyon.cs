// Decompiled with JetBrains decompiler
// Type: NetProITS.FrmDeaktivasyon
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  public class FrmDeaktivasyon : Form
  {
    private BILDIRIM AktifEvrak;
    private List<BILDIRIM_STOK> AktifKalemler;
    private int EvrakID;
    private string EvrakNo;
    private string CariKod;
    private EvrakTipi EvrakTip;
    private IContainer components = (IContainer) null;
    private DefaultLookAndFeel defaultLookAndFeel1;
    private PanelControl panelControl1;
    private XtraTabControl xtraTabControl1;
    private XtraTabPage xtraTabPage1;
    private XtraTabPage xtraTabPage2;
    private PanelControl panelControl2;
    private SimpleButton simpleButton1;
    private DateEdit tarih;
    private LabelControl labelControl2;
    private PanelControl panelControl3;
    private GridControl grdSiparis;
    private GridView gridView1;
    private GridColumn colRECNO;
    private GridColumn colTARIH;
    private GridColumn colEVRAK_SERI;
    private GridColumn colCARI_KOD;
    private GridColumn colCARI_UNVAN;
    private PanelControl pnlUst;
    private PanelControl panelControl5;
    private GridControl grdKalem;
    private GridView gridView2;
    private GridColumn colRECNO1;
    private GridColumn colCARIHR_RECNO;
    private GridColumn gridColumn1;
    private GridColumn colSTOK_KODU;
    private GridColumn colSTOK_ISIM;
    private GridColumn colMIKTAR;
    private GridColumn colKARSILANAN;
    private GridColumn gridColumn2;
    private PanelControl panelControl4;
    private SimpleButton btnBildirim;
    private SimpleButton btnKarekod;
    private Label lblDurum;
    private XtraTabPage xtraTabPage3;
    private PanelControl panelControl7;
    private GridControl grdKalemDetay;
    private GridView gridView3;
    private GridColumn colID;
    private GridColumn colTIP;
    private GridColumn colCARIHR_RECNO1;
    private GridColumn colSTOK_KOD;
    private GridColumn colSTOK_ISIM1;
    private GridColumn colBARKOD;
    private GridColumn colSERI_NO;
    private GridColumn colMIAD;
    private GridColumn colPARTINO;
    private GridColumn colDURUM;
    private PanelControl panelControl6;
    private MemoEdit memSonuc;
    private SplitterControl splitterControl1;
    private CheckBox chkBildirim;
    private Button button1;
    private SimpleButton btnImport;
    private TextEdit txtEvrakSeri;
    private LabelControl labelControl3;
    private GridColumn gridColumn3;
    private System.Windows.Forms.ComboBox cbTip;
    private SimpleButton btnExcelAktar;
    private Panel pnlDeak;
    private SimpleButton btnSeriAktar;
    private Label label1;
    private System.Windows.Forms.ComboBox cbDeak;

    public FrmDeaktivasyon() => this.InitializeComponent();

    private void simpleButton1_Click(object sender, EventArgs e)
    {
      if (this.cbTip.SelectedIndex <= -1)
        return;
      List<BILDIRIM> list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == "")).ToList<BILDIRIM>();
      DateTime dateTime = this.tarih.DateTime;
      if (true)
      {
        if (this.txtEvrakSeri.Text.Length > 0)
        {
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.SATIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.DEAKTIVASYON:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.EVRAK_NO == this.txtEvrakSeri.Text & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD == MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
          }
        }
        else if (this.chkBildirim.Checked)
        {
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.SATIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.DEAKTIVASYON:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "OK" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD == MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
          }
        }
        else
        {
          switch (this.EvrakTip)
          {
            case EvrakTipi.SATIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD != MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 2)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.SATIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 3)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.ALIS_IPTAL:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 4)).ToList<BILDIRIM>();
              break;
            case EvrakTipi.DEAKTIVASYON:
              list = MyUtils.Firma.BILDIRIM.Where<BILDIRIM>((Expression<Func<BILDIRIM, bool>>) (u => u.DURUM == "" & u.TARIH == this.tarih.DateTime & u.TIP == (int?) 1 & u.CARI_KOD == MyUtils.GetParamValue("Deaktivasyon"))).ToList<BILDIRIM>();
              break;
          }
        }
        this.grdSiparis.DataSource = (object) list;
      }
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      this.AktifEvrak = this.gridView1.GetRow(this.gridView1.FocusedRowHandle) as BILDIRIM;
      this.EvrakNo = this.AktifEvrak.EVRAK_NO;
      this.CariKod = this.AktifEvrak.CARI_KOD;
      this.EvrakID = this.AktifEvrak.TIP.Value;
      MyUtils.Refresh();
      this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
      this.grdKalem.DataSource = (object) this.AktifKalemler;
      this.grdKalemDetay.DataSource = (object) MyUtils.Firma.ITSHAR_VIEW.Where<ITSHAR_VIEW>((Expression<Func<ITSHAR_VIEW, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).ToList<ITSHAR_VIEW>();
      this.lblDurum.Text = "Evrak : " + this.EvrakNo + " " + this.AktifEvrak.CARI_UNVAN;
      this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
    }

    private void gridControl1_Click(object sender, EventArgs e)
    {
    }

    private void btnKarekod_Click(object sender, EventArgs e)
    {
      FrmKareKod karekod;
      while (true)
      {
        karekod = new FrmKareKod();
        if (karekod.ShowDialog() == DialogResult.OK)
        {
          double? nullable1;
          int? karsilanan;
          double? nullable2;
          if (karekod.chkKolii)
          {
            string koli = karekod.strKoliBarkod;
            StringBuilder stringBuilder = new StringBuilder();
            List<TBLTRANSFER_DETAY> list1 = MyUtils.Firma.TBLTRANSFER_DETAY.Where<TBLTRANSFER_DETAY>((Expression<Func<TBLTRANSFER_DETAY, bool>>) (u => u.CARRIER_LABEL == koli)).ToList<TBLTRANSFER_DETAY>();
            if (list1.Count > 0)
            {
              foreach (TBLTRANSFER_DETAY tbltransferDetay in list1)
              {
                TBLTRANSFER_DETAY item = tbltransferDetay;
                Application.DoEvents();
                string barkod = item.GTIN;
                barkod = barkod.Remove(0, 1);
                List<BILDIRIM_STOK> list2 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).ToList<BILDIRIM_STOK>();
                if (list2.Count > 0)
                {
                  nullable1 = list2[0].MIKTAR;
                  karsilanan = list2[0].KARSILANAN;
                  nullable2 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
                  if (nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault() && nullable1.HasValue & nullable2.HasValue)
                  {
                    if (MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == item.GTIN & u.EVRAK_SERI == this.EvrakNo & u.SERI_NO == item.SERIAL_NUMBER & u.PARTINO == item.LOT_NUMBER & u.MIAD == item.DATE & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>().Count == 0)
                    {
                      MyUtils.HareketEkle(item.GTIN, this.EvrakNo, this.CariKod, "", item.DATE, item.LOT_NUMBER, item.SERIAL_NUMBER, list2[0].STOK_KODU, this.EvrakID);
                      this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                      this.grdKalem.DataSource = (object) this.AktifKalemler;
                    }
                    else
                      stringBuilder.AppendLine(item.GTIN + " | " + item.SERIAL_NUMBER + " | " + item.LOT_NUMBER + " | " + item.DATE);
                  }
                  else
                    stringBuilder.AppendLine(item.GTIN + " | " + item.SERIAL_NUMBER + " | " + item.LOT_NUMBER + " | " + item.DATE);
                }
                else
                  stringBuilder.AppendLine(item.GTIN + " | " + item.SERIAL_NUMBER + " | " + item.LOT_NUMBER + " | " + item.DATE);
              }
            }
            if (stringBuilder.Length > 1)
            {
              FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
              int num = (int) frmKoliSonuc.ShowDialog();
              frmKoliSonuc.Dispose();
            }
          }
          else if (karekod.strGtin.Length > 0 & karekod.strMiad.Length > 0 & karekod.strPartiNo.Length > 0 & karekod.strseriNo.Length > 0)
          {
            if (karekod.strGtin.Length > 0)
            {
              string barkod = karekod.strGtin;
              barkod = barkod.Remove(0, 1);
              List<BILDIRIM_STOK> list3 = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod)).ToList<BILDIRIM_STOK>();
              if (list3.Count > 0)
              {
                nullable2 = list3[0].MIKTAR;
                karsilanan = list3[0].KARSILANAN;
                nullable1 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
                if (nullable2.GetValueOrDefault() > nullable1.GetValueOrDefault() && nullable2.HasValue & nullable1.HasValue)
                {
                  List<ITSHAR> list4 = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == karekod.strGtin & u.EVRAK_SERI == this.EvrakNo & u.SERI_NO == karekod.strseriNo & u.PARTINO == karekod.strPartiNo & u.MIAD == karekod.strMiad & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>();
                  if (list4.Count == 0)
                  {
                    MyUtils.HareketEkle(karekod.strGtin, this.EvrakNo, this.CariKod, "", karekod.strMiad, karekod.strPartiNo, karekod.strseriNo, list3[0].STOK_KODU, this.EvrakID);
                    this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                    this.grdKalem.DataSource = (object) this.AktifKalemler;
                  }
                  else
                  {
                    Console.Beep(1000, 1000);
                    if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                      ITSHAR entity = list4[0];
                      MyUtils.Firma.ITSHAR.Remove(entity);
                      MyUtils.Firma.SaveChanges();
                      MyUtils.Refresh();
                      this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                      this.grdKalem.DataSource = (object) this.AktifKalemler;
                    }
                  }
                }
                else
                {
                  List<ITSHAR> list5 = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == karekod.strGtin & u.EVRAK_SERI == this.EvrakNo & u.SERI_NO == karekod.strseriNo & u.PARTINO == karekod.strPartiNo & u.MIAD == karekod.strMiad & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>();
                  if (list5.Count > 0)
                  {
                    Console.Beep(1000, 1000);
                    if (MessageBox.Show("Bu Ürün okutulmuştur...\r Silmek İstermisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                      ITSHAR entity = list5[0];
                      MyUtils.Firma.ITSHAR.Remove(entity);
                      MyUtils.Firma.SaveChanges();
                      MyUtils.Refresh();
                      this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                      this.grdKalem.DataSource = (object) this.AktifKalemler;
                    }
                  }
                  else
                  {
                    Console.Beep(1000, 1000);
                    int num = (int) MessageBox.Show("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
                  }
                }
              }
              else
              {
                Console.Beep(1000, 1000);
                int num = (int) MessageBox.Show("Bu ürün bu siparişde yoktur...");
              }
            }
          }
          else if (karekod.strBarkod.Length > 0)
          {
            List<BILDIRIM_STOK> list = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == karekod.strBarkod)).ToList<BILDIRIM_STOK>();
            if (list.Count > 0 && list[0].REYON_KODU != "BESERI")
            {
              nullable1 = list[0].MIKTAR;
              karsilanan = list[0].KARSILANAN;
              nullable2 = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
              if (nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault() && nullable1.HasValue & nullable2.HasValue)
              {
                MyUtils.HareketEkle("0" + karekod.strBarkod, this.EvrakNo, this.CariKod, "", "", "", "", list[0].STOK_KODU, this.EvrakID);
                this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                this.grdKalem.DataSource = (object) this.AktifKalemler;
              }
              else
              {
                Console.Beep(1000, 1000);
                int num = (int) MessageBox.Show("Sipariş miktarı tamamlanmıştır...Lütfen okutmayınız...");
              }
            }
          }
          karekod.Dispose();
        }
        else
          break;
      }
      karekod.Dispose();
    }

    private void btnBitir_Click(object sender, EventArgs e)
    {
      try
      {
        string deakivasyonkodu = "";
        if (this.EvrakTip == EvrakTipi.DEAKTIVASYON)
        {
          if (this.cbDeak.SelectedIndex < 0)
          {
            int num = (int) MessageBox.Show("Lütfen deaktivasyon tipini seçiniz!");
            return;
          }
          switch (this.cbDeak.SelectedIndex)
          {
            case 0:
              deakivasyonkodu = "10";
              break;
            case 1:
              deakivasyonkodu = "20";
              break;
            case 2:
              deakivasyonkodu = "30";
              break;
            case 3:
              deakivasyonkodu = "40";
              break;
            case 4:
              deakivasyonkodu = "50";
              break;
            case 5:
              deakivasyonkodu = "60";
              break;
          }
        }
        if (this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.REYON_KODU == "BESERI")).Sum<BILDIRIM_STOK>((Func<BILDIRIM_STOK, double?>) (u => u.KALAN)).Value > 0.0 && MessageBox.Show("Okutulmayan kalemler mevcut\r Bildirim yapmak istediğinize eminmisiniz?", "Bildirim Uyarı", MessageBoxButtons.YesNo) == DialogResult.No)
          return;
        this.Cursor = Cursors.WaitCursor;
        MyUtils.Refresh();
        int num1 = 0;
        int num2 = 0;
        List<FrmDeaktivasyon.HataListe> source = new List<FrmDeaktivasyon.HataListe>();
        foreach (HATA_KODLARI hataKodlari in (IEnumerable<HATA_KODLARI>) MyUtils.Firma.HATA_KODLARI)
          source.Add(new FrmDeaktivasyon.HataListe()
          {
            HataKod = hataKodlari.HATAID,
            Ad = 0,
            Durum = hataKodlari.MESAJ
          });
        List<ITSHAR> list = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo & u.TIP == (int?) this.EvrakID & u.BARKOD != "" & u.SERI_NO != "" & u.PARTINO != "")).ToList<ITSHAR>();
        List<KarekodBilgi> its = new List<KarekodBilgi>();
        foreach (ITSHAR itshar in list)
        {
          string miad = BildirimHelper.ConvertMiadTarih(itshar.MIAD).ToString("yyyy-MM-dd");
          its.Add(new KarekodBilgi(itshar.BARKOD, itshar.SERI_NO, itshar.PARTINO, miad));
        }
        List<KarekodBilgi> karekodBilgiList1 = new List<KarekodBilgi>();
        if (this.EvrakTip == EvrakTipi.DEAKTIVASYON)
        {
          List<KarekodBilgi> karekodBilgiList2 = BildirimHelper.DeakivasyonBildirimi(MyUtils.GetParamValue("KullaniciAdi"), MyUtils.GetParamValue("Sifre"), MyUtils.GetParamValue("GlnNo"), deakivasyonkodu, this.cbDeak.SelectedText, its);
          string fsip4 = this.AktifEvrak.FTIRSIP;
          string fatno4 = this.AktifEvrak.EVRAK_NO;
          MyUtils.Firma.TBLFATUIRS.Where<TBLFATUIRS>((Expression<Func<TBLFATUIRS, bool>>) (u => u.FTIRSIP == fsip4 && u.FATIRS_NO == fatno4)).First<TBLFATUIRS>().S_YEDEK1 = "OK";
          MyUtils.Firma.SaveChanges();
          foreach (KarekodBilgi karekodBilgi in karekodBilgiList2)
          {
            KarekodBilgi item = karekodBilgi;
            item.Barkod.Remove(0, 1);
            ITSHAR h = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == item.Barkod & u.SERI_NO == item.SeriNo & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).ToList<ITSHAR>()[0];
            if (h != null)
            {
              h.DURUM = item.Sonuc;
              if (item.Sonuc == "00000")
                ++num1;
              source.Where<FrmDeaktivasyon.HataListe>((Func<FrmDeaktivasyon.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmDeaktivasyon.HataListe>()[0].Ad = source.Where<FrmDeaktivasyon.HataListe>((Func<FrmDeaktivasyon.HataListe, bool>) (u => u.HataKod == Convert.ToInt32(h.DURUM))).ToList<FrmDeaktivasyon.HataListe>()[0].Ad + 1;
              MyUtils.Firma.SaveChanges();
            }
            ++num2;
          }
        }
        MyUtils.Refresh();
        this.grdKalemDetay.DataSource = (object) MyUtils.Firma.ITSHAR_VIEW.Where<ITSHAR_VIEW>((Expression<Func<ITSHAR_VIEW, bool>>) (u => u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod & u.EVRAK_SERI == this.EvrakNo)).OrderBy<ITSHAR_VIEW, string>((Expression<Func<ITSHAR_VIEW, string>>) (u => u.BARKOD)).ThenBy<ITSHAR_VIEW, string>((Expression<Func<ITSHAR_VIEW, string>>) (u => u.PARTINO)).ThenBy<ITSHAR_VIEW, string>((Expression<Func<ITSHAR_VIEW, string>>) (u => u.MIAD)).ToList<ITSHAR_VIEW>();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Bildirim Detayları : ");
        foreach (FrmDeaktivasyon.HataListe hataListe in source)
        {
          if (hataListe.Ad > 0)
          {
            stringBuilder.AppendLine(hataListe.Ad.ToString() + " = " + hataListe.Durum);
            hataListe.Ad = 0;
          }
        }
        stringBuilder.AppendLine("Bildirim Durumu : " + num1.ToString() + " / " + num2.ToString());
        this.memSonuc.Text = stringBuilder.ToString();
        this.Cursor = Cursors.Default;
        this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
        if (this.EvrakTip != EvrakTipi.SATIS || num1 != num2 || MessageBox.Show("Paket gönderimi yapılsın mı?", "Paket Uyarı", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        this.btnPaketAl_Click(sender, e);
      }
      catch (Exception ex)
      {
        this.Cursor = Cursors.Default;
        int num = (int) MessageBox.Show("Hata oluştu :" + ex.Message);
      }
    }

    private void btnPaketAl_Click(object sender, EventArgs e)
    {
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Text Files (*.txt)|*.txt";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      string[] strArray = new StringBuilder(File.ReadAllText(openFileDialog.FileName)).ToString().Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length; ++index)
      {
        Application.DoEvents();
        if (strArray[index].Split('|').Length > 2)
        {
          string barkod1 = strArray[index].Split('|')[0];
          string barkod = "0" + strArray[index].Split('|')[0];
          string SeriNo = strArray[index].Split('|')[1];
          string Miad = strArray[index].Split('|')[2];
          string PartiNo = strArray[index].Split('|')[3];
          List<BILDIRIM_STOK> list = this.AktifKalemler.Where<BILDIRIM_STOK>((Func<BILDIRIM_STOK, bool>) (u => u.BARKOD == barkod1)).ToList<BILDIRIM_STOK>();
          if (list.Count > 0)
          {
            double? miktar = list[0].MIKTAR;
            int? karsilanan = list[0].KARSILANAN;
            double? nullable = karsilanan.HasValue ? new double?((double) karsilanan.GetValueOrDefault()) : new double?();
            if (miktar.GetValueOrDefault() > nullable.GetValueOrDefault() && miktar.HasValue & nullable.HasValue)
            {
              if (MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.BARKOD == barkod & u.EVRAK_SERI == this.EvrakNo & u.SERI_NO == SeriNo & u.PARTINO == PartiNo & u.MIAD == Miad & u.TIP == (int?) this.EvrakID & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>().Count == 0)
              {
                MyUtils.HareketEkle(barkod, this.EvrakNo, this.CariKod, "", Miad, PartiNo, SeriNo, list[0].STOK_KODU, this.EvrakID);
                this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
                this.grdKalem.DataSource = (object) this.AktifKalemler;
              }
              else
                stringBuilder.AppendLine(strArray[index]);
            }
            else
              stringBuilder.AppendLine(strArray[index]);
          }
          else
            stringBuilder.AppendLine(strArray[index]);
        }
      }
      int num1 = (int) MessageBox.Show("Aktarım tamamlanmıştır.");
      if (stringBuilder.Length > 1)
      {
        FrmKoliSonuc frmKoliSonuc = new FrmKoliSonuc();
        int num2 = (int) frmKoliSonuc.ShowDialog();
        frmKoliSonuc.Dispose();
      }
    }

    private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void cbTip_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cbTip.SelectedIndex)
      {
        case 0:
          this.EvrakTip = EvrakTipi.SATIS;
          break;
        case 1:
          this.EvrakTip = EvrakTipi.ALIS;
          break;
        case 2:
          this.EvrakTip = EvrakTipi.SATIS_IPTAL;
          break;
        case 3:
          this.EvrakTip = EvrakTipi.ALIS_IPTAL;
          break;
        case 4:
          this.EvrakTip = EvrakTipi.DEAKTIVASYON;
          break;
      }
    }

    private void btnExcelAktar_Click(object sender, EventArgs e)
    {
      try
      {
        this.grdKalem.ExportToXls(this.AktifEvrak.EVRAK_NO + ".xls");
        int num = (int) MessageBox.Show("Dosya aktarılmıştır.");
      }
      catch
      {
      }
    }

    private void gridView2_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        string StokKodu = this.gridView2.GetRowCellValue(this.gridView2.FocusedRowHandle, "STOK_KODU").ToString();
        List<ITSHAR> list = MyUtils.Firma.ITSHAR.Where<ITSHAR>((Expression<Func<ITSHAR, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.TIP == (int?) this.EvrakID & u.STOK_KOD == StokKodu & u.CARI_KOD == this.CariKod)).ToList<ITSHAR>();
        FrmKalemDetay frmKalemDetay = new FrmKalemDetay();
        frmKalemDetay.kalemTipi = KalemTipi.ITS;
        frmKalemDetay.itsHar = (IEnumerable<ITSHAR>) list;
        int num = (int) frmKalemDetay.ShowDialog();
        frmKalemDetay.Dispose();
        MyUtils.Refresh();
        this.AktifKalemler = MyUtils.Firma.BILDIRIM_STOK.Where<BILDIRIM_STOK>((Expression<Func<BILDIRIM_STOK, bool>>) (u => u.EVRAK_SERI == this.EvrakNo & u.CARI_KOD == this.CariKod & u.TIP == (int?) this.EvrakID)).OrderBy<BILDIRIM_STOK, long?>((Expression<Func<BILDIRIM_STOK, long?>>) (u => u.RECNO)).ToList<BILDIRIM_STOK>();
        this.grdKalem.DataSource = (object) this.AktifKalemler;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnSeriAktar_Click(object sender, EventArgs e)
    {
      try
      {
        this.grdKalemDetay.ExportToText(this.AktifEvrak.EVRAK_NO + ".txt", "|");
        int num = (int) MessageBox.Show("Dosya aktarılmıştır.");
      }
      catch
      {
      }
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
      if (!(this.gridView2.GetRow(e.RowHandle) is BILDIRIM_STOK row))
        return;
      double? kalan = row.KALAN;
      double num = 0.0;
      if (kalan.GetValueOrDefault() == num && kalan.HasValue)
      {
        e.Appearance.BackColor = Color.Green;
        e.Appearance.ForeColor = Color.White;
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
      this.defaultLookAndFeel1 = new DefaultLookAndFeel(this.components);
      this.panelControl1 = new PanelControl();
      this.cbTip = new System.Windows.Forms.ComboBox();
      this.xtraTabControl1 = new XtraTabControl();
      this.xtraTabPage1 = new XtraTabPage();
      this.panelControl3 = new PanelControl();
      this.grdSiparis = new GridControl();
      this.gridView1 = new GridView();
      this.colRECNO = new GridColumn();
      this.colTARIH = new GridColumn();
      this.colEVRAK_SERI = new GridColumn();
      this.colCARI_KOD = new GridColumn();
      this.colCARI_UNVAN = new GridColumn();
      this.panelControl2 = new PanelControl();
      this.txtEvrakSeri = new TextEdit();
      this.labelControl3 = new LabelControl();
      this.chkBildirim = new CheckBox();
      this.simpleButton1 = new SimpleButton();
      this.tarih = new DateEdit();
      this.labelControl2 = new LabelControl();
      this.xtraTabPage2 = new XtraTabPage();
      this.panelControl5 = new PanelControl();
      this.grdKalem = new GridControl();
      this.gridView2 = new GridView();
      this.colRECNO1 = new GridColumn();
      this.colCARIHR_RECNO = new GridColumn();
      this.gridColumn1 = new GridColumn();
      this.colSTOK_KODU = new GridColumn();
      this.colSTOK_ISIM = new GridColumn();
      this.colMIKTAR = new GridColumn();
      this.colKARSILANAN = new GridColumn();
      this.gridColumn2 = new GridColumn();
      this.gridColumn3 = new GridColumn();
      this.button1 = new Button();
      this.panelControl4 = new PanelControl();
      this.pnlDeak = new Panel();
      this.label1 = new Label();
      this.cbDeak = new System.Windows.Forms.ComboBox();
      this.btnSeriAktar = new SimpleButton();
      this.btnExcelAktar = new SimpleButton();
      this.btnImport = new SimpleButton();
      this.btnBildirim = new SimpleButton();
      this.btnKarekod = new SimpleButton();
      this.pnlUst = new PanelControl();
      this.lblDurum = new Label();
      this.xtraTabPage3 = new XtraTabPage();
      this.splitterControl1 = new SplitterControl();
      this.panelControl7 = new PanelControl();
      this.grdKalemDetay = new GridControl();
      this.gridView3 = new GridView();
      this.colID = new GridColumn();
      this.colTIP = new GridColumn();
      this.colCARIHR_RECNO1 = new GridColumn();
      this.colSTOK_KOD = new GridColumn();
      this.colSTOK_ISIM1 = new GridColumn();
      this.colBARKOD = new GridColumn();
      this.colSERI_NO = new GridColumn();
      this.colMIAD = new GridColumn();
      this.colPARTINO = new GridColumn();
      this.colDURUM = new GridColumn();
      this.panelControl6 = new PanelControl();
      this.memSonuc = new MemoEdit();
      this.panelControl1.BeginInit();
      this.panelControl1.SuspendLayout();
      this.xtraTabControl1.BeginInit();
      this.xtraTabControl1.SuspendLayout();
      this.xtraTabPage1.SuspendLayout();
      this.panelControl3.BeginInit();
      this.panelControl3.SuspendLayout();
      this.grdSiparis.BeginInit();
      this.gridView1.BeginInit();
      this.panelControl2.BeginInit();
      this.panelControl2.SuspendLayout();
      this.txtEvrakSeri.Properties.BeginInit();
      this.tarih.Properties.VistaTimeProperties.BeginInit();
      this.tarih.Properties.BeginInit();
      this.xtraTabPage2.SuspendLayout();
      this.panelControl5.BeginInit();
      this.panelControl5.SuspendLayout();
      this.grdKalem.BeginInit();
      this.gridView2.BeginInit();
      this.panelControl4.BeginInit();
      this.panelControl4.SuspendLayout();
      this.pnlDeak.SuspendLayout();
      this.pnlUst.BeginInit();
      this.pnlUst.SuspendLayout();
      this.xtraTabPage3.SuspendLayout();
      this.panelControl7.BeginInit();
      this.panelControl7.SuspendLayout();
      this.grdKalemDetay.BeginInit();
      this.gridView3.BeginInit();
      this.panelControl6.BeginInit();
      this.panelControl6.SuspendLayout();
      this.memSonuc.Properties.BeginInit();
      this.SuspendLayout();
      this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2010 Silver";
      this.panelControl1.Controls.Add((Control) this.cbTip);
      this.panelControl1.Dock = DockStyle.Top;
      this.panelControl1.Location = new Point(0, 0);
      this.panelControl1.LookAndFeel.SkinName = "Office 2010 Black";
      this.panelControl1.Name = "panelControl1";
      this.panelControl1.Size = new Size(924, 40);
      this.panelControl1.TabIndex = 0;
      this.cbTip.Dock = DockStyle.Fill;
      this.cbTip.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbTip.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.cbTip.FormattingEnabled = true;
      this.cbTip.Items.AddRange(new object[5]
      {
        (object) "Satış Bildirimi",
        (object) "Alış Bildirimi",
        (object) "Satış İptal Bildirimi",
        (object) "Alış İptal Bildirimi",
        (object) "Deaktivasyon"
      });
      this.cbTip.Location = new Point(2, 2);
      this.cbTip.Name = "cbTip";
      this.cbTip.Size = new Size(920, 37);
      this.cbTip.TabIndex = 3;
      this.cbTip.SelectedIndexChanged += new EventHandler(this.cbTip_SelectedIndexChanged);
      this.xtraTabControl1.Dock = DockStyle.Fill;
      this.xtraTabControl1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.xtraTabControl1.HeaderAutoFill = DefaultBoolean.False;
      this.xtraTabControl1.Location = new Point(0, 40);
      this.xtraTabControl1.LookAndFeel.SkinName = "VS2010";
      this.xtraTabControl1.Name = "xtraTabControl1";
      this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
      this.xtraTabControl1.Size = new Size(924, 470);
      this.xtraTabControl1.TabIndex = 2;
      this.xtraTabControl1.TabPages.AddRange(new XtraTabPage[3]
      {
        this.xtraTabPage1,
        this.xtraTabPage2,
        this.xtraTabPage3
      });
      this.xtraTabPage1.Controls.Add((Control) this.panelControl3);
      this.xtraTabPage1.Controls.Add((Control) this.panelControl2);
      this.xtraTabPage1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.xtraTabPage1.Name = "xtraTabPage1";
      this.xtraTabPage1.Size = new Size(918, 442);
      this.xtraTabPage1.Text = "Faturalar";
      this.panelControl3.Controls.Add((Control) this.grdSiparis);
      this.panelControl3.Dock = DockStyle.Fill;
      this.panelControl3.Location = new Point(0, 33);
      this.panelControl3.Name = "panelControl3";
      this.panelControl3.Size = new Size(918, 409);
      this.panelControl3.TabIndex = 2;
      this.grdSiparis.Dock = DockStyle.Fill;
      this.grdSiparis.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdSiparis.Location = new Point(2, 2);
      this.grdSiparis.MainView = (BaseView) this.gridView1;
      this.grdSiparis.Name = "grdSiparis";
      this.grdSiparis.Size = new Size(914, 405);
      this.grdSiparis.TabIndex = 1;
      this.grdSiparis.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView1
      });
      this.gridView1.Appearance.EvenRow.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.EvenRow.Options.UseFont = true;
      this.gridView1.Appearance.FocusedRow.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.FocusedRow.Options.UseFont = true;
      this.gridView1.Appearance.OddRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.OddRow.Options.UseFont = true;
      this.gridView1.Appearance.Row.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.Row.Options.UseFont = true;
      this.gridView1.Appearance.SelectedRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView1.Appearance.SelectedRow.Options.UseFont = true;
      this.gridView1.Columns.AddRange(new GridColumn[5]
      {
        this.colRECNO,
        this.colTARIH,
        this.colEVRAK_SERI,
        this.colCARI_KOD,
        this.colCARI_UNVAN
      });
      this.gridView1.GridControl = this.grdSiparis;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView1.OptionsBehavior.Editable = false;
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      this.colRECNO.FieldName = "RECNO";
      this.colRECNO.Name = "colRECNO";
      this.colTARIH.Caption = "Tarih";
      this.colTARIH.FieldName = "TARIH";
      this.colTARIH.Name = "colTARIH";
      this.colTARIH.OptionsColumn.AllowEdit = false;
      this.colTARIH.Visible = true;
      this.colTARIH.VisibleIndex = 0;
      this.colTARIH.Width = 116;
      this.colEVRAK_SERI.Caption = "Evrak No";
      this.colEVRAK_SERI.FieldName = "EVRAK_NO";
      this.colEVRAK_SERI.Name = "colEVRAK_SERI";
      this.colEVRAK_SERI.OptionsColumn.AllowEdit = false;
      this.colEVRAK_SERI.Visible = true;
      this.colEVRAK_SERI.VisibleIndex = 1;
      this.colEVRAK_SERI.Width = 91;
      this.colCARI_KOD.Caption = "Cari Kodu";
      this.colCARI_KOD.FieldName = "CARI_KOD";
      this.colCARI_KOD.Name = "colCARI_KOD";
      this.colCARI_KOD.OptionsColumn.AllowEdit = false;
      this.colCARI_KOD.Visible = true;
      this.colCARI_KOD.VisibleIndex = 2;
      this.colCARI_KOD.Width = 111;
      this.colCARI_UNVAN.Caption = "Cari Ünvan";
      this.colCARI_UNVAN.FieldName = "CARI_UNVAN";
      this.colCARI_UNVAN.Name = "colCARI_UNVAN";
      this.colCARI_UNVAN.OptionsColumn.AllowEdit = false;
      this.colCARI_UNVAN.Visible = true;
      this.colCARI_UNVAN.VisibleIndex = 3;
      this.colCARI_UNVAN.Width = 278;
      this.panelControl2.Controls.Add((Control) this.txtEvrakSeri);
      this.panelControl2.Controls.Add((Control) this.labelControl3);
      this.panelControl2.Controls.Add((Control) this.chkBildirim);
      this.panelControl2.Controls.Add((Control) this.simpleButton1);
      this.panelControl2.Controls.Add((Control) this.tarih);
      this.panelControl2.Controls.Add((Control) this.labelControl2);
      this.panelControl2.Dock = DockStyle.Top;
      this.panelControl2.Location = new Point(0, 0);
      this.panelControl2.LookAndFeel.SkinName = "Office 2010 Black";
      this.panelControl2.Name = "panelControl2";
      this.panelControl2.Size = new Size(918, 33);
      this.panelControl2.TabIndex = 1;
      this.txtEvrakSeri.Location = new Point(311, 8);
      this.txtEvrakSeri.Name = "txtEvrakSeri";
      this.txtEvrakSeri.Size = new Size(100, 20);
      this.txtEvrakSeri.TabIndex = 5;
      this.labelControl3.Appearance.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.labelControl3.Location = new Point(217, 8);
      this.labelControl3.Name = "labelControl3";
      this.labelControl3.Size = new Size(83, 19);
      this.labelControl3.TabIndex = 4;
      this.labelControl3.Text = "Evrak Seri";
      this.chkBildirim.AutoSize = true;
      this.chkBildirim.Location = new Point(596, 3);
      this.chkBildirim.Name = "chkBildirim";
      this.chkBildirim.Size = new Size(239, 28);
      this.chkBildirim.TabIndex = 3;
      this.chkBildirim.Text = "Bildirimi yapılanları göster";
      this.chkBildirim.UseVisualStyleBackColor = true;
      this.simpleButton1.Dock = DockStyle.Right;
      this.simpleButton1.Location = new Point(841, 2);
      this.simpleButton1.Name = "simpleButton1";
      this.simpleButton1.Size = new Size(75, 29);
      this.simpleButton1.TabIndex = 2;
      this.simpleButton1.Text = "Yenile";
      this.simpleButton1.Click += new EventHandler(this.simpleButton1_Click);
      this.tarih.EditValue = (object) null;
      this.tarih.Location = new Point(63, 8);
      this.tarih.Name = "tarih";
      this.tarih.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.tarih.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.tarih.Size = new Size(139, 20);
      this.tarih.TabIndex = 1;
      this.labelControl2.Appearance.Font = new Font("Tahoma", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.labelControl2.Location = new Point(9, 7);
      this.labelControl2.Name = "labelControl2";
      this.labelControl2.Size = new Size(43, 19);
      this.labelControl2.TabIndex = 0;
      this.labelControl2.Text = "Tarih";
      this.xtraTabPage2.Controls.Add((Control) this.panelControl5);
      this.xtraTabPage2.Controls.Add((Control) this.panelControl4);
      this.xtraTabPage2.Controls.Add((Control) this.pnlUst);
      this.xtraTabPage2.Name = "xtraTabPage2";
      this.xtraTabPage2.Size = new Size(918, 442);
      this.xtraTabPage2.Text = "Fatura Kalemleri";
      this.panelControl5.Controls.Add((Control) this.grdKalem);
      this.panelControl5.Controls.Add((Control) this.button1);
      this.panelControl5.Dock = DockStyle.Fill;
      this.panelControl5.Location = new Point(0, 37);
      this.panelControl5.Name = "panelControl5";
      this.panelControl5.Size = new Size(918, 364);
      this.panelControl5.TabIndex = 4;
      this.grdKalem.Dock = DockStyle.Fill;
      this.grdKalem.Location = new Point(2, 2);
      this.grdKalem.MainView = (BaseView) this.gridView2;
      this.grdKalem.Name = "grdKalem";
      this.grdKalem.Size = new Size(914, 360);
      this.grdKalem.TabIndex = 3;
      this.grdKalem.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView2
      });
      this.gridView2.Appearance.EvenRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.EvenRow.Options.UseFont = true;
      this.gridView2.Appearance.FocusedRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.FocusedRow.Options.UseFont = true;
      this.gridView2.Appearance.FooterPanel.Font = new Font("Tahoma", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.FooterPanel.ForeColor = Color.Red;
      this.gridView2.Appearance.FooterPanel.Options.UseFont = true;
      this.gridView2.Appearance.FooterPanel.Options.UseForeColor = true;
      this.gridView2.Appearance.OddRow.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.OddRow.Options.UseFont = true;
      this.gridView2.Appearance.Row.Font = new Font("Tahoma", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.gridView2.Appearance.Row.Options.UseFont = true;
      this.gridView2.Columns.AddRange(new GridColumn[9]
      {
        this.colRECNO1,
        this.colCARIHR_RECNO,
        this.gridColumn1,
        this.colSTOK_KODU,
        this.colSTOK_ISIM,
        this.colMIKTAR,
        this.colKARSILANAN,
        this.gridColumn2,
        this.gridColumn3
      });
      this.gridView2.GridControl = this.grdKalem;
      this.gridView2.GroupSummary.AddRange(new GridSummaryItem[3]
      {
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "MIKTAR", (GridColumn) null, "0"),
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "KARSILANAN", (GridColumn) null, "0"),
        (GridSummaryItem) new GridGroupSummaryItem(SummaryItemType.Sum, "KALAN", (GridColumn) null, "0")
      });
      this.gridView2.Name = "gridView2";
      this.gridView2.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView2.OptionsBehavior.Editable = false;
      this.gridView2.OptionsView.ShowFooter = true;
      this.gridView2.OptionsView.ShowGroupedColumns = true;
      this.gridView2.OptionsView.ShowGroupPanel = false;
      this.gridView2.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridView2_CustomDrawCell);
      this.gridView2.DoubleClick += new EventHandler(this.gridView2_DoubleClick);
      this.colRECNO1.FieldName = "RECNO";
      this.colRECNO1.Name = "colRECNO1";
      this.colCARIHR_RECNO.FieldName = "CARIHR_RECNO";
      this.colCARIHR_RECNO.Name = "colCARIHR_RECNO";
      this.gridColumn1.Caption = "Barkod";
      this.gridColumn1.FieldName = "BARKOD";
      this.gridColumn1.Name = "gridColumn1";
      this.gridColumn1.Visible = true;
      this.gridColumn1.VisibleIndex = 1;
      this.gridColumn1.Width = 89;
      this.colSTOK_KODU.Caption = "Stok Kodu";
      this.colSTOK_KODU.FieldName = "STOK_KODU";
      this.colSTOK_KODU.Name = "colSTOK_KODU";
      this.colSTOK_KODU.Visible = true;
      this.colSTOK_KODU.VisibleIndex = 2;
      this.colSTOK_KODU.Width = 89;
      this.colSTOK_ISIM.Caption = "Stok İsmi";
      this.colSTOK_ISIM.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM.Name = "colSTOK_ISIM";
      this.colSTOK_ISIM.Visible = true;
      this.colSTOK_ISIM.VisibleIndex = 3;
      this.colSTOK_ISIM.Width = 424;
      this.colMIKTAR.Caption = "Miktar";
      this.colMIKTAR.FieldName = "MIKTAR";
      this.colMIKTAR.Name = "colMIKTAR";
      this.colMIKTAR.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colMIKTAR.Visible = true;
      this.colMIKTAR.VisibleIndex = 4;
      this.colMIKTAR.Width = 70;
      this.colKARSILANAN.Caption = "Karşılanan";
      this.colKARSILANAN.FieldName = "KARSILANAN";
      this.colKARSILANAN.Name = "colKARSILANAN";
      this.colKARSILANAN.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.colKARSILANAN.Visible = true;
      this.colKARSILANAN.VisibleIndex = 5;
      this.colKARSILANAN.Width = 60;
      this.gridColumn2.Caption = "Kalan";
      this.gridColumn2.FieldName = "KALAN";
      this.gridColumn2.Name = "gridColumn2";
      this.gridColumn2.Summary.AddRange(new GridSummaryItem[1]
      {
        (GridSummaryItem) new GridColumnSummaryItem(SummaryItemType.Sum)
      });
      this.gridColumn2.Visible = true;
      this.gridColumn2.VisibleIndex = 6;
      this.gridColumn2.Width = 76;
      this.gridColumn3.Caption = "BEŞERİ DURUM";
      this.gridColumn3.FieldName = "REYON_KODU";
      this.gridColumn3.Name = "gridColumn3";
      this.gridColumn3.Visible = true;
      this.gridColumn3.VisibleIndex = 0;
      this.gridColumn3.Width = 88;
      this.button1.Location = new Point(479, 264);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 6;
      this.button1.Text = "paket";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Visible = false;
      this.button1.Click += new EventHandler(this.btnPaketAl_Click);
      this.panelControl4.Controls.Add((Control) this.pnlDeak);
      this.panelControl4.Controls.Add((Control) this.btnSeriAktar);
      this.panelControl4.Controls.Add((Control) this.btnExcelAktar);
      this.panelControl4.Controls.Add((Control) this.btnImport);
      this.panelControl4.Controls.Add((Control) this.btnBildirim);
      this.panelControl4.Controls.Add((Control) this.btnKarekod);
      this.panelControl4.Dock = DockStyle.Bottom;
      this.panelControl4.Location = new Point(0, 401);
      this.panelControl4.Name = "panelControl4";
      this.panelControl4.Size = new Size(918, 41);
      this.panelControl4.TabIndex = 3;
      this.pnlDeak.Controls.Add((Control) this.label1);
      this.pnlDeak.Controls.Add((Control) this.cbDeak);
      this.pnlDeak.Dock = DockStyle.Fill;
      this.pnlDeak.Location = new Point(97, 2);
      this.pnlDeak.Name = "pnlDeak";
      this.pnlDeak.Size = new Size(439, 37);
      this.pnlDeak.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.Dock = DockStyle.Right;
      this.label1.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.label1.Location = new Point(6, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(156, 24);
      this.label1.TabIndex = 2;
      this.label1.Text = "Deaktivasyon Tipi";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.cbDeak.Dock = DockStyle.Right;
      this.cbDeak.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbDeak.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.cbDeak.FormattingEnabled = true;
      this.cbDeak.Items.AddRange(new object[6]
      {
        (object) "SİSTEMDEN ÇIKARMA",
        (object) "ÜRETİM FİRELERİ",
        (object) "GERİ ÇEKME SEBEBİYLE İMHA",
        (object) "MİAT SEBEBİYLE İMHA",
        (object) "REVİZYON",
        (object) "SARF"
      });
      this.cbDeak.Location = new Point(162, 0);
      this.cbDeak.Name = "cbDeak";
      this.cbDeak.Size = new Size(277, 33);
      this.cbDeak.TabIndex = 1;
      this.btnSeriAktar.Dock = DockStyle.Right;
      this.btnSeriAktar.Location = new Point(536, 2);
      this.btnSeriAktar.Name = "btnSeriAktar";
      this.btnSeriAktar.Size = new Size(95, 37);
      this.btnSeriAktar.TabIndex = 11;
      this.btnSeriAktar.Text = "Seri Aktar";
      this.btnSeriAktar.Click += new EventHandler(this.btnSeriAktar_Click);
      this.btnExcelAktar.Dock = DockStyle.Right;
      this.btnExcelAktar.Location = new Point(631, 2);
      this.btnExcelAktar.Name = "btnExcelAktar";
      this.btnExcelAktar.Size = new Size(95, 37);
      this.btnExcelAktar.TabIndex = 9;
      this.btnExcelAktar.Text = "Excel Aktar";
      this.btnExcelAktar.Click += new EventHandler(this.btnExcelAktar_Click);
      this.btnImport.Dock = DockStyle.Right;
      this.btnImport.Location = new Point(726, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(95, 37);
      this.btnImport.TabIndex = 7;
      this.btnImport.Text = "Dosya Aktar";
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.btnBildirim.Appearance.ForeColor = Color.Black;
      this.btnBildirim.Appearance.Options.UseForeColor = true;
      this.btnBildirim.ButtonStyle = BorderStyles.Office2003;
      this.btnBildirim.Dock = DockStyle.Left;
      this.btnBildirim.Location = new Point(2, 2);
      this.btnBildirim.Name = "btnBildirim";
      this.btnBildirim.Size = new Size(95, 37);
      this.btnBildirim.TabIndex = 5;
      this.btnBildirim.Text = "Bildirim Yap";
      this.btnBildirim.Click += new EventHandler(this.btnBitir_Click);
      this.btnKarekod.Dock = DockStyle.Right;
      this.btnKarekod.Location = new Point(821, 2);
      this.btnKarekod.Name = "btnKarekod";
      this.btnKarekod.Size = new Size(95, 37);
      this.btnKarekod.TabIndex = 3;
      this.btnKarekod.Text = "Karekod Oku";
      this.btnKarekod.Click += new EventHandler(this.btnKarekod_Click);
      this.pnlUst.Controls.Add((Control) this.lblDurum);
      this.pnlUst.Dock = DockStyle.Top;
      this.pnlUst.Location = new Point(0, 0);
      this.pnlUst.Name = "pnlUst";
      this.pnlUst.Size = new Size(918, 37);
      this.pnlUst.TabIndex = 0;
      this.lblDurum.AutoSize = true;
      this.lblDurum.Dock = DockStyle.Left;
      this.lblDurum.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 162);
      this.lblDurum.ForeColor = Color.FromArgb(0, 64, 0);
      this.lblDurum.Location = new Point(2, 2);
      this.lblDurum.Name = "lblDurum";
      this.lblDurum.Size = new Size(0, 29);
      this.lblDurum.TabIndex = 3;
      this.xtraTabPage3.Controls.Add((Control) this.splitterControl1);
      this.xtraTabPage3.Controls.Add((Control) this.panelControl7);
      this.xtraTabPage3.Controls.Add((Control) this.panelControl6);
      this.xtraTabPage3.Name = "xtraTabPage3";
      this.xtraTabPage3.Size = new Size(918, 442);
      this.xtraTabPage3.Text = "Bildirim Sonuç";
      this.splitterControl1.Dock = DockStyle.Bottom;
      this.splitterControl1.Location = new Point(0, 281);
      this.splitterControl1.Name = "splitterControl1";
      this.splitterControl1.Size = new Size(918, 6);
      this.splitterControl1.TabIndex = 5;
      this.splitterControl1.TabStop = false;
      this.panelControl7.Controls.Add((Control) this.grdKalemDetay);
      this.panelControl7.Dock = DockStyle.Fill;
      this.panelControl7.Location = new Point(0, 0);
      this.panelControl7.Name = "panelControl7";
      this.panelControl7.Size = new Size(918, 287);
      this.panelControl7.TabIndex = 4;
      this.grdKalemDetay.Dock = DockStyle.Fill;
      this.grdKalemDetay.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 162);
      this.grdKalemDetay.Location = new Point(2, 2);
      this.grdKalemDetay.MainView = (BaseView) this.gridView3;
      this.grdKalemDetay.Name = "grdKalemDetay";
      this.grdKalemDetay.Size = new Size(914, 283);
      this.grdKalemDetay.TabIndex = 3;
      this.grdKalemDetay.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView3
      });
      this.gridView3.Columns.AddRange(new GridColumn[10]
      {
        this.colID,
        this.colTIP,
        this.colCARIHR_RECNO1,
        this.colSTOK_KOD,
        this.colSTOK_ISIM1,
        this.colBARKOD,
        this.colSERI_NO,
        this.colMIAD,
        this.colPARTINO,
        this.colDURUM
      });
      this.gridView3.GridControl = this.grdKalemDetay;
      this.gridView3.Name = "gridView3";
      this.gridView3.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.gridView3.OptionsBehavior.Editable = false;
      this.gridView3.OptionsView.ShowGroupPanel = false;
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colTIP.FieldName = "TIP";
      this.colTIP.Name = "colTIP";
      this.colCARIHR_RECNO1.FieldName = "CARIHR_RECNO";
      this.colCARIHR_RECNO1.Name = "colCARIHR_RECNO1";
      this.colSTOK_KOD.Caption = "Stok Kodu";
      this.colSTOK_KOD.FieldName = "STOK_KOD";
      this.colSTOK_KOD.Name = "colSTOK_KOD";
      this.colSTOK_KOD.Visible = true;
      this.colSTOK_KOD.VisibleIndex = 0;
      this.colSTOK_KOD.Width = 113;
      this.colSTOK_ISIM1.Caption = "Stok İsmi";
      this.colSTOK_ISIM1.FieldName = "STOK_ISIM";
      this.colSTOK_ISIM1.Name = "colSTOK_ISIM1";
      this.colSTOK_ISIM1.Visible = true;
      this.colSTOK_ISIM1.VisibleIndex = 1;
      this.colSTOK_ISIM1.Width = 130;
      this.colBARKOD.Caption = "Barkod";
      this.colBARKOD.FieldName = "BARKOD";
      this.colBARKOD.Name = "colBARKOD";
      this.colBARKOD.Visible = true;
      this.colBARKOD.VisibleIndex = 2;
      this.colBARKOD.Width = 130;
      this.colSERI_NO.Caption = "Seri No";
      this.colSERI_NO.FieldName = "SERI_NO";
      this.colSERI_NO.Name = "colSERI_NO";
      this.colSERI_NO.Visible = true;
      this.colSERI_NO.VisibleIndex = 3;
      this.colSERI_NO.Width = 130;
      this.colMIAD.Caption = "Miad";
      this.colMIAD.FieldName = "MIAD";
      this.colMIAD.Name = "colMIAD";
      this.colMIAD.Visible = true;
      this.colMIAD.VisibleIndex = 4;
      this.colMIAD.Width = 130;
      this.colPARTINO.Caption = "Parti No";
      this.colPARTINO.FieldName = "PARTINO";
      this.colPARTINO.Name = "colPARTINO";
      this.colPARTINO.Visible = true;
      this.colPARTINO.VisibleIndex = 5;
      this.colPARTINO.Width = 130;
      this.colDURUM.Caption = "Bildirim Durum";
      this.colDURUM.FieldName = "MESAJ";
      this.colDURUM.Name = "colDURUM";
      this.colDURUM.Visible = true;
      this.colDURUM.VisibleIndex = 6;
      this.colDURUM.Width = 137;
      this.panelControl6.Controls.Add((Control) this.memSonuc);
      this.panelControl6.Dock = DockStyle.Bottom;
      this.panelControl6.Location = new Point(0, 287);
      this.panelControl6.Name = "panelControl6";
      this.panelControl6.Size = new Size(918, 155);
      this.panelControl6.TabIndex = 3;
      this.memSonuc.Dock = DockStyle.Fill;
      this.memSonuc.Location = new Point(2, 2);
      this.memSonuc.Name = "memSonuc";
      this.memSonuc.Size = new Size(914, 151);
      this.memSonuc.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(924, 510);
      this.Controls.Add((Control) this.xtraTabControl1);
      this.Controls.Add((Control) this.panelControl1);
      this.IsMdiContainer = true;
      this.Name = nameof (FrmDeaktivasyon);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bildirim";
      this.WindowState = FormWindowState.Maximized;
      this.panelControl1.EndInit();
      this.panelControl1.ResumeLayout(false);
      this.xtraTabControl1.EndInit();
      this.xtraTabControl1.ResumeLayout(false);
      this.xtraTabPage1.ResumeLayout(false);
      this.panelControl3.EndInit();
      this.panelControl3.ResumeLayout(false);
      this.grdSiparis.EndInit();
      this.gridView1.EndInit();
      this.panelControl2.EndInit();
      this.panelControl2.ResumeLayout(false);
      this.panelControl2.PerformLayout();
      this.txtEvrakSeri.Properties.EndInit();
      this.tarih.Properties.VistaTimeProperties.EndInit();
      this.tarih.Properties.EndInit();
      this.xtraTabPage2.ResumeLayout(false);
      this.panelControl5.EndInit();
      this.panelControl5.ResumeLayout(false);
      this.grdKalem.EndInit();
      this.gridView2.EndInit();
      this.panelControl4.EndInit();
      this.panelControl4.ResumeLayout(false);
      this.pnlDeak.ResumeLayout(false);
      this.pnlDeak.PerformLayout();
      this.pnlUst.EndInit();
      this.pnlUst.ResumeLayout(false);
      this.pnlUst.PerformLayout();
      this.xtraTabPage3.ResumeLayout(false);
      this.panelControl7.EndInit();
      this.panelControl7.ResumeLayout(false);
      this.grdKalemDetay.EndInit();
      this.gridView3.EndInit();
      this.panelControl6.EndInit();
      this.panelControl6.ResumeLayout(false);
      this.memSonuc.Properties.EndInit();
      this.ResumeLayout(false);
    }

    public class HataListe
    {
      private int hataKod;
      private int ad;
      private string durum;

      public int HataKod
      {
        get => this.hataKod;
        set => this.hataKod = value;
      }

      public int Ad
      {
        get => this.ad;
        set => this.ad = value;
      }

      public string Durum
      {
        get => this.durum;
        set => this.durum = value;
      }
    }
  }
}
