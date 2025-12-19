using Parser.Tokens.Raw;

namespace Parser.Ops.Text;

public class RawTokenizeOperation<T> : Operation where T : notnull
{
  private readonly IEnumerable<TokenRule<T>> _rules;

  public RawTokenizeOperation (IEnumerable<TokenRule<T>> rules)
  {
    _rules = rules;
  }

  protected override void Execute ()
  {
    TokenFactory<T> raw = new(_rules);

    if (CheckInput<string>(out string? casted))
    {
      Collection<Tokens.Raw.IToken<T>> tokens = [.. raw.Produce(casted)];
      WorkToReturn = tokens;
      Status = OpStatus.Pass;
    }
  }
}

public class GenericObjCreateOperation<T> : Operation where T : notnull
{
  private readonly IEnumerable<TokenGroupRule<T>> _rules;

  public GenericObjCreateOperation (IEnumerable<TokenGroupRule<T>> rules)
  {
    _rules = rules;
  }

  protected override void Execute ()
  {
    TokenAssembler<T> raw = new(_rules);

    if (base.CheckInput<IList<Tokens.Raw.IToken<T>>>(out IList<Tokens.Raw.IToken<T>>? list))
    {
      raw.Execute(list);
      WorkToReturn = list;
      Status = OpStatus.Pass;
    }
  }
}

/// <summary>
/// Tokenizes a collection of
/// </summary>
public class TokenizeOperation : Operation
{
  private readonly Collection<TokenData> _types;

  public TokenizeOperation (IEnumerable<string> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key)
  {
    types.ThrowIfNull();
    _types = [];
    foreach (string type in types)
    {
      _types.Add(new(type, type));
    }
  }
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key)
  {
    _types = [];
  }
  public TokenizeOperation (IEnumerable<TokenData> tokenData, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key)
  {
    _types = [.. tokenData];
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
    if (_types.Count == 0)
    {
      foreach (string type in Spec.AllTokens)
      {
        _types.Add(new(type, type));
      }
    }

    Collection<IToken> tokens = [];

    if (CheckInput(out IEnumerable<MatchDataSet>? mdds))
    {
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
    else
    {
      Log(Tokenize_Wrong_Type, WorkToReturn?.GetType().Name ?? SE);
      Status = OpStatus.FailBadInputType;
    }
  }
}

public class ValidateOperation (bool abort_on_fail, string key = "result") : Operation(key, SE)
{

  protected override void Execute ()
  {
    if (abort_on_fail)
    {
      //abort?
    }
  }
}
