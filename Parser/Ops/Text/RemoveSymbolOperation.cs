//using Parser.Text.Tokens;

namespace Parser.Ops.Text;

public class RemoveSymbolOperation : Operation
{
  public string Pattern { get; init; }
  public string LookupGroup { get; init; }
  public IEnumerable<ReplaceNode> Nodes { get; init; }

  public RemoveSymbolOperation ([SS("Regex")] string pattern, string lookupGroup, IEnumerable<ReplaceNode> nodes, string input_key, string output_key)
  {
    Pattern = pattern;
    LookupGroup = lookupGroup;
    Nodes = nodes;
    LengthKey = input_key;
    OutputKey = output_key;
  }

  protected override void Execute ()
  {
    foreach (ReplaceNode node in Nodes)
    {
      if (WorkData is string s)
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

        WorkData = update;
      }
      else if (WorkData is IEnumerable<string> list)
      {
        //TODO: Complete RemoveSymbolOperation.DoOperation(ref object data) for IEnumerable<string>

        //IEnumerable<string> result =
        //  from string item in list
        //  select new Regex(node.LookFor).Replace(item, node.ReplaceWith);

        //data = result.ToCollection();
      }
      else
      {
        Status = Err.ThrowBadInput("string or IEnumerable<string>", $"{WorkData?.GetType()}");
        return;
      }
    }

    Status = OpStatus.Pass;
  }
}
