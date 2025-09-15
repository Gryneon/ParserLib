#pragma warning disable CA1822 // Mark members as static

using Parser.Text.Ops;

namespace Parser.Text;

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
  private RegexOptions _options;
  /// <summary>
  /// Sets the specified options bit to the given value;
  /// </summary>
  /// <param name="opt">The bit to set.</param>
  /// <param name="value">The value to set it as.</param>
  private void SetFlag (RegexOptions opt, bool? value) =>
    _options = (value ?? false) ? _options | opt : _options & ~opt;
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
  /// <summary>A list of tokenizable matches.</summary>
  /// <remarks>
  /// Get => Returns the backing field or an empty Dictionary.<br/>
  /// Set => Individually adds each item from the value supplied.
  /// 
  /// Setting Null clears the list.
  /// </remarks>
  public Collection<string> TokenLookup
  {
    get => field ?? [];
    set
    {
      field ??= [];

      if (value is not null)
        field.AddRange(value);
    }
  } = [];
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
    SetFlag(RegexOptions.IgnoreCase, CaseInsensitive);
    SetFlag(RegexOptions.IgnorePatternWhitespace, IgnorePatternWhitespace);
    SetFlag(RegexOptions.Multiline, MultiLine);
    SetFlag(RegexOptions.ExplicitCapture, ExplicitCapture);
    SetFlag(RegexOptions.NonBacktracking, NonBacktracking);
    SetFlag(RegexOptions.Singleline, SingleLine);
  }

  /// <summary>
  /// Loads this <see cref="TextSpec"/> to <see cref="TokenOptions"/>.
  /// </summary>
  public void Load () => TokenOptions.LoadSpec(_options, this);
}
