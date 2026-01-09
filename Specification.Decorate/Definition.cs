#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
//#pragma warning disable IDE1006 // Naming Rule Violation

using System;

using Common;

using static Parser.DefinitionStaticFunctions;

namespace Specification.Decorate;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1
//https://regex101.com/r/mTwORe/2

[DefinitionExport]
public static class Definition
{
  public const RegexOptions RxOpt = ROML | ROIPW | ROIC | ROEC;
  [Export("zdoom.decorate")]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = RxOpt,
    RegexBasicTokens = [],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    Name = "zdoom.decorate",
    Operations = [
      new TokenizeOperation<string>(),
      Operation.End
    ]
  };
}