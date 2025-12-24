namespace Parser.Tokens.Raw;

public class TokenFactory_AI<T> (IEnumerable<TokenRule<T>> rules) where T : notnull
{
  private readonly IReadOnlyList<TokenRule<T>> _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));

  private readonly struct Range
  {
    public readonly int Start;
    public readonly int End;
    public Range (int start, int length) { Start = start; End = start + length; }
    public bool Contains (int start, int length) => Start <= start && start + length <= End;
  }

  public TokenCollection<T> Produce (string input)
  {
    input.ThrowIfNull();
    string working_input = input;

    List<Token<T>> result = [];
    List<Range> exemptRanges = [];   // protected spans (strings/comments)
    List<int> splitBoundaries = [];  // indices matches must not cross

    // Pass 1: run all Competitive exact/regex rules together to establish exclusive, exempt regions
    RunCompetitivePass(working_input, result, exemptRanges, splitBoundaries);

    // Pass 2: process the remaining rules in order
    foreach (TokenRule<T> rule in _rules)
    {
      RT baseType = MaskType(rule.Type);

      switch (baseType)
      {
        case RT.None:
          continue;

        case RT.TokenExact when rule.RuleStringData is not null:
          if (rule.Type.HasFlag(RT.FromTokens))
          {
            StringComparer comparer = rule.Type.HasFlag(RT.IgnoreCase) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            foreach (Token<T> tok in result)
            {
              // keywords only match non-exempt tokens; no type checks
              if (InsideExempt(exemptRanges, tok.Position, tok.Length)) continue;
              if (comparer.Equals(tok.Content, rule.RuleStringData))
                tok.Type = rule.TypeToAssign;
            }
          }
          else
          {
            EnumerateAndAddMatches(
                working_input,
                ignored: rule.Type.HasFlag(RT.IgnoredToken),
                pattern: rule.RuleStringData,
                options: (rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON) | ROML | ROEC,
                assignType: rule.TypeToAssign,
                isExempt: rule.Type.HasFlag(RT.ExemptAllWithin),
                result, exemptRanges, splitBoundaries);
          }
          break;

        case RT.TokenMatch when rule.RuleStringData is not null:
          if (rule.Type.HasFlag(RT.FromTokens))
          {
            Regex rx = new(rule.RuleStringData, rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON);
            foreach (Token<T> tok in result)
            {
              // keywords only match non-exempt tokens; no type checks
              if (InsideExempt(exemptRanges, tok.Position, tok.Length)) continue;
              if (rx.IsMatch(tok.Content))
                tok.Type = rule.TypeToAssign;
            }
          }
          else
          {
            EnumerateAndAddMatches(
                working_input,
                ignored: rule.Type.HasFlag(RT.IgnoredToken),
                pattern: rule.RuleStringData,
                options: rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON,
                assignType: rule.TypeToAssign,
                isExempt: rule.Type.HasFlag(RT.ExemptAllWithin),
                result, exemptRanges, splitBoundaries);
          }
          break;

        case RT.SplitExact when rule.RuleStringData is not null:
          AddSplitBoundaries(working_input, rule.RuleStringData,
                             rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON, splitBoundaries);
          break;

        case RT.SplitMatch when rule.RuleStringData is not null:
          AddSplitBoundaries(working_input, rule.RuleStringData,
                             rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON, splitBoundaries);
          break;

        case RT.StoreExtra:
          StoreExtraMatches(working_input, rule, result, exemptRanges);
          break;

        case RT.StoreOther:
          StoreOther(working_input, rule, result, exemptRanges);
          break;
      }
    }

    return [.. result.OrderBy(t => t.Position)];
  }

  // --- Helpers -------------------------------------------------------------

  private static RT MaskType (RT t) =>
      t & ~(RT.FromTokens | RT.ExemptAllWithin | RT.Competitive | RT.IgnoreCase | RT.IgnoredToken);
  private static bool OverlapsExisting (List<Token<T>> tokens, int start, int length) =>
      tokens.Any(tok => !(start + length <= tok.Position || tok.LastPosition + 1 <= start));
  private static bool InsideExempt (List<Range> exemptRanges, int start, int length) =>
      exemptRanges.Any(r => r.Contains(start, length));
  private static bool CrossesBoundary (List<int> boundaries, int start, int length)
  {
    if (boundaries.Count == 0) return false;
    int end = start + length;
    int i = boundaries.BinarySearch(start);
    if (i < 0) i = ~i;
    while (i < boundaries.Count && boundaries[i] < end)
    {
      if (boundaries[i] > start && boundaries[i] < end) return true;
      i++;
    }
    return false;
  }
  private static void AddToken (List<Token<T>> result, List<Range> exemptRanges, int pos, int len, string content, T assignType, bool isExempt, bool ignored)
  {
    Token<T> token = new()
    {
      Content = content,
      Position = pos,
      Type = assignType,
      Ignored = ignored
    };
    result.Add(token);
    if (isExempt) exemptRanges.Add(new Range(pos, len));
  }
  private static void EnumerateAndAddMatches (string input,
      bool ignored,
      string pattern,
      RegexOptions options,
      T assignType,
      bool isExempt,
      List<Token<T>> result,
      List<Range> exemptRanges,
      List<int> splitBoundaries)
  {
    Regex.ValueMatchEnumerator e = Regex.EnumerateMatches(input.AsSpan(), pattern, options);
    while (e.MoveNext())
    {
      ValueMatch m = e.Current;
      int len = m.Length;
      int pos = m.Index;

      if (InsideExempt(exemptRanges, pos, len)) continue;
      if (CrossesBoundary(splitBoundaries, pos, len)) continue;
      if (OverlapsExisting(result, pos, len)) continue;

      string content = input.Substring(pos, len);
      AddToken(result, exemptRanges, pos, len, content, assignType, isExempt, ignored);
    }
  }

  private static void AddSplitBoundaries (string input, string pattern, RegexOptions options, List<int> splitBoundaries)
  {
    foreach (ValueMatch m in Regex.EnumerateMatches(input.AsSpan(), pattern, options))
    {
      splitBoundaries.Add(m.Index);
      splitBoundaries.Add(m.Index + m.Length);
    }
    splitBoundaries.Sort();
  }

  private void RunCompetitivePass (
      string input,
      List<Token<T>> result,
      List<Range> exemptRanges,
      List<int> splitBoundaries)
  {
    List<TokenRule<T>> competitive = [.. _rules.Where(r => r.Type.HasFlag(RT.Competitive)).Where(r => MaskType(r.Type) is RT.TokenExact or RT.TokenMatch)];
    if (competitive.Count == 0) return;

    List<(int pos, int len, TokenRule<T> rule)> candidates = [];

    foreach (TokenRule<T> r in competitive)
    {
      RT baseType = MaskType(r.Type);
      RegexOptions options = r.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON;

      if (baseType == RT.TokenExact && r.RuleStringData is not null)
      {
        Regex.ValueMatchEnumerator e = Regex.EnumerateMatches(input.AsSpan(), r.RuleStringData, options);
        while (e.MoveNext())
        {
          ValueMatch m = e.Current;
          candidates.Add((m.Index, m.Length, r));
        }
      }
      else if (baseType == RT.TokenMatch && r.RuleStringData is not null)
      {
        Regex.ValueMatchEnumerator e = Regex.EnumerateMatches(input.AsSpan(), r.RuleStringData, options);
        while (e.MoveNext())
        {
          ValueMatch m = e.Current;
          candidates.Add((m.Index, m.Length, r));
        }
      }
    }

    // Resolve overlaps: leftmost-longest, then rule order tie-breaker
    List<(int pos, int len, TokenRule<T> rule)> ordered = [.. candidates
        .OrderBy(c => c.pos)
        .ThenByDescending(c => c.len)
        .ThenBy(c => competitive.IndexOf(c.rule))];

    List<(int pos, int len, TokenRule<T> rule)> accepted = [];
    foreach ((int pos, int len, TokenRule<T> rule) c in ordered)
    {
      if (accepted.Any(a => !(c.pos + c.len <= a.pos || a.pos + a.len <= c.pos))) continue;
      accepted.Add(c);
    }

    foreach ((int pos, int len, TokenRule<T>? rule) in accepted)
    {
      if (CrossesBoundary(splitBoundaries, pos, len)) continue;
      string content = input.Substring(pos, len);
      AddToken(result, exemptRanges, pos, len, content, rule.TypeToAssign, rule.Type.HasFlag(RT.ExemptAllWithin), rule.Type.HasFlag(RT.IgnoredToken));
    }
  }
  private static void StoreExtraMatches (string input, TokenRule<T> rule, List<Token<T>> result, List<Range> exemptRanges)
  {
    if (rule.RuleStringData is null) return;

    bool[] covered = new bool[input.Length];
    foreach (Token<T> t in result)
      for (int i = t.Position; i < t.Position + t.Length && i < covered.Length; i++)
        covered[i] = true;
    foreach (Range r in exemptRanges)
      for (int i = r.Start; i < r.End && i < covered.Length; i++)
        covered[i] = true;

    int i0 = 0;
    while (i0 < input.Length)
    {
      while (i0 < input.Length && covered[i0]) i0++;
      if (i0 >= input.Length) break;

      int j = i0 + 1;
      while (j < input.Length && !covered[j]) j++;

      ReadOnlySpan<char> span = input.AsSpan(i0, j - i0);
      Regex.ValueMatchEnumerator e = Regex.EnumerateMatches(span, rule.RuleStringData, rule.Type.HasFlag(RT.IgnoreCase) ? ROIC : RON);

      while (e.MoveNext())
      {
        ValueMatch m = e.Current;
        int pos = i0 + m.Index;
        int len = m.Length;
        string content = input.Substring(pos, len);
        AddToken(result, exemptRanges, pos, len, content, rule.TypeToAssign, isExempt: false, ignored: rule.Type.HasFlag(RT.IgnoredToken));
        for (int k = pos; k < pos + len && k < covered.Length; k++)
          covered[k] = true;
      }
      i0 = j;
    }
  }
  private static void StoreOther (string input, TokenRule<T> rule, List<Token<T>> result, List<Range> exemptRanges)
  {
    bool[] covered = new bool[input.Length];
    foreach (Token<T> t in result)
      for (int i = t.Position; i < t.Position + t.Length && i < covered.Length; i++)
        covered[i] = true;
    foreach (Range r in exemptRanges)
      for (int i = r.Start; i < r.End && i < covered.Length; i++)
        covered[i] = true;

    int i0 = 0;
    while (i0 < input.Length)
    {
      while (i0 < input.Length && covered[i0]) i0++;
      if (i0 >= input.Length) break;

      int j = i0 + 1;
      while (j < input.Length && !covered[j]) j++;

      int len = j - i0;
      string content = input.Substring(i0, len);
      AddToken(result, exemptRanges, i0, len, content, rule.TypeToAssign, isExempt: false, ignored: rule.Type.HasFlag(RT.IgnoredToken));
      i0 = j;
    }
  }
}
