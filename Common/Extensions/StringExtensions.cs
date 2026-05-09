//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Common.Regexp;

using SysRegex = System.Text.RegularExpressions.Regex;

namespace Common.Extensions;

/// <summary>Extensions for string objects.</summary>
public static class StringExtensions
{
  /// <summary>Shorthand for case sensitive ordinal comparison.</summary>
  /// <param name="text">This text.</param>
  /// <param name="other">The text to compare to.</param>
  /// <returns><see langword="true"/> if the values match, otherwise <see langword="false"/></returns>
  public static bool Is (this string? text, string? other) => (text.IsEmpty() && other.IsEmpty()) || (text?.Equals(other, SCO) ?? false);
  public static bool IsAny (this string? text, IEnumerable<string> other) => (text.IsEmpty() && other.IsEmpty()) || text.Any(other, Equals);
  public static bool IsAlphanumeric (this string s) => s.All(item => item.IsAlphanumeric());
  public static bool IsControl (this string s) => s.All(item => item.IsControl());
  public static bool IsWhitespace (this string s) => s.All(item => item.IsWhitespace());
  public static bool IsNumber (this string s) => decimal.TryParse(s, out decimal _);
  /// <summary>Checks if this string is a positive integer.</summary>
  /// <param name="text">The string to check.</param>
  /// <returns><see langword="true"/> if the <see langword="string"/> is a positive integer, <see langword="false"/> otherwise.</returns>
  public static bool IsPosInteger (this string text) => int.TryParse(text, out int i) && i >= 0;
  /// <summary>Checks if a <see langword="string"/> is <see langword="null"/> or empty.</summary>
  /// <param name="text">The <see langword="string"/> to check.</param>
  /// <returns><see langword="true"/> if the <see langword="string"/> is empty or is <see langword="null"/>, <see langword="false"/> otherwise.</returns>
  public static bool IsEmpty ([NotNullWhen(false)][MaybeNullWhen(true)] this string? text) => string.IsNullOrEmpty(text);
  /// <summary>Checks if a <see langword="string"/> is not <see langword="null"/> or empty.</summary>
  /// <param name="text">The <see langword="string"/> to check.</param>
  /// <returns><see langword="false"/> if the <see langword="string"/> is empty or <see langword="null"/>, <see langword="true"/> otherwise.</returns>
  public static bool IsNotEmpty ([NotNullWhen(true)] this string? text) => !text.IsEmpty();
  public static bool IsNamedGroup (this string text) => !text.IsPosInteger();
  /// <summary>Shorthand for case insensitive ordinal comparison.</summary>
  /// <param name="text">This text.</param>
  /// <param name="other">The other text.</param>
  /// <returns><see langword="true"/> if the values match, otherwise <see langword="false"/>.<br/>
  /// If either <paramref name="other"/> or <paramref name="text"/> are <see langword="null"/>, this method will return <see langword="false"/>.</returns>
  public static bool Like ([NotNullWhen(true)] this string? text, [NotNullWhen(true)] string? other) => text?.Equals(other, SCOIC) ?? false;
  public static bool Like ([NotNullWhen(true)] this string? text, IEnumerable<string> other) => (text.IsEmpty() && other.IsEmpty()) || (text?.Any(other, Like) ?? false);
  public static bool Any ([NotNullWhen(true)] this string? text, IEnumerable<string>? other, Func<string, string?, bool> func) => other?.Any(item => func(item, text)) ?? false;
  public static bool ContainsAny ([NotNullWhen(true)] this string text, IEnumerable<string> other, StringComparison sc = SCO) => other.Any(x => text.Contains(x, sc));
  public static bool ContainsNewLine ([NotNullWhen(true)] this string? s) => s is not null && (s.Contains('\n', SCO) || s.Contains('\r', SCO));
  public static int FirstIndexOfAny (this string s, IEnumerable<string> checkFor, int startAt, StringComparison sc, out int found_len)
  {
    found_len = DNE;

    if (s.IsEmpty() || checkFor?.IsEmpty() != false)
      return DNE;

    int index = DNE;

    foreach (string item in checkFor)
    {
      if (item.IsEmpty())
        continue;

      int cur = s.IndexOf(item, startAt, sc);

      if (index == DNE || (cur < index && cur != DNE))
      {
        index = cur;
        found_len = item.Length;
      }
    }

    return index;
  }
  public static int ContainsCount (this string s, string checkFor, StringComparison sc = SCO)
  {
    if (s is null || checkFor is null || s.Length == 0 || checkFor.Length == 0)
      return 0;

    int count = 0;
    int pos = DNE;
    do
    {
      pos = s.IndexOf(checkFor, pos == DNE ? 0 : pos, sc);
      if (pos != DNE)
      {
        count++;
        pos++;
      }
    } while (pos != DNE);
    return count;
  }
  public static int ContainsCount (this string s, IList<string> checkForAny, StringComparison sc = SCO)
  {
    if (s is null || checkForAny is null || s.Length == 0 || checkForAny.Count == 0)
      return 0;

    int count = 0;
    int pos = DNE;
    do
    {
      pos = s.FirstIndexOfAny(checkForAny, pos == DNE ? 0 : pos, sc, out int found_len);
      if (pos != DNE)
      {
        count++;
        pos += found_len;
      }
    } while (pos != DNE);
    return count;
  }
  public static int? ToInt (this string s) => int.TryParse(s, out int value) ? value : null;
  public static decimal? ToDecimal (this string s) => decimal.TryParse(s, out decimal value) ? value : null;
  public static bool? ToBool (this string s) => bool.TryParse(s, out bool value) ? value : null;
  public static bool ValidateWithRegex (this string s, [SS("regex")] string validation_expression, RegexOptions options = RegexOptions.None)
  {
    MatchCollection matches = SysRegex.Matches(s, validation_expression, options);
    return matches.Count == 1 && matches[0].Length == s?.Length;
  }
  public static DateTime? ToDateTime (this string s) => DateTime.TryParse(s, out DateTime value) ? value : null;
  public static TimeSpan? ToTimeSpan (this string s) => TimeSpan.TryParse(s, out TimeSpan value) ? value : null;
  public static Collection<string> Expand (this string s) => [.. s.Select(item => item.ToString())];
  public static string Remove (this string? text, string regex, RegexOptions options = RegexOptions.None) => text is null ? SE : SysRegex.Replace(text, regex, SE, options);
  public static string Remove (this string s, Match match) => s is null ? SE : match is null ? s : s.Remove(match.Index, match.Length);
  public static string Remove (this string? text, int changed_pos, int changed_length) => text is null ? SE : text.Replace(changed_pos, changed_length);
  public static string RemoveChars (this string? text, string chars) => new(text?.Where(item => !chars.Contains(item, SCO)).ToArray());
  public static string RemoveAllButChars (this string? text, string chars) => new(text?.Where(item => chars.Contains(item, SCO)).ToArray());
  public static string RemoveAll (this string? text, string regex, RegexOptions options = RegexOptions.None)
  {
    string result = text ?? string.Empty;

    while (SysRegex.IsMatch(result, regex))
      result = result.Remove(regex, options);

    return result;
  }
  /// <summary>Removes all null characters from the string.</summary>
  /// <param name="text">The input string.</param>
  /// <returns>The input string with all null characters removed.</returns>
  public static string RemoveNulls (this string? text) => text?.Replace("\0", "", SCO) ?? string.Empty;
  public static string Replace (this string s, Match match, string replaceWith)
  {
    if (s is null)
      return SE;

    if (match is null || match.Length == 0)
      return s;

    s = s.Remove(match.Index, match.Length);
    s = s.Insert(match.Index, replaceWith);
    return s;
  }
  public static string Replace (this string s, IEnumerable<string> lookFor, string replaceWith)
  {
    if (s is null)
      return SE;
    if (lookFor is null)
      return s;
    foreach (string lf in lookFor)
      s = s.Replace(lf, replaceWith, SCO);
    return s;
  }
  public static string XMLEscape (this string s)
  {
    s ??= SE;
    s = s.Replace("&", "&amp;", SCO);
    s = s.Replace("'", "&apos;", SCO);
    s = s.Replace("\"", "&quot;", SCO);
    s = s.Replace("<", "&lt;", SCO);
    s = s.Replace(">", "&gt;", SCO);
    return s;
  }
  public static string Replace (this string? text, int changed_pos, int changed_length, string replacement = EmptyString)
  {
    if (text is null)
      return string.Empty;

    ArgumentOutOfRangeException.ThrowIfNegative(changed_pos);
    ArgumentOutOfRangeException.ThrowIfNegative(changed_length);

    return text.PreReplace(changed_pos) + replacement + text.PostReplace(changed_pos, changed_length);
  }
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
  public static string Replace (this string? text, ReplaceNode node, RegexOptions options = ROIPW) => node?.ReplaceRegex(text ?? SE, options) ?? SE;
  public static string ReplaceRange (this string text, int start, int length, char rep = '\0')
  {
    if (text is null)
      return SE;

    char[] array = text.ToCharArray();

    if (start + length > array.Length)
      throw new ArgumentOutOfRangeException(nameof(start));

    for (int i = start; i < length; i++)
      array[i] = rep;

    return new(array);
  }
  public static string ReplaceIfContainsGroup (this string s, Match match, string group, string replaceWith) =>
    match?.Groups.ContainsKey(group) == true ? s.Replace(match, replaceWith) : s;
  public static string ReplaceAllIfContainsGroup (this string s, MatchCollection matches, string group, string replaceWith)
  {
    if (matches is null)
      return s;
    foreach (Match m in matches)
      s = s.ReplaceIfContainsGroup(m, group, replaceWith);
    return s;
  }
  public static bool Equals (this string s, char c) => s?.Length == 1 && c == s[0];
  public static bool EqualsAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.Equals(s2, sc));
  public static bool EqualsAll (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.All(s2 => s.Equals(s2, sc));
  public static bool StartsWithAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.StartsWith(s2, sc));
  public static bool StartsWithAny (this string s, StringComparison sc = SCO, params IEnumerable<string> list) =>
    list.Any(s2 => s.StartsWith(s2, sc));
  public static bool EndsWithAny (this string s, IEnumerable<string> list, StringComparison sc = SCO) =>
    list.Any(s2 => s.EndsWith(s2, sc));
  public static bool EndsWithAny (this string s, StringComparison sc = SCO, params IEnumerable<string> list) =>
    list.Any(s2 => s.EndsWith(s2, sc));
  public static string PreReplace (this string text, int changed_pos) => text[..(changed_pos - 1)];
  public static string PostReplace (this string text, int changed_pos, int replace_length) => text[(changed_pos + replace_length)..];
  public static string RecursiveReplace (this string text, string lf, string rw, StringComparison options = SCO)
  {
    string temp = text ?? "";

    while (temp.Contains(lf, options))
      temp = temp.Replace(lf, rw, options);

    return temp;
  }
  public static (int Line, int Col) Get2DPosition (this string text, int position)
  {
    if (text is null || position < 0)
      return (DNE, DNE);
    string before = text[..position];

    int lnst = before.LastIndexOfAny(['\n', '\r', '\v']);
    int lines = before.ContainsCount(Chars.NewLines, SCO);
    int col = before.Length - lnst;
    return (lines, col);
  }
  public static int Get1DPosition (this string text, int line, int col)
  {
    if (string.IsNullOrEmpty(text) || line < 0 || col < 0)
      return DNE;
    string[] lines = text.Split('\n');
    string[] beforelns = lines[0..line];
    string flat = beforelns.TextJoin("\n");
    return flat.Length + col;
  }
  public static int Get1DPosition (this string[] lines, int line, int col)
  {
    if (lines is null || lines.Length == 0 || line < 0 || col < 0)
      return DNE;
    string[] beforelns = lines[0..line];
    string flat = beforelns.TextJoin("\n");
    return flat.Length + col;
  }

  public static void ThrowIfNullOrEmpty ([NotNull] this string? text)
  {
    if (text.IsEmpty())
      throw new ANEx(nameof(text));
  }
  public static T ToEnum<T> (this string data) where T : notnull => (T) Enum.Parse(typeof(T), data);
  public static int ToEnum (this string data, Type type) => (int) Enum.Parse(type, data);
  public static bool TryMatchAt ([SS("regex")] this string rx, RegexOptions options, int index, string input, [NotNullWhen(true)] out MatchDataSet? match)
  {
    match = null;
    Match attempt = SysRegex.Match(input, rx, options);

    if (attempt.Success && attempt.Index == index)
    {
      match = attempt.ToMatchData();
      return true;
    }

    return false;
  }
}
