#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class TokenGroupRule : TokenRule
{
  public ChkSequence Sequence { get; } = [];
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, string typeToAssign, [SS("regex")] string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, object typeToAssign, [SS("regex")] string? ruleStringData)
  {
    Type = type;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, string typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenGroupRule (RT type, object typeToAssign)
  {
    Type = type;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  public TokenGroupRule ()
  {
    TypeToAssign = SE;
  }
}

public class TokenGroupRule<T> : TokenGroupRule where T : struct
{
  public new required T TypeToAssign
  {
    get => field;
    set
    {
      field = value;
      base.TypeToAssign = value.ToString() ?? SE;
    }
  }
  public new ChkSequence<T> Sequence { get; } = [];

  [SetsRequiredMembers]
  public TokenGroupRule (RT type, T typeToAssign, string ruleStringData)
  {
    Type = type;
    TypeToAssign = typeToAssign;
    RuleStringData = ruleStringData ?? throw new ArgumentNullException(nameof(ruleStringData));
  }
  public TokenGroupRule () { }
}
