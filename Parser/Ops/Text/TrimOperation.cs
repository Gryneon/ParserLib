namespace Parser.Ops.Text;

public class TrimOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (WorkToReturn is string s)
    {
      WorkToReturn = s.Trim();
      Status = OpStatus.Pass;
    }
    else if (WorkToReturn is IEnumerable<string> ien)
    {
      WorkToReturn = ien.Select(x => x.Trim()).ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
