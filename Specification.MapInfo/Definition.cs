#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
        Rx(@"""(?<key>.*?)""\s*(?=:)"),
        Nm("strqt", @"""(?<str>.*?)"""),
        Nm("dec", @"-?[0-9]*\.[0-9]+"),
        Nm("int", @"-?[0-9]+(?:\.0*)?"),
        Nm("ws", @"\s+"),
        Nm("op", @"[:{},[\]]"),
        Nm("bool", @"true|false"),
        Nm("blockkeyword", @"\b(map|episode|gameinfo|skill)\b"),
        Nm("lookupkeyword", @"\blookup\b"),
        Nm("lumpname", @"[\w\.\-]+")
      ]),
      new TokenizeOperation(),
      new TokenTemplateOperation([])
    ],
    TokenLookup = [
      "key",
      "int",
      "dec",
      "ws",
      "op",
      "strqt",
      "str",
      "bool",
      "blockkeyword",
      "lumpname",
      "lncomment",
    ],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    RegexBasicTokens = ["int", "dec", "key", "op", "strqt", "str", "bool", "blockkeyword", "lumpname"]
  };
}
