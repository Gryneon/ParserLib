#pragma warning disable CA1822 // Mark members as static

using Parser.Ops.Text;

using static Parser.DefinitionStaticFunctions;

namespace Parser;

[DefinitionExport]
public static class DefaultSpec
{
  /// <summary>Splits a <see langword="string"/> on newlines into a <see cref="Collection{T}"/> of <see langword="string"/> objects.</summary>
  [DefinitionExport]
  public static Spec TextByLines { get; } = new()
  {
    FileInferences = [],
    Name = "textbylines",
    Operations = [
      new SplitOperation("text", "result")
    ]
  };

  public static Spec SpecLoader { get; } = new()
  {
    FileInferences = [new(ExtIs, "spec")],
    Name = "spec_internal",
    Operations = [
      new ReplaceOperation
      {
        InputKey = "text",
        OutputKey = "text",
        Nodes = [ ],
        ContinueOnFail = true
      },
      new TokenizeOperation
      {
        InputKey = "text",
        OutputKey = "tokens"
      },
      // TODO: Complete this operation for spec parsing.
      new SpecProcessOperation
      {
        InputKey = "tokens",
        OutputKey = "specs"
      }
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(string),
    DefaultRuleSet = RT.IgnoreCase,
    RxOpt = ROEC | ROIC | ROML | ROIPW,
    TokenRules = [
      new(RT.TokenComment, "None", @"\s*\/\/.*"),
      new(RT.TokenComment, "None", @"^\s*(?=\S)"),
      new(RT.TokenExtract, "SpecName", @"Spec\s+""(?'keep'[^\n""])"""),
      new(RT.TokenMatch, "BracketOpen", @"(?<!\\){(?!\s*[\d,}])"),
      new(RT.TokenMatch, "BracketClose", @"(?<!\\)}(?=$)"),
    ]
  };
}
