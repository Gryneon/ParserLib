//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class CombineDelimOperation (string delimiter, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out string? casted))
    {
      Status = OpStatus.Skipped;
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      Status = OpStatus.Pass;
      _workToReturn = list.Aggregate((v1, v2) => v1 += $"{delimiter}{v2}");
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
#if false
public class TrimWhitespaceOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      _workToReturn = s.Trim();
      Status = OpStatus.Pass;
    }
    else if (CheckInput(out IEnumerable<string>? ien))
    {
      _workToReturn = ien.Select(x => x.Trim()).ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
public class BetweenRegexOperation ([SS("Regex")] string prefix, [SS("Regex")] string suffix) : TextOperation(input_key, output_key)
{
  protected RxS Assembled => new(@$"(?:{prefix})(?<keep>[\s\S]*?)(?:{suffix})");
  protected Regex OpRegex => new(Assembled);

  protected override void Execute ()
  {
    if (data is null)
      return OpStatus.FailBadInputNull;

    if (data is string s)
      data = (from item in OpRegex.Matches(s) select item.Groups["keep"].Value).ToCollection();
    else if (data is IEnumerable<string> list)
    {
      /* TODO: Finish BetweenRegexOperation.DoOperation when data is IEnumerable<string> */
      // data = list.Select(x => x.Trim()).ToCollection();
    }
    else
      return OpStatus.FailBadInputType;

    return OpStatus.Pass;
  }
}
public class RemoveSymbolOperation ([SS("Regex")] string pattern, string lookupGroup, IEnumerable<ReplaceNode> nodes) : ReplaceRegexOperation(nodes)
{
  public string Pattern { get; init; } = pattern;
  public string LookupGroup { get; init; } = lookupGroup;

  protected override void Execute ()
  {
    if (data is null)
      return OpStatus.FailBadInputNull;

    foreach (ReplaceNode node in Nodes)
      if (data is string s)
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

        data = update;
      }
      else if (data is IEnumerable<string> list)
      {
        //TODO: Complete RemoveSymbolOperation.DoOperation(ref object data) for IEnumerable<string>

        //IEnumerable<string> result =
        //  from string item in list
        //  select new Regex(node.LookFor).Replace(item, node.ReplaceWith);

        //data = result.ToCollection();
      }
      else
        return OpStatus.FailBadInputType;

    return OpStatus.Pass;
  }
}
public class RemoveCommentsOperation ([SS("Regex")] string comment, [SS("Regex")] string quote, string replaceWith = "") : TextOperation(input_key, output_key)
{
  public string Comment { get; init; } = comment;
  public string Quote { get; init; } = quote;

  protected RxS Assembled => new(@$"(?<comment>{Comment})|(?<quote>{Quote})");
  protected Regex OpRegex => new(Assembled);

  protected override void Execute ()
  {
    if (data is string s)
      data = s.ReplaceAllIfContainsGroup(OpRegex.Matches(s), "comment", replaceWith);
    else if (data is IEnumerable<string> list)
    {
      Collection<string> result = [];
      foreach (string item in list)
        result.Add(item.ReplaceAllIfContainsGroup(OpRegex.Matches(item), "comment", replaceWith));
      data = result;
    }
    else
      return OpStatus.FailBadInputType;

    return OpStatus.Pass;
  }
}
public class ForEachOperation (Collection<IOperation> operations, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    OpStatus status = OpStatus.AtStart;
    Collection<object> result = [];

    OpStatus doOpsOnObject (ref object obj)
    {
      int opcount = operations.Count;
      OpStatus laststatus = OpStatus.Pass;
      for (int opindex = 0; opindex < opcount; opindex++)
      {
        if (laststatus > OpStatus.Fail)
          break;

        IOperation current = operations[opindex];
        bool allowfail = current.ContinueOnFail;
        bool skip = current.SkipOperation;
        laststatus = skip ? OpStatus.Skipped : current.DoOperation(ref obj);
      }

      return laststatus;
    }

    if (data is not IEnumerable<object> io)
      return doOpsOnObject(ref data);

    foreach (object obj in io)
    {
      if (status > OpStatus.Fail)
        break;

      object handoff = obj;
      status = doOpsOnObject(ref handoff);
      result.Add(handoff);
    }
    data = result;
    return status;
  }
}
public class StackPropertyOperation<TParent, TChild> (bool ignoreOrphans = true) : TextOperation(input_key, output_key) where TParent : class, IHasChildren<TChild>
{
  protected override void Execute ()
  {
    if (data is null)
      return OpStatus.FailBadInputNull;

    if (data is not IEnumerable<object> items)
      return OpStatus.FailBadInputType;

    TParent? lastParent = null;

    Collection<object> result = [];

    foreach (object item in items)
      if (item is TParent parent)
      {
        lastParent = parent;
        result.Add(parent);
      }
      else if (item is TChild child)
        if (lastParent is null && ignoreOrphans)
          Debug.Log("StackPropertyOperation", "Orphan Ignored.");
        else if (lastParent is null)
          result.Add(child);
        else
          lastParent?.Add(child);
      else
        result.Add(item);
    data = result;
    return OpStatus.Pass;
  }
}
public class EncapsulateOperation<TParent, TChild> (string input_key, ) : TextOperation(input_key, output_key) where TParent : class, IHasChildren<TChild>, new()
{
  protected override void Execute ()
  {
    if (data is not IEnumerable<object> items)
      return OpStatus.FailBadInputType;

    TParent parent = new();

    foreach (object item in items)
      if (item is TChild child)
        parent.Add(child);
      else
        Debug.Log("EncapulateOperation", $"Expected {typeof(TChild)}, Got {item.GetType().Name}.");
    data = parent;
    return OpStatus.Pass;
  }
}
public class ConsumeTokenOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out IEnumerable<IToken>? casted))
    {
      _workToReturn = casted.Where(token => !token.IsIgnored).ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
#endif
