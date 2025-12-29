#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenFactory<T> (IEnumerable<TokenRule<dynamic>> rules) where T : notnull
{
  internal const string Area = "TokenFactory<T>";
  internal string Input = SE;
  internal List<Section> CannotMatch = [];
  internal List<(Token<T> Token, bool Exempt)> Result = [];
  internal TokenRule<T>? CurrentRule;
  internal bool IgnoreCase => CurrentRule?.Type.HasFlag(RT.IgnoreCase) ?? false;
  internal StringComparison IC => IgnoreCase ? SCOIC : SCO;
  internal bool Competes => CurrentRule?.Type.HasFlag(RT.Competitive) ?? false;
  internal bool IgnoredToken => CurrentRule?.Type.HasFlag(RT.IgnoredToken) ?? false;
  internal bool FromTokens => CurrentRule?.Type.HasFlag(RT.FromTokens) ?? false;
  internal bool ExemptAllWithin => CurrentRule?.Type.HasFlag(RT.ExemptAllWithin) ?? false;
  internal RT Type => GetMaskedType(CurrentRule?.Type ?? RT.None);
  internal string RuleData => CurrentRule?.RuleStringData ?? SE;

  internal string GetMaskedInput ()
  {
    string nInput = Input;
    foreach (Section s in CannotMatch)
    {
      nInput = nInput.ReplaceRange(s.Start, s.Length, '\u2588');
    }
    return nInput;
  }

  public string GetRuleRegex (TokenRule<dynamic> rule, int? index = null)
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

  internal static RT GetMaskedType (RT type) => type.RemoveBit<RT>(RT.FromTokens | RT.ExemptAllWithin | RT.Competitive | RT.IgnoreCase | RT.IgnoredToken);
  internal static int GetRuleGroupIndex (Match match)
  {
    string num = match.Groups.
      AsReadOnly().
      First(static g => g.Name.StartsWith("_R", SCO) && g.Value.Length > 0).
      Name[2..];
    return int.TryParse(num, out int value) ? value : ErrVal;
  }
  internal static dynamic FixType (object type)
  {
    dynamic d = type;
    return type.GetType().IsAssignableTo(typeof(T)) ? d : throw new ArgumentException("", nameof(type));
  }

  internal void Tokens_FromTokens ()
  {
    foreach ((Token<T> Token, bool Exempt) tokendata in Result)
    {
      if (tokendata.Exempt)
        continue;

      if (Type is RT.TokenExact)
      {
        if (tokendata.Token.Content.Equals(RuleData, IC))
        {
          tokendata.Token.Type = CurrentRule!.TypeToAssign;
        }
      }
      else if (Type is RT.TokenMatch)
      {
        if (Regex.Match(tokendata.Token.Content, GetRuleRegex(CurrentRule!.Dynamic)).Length == tokendata.Token.Content.Length)
        {
          tokendata.Token.Type = CurrentRule!.TypeToAssign;
        }
      }
    }
  }
  internal void Tokens_StoreOther ()
  {

    foreach (Section applicant in Section.Inverse(Input.Length, CannotMatch))
    {
      Token<T> token = new()
      {
        Position = applicant.Start,
        Content = applicant.Content,
        Ignored = IgnoredToken,
        Type = CurrentRule!.TypeToAssign
      };
      Result.Add((token, ExemptAllWithin));
    }
  }
  internal void Tokens_StoreExtra ()
  {
    Collection<Section> applicants = Section.Inverse(Input.Length, CannotMatch);

    foreach (Section applicant in applicants)
    {
      if (Regex.Count(applicant.Content, RuleData) > 0)
      {
        MatchCollection mc = Regex.Matches(applicant.Content, RuleData);
        mc.ToList().ForEach(m =>
        {
          Token<T> token = new()
          {
            Position = applicant.Start + m.Index,
            Content = m.Value,
            Ignored = IgnoredToken,
            Type = CurrentRule!.TypeToAssign
          };
          Result.Add((token, ExemptAllWithin));
        });
      }

      Token<T> token = new()
      {
        Position = applicant.Start,
        Content = applicant.Content,
        Ignored = IgnoredToken,
        Type = CurrentRule!.TypeToAssign
      };
      Result.Add((token, ExemptAllWithin));
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
            Position = next,
            Content = sub,
            Type = CurrentRule!.TypeToAssign,
            Ignored = IgnoredToken
          };
          CannotMatch.Add(match);
          if (!split)
            Result.Add((token, ExemptAllWithin));
          cursor = next + length;
          next = Input.IndexOf(RuleData, cursor, IC);
        }
        else
        {
          cursor = next + 1;
          next = Input.IndexOf(RuleData, cursor, IC);
          Log("Exact Match Rejected");
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
          Token<T> token = new()
          {
            Position = match.Index,
            Content = sub,
            Type = CurrentRule!.TypeToAssign,
            Ignored = IgnoredToken
          };
          CannotMatch.Add(rng);
          if (!split)
            Result.Add((token, ExemptAllWithin));
        }
        else
        {
          Log("Match Rejected");
        }
      }
    }
  }

  internal void Tokens_Compete ()
  {
    Collection<(TokenRule<dynamic> Rule, int Index)> contestants = [.. rules.Where(r => r.Type.HasFlag(RT.Competitive) && r.RuleStringData is not null).Select((r, i) => (r, i))];
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
      TokenRule<dynamic> cRule = contestants[index].Rule;
      Token<T> token = new()
      {
        Content = match.Value,
        Ignored = cRule.Type.HasFlag(RT.IgnoredToken),
        Exempt = cRule.Type.HasFlag(RT.ExemptAllWithin),
        Position = match.Index,
        Type = cRule.TypeToAssign
      };
      Result.Add((token, cRule.Type.HasFlag(RT.ExemptAllWithin)));
    }
  }

  public TokenCollection<T> Produce (string input)
  {
    Log(Area, "Produce", "Method Started");
    bool competed = false;
    input.ThrowIfNull();
    Input = input;
    Result = [];
    foreach (TokenRule<dynamic> rule in rules)
    {
      CurrentRule = new TokenRule<T>(rule.Type, rule.TypeToAssign, rule.RuleStringData);
      RT masked_type = rule.Type.RemoveBit<RT>(RT.FromTokens | RT.ExemptAllWithin | RT.Competitive | RT.IgnoreCase | RT.IgnoredToken);

      if (Competes && !competed)
      {
        Log(Area, "Running competition.");
        Tokens_Compete();
        competed = true;
        continue;
      }
      if (Competes)
      {
        Log(Area, "Already ran competition.");
        continue;
      }

      switch (masked_type)
      {
        case RT.None:
          Log(Area, "Warning: Bad type defined");
          continue;
        case RT.TokenExact or RT.TokenMatch:
          if (FromTokens)
          {
            Log(Area, "Token matching starting, from Tokens");
            Tokens_FromTokens();
          }
          else
          {
            Log(Area, "Token matching starting, from input string.");
            Tokens_FromInput(split: false);
          }
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
      Console.Write(GetMaskedInput());
    }

    return [.. Result.Select(t => t.Token)];
  }
}
