#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser.Binary.Ops;
//TODO: Add the documentation for the byte loop.
/// <summary>
/// Represents an iterative loop, like to get objects from an archive, or read pixels.
/// </summary>
/// <param name="count">The key that determines the loop count.</param>
/// <param name="ops_loop">The operations to preform in this loop.</param>
public abstract class ByteOperationLoop (string count, IEnumerable<ByteOperation> ops_loop) : ByteOperation(EmptyString, EmptyString)
{
  public Collection<ByteOperation> Operations { get; init; } = [.. ops_loop];
  public string LoopCountVar { get; } = count;

  protected abstract int GetCount ();

  public override OpStatus DoOperation (ByteParser parser)
  {
    Initialize(parser);
    OpStatus status = OpStatus.Error;

    Collection<ByteOperation> ops = [.. Operations];
    int lpcount = GetCount();
    OpLoopData loop = new(ops, true, lpcount);

    int index = parser.OpIndex;
    parser.LoopData.Add(loop);

    int loopindex = 0;

    while (loopindex < lpcount && !status.IsFail())
    {
      status = parser.DoByteOperations();

      if (status.IsFail())
        break;

      loopindex++;
    }
    parser.LoopData.RemoveAt(parser.LoopDepth);
    parser.OpIndex = index;
    return AdjustedStatus;
  }
}
