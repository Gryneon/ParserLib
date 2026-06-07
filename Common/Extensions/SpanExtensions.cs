//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Text;

namespace Common.Extensions;

public static class SpanExtensions
{
  /// <summary>Span Extensions</summary>
  /// <param name="buffer">The span buffer to read from.</param>
  extension(Span<byte> buffer)
  {
    public short ToInt16 ()
    {
      if (buffer.Length > 2)
        throw new ArgumentOutOfRangeException(nameof(buffer));

      int result = 0;

      for (int i = 0; i < buffer.Length; i++)
        result += buffer[i] << i * 8;

      return (short) result;
    }
    public int ToInt32 ()
    {
      if (buffer.Length > 4)
        throw new ArgumentOutOfRangeException(nameof(buffer));

      int result = 0;

      for (int i = 0; i < buffer.Length; i++)
        result += buffer[i] << i * 8;

      return result;
    }
    public long ToInt64 ()
    {
      if (buffer.Length != 8)
        throw new ArgumentOutOfRangeException(nameof(buffer));

      long result = 0;

      for (int i = 0; i < buffer.Length; i++)
        result += buffer[i] << i * 8;

      return result;
    }

    public string ByteArrToString (string? encoding = null, string? endianness = null)
    {
      Encoding getEncoding () => encoding switch
      {
        "UTF7" or "UTF8" => Encoding.UTF8,
        "UTF16" when endianness.Like("Big") => Encoding.BigEndianUnicode,
        "UTF16" => Encoding.Unicode,
        "UTF32" => Encoding.UTF32,
        _ => Encoding.UTF8,
      };

      return new(getEncoding().GetChars(buffer.ToArray()));
    }
  }

  extension(Memory<byte> buffer)
  {
    public int ToInt32 ()
    {
      return buffer.Span.ToInt32();
    }
    public short ToInt16 ()
    {
      return buffer.Span.ToInt16();
    }
    public long ToInt64 ()
    {
      return buffer.Span.ToInt64();
    }
  }
}
