#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Text.Ops;

/// <summary>
/// TODO: This operation is buggy and not yet functional.
/// </summary>
public class TokenTemplateOperation : TextOperation
{
  //Collections
  protected Dictionary<string, TokenNodeGroup> RefGroups { get; set; } = [];

  public TokenTemplateOperation (Dictionary<string, string> template_definitions, string input_key = "tokens", string output_key = "tokens_templated") : base(input_key, output_key)
  {
    foreach (KeyValuePair<string, string> kvp in template_definitions)
    {
      MatchDataCollection mdds = TokenNodeFactory.GetMatchData(kvp.Value);
      TokenNodeGroup group = TokenNodeFactory.GetTokenNodes(mdds);
      RefGroups.Add(kvp.Key, group);
    }
  }
  protected override void Execute ()
  {
    if (!CheckInput(out IEnumerable<IToken>? tokens))
      throw new InvalidOperationException();

    Collection<IToken> MutableTokens = [.. tokens];

    foreach (KeyValuePair<string, TokenNodeGroup> node in RefGroups)
    {
      int pos = 0;
      while (pos < MutableTokens.Count)
      {
        int match = node.Value.CheckForMatch(MutableTokens, pos);

        if (match > 0)
        {
          int last = pos + match;
          IEnumerable<IToken>? sub = MutableTokens.Skip(pos).Take(match);
          MutableTokens.RemoveCount(match, pos);
          MutableTokens.Insert(pos, new Token(sub.Select(item => item.Content).TextJoin(), sub.First().Position, node.Key)
          {
            //Children = [.. sub], //TODO: Make not read only?
            FromNode = node.Value,
            Type = node.Key,
            Properties = [] //TODO: Get Properties!
          });
          pos = 0;
        }
        else
          pos++;
      }
    }
  }
}
