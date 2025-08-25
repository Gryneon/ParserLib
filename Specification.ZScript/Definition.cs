using Parser.Ops;
using Parser.Text.Ops;

using static Parser.DefinitionStaticFunctions;
using static Parser.Text.Tokens.TokenFlags;

namespace Specification.ZScript;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1

//ZScript Tokenizer
//https://regex101.com/r/dM72bX/1

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class PrevDefinition
{
  /// <summary>
  /// Whitespace Definitions
  /// </summary>
  protected static readonly RxS
    Com_Ln = Nm("lncomment", @"\/\/.*"),
    Com_Blk = Nm("blkcomment", @"\/\*[\s\S]*?\*\/"),
    Ws_True = Nm("ws", @"\s+"),
    // Required WS
    _ws = Or(Com_Blk, Com_Ln, Ws_True).Many,
    // Optional WS
    _s = Gp(_ws).Opt,
    _b = Rx(@"\b");

  protected static readonly RxS
    _p_name = Nm("name", @"[a-z_]\w*"),
    _p_name_def = Nm("namedef", @"[a-z_]\w*"),
    _p_flag = Nm("flagname", @"[a-z_][\w.]*"),
    _p_prop = Nm("propname", @"[a-z_][\w.]*"),
    _p_expr = Nm("expr", @"[^;]*"),
    _p_str = Nm("str", @""".*"""),
    _p_int = RX.G_Int,
    _o_bko = _s + Nm("open", @"\{") + _s,
    _o_bkc = _s + Nm("close", @"\}") + _s,
    _o_eq = _s + Nm("equals", @"\=") + _s,
    _o_col = _s + Nm("colon", @"\:") + _s,
    _o_sc = _s + Nm("semicolon", @"\;") + _s;

  protected static readonly RxS
    _n_sta_anynoterm = Nm("stcontent", "[^:;]+"),
    _n_sta_state = Nm("state", _o_col),
    _n_sta_cmd = Nm("cmd", _o_sc),
    _n_sta_item = Nm("stateitem", _n_sta_anynoterm + Or(_n_sta_state, _n_sta_cmd)),
    _n_sta = _b + Nm("statdef", "states") + _o_bko + Or(_n_sta_item, _ws).Any + _o_bkc;

  protected static readonly RxS
    _n_def_flagadd = Nm("addflag", @"\+"),
    _n_def_flagrem = Nm("remflag", @"\-"),
    _n_def_flag = Or(_n_def_flagadd, _n_def_flagrem) + _p_flag,
    _n_def_special = Or("monster", "projectile"),
    _n_def_prop = _p_prop + _o_eq + _p_expr,
    _n_def_item = Nm("propitem", Or(_n_def_flag, _n_def_special, _n_def_prop) + _o_sc),
    _n_def = _b + Nm("propdef", "default") + _o_bko + Or(_n_def_item, _ws).Any + _o_bkc;

  protected static readonly RxS
    _n_cls_ext = Nm("extend", @"\bextend" + _ws).Opt,
    _n_cls_nm = Nm("classname", _p_name_def),
    _n_cls_prnt = Gp(_o_col + Nm("parent", _p_name)).Opt,
    _n_cls = Nm("entireclass", _n_cls_ext + _n_cls_nm + _n_cls_prnt + _o_bko + Or(_n_def, _n_sta, _ws).Any + _o_bkc);

  /// <summary>
  /// https://regex101.com/r/En5C8c/7
  /// </summary>
  protected static RxSList Reader { get; } = [
    _n_cls
  ];
  public static TextSpec Spec => new()
  {
    FileInferences = [],
    CaseInsensitive = true,
    TokenLookup = [
      Mt("lncomment") | TF_Ignore,
      Mt("blkcomment") | TF_Ignore,
      Mt("ws") | TF_Ignore,
      "name",
      "parent",
      "classname",
      "open",
      "close",
      "propdef",
      "statdef",
      "extend",
      "native",
      "colon",
      "semicolon",
      "entireclass",
    ],
    Name = "zscript",
    Operations = [
      new DictionaryOperation(Reader),
      Operation.End
    ]
  };
}
