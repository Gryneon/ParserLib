namespace Parser.Text.Ops;

public class SplitByLinesOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  /// <inheritdoc/>>
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      WorkToReturn = RX.LineEnd.Split(s);
      Status = OpStatus.Pass;
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
    return;
  }
}

public class AssemblyRule
{
  public bool Global { get; set; }

}

public class AssembleByRulesOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
#pragma warning disable IDE1006 // Naming Styles
  private string? cs;
  private int i;
  private char c;
  protected override void Execute ()
  {
    if (CheckInput(out string? s))
    {
      cs = s;
      for (i = 0; i < cs.Length; i++)
      {
        c = cs[i];


      }
    }
    else
    {
      Status = OpStatus.FailBadInputType;
    }
  }
}
