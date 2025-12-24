#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;

/// <summary>Shorthand for common enum names.</summary>
public static class Names
{
  public const BindingFlags
    BFCI = BindingFlags.CreateInstance,
    BFP = BindingFlags.Public,
    BFS = BindingFlags.Static;
  /// <summary>No split alterations.</summary>
  public const StringSplitOptions SSON = StringSplitOptions.None;
  /// <summary>Trim and remove empty splits.</summary>
  public const StringSplitOptions SSORT = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
  /// <summary>Ordinal (Case-Sensitive).</summary>
  public const StringComparison SCO = StringComparison.Ordinal;
  /// <summary>Ordinal (Case-Insensitive).</summary>
  public const StringComparison SCOIC = StringComparison.OrdinalIgnoreCase;

  public const NumberStyles
    NSN = NumberStyles.None,
    NSALS = NumberStyles.AllowLeadingSign,
    NSAP = NumberStyles.AllowParentheses,
    NSAT = NumberStyles.AllowThousands,
    NSADP = NumberStyles.AllowDecimalPoint,
    NSABS = NumberStyles.AllowBinarySpecifier,
    NSAHS = NumberStyles.AllowHexSpecifier,
    NSALW = NumberStyles.AllowLeadingWhite;

  /// <summary>No regular expression options.</summary>
  public const RegexOptions RON = RegexOptions.None;
  /// <summary>Dot matches newline.</summary>
  public const RegexOptions ROSL = RegexOptions.Singleline;
  /// <summary>Caret and dollar tokens match beginning and end of lines, not just the string end.</summary>
  public const RegexOptions ROML = RegexOptions.Multiline;
  /// <summary>Case Insensitive.</summary>
  public const RegexOptions ROIC = RegexOptions.IgnoreCase;
  /// <summary>Alternate model, does not catostrphically backtrack.</summary>
  public const RegexOptions RONB = RegexOptions.NonBacktracking;
  /// <summary>Ignore pattern whitespace.</summary>
  public const RegexOptions ROIPW = RegexOptions.IgnorePatternWhitespace;
  /// <summary>Right to left, executes regular expression backwards.</summary>
  public const RegexOptions ROR2L = RegexOptions.RightToLeft;
  /// <summary>Explicit capture, only stores named groups.</summary>
  public const RegexOptions ROEC = RegexOptions.ExplicitCapture;

  public static readonly CultureInfo
    CICC = CultureInfo.CurrentCulture,
    CIIC = CultureInfo.InvariantCulture,
    CICUIC = CultureInfo.CurrentUICulture;

  /// <summary>Readable name for the '-1' not found indicator.</summary>
  public const int
    ErrVal = -1,
    NotFound = -1,
    DNE = -1;

  /// <summary>String empty reference</summary>
  public static readonly string SE = string.Empty;

  /// <summary>Const empty string for compile time availability</summary>
  public const string EmptyString = "";
}
