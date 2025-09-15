#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Tokens;

/// <summary>
/// A group of token nodes.
/// </summary>
public sealed class TokenNodeGroup : TokenNode
{
  public Collection<TokenNode> Items { get; set; } = [];
  public Collection<Collection<TokenNode>> Options { get; set; } = [];
  public int OptionCount => Options.Count + (Items.Count > 0 ? 1 : 0);
  public TokenNodeGroup () => Type = TokenNodeType.Group;
  public void Add (TokenNode item)
  {
    if (item.Type is TokenNodeType.Or)
      AddOption();
    else
      Items.Add(item);
  }
  public void AddOption ()
  {
    if (Items.Count > 0)
    {
      Options.Add(Items);
      Items = [];
    }
  }

  #region Private Properties & Methods
  private int OptionIndex { get; set; }
  private int NodeIndex { get; set; }
  private TokenNode Node => Options[OptionIndex][NodeIndex];
  private int GetConsumed () => HasMoreTokens ? Node.CheckForMatch(Tokens, StartAt + Consumed) : Node.IsOptional ? 0 : DNE;
  private int NodeCount => Options[OptionIndex].Count;
  private bool HasMoreOptions => Options.Count > OptionIndex + 1;
  private bool HasMoreTokens => StartAt + Consumed < Tokens.Count;
  private bool IsNotAMatch => TokenConsumption == DNE;
  private int TokenConsumption { get; set; }
  private void GotoNextOption ()
  {
    OptionIndex++;
    NodeIndex = 0;
    Consumed = 0;
  }
  private void ConsumeNextNode ()
  {
    Consumed += TokenConsumption;
    NodeIndex++;
  }
  #endregion
  public override int CheckForMatch (IEnumerable<IToken> pTokens, int start_at)
  {
    Tokens = [.. pTokens];
    OptionIndex = 0;
    Consumed = 0;
    NodeIndex = 0;
    StartAt = start_at;

    ValidateStart();

    while (NodeIndex < NodeCount)
    {
      TokenConsumption = GetConsumed();

      if (IsNotAMatch && HasMoreOptions)
        GotoNextOption();
      else if (IsNotAMatch && IsOptional)
        return 0;
      else if (IsNotAMatch)
        return DNE;
      else if (Match)
        ConsumeNextNode();
    }
    return Consumed;
  }
  public override bool Match => TokenConsumption >= 0;
}
