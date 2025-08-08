//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using static Common.Regex.RxS;

namespace Common.Regex;
public static class RegexStaticFunctions
{
  public static RxS Rx ([SS("Regex")] string regex) => RxS.Rx(regex);
  public static RxS Nm (string name, [SS("Regex")] string regex) => GrpNm(name, regex);
  public static RxS Gp ([SS("Regex")] string regex) => Grp(regex);

  public static RxS DQt ([SS("Regex")] string regex) => $"\"{regex}\"";
  public static RxS SQt ([SS("Regex")] string regex) => $"\'{regex}\'";

  public static RxS Bk => @"\b";
  public static RxS Start => @"^";
}
