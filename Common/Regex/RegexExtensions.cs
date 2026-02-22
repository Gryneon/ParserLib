//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Immutable;

using RegexObj = System.Text.RegularExpressions.Regex;

namespace Common.Regex;

/// <summary>A static class contianing extension methods for <see cref="RegexObj"/> related types.</summary>
public static class RegexExtensions
{
  #region MatchCollection Extensions
  /// <summary>Exposes the <see cref="IEnumerator{Match}"/> for <paramref name="mc"/>.</summary>
  /// <param name="mc">The <see cref="MatchCollection"/> to enumerate.</param>
  /// <returns>The <see cref="IEnumerator{Match}"/>.</returns>
  public static IEnumerator<Match> GetEnumerator (this MatchCollection mc) => mc.ToImmutableList().GetEnumerator();
  public static MatchDataCollection ToMDDCollection (this MatchCollection mc) =>
    [.. mc.Select(item => item.ToMatchData())];
  #endregion
  #region Match Extensions
  public static Dictionary<string, string> ToDictionary (this Match match) =>
    match is null ? [] :
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select grp.ToKvp()];
  public static Dictionary<string, GroupDataSet> ToGroupDictionary (this Match match) =>
    match is null ? [] :
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select new KeyValuePair<string, GroupDataSet>(grp.Name, grp.ToGroupData())];
  public static SortedDictionary<string, GroupDataSet> ToSortedDictionary (this Match match) =>
    match is null ? [] :
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select new KeyValuePair<string, GroupDataSet>(grp.Name, grp.ToGroupData())];
  public static MatchDataSet ToMatchData (this Match match) => new(match);
  #endregion
  #region Group Extensions
  public static KeyValuePair<string, string> ToKvp (this Group group)
  {
    group.ThrowIfNull();
    return new(group.Name, group.Value);
  }
  public static GroupDataSet ToGroupData (this Group group) => new(group);
  #endregion
  public static CaptureData ToCaptureData (this Capture cap, string groupname) =>
    new(cap, groupname);
}
