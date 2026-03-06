namespace Parser.Ops;

public sealed class OperationJump : Operation
{
  private int TargetIndex { get; set; }
  private string? TargetLabel { get; set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;
  public OperationJump (int index)
  {
    TargetIndex = index;
  }
  public OperationJump (string label)
  {
    TargetIndex = -1;
    TargetLabel = label;
  }
  protected override void Execute ()
  {
    if (TargetIndex >= Parser.OpCount)
      Status = Op.ThrowBadDef($"TargetIndex ({TargetIndex}) above maximum ({Parser.OpCount}).");
    else if (TargetIndex == -1 && TargetLabel is null)
      Status = Op.ThrowBadDef("Neagtive Jump Target");
    else if (TargetIndex == -1 && TargetLabel is not null)
      Parser.SetNextOperationIndex(Parser.Labels[TargetLabel]);
    else if (TargetIndex == Op.JumpToEnd)
      Parser.SetNextOperationIndex(-1);
    else
      Parser.SetNextOperationIndex(TargetIndex);
  }
}
