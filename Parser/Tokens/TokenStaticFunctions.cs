using static Parser.DefinitionStaticFunctions;

namespace Parser.Tokens;

public static class TokenStaticFunctions
{
  /// <summary>Adds a marker to the <see cref="MatchDataSet"/>.</summary>
  /// <param name="name">The marker name to set.</param>
  /// <param name="pattern">The regex pattern to match.</param>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  /// <returns>A regular expression that adds the marker.</returns>
  public static RxS MarkAs (string name, [SS("regex")] string pattern) => Nm($"m_{name}", pattern);
  /// <summary>Optional Whitespace.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS Ws => Nm("t_ws", @"\s*");
  /// <summary>Required Whitespace.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS WsReq => Nm("t_ws", @"\s+");
  /// <summary>An operator or symbol. This accepts a string, but breaks it down and 'or's the characters together.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS Op (string s) => Nm("t_op", s.Select(c => $@"\{c}").TextJoin("|"));
  /// <summary>A property of this token chunk.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS Mp (string property_name, [SS("regex")] string pattern) => Nm($"m_prop_{property_name}", Nm($"t_prop_{property_name}", pattern));
  /// <summary>Adds a key property with the name to be the contents of the match, that pairs with a similarly found value on the specified index.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS Kp (int index, [SS("regex")] string pattern) => Nm($"m_prop_key_{index}", Nm($"t_prop_key_{index}", pattern));
  /// <summary>Adds a value property with the value to be the contents of the match, that pairs with a similarly found key on the specified index.<br/>
  /// Use with <seealso cref="Kp"/></summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  /// 
  public static RxS Vp (int index, [SS("regex")] string pattern) => Nm($"m_prop_value_{index}", Nm($"t_prop_value_{index}", pattern));
  public static RxS En => Nm("t_ws", @".*?$");
  /// <summary>Adds a newline character.</summary>
  /// <remarks><c>Tokenized</c> - This will generate tokens.</remarks>
  public static RxS Ln { get; } = Nm("t_ln", @"\r?\n|\r");
  /// <summary>Marks the end of a line.</summary>
  /// <remarks><c>Not Tokenized</c> - This will not generate tokens.</remarks>
  public static RxS LnEnd { get; } = "$";
  public static RxS LazyAll => Rx(@".*?");
  public static RxS LazyOneLn => Rx(@"[^\n\r\v\f]*?");
  public static RxS DInt => Nm("t_int", @"-?\d+.?(?![0-9])");
  public static RxS DBool => Nm("t_bool", @"\b(true|false)\b");
  public static RxS DDec => Nm("t_dec", @"-?\d+.?(?![0-9])");
  public static RxS DNull => Nm("t_null", "null");
  public static RxS Name => Nm("t_name", @"\b\w+\b");
  public static RxS DString => Nm("t_string", @"""([^\\]|\\.)+""");
  public static RxS Value => Or(DString, DInt, DBool, DDec, DNull);
  public static RxS LnComment ([SS("regex")] string prefix) => Nm("t_lncomment", prefix + LazyOneLn + LnEnd);
  public static RxS BlkComment ([SS("regex")] string prefix, [SS("regex")] string suffix) => Nm("t_blkcomment", prefix + LazyAll + suffix);
}
