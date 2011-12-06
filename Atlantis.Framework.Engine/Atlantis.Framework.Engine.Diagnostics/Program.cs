using System;
using System.Windows.Forms;

namespace Atlantis.Framework.Engine.Diagnostics
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }

    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
      MessageBox.Show(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
    }
  }
}
