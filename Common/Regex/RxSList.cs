//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

public readonly struct RxSList : ICollection<RxS>
{
  private readonly List<RxS> _list;
  public readonly int Count => _list.Count;
  public readonly RxS Combined => RxS.Grp(_list.TextJoin("|"));

  bool ICollection<RxS>.IsReadOnly => false;

  public RxSList (IEnumerable<RxS> list) => _list = [.. list];
  public RxSList (RxS single) => _list = [single];
  public RxSList () => _list = [];

  public static implicit operator Collection<RxS> (RxSList from) =>
    from.Count != 0 ? [.. from._list] : [];
  public static implicit operator RxS (RxSList from) =>
    from.Count != 0 ? from.Combined : SE;
  public static implicit operator string (RxSList from) =>
    from.Count != 0 ? from.Combined : SE;
  public static implicit operator System.Text.RegularExpressions.Regex (RxSList from) =>
    from.Count != 0 ? new(from.Combined) : throw new ArgumentNullException(nameof(from));
  public static implicit operator RxSList (RxS from) =>
    [from];
  public static implicit operator RxSList (string from) =>
    [from];
  public static implicit operator RxSList (Collection<RxS> from) =>
    [.. from];

  public readonly IEnumerator<RxS> GetEnumerator () => _list.GetEnumerator();
  readonly IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  public void Add (RxS item) => _list.Add(item);
  public void Clear () => _list.Clear();
  public override string ToString () => Combined;
  public override int GetHashCode () => Combined.GetHashCode();
  public override bool Equals (object? other) => Combined.Equals(other?.ToString());
  bool ICollection<RxS>.Contains (RxS item) => throw new NotImplementedException();
  void ICollection<RxS>.CopyTo (RxS[] array, int arrayIndex) => throw new NotImplementedException();
  bool ICollection<RxS>.Remove (RxS item) => throw new NotImplementedException();
  public static bool operator == (RxSList left, string right) => left.Equals(right);
  public static bool operator != (RxSList left, string right) => !(left == right);
}