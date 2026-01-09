#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles

using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Extensions;

using Parser;
using Parser.Ops;
using Parser.Ops.Text;

using static Common.Names;
using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

namespace Specification.MapInfo;

[DefinitionExport]
public static class Definition
{
  private static readonly RxS
    include = Nm("include", $@"\binclude{ws}""(?'path'[^""]*)"""),
    property = Nm("property", $@"^{wso}(?'key'\w+){eq}(?'value'.*?){wso}$"),
    damagetype = Nm("damagetype", $@"\bdamagetype{ws}(?'type'\w+){brk_st}{properties}{brk_en}"),
    doomednums = Nm("doomednums", $@"\bdoomednums{brk_st}{properties}{brk_en}"),
    ws = Or(Nm("lncomment", $@"//.*?$"), Nm("blkcomment", @"/\*.*?\*/"), Nm("ws", @"\s+")),
    wso = ws.Opt,
    brk_st = $"{wso}{{{wso}",
    brk_en = $"{wso}}}",
    eq = $"{wso}={wso}",
    properties = $"(?'content'{property}*?)";

  private static readonly Dictionary<string, RxS> Translator = [
    K("{", brk_st),
    K("}", Rx($"{wso}}}")),
    K("=", Rx($"{wso}={wso}")),
    K("damagetype", Rx($@"\bdamagetype\b{ws}")),
    K("doomednums", Rx($@"\bdoomednums\b{ws}")),
    K("class", Nm("classname", @"\b\w+\b")),
    K("i", Nm("int", RX.Integers)),
    K("d", Nm("dec", RX.Decimals)),
    K("s", Nm("str", RX.CString)),
    K("c", Nm("char", RX.Chars)),

  ];
  private static readonly DictionaryOperation OldDicOperation = new([
        Nm("langref", @"""(?'reference'\$\w+)"""),
        Nm("str", @"""(?'strqt'.*?)"""),
        Nm("dec", @"-?[0-9]*\.[0-9]+"),
        Nm("int", @"-?[0-9]+(\.0*)?"),
        Nm("ws", @"\s+"),
        Nm("op", @"[():{},=;[\]]"),
        Nm("bool", @"true|false"),
        Nm("blockkeyword", @"\b(map|episode|gameinfo|skill|cluster|defaultmap|adddefaultmap|doomednums)\b"),
        Nm("keyword", @"\b(lookup|include)\b"),
        Nm("name", @"[\w\.\-]+"),
        Nm("lncomment", @"//.*?$"),
        Nm("blkcomment", @"/\*.*?\*/"),
      ]);
  private const RT RT_Competes = TokenMatch | Competitive | IgnoreCase | ExemptAllWithin;

  /// <summary>
  /// Defines a mapinfo Spec. <see href="https://regex101.com/r/iWWPub/1">Regex</see>
  /// </summary>
  [Export("zdoom.mapinfo")]
  public static Spec Spec { get; } = new()
  {
    Name = "zdoom.mapinfo",
    FileInferences = [
      IfN(ExtIs, "mapinfo"),
      IfN(FName |Is, "mapinfo")],
    Operations = [

      new TokenizeOperation<string>(),
  new DebugToStringOperation ("tokens")
    ],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    RegexBasicTokens = ["langref", "int", "dec", "op", "str", "bool", "blockkeyword", "name", "keyword"],
    RxOpt = ROIC | ROEC | ROML,
    IsTextFile = true,
    TokenType = typeof(MTT),
    TokenTypeLookup = {
      ["LangRef"] = MTT.LangRef,
      ["Str"] = MTT.Str,
      ["AInt"] = MTT.AInt,
      ["Dec"] = MTT.Dec,
      ["AChar"] = MTT.AChar,
      ["Op"] = MTT.Op,
      ["Bool"] = MTT.Bool,
      ["BlockKeyword"] = MTT.BlockKeyword,
      ["Keyword"] = MTT.Keyword,
      ["Name"] = MTT.Name,
    },
    TokenRules = [
      new (RT_Competes, MTT.LangRef, @"""\$\w+"""),
      new (RT_Competes, MTT.Str, @""".+?"""),
      new (RT_Competes | IgnoredToken, MTT.LnComment, @"\/\/.*?$"),
      new (RT_Competes | IgnoredToken, MTT.BlkComment, @"\/\*[\s\S]*?\*\/"),],
    SC = SCOIC,
    TokenCompatLookup = {
      [MTT.Property] = [MTT.AInt, MTT.Dec, MTT.Str, MTT.AChar, MTT.Bool],
    },
  };
}
