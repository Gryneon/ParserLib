namespace Common.Regex;
/// <summary>
/// A collection of groups and their captures from a regex match. This object includes additional functions and properties to make working with regex matches easier.</summary>
public sealed class MatchDataSet : GroupDataSet,
  ICollection<GroupDataSet>,
  IGeneratable,
  IEquatable<MatchDataSet>,
  IEquatable<string>
{
  /// <inheritdoc/>
  public override int Index { get; init; } = -1;
  /// <summary>
  /// The group name, unused in this class.</summary>
  public override string Name => SE;
  /// <summary>
  /// The capture groups.</summary>
  public SortedDictionary<string, GroupDataSet> Groups { get; init; } = [];
  /// <summary>
  /// The number of groups in the match.</summary>
  public override int Count => Groups.Count;
  /// <summary>
  /// Gets a value indicating whether the match has no groups.</summary>
  public override bool IsNull => Count == 0;
  /// <summary>
  /// The original <see cref="Match"/> object from which this <see cref="MatchDataSet"/> was created.</summary>
  public Match? Origin { get; }
  bool ICollection<GroupDataSet>.IsReadOnly { get; }
  /// <summary>
  /// Represents data extracted from a regular expression match, including its groups,  and provides additional
  /// functionality for working with the match.
  /// </summary>
  /// <remarks>The <see cref="Groups"/> property is initialized by converting the groups in the provided
  /// <see cref="Match"/> object into a sorted dictionary for easier access and manipulation.</remarks>
  /// <param name="m">The <see cref="Match"/> object containing the results of a regular expression match.  Cannot be <see
  /// langword="null"/>.</param>
  /// <param name="i"></param>
  public MatchDataSet (Match? m, int i = -1) : base(m)
  {
    Groups = m is null ? [] : m.ToSortedDictionary();
    Origin = m;
    Index = i;
  }

  public MatchDataSet (ICollection<GroupDataSet> groups)
  {
    Groups = [.. groups.Select(g => g.ToKVP())];
    Origin = default;
    Index = -1;
  }

  /// <summary>
  /// Empty constructor for serialization purposes only.</summary>
  public MatchDataSet () { }
  public bool UsesGroupDefinitions => HasGroupStartingWithAny("m_", "t_");
  /// <summary>Determines if a <see cref="MatchDataSet"/> has a certain marker.</summary>
  /// <param name="markerName">The marker <b><i>without</i></b> the prefix. Use <see langword="null"/> to </param>
  /// <returns><see langword="true"/> if the marker is present, <see langword="false"/> otherwise.</returns>
  public bool HasMarker (string? markerName) => markerName is null ? HasGroupStartingWith("m_") : HasGroup($"m_{markerName}");
  public bool HasProperty (string propName) => HasGroup($"m_prop_{propName}");
  public bool HasListProperty (string propName) => HasGroup($"m_prop_list_{propName}");
  public bool HasKVProperty (int index) => HasGroup($"m_prop_key_{index}") && HasGroup($"m_prop_value_{index}");
  /// <summary>Checks if the specified group exists in the match.</summary>
  /// <param name="group">The name of the group.</param>
  /// <returns><see langword="true"/> if the group exists; otherwise, <see langword="false"/>.</returns>
  public bool HasGroup (string group) => Groups.Keys.Any(item => item.Like(group));
  /// <summary>
  /// Determines whether the specified group does not exist.</summary>
  /// <param name="group">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the specified group does not exist; otherwise, <see langword="false"/>.</returns>
  public bool DoesNotHaveGroup (string group) => !HasGroup(group);
  /// <summary>
  /// Determines whether the specified group exists and contains no content.</summary>
  /// <param name="group">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the group exists and its content is empty; otherwise, <see langword="false"/>.</returns>
  public bool HasEmptyGroup (string group) => HasGroup(group) && Groups[group].Content.IsEmpty();
  /// <summary>
  /// Determines whether the specified group does not exist or exists but is empty.</summary>
  /// <param name="group">The name of the group to check. Cannot be null or empty.</param>
  /// <returns><see langword="true"/> if the group does not exist or exists but is empty; otherwise, <see langword="false"/>.</returns>
  public bool HasEmptyOrNoGroup (string group) => !HasGroup(group) || HasEmptyGroup(group);
  /// <summary>Determines whether all specified group names exist in the current collection of groups.</summary>
  /// <param name="groups">A collection of group names to check for existence.</param>
  /// <returns><see langword="true"/> if all specified group names exist in the collection; otherwise, <see langword="false"/>.</returns>
  public bool HasGroups (IEnumerable<string> groups) => groups.All(HasGroup);
  public bool HasAnyGroup () => Groups.Any(item => item.Key != "0");
  /// <summary>Determines whether the current instance contains any of the specified group names.</summary>
  /// <param name="groups">A collection of group names to check for existence.</param>
  /// <returns><see langword="true"/> if at least one of the specified group names exists in the current instance;  otherwise,
  /// <see langword="false"/>.</returns>
  public bool HasAnyGroup (IEnumerable<string> groups) => groups.Any(HasGroup);
  /// <summary>Determines whether any group name starts with the specified prefix.</summary>
  /// <remarks>This method performs a case-insensitive comparison when checking for group names that start with
  /// the specified prefix.</remarks>
  /// <param name="namePart">The prefix to search for in group names. This value is case-insensitive.</param>
  /// <returns><see langword="true"/> if at least one group name starts with the specified prefix; otherwise, <see
  /// langword="false"/>.</returns>
  public bool HasGroupStartingWith (string namePart) => Groups.Keys.Any(item => item.StartsWith(namePart, SCOIC));
  public bool HasGroupStartingWithAny (params Collection<string> nameParts) => Groups.Keys.Any(item => item.StartsWithAny(nameParts, SCOIC));
  /// <inheritdoc/>
  public new IEnumerator<GroupDataSet> GetEnumerator () => Groups.Values.GetEnumerator();
  /// <summary>
  /// Gets the GroupData associated with the specified group name.</summary>
  /// <param name="groupName">The group to look up.</param>
  /// <returns></returns>
  public GroupDataSet this[string groupName] => Groups.TryGetValue(groupName, out GroupDataSet? value) ? value : Null;
  /// <summary>
  /// Throws an <see cref="AbsentGroupException"/> if the specified group does not exist.</summary>
  /// <param name="groupName">The name of the group to check for.</param>
  /// <exception cref="AbsentGroupException"></exception>
  public void ThrowIfAbsent (string groupName)
  {
    if (!HasGroup(groupName))
      throw new AbsentGroupException(groupName);
  }
  /// <summary>Throws an <see cref="EmptyGroupException"/> if the specified group exists but has a length of 0,
  /// or an <see cref="AbsentGroupException"/> if the specified group does not exist.</summary>
  /// <param name="groupName">The name of the group to check for.</param>
  /// <exception cref="EmptyGroupException"></exception>
  /// <exception cref="AbsentGroupException"></exception>"
  public void ThrowIfEmpty (string groupName)
  {
    ThrowIfAbsent(groupName);

    if (this[groupName].Content.IsEmpty())
      throw new EmptyGroupException(groupName);
  }
  /// <summary>Returns a string representation of the current <see cref="MatchDataSet"/> object.</summary>
  /// <param name="indent">A string used for indentation in the output. Defaults to an empty string if not provided.</param>
  /// <remarks>The returned string includes a header and a detailed list of all groups in the match. If the
  /// match is empty, the string will indicate that no groups are present.</remarks>
  /// <returns>A string that represents the current <see cref="MatchDataSet"/> object, including its groups and their
  /// values.</returns>
  internal string ToString (string indent)
  {
    const string i1 = "  ";
    const string i2 = "    ";

    string head = indent + $"{typeof(MatchDataSet)} Object" + Chars.LFs;
    string grps = indent + i1 + "Groups = {";

    if (Count > 0)
    {
      grps += Chars.CRLF;
      foreach (KeyValuePair<string, GroupDataSet> grp in Groups)
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
  public Collection<IProperty<string>> MatchKVProperties
  {
    get
    {
      IEnumerable<KeyValuePair<string, string>> keys = Groups.Where(grp => grp.Key.StartsWith("m_prop_key_", SCOIC)).Select(g => (g.Key[7..], g.Value.Content).ToKVP());
      IEnumerable<KeyValuePair<string, string>> values = Groups.Where(grp => grp.Key.StartsWith("m_prop_value_", SCOIC)).Select(g => (g.Key[11..], g.Value.Content).ToKVP());
      IEnumerable<(KeyValuePair<string, string> First, KeyValuePair<string, string> Second)>? zip = keys.Zip(values);
      Collection<IProperty<string>> result = [];
      foreach ((KeyValuePair<string, string> first, KeyValuePair<string, string> second) in zip)
      {
        result.Add(new PropertyBase<string>() { Key = first.Value, Value = second.Value });
      }
      return result;
    }
  }

  /// <summary>Gets the match properties as a collection of key-value pairs.</summary>
  public Collection<KeyValuePair<string, string>> MatchProperties =>
    [.. Groups.Where(grp => grp.Key.StartsWith("m_prop_", SCOIC)).Select(g => (g.Key[7..], g.Value.Content).ToKVP())];
  public Collection<string> Markers =>
    [.. Groups.Select(g => g.Key).Where(grp => grp.StartsWith("m_", SCOIC))];
  /// <summary>Returns a string representation of the current <see cref="MatchDataSet"/> object.</summary>
  /// <remarks>The returned string includes a header and a detailed list of all groups in the match. If the
  /// match is empty, the string will indicate that no groups are present.</remarks>
  /// <returns>A string that represents the current <see cref="MatchDataSet"/> object, including its groups and their
  /// values.</returns>
  public override string ToString () => ToString(SE);
  /// <summary>
  /// Adds the specified <see cref="GroupDataSet"/> item to the collection.</summary>
  /// <param name="item">The <see cref="GroupDataSet"/> item to add to the collection. Cannot be null.</param>
  public void Add (GroupDataSet item) => Add(item);
  void ICollection<GroupDataSet>.Clear () => throw new NotSupportedException();
  bool ICollection<GroupDataSet>.Contains (GroupDataSet item) => throw new NotSupportedException();
  void ICollection<GroupDataSet>.CopyTo (GroupDataSet[] array, int arrayIndex) => throw new NotSupportedException();
  bool ICollection<GroupDataSet>.Remove (GroupDataSet item) => throw new NotSupportedException();
  /// <summary>Creates a copy of the object.</summary>
  /// <inheritdoc/>
  public static MatchDataSet Generate (MatchDataSet input) => input is not null ? new(input.Origin) : throw new ANEx(nameof(input));
  public bool Equals (MatchDataSet? other)
  {
    if (other?.IsNull != false)
    {
      return IsNull;
    }

    if (other.Count == Count)
    {
      foreach (GroupDataSet item in other)
      {
        if (!this[item.Name].Content.Equals(item.Content, SCO))
          return false;
      }
      return true;
    }

    return false;
  }

  public override bool Equals (object? obj) => Equals(obj as MatchDataSet);
  public override int GetHashCode () => Content.GetHashCode(SCO);
  public bool Equals (string? other) => Content.Equals(other, SCO);
}

public class LangSpec
{
  // actor n:class : n:parent replaces n:replace i:doomednum { r:inside_actor }
  // actor n:class : n:parent replaces n:replace { r:inside_actor }
  // actor n:class : n:parent i:doomednum { r:inside_actor }
  // actor n:class : n:parent { r:inside_actor }
  // actor n:class replaces n:replace i:doomednum { r:inside_actor }
  // actor n:class replaces n:replace { r:inside_actor }
  // actor n:class i:doomednum { r:inside_actor }
  // actor n:class { r:inside_actor }

  // inside_actor
  // 
  // property => n:prop a:value
  // addflag => + n:flagname
  // remflag => - n:flagname
  // dropitem => n:prop n:item, i:chance
  // states => states { r:inside states }
  // combo => monster
  // combo => projectile

  // inside_states
  //
  // label => n:label :
  // goto => goto n:state + i:offset
  // goto => goto n:state
  // wait => wait
  // fail => fail
  // loop => loop
  // stop => stop
  // frame => n:frame n:letter i:tics n:command ( r:inside_params )
  // frame => n:frame n:letter i:tics n:command
  // frame => n:frame n:letter i:tics
  // frame => " n:frame " n:letter i:tics n:command ( r:inside_params )
  // frame => " n:frame " n:letter i:tics n:command
  // frame => " n:frame " n:letter i:tics

  // inside_params
  //
  // expr => 
  public Collection<object> Rules { get; } = [
    new StringDef(@""".*?"""),
    new CommentDef(@"\/\/.*?$"),
    new CommentDef(@"\/\*.*?\*\/"),
    new TokenDef(@"(?i:\bactor\b)", "keyword"),
    new TokenDef(@"(?i:\+[a-z._0-9]+)", "addflag"),
    new TokenDef(@"(?i:\-[a-z._0-9]+)", "remflag"),
    new InsideTokenDef(@"\{", "actor_def"),
  ];
}
public record InsideTokenDef ([SS("regex")] string Regex, string ObjectType);
public record TokenDef ([SS("regex")] string Regex, string Type);
public record StringDef ([SS("regex")] string Regex);
public record CommentDef ([SS("regex")] string Regex);

public class MatchInfo
{
  public Dictionary<string, string> Properties { get; } = [];
  public required string Content { get; init; }
  public int Position { get; init; }
}
