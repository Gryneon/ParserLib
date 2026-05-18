#pragma warning disable CA1822 // Mark members as static

using Common.RegExp;

using BTT = Parser.BinaryTokenType;

namespace Parser;

public static class InternalSpec
{
  private const string CmdList = "(read|jump|save|loop|if|switch|end|fail)";
  private const string CmdTypeList = "(int|short|byte|text|binary|end|long|is|each|count|break|position)";

  public static Spec BinaryOpSpec => new()
  {
    Name = "internal.binary.op",
    IsTextFile = true,
    SC = SCOIC,
    RxOpt = ROIPW | ROIC | ROEC | ROML,
    DefaultRuleSet = RT.IgnoreCase,
    TokenType = typeof(BTT),
    TokenRules = [
      new(RT.Competitive | RT.IgnoredToken, BTT.Unknown, @"\/\/.*?$"),
      new(RT.TokenMatch, BTT.SaveToDataKey, @"(?<= :\s* ) (\w+)"),
      new(RT.TokenMatch, BTT.LoadFromDataKey, @"(?<=\[) (\w+) (?=\])"),
      new(RT.TokenMatch, BTT.Size, @"(?<=\() (\d+) (?=\))"),
      new(RT.TokenMatch, BTT.Cmd, RxS.Rx(@"(?<= ^\s* )") + CmdList),
      new(RT.TokenMatch, BTT.CmdType, CmdTypeList)
    ],
    GroupTokenRules = [
      //new(RT.)
    ],
  };
}
