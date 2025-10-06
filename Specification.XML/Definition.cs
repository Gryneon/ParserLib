#pragma warning disable RE0001 // Invalid regex pattern

using Parser;
using Parser.Ops;
using Parser.Text.Ops;

using static Parser.DefinitionStaticFunctions;

namespace Specification.XML;

/// <summary>
/// The XML definition object.
/// </summary>
public static class Definition
{
  /// <summary>
  /// <para>XML Regex for tokens</para>
  /// New: https://regex101.com/r/PTKqnJ/3
  /// Old: https://regex101.com/r/jcPotD/4
  /// </summary>
  public static RxSCollection Regex => [
    Nm("tagname", $@"<\s*(?<endtag>\/)?\s*(?<tagname>[A-Za-z][a-zA-Z0-9]*)(?:\s+{Attribute})*\s*(?<noinsidetag>\/)?\s*>"),
    Nm("header", Rx(@"<\?(?<tagname>[A-Za-z][a-zA-Z0-9]*)(?:\s+") + Attribute + Rx(@")*\s*\?>")),
    Nm("ws", @"(?<=>)\s+(?=<)"),
    Nm("comment", @"<!--(-(?!-)|[^-])*?-->"),
    Nm("content", @"[^><]*")
  ];

  /// <summary>
  /// The attribute regular expression.
  /// </summary>
  private static RxS Attribute => Rx(@"(?<attrname>\w+)\s*=\s*""(?<attrval>.*?)""");

  /// <summary>
  /// Operation definition.
  /// </summary>
  private static Collection<IOperation> GenerationOps => [
    new GenerateFromObjectOperation<XMLElementSingle>("tokens", "xml_single", "noinsidetag"),
    new GenerateFromObjectOperation<XMLElementClose>("tokens", "xml_close", "endtag"),
    new GenerateFromObjectOperation<XMLElementOpen>("tokens", "xml_open", "tag"),
    new GenerateFromObjectOperation<XMLContent>("tokens", "xml_content", "content"),
    new GenerateFromObjectOperation<XMLHeader>("tokens", "xml_header", "header"),
    new ConsolidateOperation<IXMLObject>(["xml_single", "xml_close", "xml_open", "xml_content", "xml_header"], "xml"),
  ];
  /// <summary>
  /// The XML specification.
  /// </summary>
  public static TextSpec Spec => new()
  {
    FileInferences = [

      IfN(ExtIs, "xml"),
      IfN(ExtIs, "csproj"),
      IfN(HeadSt, "<?xm")
    ],
    Name = "xml",
    CaseInsensitive = false,
    ExplicitCapture = true,
    Operations = [
      new DictionaryOperation(Regex, false),
      new TokenizeOperation(),
      .. GenerationOps,
      new XMLStackOperation("objects", "xml"),
      Operation.End,
    ],
    RegexBasicTokens = {
      "endtag",
      "noinsidetag",
      "tagname",
      "content",
      "header",
    },
    WhitespaceTokens =
    {
      "ws",
      "comment"
    }
  };
}
