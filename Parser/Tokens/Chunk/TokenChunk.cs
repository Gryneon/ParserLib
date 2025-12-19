//#pragma warning disable IDE0306 // Simplify collection initialization

namespace Parser.Tokens.Chunk;

public class TokenChunk : IEquatable<TokenChunk>, IGeneratable<MatchDataSet, TokenChunk>
{
  // Prefix:
  //         m_ = marker (This just is a flag to signify something is present or it is of a given type.)
  //         t_ = token (These make up the internal operators and punctuation, keywords, etc.)
  //
  // Format:
  //         t_TOKENTYPE[_TOKENNAME] - Defines a basic token, with TOKENTYPE being the type, and TOKENNAME being an optional identifier.
  //         m_MARKERNAME - Defines a match level marker. These typically are used to determine what object they are made into.
  //         m_prop_PROPERTYNAME - Defines a match level property.
  //         m_prop_list_PROPERTYNAME - Defines a list property. All instances of the property are compiled to a list.
  //         m_prop_key_INDEX - Defines the key of a property, matched with the value on INDEX.
  //         m_prop_value_INDEX- Defines the value of a property, matched with the key on INDEX.
  //
  // Data:
  //         m_prop_PROPERTYNAME = the chunk will have the content of this capture stored under 'property-name'.
  //         m_prop_list_PROPERTYNAME = the chunk will have the contents of all captures that are found with this name stored as a Collection under 'property-name'.
  //         m_prop_key_INDEX = the chunk will have a property with the key as the value of this statement.
  //         m_prop_value_INDEX = the chunk will have a property with the value as the value of this statement.
  //  Pos->  012345678901234567890

  public const string MarkerID = "m_";
  public const string TokenID = "t_";
  public const string PropID = "prop_";
  public const string ListID = "list_";
  public const string MarkerPropID = MarkerID + PropID;
  public const string MarkerListPropID = MarkerID + PropID + ListID;

  [MemberNotNull(nameof(Markers), nameof(Tokens), nameof(MarkerProperties), nameof(MarkerListProperties), nameof(MarkerKeyProperties), nameof(MarkerValueProperties))]
  private void ParseTokens (IEnumerable<GroupDataSet> groups)
  {
    List<GroupDataSet> groupList = [.. groups];

    Markers = [..
      from g in groupList
      let t = g.Name.GetGroupNameType()
      where t is GroupNameType.Marker
      select g.Name];
    Tokens = [.. new List<CaptureData>((
      from g in groupList
      let t = g.Name.GetGroupNameType()
      where t.HasFlag(GroupNameType.Token)
      orderby g.Pos
      select g.Captures).SelectMany(item => item))];
    MarkerProperties = (
      from g in groupList
      let t = g.Name.GetGroupNameType()
      where t.HasFlag(GroupNameType.Marker | GroupNameType.Basic)
      select new KeyValuePair<string, string>(g.Name, g.Content)).ToDictionary();
    MarkerListProperties = [..
      from g in groupList
      let t = g.Name.GetGroupNameType()
      where t.HasFlag(GroupNameType.List)
      select (g.Name, g.Content)];
    MarkerKeyProperties = [..
      from g in groupList
      let i = g.Name[11..].ToInt() ?? -1
      where g.Name.GetGroupNameType().HasFlag(GroupNameType.Key)
      select (g.Name, g.Content, i)];
    MarkerValueProperties = [..
      from g in groupList
      let i = g.Name[11..].ToInt() ?? -1
      where g.Name.GetGroupNameType().HasFlag(GroupNameType.Value)
      select (g.Name, g.Content, i)];
  }

  [SetsRequiredMembers]
  public TokenChunk (IEnumerable<GroupDataSet> groups)
  {
    groups.ThrowIfNull();
    ParseTokens(groups);
    Origin = groups;
  }
  [SetsRequiredMembers]
  public TokenChunk (MatchDataSet match)
  {
    match.ThrowIfNull();
    ParseTokens(match.Groups.Values);
    Origin = match;
  }

  public Collection<string> Markers { get; private set; }
  public Collection<CaptureData> Tokens { get; private set; }
  public Dictionary<string, string> MarkerProperties { get; private set; }
  public Collection<(string, string)> MarkerListProperties { get; private set; }
  public Collection<(string, string, int)> MarkerKeyProperties { get; private set; }
  public Collection<(string, string, int)> MarkerValueProperties { get; private set; }

  protected object Origin { get; private set; }

  public bool Equals (TokenChunk? other) => other is not null && Markers.SequenceEqual(other.Markers) && Tokens.SequenceEqual(other.Tokens);
  public override bool Equals (object? obj) => obj is TokenChunk other && Markers.SequenceEqual(other.Markers) && Tokens.SequenceEqual(other.Tokens);
  public override int GetHashCode () => HashCode.Combine(Markers, Tokens);

  public static bool operator == (TokenChunk left, TokenChunk right) => left is null && right is null || left is not null && left.Equals(right);
  public static bool operator != (TokenChunk left, TokenChunk right) => !(left == right);

  public bool HasMarker (string marker) => Markers.Any(m => m.Contains(marker, SCOIC));
  public bool HasProperty (string property_name) => MarkerProperties.ContainsKey(property_name);
  public T Construct<T> () where T : class, IGeneratable<MatchDataSet, T>, new() =>
   Origin is MatchDataSet mds && T.TryGenerate(mds, out T? result) ? result : throw new InvalidOperationException("Construct<T> method failed.");
  public static TokenChunk Generate (MatchDataSet mdd)
  {
    mdd.ThrowIfNull();
    TokenChunk result = new(mdd);
    return result;
  }
}
