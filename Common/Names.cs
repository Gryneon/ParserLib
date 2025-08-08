#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>
/// Common enums 
/// </summary>
public static class Names
{
  public const BindingFlags
    BFCI = BindingFlags.CreateInstance;

  public const StringSplitOptions
    SSON = StringSplitOptions.None,
    SSORT = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

  public const StringComparison
    SCO = StringComparison.Ordinal,
    SCOIC = StringComparison.OrdinalIgnoreCase;

  public const NumberStyles
    NSN = NumberStyles.None,
    NSALS = NumberStyles.AllowLeadingSign,
    NSAP = NumberStyles.AllowParentheses,
    NSAT = NumberStyles.AllowThousands,
    NSADP = NumberStyles.AllowDecimalPoint,
    NSABS = NumberStyles.AllowBinarySpecifier,
    NSAHS = NumberStyles.AllowHexSpecifier,
    NSALW = NumberStyles.AllowLeadingWhite;

  public const RegexOptions
    RON = RegexOptions.None,
    ROSL = RegexOptions.Singleline,
    ROML = RegexOptions.Multiline,
    ROIC = RegexOptions.IgnoreCase,
    RONB = RegexOptions.NonBacktracking,
    ROIPW = RegexOptions.IgnorePatternWhitespace,
    ROR2L = RegexOptions.RightToLeft,
    ROEC = RegexOptions.ExplicitCapture;

  public static readonly CultureInfo
    CICC = CultureInfo.CurrentCulture,
    CIIC = CultureInfo.InvariantCulture,
    CICUIC = CultureInfo.CurrentUICulture;

  public const int
    ErrVal = -1,
    NotFound = -1,
    DNE = -1;

  /// <summary>
  /// String empty reference
  /// </summary>
  public static readonly string SE = string.Empty;

  /// <summary>
  /// Const empty string for compile time availability
  /// </summary>
  public const string EmptyString = "";
}
