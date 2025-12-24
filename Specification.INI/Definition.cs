using Parser.Tokens.Raw;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.Raw.TokenRuleType;
using static Parser.Tokens.TokenStaticFunctions;

namespace Specification.INI;

public enum INITokenType
{
  None,
  Comment,
  Section,
  Property,
  Str,
  Ws,
  Bo, // [
  Bc, // ]
  Eq, // =
}

/// <summary>Defines the INI spec.</summary>
[DefinitionExport]
public static class Definition
{
  private const RegexOptions RXOptions = ROML | ROIPW | ROEC;

  private static RxSCollection Regex { get; } = [
    MarkAs("lncomment", Op(";") + LazyOneLn + LnEnd),
    MarkAs("section", Op("[") + Mp("sectionkey", @"[^\[\]\n\r\0\f\v\a\b\t]") + Op("]")),
    MarkAs("property", Ws + Kp(1, @"[\w_]") + Ws + Op("=") + Ws + Vp(1, @"[^;\n]+?") + Ws + En),
  ];

  /// <summary>The INI Spec</summary>
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
      //new DictionaryOperation(Regex, RXOptions),
      //new GenerateOperation<MatchDataSet, Section>(Section.Generate, item => item.HasGroup("section"), "matches", "sections"),
      //new ExternalOperation<IEnumerable<Section>, INIDocument>(INIDocument.FromSections, item => true, "sections", "result"),
      //Operation.End],
      new TokenizeOperation<INITokenType>(Spec.LoadFromSpec, "text", "tokens"),

    ],
    TokenRules = [
      new(TokenMatch | Competitive, ITT.Str, @"""([^\\]|\\.)*"""),
      new(TokenMatch | Competitive | IgnoredToken, ITT.Comment, @";.*?$"),
      new(TokenMatch, ITT.Bo, @"\["),
      new(TokenMatch, ITT.Bc, @"\]"),
      new(TokenMatch, ITT.Eq, @"\="),
      new(TokenMatch | IgnoredToken, ITT.Ws, @"\s+"),
      new(TokenMatch, ITT.Section, @"(?<=\[).*?(?=\])"),],
    GroupTokenRules = [
      new(BuildProperty, ITT.Property, "tk:Str tx:Eq tv:(Str|)"),
      new(BuildProperty, ITT.Section, "tx:Bo tx:Eq tx:Bc"),
      new(BuildProperty, ITT.Property, "tk:Str tx:Eq tv:Str"),
    ]
  };
}
