namespace Parser.Text.Tokens;

/// <summary>
/// Stores the regular expression options for the currently running specification.
/// </summary>
public static class TokenOptions
{
  public static TextSpec ActiveSpec { get; set; } = null!;
  /// <summary>
  /// The <see cref="StringComparison"/> to use based on the above flags.
  /// </summary>
  public static StringComparison SC => IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

  #region Regex Properties
  /// <summary>
  /// The bits all condensed into a single flag set.
  /// </summary>
  public static RegexOptions All { get; private set; } = RegexOptions.None;
  /// <summary>
  /// <see langword="true"/> if we do not care about case.
  /// </summary>
  public static bool IgnoreCase => All.HasFlag(RegexOptions.IgnoreCase);
  #endregion

  /// <summary>
  /// Loads the regular expression flags.
  /// </summary>
  /// <param name="options">The options to set.</param>
  /// <param name="spec">The specification currently loaded.</param>
  public static void LoadSpec (RegexOptions options, TextSpec spec)
  {
    spec.Load();
    ActiveSpec = spec;
  }
}
