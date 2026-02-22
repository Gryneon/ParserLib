//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Text;

namespace Common.Extensions;

public static class SpanExtensions
{
  // Span<byte>
  public static short ToInt16 (this Span<byte> buffer)
  {
    if (buffer.Length > 2)
      throw new ArgumentOutOfRangeException(nameof(buffer));

    int result = 0;

    for (int i = 0; i < buffer.Length; i++)
      result += buffer[i] << i * 8;

    return (short) result;
  }
  public static int ToInt32 (this Span<byte> buffer)
  {
    if (buffer.Length > 4)
      throw new ArgumentOutOfRangeException(nameof(buffer));

    int result = 0;

    for (int i = 0; i < buffer.Length; i++)
      result += buffer[i] << i * 8;

    return result;
  }
  public static int ToInt32 (this Memory<byte> buffer) => buffer.Span.ToInt32();
  public static long ToInt64 (this Span<byte> buffer)
  {
    if (buffer.Length > 8)
      throw new ArgumentOutOfRangeException(nameof(buffer));

    long result = 0;

    for (int i = 0; i < buffer.Length; i++)
      result += buffer[i] << i * 8;

    return result;
  }

  public static string ByteArrToString (this Span<byte> buffer) => new(Encoding.UTF8.GetChars(buffer.ToArray()));
}
