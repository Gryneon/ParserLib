using Common.Extensions;

namespace Parser.Ops;

public delegate OpStatus DoOperationFunction (ref object data);

public class Operation : IOperation
{
  #region Throwing Functions
  /// <summary>
  /// Throws an <see cref="NotImplementedException"/>. Use if you require an override in a class, and cannot make it abstract.
  /// </summary>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  protected static void ThrowNoOverrideError () => throw new NotImplementedException("This needs to be overridden by the inheriting class.");
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
  public virtual OpStatus DoOperation<TParser> (TParser parser_ref) where TParser : IParser => throw new NotImplementedException();
  #endregion
  /// <summary>
  /// The loaded data from the input keys if there are multiple keys provided.
  /// </summary>
  protected Collection<object?>? MultipleInputValues { get; }
  /// <summary>
  /// The object to be assigned to the output key at after the <c><see cref="Execute"/></c> step completes successfully.
  /// </summary>
  protected object? WorkToReturn { get; set; }
  /// <summary>
  /// A collection of all of the input keys. This will only contain one key if only one key is provided.
  /// </summary>
  protected Collection<string> InputKeys { get; }
  /// <summary>
  /// The input key provided, or the first input key if multiple are provided.
  /// </summary>
  protected string InputKey { get; set; }
  /// <summary>
  /// The output key provided.
  /// </summary>
  protected string OutputKey { get; set; }
  /// <summary>
  /// Whether or not this operation loads any data.
  /// </summary>
  public bool IgnoreAllLoads { get; protected set; }
  public OpStatus Status { get; protected set; } = OpStatus.Skipped;
  #region Operation Flags
  /// <inheritdoc/>
  public bool ContinueOnFail { get; set; }
  /// <inheritdoc/>
  public bool SkipOperation { get; set; }
  /// <inheritdoc/>
  public bool EndOperation { get; init; }
  /// <inheritdoc/>
  public bool DebugOperation { get; set; }
  #endregion

  /// <summary>
  /// Checks the parsers current working data, and sets the Status to <see cref="OpStatus.FailBadInputNull"/> if it is null.
  /// This method is for when one input is provided.
  /// </summary>
  protected virtual void CheckInputNull () => ThrowNoOverrideError();
  /// <summary>
  /// Checks the parsers current working data, and sets the Status to <see cref="OpStatus.FailBadInputNull"/> if it is null.
  /// This method is for when more than one input is provided.
  /// </summary>
  protected virtual void CheckInputsNull () => ThrowNoOverrideError();
  /// <inheritdoc/>
  protected virtual void Execute () => ThrowNoOverrideError();
  /// <summary>
  /// Sets the status to EndCommand or Skipped if the operation is flagged to be those.
  /// </summary>
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
  /// </returns>
  /// <exception cref="UnknownOperationException"/>
  public virtual OpStatus DoOperation (ref object data) =>
    EndOperation ? OpStatus.EndCommand : throw new UnknownOperationException();
  protected virtual bool CheckInput<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out T? casted)
  {
    if (Parser.Work.TryGetValue(InputKey, out object? item) && item is T temp)
    {
      Status = OpStatus.Pass;
      casted = temp;
      return true;
    }
    Status = OpStatus.FailBadInputType;
    casted = default;
    return false;
  }

  protected bool CheckInputs<T> ([NotNullWhen(true)][MaybeNullWhen(false)] out Collection<T> casted)
  {
    casted = [];
    for (int i = 0; i < InputKeys.Count; i++)
    {
      if (Parser.Work.TryGetValue(InputKeys[i], out object? temp) && temp is T temp2)
        casted.Add(temp2);
      else
      {
        Status = !Parser.Work.ContainsKey(InputKeys[i]) ? OpStatus.FailNoSuchVarName : OpStatus.FailBadInputType;
        return false;
      }
    }

    Status = OpStatus.Pass;
    return true;
  }
  protected virtual OpStatus AdjustedStatus =>
    Status is OpStatus.Skipped || Status.IsFail() && ContinueOnFail ? OpStatus.Skipped :
    EndOperation ? OpStatus.EndCommand :
    Status;
  /// <summary>
  /// A built in operation that ends the operation sequence.
  /// </summary>
  public static Operation End => new() { EndOperation = true };
  [AllowNull] protected IParser Parser { get; set; }
  /// <summary>
  /// Multiple input keys.
  /// </summary>
  protected Operation (IEnumerable<string> input_keys, string output_key)
  {
    InputKeys = [.. input_keys];

    if (InputKeys.Count == 0)
    {
      IgnoreAllLoads = true;
      InputKey = SE;
    }
    else
      InputKey = InputKeys[0];

    OutputKey = output_key;
  }
  /// <summary>
  /// Private constructor for the static <see cref="End"/> object.
  /// </summary>
  protected Operation ()
  {
    InputKey = SE;
    OutputKey = SE;
    InputKeys = [];
  }
  protected Operation (bool ignore_all_loads) : this() => IgnoreAllLoads = ignore_all_loads;
  /// <summary>
  /// Single input key.
  /// </summary>
  protected Operation (string input_key, string output_key)
  {
    if (input_key.IsEmpty())
      IgnoreAllLoads = true;
    InputKeys = [input_key];
    InputKey = input_key;
    OutputKey = output_key;
  }
  public bool Equals (IOperation? other) => EndOperation && (other?.EndOperation ?? false) || Equals(this, other);
}

public class Operation<TParser> : Operation where TParser : IParser
{
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
  /// </returns>
  /// <exception cref="UnknownOperationException"/>
  public virtual OpStatus DoOperation (TParser parser_ref) =>
    base.DoOperation(parser_ref);
  /// <summary>
  /// Initializes the operation. Sets <see cref="Parser"/>.
  /// </summary>
  /// <param name="parser"></param>
  protected virtual void Initialize (TParser parser) => Parser = parser;

  protected Operation (string input_key, string output_key) : base(input_key, output_key) { }
  protected Operation (IEnumerable<string> input_keys, string output_key) : base(input_keys, output_key) { }
  protected Operation (bool ignore_all_loads) : base(ignore_all_loads) { }
  protected override bool CheckInput<T> ([MaybeNullWhen(false), NotNullWhen(true)] out T? casted) where T : default =>
    base.CheckInput(out casted);
}
