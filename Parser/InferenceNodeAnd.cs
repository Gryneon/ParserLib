namespace Parser;

public class InferenceNodeAnd : IInferenceNode
{
  protected Collection<IInferenceNode> Nodes { get; }

  public IT Type { get; }

  public InferenceNodeAnd (IEnumerable<IInferenceNode> nodes)
  {
    Nodes = [.. nodes];
    Type = IT.And;
  }
  protected InferenceNodeAnd (IEnumerable<IInferenceNode> nodes, IT type)
  {
    Nodes = [.. nodes];
    Type = type;
  }

  public virtual bool CheckFile (string filepath) => Nodes.All(item => item.CheckFile(filepath));
}
