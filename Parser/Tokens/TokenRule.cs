#pragma warning disable CA1710 // Identifiers should have correct suffix
using System.Data;

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
public class TokenRule<T> : TokenRule where T : struct
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

  [SetsRequiredMembers]
  public TokenRule (RT type, T typeToAssign, [SS("regex")] string? ruleStringData) : base(type, typeToAssign.ToString() ?? SE, ruleStringData)
  {
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, string typeToAssign, [SS("regex")] string? ruleStringData) : base(type, typeToAssign?.ToString() ?? "None", ruleStringData)
  {
    typeToAssign ??= "None";
    TypeToAssign = Enum.Parse<T>(typeToAssign);
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, T typeToAssign) : base(type, typeToAssign.ToString() ?? SE)
  {
    TypeToAssign = typeToAssign;
  }
  [SetsRequiredMembers]
  public TokenRule (RT type, string typeToAssign) : base(type, typeToAssign?.ToString() ?? SE)
  {
    typeToAssign ??= "None";
    TypeToAssign = Enum.Parse<T>(typeToAssign);
  }
  public TokenRule () { }
}
