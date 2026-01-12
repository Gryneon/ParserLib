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
}
