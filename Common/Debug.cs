using System.Formats.Asn1;

using static System.Net.Mime.MediaTypeNames;

namespace Common;

/// <summary>Static class containing debugging log functions and other useful tools.</summary>
public static class Debug
{
  #region Private Properties and Methods
  /// <summary>A pair of strings, that store the classname and the method name.</summary>
  /// <param name="ClassName">The classname.</param>
  /// <param name="Method">The method name.</param>
  private sealed record class StackLoc (string ClassName, string Method);
  private static Collection<StackLoc> CallStack { get; } = [];
  private static string ThisClass => CallStack.Peek().ClassName;
  private static string ThisMethod => CallStack.Peek().Method;
  private static void DoLog (string msg, ConsoleColor? back = null, ConsoleColor? text = null, bool partial = false)
  {
    try
    {
      if (back is not null) Console.BackgroundColor = back.Value;
      if (text is not null) Console.ForegroundColor = text.Value;
      if (partial)
        Console.Write(msg);
      else
        Console.WriteLine(msg);
      if (back is not null || text is not null) Console.ResetColor();
    }
    catch (Exception)
    {
      throw;
    }
  }
  private static void DoLogHead (MsgClass cls, string classname, string method)
  {
    string format = $"{classname}.{method} : ";

    if (method.StartsWithAny(["[", "("], SCO))
      format = $"{classname}{method} : ";

    DoLog(format, GetBackColor(cls), GetTextColor(cls), true);
  }
  private static ConsoleColor GetTextColor (MsgClass msg) => msg switch
  {
    MsgClass.Debug => C_Blue,
    MsgClass.Forced => C_Cyan,
    MsgClass.Error => C_Black,
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
  #endregion
  public static int LogDepth => CallStack.Count - 1;
  public static void PurgeStackTo (int depth)
  {
    while (CallStack.Count > depth + 1)
      CallStack.Drop();
    Log(MsgClass.Debug, ThisClass, ThisMethod, "Purged back to here.");
  }
  /// <summary>Logs a message to the output stream.</summary>
  /// <remarks>This method always assumes that <see cref="DebugIn(string, string)"/> has been called, and uses that location as the caller.</remarks>
  /// <param name="cls">The color and verbosity of the message.</param>
  /// <param name="message">The message text.</param>
  public static void Log (MsgClass cls, string message)
  {
    LogHead();
    LogPart(cls, message);
    NewLine();
  }
  /// <summary>Sets the logging location for any logs.</summary>
  /// <remarks>This keeps the classname the same as it was since the last <c>DebugIn</c> call.</remarks>
  /// <param name="method">The method name.</param>
  public static void DebugIn (string method) => CallStack.Add(new(ThisClass, method));
  /// <summary>Clears the last location set by DebugIn and restores the one before it.</summary>
  public static void DebugOut () => CallStack.Drop();
  /// <summary>Sets the logging location for any logs.</summary>
  /// <param name="classname">The class name.</param>
  /// <param name="method">The method name.</param>
  public static void DebugIn (string classname, string method) => CallStack.Add(new(classname, method));

  /// <summary>Set to define what level of debugging information to display.</summary>
  public static LogClass Verbosity { get; set; }

  /// <summary>Sets the output stream.</summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);
  /// <summary>Clears the console.</summary>
  public static void ClearLog () => Console.Clear();
  /// <summary>Only writes the location of the log, and does not add a newline.</summary>
  /// <param name="cls">The color if not default.</param>
  public static void LogHead (MsgClass cls = MsgClass.Debug) => DoLogHead(cls, ThisClass, ThisMethod);

  public static void LogPart (MsgClass cls, string part) =>
    DoLog(part, GetBackColor(cls), GetTextColor(cls), true);
  public static void NewLine () => DoLog("\n", C_Black, null, true);
  public static void Log (MsgClass msgClass, string className, string methodName, string msg)
  {
    DoLogHead(MsgClass.Debug, className, methodName);
    LogPart(msgClass, msg);
    NewLine();
  }

  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="back">The background color.</param>
  /// <param name="text">The foreground color.</param>
  /// <param name="partial"> If <see langword="true"/>, no newline will be appended.</param>
  public static void Log (string src, string msg, ConsoleColor back = C_Black, ConsoleColor text = C_White, bool partial = false) =>
    DoLog($"{src} : {msg}", back, text, partial);
  /// <summary>Logs an exception that was handled internally.</summary>
  /// <param name="e">The exception to log.</param>
  public static void LogException (Exception e) =>
    Log(MsgClass.Error, ThisClass, ThisMethod, e?.Message ?? SE);
}
