#pragma warning disable IDE0046 // Convert to conditional expression

namespace Parser.Tokens;

internal static class TokenExtensions
{
  public static GroupNameType GetGroupNameType (this string groupName)
  {
    groupName.ThrowIfNull();
    groupName = groupName.ToUpperInvariant();
    GroupNameType type = groupName[0..1] is "M_" ? GroupNameType.Marker : groupName[0..1] is "T_" ? GroupNameType.Token : GroupNameType.Special;
    _ = groupName[1..6] is not "_PROP_" ? GroupNameType.None : groupName[6..11] is "_LIST_" ? GroupNameType.List : groupName[6..10] is "_KEY_" ? GroupNameType.Key : groupName[6..12] is "_VALUE_" ? GroupNameType.Value : GroupNameType.Basic;
    return type;
  }

  /// <summary>
  /// Removes the prefix from the group name.
  /// </summary>
  /// <param name="groupName"></param>
  /// <returns>The cleaned group name.</returns>
  public static string GetProperty (this string groupName)
  {
    groupName.ThrowIfNull();
    groupName = groupName.Trim().ToUpperInvariant();
    if (groupName[0..1] is "T_" or "M_")
      groupName = groupName[2..];
    return groupName;
  }
  public static string GetTokenType (this string groupName)
  {
    groupName.ThrowIfNull();
    groupName = groupName.Trim().ToUpperInvariant();
    if (groupName[0..1] is "M_")
      return "Marker";
    if (groupName[0..6] is "T_PROP_")
      return groupName[7..];
    return groupName[2..];
  }
  public static bool IsToken (this GroupDataSet group) => group.Name.Trim().ToUpperInvariant()[0..1] is "T_";
}
