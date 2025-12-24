#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class TokenGroupRule<T> : TokenRule<T> where T : notnull
{
  public ChkSequence<T> Sequence { get; } = [];

  [SetsRequiredMembers]
  public TokenGroupRule (RT type, T typeToAssign, string ruleStringData)
  {
    Type = type;
    TypeToAssign = typeToAssign;
    RuleStringData = ruleStringData ?? throw new ArgumentNullException(nameof(ruleStringData));
    Sequence = new ChkSequence<T>(ruleStringData);
  }

  public TokenGroupRule () { }
}
