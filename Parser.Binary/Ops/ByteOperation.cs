#pragma warning disable IDE0060 // Remove unused parameter

using Parser.Ops;

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

/// <summary>
/// Base class for all byte operations. must expose parser, do not use other methods.
/// </summary>
public abstract class ByteOperation (string input_key, string output_key) : Operation<ByteParser>(input_key, output_key)
{
  [AllowNull] public new ByteParser Parser { get; protected set; }
  protected ByteDataDictionary BData => Parser.ByteObjects;
  #region Unused Overrides
  /// <summary>Do not use.</summary>
  /// <returns>Throws exception.</returns>
  /// <exception cref="UnknownOperationException"></exception>
  public sealed override OpStatus DoOperation (ref object data)
  {
    ThrowUnusableOverrideError();
    return Error;
  }
  #endregion
  /// <summary>
  /// Performs an operation that uses and may alter or reassign the data.
  /// </summary>
  /// <returns>
  /// <see cref="Error"/> : The operation encountered a fatal error.<br/>
  /// <see cref="Pass"/> : The operation completed.<br/>
  /// <see cref="Skipped"/> : The operation was skipped or not executed. <br/>
  /// <c>Unused: </c><see cref="FailBadInputNull"/> : The operation was given a null value. <br/>
  /// <see cref="FailBadInputType"/> : The operation was given an incompatible object as input. <br/>
  /// <see cref="FailBadOpDefinition"/> : The operation or specification definition has an error or is not valid. <br/>
  /// <see cref="FailBadOpImpossible"/> : The operation reached an impossible statement. <br/>
  /// <see cref="FailNullOpResult"/> : The operation resulted in a null value. <br/>
  /// <see cref="FailBufferOverflow"/> : The operation advanced beyond the EOL of the input. <br/>
  /// <see cref="FailNoSuchVarName"/> : The operation was supplied an invalid key.<br/>
  /// <see cref="FailNoSpec"/> : The operation does not have a valid <see cref="Spec"/>.<br/>
  /// <see cref="EndCommand"/> : The operation completed and was the final operation. <br/>
  /// </returns>
  /// <exception cref="UnknownOperationException"></exception>
  public OpStatus DoOperation<TParser> (ByteParser parser)
  {
    CheckOperationFlags();
    Initialize(parser);
    CheckInputNull();

    if (Status.IsFail(ContinueOnFail)) return Status;

    Execute();

    return Status;
  }
  [MemberNotNull(nameof(Parser))]
  protected override void Initialize (ByteParser parser) => Parser = parser;
  protected override void CheckInputNull ()
  {
    if (Parser.ByteObjects is null)
    {
      Log("ByteOperation.CheckInputNull", "Input is null.");
      Status = FailBadInputNull;
    }
    else
      Status = Pass;
  }
  protected override void Execute () => ThrowNoOverrideError();
}
