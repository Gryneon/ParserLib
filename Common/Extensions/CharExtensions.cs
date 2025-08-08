//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class CharExtensions
{
#pragma warning disable IDE1006 // Naming Styles
  private const char
    _a = 'a', _z = 'z', _Z = 'Z', _A = 'A';
#pragma warning restore IDE1006 // Naming Styles

  public static bool IsAlphanumeric (this char c) =>
    c.IsLower() || c.IsUpper() || c.In('0', '9');
  /// <summary>
  /// Checks if this character is lowercase a thru z.
  /// </summary>
  /// <param name="c">The character to check.</param>
  /// <returns>true is the character is between ascii 'a' and 'z', false otherwise.</returns>
  public static bool IsLower (this char c) => c.In(_a, _z);
  public static bool IsUpper (this char c) => c is >= _A and <= _Z;
  public static bool IsControl (this char c) => c < 32;
  public static bool Is16bit (this char c) => c.In(256, short.MaxValue);
  public static bool IsNumber (this char c) => c.IsInteger() || c is '.';
  public static bool IsInteger (this char c) => c.IsPosInteger() || c is '-';
  public static bool IsPosInteger (this char c) => c.In('0', '9');
  public static bool IsWhitespace (this char c) =>
    c.In(9, 13) || c.In(8192, 8202) || (int)c is 32 or 133 or 160 or
    5760 or 8232 or 8233 or 8239 or 8287 or 12288;
  public static bool Is (this char c, int value) => c == value;
  public static bool Is (this char c, char value) => c == value;
  public static bool Is (this char c, params int[] values) => values.Any(i => c.Is(i));
  public static bool In (this char c, int min, int max) => c >= min && c <= max;
  public static char ToLower (this char c) =>
    c.IsUpper() ? (char)(c + 32) : c;
  /// <summary>
  /// Gets the uppercase variant of this character, or the character, if there isn't one.
  /// </summary>
  /// <param name="c">The character to make uppercase.</param>
  /// <returns>The uppercase variant, or the character itself.</returns>
  public static char ToUpper (this char c) =>
    c.IsLower() ? (char)(c - 32) : c;
}