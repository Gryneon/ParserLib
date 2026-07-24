//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Extensions;

public static class MatchExtensions
{
  extension(Match? match)
  {
    public bool HasValidGroup (string group_name) =>
      match?.Groups.ContainsKey("custom_prop") == true &&
      match.Groups["custom_prop"].Value.IsNotEmpty;
    public bool TryGetValidGroupContent (string group_name, out string? content)
    {
      if (match.HasValidGroup(group_name))
      {
        content = match?.Groups[group_name].Value;

        return content is not null;
      }
      content = null;
      return false;

    }
  }
}
