using Common;
using Common.Extensions;

namespace Specification.XML;

public class XMLString () : ITextSerializer
{
  private Collection<string> Elements { get; } = [];
  private Collection<string> Closing { get; } = [];

  public bool IsComplete => Closing.Count == 0 && Elements.Count > 0;

  public void AddElementOpen (string name)
  {
    Elements.Add($"<{name}>");
    Closing.Add($"</{name}>");
  }
  public void AddElementSingle (string name) => Elements.Add($"<{name}/>");
  public void AddElementSingle (string name, Collection<IProperty<string>> attributes)
  {
    Elements.Add($"<{name} ");

    attributes ??= [];
    foreach (IProperty<string> p in attributes)
    {
      Elements.Add($"{p.Key}=\"{p.Value}\" ");
    }

    Elements.Add($"/>");
  }
  public void AddElementOpen (string name, Collection<IProperty<string>> attributes)
  {
    Closing.Add($"</{name}>");
    Elements.Add($"<{name} ");

    attributes ??= [];
    foreach (IProperty<string> p in attributes)
    {
      Elements.Add($"{p.Key}=\"{p.Value}\" ");
    }

    Elements.Add($">");
  }
  public void CloseLastElement () => Elements.Add(Closing.Pop());
  public void AddContent (string content) => Elements.Add(content);
  public void AddLineFeed ()
  {
    Elements.Add("\n");
    int depth = Closing.Count;
    Elements.Add(new(' ', depth * 2));
  }
  public void CloseAllElements ()
  {
    while (Closing.Count > 0)
      CloseLastElement();
  }

  public string Serialize () => ToString();
  public override string ToString ()
  {
    string result = string.Empty;
    foreach (string element in Elements)
      result += element;
    return result;
  }
}
