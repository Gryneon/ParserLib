using Common.Extensions;

namespace Parser.Ops;

public delegate OpStatus DoOperationFunction (ref object data);

public class Operation : IOperation
{
  #region Throwing Functions
  [DoesNotReturn]
  protected static void ThrowNoOverrideError () => throw new UnknownOperationException("This needs to be overridden by the inheriting class.");
  [DoesNotReturn]
  protected static void ThrowUnusableOverrideError () => throw new UnknownOperationException("This overload cannot be used by this class.");
  [DoesNotReturn]
  protected static void ThrowBadParserError (object parser) => throw new ArgumentException($"Parser was not a ByteParser. Got a {parser.GetType()}.");
  #endregion
  public static Operation End => new() { EndOperation = true };
  protected Collection<object>? _multiple_input_values;
  protected object? _workToReturn;
  [NotNull] protected dynamic? _parser = null;
  /// <summary>
  /// A collection of all of the input keys. This will only contain one key if only one key is provided.
  /// </summary>
  protected Collection<string> _input_keys;
  /// <summary>
  /// The input key provided, or the first input key if multiple are provided.
  /// </summary>
  protected string _input_key;
  protected string _output_key;

  public bool IgnoreAllLoads { get; private set; }
  public OpStatus Status { get; protected set; } = OpStatus.Skipped;

  #region Operation Flags
  /// <inheritdoc/>
  public bool ContinueOnFail { get; init; }
  /// <inheritdoc/>
  public bool SkipOperation { get; init; }
  /// <inheritdoc/>
  public bool EndOperation { get; init; }
  /// <inheritdoc/>
  public bool StartOperation { get; init; }
  /// <inheritdoc/>
  public bool DebugOperation { get; init; }
  #endregion

  /// <summary>
  /// Performs an operation that uses and may alter or reassign the data.
  /// </summary>
  /// <returns>
  /// <see cref="OpStatus.Error"/> : The operation encountered a fatal error.<br/>
  /// <see cref="OpStatus.Pass"/> : The operation completed.<br/>
  /// <see cref="OpStatus.Skipped"/> : The operation was skipped or not executed. <br/>
  /// <see cref="OpStatus.FailBadInputNull"/> : The operation was given a null value. <br/>
  /// <see cref="OpStatus.FailBadInputType"/> : The operation was given an incompatible object as input. <br/>
  /// <see cref="OpStatus.FailBadOpDefinition"/> : The operation or specification definition has an error or is not valid. <br/>
  /// <see cref="OpStatus.FailBadOpImpossible"/> : The operation reached an impossible statement. <br/>
  /// <see cref="OpStatus.FailNullOpResult"/> : The operation resulted in a null value. <br/>
  /// <see cref="OpStatus.FailBufferOverflow"/> : The operation advanced beyond the EOL of the input. <br/>
  /// <see cref="OpStatus.FailNoSuchVarName"/> : The operation was supplied an invalid key.<br/>
  /// <see cref="OpStatus.FailNoSpec"/> : The operation does not have a valid <see cref="Spec"/>.<br/>
  /// <see cref="OpStatus.EndCommand"/> : The operation completed and was the final operation. <br/>
  /// <see cref="OpStatus.NoData"/> : No data was passed, but not a failure. <br/>
  /// </returns>
  /// <exception cref="UnknownOperationException"/>
  public virtual OpStatus DoOperation (ref object data) =>
    EndOperation ? OpStatus.EndCommand : throw new UnknownOperationException();

  /// <summary>
  /// Checks the parsers current working data, and sets the Status to <see cref="OpStatus.FailBadInputNull"/> if it is null.
  /// </summary>
  protected virtual void CheckInputNull () => ThrowNoOverrideError();
  protected virtual void CheckInputsNull () => ThrowNoOverrideError();
  protected virtual void Initialize (dynamic parser) => ThrowNoOverrideError();
  protected virtual void Execute () => ThrowNoOverrideError();
  protected virtual void CheckOperationFlags ()
  {
    if (EndOperation)
    {
      Debug.Log("Operation.CheckOperationFlags", "Ending operation sequence.");
      Status = OpStatus.EndCommand;
    }
    else if (SkipOperation)
    {
      Debug.Log("Operation.CheckOperationFlags", "Skipping operation.");
      Status = OpStatus.Skipped;
    }
  }
  protected virtual OpStatus AdjustedStatus =>
    Status is OpStatus.Skipped || Status.IsFail() && ContinueOnFail ? OpStatus.Skipped :
    EndOperation ? OpStatus.EndCommand :
    Status;
  /// <summary>
  /// Single input key.
  /// </summary>
  protected Operation (string input_key, string output_key)
  {
    if (input_key.IsEmpty())
      IgnoreAllLoads = true;
    _input_keys = [input_key];
    _input_key = input_key;
    _output_key = output_key;
  }
  /// <summary>
  /// Multiple input keys.
  /// </summary>
  protected Operation (IEnumerable<string> input_keys, string output_key)
  {
    _input_keys = [.. input_keys];

    if (_input_keys.Count == 0)
    {
      IgnoreAllLoads = true;
      _input_key = SE;
    }
    else
      _input_key = _input_keys[0];

    _output_key = output_key;
  }
  /// <summary>
  /// Private constructor for the static <see cref="End"/> object.
  /// </summary>
  private Operation ()
  {
    _input_key = SE;
    _output_key = SE;
    _input_keys = [];
  }
  protected Operation (bool ignore_all_loads) : this() => IgnoreAllLoads = ignore_all_loads;
}
