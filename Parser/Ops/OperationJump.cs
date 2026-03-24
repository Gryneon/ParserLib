namespace Parser.Ops;

public class OperationJump : Operation
{
  protected int TargetIndex { get; set; }
  private string? TargetLabel { get; set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;
  public OperationJump (int index)
  {
    DebugIn("OperationJump", $"({index})");
    TargetIndex = index;

    if (TargetIndex < 0)
      _ = Op.ThrowBadDef("Cannot jump to a negative index.");
  }
  public OperationJump (string label)
  {
    TargetIndex = -1;
    TargetLabel = label;
  }
  protected OperationJump () { }
  protected override void Execute ()
  {
    if (TargetIndex >= Parser.OpCount)
      Status = Op.ThrowBadDef($"TargetIndex ({TargetIndex}) above maximum ({Parser.OpCount}).");
    else if (TargetIndex == -1 && TargetLabel is null)
      Status = Op.ThrowBadDef("Neagtive Jump Target");
    else if (TargetIndex == -1 && TargetLabel is not null)
      Parser.SetNextOperationIndex(Parser.Labels[TargetLabel]);
    else
      Parser.SetNextOperationIndex(TargetIndex);
  }
}
