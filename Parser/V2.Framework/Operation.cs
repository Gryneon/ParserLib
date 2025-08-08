using OS = Parser.V2.Framework.OpStatus;

namespace Parser.V2.Framework;

public abstract class Operation : IOperation
{
  public bool ContinueOnFail { get; init; }
  public bool SkipOperation { get; init; }
  public virtual OperationType OpType { get; init; }
  public virtual Collection<Type> AllowedInputs
  { get; init; } = [typeof(object)];
  public virtual Collection<Type> PossibleOutputs { get; init; } = [typeof(object)];

  protected virtual OS CheckInput (object? data)
  {
    if (data is null)
      return OS.FailBadInputNull;

    foreach (Type t in AllowedInputs)
    {
      if (t.IsInstanceOfType(data))
        return OS.Pass;
    }
    return OS.FailBadInputType;
  }
  protected virtual OS CheckOutput (object? output)
  {
    if (output is null)
      return OS.FailNullOpResult;

    foreach (Type t in PossibleOutputs)
    {
      if (t.IsInstanceOfType(output))
        return OS.Pass;
    }
    return OS.FailBadOpResult;
  }
  protected virtual dynamic GetContent (DataStorage data, string? input)
  {
    if (input is null)
      return data.CurrentData;
    dynamic? content = data.GetKeyedData(input);
    return content is null
      ? throw new ArgumentNullException(nameof(input))
      : content;
  }
  /// <summary>
  /// Must be defined in operation to use, otherwise it returns an error.
  /// </summary>
  /// <param name="data">The data passed to the operation.</param>
  /// <param name="input">The key to load data from, or null to use CurrentData.</param>
  /// <param name="output">The key to write the data to, or null if it does not need to be written.</param>
  /// <returns>The <see cref="OpStatus"/> result. Returns OpStatus.FailBadOpDefinition is it was not defined.</returns>
  protected virtual OS DoSimpleOperation (DataStorage data, string? input, string? output) => OS.FailBadOpDefinition;
  protected virtual OS DoTransform (DataStorage data, string? input, string? output) => OS.FailBadOpDefinition;
  protected virtual OS DoStoreData (DataStorage data, string? input, string? output)
  {
    if (output is not null)
      data.AssignKeyedData(output, data.CurrentData);
    return OS.Pass;
  }
  protected virtual OS DoDebugCmd (DataStorage data, string? input, string? output) => OS.Skipped;
  public virtual OS DoOperation (DataStorage data, string? inputKey = null, string? outputKey = null)
  {
    OS inputStatus = CheckInput(inputKey is null ? data.CurrentData : data.GetKeyedData(inputKey));

    if (!ContinueOnFail && inputStatus != OS.Pass)
      return inputStatus;

    OS result;

    if (OpType is OperationType.NoData)
    {
      result = DoSimpleOperation(data, inputKey, outputKey);
      if (result.IsFail(ContinueOnFail))
        return result;
    }
    if (OpType.HasFlag(OperationType.TransformData))
    {
      result = DoTransform(data, inputKey, outputKey);
      if (result.IsFail(ContinueOnFail))
        return result;
    }
    if (OpType.HasFlag(OperationType.StoreData))
    {
      result = DoStoreData(data, inputKey, outputKey);
      if (result.IsFail(ContinueOnFail))
        return result;
    }
    if (OpType.HasFlag(OperationType.DebugOnly))
    {
      result = DoDebugCmd(data, inputKey, outputKey);
      if (result.IsFail(ContinueOnFail))
        return result;
    }

    OS outputStatus = CheckOutput(data.CurrentData);

    return outputStatus.IsFail(ContinueOnFail) ? outputStatus : OS.Pass;
  }
}
