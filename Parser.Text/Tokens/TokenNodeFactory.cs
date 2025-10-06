#pragma warning disable IDE1006 // Naming Styles

using static Parser.DefinitionStaticFunctions;

namespace Parser.Text.Tokens;

public static class TokenNodeFactory
{
  public static readonly RxS Regex = Rx(@"(?'line''(?'literal'(?:[^']|'')+?)'|\$(?'base'[\w_]+)|\#(?'ref'[\w_]+)|(?'gp_start'\()|(?'gp_end'\))|(?'opt'\?)|(?'more'\+)|(?'or'\|)|(?'any'\*)|\^(?'command'[\w_]+)\^|(?'ws'\s*))");

  internal static TokenNodeGroup GetTokenNodes (string token_type_string, out string? import_group)
  {
    MatchDataCollection mdc = new Regex(Regex).Matches(token_type_string).ToMDDCollection();

    import_group = null;

    TokenNodeGroup result = new();
    TokenNode previous = null!;
    TokenNodeGroup? parent = result;
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
      if (parent.Parent is null)
      {
        throw new InvalidOperationException("Mismatched parentheses in token type string.");
      }
    }

    foreach (MatchDataSet md in mdc)
    {
      TokenNode? item = TokenNode.Generate(md);

      if (item is null)
        continue;
      if (parent is null)
        continue;

      switch (item.Type)
      {
        case TokenNodeType.GroupSt:
          TokenNodeGroup groupItem = new()
          {
            Parent = parent
          };
          depth++;
          parent!.Add(groupItem);
          parent = groupItem;
          break;
        case TokenNodeType.GroupEn:
          parent.AddOption();
          ThrowIfGrandparentNull();
          previous = parent;
          parent = parent.Parent;
          depth--;
          ThrowIfDepthNeg();
          break;
        case TokenNodeType.Or:
          parent.AddOption();
          break;
        case TokenNodeType.Any:
          previous.IsAny = true;
          break;
        case TokenNodeType.More:
          ThrowIfPrevNull("+");
          previous.IsOneOrMany = true;
          break;
        case TokenNodeType.Opt:
          ThrowIfPrevNull("?");
          previous.IsOptional = true;
          break;
        case TokenNodeType.Command:
          string c = md["command"].Content;
          ThrowIfPrevNull($"^{c}^");
          previous.CommandString = c;
          if (c.Like("import"))
          {
            if (previous is not TokenNodeRef prev_ref)
              throw new InvalidOperationException("previous must be a ref_group");

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
          TokenNodeGroup inner_group = new()
          {
            Parent = parent
          };
          break;
        case TokenNodeType.None:
          throw new InvalidOperationException("No Token node type was assigned.");
        default:
          parent.Add(item);
          previous = item;
          break;
      }
    }
    parent?.AddOption();

    return result;
  }
}
