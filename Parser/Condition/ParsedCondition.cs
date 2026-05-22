namespace Parser.Condition;

public class ParsedCondition : ICondition
{
  #region Public Properties
  public virtual bool DoNotEvaluate { get; }
  public required string ConditionString { get; set; }
  #endregion
  [AllowNull]
  protected DataStore Data { get; set; }
  protected bool Result { get; set; }
  protected Collection<ConditionValue> Sequence { get; } = [];
  public ParsedCondition () { }
  [SetsRequiredMembers]
  protected ParsedCondition (string condition) =>
    ConditionString = condition;
  protected object? GetValue (ConditionValue @ref)
  {
    return @ref.Type switch
    {
      KeyOption.LoadKey => Data[@ref.Key],
      KeyOption.CountOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key] is IEnumerable ien ? ien.Count() : 1 : 0,
      KeyOption.CheckKeyExists => Data.CanLoad(@ref.Key),
      KeyOption.TypeOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key].GetType().Name : "null",
      KeyOption.Literal => @ref.Key,
      KeyOption.False => false,
      KeyOption.True => true,
      _ => null
    };
  }

  public bool Evaluate (DataStore data)
  {
    Data = data;

    if (!DoNotEvaluate)
    {
      Execute();
    }

    return Result;
  }

  /// <summary>
  /// This method must set <c><see cref="Result"/></c> with the results of the condition.
  /// </summary>
  /// <remarks>If the evaluation fails, it always returns <see langword="false"/>.</remarks>
  protected virtual void Execute ()
  {
    object? previous = null;
    KeyOption? op = null;
    object? current = null;
    bool? result = null;
    foreach (ConditionValue cv in Sequence)
    {
      if (previous is null)
      {
        previous = GetValue(cv);
        continue;
      }

      if (cv.IsOperator)
      {
        op = cv.Type;
        continue;
      }

      if (op is not null)
      {
        current = GetValue(cv);

        result = op switch
        {
          KeyOption.OpIs => $"{previous}".Is($"{current}"),
          KeyOption.OpEq => previous == current,
          KeyOption.OpNotEq => previous != current,
          KeyOption.OpLt => decimal.Parse($"{previous}", CIIC) < decimal.Parse($"{current}", CIIC),
          _ => null
        };

        if (result is null)
        {
          break;
        }

        previous = result;
        current = null;
        continue;
      }

      break;
    }

    Result = result ?? false;
  }
}
