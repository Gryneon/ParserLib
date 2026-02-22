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
[DefinitionExport]
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
  [DefinitionExport]
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
      [ParameterValue] = [ParameterExpression],
      [Value] = [Int, Bool, Char, Str, Fixed, Expression, Name, FunctionCall, ArrayValue, ExpressionStatement],
      [Statement] = [VarDecl, BasicCmd, FunctionCallStatement, VarAssn, VarInc, ArrayDecl, WaitCall, Block, VarDeclAssn],
      [Block] = [IfBlock, ElseBlock, ElseIfBlock]
    },
    TokenRules = [

      // Data
      new(Compet, Str, @"""([^\\""]|\\.)*"""),
      new(Compet, Char, @"'([^\\']|\\.)*'"),
      new(Ignore, None, @"\/\/.*?$"),
      new(Ignore, None, @"\/\*.*?\*\/"),

      // Preprocessor
      Tm(Preprocessor, @"\# \w+"),
      Tm(Bool, @"\b(true|false|on|off|yes|no)\b"),
      Tm(Int, @"-?(?<!\.|\w)(\d+|0x[a-f0-9]+)(?!\.|\w)"),
      Tm(Fixed, @"-?(\d+\.\d*|\.\d+)"),

      // Keywords
      .. TokenRule.MakeWordMatchRules(true, [
        "script", "function", "net", "if", "else",
        "do", "for", "switch", "case", "default", "return"
      ]),
      Tm(ScriptType, @"\b(enter|return|death|lightning|kill|reopen|open|unloading|disconnect|respawn|lightning)\b"),
      Tm(SimpleJump, @"\b(break|continue|terminate|restart)\b"),
      Tm(Wait, @"\b(delay|scriptwait|tagwait)\b"),
      Tm(MapVar, @"\b(world|global)\b"),
      Tm(Loop, @"\b(while|until)\b"),

      // Operators
      Tm(IncDec, @"(\+\+|--)"),
      Tm(Unary, @"!(?!=)|~"),
      Tm(Assign, @"[-+*^/%|&]="),
      Tm(Assign, @"(<<|>>| \|\| |&&)="),
      Tm(Binary, @"== | [!<>]="),
      Tm(Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      .. TokenRule.MakeSingleCharRules("+/%|&^*><", TExact , Binary),
      .. TokenRule.MakeSingleCharRules("[]{}()=,:;-", TExact , new ATT[] { Ao, Ac, Bo, Bc, Po, Pc, Eq, Cm, Co, Sc, Minus }),

      Tm(Void, @"\bvoid\b"),
      Tm(Type, @"\b(int|str|char|bool)\b"),
      Tm(Name, @"\b[a-z_][\w]*\b"),
    ],
    GroupTokenRules = [
      // Paremeter Expressions
      new(RT.None, Parameter,                   "t:Type n:Name x:Cm"),
      new(RT.None, FinalParameter,              "t:Type n:Name x:Pc"),
      new(RT.None, PrintParameterValue,         "n:Name{s|i} x:Co pa:Value xo:Cm"),
      new(RT.None, PrintFunction,               "n:name x:Po p:PrintParameterValue x:Pc x:Sc"),
      new(RT.None, ParameterValue,              "d:Value x:Cm"),
      new(RT.Recursive, ParameterExpression,    "l:Value t:Binary r:ParameterValue"),

      // Expressions
      new(RT.None, FunctionCallOpen,            "n:Name x:Po"),
      new(RT.None, ArrayValue,                  "n:Name x:Ao v:Value x:Ac"),
      new(RT.Recursive, Expression,             "l:Value t:Binary r:Value"),
      new(RT.None, ExpressionStatement,         "l:Value t:IncDec"),
      new(RT.None, ExpressionStatement,         "t:IncDec r:Value"),
      new(RT.Recursive, Expression,             "l:Value t:(Binary|Minus) r:Value"),
      new(RT.None, Expression,                  "t:(Unary|Minus) r:Value"),
      new(RT.None, Expression,                  "x:Po v:Value x:Pc"),
      new(RT.None, FunctionCall,                "d:FunctionCallOpen p:Value x:Pc"),
      new(RT.None, FunctionCall,                "d:FunctionCallOpen pa:ParameterValue p:Value x:Pc"),
      new(RT.None, FunctionCall,                "d:FunctionCallOpen x:Pc"),

      // Run Again
      new(RT.None, ArrayValue,                  "n:Name x:Ao v:Value x:Ac"),

      new(RT.None, PreprocessorFull,            "yi:Preprocessor{#Define|#LibDefine} n:Name v:Value"),
      new(RT.None, PreprocessorFull,            "yi:Preprocessor{#Import|#Library|#Include} v:Str"),

      // Statements
      new(RT.None, VarDecl,                     "t:Type n:Name x:Sc"),
      new(RT.None, VarDeclAssn,                 "t:Type n:Name x:Eq v:Value x:Sc"),
      new(RT.None, VarAssn,                     "n:(ArrayValue|Name) x:(Eq|Assign) v:Value x:Sc"),
      new(RT.None, ArrayDecl,                   "t:Type n:ArrayValue x:Sc"),
      //new(RT.None, ArrayDecl,                   "t:Type n:Name x:Ao v:Value x:Ac x:Sc"),
      //new(RT.None, ArrayDecl,                   "t:Type n:Name x:Ao l:Value x:Ac x:Ao r:Value x:Ac x:Sc"),
      new(RT.None, BasicCmd,                    "n:SimpleJump x:Sc"),
      new(RT.None, BasicCmd,                    "n:Return x:Sc"),
      new(RT.None, BasicCmd,                    "n:Return v:value x:Sc"),
      new(RT.None, WaitCall,                    "t:Wait x:Po p:ParameterValue x:Pc x:Sc"),
      new(RT.None, FunctionCallStatement,       "d:FunctionCall x:Sc"),
      new(RT.None, CaseLabel,                   "x:Case n:Value x:Co"),
      new(RT.None, CaseLabel,                   "n:Default x:Co"),

      new(RT.None, IfBlock,                     "t:If v:Expression x:Bo pa:Statement x:Bc"),
      new(RT.None, ElseBlock,                   "t:Else x:Bo pmo:Statement x:Bc"),
      new(RT.None, ElseIfBlock,                 "t:Else x:If v:Expression x:Bo pa:Statement x:Bc"),

      new(RT.None, FunctionHeader,              "x:Function t:(Type|Void) n:Name x:Po x:Void x:Pc"),
      new(RT.None, FunctionHeader,              "x:Function t:(Type|Void) n:Name x:Po pm:Parameter x:Pc"),
      new(RT.None, FunctionFull,                "d:FunctionHeader x:Bo s:(Statement|Block) x:Bc"),

      new(RT.None, ScriptHeader,                "x:Script n:Value t:ScriptType x:Po p:FinalParameter"),

      new(RT.None, ScriptHeader,                "x:Script n:Value t:ScriptType"),
      new(RT.None, ScriptHeader,                "x:Script n:Value x:Po pmo:Parameter p:FinalParameter"),
      new(RT.None, ScriptHeader,                "x:Script n:Value x:Po p:Void x:Pc"),
      new(RT.None, ScriptHeader,                "d:Script n:Value t:Return"),
      new(RT.None, ScriptFull,                  "d:ScriptHeader x:Bo s:(Statement|Block) x:Bc"),
    ],
  };
  [DefinitionExport]
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
