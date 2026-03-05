namespace Parser.Ops;

public sealed class OperationContinue () : Operation
{
  public int ContTarget { get; private set; }
  public override bool NoInput => true;
  public override bool NoOutput => true;

  protected override void Execute () => Parser.SetNextOperationIndex(ContTarget);
  public void SetContTarget (int target) => ContTarget = target;
}
