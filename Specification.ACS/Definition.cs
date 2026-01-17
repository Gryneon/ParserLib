#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1720 // Identifier contains type name

using Parser.Tokens;


using static Parser.DefinitionStaticFunctions;

namespace Specification.ACS;

/// <summary>
/// ACS Specification Definition <br/>
/// <see href="https://regex101.com/r/mTwORe/1">Regex</see>
/// https://regex101.com/r/FCoqFI/1
/// </summary>
[DefinitionExport(true)]
public static class Definition
{
  internal static RT Compet = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal static RT Ignore = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal static RT TMatch = RT.TokenMatch | RT.ExemptAllWithin | RT.IgnoreCase;
  internal static RT TExact = RT.TokenExact | RT.ExemptAllWithin | RT.IgnoreCase;
  internal static TokenRule Tm (ATT tokenType, [SS("regex")] string regex) => new(TMatch, tokenType, regex);
  internal static TokenRule Op ([SS("regex")] string regex) => Tm(ATT.Op, regex);
  internal static TokenRule Ex (ATT tokenType, [SS("regex")] string regex) => new(TExact, tokenType, regex);

  /// <summary>
  /// Defined Specification
  /// </summary>
  [Export("zdoom.acs")]
  public static Spec ACS => new()
  {
    FileInferences = [IfN(ExtIs, "acs")],
    Name = "zdoom.acs",
    RxOpt = ROML | ROIC | ROIPW | ROEC | ROSL,
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(ATT),
    TokenCompatLookup = {
      [ATT.] = ["", ""]
    },
    TokenRules = [

      // Data
      new(Compet, ATT.Str, @"""([^\\""]|\\.)*"""),
      new(Compet, ATT.Char, @"'([^\\']|\\.)*'"),
      new(Ignore, ATT.None, @"\/\/.*?$"),
      new(Ignore, ATT.None, @"\/\*.*?\*\/"),

      // Preprocessor
      Ex(ATT.Preprocessor, "# ((lib)?define|)"),
      Tm(ATT.Bool, @"\b(true|false|on|off|yes|no)\b"),
      Tm(ATT.Int, @"-?(\d+|0x[a-f0-9]+)(?!\.)"),
      Tm(ATT.Fixed, @"-?(\d+\.\d*|\d*\.\d+)"),

      // Keywords
      Tm(ATT.Script, @"\bscript\b"),
      Tm(ATT.ScriptType, @"\b(enter|return|death|lightning|kill|reopen|open|unloading|disconnect|respawn|lightning)\b"),
      Tm(ATT.Function, @"\bfunction\b"),
      Tm(ATT.MapVar, @"\b(global|world)\b"),
      Tm(ATT.Net, @"\bnet\b"),
      Tm(ATT.For, @"\bfor\b"),
      Tm(ATT.Condtional, @"\b(if|until|while)\b"),

      // Operators
      Tm(ATT.IncDec, @"(\+\+|--)"),
      Tm(ATT.Unary, @"!(?!=)|~"),
      Tm(ATT.Minus, @"-"),
      Tm(ATT.Assign, @"[-+*^/%|&]="),
      Tm(ATT.Assign, @"(<<|>>| \|\| |&&)="),
      Tm(ATT.Binary, @"[!<>-]="),
      Tm(ATT.Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      Tm(ATT.Binary, @"[+/%|&^*-]"),

      Ex(ATT.Ao, "["),
      Ex(ATT.Ac, "]"),
      Ex(ATT.Bo, "{"),
      Ex(ATT.Bc, "}"),
      Ex(ATT.Po, "("),
      Ex(ATT.Pc, ")"),
      Ex(ATT.Eq, "="),
      Ex(ATT.Cm, ","),
      Ex(ATT.Co, ":"),
      Ex(ATT.Sc, ";"),
    ],
    GroupTokenRules = [],
  };
  [Export("zdoom.modeldef")]
  public static Spec ModelDef => new()
  {
    FileInferences = [IfNOr(
      IfN(ExtIs, "modeldef"),
      IfN(FName|Is, "modeldef"))],
    Name = "zdoom.modeldef",
    RxOpt = ROML | ROIC | ROIPW | ROEC,
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),

    ]
  };
}
