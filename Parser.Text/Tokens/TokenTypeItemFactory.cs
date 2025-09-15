#pragma warning disable IDE1006 // Naming Styles

using static Parser.DefinitionStaticFunctions;

namespace Parser.Text.Tokens;

public static class TokenNodeFactory
{
  public static readonly RxS Regex = Rx(@"(?'line''(?'literal'(?:[^']|'')+?)'|\#(?'ref'[\w_]+)|\$(?'base'[\w_]+)|(?'gp_start'\()|(?'gp_end'\))|(?'opt'\?)|(?'more'\+)|(?'or'\|))");

  internal static MatchDataCollection GetMatchData (string token_type_string) =>
    new Regex(Regex).Matches(token_type_string).ToMDDCollection();

  internal static TokenNodeGroup GetTokenNodes (MatchDataCollection mdc)
  {
    TokenNodeGroup result = new();
    TokenNode previous = null!;
    TokenNodeGroup? parent = result;
    int depth = 0;

    void ThrowIfPrevNull (string item)
    {
      if (previous is null)
      {
        throw new NullReferenceException($"Nothing to apply {item} to in token type string.");
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
        throw new NullReferenceException("Mismatched parentheses in token type string.");
      }
    }

    foreach (MatchData md in mdc)
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
          break;
      }
    }
    parent?.AddOption();

    return result;
  }
}
