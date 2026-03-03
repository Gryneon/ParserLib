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
  [SetsRequiredMembers]
  public TokenGroupRule (object typeToAssign, [SS("regex")] string ruleStringData)
  {
    Type = RT.None;
    RuleStringData = ruleStringData;
    TypeToAssign = typeToAssign?.ToString() ?? SE;
  }
  public TokenGroupRule () => TypeToAssign = SE;

  public static Dictionary<char, RT> CharacterReference => new()
  {
    ['b'] = RT.None,
    ['c'] = RT.AssignCenter,
    ['f'] = RT.AddFlag,
    ['i'] = RT.IgnoreCase,
    ['l'] = RT.AssignLeft,
    ['m'] = RT.Mult,
    ['n'] = RT.AssignName,
    ['o'] = RT.Opt,
    ['p'] = RT.AddProperty,
    ['r'] = RT.AssignRight,
    ['s'] = RT.SubFlag,
    ['t'] = RT.None,
    ['v'] = RT.AssignValue,
    ['x'] = RT.IgnoredToken,
    ['y'] = RT.AssignType,
  };
}
