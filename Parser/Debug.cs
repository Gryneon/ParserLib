//#pragm/
//+arning disable CS1591 // Missing XML comment for publicly visible type or member

using ComDebug = Common.Debug;

namespace Parser;

/// <summary>Static class containing debugging logs.</summary>
public static class Debug
{
  /// <summary>Set to <see langword="true"/> to output debugging information to the output stream.</summary>
  public static LogClass Verbosity => ComDebug.Verbosity;
  /// <summary>Set to the line you must go to when making logs.</summary>
  public static int LineStart { get; set; }
  /// <summary>This increments when a log is written. Set to 0 to reset to top.</summary>
  public static int LineCount { get; set; }
  /// <summary>Sets the output stream.</summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);
  private static void DoLog (string msg, ConsoleColor? back = null, ConsoleColor? text = null)
  {
    try
    {
      //if (Console.CursorTop < LineStart + LineCount)
      //  Console.SetCursorPosition(0, LineStart + LineCount);
      if (Verbosity != LogClass.None)
      {
        if (back is not null) Console.BackgroundColor = back.Value;
        if (text is not null) Console.ForegroundColor = text.Value;
        Console.WriteLine(msg);
        if (back is not null || text is not null) Console.ResetColor();
        LineCount++;
      }
    }
    catch (ArgumentOutOfRangeException)
    {

    }
    catch (Exception)
    {
      throw;
    }
  }
  private static ConsoleColor GetTextColor (MsgClass msg) => msg switch
  {
    MsgClass.Debug => C_Blue,
    MsgClass.Forced => C_Cyan,
    MsgClass.Error => C_Red,
    MsgClass.Warning => C_Yellow,
    MsgClass.Critical => C_Black,
    MsgClass.None or MsgClass.Informational or _ => C_White,
  };
  private static ConsoleColor GetBackColor (MsgClass msg) => msg switch
  {
    MsgClass.Error => C_DarkRed,
    MsgClass.Critical => C_Red,
    _ => C_Black,
  };
  public static void Log (MsgClass msgClass, string className, string methodName, string msg) =>
    Log(className, methodName, msg, GetBackColor(msgClass), GetTextColor(msgClass));
  public static void Log (MsgClass msgClass, string className, string msg) =>
    Log(className, msg, GetBackColor(msgClass), GetTextColor(msgClass));

  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="msg">The message to log.</param>
  public static void Log (string msg) =>
    DoLog(msg);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="back">The background color.</param>
  /// <param name="text">The foreground color.</param>
  public static void Log (string src, string msg, ConsoleColor back = C_Black, ConsoleColor text = C_White) =>
    DoLog($"{src} : {msg}", back, text);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="target">The originating method.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="back">The background color.</param>
  /// <param name="text">The foreground color.</param>
  public static void Log (string src, string target, string msg, ConsoleColor back = C_Black, ConsoleColor text = C_White) =>
    DoLog($"{src}.{target} : {msg}", back, text);
  public static void LogException (Exception e)
  {
    e.ThrowIfNull();
    DoLog($"{e.Source}.{e.TargetSite?.Name} : {e.Message}", C_DarkRed, C_Red);
  }
}
