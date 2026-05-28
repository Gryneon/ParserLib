namespace Parser.Ops;

public class SampleOperation : Operation
{
  public override bool NoInput => true;
  protected string NewInputKey { get; init; }
  protected string ModInputKey { get; init; }
  protected string NewOutputKey { get; init; }
  protected string ModOutputKey { get; init; }
  public SampleOperation (string input_key, string input_key2, string output_key, string output_key2)
  {
    NewInputKey = input_key;
    ModInputKey = input_key2;
    NewOutputKey = output_key;
    ModOutputKey = output_key2;
  }
  protected override void Execute ()
  {
    Data[NewOutputKey] = NewInputKey;
    Data[ModOutputKey] = ModInputKey;
  }
}

/// <summary>
/// The abstract base class for operations. All operations should inherit from <see cref="Operation"/>.<br/>
/// All usage or references should be stored as <see cref="IOperation"/> objects.
/// </summary>
public abstract class Operation : IOperation
{
  private const string Area = nameof(Operation);
  #region Throwing Functions
  /// <summary>Throws an <see cref="OperationException"/> if condition is <see langword="true"/>.</summary>
  /// <param name="condition">The condition to check.</param>
  /// <param name="msg">The exception message.</param>
  /// <exception cref="OperationException"></exception>
  protected static void ThrowIf ([DoesNotReturnIf(true)] bool condition, string msg)
  {
    if (condition) throw new OperationException(msg);
  }
  #endregion
  #region Stored Keys & Data
  /// <summary>The input key provided.</summary>
  [NotNull]
  public string InputKey { get; set; }
  /// <summary>The output key provided.</summary>
  public string OutputKey { get; init; }
  /// <summary>The object to be assigned to the output key at after the <c><see cref="Execute"/></c> step completes successfully.</summary>
  [AllowNull]
  [MemberNotNull(nameof(WorkDataType))]
  protected object WorkData { get; set; }
  /// <summary>The status of the operation.</summary>
  protected OpStatus Status { get; set; } = OpStatus.Pass;
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

  [MemberNotNullWhen(false, nameof(InputKey), nameof(WorkData))]
  public virtual bool NoInput => InputKey.IsEmpty();
  #endregion
  #region Reference Properties
  /// <summary>The reference to the parser.</summary>
  [AllowNull] protected XParser Parser { get; private set; }
  /// <summary>The reference to the <see cref="DataStore"/>.</summary>
  [AllowNull] protected DataStore Data => Parser.Data;
  /// <summary>The reference to the specification.</summary>
  [AllowNull]
  protected Spec Spec
  {
    get
    {
      if (Parser.Spec is null)
        _ = Err.ThrowNoSpec("Spec is null and Operation executed.");
      return Parser.Spec;
    }
  }
  #endregion
  #region Constructors
  /// <summary>Constructor for a explicit operation object.</summary>
  protected Operation ()
  {
    InputKey = SE;
    OutputKey = SE;
  }
  /// <summary>Single input key.</summary>
  protected Operation (string input_key, string output_key)
  {
    InputKey = input_key;
    OutputKey = output_key;
  }
  #endregion
  public OpStatus DoOperation (XParser parser_ref)
  {
    DebugIn(Area, "DoOperation");
    if (SkipOperation)
      return OpStatus.Skipped;

    parser_ref.ThrowIfNull();
    Parser = parser_ref;
    WorkData = null;

    if (!NoInput)
      WorkData = Data[InputKey];

    if (!NoExecution)
    {
      DebugIn(this.TypeName, "Execute");
      Execute();
      DebugOut();
    }
    if (!NoOutput)
      Data[OutputKey] = WorkData;

    DebugOut();
    return AdjustedStatus;
  }
  /// <summary>
  /// Performs the operation and stores the value in <c><see cref="WorkData"/></c>,
  /// and the <c><see cref="OpStatus"/></c> in <c><see cref="Status"/></c>.<br/>
  /// If you output multiple values, set <c><see cref="NoOutput"/></c> to
  /// <c><see langword="true"/></c> and handle the data saving here.
  /// </summary>
  /// <exception cref="OperationException"/>
  /// <exception cref="OperationBadDefinitionException"/>
  protected virtual void Execute ()
  {
    Status = Err.ThrowBadDef("Method not overridden, or NoExecute not set.");
  }
  protected void CheckUnpacked (XParser parser)
  {
    if (parser?.OpIndex == 0)
      Status = Err.ThrowBadDef("Loop Pre-processing not complete.");
  }
  protected static IOperation JumpTo (int pos) => new OperationJump(pos);
}
