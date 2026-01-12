namespace Parser.Ops.Text;

/// <summary>
/// Tokenizes a string or list of strings.
/// <typeparamref name="T">The token type identifier. Must be a struct (Enum).</typeparamref>
/// </summary>
public class TokenizeOperation<T> : Operation where T : struct
{
  private const int
    TM_Marker = 1,
    TM_Rule = 2,
    TMF_UseSpec = 1024;
  private readonly TokenRuleCollection<T> _rules = [];
  private readonly int _mode = TM_Marker | TMF_UseSpec;

  public TokenizeOperation (IEnumerable<TokenRule<T>> rules, string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    _rules = [.. rules];
    _mode = TM_Rule;
  }
  public TokenizeOperation (string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    _rules = [];
    _mode = TM_Rule | TMF_UseSpec;
  }

  protected override void Execute ()
  {
    // Load Tokens if loading from Spec
    if (_mode.RemoveBit<int>(TMF_UseSpec) is TM_Rule && _mode.HasFlag(TMF_UseSpec))
    {
      _rules.AddRange(Spec.TokenRules);
    }

    // Process Tokens with marker method, or rule method.
    if (_mode.HasFlag(TM_Marker))
    {
      Log(MsgClass.Warning, "TokenizeOperation", "Execute", "Deprecated Operation Function.");
      Status = OpStatus.FailBadOpDefinition;
    }
    else if (_mode.HasFlag(TM_Rule) && CheckInput(out string? input))
    {
      TokenFactory factory = new(_rules);
      TokenCollection return_tokens = [.. factory.Produce(input)];
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

/// <summary>Tokenizes a string or list of strings.</summary>
public class TokenizeOperation : Operation
{
  private const int
    TM_Marker = 1,
    TM_Rule = 2,
    TMF_UseSpec = 1024;
  private readonly TokenRuleCollection _rules = [];
  private readonly int _mode = TM_Marker | TMF_UseSpec;

  public TokenizeOperation (IEnumerable<TokenRule> rules, string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    _rules = [.. rules];
    _mode = TM_Rule;
  }
  public TokenizeOperation (string input_key = "text", string output_key = "tokens") : base(input_key, output_key)
  {
    _rules = [];
    _mode = TM_Rule | TMF_UseSpec;
  }

  protected override void Execute ()
  {
    // Load Tokens if loading from Spec
    if (_mode.RemoveBit<int>(TMF_UseSpec) is TM_Rule && _mode.HasFlag(TMF_UseSpec))
    {
      _rules.AddRange(Spec.TokenRules);
    }

    // Process Tokens with marker method, or rule method.
    if (_mode.HasFlag(TM_Marker))
    {
      Log(MsgClass.Warning, "TokenizeOperation", "Execute", "Deprecated Operation Function.");
      Status = OpStatus.FailBadOpDefinition;
    }
    else if (_mode.HasFlag(TM_Rule) && CheckInput(out string? input))
    {
      TokenFactory factory = new(_rules);
      TokenCollection return_tokens = [.. factory.Produce(input)];
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
