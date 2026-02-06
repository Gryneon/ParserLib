//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class IConvertibleExtensions
{
  /// <summary>
  /// Gets a numeric integer from any enumeration.
  /// </summary>
  /// <param name="e">The enumeration to convert.</param>
  /// <returns>The enumeration value.</returns>
  public static int ToInt (this IConvertible e) => (int) e;
  /// <summary>
  /// Gets a 64 bit numeric integer from any enumeration.
  /// </summary>
  /// <param name="e">The enumeration to convert.</param>
  /// <returns>The enumeration value.</returns>
  public static long ToLong (this IConvertible e) => (long) e;
  /// <summary>
  /// Removes all of the provided bits from an enum.
  /// </summary>
  /// <typeparam name="T">The type of enum to return.</typeparam>
  /// <param name="i">The field to deflag.</param>
  /// <param name="bit">The flags to remove.</param>
  /// <returns>An enum of the given type with the given flags removed.</returns>
  public static T RemoveBit<T> (this IConvertible i, IConvertible bit) where T : IConvertible => (T) (IConvertible) ((int) i & ~(int) bit);
  public static T RemoveBitLong<T> (this IConvertible i, IConvertible bit) where T : IConvertible => (T) (IConvertible) ((long) i & ~(long) bit);
  /// <summary>
  /// Checks if the enum contains any of the provided bits.
  /// </summary>
  /// <param name="e">The enum to check.</param>
  /// <param name="check">The bits to check for.</param>
  /// <returns><see langword="true"/> if the enum contains any of the bits provided, <see langword="false"/> otherwise.</returns>
  public static bool HasAnyFlag (this IConvertible e, IConvertible check) => ((int) e & (int) check) > 0;
  public static bool IsWithin (this IConvertible e, IConvertible low, IConvertible high) =>
    (decimal) e <= (decimal) high && (decimal) e >= (decimal) low;
  public static T Mask<T> (this IConvertible value, IConvertible mask) where T : IConvertible => (T) (IConvertible) ((int) value & (int) mask);
}
