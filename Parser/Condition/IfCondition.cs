
using OCT = Parser.Condition.OperationConditionType;

namespace Parser.Condition;

public class StringCompareCondition (OCT type, string left, string right) : ICondition
{
  public OCT Type { get; } = type;
  public object Left { get; } = left;
  public object Right { get; } = right;
  public bool ConditionResult { get; protected set; }
  /// <inheritdoc/>
  public bool Evaluate (XParser parser)
  {
    bool not = Type.HasFlag(OCT.Not);
    OCT type = Type.RemoveBit<OCT>(OCT.Not);

    if (type == OCT.None)
      return ConditionResult = true;
    if (type == OCT.Fail)
      return ConditionResult = false;

    parser.ThrowIfNull();
    StringComparison sc = parser.Spec.SC;

    /*switch (type)
    {
      //case OCT.Contains:
        result = left.Contains(right, sc);
        
    }*/
    ConditionResult = !not && ConditionResult;
    return ConditionResult;
  }
}
