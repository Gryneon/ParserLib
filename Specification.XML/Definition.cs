#pragma warning disable RE0001 // Invalid regex pattern

using Parser;
using Parser.Ops;
using Parser.Ops.Text;

using static Parser.DefinitionStaticFunctions;

namespace Specification.XML;

public enum XMLTokenType
{
  Unknown,
  Comment,    // <!-- comment -->

  Ao, Ac,     // < >
  Qm, Sc,     // ? ;
  An, Co,     // & :
  Sl, Eq,     // / =

  AttrKey,
  AttrValue,
  ElementName,
  Namespace,
  Content,

  // Structures
  Attribute,
  ElementStart,
  ElementEnd,
  ElementSingle,
  Header
}

/// <summary>
/// The XML definition object.
/// </summary>
[DefinitionExport]
public static class Definition
{
  /// <summary>
  /// <para>XML Regex for tokens</para>
  /// Old: <see href="https://regex101.com/r/PTKqnJ/3"/><br/>
  /// New: <see href="https://regex101.com/r/jcPotD/4"/>
  /// </summary>
  public static RxSCollection Regex => [
    Nm("m_element", $@"<\s*(?'m_endtag'\/)?\s*{TagName}{Gp(WS + Attribute).Any}\s*(?'m_noinsidetag'\/)?\s*>"),
    Nm("m_header", $@"<\?\s*{TagName}{Gp(WS + Attribute).Any}\s*\?>"),
    Nm("m_ws", $@"(?<=>){WS}(?=<)"),
    Nm("m_comment", @"<!--(-(?!-)|[^-])*?-->"),
    Nm("m_content", @"[^<]+")
  ];

  /// <summary>
  /// The attribute regular expression.
  /// </summary>
  private static readonly RxS
    Attribute = Rx(@"(?'m_prop_key_1'\w+)\s*=\s*""(?'m_prop_value_1'.*?)"""),
    TagName = Nm("m_prop_tagname", "[A-Za-z][a-zA-Z0-9]+"),
    WS = RX.WS;

  /// <summary>
  /// The XML specification.
  /// </summary>
  [Export("xml")]
  public static Spec Spec => new()
  {
    FileInferences = [

      IfN(ExtIs, "xml"),
      IfN(ExtIs, "csproj"),
      IfN(HeadSt, "<?xm")
    ],
    Name = "xml",
    RxOpt = ROML | ROEC | ROIPW,
    IsTextFile = true,
    SC = SCO,
    TokenType = typeof(XMLTokenType),

    Operations = [
      new TokenizeOperation(),
      new GenerateFromObjectOperation<XMLElementSingle>("tokens", "xml_single", "noinsidetag"),
      new GenerateFromObjectOperation<XMLElementClose>("tokens", "xml_close", "endtag"),
      new GenerateFromObjectOperation<XMLElementOpen>("tokens", "xml_open", "tagname"),
      new GenerateFromObjectOperation<XMLContent>("tokens", "xml_content", "content"),
      new GenerateFromObjectOperation<XMLHeader>("tokens", "xml_header", "header"),
      new GenerateFromObjectOperation<XMLComment>("tokens", "xml_comment", "header"),
      new ConsolidateOperation<IXMLObject>(["xml_single", "xml_close", "xml_open", "xml_content", "xml_header", "xml_comment"], "xml"),
      new XMLStackOperation("xml", "result"),
      Operation.End,
    ],
  };
}
