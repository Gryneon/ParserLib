#pragma warning disable CA1710 // Identifiers should have correct suffix
namespace Parser.Tokens;

public class TokenRule
{
  public required RT Type { get; init; }
  public string? RuleStringData { get; init; }
  public required dynamic TypeToAssign { get; init; }

  [SetsRequiredMembers]
  public TokenRule (RT type, dynamic typeToAssign, [SS("regex")] string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, dynamic typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign;
  }
  public TokenRule () { }
}
public class TokenRule<T> where T : notnull
{
  public required RT Type { get; init; }
  public string? RuleStringData { get; init; }
  public required T TypeToAssign { get; init; }

  public static implicit operator TokenRule<T> (TokenRule rule)
  {
    rule.ThrowIfNull();
    return new()
    {
      RuleStringData = rule.RuleStringData,
      TypeToAssign = rule.TypeToAssign,
      Type = rule.Type
    };
  }
  public static implicit operator TokenRule (TokenRule<T> rule)
  {
    rule.ThrowIfNull();
    return new()
    {
      RuleStringData = rule.RuleStringData,
      TypeToAssign = rule.TypeToAssign,
      Type = rule.Type
    };
  }

  [SetsRequiredMembers]
  public TokenRule (RT type, T typeToAssign, [SS("regex")] string? ruleStringData)
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

  public TokenRule<dynamic> Dynamic => new()
  {
    Type = Type,
    RuleStringData = RuleStringData,
    TypeToAssign = TypeToAssign
  };
}
