using static Common.Regex.RegexStaticFunctions;

namespace Parser.Text;

public static partial class RX
{
  #region Precompiled Regex
  // Pregenerated Regex
  public const string LnEnd = @"(?:\r\n?|\n)";
  [GeneratedRegex(LnEnd)]
  public static partial Regex LineEnd { get; }

  public const string GpName = @"(?:(?<=\(\?\<)(?<groupname>\w+?)(?=\>\)))";
  [GeneratedRegex(GpName)]
  public static partial Regex GroupName { get; }

  public const string XlRngSplit = @"\s*,(?![^{}]*\})\s*";
  [GeneratedRegex(XlRngSplit)]
  public static partial Regex XlRangeSplitter { get; }
  #endregion

  private static TokenType Mk (string s) => new(s);

  // Common Group Types
  public static Collection<TokenType> CommonIgnores { get; } = [
    Mk("blkcomment") | TF_Ignore,
    Mk("lncomment") | TF_Ignore,
    Mk("ws") | TF_Ignore
  ];

  public static Collection<TokenType> ValueTypes { get; } = [
    "int",
    "dec",
    "str",
    "bool",
    "char",
  ];

  // Named Groups
  public static RxS G_CLnComment => field = Nm("lncomment", CLnComment);
  public static RxS G_CBlkComment => field = Nm("blkcomment", CBlkComment);
  public static RxS G_CPreProc => field = Nm("preproc", CPreProc);
  public static RxS G_WS => field = Nm("ws", WS);
  public static RxS G_Name => field = Nm("name", Name);
  public static RxS G_CString => field = Nm("string", CString);
  public static RxS G_Int => field = Nm("int", Integer);

  // C-Style Common Parts
  public static RxS CLnComment => field = Gp(@"\/\/[^\n\r]*");
  public static RxS CBlkComment => field = Gp(@"\/\*[\s\S]*?\*\/");
  public static RxS CPreProc { get; } = Rx(@"^\s*\#.+?\s*$");
  public static RxS BSEscape { get; } = Rx(@"\\.");
  public static RxS CString { get; } = Rx(@""".*?""") << BSEscape;

  // Other Common Parts
  public static RxS Char { get; } = Rx(@"'[^\\]'|'\\[\\abefnr0]'|'\\u\d\d\d\d'");
  public static RxS Name { get; } = Rx(@"[a-zA-Z_][\w]*");
  public static RxS WS { get; } = Nm("ws", @"\s+");
  public static RxS WSO { get; } = Rx(@"\s*");
  public static RxS Integer { get; } = Rx(@"-?\d+");
  public static RxS Decimal { get; } = Rx(@"-?\d*\.\d+").Orr + Integer;
  public static RxS Boolean { get; } = Rx(@"(?n:\b(false|true)\b)");

  // Modifiers
  public static RxS CaseInsensitive { get; } = Rx(@"(?i)");
  public static RxS CaseSensitive { get; } = Rx(@"(?-i)");
  public static RxS Extended { get; } = Rx(@"(?x)");
  public static RxS NotExtended { get; } = Rx(@"(?-x)");
}
