using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;

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
      new TokenAssembleOperation<ITT>("tokens", "tokens_assembled"),
      new GenerateOperation<TokenObject<ITT>, Section>(Section.Generate, item => item.Name.IsNotEmpty(), "tokens_assembled", "result"),
      Operation.End
    ],
    TokenTypeLookup = {
      ["None"] = ITT.None,
      ["Comment"] = ITT.Comment,
      ["Str"] = ITT.Str,
      ["Bo"] = ITT.Bo,
      ["Bc"] = ITT.Bc,
      ["Eq"] = ITT.Eq,
      ["Section"] = ITT.Section,
      ["Property"] = ITT.Property,
      ["Ws"] = ITT.Ws},
    TokenRules = [
      new(TokenMatch | Competitive, ITT.Str, @"""([^\\]|\\.)*"""),
      new(TokenMatch | Competitive | IgnoredToken, ITT.Comment, @";.*?$"),
      new(TokenMatch, ITT.Bo, @"\["),
      new(TokenMatch, ITT.Bc, @"\]"),
      new(TokenMatch, ITT.Eq, @"\="),
      new(TokenMatch | IgnoredToken, ITT.Ws, @"\s+"),
      new(TokenMatch, ITT.Section, @"(?<=\[).*?(?=\])"),],
    GroupTokenRules = [
      new(BuildProperty, ITT.Property, "tn:Str tx:Eq tv:Str"),
      new(BuildLabel, ITT.Section, "tx:Bo tx:Section tx:Bc"),
      new(BuildObject, ITT.Section, "tn:Section tpm:Property"),
    ]
  };
}
