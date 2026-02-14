#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using System;

using Parser.Ops;
using Parser.Tokens;

using static Common.Names;
using static Parser.Tokens.TokenRuleType;
using static Specification.ZScript.ZScriptTokenType;

using RT = Parser.Tokens.TokenRuleType;
using ZT = Specification.ZScript.ZScriptTokenType;

namespace Specification.ZScript;

[DefinitionExport]
public static class Definition
{
  /// <summary>
  /// https://regex101.com/r/En5C8c/7
  /// </summary>
  [DefinitionExport]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    Name = "zdoom.zscript",
    Operations = [
      new TokenizeOperation(),
      Op.End
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(ZT),
    DefaultRuleSet = IgnoreCase | ExemptAllWithin,
    TokenRules = [
      new(TokenMatch|Competitive|IgnoredToken, ZT.None, @"\/\/[^\n]*"),
      new(TokenMatch|Competitive|IgnoredToken, ZT.None, @"\/\*[\s\S]*?\*\/"),
      new(TokenMatch|Competitive, ZT.String, @"""([^""\\]|\\.)*"""),
      .. TokenRule.MakeSingleCharRules("{}();=,:+-", TokenExact, new ZT[] { Bo, Bc, Po, Pc, Sc, Eq, Cm, Co, Pl, Mn })

    ]
  };
}
