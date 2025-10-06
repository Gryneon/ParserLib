//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class ReplaceByStrOperation (ReplaceNodes nodes, string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      foreach (ReplaceNode node in nodes)
      {
        s = s.Replace(node.LookFor, node.ReplaceWith, Spec.SC);
      }
      WorkToReturn = s;
      Status = OpStatus.Pass;
    }
    else if (CheckInput(out IEnumerable<string>? list))
    {
      foreach (ReplaceNode node in nodes)
      {
        WorkToReturn = list.Select(item => item.Replace(node.LookFor, node.ReplaceWith, Spec.SC)).ToCollection();
        Status = OpStatus.Pass;
      }
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
#if false
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
