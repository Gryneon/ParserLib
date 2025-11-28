//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

public class RemoveSymbolOperation ([SS("Regex")] string pattern, string lookupGroup, IEnumerable<ReplaceNode> nodes, string input_key, string output_key) : Operation(input_key, output_key)
{
  public string Pattern { get; init; } = pattern;
  public string LookupGroup { get; init; } = lookupGroup;
  public IEnumerable<ReplaceNode> Nodes { get; init; } = nodes;

  protected override void Execute ()
  {
    foreach (ReplaceNode node in Nodes)
      if (WorkToReturn is string s)
      {
        Regex rx = new(node.LookFor);
        string update = s;

        IEnumerable<Match> results =
          from Match m in rx.Matches(s)
          where m.Groups.ContainsKey(LookupGroup)
          select m;

        foreach (Match m in results)
        {
          string name = m.Groups[LookupGroup].Value;
          int pos = m.Index;
          int len = m.Length;
          update = update.
            Remove(pos, len).
            Insert(pos, node.ReplaceWith ?? SE);
        }

        WorkToReturn = update;
      }
      else if (WorkToReturn is IEnumerable<string> list)
      {
        //TODO: Complete RemoveSymbolOperation.DoOperation(ref object data) for IEnumerable<string>

        //IEnumerable<string> result =
        //  from string item in list
        //  select new Regex(node.LookFor).Replace(item, node.ReplaceWith);

        //data = result.ToCollection();
      }
      else
      {
        Status = OpStatus.FailBadInputType;
        return;
      }
    Status = OpStatus.Pass;
  }
}
