using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRuleType;
using static Parser.Tokens.TokenRule;

namespace Specification.INI;

/// <summary>Defines the INI spec.</summary>
[DefinitionExport]
public static class Definition
{
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
    RxOpt = ROML | ROIPW | ROEC | ROIC,
    TokenType = typeof(ITT),
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      new GenerateOperation<TokenObject, Section>(Section.Generate, item => item.Name.IsNotEmpty(), "tokens_assembled", "result"),
      Operation.End
    ],
    TokenRules = [
      new(Competes | IgnoredToken, ITT.None, @";.*?$"),
      new(Competes, ITT.Value, @"(?<=(=))([^\\=\n;]|\\.)*(?=$|;)"),
      new(Competes, ITT.Key, @"(?<=^\s*)([^\s\\=\n;]|\\.)*(?=\s*(=))"),
      .. MakeSingleCharRules("=", TExactly, new ITT[] { ITT.Eq } ),
      new(TExtract, ITT.Section, @"\[(?'keep'.*?)\]")],
    GroupTokenRules = [
      new(BuildProperty, ITT.Property, "tn:Key tx:Eq tv:Value"),
      new(BuildObject, ITT.SectionWProps, "tn:Section tpm:Property"),
    ]
  };
}
