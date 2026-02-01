#pragma warning disable RE0001 // Invalid regex pattern
#pragma warning disable CA1720 // Identifier contains type name

using Parser;
using Parser.Ops;
using Parser.Ops.Text;
using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

using XTT = Specification.XML.XMLTokenType;

namespace Specification.XML;

public enum XMLTokenType
{
  Unknown,
  Comment,    // <!-- comment -->

  Ao, Ac,     // < >
  Qm, Sc,     // ? ;
  An, Co,     // & :
  Sl, Eq,     // / =
  Em, Hy,     // ! -

  AttrKey,
  AttrValue,
  ElementName,
  Namespace,
  Content,
  DString,
  SString,

  // Groups
  String,

  // Structures
  Attribute,
  ElementStart,
  ElementEnd,
  ElementSingle,
  Header,
  NamespaceAttr
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
    TokenType = typeof(XTT),
    TokenRules = [
      new(Competitive, XTT.DString, @"""[^""]*"""),
      new(Competitive, XTT.SString, @"'[^']*'"),
      new(Competitive, XTT.Comment, @"\<\!\s*--([^-]|(?<!-)-(?!-))*--\s*\>"),
      .. TokenRule.MakeSingleCharRules("<>/?;&:=!-", TokenExact, new Collection<XTT>() { XTT.Ao, XTT.Ac, XTT.Sl, XTT.Qm, XTT.Sc, XTT.An, XTT.Co, XTT.Eq, XTT.Em, XTT.Hy }),
      new (TokenMatch, XTT.NamespaceAttr, @"\bxmlns\b"),
      new (TokenMatch, XTT.AttrKey, @"\b[a-z]\w*\b(?=\s*[=])"),
      new (TokenMatch, XTT.Namespace, @"(?<=(\/|\<)\s* )\b[a-z]\w*\b(?=:)"),
      new (TokenMatch, XTT.ElementName, @"(?<=(\/|\< |:)\s* )\b[a-z]\w*\b(?=\s*[^=])"),
    ],
    DefaultRuleSet = ExemptAllWithin | IgnoreCase,
    GroupTokenRules = [
      new (BuildProperty, XTT.Attribute, "tin:AttrKey tx:Eq tiv:String")
    ],
    TokenCompatLookup = {
      [XTT.String] = [XTT.DString, XTT.SString]
    },
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      Operation.End,
    ],
  };
}
