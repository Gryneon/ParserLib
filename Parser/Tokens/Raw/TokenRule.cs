#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class TokenRule<T> where T : notnull
{
  public required RT Type { get; init; }
  public string? RuleStringData { get; init; }
  public required T TypeToAssign { get; init; }

  [SetsRequiredMembers]
  public TokenRule (RT type, T typeToAssign, string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, T typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign;
  }
  public TokenRule () { }
}
