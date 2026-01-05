using Parser.Ops.Binary;
using Parser.Tokens.Raw;

using RT = Parser.Tokens.Raw.TokenRuleType;

namespace Specification.JSON;

public enum JSONTokenType
{
  None,

  Str,
  Num,
  Bool,
  Null,
  Cm,
  Bo,
  Bc,
  Co,
  Ws,
  Comment,
  Property,
  AObject,
  Array,
  Value,
  Bko,
  Bkc
}

/// <summary>
/// Defines a JSON Specification.
/// </summary>
[DefinitionExport]
public static class Definition
{
  // Flags
  internal const RT RT_Comment = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal const RT RT_String = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal const RT RT_Match = RT.TokenMatch | RT.ExemptAllWithin;
  internal const RT RT_Exact = RT.TokenExact | RT.ExemptAllWithin;

  private static KeyValuePair<string, RxS> TT (string name, RxS content, RxS? prefix = null, RxS? suffix = null)
  {
    RxS pre = Rx(prefix is null ? SE : prefix);
    RxS suf = Rx(suffix is null ? SE : suffix);
    RxS formed = Nm(name, pre + Nm("_content", content) + suf);
    return new KeyValuePair<string, RxS>(name, formed);
  }

  /// <summary>
  /// https://regex101.com/r/hgS1Zq/5
  /// </summary>
  private static readonly Dictionary<string, RxS> ReaderBase = [
    TT("string", Gp(@"[^\\]|\\.").Any.Lazy, "\"", "\""),
    TT("dec", Rx(@"-?[0-9]*\.[0-9]+")),
    TT("int", Rx(@"-?[0-9]+(?:\.0+|\.)?")),
    TT("ws", Rx(@"\s+")),
    TT("op", Rx(@"[,\[\]{}:]")),
    TT("bool", Rx(@"\b(true|false)\b")),
    TT("null", Rx(@"\bnull\b"))
  ];
  internal static readonly RxSCollection Reader = [.. ReaderBase.Values];
  /*private static readonly Collection<string> BaseTokenList = [.. ReaderBase.Keys];
   *  json_object - '{' ( #json_property^import^ ( ',' #json_property^import^ )* )? '}'
   *  json_array - '[' ( #json_value ( ',' #json_value )* )? ']'
   *  json_value - ( $int | $dec | $null | $bool | $string | #json_object | #json_array )
   *  json_property - $string^key^ ':' #json_value^value^
   */
  //private static readonly Dictionary<string, string> TokenFormats = [
  // K("json_object", "'{' ( #json_property ^import^ ( ',' #json_property ^import^ )* )? '}'"),
  // K("json_array", "'[' ( #json_value ( ',' #json_value )* )? ']'"),
  // K("json_value", "$int | $dec | $null | $bool | $string | #json_object | #json_array"),
  // K("json_property", "$string ^key^ ':' #json_value ^value^")
  //];
  /// <summary>
  /// The JSON Spec.
  /// </summary>
  [Export("json")]
  public static Spec Spec => new()
  {
    Name = "json",
    FileInferences = [IfN(ExtIs, "json")],
    Operations = [
        Operation.CreateCursor("json_cursor", 0),
        ByteReadOperation.ReadRemainingBin("json_utf8", "json_cursor"),
        //TODO: Feed to JSON Reader
        Operation.CopyKey("json_object", "result"),
        //TODO: Enhance Spec to analyse content.
        //TODO: Validate?
        new TokenizeOperation<JTT>( Spec.LoadFromSpec,"text", "tokens"),
        new DebugToStringOperation( "tokens"),
      ],
    //WhitespaceTokens = ["ws"],
    //RegexBasicTokens = ["string", "dec", "int", "op", "bool", "null"],
    RxOpt = ROML | ROIPW | ROEC | ROSL,
    IsTextFile = true,
    TokenType = typeof(JTT),
    TokenRules = [
      new(RT_String, JTT.Str, $"\"{Gp(@"[^\\]|\\.").Any.Lazy}\""),
      new(RT_Match, JTT.Bool, @"\b(true|false)\b"),
      new(RT_Match, JTT.Null, @"\b(null)\b"),
      new(RT_Match, JTT.Num, @"-?\d+(\.\d*)?"),
      new(RT_Exact, JTT.Cm, ","),
      new(RT_Exact, JTT.Bko, "{"),
      new(RT_Exact, JTT.Bkc, "}"),
      new(RT_Exact, JTT.Bo, "["),
      new(RT_Exact, JTT.Bc, "]"),
      new(RT_Exact, JTT.Co, ":"),
      new(RT_Match | RT.IgnoredToken, JTT.Ws, @"\s+"),
      new(RT.StoreOther | RT.ExemptAllWithin, JTT.None)],
    TokenTypeLookup =
    {
      ["Str"] = JTT.Str,
      ["Num"] = JTT.Num,
      ["Null"] = JTT.Null,
      ["Bool"] = JTT.Bool,
      ["Cm"] = JTT.Cm,
      ["Bc"] = JTT.Bc,
    }
  };
}
