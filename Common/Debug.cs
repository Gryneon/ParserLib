//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.IO;

namespace Common;

/// <summary>
/// Static class containing debugging logs.
/// </summary>
public static class Debug
{
  /// <summary>
  /// Set to <see langword="true"/> to output debugging information to the output stream.
  /// </summary>
  public static bool Verbose { get; set; }
  /// <summary>
  /// Sets the output stream.
  /// </summary>
  /// <param name="stream">The stream to output to.</param>
  public static void SetStream (TextWriter stream) => Console.SetOut(stream);

  private static void _doLog (string msg)
  {
#if DEBUG
    if (Verbose) Console.WriteLine(msg);
#endif
  }
  /// <summary>
  /// Logs a message to the output stream.
  /// </summary>
  /// <param name="msg">The message to log.</param>
  public static void Log (string msg) =>
    _doLog(msg);
  public static void Log (string src, string msg) =>
    _doLog($"{src} : {msg}");

  public static void LogException (Exception e) =>
    LogFrom(e?.Source, e?.TargetSite?.Name, e?.Message);

  private static void LogFrom (string? src, string? target, string? msg) =>
    _doLog($"{src}.{target} : {msg}");
}