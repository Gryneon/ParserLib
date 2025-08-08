//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class IntExtensions
{
  public static bool HasFlag (this int value, IConvertible flag) => (value & (int)flag) == (int)flag;
  public static T SetFlag<T> (this int value, IConvertible flag) where T : IConvertible => (T)(IConvertible)(value | (int)flag);
  public static T MaskFlag<T> (this int value, IConvertible mask) where T : IConvertible => (T)(IConvertible)(value & (int)mask);
  public static T Toggle<T> (this int value, IConvertible bit) where T : IConvertible =>
    (T)(IConvertible)(
      HasFlag(value, bit) ?
      MaskFlag<T>(value, bit) :
      SetFlag<T>(value, bit)
    );
}
