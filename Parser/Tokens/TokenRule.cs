#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

public class TokenRule
{
  public required RT Type { get; init; }
  public string? RuleStringData { get; init; }
  public string TypeToAssign { get; set; }

  [SetsRequiredMembers]
  public TokenRule (RT type, string typeToAssign, [SS("regex")] string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, object typeToAssign, [SS("regex")] string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, string typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, object typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  public TokenRule ()
  {
    TypeToAssign = SE;
  }
  /// <summary>Makes a series of single character TokenExact styled rules.</summary>
  /// <param name="chars">The characters to use.</param>
  /// <param name="type">The rule type. Should be TokenExact or SplitExact, plus any necessary modifiers.</param>
  /// <param name="typeToAssign">The type to assign. May be a single object applied to all, or a list of objects the same length as <paramref name="chars"/>.</param>
  /// <returns>An array of TokenRules.</returns>
  public static TokenRule[] MakeSingleCharRules (string chars, RT type, dynamic typeToAssign)
  {
    Collection<TokenRule> tokenRules = [];

    if (string.IsNullOrEmpty(chars))
      return [.. tokenRules];

    if (typeToAssign.IsCollection() && typeToAssign.Count == chars.Length)
    {
      for (int i = 0; i < chars.Length; i++)
      {
        string c = chars[i].ToString();
        dynamic t = typeToAssign[i];

        tokenRules.Add(new(type, t, c));
      }
    }

    else
    {
      foreach (char v in chars)
      {
        string c = v.ToString();

        tokenRules.Add(new(type, typeToAssign, c));
      }
    }

    return [.. tokenRules];
  }
  /// <summary>Creates an array of token rules representing keywords.</summary>
  /// <returns>A list of TokenMatch rules that properly exempt ranges.</returns>
  public static TokenRule[] MakeWordMatchRules (bool ignore_case, params Collection<(string, dynamic)> rules)
  {
    Collection<TokenRule> tokenRules = [];

    if (rules.IsEmpty() || rules is null)
      return [.. tokenRules];

    foreach ((string word, dynamic type) in rules)
    {
      TokenRule r = new(RT.TokenMatch | RT.ExemptAllWithin | (ignore_case ? RT.IgnoreCase : RT.None), type, $"\b{word}\b");
    }

    return [.. tokenRules];
  }
}
