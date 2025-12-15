// Decompiled with JetBrains decompiler
// Type: NetProITS.FIRMAEntities
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#nullable disable
namespace NetProITS
{
  public class FIRMAEntities : DbContext
  {
    public FIRMAEntities()
      : base("name=FIRMAEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      throw new UnintentionalCodeFirstException();
    }

    public DbSet<NetProITS.TBLFATUIRS> TBLFATUIRS { get; set; }

    public DbSet<NetProITS.TBLSTSABIT> TBLSTSABIT { get; set; }

    public DbSet<NetProITS.TBLSIPAMAS> TBLSIPAMAS { get; set; }

    public DbSet<NetProITS.ITSHAR> ITSHAR { get; set; }

    public DbSet<NetProITS.HATA_KODLARI> HATA_KODLARI { get; set; }

    public DbSet<NetProITS.TBLTRANSFER> TBLTRANSFER { get; set; }

    public DbSet<NetProITS.TBLTRANSFER_DETAY> TBLTRANSFER_DETAY { get; set; }

    public DbSet<NetProITS.TRANSFER_CARI> TRANSFER_CARI { get; set; }

    public DbSet<NetProITS.VIEWTRANSFER_DETAY> VIEWTRANSFER_DETAY { get; set; }

    public DbSet<NetProITS.ITSHAR_VIEW> ITSHAR_VIEW { get; set; }

    public DbSet<NetProITS.PTSHAR_VIEW> PTSHAR_VIEW { get; set; }

    public DbSet<NetProITS.BILDIRIM_STOK> BILDIRIM_STOK { get; set; }

    public DbSet<NetProITS.BILDIRIM> BILDIRIM { get; set; }
  }
}
