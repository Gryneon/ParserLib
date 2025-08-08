using Microsoft.Office.Interop.Excel;

namespace Common.Excel;
internal static class App
{
  public static Application Instance { get; } = new();
}
