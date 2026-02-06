#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable RE0001 // Invalid regex pattern

using Parser.Tokens;

using static Parser.DefinitionStaticFunctions;
using static Specification.ACS.ACSTokenType;

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

  /// <summary>Defined Specification</summary>
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
      [Bool] = [Int, Fixed, Char, Str],
      [Int] = [Fixed, Bool, Char, Str],
      [Fixed] = [Int, Bool, Char, Str],
      [Value] = [Int, Bool, Char, Str, Fixed, Expression, Name, FunctionCall, ArrayValue],
      [Statement] = [VarDecl, BasicCmd, FunctionCall, VarAssn, VarInc, IfBlock, ElseBlock, ElseIfBlock, ArrayDecl, ]
    },
    TokenRules = [

      // Data
      new(Compet, Str, @"""([^\\""]|\\.)*"""),
      new(Compet, Char, @"'([^\\']|\\.)*'"),
      new(Ignore, None, @"\/\/.*?$"),
      new(Ignore, None, @"\/\*.*?\*\/"),

      // Preprocessor
      Tm(Preprocessor, @"\# (lib(define|rary)|import|include|define)"),
      Tm(Bool, @"\b(true|false|on|off|yes|no)\b"),
      Tm(Int, @"-?(?<!\.|\w)(\d+|0x[a-f0-9]+)(?!\.|\w)"),
      Tm(Fixed, @"-?(\d+\.\d*|\.\d+)"),

      // Keywords
      .. TokenRule.MakeWordMatchRules(true, [
        ("script", Script),     ("function", Function),
        ("net", Net),
        ("if", If),             ("else", Else),
        ("global", MapVar),     ("world", MapVar),
        ("do", Do),             ("for", For),
        ("while", Loop),        ("until", Loop),
        ("switch", Switch),     ("case", Case),
        ("default", Default),   ("return", Return)
        ]
      ),
      Tm(ScriptType, @"\b(enter|return|death|lightning|kill|reopen|open|unloading|disconnect|respawn|lightning)\b"),
      Tm(SimpleJump, @"\b(break|continue|terminate)\b"),
      Tm(Wait, @"\b(delay|scriptwait|tagwait)\b"),

      // Operators
      Tm(IncDec, @"(\+\+|--)"),
      Tm(Unary, @"!(?!=)|~"),
      Tm(Minus, @"-"),
      Tm(Assign, @"[-+*^/%|&]="),
      Tm(Assign, @"(<<|>>| \|\| |&&)="),
      Tm(Binary, @"[!<>-]="),
      Tm(Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      Tm(Binary, @"[+/%|&^*><-]"),

      .. TokenRule.MakeSingleCharRules("[]{}()=,:;", TExact ,new ATT[] { Ao, Ac, Bo, Bc, Po, Pc, Eq, Cm, Co, Sc }),

      Tm(Void, @"\bvoid\b"),
      Tm(Type, @"\b(int|str|char|bool)\b"),
      Tm(Name, @"\b[a-z_][\w]*\b"),
    ],
    GroupTokenRules = [
      new(RT.BuildExpression | RT.Recursive, Expression, "v:Value y:Binary v:Value"),
      new(RT.BuildStatement, VarDecl, "y:Type n:Name x:Sc"),
      new(RT.BuildStatement, VarDeclAssn, "y:Type n:Name x:Eq v:Value x:Sc"),
      new(RT.BuildStatement, Statement, "n:SimpleJump x:Sc"),
      new(RT.BuildStatement, Parameter, "y:Type n:Name xo:Cm"),
      new(RT.BuildStatement, FunctionCall, "n:Name x:Po pa:Parameter x:Pc"),
      new(RT.BuildProperty, PreprocessorFull, "yi:Preprocessor{#Define|#LibDefine} n:Name v:Value"),
      new(RT.BuildProperty, PreprocessorFull, "yi:Preprocessor{#Import|#Library|#Include} v:Str"),
      new(RT.BuildTypedValue, ArrayValue, "n:Name x:Ao v:Value x:Ac")
    ],
  };
  [Export("zdoom.modeldef")]
  public static Spec ModelDef => new()
  {
    FileInferences = [IfNOr(
      IfN(ExtIs, "modeldef"),
      IfN(FName|Is, "modeldef"))],
    Name = "zdoom.modeldef",
    RxOpt = ROML | ROIC | ROIPW | ROEC,
    SC = SCOIC,
    IsTextFile = true,
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    TokenRules = [
      .. TokenRule.MakeSingleCharRules("{}()=,;", TExact ,new ATT[] { Bo, Bc, Po, Pc, Eq, Cm, Sc }),
    ],
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),

    ]
  };
}
