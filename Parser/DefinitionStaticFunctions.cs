using Common.Regex;

namespace Parser;

/// <summary>
/// This is a static class for <see cref="Spec"/> definitions.
/// </summary>
public static class DefinitionStaticFunctions
{
  // Methods
  public static RxS NmOr (string name, Collection<string> options) => RxS.GrpNm(name, Or(options));
  public static RxS Nm (string name, [SS("Regex")] string rx) => RxS.GrpNm(name, rx);
  public static RxS Nm (string common) => RxS.GrpNm(common, common);
  public static RxS Gp ([SS("Regex")] string rx) => RxS.Grp(rx);
  public static RxS Rx ([SS("Regex")] string rx) => rx;
  public static RxS Or ([SS("Regex")] IEnumerable<string> list) => RxS.OOr(list);
  public static RxS Or ([SS("Regex")] string content, [SS("Regex")] params Collection<string> values) => RxS.Or(content, values);
  public static InferenceNode IfN (IT it, string value) => new(it, value);
  public static InferenceNodeOr IfNOr (IEnumerable<IInferenceNode> nodes) => new(nodes);
  public static InferenceNodeAnd IfNAnd (IEnumerable<IInferenceNode> nodes) => new(nodes);
  public static KeyValuePair<string, T> K<T> (string s, T type) where T : IConvertible => new(s, type);

  // Word Start RxS
  public static readonly RxS St = Rx(@"\b");

  // IT Flag Combos
  public static readonly IT
    ExtIs = Ext | Is,
    HeadSt = FileHeader | Start;

  // Constants
  public const IT
    Ext = IT.Ext,
    FName = IT.FName,
    FileHeader = IT.FileHeader,
    FileContent = IT.FileContent,
    FileSize = IT.FileSize,
    And = IT.And,
    ITOr = IT.Or,
    Is = IT.Is,
    End = IT.End,
    Start = IT.Start,
    Contains = IT.Contains,
    Larger = IT.Larger,
    Smaller = IT.Smaller,
    Not = IT.Not;
}
