namespace Parser.Text.Ops;

public class ReplaceRegexOperation (IEnumerable<ReplaceNode> nodes) : TextOperation
{
  public List<ReplaceNode> Nodes { get; init; } = [.. nodes];

  public static ReplaceNode Def ([SS("Regex")] string lookFor, string replaceWith) => ReplaceNode.From(lookFor, replaceWith);

  public override OpStatus DoOperation (ref object data)
  {
    if (data is null)
      return OpStatus.FailBadInputNull;

    foreach (ReplaceNode node in Nodes)
      if (data is string s)
        data = new Regex(node.LookFor).Replace(s, node.ReplaceWith ?? SE);
      else if (data is IEnumerable<string> list)
        data = list.Select(item => new Regex(node.LookFor).Replace(item, node.ReplaceWith ?? SE)).ToCollection();
      else
        return OpStatus.FailBadInputType;

    return OpStatus.Pass;
  }
}
