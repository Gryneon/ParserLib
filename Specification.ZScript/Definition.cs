#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using System;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using static Parser.DefinitionStaticFunctions;

namespace Specification.ZDoom;

[DefinitionExport]
public static partial class Definition
{
  // Flags
  internal const RT RT_Comment = RT.Competitive | RT.IgnoredToken;

  /// <summary>https://regex101.com/r/En5C8c/7</summary>
  [DefinitionExport]
  public static Spec ZScript => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    Name = "zdoom.zscript",
    Operations = [
      new TokenizeOperation(),
      Op.End
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenCompatLookup = new()
    {
      [ZT.FrameLump] = [ZT.String, ZT.Name],
      [ZT.StateCmd] = [ZT.GotoCmd, ZT.LoopCmd, ZT.BasicCmd],
      [ZT.StateEntry] = [ZT.StateCmd, ZT.State, ZT.FrameDef]
    },
    TokenType = typeof(ZT),
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    TokenRules = [
      new(RT_Comment, ZT.None, @"\/\/[^\n]*"),
      new(RT_Comment, ZT.None, @"\/\*[\s\S]*?\*\/"),
      new(RT.Competitive, ZT.String, @"""([^""\\]|\\.)*"""),
      .. TokenRule.MakeSingleCharRules("{}();=,:+-", RT.TokenExact, new ZT[] { ZT.Bo, ZT.Bc, ZT.Po, ZT.Pc, ZT.Sc, ZT.Eq, ZT.Cm, ZT.Co, ZT.Pl, ZT.Mn })
    ],
    GroupTokenRules = [
     new(RT.TokenMatch, ZT.FrameDef, "n:(name|String) v:Name v:Num fo:Bright "),
      ]
  };

  [DefinitionExport]
  public static Spec UDMF => new()
  {
    Name = "zdoom.udmf",
    FileInferences = [IfN(InferenceType.Ext | InferenceType.Like, "udmf")],
    RxOpt = ROML | ROIPW | ROIC | ROEC | ROSL,
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      Op.End,
    ],
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    TokenRules = [
      new(RT.Competitive, UT.String, Rx(@"""(?:[^""\\\n\r]|\\.)*""")),
      new(RT_Comment, UT.None, Rx(@"//[^\n\r]*")),
      new(RT_Comment, UT.None, Rx(@"/\*.*?\*/")),
      new(RT.TokenExact, UT.Bo, "{"),
      new(RT.TokenExact, UT.Bc, "}"),
      new(RT.TokenExact, UT.Eq, "="),
      new(RT.TokenExact, UT.Sc, ";"),
      new(RT.TokenMatch, UT.Bool,  @"\b(true|false)\b"),
      new(RT.TokenMatch, UT.Dec,  Rx(@"-?\b(\d+\.\d+|\.?\d+)\b")),
      new(RT.TokenMatch, UT.Namespace, @"\bnamespace\b"),
      new(RT.TokenMatch, UT.Vertex,    @"\bvertex\b"),
      new(RT.TokenMatch, UT.Thing,     @"\bthing\b"),
      new(RT.TokenMatch, UT.SideDef,   @"\bsidedef\b"),
      new(RT.TokenMatch, UT.LineDef,   @"\blinedef\b"),
      new(RT.TokenMatch, UT.Sector,    @"\bsector\b(?=[^=}]*\{)"),
      new(RT.TokenMatch, UT.Name, Rx(@"\b[a-z]\w*\b"))],
    // new(RT.StoreExtra | RT.IgnoredToken | RT.ExemptAllWithin, Ws,   Rx(@"\s+"))],
    //new(RT.StoreOther, None)],
    GroupTokenRules = [
      new(UT.Structure, "n:Namespace x:Eq v:Str x:Sc"),
      new(UT.Property, "n:Name x:Eq v:Value x:Sc"),
      new(UT.Structure, "n:Vertex x:Bo pm:Property x:Bc"),
      new(UT.Structure, "n:Thing x:Bo pm:Property x:Bc"),
      new(UT.Structure, "n:Sector x:Bo pm:Property x:Bc"),
      new(UT.Structure, "n:LineDef x:Bo pm:Property x:Bc"),
      new(UT.Structure, "n:SideDef x:Bo pm:Property x:Bc"),
      ],
    SC = SCOIC,
    IsTextFile = true,
    TokenType = typeof(UT),
    TokenCompatLookup = new Dictionary<dynamic, Collection<dynamic>>()
    {
      [UT.Keyword] = [UT.Vertex, UT.Thing, UT.Sector, UT.LineDef, UT.SideDef],
      [UT.Op] = [UT.Eq, UT.Sc, UT.Bo, UT.Bc],
      [UT.Value] = [UT.Bool, UT.Dec, UT.String],
    },
  };

  [DefinitionExport]
  public static Spec SndInfo => new()
  {
    Name = "zdoom.sndinfo",
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    FileInferences = [
      new InferenceNodeOr([
        IfN(ExtIs, "sndinfo"),
        IfN(FName | Is, "sndinfo"),
      ])
    ],
    IsTextFile = true,
    TokenType = typeof(SndIT),
    SC = SCOIC,
    TokenRules = [],
    GroupTokenRules = [],
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      Op.End
    ],
  };

  /// <summary>Defines a mapinfo Spec. <see href="https://regex101.com/r/iWWPub/1">Regex</see></summary>
  [DefinitionExport]
  public static Spec MapInfo { get; } = new()
  {
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    Name = "zdoom.mapinfo",
    FileInferences = [
      IfN(ExtIs, "mapinfo"),
      IfN(ExtIs, "zmapinfo"),
      IfN(FName | Is, "mapinfo"),
      IfN(FName | Is, "zmapinfo")],
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation ("tokens")
    ],
    RxOpt = ROIC | ROEC | ROML,
    IsTextFile = true,
    TokenType = typeof(MT),
    TokenRules = [
      new (RT.Competitive, MT.LangRef, @"""\$\w+"""),
      new (RT.Competitive, MT.String, @""".+?"""),
      new (RT_Comment, MT.None, @"\/\/.*?$"),
      new (RT_Comment, MT.None, @"\/\*[\s\S]*?\*\/"),
      .. TokenRule.MakeWordMatchRules(true,
        MT.Doomednums, MT.AddDefaultMap, MT.GameInfo,
        MT.Skill, MT.Map, MT.DamageType, MT.Episode,
        MT.Cluster, MT.Include, MT.Intermission,
        MT.Cast, MT.Fader, MT.GotoTitle, MT.Image,
        MT.Scroller, MT.TextScreen, MT.Wiper, MT.Cutscene),
      new (RT.TokenMatch, MT.PropertyName, "Background2?|Draw(Conditional)?|Music|Sound|Time|Cast(Class|Name)|AttackSound|FadeType|InitialDelay|Scroll(Direction|Time)|WipeType"),
    ],
    SC = SCOIC,
    TokenCompatLookup = {
      [MT.Value] = [MT.Int, MT.Dec, MT.String, MT.Char, MT.Bool],
      [MT.String] = [MT.LangRef],
      [MT.Dec] = [MT.Int]
    },
  };

  [DefinitionExport]
  public static Spec Decorate => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    IsTextFile = true,
    SC = SCOIC,
    TokenType = typeof(DecorateTokenType),
    Name = "zdoom.decorate",
    Operations = [
      new TokenizeOperation(),
      new TokenAssembleOperation(),
      Op.End
    ]
  };

  internal static RT Compet = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin;
  internal static RT Ignore = RT.TokenMatch | RT.Competitive | RT.ExemptAllWithin | RT.IgnoredToken;
  internal static RT TMatch = RT.TokenMatch | RT.ExemptAllWithin | RT.IgnoreCase;
  internal static RT TExact = RT.TokenExact | RT.ExemptAllWithin | RT.IgnoreCase;
  internal static TokenRule Tm (AT tokenType, [SS("regex")] string regex) => new(TMatch, tokenType, regex);

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
    TokenType = typeof(AT),
    TokenCompatLookup = {
      [AT.ParameterValue] = [AT.ParameterExpression],
      [AT.Value] = [AT.Int, AT.Bool, AT.Char, AT.Str, AT.Fixed, AT.Expression, AT.Name, AT.FunctionCall, AT.ArrayValue, AT.ExpressionStatement],
      [AT.Statement] = [AT.VarDecl, AT.BasicCmd, AT.FunctionCallStatement, AT.VarAssn, AT.VarInc, AT.ArrayDecl, AT.WaitCall, AT.Block, AT.VarDeclAssn],
      [AT.Block] = [AT.IfBlock, AT.ElseBlock, AT.ElseIfBlock]
    },
    TokenRules = [

      // Data
      new(Compet, AT.Str, @"""([^\\""]|\\.)*"""),
      new(Compet, AT.Char, @"'([^\\']|\\.)*'"),
      new(Ignore, AT.None, @"\/\/.*?$"),
      new(Ignore, AT.None, @"\/\*.*?\*\/"),

      // Preprocessor
      Tm(AT.Preprocessor, @"\# \w+"),
      Tm(AT.Bool, @"\b(true|false|on|off|yes|no)\b"),
      Tm(AT.Int, @"-?(?<!\.|\w)(\d+|0x[a-f0-9]+)(?!\.|\w)"),
      Tm(AT.Fixed, @"-?(\d+\.\d*|\.\d+)"),

      // Keywords
      .. TokenRule.MakeWordMatchRules(true, [
        "script", "function", "net", "if", "else",
        "do", "for", "switch", "case", "default", "return"
      ]),
      Tm(AT.ScriptType, @"\b(enter|return|death|lightning|kill|reopen|open|unloading|disconnect|respawn|lightning)\b"),
      Tm(AT.SimpleJump, @"\b(break|continue|terminate|restart)\b"),
      Tm(AT.Wait, @"\b(delay|scriptwait|tagwait)\b"),
      Tm(AT.MapVar, @"\b(world|global)\b"),
      Tm(AT.Loop, @"\b(while|until)\b"),

      // Operators
      Tm(AT.IncDec, @"(\+\+|--)"),
      Tm(AT.Unary, @"!(?!=)|~"),
      Tm(AT.Assign, @"[-+*^/%|&]="),
      Tm(AT.Assign, @"(<<|>>| \|\| |&&)="),
      Tm(AT.Binary, @"== | [!<>]="),
      Tm(AT.Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      .. TokenRule.MakeSingleCharRules("+/%|&^*><", TExact , AT.Binary),
      .. TokenRule.MakeSingleCharRules("[]{}()=,:;-", TExact , new AT[] { AT.Ao, AT.Ac, AT.Bo, AT.Bc, AT.Po, AT.Pc, AT.Eq, AT.Cm, AT.Co, AT.Sc, AT.Minus }),

      Tm(AT.Void, @"\bvoid\b"),
      Tm(AT.Type, @"\b(int|str|char|bool)\b"),
      Tm(AT.Name, @"\b[a-z_][\w]*\b"),
    ],
    GroupTokenRules = [
      // Paremeter Expressions
      new(RT.None, AT.Parameter,                   "t:Type n:Name x:Cm"),
      new(RT.None, AT.FinalParameter,              "t:Type n:Name x:Pc"),
      new(RT.None, AT.PrintParameterValue,         "n:Name{s|i} x:Co pa:Value xo:Cm"),
      new(RT.None, AT.PrintFunction,               "n:name x:Po p:PrintParameterValue x:Pc x:Sc"),
      new(RT.None, AT.ParameterValue,              "d:Value x:Cm"),
      new(RT.Recursive, AT.ParameterExpression,    "l:Value t:Binary r:ParameterValue"),

      // Expressions
      new(RT.None, AT.FunctionCallOpen,            "n:Name x:Po"),
      new(RT.None, AT.ArrayValue,                  "n:Name x:Ao v:Value x:Ac"),
      new(RT.Recursive, AT.Expression,             "l:Value t:Binary r:Value"),
      new(RT.None, AT.ExpressionStatement,         "l:Value t:IncDec"),
      new(RT.None, AT.ExpressionStatement,         "t:IncDec r:Value"),
      new(RT.Recursive, AT.Expression,             "l:Value t:(Binary|Minus) r:Value"),
      new(RT.None, AT.Expression,                  "t:(Unary|Minus) r:Value"),
      new(RT.None, AT.Expression,                  "x:Po v:Value x:Pc"),
      new(RT.None, AT.FunctionCall,                "d:FunctionCallOpen p:Value x:Pc"),
      new(RT.None, AT.FunctionCall,                "d:FunctionCallOpen pa:ParameterValue p:Value x:Pc"),
      new(RT.None, AT.FunctionCall,                "d:FunctionCallOpen x:Pc"),

      // Run Again
      new(RT.None, AT.ArrayValue,                  "n:Name x:Ao v:Value x:Ac"),

      new(RT.None, AT.PreprocessorFull,            "yi:Preprocessor{#Define|#LibDefine} n:Name v:Value"),
      new(RT.None, AT.PreprocessorFull,            "yi:Preprocessor{#Import|#Library|#Include} v:Str"),

      // Statements
      new(RT.None, AT.VarDecl,                     "t:Type n:Name x:Sc"),
      new(RT.None, AT.VarDeclAssn,                 "t:Type n:Name x:Eq v:Value x:Sc"),
      new(RT.None, AT.VarAssn,                     "n:(ArrayValue|Name) x:(Eq|Assign) v:Value x:Sc"),
      new(RT.None, AT.ArrayDecl,                   "t:Type n:ArrayValue x:Sc"),
      //new(RT.None, AT.ArrayDecl,                   "t:Type n:Name x:Ao v:Value x:Ac x:Sc"),
      //new(RT.None, AT.ArrayDecl,                   "t:Type n:Name x:Ao l:Value x:Ac x:Ao r:Value x:Ac x:Sc"),
      new(RT.None, AT.BasicCmd,                    "n:SimpleJump x:Sc"),
      new(RT.None, AT.BasicCmd,                    "n:Return x:Sc"),
      new(RT.None, AT.BasicCmd,                    "n:Return v:value x:Sc"),
      new(RT.None, AT.WaitCall,                    "t:Wait x:Po p:ParameterValue x:Pc x:Sc"),
      new(RT.None, AT.FunctionCallStatement,       "d:FunctionCall x:Sc"),
      new(RT.None, AT.CaseLabel,                   "x:Case n:Value x:Co"),
      new(RT.None, AT.CaseLabel,                   "n:Default x:Co"),

      new(RT.None, AT.IfBlock,                     "t:If v:Expression x:Bo pa:Statement x:Bc"),
      new(RT.None, AT.ElseBlock,                   "t:Else x:Bo pmo:Statement x:Bc"),
      new(RT.None, AT.ElseIfBlock,                 "t:Else x:If v:Expression x:Bo pa:Statement x:Bc"),

      new(RT.None, AT.FunctionHeader,              "x:Function t:(Type|Void) n:Name x:Po x:Void x:Pc"),
      new(RT.None, AT.FunctionHeader,              "x:Function t:(Type|Void) n:Name x:Po pm:Parameter x:Pc"),
      new(RT.None, AT.FunctionFull,                "d:FunctionHeader x:Bo s:(Statement|Block) x:Bc"),

      new(RT.None, AT.ScriptHeader,                "x:Script n:Value t:ScriptType x:Po p:FinalParameter"),

      new(RT.None, AT.ScriptHeader,                "x:Script n:Value t:ScriptType"),
      new(RT.None, AT.ScriptHeader,                "x:Script n:Value x:Po pmo:Parameter p:FinalParameter"),
      new(RT.None, AT.ScriptHeader,                "x:Script n:Value x:Po p:Void x:Pc"),
      new(RT.None, AT.ScriptHeader,                "d:Script n:Value t:Return"),
      new(RT.None, AT.ScriptFull,                  "d:ScriptHeader x:Bo s:(Statement|Block) x:Bc"),
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
      .. TokenRule.MakeSingleCharRules("{}()=,;", TExact ,new MdlT[] { MdlT.Bo, MdlT.Bc, MdlT.Po, MdlT.Pc, MdlT.Eq, MdlT.Cm, MdlT.Sc }),
    ],
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),

    ]
  };
}
