#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class ChkToken<T> : IEquatable<IToken<T>> where T : notnull
{
  private readonly string _data_string;

  public required RT TokenRule { get; init; } = RT.None;
  public Collection<T> AllowedTypes { get; init; } = [];
  public Collection<string> AllowedContents { get; init; } = [];
  public bool UseAsLiteral { get; init; }
  public bool IgnoreCase => TokenRule.HasFlag(RT.IgnoreCase);
  internal bool AllowAnyToken =>
    AllowedTypes.IsEmpty() &&
    AllowedContents.IsEmpty();
  internal StringComparison SC => IgnoreCase ? SCOIC : SCO;

  /// <summary>Builds a <see cref="ChkToken{T}"/> from a <see langword="string"/>.<br/>
  /// <list type="bullet">
  /// <listheader>Syntax</listheader>
  /// <item><c>prefix:value</c></item>
  /// <item><c>prefix:(value1|value2|value3...)</c></item>
  /// </list>
  /// </summary>
  /// <param name="data_string">The individual token defintion.</param>
  /// <remarks>
  /// <list type="table">
  /// <listheader>Specifications</listheader>
  /// <item>t</item>
  /// </list>
  /// <list type="table">
  /// <listheader>Examples</listheader>
  /// <item><c>tn:Name</c> - Token Type 'Name', Store as a Name.</item>
  /// <item><c>tv:Dec</c> - Token Type 'Dec', store as a Value.</item>
  /// <item><c>cyi:Script</c> - Content 'Script', Case insensitive, store as a Type.</item>
  /// </list>
  /// </remarks>
  public ChkToken (string data_string)
  {
    _data_string = data_string;
  }

  internal bool Check_Type (IToken<T>? token) => token is not null && token.HasType && AllowedTypes.Any(type => Equals(token.Type, type)) || AllowedTypes.Count == 0;
  internal bool Check_Content (IToken<T>? token) => token is not null && token.Content.Length > 0 && AllowedContents.Any(str => token.Content.Equals(str, SC)) || AllowedContents.Count == 0;
  public bool Equals (IToken<T>? other) =>
    Check_Content(other) && Check_Type(other);
  public override string ToString () => $"ChkToken: {_data_string}";
}
