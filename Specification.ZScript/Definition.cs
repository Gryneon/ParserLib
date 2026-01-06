#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

using static Common.Names;
using static Parser.DefinitionStaticFunctions;
using static Parser.Tokens.Raw.TokenRuleType;

namespace Specification.ZScript;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1

//ZScript Tokenizer
//https://regex101.com/r/dM72bX/1

public enum ZScriptTokenType
{
  None,
  Class,
  Include,
  Num,
  Str,
  Name,
  State,
  Keyword,
  Comment,
  AddFlag,
  SubFlag,
  Property,
  FunctionCall,
  StateCmd,
  Value,
  Array,
  ClassRef
}

[DefinitionExport]
public static class PrevDefinition
{
  /// <summary>
  /// Whitespace Definitions
  /// </summary>
  private static readonly RxS
    _com_Ln = Nm("lncomment", @"\/\/.*"),
    _com_Blk = Nm("blkcomment", @"\/\*[\s\S]*?\*\/"),
    _ws_True = Nm("ws", @"\s+"),
    // Required WS
    _ws = Or(_com_Blk, _com_Ln, _ws_True).Many,
    // Optional WS
    _s = Gp(_ws).Opt,
    _b = Rx(@"\b");

  private static readonly RxS
    _p_name = Nm("name", @"[a-z_]\w*"),
    _p_name_def = Nm("namedef", @"[a-z_]\w*"),
    _p_flag = Nm("flagname", @"[a-z_][\w.]*"),
    _p_prop = Nm("propname", @"[a-z_][\w.]*"),
    _p_expr = Nm("expr", @"[^;]*"),
    //_p_str = Nm("str", @""".*"""),
    //_p_int = RX.G_Int,
    _o_bko = _s + Nm("open", @"\{") + _s,
    _o_bkc = _s + Nm("close", @"\}") + _s,
    _o_eq = _s + Nm("equals", @"\=") + _s,
    _o_col = _s + Nm("colon", @"\:") + _s,
    _o_sc = _s + Nm("semicolon", @"\;") + _s;

  private static readonly RxS
    _n_sta_anynoterm = Nm("stcontent", "[^:;]+"),
    _n_sta_state = Nm("state", _o_col),
    _n_sta_cmd = Nm("cmd", _o_sc),
    _n_sta_item = Nm("stateitem", _n_sta_anynoterm + Or(_n_sta_state, _n_sta_cmd)),
    _n_sta = _b + Nm("statdef", "states") + _o_bko + Or(_n_sta_item, _ws).Any + _o_bkc;

  private static readonly RxS
    _n_def_flagadd = Nm("addflag", @"\+"),
    _n_def_flagrem = Nm("remflag", @"\-"),
    _n_def_flag = Or(_n_def_flagadd, _n_def_flagrem) + _p_flag,
    _n_def_special = Or("monster", "projectile"),
    _n_def_prop = _p_prop + _o_eq + _p_expr,
    _n_def_item = Nm("propitem", Or(_n_def_flag, _n_def_special, _n_def_prop) + _o_sc),
    _n_def = _b + Nm("propdef", "default") + _o_bko + Or(_n_def_item, _ws).Any + _o_bkc;

  private static readonly RxS
    _n_cls_ext = Nm("extend", @"\bextend" + _ws).Opt,
    _n_cls_nm = Nm("classname", _p_name_def),
    _n_cls_prnt = Gp(_o_col + Nm("parent", _p_name)).Opt,
    _n_cls = Nm("entireclass", _n_cls_ext + _n_cls_nm + _n_cls_prnt + _o_bko + Or(_n_def, _n_sta, _ws).Any + _o_bkc);

  /// <summary>
  /// https://regex101.com/r/En5C8c/7
  /// </summary>
  private static RxSCollection Reader { get; } = [
    _n_cls
  ];
  [Export("zdoom.zscript")]
  public static Spec Spec => new()
  {
    FileInferences = [],
    RxOpt = ROML | ROIPW | ROIC | ROEC,
    RegexBasicTokens = [
      "entireclass",
    ],
    WhitespaceTokens = ["ws", "lncomment", "blkcomment"],
    Name = "zdoom.zscript",
    Operations = [
      new SplitOperation(),
      new DictionaryOperation(Reader, ROML | ROIPW | ROIC | ROEC, false, "textparts"),
      new TokenizeOperation<string>(),
      new TokenTemplateOperation([]),
      //TemplateToObjectOperation
      Parser.Ops.Operation.End
    ],
    IsTextFile = true,
    SC = SCOIC,
    TokenRules = [
      //new(TokenExact, )
    ]
  };
}
