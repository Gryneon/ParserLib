namespace Parser;

/// <summary>
/// Logically ORs the nodes this node contains.
/// </summary>
/// <param name="nodes">The nodes this node contains.</param>
public class InferenceNodeOr (IEnumerable<IInferenceNode> nodes) : InferenceNodeAnd(nodes, IT.Or)
{
  /// <summary>
  /// Check if the file at the specified location matches this node.
  /// </summary>
  /// <param name="filepath">The path to the file to check.</param>
  /// <returns><see langword="true"/> if the file matches the node, <see langword="false"/> otherwise.</returns>
  public override bool CheckFile (string filepath) => Nodes.Any(item => item.CheckFile(filepath));
}