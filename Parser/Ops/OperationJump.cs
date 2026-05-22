namespace Parser.Ops;

public class OperationJump : Operation
{
  protected int TargetIndex { get; set; }
  private string? TargetIndexVar { get; }
  private string? TargetLabel { get; }
  public override bool NoInput => true;
  public override bool NoOutput => true;
  public OperationJump (int index)
  {
    TargetIndex = index;

    if (TargetIndex < 0)
      _ = Err.ThrowBadDef("Cannot jump to a negative index.");
  }
  public OperationJump (string label_or_var, bool use_var = false)
  {
    TargetIndex = -1;
    if (use_var)
    {
      TargetIndexVar = label_or_var;
    }
    else
    {
      TargetLabel = label_or_var;
    }
  }

  protected OperationJump () { }
  protected override void Execute ()
  {
    int index;

    if (TargetIndex >= Parser.OpCount)
    {
      index = (int) Err.ThrowBadDef($"TargetIndex ({TargetIndex}) above maximum ({Parser.OpCount}).");
    }
    else
    {
      index = TargetIndex == -1 && TargetLabel is null && TargetIndexVar is not null
          ? Data.TryLoad(TargetIndexVar, out int i) ? i : throw new OperationBadDefinitionException("Bad index at " + TargetIndexVar)
          : TargetIndex == -1 && TargetLabel is not null ? Parser.Labels[TargetLabel] : 0;
    }

    Parser.SetNextOperationIndex(index);
  }
}
