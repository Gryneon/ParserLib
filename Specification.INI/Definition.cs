using System.Text.RegularExpressions;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenStaticFunctions;

namespace Specification.INI;

/// <summary>
/// Defines the INI spec.
/// </summary>
[DefinitionExport]
public static class Definition
{
  private const RegexOptions RXOptions = ROML | ROIPW | ROEC;

  private static RxSCollection Regex { get; } = [
    MarkAs("lncomment", Op(";") + LazyOneLn + LnEnd),
    MarkAs("section", Op("[") + Mp("sectionkey", @"[^\[\]\n\r\0\f\v\a\b\t]") + Op("]")),
    MarkAs("property", Ws + Kp(1, @"[\w_]") + Ws + Op("=") + Ws + Vp(1, @"[^;\n]+?") + Ws + En),
  ];

  /// <summary>
  /// The INI Spec
  /// </summary>
  [Export("ini")]
  public static Spec Spec => new()
  {
    Name = "ini",
    FileInferences = [
      IfN(ExtIs, "ini"),
      IfN(ExtIs, "vnc"),
      IfN(ExtIs, "inf")],
    RxOpt = RXOptions,
    Operations = [
      new DictionaryOperation(Regex, RXOptions),
      new GenerateOperation<Section>(Section.Generate, item => item.HasGroup("section"), "matches", "sections"),
      new ExternalOperation<IEnumerable<Section>, INIDocument>(INIDocument.FromSections, item => true, "sections", "result"),
      Parser.Ops.Operation.End]
  };
}
