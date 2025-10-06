using Parser.Ops;

namespace Parser.Text.Ops;

/// <summary>
/// A variant of the <see cref="Operation"/> class that adds some text specific features, as well as referencing a <see cref="TextSpec"/> rather than a <see cref="Spec"/>.
/// </summary>
public abstract class TextOperation : Operation<TextParser>, IOperation
{
  /// <summary>
  /// The parser reference, initialized in <see cref="Initialize(TextParser)"/>
  /// </summary>
  [AllowNull] protected new TextParser Parser { get; set; }
  /// <summary>
  /// The spec reference, initialized in <see cref="Initialize(TextParser)"/>
  /// </summary>
  [AllowNull] protected TextSpec Spec { get; set; }
  public DictionaryMode CurrentMode { get; set; } = DictionaryMode.Overwrite;
  protected TextOperation (string input_key, string output_key) : base(input_key, output_key) { }
  protected TextOperation (IEnumerable<string> input_keys, string output_key) : base(input_keys, output_key) { }
  protected TextOperation (bool ignore_all_loads) : base(ignore_all_loads) { }

  public override OpStatus DoOperation (ref object data)
  {
    ThrowNoOverrideError();
    return OpStatus.Error;
  }
  public override OpStatus DoOperation<TParser> (TParser parser)
  {
    if (EndOperation)
      return OpStatus.EndCommand;
    if (SkipOperation)
      return OpStatus.Skipped;
    if (DebugOperation)
      Debug.Log("TextOperation.DoOperation(TParser)", "Debug operation started.");

    if (parser is not TextParser text_parser)
      throw new ArgumentException($"Parser was not a {nameof(TextParser)}. Got a {parser?.GetType()}.");

    Initialize(text_parser);
    CheckInputNull();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    Execute();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    AssignResult();

    return AdjustedStatus;
  }
  public override OpStatus DoOperation (TextParser parser) => DoOperation<TextParser>(parser);
  protected override void CheckInputNull ()
  {
    if (InputKey == SE || IgnoreAllLoads)
    {
      Debug.Log("TextOperation.CheckInputNull", $"No key checked.");
      Status = OpStatus.Skipped;
    }
    if (!Parser.Work.ContainsKey(InputKey))
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {InputKey} does not exist.");
      Status = OpStatus.FailNoSuchVarName;
    }
    else if (Parser.Work.TryGetValue(InputKey, out object? data) && data is not null)
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {InputKey} is not null.");
      Status = OpStatus.Pass;
    }
    else
    {
      Debug.Log("TextOperation.CheckInputNull", $"Key {InputKey} is null.");
      Status = OpStatus.FailBadInputNull;
    }
  }
  protected override void CheckInputsNull ()
  {
    foreach (string key in InputKeys)
    {
      if (!Parser.Work.ContainsKey(key))
      {
        Debug.Log("TextOperation.CheckInputsNull", $"Key {key} does not exist.");
        Status = OpStatus.FailNoSuchVarName;
        return;
      }
      else if (Parser.Work.TryGetValue(key, out object? data) && data is not null)
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
  /// Performs the operation and stores the value in <c><see cref="Operation.WorkToReturn"/></c>,
  /// and the <see cref="OpStatus"/> in <c><see cref="Operation.Status"/></c>
  /// </summary>
  protected override void Execute ()
  {
    if (WorkToReturn is null)
      ThrowNoOverrideError();

    object? data = WorkToReturn;
    Status = OpStatus.FailBadOpDefinition;
    WorkToReturn = data;
  }
  /// <summary>
  /// Assigns the parser to <c><see cref="Parser"/></c>.
  /// </summary>
  /// <param name="parser">The parser reference to pass to the operation.</param>
  [MemberNotNull(nameof(Parser))]
  protected override void Initialize (TextParser parser)
  {
    ArgumentNullException.ThrowIfNull(parser);
    Parser = parser;
    Spec = parser.Spec;

    if (IgnoreAllLoads)
    {
      WorkToReturn = null;
      return;
    }

    _ = loadKey(InputKey);

    object? loadKey (string input_key)
    {
      if (Parser.Work.TryGetValue(InputKey, out object? value))
      {
        Debug.Log("TextOperation.Initialize", $"Loaded {InputKey} with value {value}.");
        WorkToReturn = value;
      }
      else
      {
        Debug.Log("TextOperation.Initialize", $"Key {InputKey} does not exist or is null.");
        WorkToReturn = null;
        Status = OpStatus.FailNoSuchVarName;
      }
      return WorkToReturn;
    }

    if (InputKeys.Count > 1)
    {
      MultipleInputValues?.AddRange(InputKeys.Select(loadKey));

      if (MultipleInputValues?.Any(item => item is null) ?? true)
      {
        Status = OpStatus.FailBadInputNull;
      }
    }
  }
  protected virtual void AssignResult (DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (WorkToReturn is null) return;
    Parser.Mode = mode;
    Parser.Work.Add(OutputKey, WorkToReturn);
  }
}
