#pragma warning disable CA1822 // Mark members as static

using Parser.Text.Ops;

namespace Parser.Text;

public class TextSpec : Spec
{
  /// <summary>
  /// Splits a string into a <see cref="Collection{T}"/> of <see langword="string"/> objects.
  /// </summary>
  public static TextSpec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitByLinesOperation("initial", "results"),
      Operation.End
    ]
  };

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
  /// Get => Returns the backing field or an empty Dictionary.
  /// Set => Individually adds each item from the value supplied.
  /// 
  /// Setting Null clears the list.
  /// </summary>
  public Collection<TokenType> TokenLookup
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
  public bool? MultiLine { get; init; } = true;
  /// <summary>
  /// Ignores whitepace that is not explicitly defined or escaped.
  /// </summary>
  public bool? IgnorePatternWhitespace { get; init; } = true;
  /// <summary>
  /// Expression will not backtrack.
  /// </summary>
  public bool? NonBacktracking { get; init; } = false;
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
  public void Load ()
  {
    TokenOptions.LoadRegexSpec(_options);
    TokenOptions.LoadTokenSpec([.. TokenLookup]);
  }

  /// <summary>
  /// Checks to see if a group is able to be ignored by future operations.
  /// </summary>
  /// <param name="groupName">The group name to check.</param>
  /// <returns><see langword="true"/> if the <see cref="TextSpec"/> TokenLookup contains that group and the group contains the <see cref="TokenType"/> T_Ignore.</returns>
  public bool IsIgnoreGroup (string groupName) =>
    TokenLookup.Contains(groupName.ToLowerInvariant());
}
