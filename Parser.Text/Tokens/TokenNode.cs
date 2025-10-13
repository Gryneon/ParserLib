#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA2227 // Collection properties should be read only

namespace Parser.Text.Tokens;

public abstract class TokenNode : IGeneratable<MatchDataSet, TokenNode>
{
  #region Public Properties
  public TokenNodeType Type { get; set; }
  public bool ImportKey { get; set; }
  public bool ImportValue { get; set; }
  public bool IsOptional { get; set; }
  public bool IsOneOrMany { get; set; }
  public bool IsAny { get; set; }
  public bool IsIgnored =>
    this is TokenNodeRef tnbr && Spec.Active is TextSpec ts && ts.WhitespaceTokens.Contains(tnbr.RefName);
  public TokenNodeGroup? Parent { get; set; }
  public string? CommandString { get; set; }
  public MatchDataSet? ParseData { get; private set; }
  #endregion
  #region Protected Properties
  protected Collection<IToken> Tokens { get; } = [];
  protected int StartAt { get; set; }
  protected int Consumed { get; set; }
  protected bool IsMoreTokens => StartAt + Consumed < Tokens.Count;
  protected IToken? CurrentToken => Tokens[StartAt];

  protected void ValidateStart ()
  {
    if (StartAt < 0 || StartAt >= Tokens.Count)
    {
      Debug.Log("TokenNode", "ValidateStart", "StartAt is out of range.");
      throw new ArgumentOutOfRangeException();
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
    Tokens.Clear();
    Tokens.AddRange(pTokens);
    StartAt = start_at;
    Consumed = 0;

    ValidateStart();
    Consume();
    return Consumed;
  }

  #endregion
  public static TokenNode? Generate (MatchDataSet input)
  {
    input.ThrowIfNull();

    if (input.DoesNotHaveGroup("line"))
      return null;

    TokenNode? tokenNode = null;

    if (input.HasGroup("gp_start"))
      tokenNode = new TokenNodeBasic(TokenNodeType.GroupSt, "(");

    else if (input.HasGroup("gp_end"))
      tokenNode = new TokenNodeBasic(TokenNodeType.GroupEn, ")");

    else if (input.HasGroup("or"))
      tokenNode = new TokenNodeBasic(TokenNodeType.Or, "|");

    else if (input.HasGroup("literal"))
      tokenNode = new TokenNodeBasic(TokenNodeType.Literal, input["literal"].Content);

    else if (input.HasGroup("opt"))
      tokenNode = new TokenNodeBasic(TokenNodeType.Opt, "?");

    else if (input.HasGroup("more"))
      tokenNode = new TokenNodeBasic(TokenNodeType.More, "+");

    else if (input.HasGroup("any"))
      tokenNode = new TokenNodeBasic(TokenNodeType.Any, "*");

    else if (input.HasGroup("command"))
      tokenNode = new TokenNodeCommand(input["command"].Content);

    else if (input.HasGroup("base"))
      tokenNode = new TokenNodeRef(input["base"].Content);

    else if (input.HasGroup("ref"))
      tokenNode = new TokenNodeRef(input["ref"].Content);

    if (tokenNode is null)
      return null;

    tokenNode.ParseData = input;

    return tokenNode;
  }
  public virtual int CheckForMatch (IEnumerable<IToken> tokens, int start_at) => CheckForMatchSingle(tokens, start_at);
  public abstract bool Match { get; }

  public override string ToString () =>
    $"Token Node:\n\t" +
      $"Start At:{StartAt}\n\t" +
      $"Type:{Type}\n\t" +
      $"Child Count:{Tokens.Count}" +
      $"{(CommandString is null ? "" : "\n\tCommandString:" + CommandString)}";
}
