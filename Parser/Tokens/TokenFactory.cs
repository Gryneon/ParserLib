#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class TokenFactory
{
  private const string Area = "TokenFactory";
  private readonly IEnumerable<TokenRule> _rules;

  private string Input { get; set; } = SE;
  private SectionCollection CannotMatch { get; } = [];
  private TokenCollection _result = [];
  private TokenRule? _currentRule;
  private readonly Spec? _spec;

  public TokenFactory (IEnumerable<TokenRule> rules) => _rules = rules;
  public TokenFactory (Spec spec)
  {
    _spec = spec;
    _rules = spec?.TokenRules ?? [];
  }

  private bool IgnoreCase => _currentRule?.Type.HasFlag(RT.IgnoreCase) ?? false;
  private StringComparison IC => IgnoreCase ? SCOIC : SCO;
  private bool Competes => _currentRule?.Type.HasFlag(RT.Competitive) ?? false;
  private bool IgnoredToken => _currentRule?.Type.HasFlag(RT.IgnoredToken) ?? false;
  private bool FromTokens => _currentRule?.Type.HasFlag(RT.FromTokens) ?? false;
  private bool ExemptAllWithin => _currentRule?.Type.HasFlag(RT.ExemptAllWithin) ?? false;
  private RT Type => GetMaskedType(_currentRule?.Type ?? RT.None);
  private string RuleData => _currentRule?.RuleStringData ?? SE;

  public TokenCollection Produce (string input)
  {
    static void debug (string msg) => Log(MsgClass.Debug, Area, "Produce", msg);
    static void log (MsgClass type, string msg) => Log(type, Area, "Produce", msg);

    debug("Method Started");
    bool competed = false;
    input.ThrowIfNull();
    Input = input;
    _result = [];
    foreach (TokenRule rule in _rules)
    {
      debug("Rule processing started.");
      _currentRule = rule;
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
        Log(MsgClass.Debug, Area, "Produce", "Already ran competition.");
        continue;
      }

      switch (masked_type)
      {
        case RT.None:
          log(MsgClass.Warning, "Warning: Bad type defined.");
          continue;
        case RT.TokenExact or RT.TokenMatch or RT.TokenExtract when FromTokens:
          Log(Area, "Token matching starting, from Tokens");
          Tokens_FromTokens();
          break;
        case RT.TokenExact or RT.TokenMatch or RT.TokenExtract when !FromTokens:
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
          //TODO: Fix this, very buggy.
          Log(Area, $"Storing remaining zones.");
          Tokens_StoreOther();
          break;
        case RT.ErrorMatch:
          //TODO: Fix this, very buggy.
          Log(Area, "Error Matching");
          break;
        default:
          Log(Area, "Bad rule type, skipping rule.");
          break;
      }
    }
    _result = [.. _result.OrderBy(item => item.Index)];
    return [.. _result];
  }
  public static string GetRuleRegex (TokenRule rule, int? index = null)
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
    foreach (IToken tokendata in _result.Cast<IToken>())
    {
      if (tokendata.Exempt)
        continue;

      if (_currentRule is null)
        break;

      if (Type is RT.TokenExact)
      {
        if (tokendata.Content.Equals(RuleData, IC))
        {
          tokendata.Type = _currentRule!.TypeToAssign;
        }
      }
      else if (Type is RT.TokenMatch)
      {
        if (Regex.Match(tokendata.Content, GetRuleRegex(_currentRule)).Length == tokendata.Content.Length)
        {
          tokendata.Type = _currentRule!.TypeToAssign;
          tokendata.Exempt = ExemptAllWithin;
        }
      }
      else if (Type is RT.TokenExtract)
      {
        Match m = Regex.Match(tokendata.Content, GetRuleRegex(_currentRule));
        if (m.Length == tokendata.Content.Length)
        {
          tokendata.Type = _currentRule!.TypeToAssign;
          tokendata.Exempt = ExemptAllWithin;
          tokendata.Content = m.Groups["keep"].Value;
        }
      }
    }
  }
  internal void Tokens_StoreOther ()
  {
    foreach (Section applicant in CannotMatch.Inverse())
    {
      Log(Area, "Tokens_StoreOther", $"Section: {applicant} Found with no token.");
      _result.Add(new IToken()
      {
        Index = applicant.Start,
        Ignored = IgnoredToken,
        Type = _currentRule!.TypeToAssign,
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
          _result.Add(new IToken()
          {
            Index = applicant.Start + m.Index,
            Content = m.Value,
            Ignored = IgnoredToken,
            Type = _currentRule!.TypeToAssign,
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
          IToken token = new()
          {
            Index = next,
            Content = sub,
            Type = _currentRule!.TypeToAssign,
            Ignored = IgnoredToken,
            Exempt = ExemptAllWithin
          };
          CannotMatch.Add(match);
          if (!split)
            _result.Add(token);
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
    else if (Type is RT.TokenMatch or RT.SplitMatch or RT.TokenExtract)
    {
      Regex regex = new(RuleData, ROEC | ROML);

      MatchCollection mc = regex.Matches(Input);

      foreach (Match match in mc)
      {
        Section rng = Section.ByLength(match.Index, match.Length, Input);

        if (!rng.Overlaps(CannotMatch))
        {
          string sub = match.Value;
          //TODO: TokenExtract must save all the captures as tokens.
          if (Type is RT.TokenExtract)
            sub = match.Groups["keep"].Value;
          if (ExemptAllWithin)
            CannotMatch.Add(rng);
          if (!split)
            _result.Add(new IToken()
            {
              Index = match.Index,
              Content = sub,
              Type = _currentRule!.TypeToAssign,
              Ignored = IgnoredToken,
              Exempt = ExemptAllWithin
            });
        }
      }
    }
  }

  internal void Tokens_Compete ()
  {
    Collection<(TokenRule Rule, int Index)> contestants = [.. _rules.Where(r => r.Type.HasFlag(RT.Competitive) && r.RuleStringData is not null).Select((r, i) => (r, i))];
    int contestant_count = contestants.Count;
    string regexPatterns = contestants.Select(r => GetRuleRegex(r.Rule, r.Index)).TextJoin("|");
    Regex regex = new(regexPatterns, ROEC | ROML | ROIPW);

    MatchCollection mc = regex.Matches(Input);

    foreach (Match match in mc)
    {
      int index = GetRuleGroupIndex(match);

      if (index == ErrVal)
      {
        Log(Area, "GetRuleGroupIndex Returned -1");
        continue;
      }
      TokenRule cRule = contestants[index].Rule;
      IToken token = new()
      {
        Content = match.Value,
        Ignored = cRule.Type.HasFlag(RT.IgnoredToken),
        Exempt = cRule.Type.HasFlag(RT.ExemptAllWithin),
        Index = match.Index,
        Type = cRule.TypeToAssign
      };
      if (!token.Ignored)
        _result.Add(token);
      if (ExemptAllWithin)
        CannotMatch.Add(Section.ByLength(match.Index, match.Length, Input));
    }
  }
}
