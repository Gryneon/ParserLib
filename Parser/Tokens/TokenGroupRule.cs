#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class TokenGroupRule : TokenRule
{
  public ChkSequence Sequence { get; } = [];
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, string typeToAssign, [SS("regex")] string ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, object typeToAssign, [SS("regex")] string ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  public TokenGroupRule ()
  {
    TypeToAssign = SE;
  }
}
