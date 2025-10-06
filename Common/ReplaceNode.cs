
using System.Diagnostics.CodeAnalysis;

using Common.Regex;

using SysRegex = System.Text.RegularExpressions.Regex;

namespace Common;
/// <summary>
/// A node that defines a string replacement operation.
/// </summary>
public class ReplaceNode : IEquatable<ReplaceNode>
{
  #region Static Members
  /// <summary>
  /// Creates a <see cref="ReplaceNode"/> from the specified parameters.
  /// </summary>
  /// <param name="lf">The string to look for.</param>
  /// <param name="rw">The string to replace with</param>
  /// <returns>A new <see cref="ReplaceNode"/> object.</returns>
  public static ReplaceNode From (string lf, string? rw) => new(lf, rw);
  public static ReplaceNode From ((string, string?) tp) => new(tp.Item1, tp.Item2);
  public static ReplaceNode From (KeyValuePair<string, string?> kvp) => new(kvp.Key, kvp.Value);
  public static ReplaceNode From (IProperty<string?> prop)
  {
    prop.ThrowIfNull();
    return new(prop.Key, prop.Value);
  }

  public static implicit operator ReplaceNode ((string, string?) tuple) => From(tuple);
  public static implicit operator ReplaceNode (KeyValuePair<string, string?> kvp) => From(kvp);
  public static implicit operator ReplaceNode (Tuple<string, string?> prop) => From(prop.ToValueTuple());
  public static implicit operator KeyValuePair<string, string?> ([NotNull] ReplaceNode node) => node.ToKVP();
  #endregion
  /// <summary>
  /// The regular expression to look for.
  /// </summary>
  public RxS LookFor { get; init; }
  /// <summary>
  /// The string to replace matches with. If <see langword="null"/>, matches will be removed.
  /// </summary>
  public string? ReplaceWith { get; init; }
  /// <summary>
  /// An empty node.
  /// </summary>
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
  /// <summary>
  /// Checks if the 2 nodes are equal to each other.
  /// </summary>
  /// <param name="other">The other <see cref="ReplaceNode"/>.</param>
  /// <returns><see langword="true"/> if the <see cref="ReplaceNode"/> objects are equal to each other, otherwise <see langword="false"/>.</returns>
  public bool Equals (ReplaceNode? other) =>
    LookFor == other?.LookFor && ReplaceWith == other.ReplaceWith;

  public bool Equals (ReplaceNode? other, StringComparison sc) =>
    LookFor.Content.Equals(other?.LookFor, sc) && (ReplaceWith is not null && other.ReplaceWith is not null && ReplaceWith.Equals(other.ReplaceWith, sc) || ReplaceWith is null && other.ReplaceWith is null);

  /// <inheritdoc/>
  public override bool Equals (object? obj) => Equals(obj as ReplaceNode, SCO);
  /// <inheritdoc/>
  public override int GetHashCode () => HashCode.Combine(LookFor, ReplaceWith);

  public virtual SysRegex OpRegex => new(LookFor);

  public string ReplaceRegex (string input) => OpRegex.Replace(input, ReplaceWith ?? SE);
  public string ReplaceText (string input, StringComparison sc = SCO) => input.RecursiveReplace(LookFor, ReplaceWith ?? SE, sc);
}
