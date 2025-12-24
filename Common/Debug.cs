//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.IO;

namespace Common;

/// <summary>Static class containing debugging logs.</summary>
public static class Debug
{
  /// <summary>Set to <see langword="true"/> to output debugging information to the output stream.</summary>
  public static bool Verbose { get; set; }
  /// <summary>Set to the line you must go to when making logs.</summary>
  public static int LineStart { get; set; }
  /// <summary>This increments when a log is written. Set to 0 to reset to top.</summary>
  public static int LineCount { get; set; }
  /// <summary>Sets the output stream.</summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);

  private static void DoLog (string msg)
  {
    try
    {
      //if (Console.CursorTop < LineStart + LineCount)
      //  Console.SetCursorPosition(0, LineStart + LineCount);
      if (Verbose) Console.WriteLine(msg);
      LineCount++;
    }
    catch (ArgumentOutOfRangeException)
    {

    }
    catch (Exception)
    {
      throw;
    }
  }
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="msg">The message to log.</param>
  public static void Log (string msg) =>
    DoLog(msg);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  public static void Log (string src, string msg) => DoLog($"{src} : {msg}");
  public static void Log (string src, string proc, string msg) => DoLog($"{src}.{proc} : {msg}");
  public static void LogException (Exception e) =>
    LogFrom(e?.Source, e?.TargetSite?.Name, e?.Message);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="target">The originating method.</param>
  /// <param name="msg">The message to log.</param>
  private static void LogFrom (string? src, string? target, string? msg) =>
    DoLog($"{src}.{target} : {msg}");
}
