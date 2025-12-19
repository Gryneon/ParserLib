using Common.Extensions;

using Parser.Ops;

using OS = Parser.OpStatus;

namespace Specification.XML;

public sealed class XMLDocument : IXMLObject
{
  public string Tag { get; set; } = SE;
  public Collection<IXMLObject> Content { get; } = [];
  public XMLDocument (IEnumerable<IXMLObject> objects) => Content = [.. objects];
}

/// <summary>
/// Operation to stack the nodes inside each other.
/// </summary>
public class XMLStackOperation (string input_key, string output_key) : Operation(input_key, output_key)
{
  /// <inheritdoc/>
  protected override void Execute ()
  {
    if (!CheckInput(out IEnumerable<IXMLObject>? items))
    {
      Status = OS.FailBadInputType;
      return;
    }
    OS result = OS.Error;
    Collection<IXMLObject> tree = [];

    int depth = 0;
    Collection<string> nodeNames = [];
    Collection<XMLNodeAttr> parentNodes = [];

    foreach (IXMLObject item in items)
    {
      if (item is XMLHeader)
      {
        tree.Add(item);
      }
      else if (item is XMLElementOpen open)
      {
        if (depth == 0)
          tree.Add(open);
        else
          parentNodes[depth - 1].Children.Add(open);
        parentNodes.Add(open);
        nodeNames.Add(open.Tag);
        depth++;
      }
      else if (item is XMLContent content)
      {
        if (depth == 0)
          tree.Add(content);
        else
          parentNodes[depth - 1].Children.Add(content);
      }
      else if (item is XMLElementSingle single)
      {
        if (depth == 0)
          tree.Add(single);
        else
          parentNodes[depth - 1].Children.Add(single);
      }
      else if (item is XMLElementClose close)
      {
        if (depth == 0)
        {
          result = OS.FailBadOpResult;
          goto Finish;
        }
        else
        {
          if (!close.Tag.Equals(nodeNames.Pop(), SCO))
          {
            result = OS.FailBadOpResult;
            goto Finish;
          }

          depth--;
          parentNodes.Drop();
        }
      }
      else if (item is XMLComment comment)
      {
        if (depth == 0)
          tree.Add(comment);
        else
          parentNodes[depth - 1].Children.Add(comment);
      }
    }
    result = OS.Pass;

  Finish:
    WorkToReturn = new XMLDocument(tree);
    Status = result;
  }
}
