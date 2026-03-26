using Parser.Exceptions;

namespace Parser.Ops;

/// <summary>
/// The abstract base class for operations. All operations should inherit from <see cref="Operation"/>.<br/>
/// All usage or references should be stored as <see cref="IOperation"/> objects.
/// </summary>
public abstract class Operation : IOperation
{
  #region Throwing Functions
  /// <summary>Throws an <see cref="InvalidOperationException"/> if condition is <see langword="true"/>.</summary>
  /// <param name="condition">The condition to check.</param>
  /// <param name="msg">The exception message.</param>
  /// <exception cref="InvalidOperationException"></exception>
  protected static void ThrowIf ([DoesNotReturnIf(true)] bool condition, string msg)
  {
    if (condition) throw new InvalidOperationException(msg);
  }
  /// <summary>Throws an <see cref="NotImplementedException"/>. Use if you require an override in a class, and cannot make it abstract.</summary>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  protected static void ThrowNoOverrideError () => throw new NotImplementedException();
  /// <summary>Throws an <see cref="NotImplementedException"/>. Use if you require an override in a class, and cannot make it abstract.</summary>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  protected static T ThrowNoOverrideError<T> () => throw new NotImplementedException("This needs to be overridden by the inheriting class.");
  /// <summary>Throws an <see cref="NotSupportedException"/>. Use if you must prevent a valid overload from a base class from being used.</summary>
  /// <exception cref="NotSupportedException"/>
  [DoesNotReturn]
  protected static void ThrowUnusableOverrideError () => throw new NotSupportedException("This overload cannot be used by this class.");
  /// <summary>Throws an <see cref="ArgumentException"/>. Use if the parser type was not correct and you cannot recover.</summary>
  /// <param name="parser">The parser object.</param>
  /// <param name="desired_parser">The type of parser you need.</param>
  /// <exception cref="ArgumentException"/>
  [DoesNotReturn]
  protected static void ThrowBadParserError (object parser, [NotNull] Type desired_parser) =>
    throw new ArgumentException($"Parser was not a {desired_parser.Name}. Got a {parser?.GetType()}.");
  #endregion
  #region Stored Keys & Data
  /// <summary>The loaded data from the input keys if there are multiple keys provided.</summary>
  protected Collection<object> MultipleInputValues { get; private set; } = [];
  /// <summary>A collection of all of the input keys. This will only contain one key if only one key is provided.</summary>
  [NotNull]
  protected Collection<string> InputKeys { get; private set; } = [];
  /// <summary>The input key provided, or the first input key if multiple are provided.</summary>
  [NotNull]
  protected string InputKey
  {
    get => InputKeys.IsEmpty() ? $"{Op.ThrowBadInput("string", "an empty input Collection")}" : InputKeys[0];
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
  /// <summary>The output key provided.</summary>
  protected string OutputKey { get; set; }
  /// <summary>The object to be assigned to the output key at after the <c><see cref="Execute"/></c> step completes successfully.</summary>
  [MemberNotNull(nameof(WorkDataType))]
  protected object? WorkData { get; set; }
  /// <summary>The status of the operation.</summary>
  protected OpStatus Status { get; set; } = OpStatus.Pass;
  protected bool MakeListOnSave { get; set; }
  protected Type? WorkDataType => WorkData?.GetType();
  #endregion
  #region Calculated Properties
  /// <summary>The adjusted status taking into account operation flags.</summary>
  protected virtual OpStatus AdjustedStatus =>
    Status is OpStatus.Skipped ? OpStatus.Skipped : Status.IsFail() && ContinueOnFail ? OpStatus.FailOverride : Status;
  #endregion
  #region Operation Flags
  public bool ContinueOnFail { get; set; }
  public bool SkipOperation { get; set; }
  public virtual bool NoOutput => SkipOperation;
  public virtual bool NoExecution => SkipOperation;
  /// <summary>Whether or not this operation loads a key from a <see cref="DataStore"/>.</summary>
  /// <remarks>Set this to false on any operation that does not use or load data.</remarks>

  [MemberNotNullWhen(false, nameof(InputKey), nameof(InputKeys), nameof(WorkData))]
  public virtual bool NoInput => InputKey.IsEmpty();
  #endregion
  #region Input Checks
  /// <summary>
  /// Checks the parsers current working data, and throws an <see cref="OperationNoSuchVarException"/> if it is missing.
  /// This method works for one or many input keys. It will check each one.
  /// </summary>
  /// <exception cref="OperationNoSuchVarException"/>
  protected void CheckInputNull ()
  {
    DebugIn("Operation", "CheckInputNull");
    if (InputKey == SE || NoInput)
    {
      Log(MsgClass.Debug, $"No key checked.");
      Status = OpStatus.Skipped;
      DebugOut();
      return;
    }
    foreach (string key in InputKeys)
    {
      if (!Parser.Data.CanLoad(key))
      {
        Log(MsgClass.Error, $"Key {key} does not exist.");
        Status = Op.ThrowNoVar(key);
      }
    }
    if (InputKeys.Count == 1)
      Log(MsgClass.Debug, $"Key {InputKey} is not null.");
    else
      Log(MsgClass.Debug, $"All keys are not null.");
    Status = OpStatus.Pass;
    DebugOut();
  }
  /// <summary>Checks if the data stored in <see cref="InputKey"/> is of type <typeparamref name="T"/>.</summary>
  /// <typeparam name="T">The type or interface to check against.</typeparam>
  /// <param name="casted">The data casted to the type specified.</param>
  /// <returns>Returns <see langword="true"/> if the data is of the correct type, <see langword="false"/> otherwise.</returns>
  [Obsolete("Use pattern matching with the WorkData or MultipleInputValues objects.")]
  [MemberNotNullWhen(true, nameof(InputKey), nameof(InputKeys))]
  protected virtual bool CheckInput<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out T casted)
  {
    DebugIn("Operation", "CheckInput");
    casted = default;
    if (NoInput)
    {
      DebugOut();
      return false;
    }

    if (Parser.Data.TryLoad(InputKey, out T? temp))
    {
      Status = OpStatus.Pass;
      casted = temp;
      DebugOut();
      return true;
    }
    Status = Op.ThrowBadInput($"{typeof(T)}", $"{Parser.Data[InputKey].GetType()}");
    return false;
  }
  #endregion
  #region Reference Properties
  /// <summary>The reference to the parser.</summary>
  [AllowNull] protected XParser Parser { get; private set; }
  /// <summary>The reference to the <see cref="DataStore"/>.</summary>
  [AllowNull] protected DataStore Data => Parser.Data;
  /// <summary>The reference to the specification.</summary>
  [AllowNull] protected Spec Spec => Parser.Spec;
  #endregion
  #region Constructors
  /// <summary>Multiple input keys.</summary>
  protected Operation (IEnumerable<string> input_keys, string output_key)
  {
    InputKeys = [.. input_keys];
    InputKey = !input_keys.Any() ? SE : InputKeys[0];
    OutputKey = output_key;
  }
  /// <summary>Constructor for the static <see cref="Op.End"/> object, and for operations that do not touch data.</summary>
  protected Operation ()
  {
    InputKey = SE;
    OutputKey = SE;
    InputKeys = [];
  }
  /// <summary>Single input key.</summary>
  protected Operation (string input_key, string output_key)
  {
    InputKeys = NoInput ? [] : [input_key];
    InputKey = input_key;
    OutputKey = output_key;
  }
  #endregion
  public OpStatus DoOperation (XParser parser_ref)
  {
    if (SkipOperation)
      return OpStatus.Skipped;

    Initialize(parser_ref);

    if (!NoInput)
      CheckInputNull();

    if (!NoExecution)
      Execute();

    if (!NoOutput)
      AssignResult();

    return AdjustedStatus;
  }
  /// <summary>
  /// Performs the operation and stores the value in <c><see cref="WorkData"/></c>,
  /// and the <c><see cref="OpStatus"/></c> in <c><see cref="Status"/></c>.<br/>
  /// Use <c><see cref="WorkData"/></c> for a single passed input, and
  /// <c><see cref="MultipleInputValues"/></c> for multiple values.<br/>
  /// If you output multiple values, set <c><see cref="NoOutput"/></c> to
  /// <c><see langword="true"/></c> and handle the data saving here.
  /// </summary>
  /// <exception cref="OperationException"/>
  /// <exception cref="OperationBadDefinitionException"/>
  protected virtual void Execute ()
  {
    Status = Op.ThrowBadDef("Method not overridden, or NoExecute not set.");
  }
  /// <summary>Assigns the <c><see cref="XParser"/></c> to this operation and loads the data for the operation to work on.</summary>
  /// <param name="parser">The parser reference to pass to the operation.</param>
  private void Initialize (XParser parser)
  {
    DebugIn("Initialize");
    parser.ThrowIfNull();
    Parser = parser;
    WorkData = null;

    if (NoInput)
      return;

    object loadkey (string key)
    {
      if (Parser.Data.TryLoad(key, out object? value))
      {
        return value;
      }
      else
      {
        Log(MsgClass.Error, $"Key {key} does not exist or is null.");
        return Op.ThrowNoVar(key);
      }
    }
    if (InputKeys.Count == 1)
    {
      WorkData = loadkey(InputKey);
    }
    else if (InputKeys.Count > 1)
    {
      MultipleInputValues = [.. InputKeys.Select(loadkey)];
    }
    DebugOut();
  }
  private void AssignResult ()
  {
    if (WorkData is null) return;

    if (!MakeListOnSave)
    {
      Parser.Data.Save(OutputKey, WorkData);
      return;
    }
    Parser.Data.Save(OutputKey, WorkData, DM.AddToCollection | DM.MakeCollection);
  }
}
