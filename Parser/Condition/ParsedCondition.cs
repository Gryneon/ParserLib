using static Parser.Condition.KeyOption;

namespace Parser.Condition;

public class ParsedCondition : ICondition
{
  private static readonly OperationConditionStringException AndOrInvalidTypeException = new("Both values must be a boolean type for logical operations. There is no PEMDAS, it goes left to right.");

  #region Public Properties
  public required string ConditionString { get; set; }
  #endregion
  [AllowNull]
  public DataStore Data { get; protected set; }
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
      LoadKey => Data[@ref.Key],
      CountOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key] is IEnumerable ien ? ien.Count() : 1 : 0,
      CheckKeyExists => Data.CanLoad(@ref.Key),
      TypeOfKey => Data.CanLoad(@ref.Key) ? Data[@ref.Key].GetType().Name : "null",
      Literal => @ref.Key,
      False => false,
      True => true,
      Null => null,
      _ => null
    };
  }

  protected void ParseConditionString ()
  {
    if (ConditionString.IsEmpty())
      return;

    // https://regex101.com/r/aTwSlR/2
    string conditionPattern = new RxS(@"(?:(?'exists'exists)|(?'countof'countof)|(?'typeof'typeof))?\[(?<varname>.*?)\]|\{(?<literal>.*?)\}|(?'gteq'\>\=)|(?'lteq'<\=)|(?'gt'\>)|(?'lt'<)|(?'eq'==)|(?'like'like)|(?'is'is)|(?'and'&&)|(?'or'\|\|)|(?'int'-?\d+)|(?'dec'-?\d*\.\d+)|(?'true'(?i:true))|(?'false'(?i:false))|(?'null'(?i:null))");
    foreach (Match m in Regex.Matches(ConditionString, conditionPattern, ROSL | ROIPW | ROML, TimeSpan.FromSeconds(1)))
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
      chkAdd("true", True, "true");
      chkAdd("false", False, "false");

      chkAdd("gteq", OpGteq);
      chkAdd("lteq", OpLteq);
      chkAdd("gt", OpGt);
      chkAdd("lt", OpLt);
      chkAdd("eq", OpEq);
    }
  }

  /// <summary>Describes what the data is.</summary>
  protected enum DataType
  {
    /// <summary>The value null.</summary>
    Null,
    /// <summary>A boolean (true/false) value.</summary>
    Bool,
    /// <summary>An integer value.</summary>
    Int,
    /// <summary>A decimal value.</summary>
    Dec,
    /// <summary>A string (text) value.</summary>
    String,
    /// <summary>The type of a value (int, List, object, Dictionary, etc.).</summary>
    Type
  }

  public bool Evaluate (DataStore data)
  {
    Data = data;
    Execute();
    return Result;
  }

  /// <summary>
  /// This method must set <c><see cref="Result"/></c> with the results of the condition.
  /// </summary>
  /// <remarks>If the evaluation fails, it always returns <see langword="false"/>.</remarks>
  protected virtual void Execute ()
  {
    object? previous = null;
    DataType prevtype = DataType.Null;
    KeyOption? op = null;
    object? current;
    bool? result = null;
    foreach (ConditionValue cv in Sequence)
    {
      if (previous is null)
      {
        previous = GetValue(cv);
        prevtype = cv.Type switch
        {
          LoadKey => throw new NotImplementedException(),
          CountOfKey => throw new NotImplementedException(),
          TypeOfKey => DataType.Type,
          Literal => throw new NotImplementedException(),
          CheckKeyExists or True or False => DataType.Bool,
          Null => throw new NotImplementedException(),
          Integer => throw new NotImplementedException(),
          KeyOption.Decimal => throw new NotImplementedException(),
        };
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

        if (current is null)
          break;

        result = op switch
        {
          OpIs => $"{previous}".Is($"{current}"),
          OpEq => previous == current,
          OpNotEq => previous != current,
          OpLt => decimal.Parse($"{previous}", CIIC) < decimal.Parse($"{current}", CIIC),
          OpGt => decimal.Parse($"{previous}", CIIC) > decimal.Parse($"{current}", CIIC),
          OpLteq => decimal.Parse($"{previous}", CIIC) <= decimal.Parse($"{current}", CIIC),
          OpGteq => decimal.Parse($"{previous}", CIIC) >= decimal.Parse($"{current}", CIIC),
          OpLike => $"{previous}".Like($"{current}"),
          OpOr => previous is bool p && current is bool c ? p || c : throw AndOrInvalidTypeException,
          OpAnd => previous is bool p && current is bool c ? p && c : throw AndOrInvalidTypeException,
          _ => null
        };

        if (result is null)
          break;

        previous = result;
        current = null;
        continue;
      }

      break;
    }

    Result = result ?? false;
  }
}
