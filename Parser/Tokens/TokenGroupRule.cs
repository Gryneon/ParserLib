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
    ['v'] = RT.AssignValue,
    ['i'] = RT.IgnoreCase,
    ['m'] = RT.Mult,
    ['o'] = RT.Opt,
    ['x'] = RT.IgnoredToken,
    ['n'] = RT.AssignName,
    ['y'] = RT.AssignType,
    ['p'] = RT.AddProperty,
    ['f'] = RT.AddFlag,
    ['s'] = RT.SubFlag,
    ['r'] = RT.AssignRight,
    ['l'] = RT.AssignLeft,
    ['t'] = RT.None,
    ['c'] = RT.AssignCenter,
    ['b'] = RT.None,
  };
}
