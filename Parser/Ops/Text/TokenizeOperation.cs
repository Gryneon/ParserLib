namespace Parser.Ops.Text;

/// <summary>
/// Tokenizes a collection of
/// </summary>
public class TokenizeOperation : Operation
{
  private readonly IEnumerable<string>? _types;

  public TokenizeOperation (IEnumerable<string> types, string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = types;
  public TokenizeOperation (string input_key = "matches", string output_key = "tokens") : base(input_key, output_key) => _types = null;
  protected override void Execute ()
  {
    Collection<IToken> tokens = [];
    IEnumerable<string> types = _types ?? Spec.AllTokens;

    if (CheckInput(out IEnumerable<MatchDataSet>? mdds))
    {
      foreach (MatchDataSet mdd in mdds)
      {
        string type = types.First(mdd.HasMarker);
        Token? token = new(mdd, type);
        if (!Spec.WhitespaceTokens.Contains(token.Type))
          tokens.Add(token);
        else
          Log(Tokenize_Ignore_Token, mdd.Content.Replace(["\r\n", "\n", "\r"], "<NL>"));
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

/// <summary>
/// Takes an input as a collection of tokens,
/// and assigns a depth value to them. 
/// </summary>
/// <remarks><code>
/// Inputs: <see langword="string"/>, <see cref="IEnumerable{T}"/>(<see langword="string"/>)<br/>
/// Output: <c>Same as input</c>
/// </code><br/>
/// Statuses:
/// <code>
/// <see cref="OpStatus.Pass"/>: Operation completed successfully.
/// <see cref="OpStatus.FailBadInputType"/>: Operation was provided the wrong type as input.
/// <see cref="OpStatus.FailBadInputNull"/>: The data at the key was <see langword="null"/> or missing.
/// </code>
/// </remarks>
public class DepthOperation : Operation
{
  private Collection<DepthMarker> Markers { get; set; }

  public DepthOperation (IEnumerable<DepthMarker> markers, string input_key = "tokens", string output_key = "tokens") : base(input_key, output_key)
  {
    Markers = [.. markers];
  }

  protected override void Execute ()
  {
    if (CheckArray(out IEnumerable<IToken>? casted))
    {
      Collection<IToken> tokens = [.. casted];
      DepthMarker? openMatch = null;
      DepthMarker? closeMatch = null;
      List<DepthMarker> stack = [];
      int depth = 0;
      foreach (IToken token in tokens)
      {
        void tryMatch ()
        {
          Collection<DepthMarker> matches = [.. Markers.Where(item => item.Open.Equals(token.Content, Spec.SC))];
          openMatch = !matches.IsEmpty() ? matches[0] : null;
          matches = [.. Markers.Where(item => item.Close.Equals(token.Content, Spec.SC))];
          closeMatch = !matches.IsEmpty() ? matches[0] : null;
        }

        tryMatch();
        if (closeMatch is not null && stack.Count > 0 && stack.Peek().Close.Equals(token.Content, Spec.SC) && closeMatch.Value.AscendAfterToken)
        {
          stack.Drop();
          token.Depth = depth;
          depth--;
        }
        else if (closeMatch is not null && stack.Count > 0 && stack.Peek().Close.Equals(token.Content, Spec.SC))
        {
          depth--;
          stack.Drop();
          token.Depth = depth;
        }
        else if (openMatch is not null)
        {
          if (openMatch.Value.AscendAfterToken)
          {
            depth++;
            token.Depth = depth;
          }
          else
          {
            token.Depth = depth;
            depth++;
          }
          stack.Add(openMatch.Value);
        }
        else
        {
          token.Depth = depth;
        }
      }
      WorkToReturn = tokens;
      Status = OpStatus.Pass;
    }
  }
}
