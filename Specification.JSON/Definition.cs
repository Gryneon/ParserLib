using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Common.Regex;

using Parser.Text.Ops;

using static Common.Names;

namespace Specification.JSON;

/// <summary>
/// Defines a JSON Specification.
/// </summary>
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
    TT("colon", Rx(@"\:")),
    TT("comma", Rx(@"\,")),
    TT("arr_st", Rx(@"\[")),
    TT("arr_en", Rx(@"\]")),
    TT("obj_st", Rx(@"\{")),
    TT("obj_en", Rx(@"\}")),
    TT("bool", Rx("true|false")),
    TT("null", Rx("null"))
  ];
  //private static readonly Dictionary<string, string> FormatLiterals = [
  //  K("[","$arr_st"),
  //  K("]","$arr_en"),
  //  K("{","$obj_st"),
  //  K("}","$obj_en"),
  //  K(",","$comma"),
  //  K(":","$colon")
  //];
  private static readonly RxSList Reader = [.. ReaderBase.Values];
  private static readonly Collection<string> BaseTokenList = [.. ReaderBase.Keys];
  /* json_object - '{' ( #json_property^import^ ( ',' #json_property^import^ )* )? '}'
   * json_array - '[' ( #json_value ( ',' #json_value )* )? ']'
   * json_value - ( $int | $dec | $null | $bool | $string | #json_object | #json_array )
   * json_property - $string^key^ ':' #json_value^value^
   */
  private static readonly Dictionary<string, string> TokenFormats = [
    K("json_object", "'{' ( #json_property^import^ ( ',' #json_property^import^ )* )? '}'"),
    K("json_array", "'[' ( #json_value ( ',' #json_value )* )? ']'"),
    K("json_value", "( $int | $dec | $null | $bool | $string | #json_object | #json_array )"),
    K("json_property", "$string^key^ $colon #json_value^value^")
  ];
  /// <summary>
  /// The JSON Spec.
  /// </summary>
  public static readonly TextSpec Spec = new()
  {
    Name = "json",
    CaseInsensitive = true,
    FileInferences = [IfN(ExtIs, "json")],
    Operations = [
        new DictionaryOperation(Reader, false),
        new DebugToStringOperation("matches"),
        new TokenizeOperation(),
        new DebugToStringOperation("tokens"),
        new DebugWaitForInputOperation(),
        new TokenTemplateOperation(TokenFormats),
        new DebugToStringOperation("tokens_templated"),
        new DebugWaitForInputOperation(),
        new CopyOperation("tokens_templated", "result")
      ],
    TokenLookup = BaseTokenList,
    ExplicitCapture = true,
    WhitespaceTokens = ["ws"],
    IgnorePatternWhitespace = true,
    MultiLine = true,
    RegexBasicTokens = ["string", "dec",]
  };
}

/// <summary>
/// Basic interface for JSON parts.
/// </summary>
public interface IJSONNode
{
  /// <summary>
  /// The value stored in this node.
  /// </summary>
  object? Value { get; }
  /// <summary>
  /// Gets the JSON of this node.
  /// </summary>
  /// <returns>The JSON as a string.</returns>
  string? ToString () => Value switch
  {
    null => "null",
    string str => $"\"{str}\"",
    bool b => b ? "true" : "false",
    IEnumerable<JSONProperty> props => $"[{props.TextJoin(",")}]",
    IEnumerable<JSONValue> vals => $"{{{vals.TextJoin(",")}}}",
    IConvertible iConv => iConv.ToString(CICC),
    _ when Value.IsCollection() => Value.AsCollection().Select(item => item is IConvertible cConv ? cConv.ToString(CICC) : "null").TextJoin(","),
    _ => throw new InvalidCastException("Unknown object type.")
  };
}

/// <summary>
/// A JSON value.
/// </summary>
/// <param name="value"></param>
public class JSONValue (object? value = null) : IJSONNode
{
  /// <inheritdoc/>
  public object? Value { get; } = value;
}
/// <summary>
/// A JSON keyed property.
/// </summary>
/// <param name="key">The key name.</param>
/// <param name="value">The value stored.</param>
public class JSONProperty (string key, object? value = null) : IJSONNode
{
  /// <summary>
  /// Gets the property key name.
  /// </summary>
  public string Key { get; } = key;
  /// <inheritdoc/>
  public object? Value { get; } = value;
}
