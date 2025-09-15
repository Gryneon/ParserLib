#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Tokens;

public abstract class TokenNode : IGeneratable<MatchData, TokenNode>
{
  #region Public Properties
  public TokenNodeType Type { get; set; }
  public bool IsOptional { get; set; }
  public bool IsOneOrMany { get; set; }
  public bool IsAny { get; set; }
  public bool IsIgnored =>
    this is TokenNodeBaseRx tnbr && TokenOptions.ActiveSpec.WhitespaceTokens.Contains(tnbr.RefName) ||
    this is TokenNodeRef tnr && TokenOptions.ActiveSpec.WhitespaceTokens.Contains(tnr.RefName);
  public TokenNodeGroup? Parent { get; set; }
  #endregion
  #region Protected Properties
  protected Collection<IToken> Tokens { get; set; } = [];
  protected int StartAt { get; set; }
  protected int Consumed { get; set; }
  protected bool IsMoreTokens => StartAt + Consumed < Tokens.Count;
  protected IToken CurrentToken => Tokens[StartAt];

  protected void ValidateStart ()
  {
    if (StartAt < 0 || StartAt >= Tokens.Count)
    {
      Debug.Log("TokenNode", "ValidateStart", "StartAt is out of range.");
      throw new IndexOutOfRangeException();
    }
  }

  /// <summary>
  /// Sets <c>Consumed</c> and honors <c>IsOptional</c> and <c>IsOneOrMany</c> node options.
  /// </summary>
  protected void Consume ()
  {
    if (!Match && IsOptional)
    {
      Consumed = 0;
      return;
    }
    if (Match)
    {
      while (IsMoreTokens && IsOneOrMany && Match)
        Consumed++;

      return;
    }

    Consumed = DNE;
  }
  protected int CheckForMatchSingle (IEnumerable<IToken> pTokens, int start_at)
  {
    Tokens = [.. pTokens];
    StartAt = start_at;
    Consumed = 0;

    ValidateStart();
    Consume();
    return Consumed;
  }

  #endregion
  public static TokenNode? Generate (MatchData input)
  {
    if (input.DoesNotHaveGroup("line"))
      return null;

    else if (input.HasGroup("gp_start"))
      return new TokenNodeBasic(TokenNodeType.GroupSt, "(");

    else if (input.HasGroup("gp_end"))
      return new TokenNodeBasic(TokenNodeType.GroupEn, ")");

    else if (input.HasGroup("or"))
      return new TokenNodeBasic(TokenNodeType.Or, "|");

    else if (input.HasGroup("literal"))
      return new TokenNodeBasic(TokenNodeType.Literal, input["literal"].Content);

    else if (input.HasGroup("opt"))
      return new TokenNodeBasic(TokenNodeType.Opt, "?");

    else if (input.HasGroup("more"))
      return new TokenNodeBasic(TokenNodeType.More, "+");

    else if (input.HasGroup("base"))
      return new TokenNodeBasic(TokenNodeType.More, "+");

    return null;
  }
  public virtual int CheckForMatch (IEnumerable<IToken> tokens, int start_at) => CheckForMatchSingle(tokens, start_at);
  public abstract bool Match { get; }
}
