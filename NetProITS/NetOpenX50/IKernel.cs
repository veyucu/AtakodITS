// Decompiled with JetBrains decompiler
// Type: NetOpenX50.IKernel
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace NetOpenX50
{
  [CompilerGenerated]
  [Guid("F81334BC-DF24-4E73-886B-1486A3BF4D72")]
  [TypeIdentifier]
  [ComImport]
  public interface IKernel
  {
    [DispId(201)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void FreeNetsisLibrary();

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_1();

    [DispId(203)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    Sirket yeniSirket(
      [In] TVTTipi vtTipi,
      [MarshalAs(UnmanagedType.BStr), In] string vtAdi,
      [MarshalAs(UnmanagedType.BStr), In] string vtKulAdi,
      [MarshalAs(UnmanagedType.BStr), In] string vtKulSifre,
      [MarshalAs(UnmanagedType.BStr), In] string NetKul,
      [MarshalAs(UnmanagedType.BStr), In] string NetSifre,
      [In] int Sube_Kodu);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap2_1();

    [DispId(205)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    Fatura yeniFatura([MarshalAs(UnmanagedType.Interface), In] Sirket Sirket, [In] TFaturaTip FaturaTipi);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap3_12();

    [DispId(218)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    NetRS yeniNetRS([MarshalAs(UnmanagedType.Interface), In] Sirket Sirket);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap4_3();

    [DispId(222)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    Basim yeniBasim([MarshalAs(UnmanagedType.Interface), In] Sirket Sirket);
  }
}
