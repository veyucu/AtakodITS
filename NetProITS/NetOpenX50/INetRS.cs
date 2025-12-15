// Decompiled with JetBrains decompiler
// Type: NetOpenX50.INetRS
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace NetOpenX50
{
  [CompilerGenerated]
  [Guid("A66442EA-C7DF-4699-9818-E63848EEBF48")]
  [TypeIdentifier]
  [ComImport]
  public interface INetRS : IData
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_30();

    [DispId(413)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Calistir([MarshalAs(UnmanagedType.BStr), In] string SQL);
  }
}
