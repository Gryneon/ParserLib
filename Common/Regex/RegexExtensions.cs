//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Immutable;

namespace Common.Regex;

public static class RegexExtensions
{
  #region MatchCollection Extensions
  /// <summary>
  /// Exposes the <see cref="IEnumerator{Match}"/> for <paramref name="mc"/>.
  /// </summary>
  /// <param name="mc">The <see cref="MatchCollection"/> to enumerate.</param>
  /// <returns>The <see cref="IEnumerator{Match}"/>.</returns>
  public static IEnumerator<Match> GetEnumerator (this MatchCollection mc) => mc.ToImmutableList().GetEnumerator();
  public static Collection<MatchDataDictionary> ToMDDCollection (this MatchCollection mc) =>
    [.. mc.Select(item => item.ToMatchDictionary())];
  #endregion
  #region Match Extensions
  public static Dictionary<string, string> ToDictionary (this Match match) =>
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select grp.ToKvp()];
  public static Dictionary<string, GroupData> ToGroupDictionary (this Match match) =>
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select new KeyValuePair<string, GroupData>(grp.Name, grp.ToGroupData())];
  public static SortedDictionary<string, GroupData> ToSortedDictionary (this Match match) =>
    [.. from Group grp in match.Groups
        where grp.Value.Length > 0
        select new KeyValuePair<string, GroupData>(grp.Name, grp.ToGroupData())];
  public static MatchDataDictionary ToMatchDictionary (this Match match) => new(match);
  #endregion
  #region Group Extensions
  public static KeyValuePair<string, string> ToKvp (this Group group) =>
    new(group.Name, group.Value);
  public static GroupData ToGroupData (this Group group) => new(group);
  #endregion
  public static CaptureData ToCaptureData (this Capture cap, string groupname) =>
    new(cap, groupname);
}