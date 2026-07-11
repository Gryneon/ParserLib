namespace Parser.Ops.Text;

public class RemoveSymbolOperation : Operation
{
  public string Pattern { get; init; }
  public string LookupGroup { get; init; }
  public required string InputKey { get; init; }
  public required string OutputKey { get; init; }
  public IEnumerable<ReplaceNode> Nodes { get; init; }

  public RemoveSymbolOperation ([SS("Regex")] string pattern, string lookupGroup, IEnumerable<ReplaceNode> nodes, string input_key, string output_key)
  {
    Pattern = pattern;
    LookupGroup = lookupGroup;
    Nodes = nodes;
    InputKey = input_key;
    OutputKey = output_key;
  }

  private string DoReplace (string input)
  {
    string update = input;
    foreach (ReplaceNode node in Nodes)
    {
      Regex rx = new(node.LookFor);

      IEnumerable<Match> results =
        from Match m in rx.Matches(update)
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
    }
    return update;
  }

  protected override void Execute ()
  {
    if (Data[InputKey] is string s)
    {
      Data[OutputKey] = DoReplace(s);
    }
    else if (Data[InputKey] is IEnumerable<string> list)
    {
      Collection<string> strings = [.. list];

      Data[OutputKey] = strings.Select(DoReplace);
    }
    else
    {
      throw Err.ThrowBadInput("string or IEnumerable<string>", Data[InputKey].TypeName);
    }

    Status = OpStatus.Pass;
  }
}
