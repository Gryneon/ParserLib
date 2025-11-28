#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using static Parser.DefinitionStaticFunctions;

namespace Specification.Decorate;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1
//https://regex101.com/r/mTwORe/2

[DefinitionExport]
public static class Definition
{
  private static RxSCollection Reader { get; } = [
    Nm("state_label", Nm("name", @"\w+.*?") + @"\:"),
    Nm("frame_line", Gp(@"\w{4}|"".{4}""") + @"\s+" + Rx(@"\w+")),
    Nm("block_close", @"\}"),
    Nm("states_head", Rx(@"\bstates\s*?" + Nm("block_open", @"\{"))),

  ];

  public const RegexOptions RxOpt = ROML | ROIPW | ROIC | ROEC;
  [Export("zdoom.decorate")]
  public static ISpec Spec => new Spec()
  {
    FileInferences = [],
    RxOpt = RxOpt,
    RegexBasicTokens = [],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    Name = "zdoom.decorate",
    Operations = [
      new SplitOperation(),
      new DictionaryOperation(Reader, RxOpt, false, "textparts"),
      new TokenizeOperation(),
      new TokenTemplateOperation([]),
      //TemplateToObjectOperation
      Operation.End
    ]
  };
}
