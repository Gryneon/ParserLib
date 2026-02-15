namespace Parser.Condition;

/// <summary>
/// Represents a condition or a check, and its result.
/// </summary>
public interface ICondition
{
  /// <summary>
  /// The type of condition.
  /// </summary>
  OperationConditionType Type { get; }
  /// <summary>
  /// The result of the condition evaluation.
  /// </summary>
  bool ConditionResult { get; }
  /// <summary>
  /// Evalutates the condition and stores the result.
  /// </summary>
  bool Evaluate (XParser parser);
}
