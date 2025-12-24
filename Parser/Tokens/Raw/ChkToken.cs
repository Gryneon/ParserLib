#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class ChkToken<T> : IEquatable<IToken<T>> where T : notnull
{
  private readonly string _data_string;

  public RT TokenRule { get; private set; } = RT.None;
  public Collection<T> AllowedTypes { get; init; } = [];
  public Collection<string> AllowedContents { get; init; } = [];
  public bool UseAsLiteral { get; private set; }
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
    ParseDataString();
  }

  internal void ParseDataString ()
  {
    int colon = _data_string.IndexOf(':', SCO);
    string pre = _data_string[..(colon - 1)];
    string post = _data_string[(colon + 1)..];

    RT rule = RT.None;
    rule |= pre.Contains('x', SCOIC) ? RT.IgnoredToken : RT.None;
    rule |= pre.Contains('m', SCOIC) ? RT.Mult : RT.None;
    rule |= pre.Contains('o', SCOIC) ? RT.Opt : RT.None;
    rule |= pre.Contains('i', SCOIC) ? RT.IgnoreCase : RT.None;
    pre = pre.RemoveChars("xmoi");

    /*    
    t - Value is Token Type
    c - Value is String Literal

Can have one of each of these:

    i - Ignore Case (String Literal Only)
    m - One or many, this token will repeat as long as it can, Possessive, Greedy.
    o - Optional, this token does not trigger a fail if it does not match. Greedy.

Must have only one of these:

    x - Ignore Token
    n - Token is 'Name' in object
    y - Token is 'Type' in object
    v - Token is 'Value' in object
    p - Token is 'Property' in object
    f - Token is 'AddFlag' and is additive.
    r - Token is 'RemFlag' and is subtractive.
    */

    if (pre.Contains('t', SCOIC))
      UseAsLiteral = false;
    else if (pre.Contains('c', SCOIC))
      UseAsLiteral = true;

    TokenRule = (RT) (int) rule + (int) (pre.RemoveChars("tc") switch
    {
      "y" => RT.AssignType,
      "v" => RT.AssignValue,
      "n" => RT.AssignName,
      "p" => RT.AddProperty,
      "f" => RT.AddFlag,
      "r" => RT.RemFlag,
      "x" => RT.IgnoredToken,
      _ => throw new InvalidOperationException("Unknown letter encountered.")
    });

    if (post.StartsWith('(') && post.EndsWith(')'))
    {
      post = post[1..^1];
      IEnumerable<string> strs = post.Split(['-', '|', '+', '&'], 0, SSORT);

      foreach (string s in strs)
      {
        TryAddRule(s);
      }
    }
    else
    {
      TryAddRule(post);
    }
  }
  internal void TryAddRule (string s)
  {
    if (UseAsLiteral)
      AddToContents(s);
    else
      AddToTypes(s);
  }
  internal void AddToTypes (string s) => AllowedTypes.Add(s.ToEnum<T>());
  internal void AddToContents (string s) => AllowedContents.Add(s);
  internal bool Check_Type (IToken<T>? token) => token is not null && token.HasType && AllowedTypes.Any(type => Equals(token.Type, type)) || AllowedTypes.Count == 0;
  internal bool Check_Content (IToken<T>? token) => token is not null && token.Content.Length > 0 && AllowedContents.Any(str => token.Content.Equals(str, SC)) || AllowedContents.Count == 0;
  public bool Equals (IToken<T>? other) =>
    Check_Content(other) && Check_Type(other);
  public override string ToString () => $"ChkToken: {_data_string}";
}
