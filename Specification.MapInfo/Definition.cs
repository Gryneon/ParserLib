#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.ObjectModel;

using Common.Regex;

using Parser.Text.Ops;
using Parser.Text.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Text.Tokens.TokenType;

namespace Specification.MapInfo;
public static class Definition
{
  /// <summary>
  /// https://regex101.com/r/hgS1Zq/5
  /// </summary>
  private static RxSList Reader { get; } = [
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
  ];
  private static Dictionary<string, TokenType> TokenInfo { get; } = new() {
    ("key", T_Key),
    ("int", T_Int),
    ("dec", T_Dec),
    ("ws", T_WS|T_Ignore),
    ("op", T_Operator),
    ("strqt", T_String),
    ("str", T_Value),
    ("bool", T_Bool),
    ("blockkeyword", T_BlockStart),
    ("lumpname", T_Name),
  };

  private static Collection<TokenTemplate> Templates { get; } = [
    new() {
      Type = T_Block,
      Template = [
        new(T_BlockStart, ["map"], "definitiontype"),
        new(T_Name, null, "definitionname"),
        new(T_String, null, "nicename"),
        new(T_LBracket),
        new(T_Property | T_OneOrMany, null, "properties"),
        new(T_RBracket),
      ]
    },
    new() {
      Type = T_Param,
      Template = [
        new([T_String, T_Int, T_Name, T_Dec, T_Bool], null, "parameter"),
        new(T_Comma | T_Optional)
      ]
    },
    new() {
      Type = T_Property,
      Template = [
        new(T_Name, null, "key"),
        new(T_Equals),
        new([T_String, T_Int, T_Dec, T_Bool, T_Name], null, "value"),
      ]
    },

  ];

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
        new DictionaryOperation(Reader, "initial"),
        new TokenizeOperation(),
        new TokenTemplateOperation("tokens", "tokens_templated", Templates)
      ],
    TokenLookup = TokenInfo
  };
}
