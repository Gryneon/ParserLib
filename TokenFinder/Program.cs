using System;
using System.Windows.Forms;

using ParserDebuggerApp;

namespace TokenFinder;

internal static class Program
{
  [STAThread]
  internal static void Main ()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainForm());
  }
}
