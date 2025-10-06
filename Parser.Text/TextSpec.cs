#pragma warning disable CA1822 // Mark members as static

using Parser.Ops;
using Parser.Text.Ops;

using RO = System.Text.RegularExpressions.RegexOptions;

namespace Parser.Text;

/// <summary>
/// Defines a specification for parsing text files.
/// </summary>
public class TextSpec : Spec
{
  #region Static Specs
  /// <summary>
  /// Splits a string into a <see cref="Collection{T}"/> of <see langword="string"/> objects.
  /// </summary>
  public static TextSpec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitByLinesOperation("initial", "result"),
      Operation.End
    ]
  };
  #endregion

  #region Private Members
  /// <summary>
  /// Sets the specified options bit to the given value;
  /// </summary>
  /// <param name="opt">The bit to set.</param>
  /// <param name="value">The value to set it as.</param>
  private void SetFlag (RO opt, bool? value) =>
    RxOpt = (value ?? false) ? RxOpt | opt : RxOpt & ~opt;
  #endregion

  /// <summary>
  /// Determines whether to use a byte parser or a text one.
  /// </summary>
  public bool IsTextFile => true;
  /// <summary>
  /// Token types that are basic building blocks.
  /// </summary>
  public Collection<string> RegexBasicTokens { get; init; } = [];
  /// <summary>
  /// Token types to ignore.
  /// </summary>
  public Collection<string> WhitespaceTokens { get; init; } = [];
  public Collection<string> AllTokens => RegexBasicTokens.Concat(WhitespaceTokens).ToCollection();
  /// <summary>
  /// The regex options to use.
  /// </summary>
  public RO RxOpt { get; private set; }
  /// <summary>
  /// The string comparison type to use.
  /// </summary>
  public StringComparison SC => RxOpt.HasFlag(RO.IgnoreCase) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
  #region Regex Properties
  public bool? ExplicitCapture { get; init; } = false;
  /// <summary>
  /// Case insensitive match.
  /// </summary>
  public bool? CaseInsensitive { get; init; } = false;
  /// <summary>
  /// $ and ^ match the start and end of each line (not the whole string).
  /// </summary>
  public bool? MultiLine { get; init; } = true;
  /// <summary>
  /// Ignores whitepace that is not explicitly defined or escaped.
  /// </summary>
  public bool? IgnorePatternWhitespace { get; init; } = true;
  /// <summary>
  /// Expression will not backtrack.
  /// </summary>
  public bool? NonBacktracking { get; init; } = false;
  /// <summary>
  /// Dot matches newline characters.
  /// </summary>
  public bool? SingleLine { get; init; } = false;
  #endregion
  /// <summary>
  /// Defines a new <see cref="TextSpec"/>.
  /// </summary>
  public TextSpec ()
  {
    SetFlag(RO.IgnoreCase, CaseInsensitive);
    SetFlag(RO.IgnorePatternWhitespace, IgnorePatternWhitespace);
    SetFlag(RO.Multiline, MultiLine);
    SetFlag(RO.ExplicitCapture, ExplicitCapture);
    SetFlag(RO.NonBacktracking, NonBacktracking);
    SetFlag(RO.Singleline, SingleLine);
  }
}
