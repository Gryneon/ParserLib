#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1720 // Identifier contains type name

using Parser;
using Parser.Inference;
using Parser.Ops;
using Parser.Ops.Text;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;
using static Parser.RX;

namespace Specification.SndInfo;

[DefinitionExport]
public static class Definition
{
  private static readonly RxS Ws = Rx(@"\s*");
  private static readonly RxS Ref = Gp(@"(?<qt>""?)\*?[\/.\w-]+\{qt}");
  private static readonly RxS D = Nm("int", @"\d+");
  private static readonly RxS Sndref = Nm("sound", Ref);
  private static readonly RxS Format = Rx(@"\s+|") + Gp(@"\s*\=\s*");
  private static readonly RxS Act = Nm("action", @"\*\w+");
  private static RxS Cmd (string cmd) => @"\$" + Nm(cmd, Nm("type", cmd));
  private static RxS Wd (string word) => Nm(word, @"\w+");

  //https://regex101.com/r/syUVmo/3
  internal static RxSCollection Regex { get; } = [
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

  [DefinitionExport]
  public static Spec Spec => new()
  {
    Name = "zdoom.sndinfo",
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    FileInferences = [
      new InferenceNodeOr([
        IfN(ExtIs, "sndinfo"),
        IfN(FName | Is, "sndinfo"),
      ])
    ],
    IsTextFile = true,
    TokenType = typeof(SndInfoTokenType),
    SC = SCOIC,
    TokenRules = [],
    GroupTokenRules = [],
    Operations = [
      new TokenizeOperation(),
      Op.End
    ],
  };
}
