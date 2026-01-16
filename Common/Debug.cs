//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>The logging level of the program.</summary>
public enum LogClass
{
  None,
  NoLog,
  Minimal,
  Standard,
  Verbose,
  All,
  DebugAll
}

/// <summary>The log level of the log command.</summary>
public enum MsgClass
{
  None,
  Debug,
  Forced,
  Error,
  Warning,
  Informational,
  Critical,
}

/// <summary>Static class containing debugging logs.</summary>
public static class Debug
{
  /// <summary>The level of logs to display.</summary>
  public static LogClass Verbosity { get; set; }
  /// <summary>Set to the line you must go to when making logs.</summary>
  public static int LineStart { get; set; }
  /// <summary>This increments when a log is written. Set to 0 to reset to top.</summary>
  public static int LineCount { get; set; }
  /// <summary>Sets the output stream.</summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);

  private static void DoLog (string msg, MsgClass msgClass)
  {
    //if (Console.CursorTop < LineStart + LineCount)
    //  Console.SetCursorPosition(0, LineStart + LineCount);
    if (msgClass is not MsgClass.None)
      Console.WriteLine(msg);
    LineCount++;
  }
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="msg">The message to log.</param>
  /// <param name="msgClass">The type of message to log.</param>
  public static void Log (string msg, MsgClass msgClass = MsgClass.Debug) =>
    DoLog(msg, msgClass);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  public static void Log (string src, string msg) => DoLog($"{src} : {msg}", MsgClass.Debug);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="proc">The method that called the log command.</param>
  public static void Log (string src, string proc, string msg) => DoLog($"{src}.{proc} : {msg}", MsgClass.Debug);
  public static void LogException (Exception e) =>
    LogFrom(e?.Source, e?.TargetSite?.Name, e?.Message, MsgClass.Error);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="target">The originating method.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="msgClass">The type of message to log.</param>
  private static void LogFrom (string? src, string? target, string? msg, MsgClass msgClass) =>
    DoLog($"{src}.{target} : {msg}", msgClass);
}
