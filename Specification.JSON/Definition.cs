using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Common.Regex;

using Parser;
using Parser.Text.Ops;
using Parser.Text.Tokens;

using static Common.Names;

namespace Specification.JSON;

/// <summary>
/// Defines a JSON Specification.
/// </summary>
public static class Definition
{
  /// <summary>
  /// https://regex101.com/r/hgS1Zq/5
  /// </summary>
  private static RxSList Reader { get; } = [
    Nm("string", @""".*?"""),
    Nm("dec", @"-?[0-9]*\.[0-9]+"),
    Nm("int", @"-?[0-9]+(?:\.0+|\.)?"),
    Nm("ws", @"\s+"),
    Nm("colon", @"\:"),
    Nm("comma", @"\,"),
    Nm("arr_start", @"\["),
    Nm("arr_end", @"\]"),
    Nm("obj_start", @"\{"),
    Nm("obj_end", @"\}"),
    Nm("bool", "true|false"),
    //Nm("null", "null")
  ];
  private static readonly Collection<TokenType> TokenInfo = [
    "int",
    Mt("ws") | TF_Ignore,
    "string",
    "bool",
    "colon",
    "comma",
    "arr_start",
    "arr_end",
    "obj_start",
    "obj_end",
  ];
  private static Collection<TokenTemplate> TokenTemplates { get; } =
  [
    new()
    {
      Type = "Property",
      Template =
      [
        new("string", null, "key"),
        new("colon"),
        new(["int", "decimal", "string", "bool", "object", "array"], null, "value"),
        new(Mt("Comma") | TF_Optional)
      ]
    },
    new()
    {
      Type = "Array",
      Template = [
        new("lbrace"),
        new(Mt("arrayitem") | TF_OneOrMany),
        new("rbrace"),
        ]
    },
    new()
    {
      Type = "arrayitem",
      Template = [
        new(["int", "decimal", "string", "bool", "object", "array"]),
        new(Mt("comma") | TF_Optional)
        ]
    },
    new()
    {
      Type = "object",
      Template = [
        new("lbracket"),
        new(Mt("property") | TF_OneOrMany),
        new("rbracket"),
        ]
    },
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
        new DictionaryOperation(Reader, "initial", "matches"),
        new DebugToStringOperation("matches"),
        new TokenizeOperation(),
        new DebugToStringOperation("tokens"),
        new DebugWaitForInputOperation(),
        new TokenTemplateOperation("tokens", "tokens_templated", TokenTemplates),
        new DebugToStringOperation("tokens_templated"),
        new DebugWaitForInputOperation(),
        new CopyOperation("tokens_templated", "result")
      ],
    TokenLookup = TokenInfo
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
