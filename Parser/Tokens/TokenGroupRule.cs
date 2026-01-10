#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class TokenGroupRule : TokenRule
{
  public ChkSequence Sequence { get; } = [];

  [SetsRequiredMembers]
  public TokenGroupRule (RT type, string typeToAssign, string ruleStringData)
  {
    Type = type;
    TypeToAssign = typeToAssign;
    RuleStringData = ruleStringData ?? throw new ArgumentNullException(nameof(ruleStringData));
  }
  public TokenGroupRule () { }
}

public class TokenGroupRule<T> : TokenRule<T> where T : struct
{
  public ChkSequence<T> Sequence { get; } = [];

  [SetsRequiredMembers]
  public TokenGroupRule (RT type, T typeToAssign, string ruleStringData)
  {
    Type = type;
    TypeToAssign = typeToAssign;
    RuleStringData = ruleStringData ?? throw new ArgumentNullException(nameof(ruleStringData));
  }
  public TokenGroupRule () { }
}
