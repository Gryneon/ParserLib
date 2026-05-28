using static Parser.Condition.KeyOption;

namespace Parser.Condition;

public class ParsedExpression : IExpression
{
  private static readonly OperationConditionStringException AndOrInvalidTypeException = new("Both values must be a boolean type for logical operations. There is no PEMDAS, it goes left to right.");

  #region Public Properties
  public required string Expression { get; set; }
  #endregion
  [AllowNull]
  public DataStore Data { get; protected set; }
  protected Collection<ConditionValue> Sequence { get; } = [];
  public ParsedExpression () { }
  [SetsRequiredMembers]
  protected ParsedExpression (string expression) =>
    Expression = expression;
  protected object? GetValue (ConditionValue @ref) => @ref.Type switch
  {
    LoadKey => Data[@ref.Key],
    CountOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key] is IEnumerable ien ? ien.Count() : 1 : 0,
    CheckKeyExists => Data.CanLoad(@ref.Key),
    TypeOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key].GetType().Name : "null",
    Literal => @ref.Key,
    False => false,
    True => true,
    Null => null,
    >= OpStart => Err.ThrowBadDef($"Condition String Tried to GetValue from an operator {@ref}."),
    _ => Err.ThrowBadDef($"Condition String Tried to GetValue from an unknown type {@ref}.")
  };
  protected decimal GetDecimal (object? value)
  {

  }

  protected void Parse ()
  {
    if (Expression.IsEmpty())
      return;

    // https://regex101.com/r/aTwSlR/2
    string conditionPattern = new RxS(@"(?:(?:\b(?'exists'exists)|(?'countof'countof)|(?'typeof'typeof))\b)?\[(?<varname>.*?)\]|\{(?<literal>.*?)\}|(?'gteq'>=)|(?'lteq'<=)|(?'gt'>)|(?'lt'<)|(?'eq'==)|(?'noteq'!=)|(?:\b(?'like'like)|(?'seqeq'matches)|(?'is'is)\b)|(?'and'&&)|(?'or'\|\|)|(?'int'-?\d+)|(?'dec'-?\d*\.\d+)|(?:\b(?'true'(?i:true))|(?'false'(?i:false))|(?'null'(?i:null))\b)");
    foreach (Match m in Regex.Matches(Expression, conditionPattern, ROSL | ROIPW | ROML, TimeSpan.FromSeconds(1)))
    {
      string? keyname = m.Groups.ContainsKey("varname") ? m.Groups["varname"].Value : null;

      void chkAdd (string group, KeyOption type, string? value = null)
      {
        if (m.Groups.ContainsKey(group))
          Sequence.Add(new(type, value));
      }
      chkAdd("exists", CheckKeyExists, keyname);
      chkAdd("countof", CountOfKey, keyname);
      chkAdd("typeof", TypeOfKey, keyname);
      chkAdd("true", True);
      chkAdd("false", False);
      chkAdd("null", Null);

      chkAdd("gteq", OpGteq);
      chkAdd("lteq", OpLteq);
      chkAdd("gt", OpGt);
      chkAdd("lt", OpLt);
      chkAdd("eq", OpEq);
      chkAdd("noteq", OpNotEq);
      chkAdd("and", OpAnd);
      chkAdd("or", OpOr);
      chkAdd("int", Integer, m.Groups["int"].Value);
      chkAdd("dec", KeyOption.Decimal, m.Groups["dec"].Value);
    }
  }

  /// <summary>
  /// Evaluate
  /// </summary>
  /// <param name="data">The <see cref="DataStore"/> reference.</param>
  /// <remarks>If the evaluation fails, it always returns <see langword="false"/>.</remarks>
  public virtual object? Evaluate (DataStore data)
  {
    object? previous = null;
    KeyOption? op = null;
    object? current;
    object? result = null;
    HashSet<int> or_indexes = [];
    HashSet<int> and_indexes = [];
    Dictionary<int, object?> evalStore = [];
    bool nextItem = false;

    for (int i = 0; i < Sequence.Count; i++)
    {
      ConditionValue cv = Sequence[i];
      if (previous is null)
      {
        previous = GetValue(cv);
        continue;
      }

      if (cv.Type is OpAnd or OpOr)
      {
        evalStore[i] = previous;
        _ = cv.Type is OpAnd ? and_indexes.Add(i) : or_indexes.Add(i);
        previous = null;
        continue;
      }

      if (cv.IsOperator)
      {
        op = cv.Type;
        continue;
      }

      current = GetValue(cv);

      if (op is not null && current is not null)
      {
        result = op switch
        {
          OpIs when previous is Type typ => $"{typ.Name}".Is($"{current}"),
          OpIs when previous is string str => str.Is($"{current}"),
          OpLike => $"{previous}".Like($"{current}"),
          OpEq => previous == current,
          OpNotEq => previous != current,
          OpDiv => previous / current
          OpLt => decimal.Parse($"{previous}", CIIC) < decimal.Parse($"{current}", CIIC),
          OpGt => decimal.Parse($"{previous}", CIIC) > decimal.Parse($"{current}", CIIC),
          OpLteq => decimal.Parse($"{previous}", CIIC) <= decimal.Parse($"{current}", CIIC),
          OpGteq => decimal.Parse($"{previous}", CIIC) >= decimal.Parse($"{current}", CIIC),
          //OpOr => previous is bool p && current is bool c ? p || c : throw AndOrInvalidTypeException,
          //OpAnd => previous is bool p && current is bool c ? p && c : throw AndOrInvalidTypeException,
          _ => null
        };

        if (result is null)
          break;

        previous = result;
      }
    }
    evalStore[Sequence.Count - 1] = previous;
    previous = null;
    foreach (int i in and_indexes)
    {
      if (previous is null)
      {
        previous = evalStore[i];
        continue;
      }

      result = (bool) previous && (bool) (evalStore[i] ?? false);
    }

    return result;
  }
}
