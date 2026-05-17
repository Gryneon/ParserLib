using System;
using System.Windows.Forms;

using ParserDebuggerApp;

namespace TokenFinder;

static class Program
{
  [STAThread]
  static void Main ()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainForm());
  }
}
