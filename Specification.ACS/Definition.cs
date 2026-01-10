#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1720 // Identifier contains type name

using System.Text.RegularExpressions;

using static Parser.DefinitionStaticFunctions;
using static Parser.RX;

using ATT = Specification.ACS.ACSTokenType;
using RT = Parser.Tokens.TokenRuleType;

namespace Specification.ACS;

public enum ACSTokenType
{
  None,
  Str,
  Char,
  Int,
  Fixed,
  Bool,
  Name,
  FunctionCall,
  Script,
  Function,
  Global,
  World,
  ScriptType,
  Value,
  Bo, Bc,
  Po, Pc,
  Expression,
  Sc, Cm,
  Op, Co,
  Ao, Ac,
  Hash,
  Eq,
  DataType,
}

/// <summary>
/// ACS Specification Definition <br/>
/// <see href="https://regex101.com/r/mTwORe/1">Regex</see>
/// https://regex101.com/r/FCoqFI/1
/// </summary>
[DefinitionExport(true)]
public static class Definition
{
  internal static RT Compet = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal static RT Ignore = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal static RT TMatch = RT.TokenMatch | RT.ExemptAllWithin | RT.IgnoreCase;
  internal static RT TExact = RT.TokenExact | RT.ExemptAllWithin | RT.IgnoreCase;

  /// <summary>
  /// Defined Specification
  /// </summary>
  [Export("zdoom.acs")]
  public static Spec ACS => new()
  {
    FileInferences = [IfN(ExtIs, "acs")],
    Name = "zdoom.acs",
    RxOpt = ROML | ROIC | ROIPW | ROEC | ROSL,
    Operations = [
      new TokenizeOperation<ATT>(),
      new TokenAssembleOperation<ATT>(),
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(ATT),
    TokenTypeLookup = [],
    TokenCompatLookup = [],
    TokenRules = [
      new(Compet, ATT.Str, @"""([^\\""]|\\.)*"""),
      new(Compet, ATT.Char, @"'([^\\']|\\.)*'"),
      new(Ignore, ATT.None, @"\/\/.*?$"),
      new(Ignore, ATT.None, @"\/\*.*?\*\/"),

      new(TExact, ATT.Hash, "#"),
      new(TExact, ATT.Bool, @"\b(true|false|on|off|yes|no)\b"),
      new(TExact, ATT.Int, @"-?(\d+|0x[a-f0-9]+)(?!\.)"),
      new(TExact, ATT.Fixed, @"-?(\d+\.\d*|\d*\.\d+)"),
      new(TExact, ATT.Script, @"\bscript\b"),

      new(TExact, ATT.Ao, "["),
      new(TExact, ATT.Ac, "]"),
      new(TExact, ATT.Bo, "{"),
      new(TExact, ATT.Bc, "}"),
      new(TExact, ATT.Po, "("),
      new(TExact, ATT.Pc, ")"),
      new(TExact, ATT.Eq, "="),
      new(TExact, ATT.Cm, ","),
      new(TExact, ATT.Co, ":"),
      new(TExact, ATT.Sc, ";"),
    ],
    GroupTokenRules = [],
  };
  [Export("zdoom.modeldef")]
  public static Spec ModelDef => new()
  {
    FileInferences = [IfNOr(
      IfN(ExtIs, "modeldef"),
      IfN(FName|Is, "modeldef"))],
    Name = "zdoom.modeldef",
    RxOpt = ROML | ROIC | ROIPW | ROEC,
    Operations = [
      //new DictionaryOperation([], RxOptions),
      new DebugToStringOperation("matches"),
      new DebugWaitForInputOperation(),
      new TokenizeOperation<string>(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),
    ]
  };
}
