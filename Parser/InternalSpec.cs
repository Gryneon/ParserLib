#pragma warning disable CA1822 // Mark members as static

using BTT = Parser.BinaryTokenType;

namespace Parser;

public static class InternalSpec
{
  private const RT Match = RT.TokenMatch | RT.ExemptAllWithin | RT.IgnoreCase;
  private const string CmdList = "(read|jump|save|loop|if|switch|end|fail)";
  private const string CmdTypeList = "(int|short|byte|text|binary|end|long|is|each|count|break|position|)";

  public static Spec BinaryOpSpec => new()
  {
    Name = "internal.binary.op",
    IsTextFile = true,
    SC = SCOIC,
    RxOpt = ROIPW | ROIC | ROEC | ROML,
    TokenType = typeof(BTT),
    TokenRules = [
      new(Match | RT.Competitive | RT.IgnoredToken, BTT.Unknown, @"\/\/.*?$"),
      new(Match, BTT.SaveToDataKey, @"(?<= :\s* ) (\w+)"),
      new(Match, BTT.LoadFromDataKey, @"(?<=\[) (\w+) (?=\])"),
      new(Match, BTT.Size, @"(?<=\() (\d+) (?=\))"),
      new(Match, BTT.Cmd, RxS.Rx(@"(?<= ^\s* )") + CmdList),
      new(Match, BTT.CmdType, CmdTypeList)
    ],
    GroupTokenRules = [
      //new(RT.)
    ],
  };
}
