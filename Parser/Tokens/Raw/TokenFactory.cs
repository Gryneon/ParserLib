#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenFactory<T> (IEnumerable<TokenRule<T>> rules) where T : notnull
{
  internal const string Area = "TokenFactory<T>";
  internal string Input = SE;
  internal SectionCollection CannotMatch = [];
  internal TokenCollection<T> Result = [];
  internal TokenRule<T>? CurrentRule;
  internal bool IgnoreCase => CurrentRule?.Type.HasFlag(RT.IgnoreCase) ?? false;
  internal StringComparison IC => IgnoreCase ? SCOIC : SCO;
  internal bool Competes => CurrentRule?.Type.HasFlag(RT.Competitive) ?? false;
  internal bool IgnoredToken => CurrentRule?.Type.HasFlag(RT.IgnoredToken) ?? false;
  internal bool FromTokens => CurrentRule?.Type.HasFlag(RT.FromTokens) ?? false;
  internal bool ExemptAllWithin => CurrentRule?.Type.HasFlag(RT.ExemptAllWithin) ?? false;
  internal RT Type => GetMaskedType(CurrentRule?.Type ?? RT.None);
  internal string RuleData => CurrentRule?.RuleStringData ?? SE;

  public string GetRuleRegex (TokenRule<T> rule, int? index = null)
  {
    string regex = rule?.RuleStringData ?? SE;

    if (GetMaskedType(rule?.Type ?? RT.None) == RT.TokenExact)
    {
      regex = Regex.Escape(regex);
    }

    string casemod = rule?.Type.HasFlag(RT.IgnoreCase) ?? false ? "(?i)" : "(?-i)";

    regex = index is not null ? $"{casemod}(?'_R{index}'{regex})" : $"{casemod}{regex}";

    return regex;
  }
  internal static RT GetMaskedType (RT type) => type.RemoveBit<RT>(RT.FlagBits);
  internal static int GetRuleGroupIndex (Match match)
  {
    string num = match.Groups.
      AsReadOnly().
      First(static g => g.Name.StartsWith("_R", SCO) && g.Value.Length > 0).
      Name[2..];
    return int.TryParse(num, out int value) ? value : ErrVal;
  }
  internal void Tokens_FromTokens ()
  {
    foreach (IToken<T> tokendata in Result)
    {
      if (tokendata.Exempt)
        continue;

      if (CurrentRule is null)
        break;

      if (Type is RT.TokenExact)
      {
        if (tokendata.Content.Equals(RuleData, IC))
        {
          tokendata.Type = CurrentRule!.TypeToAssign;
        }
      }
      else if (Type is RT.TokenMatch)
      {
        if (Regex.Match(tokendata.Content, GetRuleRegex(CurrentRule)).Length == tokendata.Content.Length)
        {
          tokendata.Type = CurrentRule!.TypeToAssign;
        }
      }
    }
  }
  internal void Tokens_StoreOther ()
  {
    foreach (Section applicant in CannotMatch.Inverse())
    {
      Log(Area, "Tokens_StoreOther", $"Section: {applicant} Found with no token.");
      Result.Add(new Token<T>()
      {
        Index = applicant.Start,
        Ignored = IgnoredToken,
        Type = CurrentRule!.TypeToAssign,
        Exempt = ExemptAllWithin,
        Content = applicant.Content
      });
    }
  }
  internal void Tokens_StoreExtra ()
  {
    foreach (Section applicant in CannotMatch.Inverse())
    {
      if (Regex.IsMatch(applicant.Content, RuleData))
      {
        MatchCollection mc = Regex.Matches(applicant.Content, RuleData);
        foreach (Match m in mc)
        {
          CannotMatch.Add(Section.ByLength(applicant.Start + m.Index, m.Length, Input));
          Result.Add(new Token<T>()
          {
            Index = applicant.Start + m.Index,
            Content = m.Value,
            Ignored = IgnoredToken,
            Type = CurrentRule!.TypeToAssign,
            Exempt = ExemptAllWithin
          });
        }
      }
    }
  }
  internal void Tokens_FromInput (bool split)
  {
    if (Type is RT.TokenExact or RT.SplitExact)
    {
      int length = RuleData.Length;
      int cursor = 0;

      if (length == 0)
        return;

      int next = Input.IndexOf(RuleData, cursor, IC);

      while (next >= 0 && cursor <= Input.Length)
      {
        Section match = Section.ByLength(next, length, Input);

        if (!match.Overlaps(CannotMatch))
        {
          string sub = Input.Substring(next, length);
          Token<T> token = new()
          {
            Index = next,
            Content = sub,
            Type = CurrentRule!.TypeToAssign,
            Ignored = IgnoredToken,
            Exempt = ExemptAllWithin
          };
          CannotMatch.Add(match);
          if (!split)
            Result.Add(token);
          cursor = next + length;
          next = Input.IndexOf(RuleData, cursor, IC);
        }
        else
        {
          cursor = next + 1;
          next = Input.IndexOf(RuleData, cursor, IC);
        }
      }
    }
    else if (Type is RT.TokenMatch or RT.SplitMatch)
    {
      Regex regex = new(RuleData, ROEC | ROML);

      MatchCollection mc = regex.Matches(Input);

      foreach (Match match in mc)
      {
        Section rng = Section.ByLength(match.Index, match.Length, Input);

        if (!rng.Overlaps(CannotMatch))
        {
          string sub = match.Value;
          if (ExemptAllWithin)
            CannotMatch.Add(rng);
          if (!split)
            Result.Add(new Token<T>()
            {
              Index = match.Index,
              Content = sub,
              Type = CurrentRule!.TypeToAssign,
              Ignored = IgnoredToken,
              Exempt = ExemptAllWithin
            });
        }
      }
    }
  }

  internal void Tokens_Compete ()
  {
    Collection<(TokenRule<T> Rule, int Index)> contestants = [.. rules.Where(r => r.Type.HasFlag(RT.Competitive) && r.RuleStringData is not null).Select((r, i) => (r, i))];
    int contestant_count = contestants.Count;
    string regexPatterns = contestants.Select(r => GetRuleRegex(r.Rule, r.Index)).TextJoin("|");
    Regex regex = new(regexPatterns, ROEC | ROML);

    MatchCollection mc = regex.Matches(Input);

    foreach (Match match in mc)
    {
      int index = GetRuleGroupIndex(match);

      if (index == ErrVal)
      {
        Log(Area, "GetRuleGroupIndex Returned -1");
        continue;
      }
      TokenRule<T> cRule = contestants[index].Rule;
      Token<T> token = new()
      {
        Content = match.Value,
        Ignored = cRule.Type.HasFlag(RT.IgnoredToken),
        Exempt = cRule.Type.HasFlag(RT.ExemptAllWithin),
        Index = match.Index,
        Type = cRule.TypeToAssign
      };
      if (!token.Ignored)
        Result.Add(token);
      if (ExemptAllWithin)
        CannotMatch.Add(Section.ByLength(match.Index, match.Length, Input));
    }
  }

  public TokenCollection<T> Produce (string input)
  {
    Log(Area, "Produce", "Method Started", text: ConsoleColor.Blue);
    bool competed = false;
    input.ThrowIfNull();
    Input = input;
    Result = [];
    foreach (TokenRule<T> rule in rules)
    {
      Log(Area, "Rule processing started.");
      CurrentRule = new TokenRule<T>(rule.Type, rule.TypeToAssign, rule.RuleStringData);
      RT masked_type = rule.Type.RemoveBit<RT>(RT.FlagBits);

      if (Competes && !competed)
      {
        Log(Area, "Running competition.", text: ConsoleColor.Cyan);
        Tokens_Compete();
        competed = true;
        continue;
      }
      if (Competes)
      {
        Log(Area, "Already ran competition.", text: ConsoleColor.Yellow);
        continue;
      }

      switch (masked_type)
      {
        case RT.None:
          Log(Area, "Warning: Bad type defined");
          continue;
        case RT.TokenExact or RT.TokenMatch when FromTokens:
          Log(Area, "Token matching starting, from Tokens");
          Tokens_FromTokens();
          break;
        case RT.TokenExact or RT.TokenMatch when !FromTokens:
          Log(Area, "Token matching starting, from input string.");
          Tokens_FromInput(split: false);
          break;
        case RT.SplitMatch or RT.SplitExact:
          Log(Area, "Token splitting starting, from input string.");
          Tokens_FromInput(split: true);
          break;
        case RT.StoreExtra:
          Log(Area, $"Storing remaining zones matching {RuleData}");
          Tokens_StoreExtra();
          break;
        case RT.StoreOther:
          Log(Area, $"Storing remaining zones.");
          Tokens_StoreOther();
          break;
        default:
          Log(Area, "Bad rule type, skipping rule.");
          break;
      }
    }
    Result = [.. Result.OrderBy(item => item.Index)];
    return [.. Result];
  }
}
