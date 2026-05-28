namespace Parser.Condition;
/// <summary>A type of expression that returns a <see langword="bool"/>.</summary>

public interface ICondition : IExpression
{
  object? IExpression.Evaluate (DataStore data) => Evaluate(data);
  /// <summary>Evalutates the condition.</summary>
  /// <returns><see langword="true"/> if the condition is true, or a value that is: non-zero, non-empty and not null. Returns <see langword="false"/> otherwise.</returns>
  /// <remarks>If the evaluation fails, it returns <see langword="false"/>.</remarks>
  /// <example>
  /// An empty <see cref="IEnumerable{T}"/> returns <see langword="false"/><br/>
  /// The number <c>4</c> returns <see langword="true"/><br/>
  /// The value <see langword="null"/> returns <see langword="false"/>
  /// </example>

  new bool Evaluate (DataStore data);
}
