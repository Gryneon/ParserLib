//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

/// <summary>
/// Represents a group in a match.
/// </summary>
public class GroupData : CaptureData, IReadOnlyCollection<CaptureData>, IEquatable<GroupData>
{
  public Collection<CaptureData> Captures { get; init; } = [];

  internal GroupData () { }
  /// <summary>
  /// Creates a new <see cref="GroupData"/> from a <see cref="Group"/>.
  /// </summary>
  /// <param name="g">The <see cref="Group"/> to base this object on.</param>
  /// <param name="index">The group number of this group.</param>
  public GroupData (Group? g, int index = -1) : base(g, g is null ? SE : g.Name, index) =>
    Captures = g is null ? [] : [.. g.Captures.Select((item, i) => new CaptureData(item, g.Name, i))];
  /// <summary>
  /// Manual Constructor.
  /// </summary>
  /// <param name="name"></param>
  /// <param name="content"></param>
  /// <param name="pos"></param>
  /// <param name="len"></param>
  /// <param name="index"></param>
  protected GroupData (string name, string content, int pos, int len, int index = -1) : base(content, pos, len, name, index) =>
    Captures = [];

  public CaptureData this[int index] => Captures[index];
  /// <inheritdoc/>
  public IEnumerator<CaptureData> GetEnumerator () => Captures.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  public bool IsRemoveGroup => Name.StartsWith("rem", SCO) && Content.Length > 0;
  public bool IsNamedGroup => Name.IsNamedGroup() && Content.Length > 0;
  public override bool IsNull => Content.Length == 0 || Captures.Count == 0;
  public virtual int Count => IsNull ? 0 : Captures.Count > 0 ? Captures.Count : 1;
  public static implicit operator GroupData (Group group) => FromGroup(group);
  public static GroupData Null { get; } = new(string.Empty, string.Empty, -1, -1);
  public static GroupData FromGroup (Group group) => group is null ? Null : new(group.Name, group.Value, group.Index, group.Length);

  public override string ToString () =>
    Count > 1 ? "[ " + Captures.Select(item => $"\"{item.Content}\"").TextJoin(", ") + " ]"
    : Count == 1 ? $"\"{Content}\""
    : $"<null data>";
  /// <inheritdoc/>
  public bool Equals (GroupData? other) => other is null || other.IsNull ? IsNull : other.Content.Equals(Content, SCO);

  /// <inheritdoc/>
  public override bool Equals (object? obj) => Equals(obj as GroupData);

  /// <inheritdoc/>
  public override int GetHashCode () => HashCode.Combine(Name, Content, Pos, Len);
}
