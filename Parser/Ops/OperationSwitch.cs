using Parser.Condition;

namespace Parser.Ops;

public class OperationSwitch : Operation, IPlaceholderOperation
{
  public required string ConditionString { get; init; }
  public Collection<SwitchCaseItem> Cases { get; init; } = [];
  public SwitchCaseItem? Default => Cases.LastOrDefault(item => item.IsDefaultCase, null!);
  protected ParsedExpression Expression => (field) ?? (field = (ParsedExpression) ConditionString);
  protected override void Execute ()
  {
    object? result = Expression.Evaluate(Data);

    foreach (SwitchCaseItem @case in Cases)
    {
      object? case_value = @case.Expression?.Evaluate(Data);

      if (case_value?.Equals(result) ?? false)
      {
        Parser.SetNextOperationIndex(@case.CasePosition);
        Status = OpStatus.Pass;
        break;
      }
    }

    Parser.SetNextOperationIndex(Default?.CasePosition ?? throw new OperationBadResultException($"The switch did not satisfy the provided value ({result}) nor did it provide a default case."));
    Status = OpStatus.Pass;
  }

  public int Unpack ([NotNull] Collection<IOperation> operations, int index, XParser? parser_ref = null)
  {
    foreach (SwitchCaseItem @case in Cases)
    {
      @case.CasePosition = operations.Count;
      operations.AddRange(@case.Operations);
      operations.Add(JumpTo(index + 1));
    }
    return operations.Count;
  }
  public void CheckUnpacked ()
  {
    foreach (SwitchCaseItem @case in Cases)
    {
      if (@case.CasePosition == 0)
        Err.ThrowUnpacked("Case not unpacked, unpacking failure.");
    }
  }
}
