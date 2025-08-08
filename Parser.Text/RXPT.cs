using static Common.Regex.RegexStaticFunctions;

namespace Parser.Text;

public static class RXPT
{
  public static readonly RxS b = @"\b";
  public static readonly RxS s = @"\s";
  public static readonly RxS sa = @"\s*";
  public static readonly RxS sp = @"\s+";
  public static readonly RxS wp = @"\w+";

  public static readonly RxS st = @"^";
  public static readonly RxS en = @"$";
  public static RxS paren (RxS inner) => Nm("open", @"\(") + inner + Nm("close", @"\)");
  public static RxS brack (RxS inner) => Nm("open", @"\{") + inner + Nm("close", @"\}");
  public static RxS brace (RxS inner) => Nm("open", @"\[") + inner + Nm("close", @"\]");
}
