using Parser.Tokens.Raw;

namespace Parser.Ops.Text;

/// <summary>
/// Tokenizes a string or list of strings.
/// <typeparamref name="T">The token type identifier. Enums and strings are supported.</typeparamref>
/// </summary>
public class TokenizeOperation<T> : Operation where T : notnull
{
  private const int
    TM_Marker = 1,
    TM_Rule = 2,
    TMF_UseSpec = 1024;
  private readonly Collection<TokenData> _types = [];
  private readonly Collection<TokenRule<dynamic>> _rules = [];
  private readonly int _mode = TM_Marker | TMF_UseSpec;

  public TokenizeOperation (IEnumerable<string> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key)
  {
    types.ThrowIfNull();
    foreach (string type in types)
    {
      _types.Add(new(type, type));
    }
    _mode = TM_Marker;
  }
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) { }
  public TokenizeOperation (IEnumerable<TokenData> tokenData, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key)
  {
    _types = [.. tokenData];
    _mode = TM_Marker;
  }
  public TokenizeOperation (IEnumerable<TokenRule<dynamic>> rules, string input_key = "text", string output_key = "tokens")
  {
    _rules = [.. rules];
    _mode = TM_Rule;
  }
  public TokenizeOperation (int special_case_id, string input_key = "text", string output_key = "tokens")
  {
    _rules = [];
    _mode = TM_Rule | TMF_UseSpec;
  }

  internal TokenData? GetTokenData (MatchDataSet mds)
  {
    foreach (TokenData t in _types)
    {
      if (mds.UsesGroupDefinitions)
      {
        if (mds.HasMarker(t.RequiredMarker))
        {
          return t;
        }
      }
      else
      {
        if (mds.HasGroup(t.RequiredMarker))
        {
          return t;
        }
      }
    }
    return null;
  }

  internal string? GetTokenType (MatchDataSet mds)
  {
    TokenData? t = GetTokenData(mds);

    return t?.TypeToAssign;
  }
  internal bool IsIgnored (MatchDataSet mds)
  {
    TokenData? t = GetTokenData(mds);

    return t != null && t.Value.Ignored;
  }
  protected override void Execute ()
  {
    // Load Tokens if loading from Spec
    if (_mode.RemoveBit<int>(TMF_UseSpec) is TM_Marker && _mode.HasFlag(TMF_UseSpec))
    {
      foreach (string type in Spec.AllTokens)
      {
        _types.Add(new(type, type));
      }
    }
    else if (_mode.RemoveBit<int>(TMF_UseSpec) is TM_Rule && _mode.HasFlag(TMF_UseSpec))
    {
      _rules.AddRange(Spec.TokenRules);
    }

    // Process Tokens with marker method, or rule method.
    if (_mode.HasFlag(TM_Marker) && CheckInput(out IEnumerable<MatchDataSet>? mdds))
    {
      Collection<IToken> tokens = [];
      foreach (MatchDataSet mdd in mdds)
      {
        RegexToken token = new(mdd, GetTokenType(mdd) ?? SE);
        if (Spec.WhitespaceTokens.Contains(token.Type))
        {
          token.IsIgnored = true;
          Log(Tokenize_Ignore_Token, mdd.Content.Replace(["\r\n", "\n", "\r"], "<NL>"));
        }
        else
        {
          tokens.Add(token);
          Log(Tokenize_Token_Added, token.Type, token.Content ?? "<NULL DATA>");
        }
      }
      WorkToReturn = tokens;
      Status = OpStatus.Pass;
    }
    else if (_mode.HasFlag(TM_Rule) && CheckInput(out string? input))
    {
      TokenFactory<T> factory = new(_rules);
      TokenCollection<T> return_tokens = [.. factory.Produce(input)];
      WorkToReturn = return_tokens;
      Status = OpStatus.Pass;
    }
    else
    {
      Log(Tokenize_Wrong_Type, WorkToReturn?.GetType().Name ?? SE);
      Status = OpStatus.FailBadInputType;
    }
  }
}
