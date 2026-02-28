namespace Parser.Ops.Text;

public class TrimOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      WorkData = s.Trim();
      Status = OpStatus.Pass;
    }
    else if (CheckInput(out IEnumerable<string>? ien))
    {
      WorkData = ien.Select(x => x.Trim()).ToCollection();
      Status = OpStatus.Pass;
    }
    else
      Status = OpStatus.FailBadInputType;
  }
}
