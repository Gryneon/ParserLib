using System.Formats.Asn1;

using static System.Net.Mime.MediaTypeNames;

namespace Common;

/// <summary>Static class containing debugging log functions and other useful tools.</summary>
public static class Debug
{
  /// <summary>A pair of strings, that store the classname and the method name.</summary>
  /// <param name="ClassName">The classname.</param>
  /// <param name="Method">The method name.</param>
  private sealed record class StackLoc (string ClassName, string Method);
  private static Collection<StackLoc> CallStack { get; } = [];
  private static string ThisClass => CallStack.Peek().ClassName;
  private static string ThisMethod => CallStack.Peek().Method;
  public static void Log (MsgClass cls, string message) => Log(cls, ThisClass, ThisMethod, message);
  public static void DebugIn (string method) => CallStack.Add(new(ThisClass, method));
  public static void DebugOut () => CallStack.Drop();
  public static void DebugIn (string classname, string method) => CallStack.Add(new(classname, method));

  /// <summary>Set to <see langword="true"/> to output debugging information to the output stream.</summary>
  public static LogClass Verbosity
  {
    get => field;
    set => field = value;
  }

  /// <summary>Sets the output stream.</summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);
  public static void ClearLog () => Console.Clear();
  private static void DoLog (string msg, ConsoleColor? back = null, ConsoleColor? text = null, bool partial = false)
  {
    try
    {
      //if (Console.CursorTop < LineStart + LineCount)
      //  Console.SetCursorPosition(0, LineStart + LineCount);
      if (Verbosity != LogClass.None)
      {
        if (back is not null) Console.BackgroundColor = back.Value;
        if (text is not null) Console.ForegroundColor = text.Value;
        if (partial)
          Console.Write(msg);
        else
          Console.WriteLine(msg);
        if (back is not null || text is not null) Console.ResetColor();
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
  public static void LogHead (MsgClass cls) =>
    DoLog($"{ThisClass}.{ThisMethod} : ", GetBackColor(cls), GetTextColor(cls), true);
  public static void LogHead (MsgClass cls, string classname, string method) =>
    DoLog($"{classname}.{method} : ", GetBackColor(cls), GetTextColor(cls), true);
  public static void LogPart (MsgClass cls, string part) =>
    DoLog(part, GetBackColor(cls), GetTextColor(cls), true);
  public static void NewLine () => DoLog("\n", null, null, true);
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
  public static void Log (MsgClass msgClass, string className, string methodName, string msg, bool partial = false) =>
    Log(className, methodName, msg, GetBackColor(msgClass), GetTextColor(msgClass), partial);
  public static void Log (MsgClass msgClass, string className, string msg, bool partial = false) =>
    Log(className, msg, GetBackColor(msgClass), GetTextColor(msgClass), partial);

  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="back">The background color.</param>
  /// <param name="text">The foreground color.</param>
  public static void Log (string src, string msg, ConsoleColor back = C_Black, ConsoleColor text = C_White, bool partial = false) =>
    DoLog($"{src} : {msg}", back, text, partial);
  /// <summary>Logs a message to the output stream.</summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="target">The originating method.</param>
  /// <param name="msg">The message to log.</param>
  /// <param name="back">The background color.</param>
  /// <param name="text">The foreground color.</param>
  public static void Log (string src, string target, string msg, ConsoleColor back = C_Black, ConsoleColor text = C_White, bool partial = false) =>
    DoLog($"{src}.{target} : {msg}", back, text, partial);
  public static void LogException (Exception e)
  {
    e.ThrowIfNull();
    DoLog($"{e.Source}.{e.TargetSite?.Name} : {e.Message}", C_DarkRed, C_Red);
  }
}
