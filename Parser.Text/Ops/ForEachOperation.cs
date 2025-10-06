namespace Parser.Text.Ops;

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
    WorkToReturn = result;
    Status = OpStatus.Pass;
    return;
  }
}
