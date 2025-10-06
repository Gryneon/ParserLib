#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Parser.Inference;
using Parser.Text.Ops;

using static Parser.DefinitionStaticFunctions;
using static Parser.Text.RX;

namespace Specification.SndInfo;

public static class Definition
{
  private static readonly RxS Ws = Rx(@"\s*");
  private static readonly RxS Ref = Gp(@"(?<qt>""?)\*?[\/.\w-]+\{qt}");
  private static readonly RxS D = Nm("int", @"\d+");
  private static readonly RxS Sndref = Nm("sound", Ref);
  private static readonly RxS Format = Rx(@"\s+").Orr.Add(@"\s*\=\s*");
  private static readonly RxS Act = Nm("action", @"\*\w+");
  private static RxS Cmd (string cmd) => @"\$" + Nm(cmd, Nm("type", cmd));
  private static RxS Wd (string word) => Nm(word, @"\w+");

  //https://regex101.com/r/syUVmo/3
  private static RxSCollection Regex { get; } = [
    // Comments and WS
    G_CLnComment,
    G_CBlkComment,
    G_WS,

    // Standard Def
    Nm("definition", Sndref + Format + Nm("lump", Ref)),

    // Commands
    Cmd("random") + WS + Sndref + WS + @"\{" + Ws + Gp(Nm("possibility", Ref) + WS).Many + @"\}",
    Cmd("alias") + WS + Sndref,
    Cmd("rolloff") + WS + Sndref + WS + D + WS + D,
    Cmd("archivepath") + WS + Nm("path", CString),
    Cmd("playersound") + @"(?:dup)?" + WS + Wd("player") + WS + Wd("gender") + WS + Act + WS + Sndref
  ];

  public static readonly TextSpec Spec = new()
  {
    Name = "sndinfo",
    CaseInsensitive = true,
    ExplicitCapture = true,
    IgnorePatternWhitespace = true,
    SingleLine = false,
    NonBacktracking = false,
    MultiLine = true,
    FileInferences = [
      new InferenceNodeOr([
        IfN(ExtIs, "sndinfo"),
        IfN(FName | Is, "sndinfo"),
      ])
    ],
    Operations = [
      new DictionaryOperation(Regex)
    ],
    RegexBasicTokens = [
      "definition",
      "random",
      "alias",
      "rolloff",
      "archivepath",
      "playersound"
    ],
    WhitespaceTokens = [
      "ws",
      "lncomment",
      "blkcomment"
    ]
  };
}
