#pragma warning disable IDE1006 // Naming Styles

using System.Collections;

namespace Parser.Text.Tokens;

public sealed class TokenNodeLibrary
{
  public Dictionary<string, TokenNodeGroup> Library { get; } = [];
}

/// <summary>
/// A group of token nodes.
/// </summary>
public sealed class TokenNodeGroup : TokenNode, ICollection<TokenNode>
{
  public Collection<TokenNode> Items { get; } = [];
  public Collection<Collection<TokenNode>> Options { get; } = [];
  public int OptionCount => Options.Count + (Items.Count > 0 ? 1 : 0);
  public string? ImportGroup { get; set; }
  public TokenNodeGroup () => Type = TokenNodeType.Group;
  public void Add (TokenNode item)
  {
    if (item is null)
    {
      Debug.Log("Null Item");
      return;
    }
    if (item.Type is TokenNodeType.Or)
      AddOption();
    else
      Items.Add(item);
  }
  public void AddOption ()
  {
    if (Items.Count > 0)
    {
      Options.Add([.. Items]);
      Items.Clear();
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
  public override int CheckForMatch (IEnumerable<IToken> tokens, int start_at)
  {
    Tokens.Clear();
    Tokens.AddRange(tokens);
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

  public void Clear () => ((ICollection<TokenNode>) Items).Clear();
  public bool Contains (TokenNode item) => ((ICollection<TokenNode>) Items).Contains(item);
  public void CopyTo (TokenNode[] array, int arrayIndex) => ((ICollection<TokenNode>) Items).CopyTo(array, arrayIndex);
  public bool Remove (TokenNode item) => ((ICollection<TokenNode>) Items).Remove(item);
  public IEnumerator<TokenNode> GetEnumerator () => ((IEnumerable<TokenNode>) Items).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) Items).GetEnumerator();

  public override bool Match => TokenConsumption >= 0;

  public int Count => ((ICollection<TokenNode>) Items).Count;

  public bool IsReadOnly => ((ICollection<TokenNode>) Items).IsReadOnly;
}
