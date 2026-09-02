#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

using System.Numerics;
using System.Text;

namespace Common.Extensions;

public static class SpanExtensions
{
  private static void Adjust (ref int value, bool big_endian) => value = big_endian ? value - 1 : value + 1;
  private static int Init (int size, bool big_endian) => big_endian ? size - 1 : 0;

  private static unsafe T Calc<T> (bool big_endian, ReadOnlySpan<byte> bytes) where T : struct, IMinMaxValue<T>, INumber<T>, IIncrementOperators<T>
  {
    int size = sizeof(T);

    ArgumentOutOfRangeException.ThrowIfNotEqual(size, bytes.Length);

    long result = 0;
    for (int p = 0; p < size; p++)
    {
      int i = big_endian ? size - 1 - p : p;
      result += bytes[i] << (p * 8);
    }

    dynamic dynamicResult = result;

    return (T) dynamicResult;
  }
  /// <summary>Span Extensions</summary>
  /// <param name="buffer">The span buffer to read from.</param>
  extension(Span<byte> buffer)
  {
    public short ToInt16 (bool big_endian = false) => Calc<short>(big_endian, buffer);
    public short AsShort => buffer.ToInt16();
    public short AsShortBE => buffer.ToInt16(true);
    public int ToInt32 (bool big_endian = false) => Calc<int>(big_endian, buffer);
    public int AsInt => buffer.ToInt32();
    public int AsIntBE => buffer.ToInt32(true);
    public long ToInt64 (bool big_endian = false) => Calc<long>(big_endian, buffer);
    public long AsLong => buffer.ToInt64();
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
    public int AsInt => buffer.Span.AsInt;
    public int AsIntBE => buffer.Span.AsIntBE;
    public short AsShort => buffer.Span.AsShort;
    public long AsInt64 => buffer.Span.AsLong;
  }
}
