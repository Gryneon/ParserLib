#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser.Binary.Ops;

/// <summary>
/// Represents an iterative loop, like to get objects from an archive, or read pixels.
/// This one loads the count from the number of objects in the collection stored in the count key provided.
/// </summary>
/// <param name="count_varname">The key that has the count stored.</param>
/// <param name="ops_loop">The operations to iterate through.</param>
public class ByteLoadDataOperationLoop (string count_varname, IEnumerable<ByteOperation> ops_loop) : ByteOperationLoop(count_varname, ops_loop)
{
  /// <summary>
  /// Gets the number of iterations this loop does.
  /// </summary>
  /// <returns>The </returns>
  protected override int GetCount () => BParser.Load<int>(LoopCountVar);
}
