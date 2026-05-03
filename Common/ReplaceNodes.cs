namespace Common;

public sealed class ReplaceNodes : KeyedCollection<string, ReplaceNode>
{
  public void Add ((string lf, string rw) t) => Add(new ReplaceNode(t.lf, t.rw));
  public void Add (string lookFor, string replaceWith) => Add(new ReplaceNode(lookFor, replaceWith));
  protected override string GetKeyForItem ([NotNull] ReplaceNode item) => item.LookFor;
  public string Invoke (string input, StringComparison sc) => this.Aggregate(input, (text, node) => text = node.ReplaceText(text, sc));
}
