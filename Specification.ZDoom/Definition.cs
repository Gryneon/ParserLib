#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using static Parser.DefinitionStaticFunctions;

namespace Specification.ZDoom;

[DefinitionExport]
public static class Definition
{
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
      new(RT.TokenComment, ZT.None, @"\/\/[^\n]*"),
      new(RT.TokenComment, ZT.None, @"\/\*[\s\S]*?\*\/"),
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
      new(RT.TokenComment, UT.None, Rx(@"//[^\n\r]*")),
      new(RT.TokenComment, UT.None, Rx(@"/\*.*?\*/")),
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
      new(UT.NamespaceDec, "n:Namespace x:Eq v:String x:Sc"),
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
  /// <summary>This is the specification for the SndInfo ZDoom lump.</summary>
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
    TokenRules = [
      new (RT.TokenComment, SndIT.None, @"\/\/.*?$"),
      new (RT.TokenComment, SndIT.None, @"\/\*[\s\S]*?\*\/"),
      new (RT.Competitive, SndIT.String, @""".+?""")
    ],
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
      new (RT.TokenComment, MT.None, @"\/\/.*?$"),
      new (RT.TokenComment, MT.None, @"\/\*[\s\S]*?\*\/"),
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

  internal static TokenRule Tm (AT tokenType, [SS("regex")] string regex) => new(RT.TokenMatch, tokenType, regex);

  /// <summary>Defined Specification</summary>
  /// <remarks><see href="https://regex101.com/r/bNaEDc/1">Regex for Tokens</see></remarks>
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
    DefaultRuleSet = RT.ExemptAllWithin | RT.IgnoreCase,
    TokenCompatLookup = {
      [AT.Value] = [AT.Int, AT.Char, AT.Str, AT.Fixed, AT.Expression, AT.ExprName, AT.FunctionCall, AT.ArrayValue, AT.ExpressionStandalone],
      [AT.Stmt] = [AT.VarDecl, AT.BasicCmd, AT.FunctionCallStmt, AT.VarAssn, AT.VarInc, AT.ArrayDecl, AT.WaitStmt, AT.VarDeclAssn],
      [AT.FuncStmt] = [AT.VarDecl, AT.BasicCmd, AT.FunctionCallStmt, AT.VarAssn, AT.VarInc, AT.ArrayDecl, AT.VarDeclAssn, AT.ReturnStmt],
      [AT.Block] = [AT.IfBlock, AT.ElseBlock, AT.ElseIfBlock, AT.LoopBlock, AT.SwitchBlock],
      [AT.MapVar] = ["global", "world"],
      [AT.Loop] = ["until", "while"],
      [AT.Wait] = ["delay", "tagwait", "scriptwait", "polywait", "NamedScriptWait", AT.ScriptCallWaitStmt],
      [AT.Name] = [AT.PreProcName, AT.ExprName, AT.FuncName, AT.FuncDefName, AT.ArrVarName, AT.VarName, AT.ParamName, AT.DefineName],
      [AT.Literal] = [AT.Int, AT.Str, AT.Char, AT.Fixed]
    },
    TokenRules = [

      // Data
      new(RT.Competitive,   AT.Str,   @"""([^\\""]|\\.)*"""),
      new(RT.Competitive,   AT.Char,  @"'([^\\']|\\.)*'"),
      new(RT.TokenComment,  AT.None,  @"\/\/.*?$"),
      new(RT.TokenComment,  AT.None,  @"\/\*.*?\*\/"),

      // Preprocessor
      Tm(AT.Int, @"-?(?<!\.|\w)(\d+|0x[a-f0-9]+)(?!\.|\w)"),
      Tm(AT.Fixed, @"-?(\d+\.\d*|\.\d+)"),

      // Keywords
      .. TokenRule.MakeWordMatchRules(true, [
        "script", "function", "net", "if", "else",
        "do", "for", "switch", "case", "default", "return",
        "while", "until", "world", "global", "void", "terminate"
      ]),

      // Wait Functions
      .. TokenRule.MakeWordMatchRules(true, [
        "scriptwait", "delay", "polywait", "tagwait", "ACS_ExecuteWait", "namedscriptwait", "ACS_NamedExecuteWait"
      ]),

      // Script Functions
      Tm(AT.ScriptFunc, @"\b(ACS_Execute\w*)\b"),
      Tm(AT.ScriptFunc, @"\b(ACS_NamedExecute\w*)\b"),

      Tm(AT.ScriptType, @"\b(enter|re(turn|open|spawn)|death|kill|open|unloading|disconnect|lightning)\b"),
      Tm(AT.SimpleJump, @"\b(break|continue|terminate|restart)\b"),

      // Operators
      Tm(AT.IncDec, @"(\+\+|--)"),
      Tm(AT.Unary, @"!(?!=)|~"),
      Tm(AT.Assign, @"[-+*^/%|&]="),
      Tm(AT.Assign, @"(<<|>>| \|\| |&&)="),
      Tm(AT.Binary, @"== | [!<>]="),
      Tm(AT.Binary, @"(&&| \|\| |<<|>>)(?!=)"),
      .. TokenRule.MakeSingleCharRules("+/%|&^*><", RT.TokenExact , AT.Binary),
      .. TokenRule.MakeSingleCharRules("[]{}()=,:;#-", RT.TokenExact , new AT[] { AT.Ao, AT.Ac, AT.Bo, AT.Bc, AT.Po, AT.Pc, AT.Eq, AT.Cm, AT.Co, AT.Sc, AT.Pre, AT.Minus }),
      Tm(AT.Type, @"\b(int|str|char|bool)\b"),
      Tm(AT.FuncDefName, @"(?<=function\s*\w+\s*) (?>[a-z_]\w*)"),
      Tm(AT.FuncName, @"(?>[a-z_]\w*) (?=\s*\()"),
      Tm(AT.VarName, @"(?<= (int|str|bool|char) \s+ ( \w+\s*\,\s* )*) (?>[a-z_]\w*) (?!\s*\(|\s*,\s*(int|bool|str|char)) (?=\s*(;|\=|,))"),
      Tm(AT.ArrVarName, @"(?<= (int|str|bool|char) \s+) (?>[a-z_]\w*) (?!\s*\() (?=\s*(\[))"),
      Tm(AT.ParamName, @"(?<= (int|str|bool|char) \s+) (?>[a-z_]\w*) (?!\s*\()"),
      Tm(AT.DefineName, @"(?<= \#\w+\s+) (?>[a-z_]\w*)"),
      Tm(AT.PreProcName, @"(?<= \#) (?>[a-z]+)"),
      Tm(AT.ExprName, @"\b[a-z_]\w*\b"),
    ],
    GroupTokenRules = [
      // Paremeter Expressions
      new(RT.None, AT.ParamDef,                   "t:Type n:ParamName xo:Cm"),
      new(RT.None, AT.PrintParameterValue,        "n:Name{s|i} x:Co pa:Value xo:Cm"),      

      // Expressions (Must be recurrsive!)
      new(RT.Recursive, AT.ArrayDim,               "x:Ao v:Value x:Ac"),
      new(RT.Recursive, AT.ArrayValue,             "n:ExprName vm:ArrayDim"),
      new(RT.Recursive, AT.Expression,             "l:Value c:(Binary|Minus) r:Value"),
      new(RT.Recursive, AT.Expression,             "c:(Unary|Minus) r:Value"),
      new(RT.Recursive, AT.ExpressionStandalone,   "l:Value c:IncDec"),
      new(RT.Recursive, AT.ExpressionStandalone,   "c:IncDec r:Value"),
      new(RT.Recursive, AT.Expression,             "l:Value c:(Binary|Minus) r:Value"),
      new(RT.Recursive, AT.FunctionCall,           "n:FuncName x:Po p:PrintParameterValue x:Pc"),
      new(RT.Recursive, AT.FunctionCall,           "n:FuncName x:Po p:Value x:Pc"),
      new(RT.Recursive, AT.FunctionCall,           "n:FuncName x:Po p:Value xa:Cm pa:Value xa:Cm pa:Value xa:Cm pa:Value xa:Cm pa:Value xa:Cm pa:Value x:Pc"),
      new(RT.Recursive, AT.FunctionCall,           "n:FuncName x:Po x:Pc"),

      new(RT.None, AT.ScriptCallStmt,              "n:FuncName{ACS_Execute.*} x:Po p:Value x:Cm p:Value xa:Cm pa:Value xa:Cm pa:Value xa:Cm pa:Value x:Pc x:Sc"),

      new(RT.None, AT.Preprocessor,                "x:Pre ti:PreProcName{(lib)?define} n:DefineName v:Value"),
      new(RT.None, AT.Preprocessor,                "x:Pre ti:PreProcName{i(mport|nclude)|library} v:Str"),

      // Statements
      new(RT.None, AT.VarDecl,                     "t:Type n:VarName x:Sc"),
      new(RT.None, AT.VarDeclAssn,                 "t:Type n:VarName x:Eq v:Value x:Sc"),
      new(RT.None, AT.VarAssn,                     "n:(ArrayValue|ExprName) x:(Eq|Assign) v:Value x:Sc"),
      new(RT.None, AT.ArrayDecl,                   "t:Type n:ArrVarName vm:ArrayDim x:Sc"),
      new(RT.None, AT.BasicCmd,                    "n:SimpleJump x:Sc"),
      new(RT.None, AT.ReturnStmt,                  "n:Return x:Sc"),
      new(RT.None, AT.ReturnStmt,                  "n:Return v:value x:Sc"),
      new(RT.None, AT.WaitStmt,                    "t:Wait x:Po p:Value x:Pc x:Sc"),
      new(RT.None, AT.FunctionCallStmt,            "d:FunctionCall x:Sc"),
      new(RT.None, AT.CaseLabel,                   "x:Case n:Value x:Co"),
      new(RT.None, AT.CaseLabel,                   "n:Default x:Co"),
      new(RT.None, AT.CaseLabel,                   "n:Default x:Co"),

      new(RT.None, AT.IfBlock,                     "x:If x:Po v:Value x:Pc x:Bo pa:Stmt x:Bc"),
      new(RT.None, AT.IfBlock,                     "x:If x:Po v:Value x:Pc p:Stmt"),
      new(RT.None, AT.LoopBlock,                   "t:Loop x:Po v:Value x:Pc x:Bo pa:Stmt x:Bc"),
      new(RT.None, AT.LoopBlock,                   "t:Loop x:Po v:Value x:Pc p:Stmt"),
      new(RT.None, AT.ElseBlock,                   "x:Else x:Bo pa:Stmt x:Bc"),
      new(RT.None, AT.ElseBlock,                   "x:Else p:Stmt"),
      new(RT.None, AT.ElseIfBlock,                 "x:Else x:If x:Po v:Value x:Pc x:Bo pa:(Stmt|Block) x:Bc"),
      new(RT.None, AT.ElseIfBlock,                 "x:Else x:If x:Po v:Value x:Pc p:(Stmt|Block)"),
      new(RT.None, AT.SwitchBlock,                 "x:Switch x:Po v:Value x:Pc x:Bo sa:(CaseLabel|Stmt|Block) x:Bc"),

      new(RT.None, AT.FunctionHeader,              "x:Function t:(Type|Void) n:FuncDefName x:Po x:Void x:Pc"),
      new(RT.None, AT.FunctionHeader,              "x:Function t:(Type|Void) n:FuncDefName x:Po pm:ParamDef x:Pc"),
      new(RT.None, AT.FunctionFull,                "d:FunctionHeader x:Bo sa:(FuncStmt|Block) x:Bc"),

      new(RT.None, AT.ScriptHeader,                "x:Script n:Value ti:ScriptType{lightning} x:Po p:ParamDef x:Pc"),
      new(RT.None, AT.ScriptHeader,                "x:Script n:Value t:ScriptType"),
      new(RT.None, AT.ScriptHeader,                "x:Script n:Value x:Po pa:ParamDef x:Pc"),
      new(RT.None, AT.ScriptHeader,                "x:Script n:Value x:Po p:Void x:Pc"),
      new(RT.None, AT.ScriptHeader,                "d:Script n:Value t:Return"),
      new(RT.None, AT.ScriptFull,                  "d:ScriptHeader x:Bo sa:(Stmt|Block) x:Bc"),
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
    TokenType = typeof(MdlT),
    GroupTokenRules = [
      //TODO: Start Group Defs
    ],
    DefaultRuleSet = RT.IgnoreCase | RT.ExemptAllWithin,
    TokenRules = [
      .. TokenRule.MakeSingleCharRules("{}()=,;", RT.TokenExact ,new MdlT[] { MdlT.Bo, MdlT.Bc, MdlT.Po, MdlT.Pc, MdlT.Eq, MdlT.Cm, MdlT.Sc }),
    ],
    Operations = [
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),
      new TokenAssembleOperation(),
      new DebugWaitForInputOperation(),
    ]
  };
}
