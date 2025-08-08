using System.IO;

namespace Common.Extensions;

/// <summary>
/// Extensions for <see cref="StreamReader"/> objects.
/// </summary>
public static class StreamReaderExtensions
{
  /// <summary>
  /// Resets the <see cref="StreamReader"/> to the beginning of the stream.
  /// </summary>
  /// <param name="reader">The <see cref="StreamReader"/> to return to the beginning.</param>
  public static void Reset (this StreamReader? reader) => reader?.BaseStream.Position = 0;
}