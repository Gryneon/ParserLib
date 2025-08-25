namespace Parser.Text.Ops;

public abstract class TextOperation : Operation
{
  protected DoOperationFunction? _function;
  /// <summary>
  /// The parser reference, initialized in <see cref="Initialize(TextParser)"/>
  /// </summary>
  [AllowNull] protected new TextParser _parser;

  public TextOperation (string input_key, string output_key) : base(input_key, output_key) { }
  public TextOperation (IEnumerable<string> input_keys, string output_key) : base(input_keys, output_key) { }

  public TextOperation (bool ignore_all_loads) : base(ignore_all_loads) { }

  public override OpStatus DoOperation (ref object data)
  {
    ThrowNoOverrideError();
    return OpStatus.Error;
  }
  public virtual OpStatus DoOperation (TextParser parser)
  {
    if (EndOperation)
      return OpStatus.EndCommand;
    if (SkipOperation)
      return OpStatus.Skipped;
    if (DebugOperation)
      Debug.Log("TextOperation.DoOperation(TextParser)", "Debug operation started.");

    Initialize(parser);
    CheckInputNull();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    Execute();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    AssignResult();

    return AdjustedStatus;
  }
  protected bool CheckInput<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out T casted)
  {
    if (_parser.Work.TryLoad(_input_key, out casted))
    {
      return true;
    }

    Status = OpStatus.FailBadInputType;
    casted = default;
    return false;
  }
  protected bool CheckInputs<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out Collection<T> casted)
  {
    casted = [];
    for (int i = 0; i < _input_keys.Count; i++)
    {
      if (!_parser.Work.ContainsKey(_input_keys[i]))
      {
        Status = OpStatus.FailNoSuchVarName;
        return false;
      }
      if (!_parser.Work.TryLoad(_input_keys[i], out T? temp))
      {
        Status = OpStatus.FailBadInputType;
        return false;
      }
      casted.Add(temp);
    }

    Status = OpStatus.Pass;
    return true;
  }
  /// <summary>
  /// Performs the operation and stores the value in <c><see cref="Operation._workToReturn"/></c>,
  /// and the <see cref="OpStatus"/> in <c><see cref="Operation.Status"/></c>
  /// </summary>
  protected override void Execute ()
  {
    if (_function is null || _workToReturn is null)
      return;

    Status = _function(ref _workToReturn);
  }
  protected override void CheckInputNull ()
  {
    if (!_parser.Work.ContainsKey(_input_key))
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {_input_key} does not exist.");
      Status = OpStatus.FailNoSuchVarName;
    }
    else if (_parser.Work.TryLoad(_input_key, out object? data) && data is not null)
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {_input_key} is not null.");
      Status = OpStatus.Pass;
    }
    else
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {_input_key} is null.");
      Status = OpStatus.FailBadInputNull;
    }
  }
  protected override void CheckInputsNull ()
  {
    foreach (string key in _input_keys)
    {
      if (!_parser.Work.ContainsKey(key))
      {
        Debug.Log("TextOperation.CheckInputsNull", $"Key {key} does not exist.");
        Status = OpStatus.FailNoSuchVarName;
        return;
      }
      else if (_parser.Work.TryLoad(key, out object? data) && data is not null)
      {
        continue;
      }
      else
      {
        Debug.Log("TextOperation.CheckInputsNull", $"Key {key} is null.");
        Status = OpStatus.FailBadInputNull;
        return;
      }
    }
    Debug.Log("TextOperation.CheckInputsNull", $"All keys are not null.");
    Status = OpStatus.Pass;
  }
  /// <summary>
  /// Assigns the parser to <c><see cref="_parser"/></c> and defines <c><see cref="_function"/></c>.
  /// </summary>
  /// <param name="parser">The parser reference to pass to the operation.</param>
  [MemberNotNull(nameof(_parser), nameof(_function))]
  protected virtual void Initialize (TextParser parser)
  {
    _function ??= DoOperation;
    _parser = parser;

    if (IgnoreAllLoads)
      _workToReturn = null;
    else if (parser.Work.TryLoad(_input_key, out _workToReturn))
      Debug.Log("TextOperation.Initialize", $"Loaded {_input_key} with value {_workToReturn}.");
    else
    {
      Debug.Log("TextOperation.Initialize", $"Key {_input_key} does not exist or is null.");
      Status = OpStatus.FailNoSuchVarName;
      _workToReturn = null;
    }

    object? getKey (string input_key)
    {
      if (parser.Work.TryLoad(_input_key, out object? value))
        return value;
      else
      {
        Debug.Log("TextOperation.Initialize.getKey(string)", $"Key {input_key} does not exist or is null.");
        return null;
      }
    }

    if (_input_keys.Count > 1)
    {
      _multiple_input_values = [.. _input_keys.Select(getKey)];

      if (_multiple_input_values.Any(item => item is null))
      {
        Status = OpStatus.FailBadInputNull;
      }
    }
  }
  protected virtual void AssignResult<T> (DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (_workToReturn is null) return;
    _ = _parser.Work.Save<T>(_output_key, _workToReturn, mode);
  }
  protected virtual void AssignResult (DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (_workToReturn is null) return;
    _ = _parser.Work.Save(_output_key, _workToReturn, mode);
  }
}

public class CopyOperation (string input_key, string output_key) : TextOperation(input_key, output_key)
{
  protected override void Execute () => Status = OpStatus.Pass;
}
