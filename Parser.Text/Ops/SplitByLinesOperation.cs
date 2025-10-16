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

public class SplitOperation
{
  public SplitOperation (string delimeter, string input_key = "text", string output_key)
  {

  }
  public SplitOperation (IEnumerable<string> delimeters)
  {

  }
  public SplitOperation (RxS regex)
  {

  }
  public SplitOperation (RxSCollection regexes)
  {

  }
  public SplitOperation ()
  {

  }
}
