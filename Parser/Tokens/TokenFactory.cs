#pragma warning disable CA1710 // Identifiers should have correct suffix

using MC = Common.MsgClass;

namespace Parser.Tokens;

public class ErrorPlacement
{
  public MC McPrev;
  public MC McOuter;
  public MC McInner;
  public string PrevLine;
  public string ErrorLine;
  public int StartLine;
  public int StartOuter;
  public int PointCol;
  public int StartInner;
  public int EndInner;
  public int EndOuter;
  public void WriteError ()
  {
    WritePrevLine();
    WriteErrorLine();
    WritePointLine();
  }
  public void WritePrevLine ()
  {

  }
  public void WriteErrorLine ()
  {
    LogHead(MC.Debug);
    LogPart(McPrev, $"> ");
    if (StartOuter == StartLine)
    {
      LogPart(McOuter, ErrorLine[0..StartInner]);
    }
    else
    {
      LogPart(McPrev, ErrorLine[0..StartOuter]);
      LogPart(McOuter, ErrorLine[StartOuter..StartInner]);
    }
    LogPart(McInner, ErrorLine[StartInner..EndInner]);
    if (EndOuter == ErrorLine.Length - 1)
    {
      LogPart(McOuter, ErrorLine[EndInner..]);
    }
    else
    {
      LogPart(McOuter, ErrorLine[EndInner..EndOuter]);
      LogPart(McPrev, ErrorLine[EndOuter..]);
    }
    NewLine();
  }
  public void WritePointLine ()
  {

  }
}

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
  private bool _competed;
  #endregion
  #region Public Properties
  public string Input { get; private set; } = SE;
  public SectionCollection CannotMatch { get; } = [];
  public bool PromptAfterEach { get; set; }
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
  public TokenFactory (Spec spec) => SetSpec(spec);
  public TokenFactory () => SetSpec(DefaultSpec.Unknown);
  #endregion
  #region Private Helper Properties
  private bool IgnoreCase => (_currentRule?.Type.HasFlag(RT.IgnoreCase) ?? false) || _default_rule.HasFlag(RT.IgnoreCase);
  private StringComparison IC => IgnoreCase ? SCOIC : SCO;
  private bool Competes => (_currentRule?.Type.HasFlag(RT.Competitive) ?? false) || _default_rule.HasFlag(RT.Competitive);
  private bool IgnoredToken => (_currentRule?.Type.HasFlag(RT.IgnoredToken) ?? false) || _default_rule.HasFlag(RT.IgnoredToken);
  private bool HasError => _currentRule?.Type.HasFlag(RT.Error) ?? false;
  private RT Type => GetMaskedType(_currentRule?.Type ?? RT.None);
  private string RuleData => _currentRule?.RuleStringData ?? SE;
  private string AssignType => _currentRule?.TypeToAssign ?? SE;
  private Action RuleAction => Type switch
  {
    RT when HasError => ActionInvalidLog,
    RT when Competes && !_competed => RunCompete,
    RT when Competes && _competed => ActionCompetedLog,
    RT.None => ActionBadLog,
    RT.TokenMatch or RT.TokenExtract or RT.SplitMatch => RegexMatch,
    RT.TokenExact or RT.SplitExact => ExactMatch,
    RT.StoreExtra => StoreExtra,
    RT.StoreOther => StoreOther,
    RT.ErrorMatch => ErrorMatch,
    _ => ActionBadLog
  };
  #endregion
  #region Private Logging Methods
  private static void DebugLog (string msg)
  {
    LogHead(MC.Debug, Area, s_method);
    LogPart(MC.Informational, msg);
    NewLine();
  }
  private static void WarnLog (string msg)
  {
    LogHead(MC.Debug, Area, s_method);
    LogPart(MC.Warning, msg);
    NewLine();
  }
  private static void ErrorLog (string msg) => Log(MC.Error, Area, s_method, msg);
  #endregion
  #region Private Static Methods
  private static string GetRuleRegex (TokenRule rule, int? index = null)
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
      ErrorLog("GetRuleGroupIndex Returned -1");
    }
    return result;
  }
  #endregion
  private void MakeAddToken (Section match, TokenRule? rule = null)
  {
    rule ??= _currentRule!;

    if (rule.Type.HasFlag(RT.IgnoredToken))
      return;

    Token token = new()
    {
      Index = match.Start,
      Content = match.Content,
      Type = rule.TypeToAssign,
    };
    bool any = _result.Any(t => t.Index == token.Index);
    if (any)
    {
      IToken first = _result.First(t => t.Index == token.Index);
      throw new InvalidOperationException($"Index {token.Index} already has a token! ({first.Content}) adding ({token.Content})");
    }

    _result.Add(token);
  }
  private void StoreOther ()
  {
    s_method = "StoreOther";
    DebugLog($"Storing remaining zones.");
    foreach (Section applicant in CannotMatch.Inverse())
    {
      DebugLog($"Section: {applicant} Found with no token.");
      CannotMatch.Add(Section.ByLength(applicant.Start, applicant.Length, Input));
      MakeAddToken(applicant);
    }
  }
  /// <summary>Checks every unmatched section for a match, and if said section matches, adds the token.</summary>
  /// <remarks>This will NOT see anything but the unmatched section, so any lookaheads or lookbehinds will FAIL.</remarks>
  private void StoreExtra ()
  {
    s_method = "StoreExtra";
    DebugLog($"Storing remaining zones matching {RuleData}");
    foreach (Section applicant in CannotMatch.Inverse())
    {
      if (Regex.IsMatch(applicant.Content, RuleData))
      {
        MatchCollection mc = Regex.Matches(applicant.Content, RuleData);
        foreach (Match m in mc)
        {
          Section tsec = Section.ByLength(applicant.Start + m.Index, m.Length, Input);
          CannotMatch.Add(tsec);
          MakeAddToken(tsec);
        }
      }
    }
  }
  /// <summary>Exact (no regex) match, uses the rule's ignore case property, not the spec's one.</summary>
  private void ExactMatch ()
  {
    DebugLog("Token Exact starting, from input string.");
    int length = 0;
    if (RuleData.Length > 0)
      length = RuleData.Length;
    else
      _ = Op.ThrowBadDef("RuleData has a length of 0 on an exact token.");

    int cursor = 0;
    int next = Input.IndexOf(RuleData, cursor, IC);

    while (next >= 0 && cursor <= Input.Length)
    {
      Section match = Section.ByLength(next, length, Input);
      if (!CannotMatch.Overlaps(match))
      {
        if (Type is RT.TokenExact)
        {
          MakeAddToken(match);
          CannotMatch.Add(match);
        }
      }
      cursor = next + 1;
      next = Input.IndexOf(RuleData, cursor, IC);
    }
  }
  private void RegexMatch ()
  {
    DebugLog("Token matching starting, from input string.");
    Regex regex = new(RuleData, IgnoreCase ? _spec.RxOpt | ROIC : _spec.RxOpt);

    MatchCollection mc = regex.Matches(Input);

    foreach (Match match in mc)
    {
      Section rng = new(match, Input);

      if (!CannotMatch.Overlaps(rng))
      {
        if (Type is RT.TokenExtract)
          foreach (Section c in match.Groups["keep"].Captures.Select(c => new Section(c, Input)))
            MakeAddToken(c);

        else if (Type is RT.TokenMatch)
          MakeAddToken(rng);

        CannotMatch.Add(rng);
      }
    }
  }
  private void ErrorMatch ()
  {
    DebugLog("Error matching starting, from input string.");
    Regex regex = new(RuleData, IgnoreCase ? _spec.RxOpt | ROIC : _spec.RxOpt);

    MatchCollection mc = regex.Matches(Input);
    bool failUponEnding = false;
    foreach (Match match in mc)
    {
      failUponEnding = true;

      int error_pos = match.Groups.ContainsKey("error_pos") ? match.Groups["error_pos"].Index : match.Index;
      int error_len = match.Groups.ContainsKey("error_pos") ? match.Groups["error_pos"].Length : match.Index;
      int error_surround_pos = match.Groups.ContainsKey("error_surround") ? match.Groups["error_surround"].Index : -1;
      int error_surround_len = match.Groups.ContainsKey("error_surround") ? match.Groups["error_surround"].Length : -1;
      (int err_line, int err_col) = Input.Get2DPosition(error_pos);
      string[] lines = Input.Split('\n');
      int line_max = lines.Length;
      WarnLog($"Error at line {err_line}, column {err_col}.");
      WarnLog($"  {(err_line > 0 ? lines[err_line - 1] : "*** FIRST LINE BELOW ***")}");
      if (error_surround_pos == -1)
      {
        LogHead(MC.Debug);
        LogPart(MC.Warning, $"> {lines[err_line][0..err_col]}");
        LogPart(MC.Critical, lines[err_line][err_col..(err_col + error_len - 1)]);
        LogPart(MC.Warning, lines[err_line][(err_col + error_len - 1)..]);
        NewLine();
      }
      string pushoff = new(' ', err_col < 1 ? 0 : err_col - 1);
      WarnLog($"  {pushoff}^");
    }

    if (failUponEnding)
    {
      _ = Op.ThrowBadResult("You must correct the above listed errors to parse this file.");
    }
  }
  private void RunCompete ()
  {
    DebugLog("Running competition.");
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
        MakeAddToken(rng);

      CannotMatch.Add(rng);
    }
    _competed = true;
  }
  private void ActionInvalidLog () => ErrorLog("Error: Invalid rule. Skipping rule.");
  private void ActionCompetedLog () => DebugLog("Already ran competition. Skipping rule.");
  private void ActionBadLog () => WarnLog("Warning: Bad type defined. Skipping rule.");
  #region Public Methods
  [MemberNotNull(nameof(_spec), nameof(_default_rule), nameof(_rules))]
  public void SetSpec (Spec spec)
  {
    _spec = spec;
    _default_rule = _spec.DefaultRuleSet;
    _rules = _spec.TokenRules;
  }
  public TokenCollection Produce (string input)
  {
    s_method = "Produce";
    _competed = false;
    DebugLog("Method Started");
    input.ThrowIfNull();
    Input = input;
    foreach (TokenRule rule in _rules)
    {
      _currentRule = rule;

      try { RuleAction.Invoke(); } catch (OperationException e) { LogException(e); }
    }
    _result.SortByIndex();
    return [.. _result];
  }
  #endregion
}
