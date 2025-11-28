//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Parser;

internal enum DebugMsg
{
  DM_None,
  Tokenize_Ignore_Token,
  Tokenize_Wrong_Type,
  Debug_Log_Extra_Values
}

internal enum ExceptionMsg
{
  EM_None,
  Debug_Unknown_Exception,
  Override_Required,

}

/// <summary>
/// Static class containing debugging logs.
/// </summary>
internal static class Debug
{
  /// <summary>
  /// Set to <see langword="true"/> to output debugging information to the output stream.
  /// </summary>
  public static bool Verbose { get; set; }
  /// <summary>
  /// Set to the line you must go to when making logs.
  /// </summary>
  public static int LineStart { get; set; }
  /// <summary>
  /// This increments when a log is written. Set to 0 to reset to top.
  /// </summary>
  public static int LineCount { get; set; }
  /// <summary>
  /// Sets the output stream.
  /// </summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);
  private static void DoLog (string msg)
  {
#if DEBUG
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
#endif
  }
  private static readonly Dictionary<DebugMsg, Func<string, string, string>> MsgFormats = new()
  {
    (DM_None, static (_, __) => SE),
    (Tokenize_Wrong_Type, static (type, _) => $"TokenizeOperation:Execute(): My type is wrong! I am a {type}"),
    (Tokenize_Ignore_Token, static (mdd, _) => $"TokenizeOperation.Execute(): Token is whitespace or ignored \"{mdd}\""),
  };
  private static readonly Dictionary<ExceptionMsg, Func<string, string, string>> XMsgFormats = new()
  {
    (EM_None, static (_, __) => "Exception not defined."),
    (Debug_Unknown_Exception, static (_, __) => "Invalid Exception Parameters, or Unknown Exception Message"),
    (Override_Required, static (_, __) => "This needs to be overridden by the inheriting class."),
  };
  public static void Log (DebugMsg msg, params Collection<string> values)
  {
    if (values.Count == 0)
      DoLog(MsgFormats[msg](SE, SE));
    else if (values.Count == 1)
      DoLog(MsgFormats[msg](values[0], SE));
    else if (values.Count == 2)
      DoLog(MsgFormats[msg](values[0], values[1]));
  }
  [DoesNotReturn]
  public static void Throw<T> (ExceptionMsg msg, params Collection<string> values) where T : Exception => _ = Throw<T, object>(msg, values);
  [DoesNotReturn]
  public static TReturn Throw<T, TReturn> (ExceptionMsg msg, params Collection<string> values) where T : Exception
  {
    string text;

    if (values.Count == 0)
      text = XMsgFormats[msg](SE, SE);
    else if (values.Count == 1)
      text = XMsgFormats[msg](values[0], SE);
    else if (values.Count == 2)
      text = XMsgFormats[msg](values[0], values[1]);
    else
    {
      Log(Debug_Log_Extra_Values);
      throw new InvalidOperationException(XMsgFormats[Debug_Unknown_Exception](SE, SE));
    }

    throw new InvalidOperationException(text);
  }

  /// <summary>
  /// Logs a message to the output stream.
  /// </summary>
  /// <param name="msg">The message to log.</param>
  public static void Log (string msg) =>
    DoLog(msg);
  /// <summary>
  /// Logs a message to the output stream.
  /// </summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="msg">The message to log.</param>
  public static void Log (string src, string msg) =>
    DoLog($"{src} : {msg}");
  public static void Log (string src, string proc, string msg) => DoLog($"{src}.{proc} : {msg}");
  public static void LogException (Exception e) =>
    LogFrom(e?.Source, e?.TargetSite?.Name, e?.Message);
  /// <summary>
  /// Logs a message to the output stream.
  /// </summary>
  /// <param name="src">The orignating class.</param>
  /// <param name="target">The originating method.</param>
  /// <param name="msg">The message to log.</param>
  private static void LogFrom (string? src, string? target, string? msg) =>
    DoLog($"{src}.{target} : {msg}");

}
