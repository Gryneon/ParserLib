#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class TokenFactory
{
  private const string Area = "TokenFactory";
  private readonly IEnumerable<TokenRule> _rules;

  private string Input { get; set; } = SE;
  private SectionCollection CannotMatch { get; } = [];
  private readonly TokenCollection _result = [];
  private TokenRule? _currentRule;
  private readonly Spec _spec;

  public TokenFactory (IEnumerable<TokenRule> rules, Spec spec)
  {
    _spec = spec;
    _rules = rules;
  }
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
  private string AssignType => _currentRule?.TypeToAssign ?? SE;
  internal void SaveResult (Token token)
  {
    if (_result.Any(t => t.Index == token.Index))
    {
      throw new InvalidOperationException("Index already has a token!");
    }

    _result.Add(token);
  }
  internal Token MakeToken (string content, int index, TokenRule? rule = null) => new()
  {
    Index = index,
    Content = content,
    Type = (rule is null) ? AssignType : rule.TypeToAssign,
    Ignored = (rule is null) ? IgnoredToken : rule.Type.HasFlag(RT.IgnoredToken),
    Exempt = (rule is null) ? ExemptAllWithin : rule.Type.HasFlag(RT.ExemptAllWithin),
  };
  internal Token MakeToken (Match match, TokenRule? rule = null) => new()
  {
    Index = match.Index,
    Content = match.Value,
    Type = (rule is null) ? AssignType : rule.TypeToAssign,
    Ignored = (rule is null) ? IgnoredToken : rule.Type.HasFlag(RT.IgnoredToken),
    Exempt = (rule is null) ? ExemptAllWithin : rule.Type.HasFlag(RT.ExemptAllWithin),
  };
  internal Token MakeToken (Section match, TokenRule? rule = null) => new()
  {
    Index = match.Start,
    Content = match.Content,
    Type = (rule is null) ? AssignType : rule.TypeToAssign,
    Ignored = (rule is null) ? IgnoredToken : rule.Type.HasFlag(RT.IgnoredToken),
    Exempt = (rule is null) ? ExemptAllWithin : rule.Type.HasFlag(RT.ExemptAllWithin),
  };
  internal Token MakeToken (Capture capture, TokenRule? rule = null) => new()
  {
    Index = capture.Index,
    Content = capture.Value,
    Type = (rule is null) ? AssignType : rule.TypeToAssign,
    Ignored = (rule is null) ? IgnoredToken : rule.Type.HasFlag(RT.IgnoredToken),
    Exempt = (rule is null) ? ExemptAllWithin : rule.Type.HasFlag(RT.ExemptAllWithin),
  };
  public TokenCollection Produce (string input)
  {
    static void debug (string msg) => Log(MsgClass.Debug, Area, "Produce", msg);
    static void log (MsgClass type, string msg) => Log(type, Area, "Produce", msg);

    debug("Method Started");
    bool competed = false;
    input.ThrowIfNull();
    Input = input;
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
        log(MsgClass.Debug, "Already ran competition.");
        continue;
      }

      switch (masked_type)
      {
        case RT.None:
          log(MsgClass.Warning, "Warning: Bad type defined.");
          continue;
        case RT.TokenExact or RT.TokenMatch or RT.TokenExtract or RT.SplitMatch or RT.SplitExact when FromTokens:
          Log(Area, "Token matching starting, from Tokens");
          Tokens_FromTokens();
          break;
        case RT.TokenMatch or RT.TokenExtract or RT.SplitMatch when !FromTokens:
          Log(Area, "Token matching starting, from input string.");
          RegexMatch();
          break;
        case RT.TokenExact or RT.SplitExact when !FromTokens:
          Log(Area, "Token Exact starting, from input string.");
          ExactMatch();
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
    _result.SortByIndex();
    log(MsgClass.Critical, _result.ToString2());
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
    foreach (Token tokendata in _result.Cast<Token>())
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
      Token t = MakeToken(applicant);
      SaveResult(t);
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
          Token t = MakeToken(m.Value, applicant.Start + m.Index);
          SaveResult(t);
        }
      }
    }
  }
  internal void ExactMatch ()
  {
    int length = RuleData.Length > 0 ? RuleData.Length : throw new InvalidOperationException("RuleData has a length of 0 on an exact token.");
    int cursor = 0;
    int next = Input.IndexOf(RuleData, cursor, IC);

    while (next >= 0 && cursor <= Input.Length)
    {
      Section match = Section.ByLength(next, length, Input);
      if (!match.Overlaps(CannotMatch))
      {
        if (Type is RT.TokenExact)
        {
          SaveResult(MakeToken(match));
        }
        if (ExemptAllWithin || Type is RT.SplitExact)
          CannotMatch.Add(match);
      }
      next = Input.IndexOf(RuleData, cursor, IC);
    }
  }
  internal void RegexMatch ()
  {
    Regex regex = new(RuleData, _spec.RxOpt);

    MatchCollection mc = regex.Matches(Input);

    foreach (Match match in mc)
    {
      Section rng = Section.ByLength(match.Index, match.Length, Input);

      if (!rng.Overlaps(CannotMatch))
      {
        string sub = match.Value;
        if (ExemptAllWithin)
          CannotMatch.Add(rng);
        if (Type is RT.TokenExtract)
          foreach (Capture c in match.Groups["keep"].Captures)
          {
            _currentRule.ThrowIfNull();
            SaveResult(MakeToken(c));
          }
        else if (Type is RT.TokenMatch)
          SaveResult(MakeToken(rng));
      }
    }
  }
  internal void Tokens_Compete ()
  {
    Collection<(TokenRule Rule, int Index)> contestants = [.. _rules.Where(r => r.Type.HasFlag(RT.Competitive) && r.RuleStringData is not null).Select((r, i) => (r, i))];
    int contestant_count = contestants.Count;
    string regexPatterns = contestants.Select(r => GetRuleRegex(r.Rule, r.Index)).TextJoin("|");
    Regex regex = new(regexPatterns, _spec.RxOpt.RemoveBit<RegexOptions>(ROIC));

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
      Token token = MakeToken(match, cRule);
      if (!token.Ignored)
        SaveResult(token);
      if (token.Exempt)
        CannotMatch.Add(Section.ByLength(match.Index, match.Length, Input));
    }
  }
}
