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
      [Value] = [Int, Bool, Char, Str, Fixed, Expression, Name, FunctionCall, ArrayValue, ExpressionStatement, ParameterValue],
      [Statement] = [VarDecl, BasicCmd, FunctionCallStatement, VarAssn, VarInc, ArrayDecl, WaitCall],
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
      Tm(Minus, @"-"),
      Tm(Assign, @"(<<|>>| \|\| |&&)="),
      Tm(Binary, @"== | [!<>]="),
      Tm(Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      .. TokenRule.MakeSingleCharRules("+/%|&^*><-", TExact , Binary),
      .. TokenRule.MakeSingleCharRules("[]{}()=,:;", TExact , new ATT[] { Ao, Ac, Bo, Bc, Po, Pc, Eq, Cm, Co, Sc }),

      Tm(Void, @"\bvoid\b"),
      Tm(Type, @"\b(int|str|char|bool)\b"),
      Tm(Name, @"\b[a-z_][\w]*\b"),
    ],
    GroupTokenRules = [
      // Expressions
      new(RT.BuildTypedValue, ArrayValue,                 "n:Name x:Ao v:Value x:Ac"),
      new(RT.BuildExpression | RT.Recursive, Expression,  "l:Value y:Binary r:Value"),
      new(RT.BuildExpression, ExpressionStatement,        "l:Value y:IncDec"),
      new(RT.BuildExpression, ExpressionStatement,        "y:IncDec r:Value"),
      new(RT.BuildExpression, Expression,                 "y:(Unary|Minus) r:Value"),
      new(RT.BuildExpression | RT.Recursive, Expression,  "l:Value y:Binary r:Value"),
      new(RT.BuildTypedValue, Expression,                 "x:Po v:Expression x:Pc"),
      new(RT.BuildStatement, FunctionCall,                "n:Name x:Po p:Value x:Pc"),
      new(RT.BuildStatement, FunctionCall,                "n:Name x:Po pa:ParameterValue p:Value x:Pc"),
      new(RT.BuildStatement, FunctionCall,                "n:Name x:Po pa:ParameterValue x:Pc"),

      new(RT.BuildProperty, PreprocessorFull,             "yi:Preprocessor{#Define|#LibDefine} n:Name v:Value"),
      new(RT.BuildProperty, PreprocessorFull,             "yi:Preprocessor{#Import|#Library|#Include} v:Str"),

      // Statements
      new(RT.BuildStatement, VarDecl,                     "y:Type n:Name x:Sc"),
      new(RT.BuildStatement, VarDeclAssn,                 "y:Type n:Name x:Eq v:Value x:Sc"),
      new(RT.BuildStatement, ArrayDecl,                   "y:Type d:ArrayValue x:Sc"),
      new(RT.BuildStatement, BasicCmd,                    "n:SimpleJump x:Sc"),
      new(RT.BuildStatement, BasicCmd,                    "n:Return x:Sc"),
      new(RT.BuildProperty, BasicCmd,                     "n:Return v:value x:Sc"),
      new(RT.BuildStatement, WaitCall,                    "y:Wait x:Po pm:ParameterValue x:Pc x:Sc"),
      new(RT.BuildStatement, FunctionCallStatement,       "d:FunctionCall x:Sc"),

      new(RT.BuildStatement, Parameter,                   "y:Type n:Name xo:Cm"),
      new(RT.BuildExpression, PrintParameterValue,        "n:Name{s|i} x:Co pa:Value xo:Cm"),
      new(RT.BuildTypedValue, ParameterValue,             "v:Value x:Cm"),

      new(RT.BuildStatement, FunctionHeader,              "x:Function y:(Type|Void) n:Name x:Po x:Void x:Pc"),
      new(RT.BuildStatement, FunctionHeader,              "x:Function y:(Type|Void) n:Name x:Po pm:Parameter x:Pc"),
      new(RT.BuildStatement, FunctionFull,                "d:FunctionHeader x:Bo s:(Statement|Block) x:Bc"),

      new(RT.BuildStatement, ScriptHeader,                "x:Script n:Value y:ScriptType x:Po p:Parameter x:Pc"),

      new(RT.BuildStatement, ScriptHeader,                "x:Script n:Value y:ScriptType"),
      new(RT.BuildStatement, ScriptHeader,                "x:Script n:Value x:Po pm:Parameter x:Pc"),
      new(RT.BuildStatement, ScriptHeader,                "x:Script n:Value x:Po p:Void x:Pc"),
      new(RT.BuildStatement, ScriptFull,                  "d:ScriptHeader x:Bo s:(Statement|Block) x:Bc"),
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
