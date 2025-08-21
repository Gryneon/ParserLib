#pragma warning disable IDE0060 // Remove unused parameter

using Parser.Ops;

using static Parser.OpStatus;

namespace Parser.Binary.Ops;

/// <summary>
/// Base class for all byte operations. must expose parser, do not use other methods.
/// </summary>
public abstract class ByteOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  protected ByteParser BParser => (_parser as ByteParser)!;
  protected ByteDataDictionary BData => BParser.ByteObjects;
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
  /// <see cref="OpStatus.Error"/> : The operation encountered a fatal error.<br/>
  /// <see cref="OpStatus.Pass"/> : The operation completed.<br/>
  /// <see cref="OpStatus.Skipped"/> : The operation was skipped or not executed. <br/>
  /// <c>Unused: </c><see cref="OpStatus.FailBadInputNull"/> : The operation was given a null value. <br/>
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
  /// <exception cref="UnknownOperationException"></exception>
  public virtual OpStatus DoOperation (ByteParser parser)
  {
    CheckOperationFlags();
    Initialize(parser);
    CheckInputNull();

    if (Status.IsFail(ContinueOnFail)) return Status;

    Execute();

    return Status;
  }

  protected override void Initialize (dynamic parser)
  {
    if (parser is not ByteParser)
      ThrowBadParserError(parser);

    _parser = parser;
  }
  protected override void CheckInputNull ()
  {
    if (BParser.ByteObjects is null)
    {
      Log("ByteOperation.CheckInputNull", "Input is null.");
      Status = FailBadInputNull;
    }
    else
      Status = Pass;
  }
  protected override void Execute () => ThrowNoOverrideError();
}
