using Common.Extensions;

namespace Specification.XML;

public sealed class XMLComment () : IGeneratable<MatchDataSet, XMLComment>, IXMLObject
{
  /// <summary>Comment content.</summary>
  public string Content { get; set; } = SE;
  string IXMLObject.Tag => Content;
  public static XMLComment Generate (MatchDataSet mdd)
  {
    mdd.ThrowIfNull();
    XMLComment result = new()
    {
      Content = mdd["content"].Content,
    };

    return result;
  }
}
