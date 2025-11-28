//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

/// <summary>
/// Integer extensions for bitwise operations.
/// </summary>
public static class IntExtensions
{
  /// <summary>Checks if a specific flag is set in the integer value.</summary>
  /// <param name="value">The integer value to look in.</param>
  /// <param name="flag">The flag to look for.</param>
  /// <returns>Returns <see langword="true"/> if the flag is set, <see langword="false"/> otherwise.</returns>
  public static bool HasFlag (this int value, IConvertible flag) => (value & (int) flag) == (int) flag;
  /// <summary>
  /// Sets a specific flag in the integer value.
  /// </summary>
  /// <typeparam name="T">The type of integer value to return.</typeparam>
  /// <param name="value">The integer value to modify.</param>
  /// <param name="flag">The flag to set.</param>
  /// <returns>The resulting value.</returns>
  public static T SetFlag<T> (this int value, IConvertible flag) where T : IConvertible => (T) (IConvertible) (value | (int) flag);
  /// <summary>Masks a specific flag in the integer value.</summary>
  /// <typeparam name="T">The type of integer value to return.</typeparam>
  /// <param name="value">The integer value to modify.</param>
  /// <param name="mask">The flag to mask.</param>
  /// <returns>The masked value.</returns>
  public static T MaskFlag<T> (this int value, IConvertible mask) where T : IConvertible => (T) (IConvertible) (value & (int) mask);
  /// <summary>
  /// Toggles a specific flag in the integer value.
  /// </summary>
  /// <typeparam name="T">The type of integer value to return.</typeparam>
  /// <param name="value">The integer value to modify.</param>
  /// <param name="bit">The flag to toggle.</param>
  /// <returns></returns>
  public static T Toggle<T> (this int value, IConvertible bit) where T : IConvertible =>
    (T) (IConvertible) (
      HasFlag(value, bit) ?
      MaskFlag<T>(value, bit) :
      SetFlag<T>(value, bit)
    );
}
