#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Net.NetworkInformation;

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
  private Spec? _spec;
  internal static Dictionary<char, RT> LetterReference { get; } = new()
  {
    ['a'] = RT.Any,
    ['b'] = RT.Error,
    ['c'] = RT.AssignCenter,
    ['d'] = RT.Descendant,
    ['e'] = RT.Error,
    ['f'] = RT.AddFlag,
    ['g'] = RT.SubFlag,
    ['h'] = RT.Error,
    ['i'] = RT.IgnoreCase,
    ['j'] = RT.Error,
    ['l'] = RT.AssignLeft,
    ['m'] = RT.Mult,
    ['n'] = RT.AssignName,
    ['o'] = RT.Opt,
    ['p'] = RT.AddProperty,
    ['q'] = RT.AddParameter,
    ['r'] = RT.AssignRight,
    ['s'] = RT.AddStatement,
    ['t'] = RT.AssignType,
    ['u'] = RT.Error,
    ['v'] = RT.AssignValue,
    ['w'] = RT.Error,
    ['x'] = RT.IgnoredToken,
    ['y'] = RT.Error,
    ['z'] = RT.Error,

    //['<'] = RT.LookAround,
    //['>'] = RT.LookAround,
    //['!'] = RT.Negative,
  };
  public required RT TokenRule { get; init; } = RT.None;
  public string? CustomPropertyName { get; set; }
  public Collection<string> AllowedTypes { get; init; } = [];
  public string RegexValidator { get; init; } = SE;
  public bool IgnoreCase => TokenRule.HasFlag(RT.IgnoreCase);
  public bool LookAround => TokenRule.HasFlag(RT.LookAround);
  public bool Negative => TokenRule.HasFlag(RT.Negative);
  private StringComparison SC => IgnoreCase || _spec?.SC == SCOIC ? SCOIC : SCO;
  /// <summary>Creates a <see cref="ChkToken"/> object from the <see langword="string"/>.</summary>
  /// <param name="definition">The definition string.</param>
  /// <param name="spec">The <see cref="Spec"/> reference.</param>
  /// <returns>A <see cref="ChkToken"/> object from the <see langword="string"/>.</returns>
  /// <exception cref="ArgumentException"/>
  public static ChkToken Parse (string definition, Spec spec)
  {
    RxS regex = RxS.Rx(@"^((?'prefix'[\w=!><]+)|\[(?'custom_prop'\w+)\])\:(?:\((?'type_def'[^,&|+}-]+)([,&+|-](?'type_def'[^,&+|}-]+))*\)|\{(?'literal_def'[^}]+)\}|(?'type_def'\w+))+");
    Regex regexobj = new(regex, ROEC, new TimeSpan(0, 0, 1));
    Match m = regexobj.Match(definition);
    if (!m.Success)
      _ = Op.ThrowBadDef($"Bad Token Sequence String. {definition}");

    RT rule = RT.None;
    string prefix = SE;
    string? prop = null;

    if (m.Groups.ContainsKey("custom_prop") && m.Groups["custom_prop"].Value.IsNotEmpty())
    {
      rule = RT.AssignCustomProp;
      prop = m.Groups["custom_prop"].Value;
    }
    else
    {
      prefix = m.Groups["prefix"].Value;
    }
    Collection<string> allowed_types = [];
    string regex_validator = SE;

    foreach (char c in prefix)
    {
      rule |= LetterReference[c];
    }

    if (rule.HasFlag(RT.Error))
    {
      _ = Op.ThrowBadDef($"Bad Prefix Char. ({prefix})");
    }

    if (m.Groups.ContainsKey("type_def"))
      foreach (Capture c in m.Groups["type_def"].Captures)
        allowed_types.Add(c.Value);

    if (m.Groups.ContainsKey("literal_def"))
      regex_validator = m.Groups["literal_def"].Value;

    Collection<string> expanded_types = spec is null ? allowed_types : AllAllowedTypes(allowed_types, spec);

    ChkToken result = new()
    {
      CustomPropertyName = prop,
      RegexValidator = regex_validator,
      AllowedTypes = expanded_types,
      TokenRule = rule,
      _spec = spec
    };

    return result;
  }
  private bool IsFullRegexMatch (IToken? token) =>
    token is not null && (RegexValidator.IsEmpty() || Regex.Match(token.Content, RegexValidator, ROEC | ROML | ROIPW | (TokenRule.HasFlag(RT.IgnoreCase) ? ROIC : RON)).Value.Length == token.Content.Length);
  internal static Collection<string> AllAllowedTypes (IEnumerable<string> types, Spec spec)
  {
    HashSet<string> all_types_allowed = [.. types];

    foreach (string item in types)
    {
      if (spec.TokenCompatLookup.Keys.Any(i => i.ToString().Equals(item, SCOIC)))
      {
        dynamic key = spec.TokenCompatLookup.Keys.Single(i => i.ToString().Equals(item, SCOIC));
        Collection<object> list = spec.TokenCompatLookup[key];
        foreach (string? s in list.Select(obj => obj.ToString()))
        {
          if (s is null) continue;

          bool added = all_types_allowed.Add(s);

          if (added)
          {
            all_types_allowed = [.. AllAllowedTypes(all_types_allowed, spec)];
          }
        }
      }
    }
    return [.. all_types_allowed];
  }
  internal bool Check_Type (IToken? token) => token is not null && token.HasType && AllowedTypes.Any(type => token.Type.Like(type)) || AllowedTypes.Count == 0;
  /// <summary>Checks if the specified token satisfies this object's conditions.</summary>
  /// <param name="other">The token to check.</param>
  /// <returns><see langword="true"/> if the token satisfies this object's conditions, <see langword="false"/> otherwise.</returns>
  public bool Equals (IToken? other) =>
    IsFullRegexMatch(other) && Check_Type(other);
  public override string ToString () => $"ChkToken: {AllowedTypes.TextJoin("-")}" + (RegexValidator.Length > 0 ? $"{{{RegexValidator}}}" : "");
}
