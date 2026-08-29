namespace Parser.Inference;

public class InferenceNodeAnd : InferenceNode
{
  protected Collection<InferenceNode> Nodes { get; }

  public InferenceNodeAnd (IEnumerable<InferenceNode> nodes) : base(IT.And, SE) => Nodes = [.. nodes];
  protected InferenceNodeAnd (IEnumerable<InferenceNode> nodes, IT type) : base(type, SE) => Nodes = [.. nodes];

  public override bool CheckFile (string filepath) => Nodes.All(item => item.CheckFile(filepath));
}
