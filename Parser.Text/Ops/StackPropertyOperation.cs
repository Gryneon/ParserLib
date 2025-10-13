//using Parser.Text.Tokens;

namespace Parser.Text.Ops;

public class StackPropertyOperation<TParent, TChild> (bool ignoreOrphans, string input_key, string output_key) : TextOperation(input_key, output_key) where TParent : class, IHasChildren<TChild>
{
  protected override void Execute ()
  {
    if (!CheckInput(out IEnumerable<object>? items))
    {
      Status = OpStatus.FailBadInputType;
      return;
    }

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
          lastParent.Add(child);
      else
        result.Add(item);
    WorkToReturn = result;
    Status = OpStatus.Pass;
  }
}
