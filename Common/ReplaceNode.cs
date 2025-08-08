
using Common.Regex;

using SysRegex = System.Text.RegularExpressions.Regex;

namespace Common;
public class ReplaceNode : IEquatable<ReplaceNode>
{
  #region Static Members
  public static ReplaceNode From (string lf, string? rw) => new(lf, rw);
  public static ReplaceNode From ((string, string?) tp) => new(tp.Item1, tp.Item2);
  public static ReplaceNode From (KeyValuePair<string, string?> kvp) => new(kvp.Key, kvp.Value);
  public static ReplaceNode From (IProperty<string?> prop) => new(prop.Key, prop.Value);

  public static implicit operator ReplaceNode ((string, string?) tuple) => From(tuple);
  public static implicit operator ReplaceNode (KeyValuePair<string, string?> kvp) => From(kvp);
  public static implicit operator ReplaceNode (Tuple<string, string?> prop) => From(prop.ToValueTuple());
  public static implicit operator KeyValuePair<string, string?> (ReplaceNode node) => node.ToKVP();
  #endregion

  public bool IsValid => ReplaceWith is not null;
  public RxS LookFor { get; init; }
  public string? ReplaceWith { get; init; }

  protected ReplaceNode ()
  {
    LookFor = SE;
    ReplaceWith = null;
  }
  public ReplaceNode ([SS("Regex")] string lookFor, string? replaceWith)
  {
    LookFor = lookFor;
    ReplaceWith = replaceWith;
  }
  public ReplaceNode ((string LookFor, string? ReplaceWith) tuple)
  {
    LookFor = tuple.LookFor;
    ReplaceWith = tuple.ReplaceWith;
  }
  public ReplaceNode (KeyValuePair<string, string?> kvp)
  {
    LookFor = kvp.Key;
    ReplaceWith = kvp.Value;
  }

  public KeyValuePair<string, string?> ToKVP () => new(LookFor, ReplaceWith);

  public bool Equals (ReplaceNode? other) =>
    LookFor == other?.LookFor && ReplaceWith == other.ReplaceWith;

  public bool Equals (ReplaceNode? other, StringComparison sc) =>
    LookFor.Content.Equals(other?.LookFor, sc) && ReplaceWith == other.ReplaceWith;

  public override bool Equals (object? obj) => Equals(obj as ReplaceNode);
  public override int GetHashCode () => HashCode.Combine(LookFor, ReplaceWith);

  public virtual SysRegex OpRegex => new(LookFor);

  public string ReplaceRegex (string input) => OpRegex.Replace(input, ReplaceWith ?? SE);
  public string ReplaceText (string input, StringComparison sc = SCO) => input.RecursiveReplace(LookFor, ReplaceWith ?? SE, sc);
}