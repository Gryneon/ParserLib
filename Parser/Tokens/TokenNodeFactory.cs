#pragma warning disable IDE1006 // Naming Styles

using static Parser.DefinitionStaticFunctions;

namespace Parser.Tokens;

/// <summary>A factory that produces <see cref="TokenNode"/> objects.</summary>
public static class TokenNodeFactory
{
  private const string Area = "TokenNodeFactory";

  public static readonly RxS Regex = Rx(
"""
(?'line'
  '(?'literal'(?:[^']|'')+?)'|
  \$(?'base'[\w_]+)|
  \#(?'ref'[\w_]+)|
  (?'gp_start'\()|
  (?'gp_end'\))|
  (?'opt'\?)|
  (?'more'\+)|
  (?'or'\|)|
  (?'any'\*)|
  \^(?'command'[\w_]+)\^|
  (?'ws'\s*))
""");
  public static TokenNodeGroup GetTokenNodes (string token_type_string, out string? import_group)
  {
    MatchDataCollection mdc = new Regex(Regex, ROML | ROIPW).Matches(token_type_string).ToMDDCollection();

    import_group = null;

    TokenNode? previous = null;
    TokenNodeGroup filling_group = [];
    int depth = 0;

    void ThrowIfPrevNull (string item)
    {
      if (previous is null)
      {
        throw new InvalidOperationException($"Nothing to apply {item} to in token type string.");
      }
    }
    void ThrowIfDepthNeg ()
    {
      if (depth < 0)
      {
        throw new InvalidOperationException("Mismatched parentheses in token type string.");
      }
    }
    void ThrowIfGrandparentNull ()
    {
      if (filling_group.Parent is null)
      {
        throw new InvalidOperationException("Mismatched parentheses in token type string.");
      }
    }
    Debug.Log("TokenNodeFactory.GetTokenNodes", "Beginning iteration through nodes");
    foreach (MatchDataSet md in mdc)
    {
      TokenNode? item = TokenNode.Generate(md);
      if (item is null)
        continue;
      if (filling_group is null)
        continue;
      Debug.Log("TokenNodeFactory.GetTokenNodes", $"Node Processed: {item.Type}");

      switch (item.Type)
      {
        case TokenNodeType.GroupSt:
          TokenNodeGroup groupItem = new()
          {
            Parent = filling_group
          };
          depth++;
          filling_group!.Add(groupItem);
          filling_group = groupItem;
          break;
        case TokenNodeType.GroupEn:
          filling_group.AddOption();
          ThrowIfGrandparentNull();
          previous = filling_group;
          filling_group = filling_group.Parent!;
          depth--;
          ThrowIfDepthNeg();
          break;
        case TokenNodeType.Or:
          filling_group.AddOption();
          break;
        case TokenNodeType.Any:
          ThrowIfPrevNull("*");
          previous!.IsAny = true;
          break;
        case TokenNodeType.More:
          ThrowIfPrevNull("+");
          previous!.IsOneOrMany = true;
          break;
        case TokenNodeType.Opt:
          ThrowIfPrevNull("?");
          previous!.IsOptional = true;
          break;
        case TokenNodeType.Command:
          string c = md["command"].Content;
          ThrowIfPrevNull($"^{c}^");
          previous!.CommandString = c;
          if (c.Like("import"))
          {
            if (previous is not TokenNodeRef prev_ref)
              throw new InvalidOperationException("previous must be a ref_group");

            previous.ImportGroup = true;
            import_group = prev_ref.RefName;
          }
          else if (c.Like("key"))
            previous.ImportKey = true;
          else
            previous.ImportValue = c.Like("value") ? true : throw new InvalidOperationException($"Command is unknown '{c}'");
          break;
        case TokenNodeType.Literal:
          goto default;
        case TokenNodeType.Ref:
          goto default;
        case TokenNodeType.Base:
          goto default;
        case TokenNodeType.Group:
          Debug.Log(Area, "Group Encountered. This should never occur.");
          TokenNodeGroup inner_group = new()
          {
            Parent = filling_group!
          };
          break;
        case TokenNodeType.None:
          throw new InvalidOperationException("No Token node type was assigned.");
        default:
          filling_group.Add(item);
          previous = item;
          break;
      }
    }
    filling_group!.AddOption();

    return filling_group;
  }
}
