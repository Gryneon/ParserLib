#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members

using System.Text.RegularExpressions;

using static Parser.RX;
using static Parser.Tokens.TokenStaticFunctions;

namespace Specification.ACS;
/// <summary>
/// ACS Specification Definition <br/>
/// <see href="https://regex101.com/r/mTwORe/1">Regex</see>
/// https://regex101.com/r/FCoqFI/1
/// </summary>
[DefinitionExport(true)]
public static class Definition
{
  /// <summary>
  /// https://regex101.com/r/NoIMcf/1 <br/>
  /// </summary>
  /// https://regex101.com/r/tfW4Sl/1
  /// <remarks>https://regex101.com/r/deLv0L/2</remarks>

  //Splitter
  //https://regex101.com/r/yR81eF/1
  private static RxS Splitter (string keeps) => Rx($@"\s+|(?<!\s)(?=[{keeps}])|(?<=[{keeps}])");
  private static string Ws => Nm("t_ws", @"\s+");
  private static string Wso => Nm("t_ws", @"\s*");
  private static string ACS_PreProc1 => MarkAs("preproc", @"^\s*\#(library|import|include)\s+");
  private const RegexOptions RxOptions = ROML | ROIC | ROIPW | ROEC;
  /// <summary>
  /// Defined Specification
  /// </summary>

  [Export("zdoom.acs")]
  public static Spec Spec => new()
  {
    FileInferences = [IfN(ExtIs, "acs")],
    Name = "zdoom.acs",
    RxOpt = RxOptions | ROSL,
    Operations = [
      new DictionaryOperation([
        G_CBlkComment,
        G_CLnComment,
        G_WS,
        G_CPreProc,
        G_Int,
        G_Name,
        G_CString,
      ], RxOptions | ROSL)
    ]
  };
  [Export("zdoom.modeldef")]
  public static Spec ModelDef => new()
  {
    FileInferences = [IfNOr(
      IfN(ExtIs, "modeldef"),
      IfN(FName|Is, "modeldef"))],
    Name = "zdoom.modeldef",
    RxOpt = RxOptions,
    Operations = [
      new DictionaryOperation([], RxOptions),
      new DebugToStringOperation("matches"),
      new DebugWaitForInputOperation(),
      new TokenizeOperation(),
      new DebugToStringOperation("tokens"),
      new DebugWaitForInputOperation(),
    ]
  };
}
