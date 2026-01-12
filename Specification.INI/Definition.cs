using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

namespace Specification.INI;

/// <summary>Defines the INI spec.</summary>
[DefinitionExport]
public static class Definition
{
  private const RegexOptions RXOptions = ROML | ROIPW | ROEC;
  private const TokenRuleType Competes = TMatches | Competitive;
  private const TokenRuleType TMatches = TokenMatch | ExemptAllWithin | IgnoreCase;
  private const TokenRuleType TExactly = TokenExact | ExemptAllWithin | IgnoreCase;
  private const TokenRuleType TExtract = TokenExtract | ExemptAllWithin | IgnoreCase;

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
    TokenType = typeof(ITT),
    Operations = [
      //new DictionaryOperation(Regex, RXOptions),
      //new GenerateOperation<MatchDataSet, Section>(Section.Generate, item => item.HasGroup("section"), "matches", "sections"),
      //new ExternalOperation<IEnumerable<Section>, INIDocument>(INIDocument.FromSections, item => true, "sections", "result"),
      //Operation.End],
      new TokenizeOperation<ITT>(),
      new TokenAssembleOperation<ITT>(),
      new GenerateOperation<TokenObject, Section>(Section.Generate, item => item.Name.IsNotEmpty(), "tokens_assembled", "result"),
      Operation.End
    ],
    TokenRules = [
      new(Competes, ITT.Str, @"""([^\\]|\\.)*"""),
      new(Competes | IgnoredToken, ITT.None, @";.*?$"),
      new(TExactly, ITT.Eq, @"="),
      new(TExtract, ITT.Section, @"\[(?'keep'.*?)\]")],
    GroupTokenRules = [
      new(BuildProperty, ITT.Property, "tn:Str tx:Eq tv:Str"),
      new(BuildObject, ITT.Section, "tn:Section tpm:Property"),
    ]
  };
}
