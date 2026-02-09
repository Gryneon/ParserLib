using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.TokenRule;
using static Parser.Tokens.TokenRuleType;

namespace Specification.INI;

/// <summary>Defines the INI spec.</summary>
[DefinitionExport]
public static class Definition
{
  /// <summary>The INI Spec</summary>
  [Export]
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
      new GenerateOperation<TokenObject, INISection>(INISection.Generate, static item => item.Name.IsNotEmpty(), "tokens_assembled", "result"),
      Operation.End
    ],
    TokenRules = [
      new(Competitive | IgnoredToken, ITT.None, @";.*?$"),
      new(Competitive, ITT.Value, @"(?<=(=))([^\\=\n;]|\\.)*(?=$|;)"),
      new(Competitive, ITT.Key, @"(?<=^\s*)([^\s\\=\n;]|\\.)*(?=\s*(=))"),
      .. MakeSingleCharRules("=", TokenExact, new ITT[] { ITT.Eq } ),
      new(TokenExtract, ITT.Section, @"\[(?'keep'.*?)\]")],
    GroupTokenRules = [
      new(BuildProperty, ITT.Property, "tn:Key tx:Eq tv:Value"),
      new(BuildObject, ITT.SectionWProps, "tn:INISection tpm:Property")
    ],
    DefaultRuleSet = ExemptAllWithin | IgnoreCase,
  };
}
