using Common.Extensions;
using Common.Regex;

using static Parser.DefinitionStaticFunctions;
using static Parser.V2.Framework.TokenType;

namespace Parser.V2.Framework.JSON;

public class JSONDataFormat (string name) : DataFormat(name)
{
  public override DataFormatType Type => DataFormatType.JSON;
  public bool Strict { get; set; }
  public Collection<RxS> RemoveWS { get; } = [
    Or(@""".*?""", @"\s+", @"\/\/.*(?:\r\n?|\n)", @"\/\*")
  ];
  public RxSList FormatRegex { get; } = [
    Nm("bool", @"true|false"),
    Nm("lobj", @"\{"),
    Nm("larr", @"\["),
    Nm("robj", @"\}"),
    Nm("rarr", @"\]"),
    Nm("qt", @""".*?"""),
    Nm("int", @"\-?\d+|0x[\dabcdefABCDEF]{,16}"),
    Nm("cm", @"\,"),
    Nm("col", @"\:"),
    Nm("dec", @"\-?(\d*\.\d+|\d+\.\d*)")
  ];
  public Dictionary<string, TokenType> TokenLookup { get; } = [
    K("bool", T_Bool),
    K("lobj", T_LBracket),
    K("larr", T_LBrace),
    K("robj", T_RBracket),
    K("rarr", T_RBrace),
    K("qt", T_DblQt),
    K("int", T_Int),
    K("cm", T_Comma),
    K("col", T_Colon),
    K("dec", T_Dec)
  ];
  public Collection<TokenSequence> TokenReducer { get; } = [
    new(T_Property, [
      new(T_String, null, "key"),
      new(T_Colon),
      new([T_Int, T_Bool, T_Dec, T_String, T_Object, T_Array], null, "value"),
      new(T_Comma | T_Optional)]),
    new(T_Object, [
      new(T_LBracket),
      new(T_Property | T_OneOrMany, null, "properties"),
      new(T_RBracket)]),
    new (T_Array, [
      new(T_LBrace),
      new( new TokenType[] { T_Object, T_Int, T_String, T_Dec, T_Bool, T_Array }.AddFlag(T_OneOrMany), null, "items"),
      new(T_RBrace)]),
  ];
}
