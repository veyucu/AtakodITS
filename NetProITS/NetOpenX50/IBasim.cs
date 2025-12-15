// Decompiled with JetBrains decompiler
// Type: NetOpenX50.IBasim
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace NetOpenX50
{
  [CompilerGenerated]
  [Guid("8D20DEA3-ACB5-4A40-84BA-90F7438E555A")]
  [TypeIdentifier]
  [ComImport]
  public interface IBasim : IMerkez
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_6();

    [DispId(302)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool FaturaBasimDizaynNo(
      [In] TFaturaTip FaturaTipi,
      [MarshalAs(UnmanagedType.BStr), In] string FaturaNo,
      [MarshalAs(UnmanagedType.BStr), In] string CariKodu,
      [In] int DizaynNo);
  }
}
