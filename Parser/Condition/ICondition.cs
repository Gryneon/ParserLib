namespace Parser.Condition;

/// <summary>Represents a condition or a check, and its result.</summary>
public interface ICondition
{
  /// <summary>Evalutates the condition.</summary>
  /// <returns><see langword="true"/> if the condition is true, <see langword="false"/> otherwise.</returns>
  bool Evaluate (DataStore data);
}
