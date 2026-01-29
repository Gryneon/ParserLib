namespace Parser.Ops;

/// <summary>
/// The abstract base class for operations. All operations should inherit from <see cref="Operation"/>.<br/>
/// All usage or references should be stored as <see cref="IOperation"/> objects.
/// </summary>
public abstract class Operation : IOperation
{
  #region Throwing Functions
  /// <summary>
  /// Throws an <see cref="InvalidOperationException"/> if condition is <see langword="true"/>.
  /// </summary>
  /// <param name="condition">The condition to check.</param>
  /// <param name="msg">The exception message.</param>
  /// <exception cref="InvalidOperationException"></exception>
  protected static void ThrowIf ([DoesNotReturnIf(true)] bool condition, string msg)
  {
    if (condition) throw new InvalidOperationException(msg);
  }
  /// <summary>
  /// Throws an <see cref="NotImplementedException"/>. Use if you require an override in a class, and cannot make it abstract.
  /// </summary>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  protected static void ThrowNoOverrideError () => throw new NotImplementedException();
  /// <summary>
  /// Throws an <see cref="NotImplementedException"/>. Use if you require an override in a class, and cannot make it abstract.
  /// </summary>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  protected static T ThrowNoOverrideError<T> () => throw new NotImplementedException("This needs to be overridden by the inheriting class.");
  /// <summary>
  /// Throws an <see cref="NotSupportedException"/>. Use if you must prevent a valid overload from a base class from being used.
  /// </summary>
  /// <exception cref="NotSupportedException"/>
  [DoesNotReturn]
  protected static void ThrowUnusableOverrideError () => throw new NotSupportedException("This overload cannot be used by this class.");
  /// <summary>
  /// Throws an <see cref="ArgumentException"/>. Use if the parser type was not correct and you cannot recover.
  /// </summary>
  /// <param name="parser">The parser object.</param>
  /// <param name="desired_parser">The type of parser you need.</param>
  /// <exception cref="ArgumentException"/>
  [DoesNotReturn]
  protected static void ThrowBadParserError (object parser, [NotNull] Type desired_parser) =>
    throw new ArgumentException($"Parser was not a {desired_parser.Name}. Got a {parser?.GetType()}.");
  #endregion
  #region Static Operation Methods & Properties
  public static IOperation JumpTo (string label) => new OperationAction(OAT.GotoLabel, label);
  public static IOperation JumpToFirst () => new OperationAction(OAT.GotoFirst);
  public static IOperation JumpTo (int index) => new OperationAction(OAT.GotoIndex, index);
  public static IOperation JumpIf (int index, ICondition condition) => new OperationAction(OAT.JumpIf, index, condition);

  public static IOperation Fail () => new OperationAction(OAT.ForceFail);
  public static IOperation Done () => new OperationAction(OAT.ForcePass);
  public static IOperation Prompt () => new OperationAction(OAT.Prompt);

  public static IOperation EraseKey (string key) => new OperationAction(OAT.EraseKey, key);
  public static IOperation StoreKey (string key) => new OperationAction(OAT.StoreKey, key);
  public static IOperation DebugKey (string key) => new OperationAction(OAT.DebugKey, key);
  public static IOperation CopyKey (string key, string to) => new OperationAction(OAT.CopyKey, key, to);
  public static IOperation SetResultKey (string key) => new OperationAction(OAT.CopyKey, key, "result");

  public static IOperation BreakLoop () => new OperationAction(OAT.BreakLoop);
  public static IOperation StartLoop (LoopOperation loopOperation) => new OperationAction(OAT.StartLoop, loopOperation);
  public static IOperation NextLoop (int increment = 1) => new OperationAction(OAT.NextLoop, increment);
  public static IOperation ContinueLoop (int increment = 1) => new OperationAction(OAT.ContinueLoop, increment);

  public static IOperation ClearCursor () => new OperationAction(OAT.ClearCursor);
  public static IOperation CreateCursor (string key, int start_at = 0) => new OperationAction(OAT.CreateCursor, key, start_at);
  public static IOperation SetCursor (int position) => new OperationAction(OAT.SetCursor, position);

  public static IOperation While (IEnumerable<IOperation> operations, ICondition condition) => new LoopOperation(operations)
  {
    Type = LoopType.While,
    Condition = condition,
    Count = null
  };
  public static IOperation Until (IEnumerable<IOperation> operations, ICondition condition) => new LoopOperation(operations)
  {
    Type = LoopType.Until,
    Condition = condition,
    Count = null
  };
  public static IOperation ForEach (IEnumerable<IOperation> operations, string cursor_key) => new LoopOperation(operations)
  {
    Type = LoopType.ForEach,
    CursorKey = cursor_key,
    Count = null
  };
  public static IOperation ForCount (IEnumerable<IOperation> operations, string cursor_key) => new LoopOperation(operations)
  {
    Type = LoopType.ForCount,
    CursorKey = cursor_key,
    Count = null
  };
  public static IOperation ForCount (IEnumerable<IOperation> operations, string count_key, int count) => new LoopOperation(operations)
  {
    Type = LoopType.ForCount,
    CursorKey = count_key,
    Count = count
  };

  /// <summary>
  /// A built in operation that ends the operation sequence.
  /// </summary>
  public static IOperation End => new OperationAction(OperationActionType.ForcePass);
  #endregion
  #region Stored Keys & Data
  /// <summary>
  /// The loaded data from the input keys if there are multiple keys provided.
  /// </summary>
  protected Collection<object?> MultipleInputValues { get; } = [];
  /// <summary>
  /// A collection of all of the input keys. This will only contain one key if only one key is provided.
  /// </summary>
  protected Collection<string> InputKeys { get; } = [];
  /// <summary>
  /// The input key provided, or the first input key if multiple are provided.
  /// </summary>
  [NotNullIfNotNull(nameof(InputKeys))]
  protected string? InputKey
  {
    get => InputKeys.IsEmpty() ? null : InputKeys[0];
    set
    {
      value ??= SE;
      if (InputKeys.IsEmpty())
      {
        InputKeys.Add(value);
      }
      else
      {
        InputKeys[0] = value;
      }
    }
  }
  /// <summary>
  /// The output key provided.
  /// </summary>
  protected string OutputKey { get; set; }
  /// <summary>
  /// The object to be assigned to the output key at after the <c><see cref="Execute"/></c> step completes successfully.
  /// </summary>
  protected object? WorkToReturn { get; set; }
  /// <summary>
  /// The status of the operation.
  /// </summary>
  protected OpStatus Status { get; set; } = OpStatus.Skipped;
  #endregion
  #region Calculated Properties
  /// <summary>
  /// The adjusted status taking into account operation flags.
  /// </summary>
  protected virtual OpStatus AdjustedStatus =>
    Status is OpStatus.Skipped ? OpStatus.Skipped : Status.IsFail() && ContinueOnFail ? OpStatus.FailOverride : Status;
  #endregion
  #region Operation Flags
  /// <inheritdoc/>
  public bool ContinueOnFail { get; set; }
  /// <inheritdoc/>
  public bool SkipOperation { get; set; }
  public bool NeverExecutes { get; }
  /// <summary>
  /// Whether or not this operation loads any data.
  /// </summary>

  [MemberNotNullWhen(false, nameof(InputKey), nameof(InputKeys), nameof(WorkToReturn))]
  public bool IgnoreAllLoads => InputKey.IsEmpty();
  #endregion
  #region Input Checks
  /// <summary>
  /// Checks the parsers current working data, and sets the Status to <see cref="OpStatus.FailNoSuchVarName"/> if it is missing.
  /// This method is for when one input is provided.
  /// </summary>

  protected void CheckInputNull ()
  {
    if (InputKey == SE || IgnoreAllLoads)
    {
      Log("Operation.CheckInputNull", $"No key checked.");
      Status = OpStatus.Skipped;
    }
    else if (!Data.CanLoad(InputKey))
    {
      Log("Operation.CheckInputNull", $"Key {InputKey} does not exist.");
      Status = OpStatus.FailNoSuchVarName;
    }
    else if (Data.CanLoad(InputKey))
    {
      Log("Operation.CheckInputNull", $"Key {InputKey} is not null.");
      Status = OpStatus.Pass;
    }
  }
  /// <summary>
  /// Checks the parsers current working data, and sets the Status to <see cref="OpStatus.FailNoSuchVarName"/> if it is missing.
  /// This method is for when more than one input is provided.
  /// </summary>
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected bool CheckInputsNull ()
  {
    if (InputKeys is null)
    {
      Status = OpStatus.FailBadInputNull;
      return false;
    }

    foreach (string key in InputKeys)
    {
      if (!Parser.Data.ContainsKey(key))
      {
        Debug.Log("Operation.CheckInputsNull", $"Key {key} does not exist.");
        Status = OpStatus.FailNoSuchVarName;
        return false;
      }
      else if (Parser.Data.TryGetValue(key, out object? data) && data is not null)
      {
        continue;
      }
      else
      {
        Debug.Log("Operation.CheckInputsNull", $"Key {key} is null.");
        Status = OpStatus.FailBadInputNull;
        return false;
      }
    }
    Debug.Log("Operation.CheckInputsNull", $"All keys are not null.");
    Status = OpStatus.Pass;
    InputKey ??= SE;
    return true;
  }
  /// <summary>
  /// Sets the status to EndCommand or Skipped if the operation is flagged to be those.
  /// </summary>
  protected virtual void CheckOperationFlags ()
  {
    if (SkipOperation)
    {
      Debug.Log("Operation.CheckOperationFlags", "Skipping operation.");
      Status = OpStatus.Skipped;
    }
  }
  /// <summary>
  /// Checks if the data stored in <see cref="InputKey"/> is of type <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">The type or interface to check against.</typeparam>
  /// <param name="casted">The data casted to the type specified.</param>
  /// <returns>Returns <see langword="true"/> if the data is of the correct type, <see langword="false"/> otherwise.</returns>
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected virtual bool CheckInput<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out T casted)
  {
    if (!IgnoreAllLoads && Parser.Data.TryLoad(InputKey, out T? temp))
    {
      Status = OpStatus.Pass;
      casted = temp;
      return true;
    }
    Status = OpStatus.FailBadInputType;
    casted = default;
    return false;
  }
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected virtual bool CheckArray<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out IEnumerable<T> casted)
  {
    if (!IgnoreAllLoads && Parser.Data.TryLoadArray(InputKey, out IEnumerable<T>? temp))
    {
      Status = OpStatus.Pass;
      casted = temp;
      return true;
    }
    Status = OpStatus.FailBadInputType;
    casted = default;
    return false;
  }
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected virtual bool CheckArray ([NotNullWhen(true)][MaybeNullWhen(false)] out IEnumerable casted)
  {
    if (!IgnoreAllLoads && Parser.Data.TryLoadArray(InputKey, out IEnumerable? temp))
    {
      Status = OpStatus.Pass;
      casted = temp;
      return true;
    }
    Status = OpStatus.FailBadInputType;
    casted = default;
    return false;
  }
  /// <summary>
  /// Checks all the inputs provided and validates them to a common class or interface.
  /// </summary>
  /// <typeparam name="T">The common class or interface.</typeparam>
  /// <param name="casted">The collection of inputs.</param>
  /// <returns>Returns <see langword="true"/> if the check passed, <see langword="false"/> otherwise.</returns>
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected bool CheckInputs<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out Collection<T> casted)
  {
    casted = [];
    if (InputKeys is null) throw new InvalidOperationException();
    for (int i = 0; i < InputKeys.Count; i++)
    {
      if (Parser.Data.TryLoad(InputKeys[i], out T? temp))
        casted.Add(temp);
      else
      {
        Status = !Parser.Data.ContainsKey(InputKeys[i]) ? OpStatus.FailNoSuchVarName : OpStatus.FailBadInputType;
        return false;
      }
    }

    Status = OpStatus.Pass;
    InputKey ??= SE;
    return true;
  }
  #endregion
  #region Reference Properties
  /// <summary>
  /// The reference to the parser.
  /// </summary>
  [AllowNull] protected XParser Parser { get; set; }
  /// <summary>
  /// The reference to the <see cref="DataDictionary"/>.
  /// </summary>
  [AllowNull] protected DataDictionary Data => Parser.Data;
  /// <summary>
  /// The reference to the specification.
  /// </summary>
  [AllowNull] protected Spec Spec { get; set; }
  #endregion
  #region State Properties
  /// <summary>
  /// The jump target of any <see cref="OperationActionType.BreakLoop"/> action that is called.
  /// </summary>
  public int LoopBreak { get; set; }
  /// <summary>
  /// The jump target of any <see cref="OperationActionType.ContinueLoop"/> and the beginning of the loop section.
  /// </summary>
  public int LoopStart { get; set; }
  #endregion
  #region Constructors
  /// <summary>
  /// Multiple input keys.
  /// </summary>
  protected Operation (IEnumerable<string> input_keys, string output_key)
  {
    InputKeys = [.. input_keys];
    InputKey = !input_keys.Any() ? SE : InputKeys[0];
    OutputKey = output_key;
  }
  /// <summary>
  /// Constructor for the static <see cref="End"/> object, and for operations that do not touch data.
  /// </summary>
  protected Operation ()
  {
    InputKey = SE;
    OutputKey = SE;
    InputKeys = [];
  }
  /// <summary>
  /// Single input key.
  /// </summary>
  protected Operation (string input_key, string output_key)
  {
    InputKeys = IgnoreAllLoads ? [] : [input_key];
    InputKey = input_key;
    OutputKey = output_key;
  }
  #endregion
  public virtual OpStatus DoOperation (XParser parser_ref)
  {
    if (SkipOperation)
      return OpStatus.Skipped;

    Initialize(parser_ref);
    CheckInputNull();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    Execute();

    if (Status.IsFail(ContinueOnFail)) return AdjustedStatus;

    AssignResult();

    return AdjustedStatus;
  }
  /// <summary>
  /// Performs the operation and stores the value in <c><see cref="Operation.WorkToReturn"/></c>,
  /// and the <see cref="OpStatus"/> in <c><see cref="Operation.Status"/></c><br/>
  /// Use <c><see cref="CheckInput{T}(out T)"/></c> or <see cref="CheckArray{T}(out IEnumerable{T})"/> to validate single variables.
  /// Use <see cref="CheckInputs{T}(out Collection{T})"/> to validate mulitple.
  /// </summary>
  protected virtual void Execute ()
  {
    ThrowIf(WorkToReturn is null, "This needs to be overridden by the inheriting class.");

    object data = WorkToReturn;
    Status = OpStatus.FailBadOpDefinition;
    WorkToReturn = data;
  }
  /// <summary>
  /// Assigns the <c><see cref="XParser"/></c> to this operation and loads the data for the operation to work on.
  /// </summary>
  /// <param name="parser">The parser reference to pass to the operation.</param>
  protected void Initialize (XParser parser)
  {
    parser.ThrowIfNull();
    Parser = parser;
    Spec = parser.Spec;

    if (IgnoreAllLoads)
    {
      WorkToReturn = null;
      return;
    }

    object? loadKey (string input_key)
    {
      if (Parser.Data.TryGetValue(InputKey, out object? value))
      {
        Log("Operation.Initialize", $"Loaded {InputKey} with value {value}.");
        WorkToReturn = value;
      }
      else
      {
        Log("Operation.Initialize", $"Key {InputKey} does not exist or is null.");
        WorkToReturn = null;
        Status = OpStatus.FailNoSuchVarName;
      }
      return WorkToReturn;
    }

    object? data = loadKey(InputKey);

    if (InputKeys.Count > 1)
    {
      MultipleInputValues?.AddRange(InputKeys.Select(loadKey));

      if (MultipleInputValues?.Any(item => item is null) ?? true)
      {
        Status = OpStatus.FailBadInputNull;
      }
    }
  }
  protected void AssignResult (DictionaryMode mode = DictionaryMode.Overwrite)
  {
    if (WorkToReturn is null) return;
    Parser.Data.Add(OutputKey, WorkToReturn);
  }
}
