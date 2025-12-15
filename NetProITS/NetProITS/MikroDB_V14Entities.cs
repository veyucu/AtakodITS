// Decompiled with JetBrains decompiler
// Type: NetProITS.MikroDB_V14Entities
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#nullable disable
namespace NetProITS
{
  public class MikroDB_V14Entities : DbContext
  {
    public MikroDB_V14Entities()
      : base("name=MikroDB_V14Entities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      throw new UnintentionalCodeFirstException();
    }

    public DbSet<NetProITS.TBL_AYARLAR> TBL_AYARLAR { get; set; }
  }
}
