#pragma warning disable CA1416 // Validate platform compatibility

namespace FormObjectViewer;

internal static class Program
{
  /// <summary> The main entry point for the application.</summary>
  [STAThread]
  internal static void Main ()
  {
    // To customize application configuration such as set high DPI settings or default font,
    // see https://aka.ms/applicationconfiguration.
    ApplicationConfiguration.Initialize();
    //Collection<object> t = [54, 345, 87f, 42m, '3', "str", new Collection<object>()];
    Form start = new ParserForm();
    Application.Run(start);
    start.Dispose();
  }
}
