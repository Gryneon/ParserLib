#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

/// <summary>Builds a <see cref="ChkToken"/> from a <see langword="string"/>.<br/>
/// <list type="bullet">
/// <listheader>Syntax</listheader>
/// <item><c>prefix:value</c></item>
/// <item><c>prefix:(value1|value2|value3...)</c></item>
/// </list>
/// </summary>
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
public sealed class ChkToken () : IEquatable<IToken>
{
  internal static Dictionary<char, RT> LetterReference { get; } = new()
  {
    ['a'] = RT.Any,
    ['b'] = RT.Error,
    ['c'] = RT.Error,
    ['d'] = RT.Descendant,
    ['e'] = RT.Error,
    ['f'] = RT.AddFlag,
    ['g'] = RT.Error,
    ['h'] = RT.Error,
    ['i'] = RT.IgnoreCase,
    ['j'] = RT.Error,
    ['k'] = RT.Error,
    ['l'] = RT.Error,
    ['m'] = RT.Mult,
    ['n'] = RT.AssignName,
    ['o'] = RT.Opt,
    ['p'] = RT.AddProperty,
    ['q'] = RT.Error,
    ['r'] = RT.RemFlag,
    ['s'] = RT.Error,
    ['t'] = RT.Error,
    ['u'] = RT.Error,
    ['v'] = RT.AssignValue,
    ['w'] = RT.Error,
    ['x'] = RT.IgnoredToken,
    ['y'] = RT.AssignType,
    ['z'] = RT.Error,
  };
  public required RT TokenRule { get; init; } = RT.None;
  public Collection<string> AllowedTypes { get; init; } = [];
  public Collection<string> AllowedContents { get; init; } = [];
  public bool IgnoreCase => TokenRule.HasFlag(RT.IgnoreCase);
  private StringComparison SC => IgnoreCase ? SCOIC : SCO;
  public static ChkToken Parse (string definition)
  {
    RxS regex = RxS.Rx(@"^(?'prefix'\w+)\:(?:\((?'type_def'[^,&|+}-]+)([,&+|-](?'type_def'[^,&+|}-]+))*\)|\{(?'literal_def'[^,&|+}-]+)([,&+|-](?'literal_def'[^,&+|}-]+))*\}|(?'type_def'\w+))+");
    Regex regexobj = new(regex, ROEC, new TimeSpan(0, 0, 1));
    Match m = regexobj.Match(definition);
    if (!m.Success)
      throw new ArgumentException($"Bad Token Sequence String. {definition}");

    string prefix = m.Groups["prefix"].Value;
    Collection<string> allowed_types = [];
    Collection<string> allowed_literals = [];
    RT rule = RT.None;

    foreach (char c in prefix)
    {
      rule |= LetterReference[c];
    }

    if (rule.HasFlag(RT.Error))
    {
      throw new ArgumentException($"Bad Prefix Char. {prefix}");
    }

    if (m.Groups.ContainsKey("type_def"))
      foreach (Capture c in m.Groups["type_def"].Captures)
        allowed_types.Add(c.Value);

    if (m.Groups.ContainsKey("literal_def"))
      foreach (Capture c in m.Groups["literal_def"].Captures)
        allowed_literals.Add(c.Value);

    ChkToken result = new()
    {
      AllowedContents = allowed_literals,
      AllowedTypes = allowed_types,
      TokenRule = rule
    };

    return result;
  }
  internal bool Check_Type (IToken? token) => token is not null && token.HasType && AllowedTypes.Any(type => token.Type.Like(type)) || AllowedTypes.Count == 0;
  internal bool Check_Content (IToken? token) => token is not null && token.Content.Length > 0 && AllowedContents.Count > 0 && AllowedTypes.Any(i => i.Equals(token.Content, SC)) || AllowedContents.Count == 0;
  public bool Equals (IToken? other) =>
    Check_Content(other) && Check_Type(other);
  public override string ToString () => $"ChkToken: {AllowedTypes.TextJoin("-")}" + (AllowedContents.Count > 0 ? $"{{{AllowedContents.TextJoin("|")}}}" : "");

}
