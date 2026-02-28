
using OCT = Parser.Condition.OperationConditionType;

namespace Parser.Condition;

public class StringCompareCondition (OCT type, string left, string right) : ICondition
{
  public OCT Type { get; } = type;
  public object Left { get; } = left;
  public object Right { get; } = right;
  public bool Evaluate (XParser parser)
  {
    bool result = true;
    bool not = Type.HasFlag(OCT.Not);
    OCT type = Type.RemoveBit<OCT>(OCT.Not);

    if (type == OCT.None)
      return true && !not;
    if (type == OCT.Fail)
      return false || not;

    parser.ThrowIfNull();
    //StringComparison sc = parser.Spec.SC;

    /*switch (type)
    {
      //case OCT.Contains:
        result = left.Contains(right, sc);
        
    }*/
    return !not && result;
  }
}
