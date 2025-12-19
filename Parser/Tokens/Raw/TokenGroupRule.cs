#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

/*
public class RawTokenGroupRule_Orig<T> where T : struct, Enum
{
  public required RT Type { get; init; }
  public required T TypeToAssign { get; init; }
  public required string RuleStringData { get; init; }
  public IList<(T Type, RT Flag)> Sequence { get; } = [];

  [SetsRequiredMembers]
  public RawTokenGroupRule_Orig (RT type, T typeToAssign, string ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;

    Collection<string> items = [.. RuleStringData.Split(" ")];
    foreach (string item in items)
    {
      char flag = item[0];
      string token = item[2..];

      Enum.Parse<T>(token);
    }
  }
  public RawTokenGroupRule_Orig () { }
}*/

public class TokenGroupRule<T> where T : notnull
{
  public required RT Type { get; init; }
  public required T TypeToAssign { get; init; }
  public required string RuleStringData { get; init; }
  public IList<(T TokenType, RT Flag)> Sequence { get; } = new List<(T, RT)>();

  private static T ConvertToken (string s)
  {
    if (typeof(T).IsEnum)
    {
      // Use the non-generic Enum.TryParse with a Type, then unbox to T
      return Enum.TryParse(typeof(T), s, ignoreCase: true, out object? boxed) && boxed is T value
        ? value
        : throw new ArgumentException($"'{s}' is not a valid {typeof(T).Name}.");
    }

    return typeof(T) == typeof(string) ? (T) (object) s : throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
  }
  private static bool TryMapFlag (char c, out RT flag)
  {
    switch (char.ToLowerInvariant(c))
    {
      case 'i': flag = RT.IgnoredToken; return true;
      case 'n': flag = RT.AssignName; return true;
      case 'v': flag = RT.AssignValue; return true;
      case 't': flag = RT.AssignType; return true;
      case 'p': flag = RT.AddProperty; return true;
      default: flag = default; return false;
    }
  }
  private static Collection<(T, RT)> CalculateSequence (string data)
  {
    Collection<(T, RT)> seq = [];
    foreach (string item in data.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    {
      // Expect "<letter>:<Token>", e.g., "i:Whitespace" or "n:Identifier"
      int sep = item.IndexOf(':', SCO);
      if (sep <= 0 || sep == item.Length - 1) continue;

      if (!TryMapFlag(item[0], out RT flag)) continue;

      string tokenText = item[(sep + 1)..];
      T token = ConvertToken(tokenText);
      seq.Add((token, flag));
    }
    return seq;
  }

  [SetsRequiredMembers]
  public TokenGroupRule (RT type, T typeToAssign, string ruleStringData)
  {
    Type = type;
    TypeToAssign = typeToAssign;
    RuleStringData = ruleStringData ?? throw new ArgumentNullException(nameof(ruleStringData));
    Sequence = CalculateSequence(ruleStringData);
  }
  public TokenGroupRule () { }
}
