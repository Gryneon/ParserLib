namespace Common.Regex;
public class MatchData : GroupData,
  ICollection<GroupData>,
  IGeneratable<MatchData, MatchData>,
  IEquatable<MatchData>,
  IEquatable<string>
{

  public override string Name => SE;

  public SortedDictionary<string, GroupData> Groups { get; init; } = [];

  public Dictionary<string, object> Metadata { get; init; } = [];

  public int Type { get; protected set; }
  public override int Count => Groups.Count;
  public bool IsEmpty => Count == 0;

  bool ICollection<GroupData>.IsReadOnly { get; }

  public bool MeetsCondition (string group) => HasGroup(group);

  public MatchData (Match m) : base(m) => Groups = m.ToSortedDictionary();
  public MatchData (Match m, int t) : this(m) => Type = t;
  public MatchData (Match m, IConvertible? val) : this(m) => Type = (int) (val ?? 0);
  public MatchData () { }

  public bool HasGroup (string groupName) => Groups.ContainsKey(groupName);
  public bool DoesNotHaveGroup (string groupName) => !HasGroup(groupName);
  public bool HasGroups (IEnumerable<string> groupNames) => groupNames.All(Groups.ContainsKey);
  public bool HasAnyGroup (IEnumerable<string> groupNames) => groupNames.Any(Groups.ContainsKey);
  public bool HasGroupStartingWith (string namePart) => Groups.Keys.Any(item => item.StartsWith(namePart, SCOIC));
  public new IEnumerator<GroupData> GetEnumerator () => Groups.Values.GetEnumerator();

  public GroupData this[string groupName] => Groups.TryGetValue(groupName, out GroupData? value) ? value : Null;
  public void ThrowIfAbsent (string groupName)
  {
    if (!HasGroup(groupName))
      throw new AbsentGroupException(groupName);
  }
  public void ThrowIfEmpty (string groupName)
  {
    ThrowIfAbsent(groupName);

    if (this[groupName].Content.IsEmpty())
      throw new EmptyGroupException(groupName);
  }
  public override string ToString ()
  {
    const string i1 = "  ";
    const string i2 = "    ";

    const string head = "MatchDataDictionary Object" + Chars.LFs;
    string grps = i1 + "Groups = {";

    if (Count > 0)
    {
      grps += Chars.CRLF;
      foreach (KeyValuePair<string, GroupData> grp in Groups)
      {
        grps += i2 + grp.Key + " = " + grp.Value + Chars.CRLF;
      }
      grps += i1 + "}" + Chars.CRLF;
    }
    else
    {
      grps += " <Empty> }" + Chars.CRLF;
    }

    return head + grps;
  }

  public void Add (GroupData item) => Add(item);
  void ICollection<GroupData>.Clear () => throw new NotSupportedException();
  bool ICollection<GroupData>.Contains (GroupData item) => throw new NotSupportedException();
  void ICollection<GroupData>.CopyTo (GroupData[] array, int arrayIndex) => throw new NotSupportedException();
  bool ICollection<GroupData>.Remove (GroupData item) => throw new NotSupportedException();
  public static MatchData Generate (MatchData input) => input;
  public bool Equals (MatchData? other)
  {
    if (other is null)
    {
      return IsEmpty;
    }

    if (other.Count == Count)
    {
      foreach (GroupData item in other)
      {
        if (!this[item.Name].Content.Equals(item.Content, SCO))
          return false;
      }
      return true;
    }

    return false;
  }

  public override bool Equals (object? obj) => Equals(obj as MatchData);
  public override int GetHashCode () => Content.GetHashCode();
  public bool Equals (string? other) => Content.Equals(other, SCO);
}
