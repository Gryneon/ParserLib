#pragma warning disable IDE1006 // Naming Styles

namespace Parser;

public static class RXPT
{
  public static readonly RxS b = @"\b";
  public static readonly RxS s = @"\s";
  public static readonly RxS sa = @"\s*";
  public static readonly RxS sp = @"\s+";
  public static readonly RxS wp = @"\w+";

  public static readonly RxS st = @"^";
  public static readonly RxS en = @"$";
}
