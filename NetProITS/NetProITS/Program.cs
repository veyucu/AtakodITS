// Decompiled with JetBrains decompiler
// Type: NetProITS.Program
// Assembly: NetProITS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64902309-711B-4AE1-AFF6-78FB4B0E99F7
// Assembly location: C:\Dosyalar\Müşteriler\Afyon Şifa\NetProITS\NetProITS\NetProITS.exe

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace NetProITS
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.ThreadException += new ThreadExceptionEventHandler(Program.Form1_UIThreadException);
      Application.Run((Form) new FrmGiris());
    }

    private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
    {
      DialogResult dialogResult = DialogResult.Cancel;
      try
      {
        if (MyUtils.sirket != null)
        {
          // ISSUE: reference to a compiler-generated method
          MyUtils.sirket.LogOff();
          Marshal.ReleaseComObject((object) MyUtils.sirket);
        }
        if (MyUtils.kernel != null)
        {
          // ISSUE: reference to a compiler-generated method
          MyUtils.kernel.FreeNetsisLibrary();
          Marshal.ReleaseComObject((object) MyUtils.kernel);
        }
        dialogResult = Program.ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
      }
      catch
      {
        try
        {
          int num = (int) MessageBox.Show("Fatal Windows Forms Error", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
        }
        finally
        {
          Application.Exit();
        }
      }
      if (dialogResult != DialogResult.Abort)
        return;
      Application.Exit();
    }

    private static DialogResult ShowThreadExceptionDialog(string title, Exception e)
    {
      return MessageBox.Show("An application error occurred. Please contact the adminstrator with the following information:\n\n" + e.Message + "\n\nStack Trace:\n" + e.StackTrace, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
    }
  }
}
