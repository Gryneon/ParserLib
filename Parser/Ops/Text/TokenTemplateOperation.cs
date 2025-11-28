#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Ops.Text;

/// <summary>
/// TODO: This operation is buggy and not yet functional.
/// </summary>
public class TokenTemplateOperation : Operation
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
      group.ImportGroupName = import_group;
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
    ParentNode?.Options[OptionIndex].Count is null or 0 ? null :
    ParentNode.Options[OptionIndex][NodeIndex];
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
        //if it Goes from false to true, set InitialMatch
        if (value && !field)
          InitialMatch = value;
        field = value;
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
  }
  private static void AssignProperties (IParentToken parent, IEnumerable<IToken>? children)
  {
    TokenNode? this_node = parent.LinkNode;
    bool do_command_property_copy = this_node?.ImportGroup ?? false;

    Collection<string> keys = [];
    Collection<string> values = [];

    if (do_command_property_copy && children is not null)
    {
      foreach (IToken child in children)
      {
        TokenNode? child_node = child.LinkNode;

        if (child_node is null)
        {
          Log(Area, "Child Node was null when assigning properties.");
          continue;
        }

        if (child.Content is null)
        {
          Log(Area, "Child Content was null when assigning properties.");
          continue;
        }

        if (child_node.ImportKey) keys.Add(child.Content);
        if (child_node.ImportValue) values.Add(child.Content);
      }

      IEnumerable<(string, string)> zipped = Enumerable.Zip(keys, values);
      parent.Properties.AddRange(zipped);
    }
  }
  private void CompleteMatch ()
  {
    IEnumerable<IToken>? sub = Tokens.Skip(TokenState.MatchStart).Take(TokenState.MatchLength);
    Tokens.RemoveCount(TokenState.MatchLength, TokenState.MatchStart);
    ParentNode.ThrowIfNull();

    IParentToken new_token = new ParentToken(ParentNode.Parent is null ? ParentNode : ParentNode.Parent, sub, NodeKeyName)
    {
      Properties = [],
    };
    AssignProperties(new_token, sub);
    Tokens.Insert(TokenState.MatchStart, new_token);
    TokenState.StartAt = TokenState.MatchStart;
  }
  private void DoCheck ()
  {
    if (Node is null)
    {
      Log(Area, "Node is null. Something went wrong.");
      return;
    }

    switch (Node)
    {
      case TokenNodeRef node_ref:
        TokenState.Match = Token.Type.Like(node_ref.RefName);
        return;

      case TokenNodeBasic node_bas:
        TokenState.Match = Token?.Content?.Equals(node_bas.Literal, Spec.SC) ?? false;
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
    TokenIndex = TokenState.MatchStart == -1 ? TokenIndex : TokenState.MatchStart;
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
            TokenState.InitialMatch = false;
            //No Continue.
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
      }
    }

    WorkToReturn = Tokens;
    Status = OpStatus.Pass;
  }
}
