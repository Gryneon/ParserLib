#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenFactory
{
  #region Private Fields
  private const string Area = "TokenFactory";
  private static string s_method = SE;
  private TokenRuleCollection _rules;
  private readonly TokenCollection _result = [];
  private TokenRule? _currentRule;
  private Spec _spec;
  private RT _default_rule;
  #endregion
  #region Public Properties
  public string Input { get; private set; } = SE;
  public SectionCollection CannotMatch { get; } = [];
  #endregion
  #region Constructors
  public TokenFactory (IEnumerable<TokenRule> rules, Spec spec, bool no_rules_from_spec = false)
  {
    SetSpec(spec);
    if (no_rules_from_spec)
    {
      _rules.Clear();
    }
    _rules.AddRange(rules);
  }
  public TokenFactory (Spec spec)
  {
    SetSpec(spec);
  }
  public TokenFactory ()
  {
    SetSpec(DefaultSpec.Unknown);
  }
  #endregion
  #region Private Helper Properties
  private bool IgnoreCase => (_currentRule?.Type.HasFlag(RT.IgnoreCase) ?? false) || _default_rule.HasFlag(RT.IgnoreCase);
  private StringComparison IC => IgnoreCase ? SCOIC : SCO;
  private bool Competes => (_currentRule?.Type.HasFlag(RT.Competitive) ?? false) || _default_rule.HasFlag(RT.Competitive);
  private bool IgnoredToken => (_currentRule?.Type.HasFlag(RT.IgnoredToken) ?? false) || _default_rule.HasFlag(RT.IgnoredToken);
  private bool FromTokens => (_currentRule?.Type.HasFlag(RT.FromTokens) ?? false) || _default_rule.HasFlag(RT.FromTokens);
  private bool ExemptAllWithin => (_currentRule?.Type.HasFlag(RT.ExemptAllWithin) ?? false) || _default_rule.HasFlag(RT.ExemptAllWithin);
  private RT Type => GetMaskedType(_currentRule?.Type ?? RT.None);
  private string RuleData => _currentRule?.RuleStringData ?? SE;
  private string AssignType => _currentRule?.TypeToAssign ?? SE;
  #endregion
  #region Private Logging Methods
  private static void DebugLog (string msg) => Debug.Log(MsgClass.Debug, Area, s_method, msg);
  private static void Log (MsgClass type, string msg) => Debug.Log(type, Area, s_method, msg);
  private static void WarnLog (string msg) => Debug.Log(MsgClass.Warning, Area, s_method, msg);
  private static void ErrorLog (string msg) => Debug.Log(MsgClass.Error, Area, s_method, msg); 
  #endregion
  private void SaveResult (Token token)
  {
    bool any = _result.Any(t => t.Index == token.Index);
    if (any)
    {
      IToken first = _result.First(t => t.Index == token.Index);
      throw new InvalidOperationException($"Index {token.Index} already has a token! ({first.Content}) adding ({token.Content})");
    }

    _result.Add(token);
  }
  [MemberNotNull(nameof(_spec), nameof(_default_rule), nameof(_rules))]
  public void SetSpec (Spec spec)
  {
    _spec = spec;
    _default_rule = _spec.DefaultRuleSet;
    _rules = _spec.TokenRules;
  }
  #region MakeToken
  internal Token MakeToken (string content, int index, TokenRule? rule = null) => new()
  {
    Index = index,
    Content = content,
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
  #endregion
  public TokenCollection Produce (string input)
  {
    s_method = "Produce";

    DebugLog("Method Started");
    bool competed = false;
    input.ThrowIfNull();
    Input = input;
    foreach (TokenRule rule in _rules)
    {
      DebugLog("Rule processing started.");
      _currentRule = rule;

      switch (Type)
      {
        case RT when Competes && !competed:
          DebugLog("Running competition.");
          Tokens_Compete();
          competed = true;
          break;
        case RT when Competes && competed:
          DebugLog("Already ran competition.");
          break;
        case RT.None:
          Log(MsgClass.Warning, "Warning: Bad type defined.");
          break;
        case RT.TokenExact or RT.TokenMatch or RT.TokenExtract or RT.SplitMatch or RT.SplitExact when FromTokens:
          DebugLog("Token matching starting, from Tokens");
          Tokens_FromTokens();
          break;
        case RT.TokenMatch or RT.TokenExtract or RT.SplitMatch when !FromTokens:
          DebugLog("Token matching starting, from input string.");
          RegexMatch();
          break;
        case RT.TokenExact or RT.SplitExact when !FromTokens:
          DebugLog("Token Exact starting, from input string.");
          ExactMatch();
          break;
        case RT.StoreExtra:
          DebugLog($"Storing remaining zones matching {RuleData}");
          Tokens_StoreExtra();
          break;
        case RT.StoreOther:
          //TODO: Fix this, very buggy.
          DebugLog($"Storing remaining zones.");
          Tokens_StoreOther();
          break;
        case RT.ErrorMatch:
          //TODO: Fix this, very buggy.
          DebugLog("Error Matching");
          break;
        default:
          WarnLog("Bad rule type, skipping rule.");
          break;
      }
    }
    _result.SortByIndex();
    Log(MsgClass.Critical, _result.ToString2());
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
  private static RT GetMaskedType (RT type) => type.RemoveBitLong<RT>(RT.FlagBits);
  private static int GetRuleGroupIndex (Match match)
  {
    s_method = "GetRuleGroupIndex";
    string num = match.Groups.
      AsReadOnly().
      First(static g => g.Name.StartsWith("_R", SCO) && g.Value.Length > 0).
      Name[2..];
    int result = int.TryParse(num, out int value) ? value : ErrVal;
    if (result == ErrVal)
    {
      ErrorLog( "GetRuleGroupIndex Returned -1");
    }
    return result;
  }
  private void Tokens_FromTokens ()
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
          tokendata.Type = AssignType;
          tokendata.Exempt = ExemptAllWithin;
        }
      }
      else if (Type is RT.TokenMatch)
      {
        if (Regex.Match(tokendata.Content, GetRuleRegex(_currentRule)).Length == tokendata.Content.Length)
        {
          tokendata.Type = AssignType;
          tokendata.Exempt = ExemptAllWithin;
        }
      }
      else if (Type is RT.TokenExtract)
      {
        Match m = Regex.Match(tokendata.Content, GetRuleRegex(_currentRule));
        if (m.Length == tokendata.Content.Length)
        {
          tokendata.Type = AssignType;
          tokendata.Exempt = ExemptAllWithin;
          tokendata.Content = m.Groups["keep"].Value;
        }
      }
    }
  }
  private void Tokens_StoreOther ()
  {
    foreach (Section applicant in CannotMatch.Inverse())
    {
      Debug.Log(Area, "Tokens_StoreOther", $"Section: {applicant} Found with no token.");
      CannotMatch.Add(Section.ByLength(applicant.Start, applicant.Length, Input));
      Token t = MakeToken(applicant);
      SaveResult(t);
    }
  }
  private void Tokens_StoreExtra ()
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
  private void ExactMatch ()
  {
    int length = RuleData.Length > 0 ? RuleData.Length : throw new InvalidOperationException("RuleData has a length of 0 on an exact token.");
    int cursor = 0;
    int next = Input.IndexOf(RuleData, cursor, IC);

    while (next >= 0 && cursor <= Input.Length)
    {
      Section match = Section.ByLength(next, length, Input);
      if (!CannotMatch.Overlaps(match))
      {
        if (Type is RT.TokenExact)
        {
          SaveResult(MakeToken(match));
        }
        if (ExemptAllWithin || Type is RT.SplitExact)
          CannotMatch.Add(match);
      }
      cursor = next + 1;
      next = Input.IndexOf(RuleData, cursor, IC);
    }
  }
  private void RegexMatch ()
  {
    Regex regex = new(RuleData, _spec.RxOpt);

    MatchCollection mc = regex.Matches(Input);

    foreach (Match match in mc)
    {
      Section rng = new(match, Input);

      if (!CannotMatch.Overlaps(rng))
      {
        if (Type is RT.TokenExtract)
          foreach (Section c in match.Groups["keep"].Captures.Select(c => new Section(c, Input)))
            SaveResult(MakeToken(c));

        else if (Type is RT.TokenMatch)
          SaveResult(MakeToken(rng));

        if (ExemptAllWithin)
          CannotMatch.Add(rng);
      }
    }
  }
  private void Tokens_Compete ()
  {
    Collection<(TokenRule Rule, int Index)> contestants = [.. _rules.Where(r => r.Type.HasFlag(RT.Competitive) && r.RuleStringData is not null).Select((r, i) => (r, i))];
    string regexPatterns = contestants.Select(r => GetRuleRegex(r.Rule, r.Index)).TextJoin("|");
    Regex regex = new(regexPatterns, _spec.RxOpt.RemoveBit<RegexOptions>(ROIC));

    MatchCollection mc = regex.Matches(Input);
    foreach (Match match in mc)
    {
      int index = GetRuleGroupIndex(match);
      Section rng = new(match, Input);

      _currentRule = contestants[index].Rule;

      if (!IgnoredToken)
        SaveResult(MakeToken(rng));

      if (ExemptAllWithin)
        CannotMatch.Add(rng);
    }
  }
}
