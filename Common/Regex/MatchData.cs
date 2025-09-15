namespace Common.Regex;
/// <summary>
/// A collection of groups and their captures from a regex match. This object includes additional functions and properties to make working with regex matches easier.
/// </summary>
public sealed class MatchData : GroupData,
  ICollection<GroupData>,
  IGeneratable<MatchData, MatchData>,
  IEquatable<MatchData>,
  IEquatable<string>
{
  /// <inheritdoc/>
  public override int Index { get; init; } = -1;
  /// <summary>
  /// The group name, unused in this class.
  /// </summary>
  public override string Name => SE;
  /// <summary>
  /// The capture groups.
  /// </summary>
  public SortedDictionary<string, GroupData> Groups { get; init; } = [];
  /// <summary>
  /// The number of groups in the match.
  /// </summary>
  public override int Count => Groups.Count;
  /// <summary>
  /// Gets a value indicating whether the match has no groups.
  /// </summary>
  public override bool IsNull => Count == 0;
  /// <summary>
  /// The original <see cref="Match"/> object from which this <see cref="MatchData"/> was created.
  /// </summary>
  public Match? Origin { get; private set; }
  bool ICollection<GroupData>.IsReadOnly { get; }
  /// <summary>
  /// Represents data extracted from a regular expression match, including its groups,  and provides additional
  /// functionality for working with the match.
  /// </summary>
  /// <remarks>The <see cref="Groups"/> property is initialized by converting the groups in the provided  <see
  /// cref="Match"/> object into a sorted dictionary for easier access and manipulation.</remarks>
  /// <param name="m">The <see cref="Match"/> object containing the results of a regular expression match.  Cannot be <see
  /// langword="null"/>.</param>
  /// <param name="i"></param>
  public MatchData (Match? m, int i = -1) : base(m)
  {
    Groups = m is null ? [] : m.ToSortedDictionary();
    Origin = m;
    Index = i;
  }

  /// <summary>
  /// Empty constructor for serialization purposes only.
  /// </summary>
  public MatchData () { }
  /// <summary>
  /// Checks if the specified group exists in the match.
  /// </summary>
  /// <param name="groupName">The name of the group.</param>
  /// <returns>
  /// <see langword="true"/> if the group exists; otherwise, <see langword="false"/>.
  /// </returns>
  public bool HasGroup (string groupName) => Groups.ContainsKey(groupName);
  /// <summary>
  /// Determines whether the specified group does not exist.
  /// </summary>
  /// <param name="groupName">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the specified group does not exist; otherwise, <see langword="false"/>.</returns>
  public bool DoesNotHaveGroup (string groupName) => !HasGroup(groupName);
  /// <summary>
  /// Determines whether the specified group exists and contains no content.
  /// </summary>
  /// <param name="groupName">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the group exists and its content is empty; otherwise, <see langword="false"/>.</returns>
  public bool HasEmptyGroup (string groupName) => HasGroup(groupName) && Groups[groupName].Content.IsEmpty();
  /// <summary>
  /// Determines whether the specified group does not exist or exists but is empty.
  /// </summary>
  /// <param name="groupName">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the group does not exist or exists but is empty; otherwise, <see langword="false"/>.</returns>
  public bool HasEmptyOrNoGroup (string groupName) => !HasGroup(groupName) || HasEmptyGroup(groupName);
  /// <summary>
  /// Determines whether all specified group names exist in the current collection of groups.
  /// </summary>
  /// <param name="groupNames">A collection of group names to check for existence.</param>
  /// <returns><see langword="true"/> if all specified group names exist in the collection; otherwise, <see langword="false"/>.</returns>
  public bool HasGroups (IEnumerable<string> groupNames) => groupNames.All(Groups.ContainsKey);
  /// <summary>
  /// Determines whether the current instance contains any of the specified group names.
  /// </summary>
  /// <param name="groupNames">A collection of group names to check for existence.</param>
  /// <returns><see langword="true"/> if at least one of the specified group names exists in the current instance;  otherwise,
  /// <see langword="false"/>.</returns>
  public bool HasAnyGroup (IEnumerable<string> groupNames) => groupNames.Any(Groups.ContainsKey);
  /// <summary>
  /// Determines whether any group name starts with the specified prefix.
  /// </summary>
  /// <remarks>This method performs a case-insensitive comparison when checking for group names that start with
  /// the specified prefix.</remarks>
  /// <param name="namePart">The prefix to search for in group names. This value is case-insensitive.</param>
  /// <returns><see langword="true"/> if at least one group name starts with the specified prefix; otherwise, <see
  /// langword="false"/>.</returns>
  public bool HasGroupStartingWith (string namePart) => Groups.Keys.Any(item => item.StartsWith(namePart, SCOIC));
  /// <inheritdoc/>
  public new IEnumerator<GroupData> GetEnumerator () => Groups.Values.GetEnumerator();
  /// <summary>
  /// Gets the GroupData associated with the specified group name.
  /// </summary>
  /// <param name="groupName">The group to look up.</param>
  /// <returns></returns>
  public GroupData this[string groupName] => Groups.TryGetValue(groupName, out GroupData? value) ? value : Null;
  /// <summary>
  /// Throws an <see cref="AbsentGroupException"/> if the specified group does not exist.
  /// </summary>
  /// <param name="groupName">The name of the group to check for.</param>
  /// <exception cref="AbsentGroupException"></exception>
  public void ThrowIfAbsent (string groupName)
  {
    if (!HasGroup(groupName))
      throw new AbsentGroupException(groupName);
  }
  /// <summary>
  /// Throws an <see cref="EmptyGroupException"/> if the specified group exists but has a length of 0,
  /// or an <see cref="AbsentGroupException"/> if the specified group does not exist.
  /// </summary>
  /// <param name="groupName">The name of the group to check for.</param>
  /// <exception cref="EmptyGroupException"></exception>
  /// <exception cref="AbsentGroupException"></exception>"
  public void ThrowIfEmpty (string groupName)
  {
    ThrowIfAbsent(groupName);

    if (this[groupName].Content.IsEmpty())
      throw new EmptyGroupException(groupName);
  }
  /// <summary>
  /// Returns a string representation of the current <see cref="MatchData"/> object.
  /// </summary>
  /// <param name="indent">A string used for indentation in the output. Defaults to an empty string if not provided.</param>
  /// <remarks>The returned string includes a header and a detailed list of all groups in the match. If the
  /// match is empty, the string will indicate that no groups are present.</remarks>
  /// <returns>A string that represents the current <see cref="MatchData"/> object, including its groups and their
  /// values.</returns>
  internal string ToString (string indent)
  {
    const string i1 = "  ";
    const string i2 = "    ";

    string head = indent + $"{typeof(MatchData)} Object" + Chars.LFs;
    string grps = indent + i1 + "Groups = {";

    if (Count > 0)
    {
      grps += Chars.CRLF;
      foreach (KeyValuePair<string, GroupData> grp in Groups)
      {
        grps += indent + i2 + grp.Key + " = " + grp.Value + Chars.CRLF;
      }
      grps += indent + i1 + "}" + Chars.CRLF;
    }
    else
    {
      grps += " <Empty> }" + Chars.CRLF;
    }

    return head + grps;
  }
  /// <summary>
  /// Returns a string representation of the current <see cref="MatchData"/> object.
  /// </summary>
  /// <remarks>The returned string includes a header and a detailed list of all groups in the match. If the
  /// match is empty, the string will indicate that no groups are present.</remarks>
  /// <returns>A string that represents the current <see cref="MatchData"/> object, including its groups and their
  /// values.</returns>
  public override string ToString () => ToString(SE);
  /// <summary>
  /// Adds the specified <see cref="GroupData"/> item to the collection.
  /// </summary>
  /// <param name="item">The <see cref="GroupData"/> item to add to the collection. Cannot be null.</param>
  public void Add (GroupData item) => Add(item);
  void ICollection<GroupData>.Clear () => throw new NotSupportedException();
  bool ICollection<GroupData>.Contains (GroupData item) => throw new NotSupportedException();
  void ICollection<GroupData>.CopyTo (GroupData[] array, int arrayIndex) => throw new NotSupportedException();
  bool ICollection<GroupData>.Remove (GroupData item) => throw new NotSupportedException();
  /// <summary>Creates a copy of the object.</summary>
  /// <inheritdoc/>
  public static MatchData Generate (MatchData input) => new(input.Origin);
  /// <inheritdoc/>
  public bool Equals (MatchData? other)
  {
    if (other is null || other.IsNull)
    {
      return IsNull;
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

  /// <inheritdoc/>
  public override bool Equals (object? obj) => Equals(obj as MatchData);
  /// <inheritdoc/>
  public override int GetHashCode () => Content.GetHashCode();
  /// <inheritdoc/>
  public bool Equals (string? other) => Content.Equals(other, SCO);
}
