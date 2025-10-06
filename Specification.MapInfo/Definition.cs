#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles

using System.Collections.Generic;

using Common.Extensions;
using Common.Regex;

using static Parser.DefinitionStaticFunctions;

namespace Specification.MapInfo;

public static class Definition
{
  // <summary>
  // https://regex101.com/r/hgS1Zq/5
  // </summary>
  //private static readonly TokenFormat GameInfoSingle = new()
  //{
  //  Type = "property",
  //  Template = [
  //      N("name", ["CheatKey", "EasyKey"], "key"),
  //      N("equals"),
  //      N("string", null, "value"),
  //    ]
  //};
  //private static readonly TokenFormat GameInfoParam4 = new()
  //{
  //  Type = "property",
  //  Template = [
  //    N("name", ["PrecacheSounds"], "key"),
  //    N("equals"),
  //    N("string", null, "value1"),
  //    N("comma"),
  //    N("string", null, "value2"),
  //    N("comma"),
  //    N("string", null, "value3"),
  //    N("comma"),
  //    N("string", null, "value4"),
  //  ]
  //};
  //private static TokenTemplate ;

  //private static TokenTemplateNode N (string t, string[]? s = null, string? p = null) => new(t, s, p);
  //private static TokenTemplateNode N (TokenType t, string s) => new(t, s is null ? null : [s], null);
  //private static TokenTemplateNode N (TokenType[] t, string[]? s = null, string? p = null) => new(t, s, p);
  //private static Collection<TokenFormat> Templates { get; } = [
  //  new() {
  //    Type = "block",
  //    Template = [
  //  //    N("blockstart", ["map"], "definitiontype"),
  //  //    N("name", null, "definitionname"),
  //  //    N("string", null, "nicename"),
  //  //    N("lbracket"),
  //  //    N("property", null, "properties"), //one or many
  //  //    N("rbracket"),
  //    ]
  //  },
  //  new() {
  //    Type = "parameter",
  //    Template = [
  //      new(["string", "int", "decimal", "bool", "name"], null, "parameter"),
  //   //   N("comma") // opt
  //    ]
  //  },
  //  //Gameinfo Properties
  //  //GameInfoSingle,
  //  //GameInfoParam4,
  //  new() {
  //    Type = "property",
  //    Template = [
  //      new("int", null, "key"),
  //      new("equals"),
  //      new(["string", "int", "decimal", "bool", "name"], null, "value"),
  //    ]
  //  },

  //];

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

  /// <summary>
  /// https://regex101.com/r/iWWPub/1
  /// </summary>
  public static readonly TextSpec Spec = new()
  {
    Name = "mapinfo",
    CaseInsensitive = true,
    ExplicitCapture = true,
    FileInferences = [
      IfN(ExtIs, "mapinfo"),
      IfN(FName |Is, "mapinfo")],
    Operations = [
      new DictionaryOperation([
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
      ]),
      new TokenizeOperation(),
      new TokenTemplateOperation(new Dictionary<string, string>() {
        ("numprop1",   "$int ^key^ '=' ($name  (',' $int)? (',' '+')?) ^value^"),
        ("numprop2",   "$int '=' $str (',' $int)?")
      }),
      new DebugToStringOperation("tokens_templated")
    ],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    RegexBasicTokens = ["langref", "int", "dec", "op", "str", "bool", "blockkeyword", "name", "keyword"]
  };
}
