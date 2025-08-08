using Parser.Ops;

using OS = Parser.OpStatus;

namespace Specification.XML;

/// <summary>
/// Operation to stack the nodes inside each other.
/// </summary>
public class XMLStackOperation : Operation
{
  /// <inheritdoc/>
  public override OS DoOperation (ref object data)
  {
    OS result = OS.Error;
    Collection<IXMLObject> tree = [];

    int depth = 0;
    Collection<string> nodeNames = [];
    Collection<XMLNodeAttr> parentNodes = [];

    if (data is null)
      return OS.FailBadInputNull;

    if (data is not IEnumerable<IXMLObject> items)
      return OS.FailBadInputType;

    foreach (IXMLObject item in items)
    {
      if (item is XMLHeader)
        tree.Add(item);
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
        if (depth == 0)
          tree.Add(content);
        else
          parentNodes[depth - 1].Children.Add(content);
      else if (item is XMLElementSingle single)
        if (depth == 0)
          tree.Add(single);
        else
          parentNodes[depth - 1].Children.Add(single);
      else if (item is XMLElementClose close)
        if (depth == 0)
        {
          result = OS.FailBadOpResult;
          goto Finish;
        }
        else
        {
          if (!close.Tag.Equals(nodeNames.Last()))
          {
            result = OS.FailBadOpResult;
            goto Finish;
          }

          depth--;
          nodeNames.RemoveAt(depth);
          parentNodes.RemoveAt(depth);
        }
    }
    result = OS.Pass;

  Finish:
    data = tree;
    return result;
  }
}