//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

/// <summary>
/// Represents a group in a match.
/// </summary>
public class GroupData : CaptureData, IReadOnlyCollection<CaptureData>
{
  public Collection<CaptureData> Captures { get; init; } = [];

  internal GroupData () { }
  public GroupData (Group g) : base(g, g.Name) =>
    Captures = [.. g.Captures.Select(item => new CaptureData(item, g.Name))];

  protected GroupData (string name, string content, int pos, int len) : base(content, pos, len, name) =>
    Captures = [];

  public CaptureData this[int index] => Captures[index];

  public IEnumerator<CaptureData> GetEnumerator () => Captures.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  public bool IsRemoveGroup => Name.StartsWith("rem", SCO) && Content.Length > 0;
  public bool IsNamedGroup => Name.IsNamedGroup() && Content.Length > 0;
  public bool IsNull => Content.Length == 0;
  public virtual int Count => IsNull ? 0 : Captures.Count > 0 ? Captures.Count : 1;
  public static implicit operator GroupData (Group group) => FromGroup(group);
  public static GroupData Null { get; } = new(string.Empty, string.Empty, -1, -1);
  public static GroupData FromGroup (Group group) => group is null ? Null : new(group.Name, group.Value, group.Index, group.Length);

  public override string ToString () =>
    Count > 1 ? "[ " + Captures.Select(item => $"\"{item.Content}\"").TextJoin(", ") + " ]"
    : Count == 1 ? $"\"{Content}\""
    : $"<null data>";
}
