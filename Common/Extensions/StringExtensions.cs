//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using SysRegex = System.Text.RegularExpressions.Regex;

namespace Common.Extensions;

public static class StringExtensions
{
  public static bool Is (this string? text, string? other) => text.IsEmpty() && other.IsEmpty() || (text?.Equals(other, SCO) ?? false);
  public static bool IsAny (this string? text, IEnumerable<string> other) => text.IsEmpty() && other.IsEmpty() || text.Any(other, Equals);
  public static bool Like (this string? text, string? other) => text.IsEmpty() && other.IsEmpty() || (text?.Equals(other, SCOIC) ?? false);
  public static bool Like (this string? text, string[] other) => text.IsEmpty() && other.IsEmpty() || (text?.Any(other, Like) ?? false);
  public static bool Any (this string? text, IEnumerable<string>? other, Func<string, string?, bool> func) => other?.Any(item => func(item, text)) ?? false;
  public static bool ContainsAny (this string text, IEnumerable<string> other) => other.Any(text.Contains);
  public static bool IsAlphanumeric (this string s) => s.All(item => item.IsAlphanumeric());
  public static bool IsControl (this string s) => s.All(item => item.IsControl());
  public static bool IsWhitespace (this string s) => s.All(item => item.IsWhitespace());
  public static bool IsNumber (this string s) => decimal.TryParse(s, out decimal _);
  public static bool IsPosInteger (this string s) => int.TryParse(s, out int _) && s.All(item => item.IsPosInteger());
  public static bool IsEmpty (this string? text) => string.IsNullOrEmpty(text);
  public static bool IsNotEmpty (this string? text) => !text.IsEmpty();
  public static int? ToInt (this string s) => int.TryParse(s, out int value) ? value : null;
  public static decimal? ToDecimal (this string s) => decimal.TryParse(s, out decimal value) ? value : null;
  public static Collection<string> Expand (this string s) => [.. s.Select(item => item.ToString())];
  public static string Remove (this string s, Match match) => s.Remove(match.Index, match.Length);
  public static string Replace (this string s, Match match, string replaceWith)
  {
    if (match.Length == 0)
      return s;

    s = s.Remove(match.Index, match.Length);
    s = s.Insert(match.Index, replaceWith);
    return s;
  }
  public static string Replace (this string s, IEnumerable<string> lookFor, string replaceWith)
  {
    if (!lookFor.Any())
      return s;

    foreach (string lf in lookFor)
      s = s.Replace(lf, replaceWith);
    return s;
  }
  public static string ReplaceIfContainsGroup (this string s, Match match, string group, string replaceWith) =>
    match.Groups.ContainsKey(group) ? s.Replace(match, replaceWith) : s;
  public static string ReplaceAllIfContainsGroup (this string s, MatchCollection matches, string group, string replaceWith)
  {
    foreach (Match m in matches)
      s = s.ReplaceIfContainsGroup(m, group, replaceWith);
    return s;
  }
  public static bool Equals (this string s, char c) => s.Length == 1 && c == s[0];
  public static bool EqualsAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.Equals(s2, sc));
  public static bool EqualsAll (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.All(s2 => s.Equals(s2, sc));
  public static bool StartsWithAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.StartsWith(s2, sc));
  public static bool EndsWithAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.EndsWith(s2, sc));
  public static int ContainsCount (this string s, string checkFor)
  {
    int count = 0;
    int pos = -1;
    do
    {
      pos = s.IndexOf(checkFor, pos);
      if (pos != -1)
      {
        count++;
        pos++;
      }
    } while (pos != -1);
    return count;
  }
  public static bool IsNamedGroup (this string text) => !text.IsPosInteger();
  public static string RemoveAll (this string? text, string regex, RegexOptions options = RegexOptions.None)
  {
    string result = text ?? string.Empty;

    while (SysRegex.IsMatch(result, regex))
      result = result.Remove(regex, options);

    return result;
  }
  public static string Remove (this string? text, string regex, RegexOptions options = RegexOptions.None) => text is null ? SE : SysRegex.Replace(text ?? SE, regex, SE, options);
  public static string Replace (this string? text, IEnumerable<string> lfs, IEnumerable<string> rws)
  {
    Collection<string> lfsc = [.. lfs];
    Collection<string> rwsc = [.. rws];

    if (text is null)
      return string.Empty;

    if (lfsc.Count != rwsc.Count)
      throw new ArgumentException("lfs and rws must be equal in size.", nameof(rws));

    for (int i = 0; i < lfsc.Count; i++)
    {
      string lf = lfsc[i];
      string rw = rwsc[i];

      text = text.Replace(lf, rw, SCO);
    }
    return text;
  }
  public static string ReplaceRange (this string text, int start, int length, char rep = '\0')
  {
    char[] array = text.ToCharArray();

    if (start + length > array.Length)
      throw new IndexOutOfRangeException("ReplaceRange: Index out of Range.");

    for (int i = start; i < length; i++)
      array[i] = rep;

    return new(array);
  }
  public static string RemoveNulls (this string? text) => text?.Replace("\0", "") ?? string.Empty;
  public static string PreReplace (this string text, int changed_pos) => text[..(changed_pos - 1)];
  public static string PostReplace (this string text, int changed_pos, int replace_length) => text[(changed_pos + replace_length)..];
  public static string Replace (this string? text, int changed_pos, int changed_length, string replacement = EmptyString)
  {
    if (text is null)
      return string.Empty;

    ArgumentOutOfRangeException.ThrowIfNegative(changed_pos);
    ArgumentOutOfRangeException.ThrowIfNegative(changed_length);

    return text.PreReplace(changed_pos) + replacement + text.PostReplace(changed_pos, changed_length);
  }
  public static string RecursiveReplace (this string text, string lf, string rw, StringComparison options = SCO)
  {
    string temp = text ?? "";

    while (temp.Contains(lf, options))
      temp = temp.Replace(lf, rw, options);

    return temp;
  }
  public static string Remove (this string? text, int changed_pos, int changed_length) => text.Replace(changed_pos, changed_length);
}
