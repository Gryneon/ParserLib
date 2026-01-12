#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using Parser.Ops;

using static Common.Names;
using static Parser.Tokens.TokenRuleType;

namespace Specification.ZScript;

[DefinitionExport]
public static class Definition
{
  /// <summary>
  /// https://regex101.com/r/En5C8c/7
  /// </summary>
  [Export("zdoom.zscript")]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    Name = "zdoom.zscript",
    Operations = [
      new TokenizeOperation(),
      Operation.End
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenRules = [
      new(TokenMatch|Competitive|IgnoredToken, "lncomment",@"\/\/[^\n]*"),
      new(TokenMatch|Competitive|IgnoredToken, "blkcomment",@"\/\*[\s\S]*?\*\/"),
      new(TokenMatch|Competitive, "string",@"""([^""\\]|\\.)*"""),
      new(TokenExact, "bk_open","{"),
      new(TokenExact, "bk_close","}"),

    ]
  };
}
