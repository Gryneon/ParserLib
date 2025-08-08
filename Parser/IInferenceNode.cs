namespace Parser;

/// <summary>
/// A node specifiing a property of the files that use the parent format.
/// </summary>
public interface IInferenceNode
{
  /// <summary>
  /// The type of node.
  /// </summary>
  IT Type { get; }
  /// <summary>
  /// Check if the file at the specified location matches this node.
  /// </summary>
  /// <param name="filepath">The path to the file to check.</param>
  /// <returns><see langword="true"/> if the file matches the node, <see langword="false"/> otherwise.</returns>
  bool CheckFile (string filepath);
}
