//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

/// <summary>Extension methods for the <see cref="char"/> type.</summary>
public static class CharExtensions
{
  private const char
    _a = 'a', _z = 'z', _Z = 'Z', _A = 'A';
  /// <summary>Char Extensions</summary>
  /// <param name="c">The character to check.</param>
  extension(char c)
  {

    public bool IsAlphanumeric () =>
      c.IsLower() || c.IsUpper() || c.In('0', '9');
    /// <summary>Checks if this character is lowercase a thru z.</summary>
    /// <returns>true is the character is between ascii 'a' and 'z', false otherwise.</returns>
    public bool IsLower () => c.In(_a, _z);
    public bool IsUpper () => c is >= _A and <= _Z;
    public bool IsControl () => c < 32;
    /// <summary>Checks if this character must be represented in 16 bits (256-65535).</summary>
    /// <returns><see langword="true"/> if it is within the specified range, otherwise <see langword="false"/>.</returns>
    public bool Is16bit () => c.In(256, short.MaxValue);
    public bool IsNumber () => c.IsInteger() || c is '.';
    public bool IsInteger () => c.IsPosInteger() || c is '-';
    public bool IsPosInteger () => c.In('0', '9');
    /// <summary>Checks if a character is considered whitespace by Unicode standards.</summary>
    /// <returns><see langword="true"/> if the character is whitespace, otherwise <see langword="false"/>.</returns>
    public bool IsWhitespace () =>
      c.In(9, 13) || c.In(8192, 8202) || (int) c is 32 or 133 or 160 or
      5760 or 8232 or 8233 or 8239 or 8287 or 12288;
    public bool Is (int value) => c == value;
    public bool Is (char value) => c == value;
    public bool Is (params int[] values) => values.Any(i => c.Is(i));
    public bool In (int min, int max) => c >= min && c <= max;
    public char ToLower () =>
      c.IsUpper() ? (char) (c + 32) : c;
    /// <summary>Gets the uppercase variant of this character, or the character, if there isn't one.</summary>
    /// <returns>The uppercase variant, or the character itself.</returns>
    public char ToUpper () =>
      c.IsLower() ? (char) (c - 32) : c;
  }
}
