namespace Specification.JSON;

/// <summary>
/// Defines a JSON Specification.
/// </summary>
[DefinitionExport]
public static class Definition
{
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
  private static readonly RxSCollection Reader = [.. ReaderBase.Values];
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
  public static ISpec Spec => new Spec()
  {
    Name = "json",
    FileInferences = [IfN(ExtIs, "json")],
    Operations = [
        new DictionaryOperation(Reader, ROML | ROIPW | ROEC | ROIC, false),
        new DebugToStringOperation("matches"),
        new TokenizeOperation(),
        new DebugToStringOperation("tokens"),
        //new TokenTemplateOperation(TokenFormats) {ContinueOnFail=true},
        new JSONOperation("tokens", "json"),
        new DebugToStringOperation("json"),
        Operation.SetResultKey("json")
      ],
    WhitespaceTokens = ["ws"],
    RegexBasicTokens = ["string", "dec", "int", "op", "bool", "null"],
    RxOpt = ROML | ROIPW | ROEC | ROIC
  };
}
