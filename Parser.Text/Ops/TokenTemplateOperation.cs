#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Ops;

/// <summary>
/// TODO: This operation is buggy and not yet functional.
/// </summary>
public class TokenTemplateOperation : TextOperation
{
  private const string Area = "TokenTemplateOperation";

  //Collections
  protected Dictionary<string, TokenNodeGroup> RefGroups { get; } = [];

  public TokenTemplateOperation (Dictionary<string, string> template_definitions, string input_key = "tokens", string output_key = "tokens_templated") : base(input_key, output_key)
  {
    template_definitions.ThrowIfNull();
    foreach (KeyValuePair<string, string> kvp in template_definitions)
    {
      TokenNodeGroup group = TokenNodeFactory.GetTokenNodes(kvp.Value, out string? import_group);
      group.ImportGroup = import_group;
      RefGroups.Add(kvp.Key, group);
    }
    TokenState = new(this);
  }
  /// <inheritdoc/>
  /// <remarks>
  /// This operation checks for token groups and builds them.
  /// </remarks>

  #region Private Properties & Methods

  private TokenNodeGroup? ParentNode;
  [AllowNull] private Collection<IToken> Tokens { get; set; }
  private Stack<(TokenNodeGroup group, int token_index, int node_index, int option_index)> OptionIndexes { get; set; } = [];

  private int Depth { get; set; }
  private int TokenIndex { get; set; }
  private int OptionIndex { get; set; }
  private int NodeIndex { get; set; }
  private TokenNode? Node =>
    ParentNode?.Options[OptionIndex].Count is null or 0 ? null : (ParentNode?.Options[OptionIndex][NodeIndex]);
  private IToken Token => Tokens[TokenIndex];
  private string NodeKeyName = SE;
  private int NodeCount => ParentNode?.Options[OptionIndex].Count ?? 0;
  private State TokenState { get; set; }
  #endregion
  #region State Class
  internal sealed class State (TokenTemplateOperation operation)
  {
    public bool Match
    {
      get;
      set
      {
        if (value && !field)
        {
          InitialMatch = value;
          field = value;
        }
        else
        {
          field = value;
          InitialMatch = false;
        }
      }
    }
    public bool InitialMatch { get; set; }
    public bool CompleteOnNextMisMatch { get; set; }
    public int StartAt { get; set; }
    public int MatchStart { get; set; }

    public TokenTemplateOperation Operation { get; } = operation;

    public bool CanAdvanceNode => NodeIndex + 1 < Operation.NodeCount;
    public bool CanAdvanceOption => NodeIndex + 1 == Operation.NodeCount && OptionIndex + 1 < Operation.ParentNode?.OptionCount;
    public bool CanAdvanceToken => TokenIndex + 1 < Operation.Tokens.Count;
    public bool OneOrMany => Operation.Node?.IsOneOrMany ?? false;
    public bool Optional => Operation.Node?.IsOptional ?? false;
    public bool JumpBack => !CanAdvanceOption && AtEndOfOption && !Match;
    public bool Complete => AtEndOfOption && Match && !CanReduceDepth;
    public bool HasMoreOptions => Operation.ParentNode?.OptionCount > OptionIndex + 1;
    public bool AtEndOfOption => NodeIndex + 1 == Operation.NodeCount;
    public bool CanReduceDepth => Operation.Depth > 0;
    public bool ReduceDepth => AtEndOfOption && !Match && CanReduceDepth;
    public int MatchLength => TokenIndex - MatchStart + 1;

    public int NodeIndex => Operation.NodeIndex;
    public int OptionIndex => Operation.OptionIndex;
    public int TokenIndex => Operation.TokenIndex;
  }
  #endregion
  #region Private Methods
  /// <summary>
  /// Checks the current position for a match.
  /// </summary>
  private void CheckPosition ()
  {
    if (Tokens is null || ParentNode is null)
      return;

    bool token_index_valid = Tokens.Count > TokenIndex && TokenIndex >= 0;
    bool option_index_valid = ParentNode.OptionCount > OptionIndex && OptionIndex >= 0;

    if (!token_index_valid || !option_index_valid)
      return;

    DoCheck();

    Node.ThrowIfNull();

    if (TokenState.Match && TokenState.Complete)
    {
      CompleteMatch();
    }
  }
  private void CompleteMatch ()
  {
    IEnumerable<IToken>? sub = Tokens.Skip(TokenState.MatchStart).Take(TokenState.MatchLength);
    Tokens.RemoveCount(TokenState.MatchLength, TokenState.MatchStart);
    ParentNode.ThrowIfNull();
    IEnumerable<(string, string)>? properties =
      sub.
      Where(item => item.Type.Like(ParentNode.ImportGroup)).
      Select(item =>
          (item.Children.First(item => item.LinkNode?.ImportKey ?? false).Content,
          item.Children.First(item => item.LinkNode?.ImportValue ?? false).Content)
        );
    Token new_token = new(sub.Select(item => item.Content).TextJoin(), sub.First().Position, NodeKeyName)
    {
      FromNode = ParentNode.Parent is null ? ParentNode : ParentNode.Parent,
      Type = NodeKeyName,
      Properties = [.. properties.Select<(string, string), KeyValuePair<string, string>>((a) => new(a.Item1, a.Item2))]
    };
    new_token.Properties.AddRange(properties);
    new_token.Children.AddRange(sub);
    Tokens.Insert(TokenState.MatchStart, new_token);
    TokenState.StartAt = TokenState.MatchStart;
  }
  private void DoCheck ()
  {
    if (Node is null)
    {
      Debug.Log(Area, "Node is null. Something went wrong.");
      return;
    }

    switch (Node)
    {
      case TokenNodeRef node_ref:
        TokenState.Match = Token.Type.Like(node_ref.RefName);
        return;

      case TokenNodeBasic node_bas:
        TokenState.Match = Token.Content.Equals(node_bas.Literal, Parser.Spec.SC);
        return;

      case TokenNodeGroup node_grp:
        OptionIndexes.Push((ParentNode!, TokenIndex, NodeIndex, OptionIndex));
        TokenState.StartAt = TokenIndex;
        Depth++;
        ParentNode = node_grp;
        OptionIndex = 0;
        NodeIndex = 0;
        DoCheck();
        return;

      default:
        return;
    }
  }
  private void PopStack ()
  {
    (ParentNode, TokenIndex, NodeIndex, OptionIndex) = OptionIndexes.Pop();
    Depth--;

    if (TokenState.CanAdvanceOption)
    {
      NextOption();
    }
    else if (TokenState.CanReduceDepth)
    {
      PopStack();
    }
    else
    {
      SoftReset(false);
    }
  }
  private void NextOption ()
  {
    OptionIndex++;
    NodeIndex = 0;
  }
  private void SoftReset (bool pass)
  {
    TokenState.Match = false;
    TokenState.CompleteOnNextMisMatch = false;
    Depth = 0;
    TokenIndex = TokenState.MatchStart;
    TokenIndex += pass ? 0 : 1;
    TokenState.MatchStart = -1;
    TokenState.StartAt = TokenIndex;
    OptionIndex = 0;
    NodeIndex = 0;
    OptionIndexes.Clear();
    ParentNode = GetParent(ParentNode);
  }
  private static TokenNodeGroup GetParent (TokenNodeGroup? grp) => grp is null ? throw new ArgumentNullException(nameof(grp)) : grp.Parent is null ? grp : GetParent(grp.Parent);
  #endregion
  protected override void Execute ()
  {
    if (!CheckInput(out IEnumerable<IToken>? tokens))
      throw new InvalidOperationException();

    //Assign Tokens Once
    Tokens = [.. tokens];

    foreach (KeyValuePair<string, TokenNodeGroup> node in RefGroups)
    {
      //Assign ParentNode
      ParentNode = node.Value;
      NodeKeyName = node.Key;

      while (TokenIndex < Tokens.Count)
      {
        CheckPosition();

        if (TokenState.Match)
        {
          if (TokenState.InitialMatch)
          {
            TokenState.MatchStart = TokenIndex;
            TokenState.StartAt = TokenIndex;
          }
          if (TokenState.Complete)
          {
            CompleteMatch();
            SoftReset(true);
            continue;
          }
          if (TokenState.CanAdvanceNode)
          {
            // Also is one or many, sets complete on next mismatch
            if (TokenState.OneOrMany)
            {
              TokenState.CompleteOnNextMisMatch = true;
              TokenIndex++;
            }
            else // Not OneOrMany
            {
              NodeIndex++;
            }
            continue;
          }
          else //Cannot Advance Node
          {
            if (TokenState.CompleteOnNextMisMatch || TokenState.Optional)
            {
              CompleteMatch();
              SoftReset(true);
              continue;
            }
          }
        }
        else // Not Match
        {
          if (TokenState.CanAdvanceOption)
          {
            NextOption();
            continue;
          }
          if (Depth > 0) // Can Reduce Depth
          {
            PopStack();
            continue;
          }
          if (Depth == 0) // At Surface
          {
            SoftReset(false);
            continue;
          }
        }

        /*
        int last = pos + match;
        IEnumerable<IToken>? sub = MutableTokens.Skip(pos).Take(match);
        MutableTokens.RemoveCount(match, pos);
        Token new_token = new(sub.Select(item => item.Content).TextJoin(), sub.First().Position, node.Key)
        {
          FromNode = node.Value,
          Type = node.Key
        };
        IEnumerable<(string, string)>? properties = sub.
          Where(item => item.FromNode is TokenNodeRef node_ref && node_ref.RefName.Like(node.Value.ImportGroup)).
          Select(item => (
            item.Children.First(item => item.FromNode.ImportKey).Content,
            item.Children.First(item => item.FromNode.ImportValue).Content
          ));
        new_token.Properties.AddRange(properties);
        new_token.Children.AddRange(sub);
        MutableTokens.Insert(pos, new_token);
        pos = 0;
      }
        else
        pos++;
    }*/
      }
    }
  }
}
