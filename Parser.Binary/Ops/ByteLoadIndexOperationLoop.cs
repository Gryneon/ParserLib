#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser.Binary.Ops;

/// <summary>
/// Represents an iterative loop, like to get objects from an archive, or read pixels.
/// This one loads the count from the count key provided.
/// </summary>
/// <param name="count_varname">The key to count the values from.</param>
/// <param name="ops_loop">The operations to iterate through</param>
public class ByteLoadIndexOperationLoop (string count_varname, IEnumerable<ByteOperation> ops_loop) : ByteOperationLoop(count_varname, ops_loop)
{
  protected override int GetCount () => (BParser.Load<IEnumerable<object>>(LoopCountVar) ?? []).Count();
}
