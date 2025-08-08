//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common;
public class Arg (string content, bool isOption)
{
  public string Content { get; init; } = content;
  public bool IsOption { get; init; } = isOption;

  public static implicit operator Arg (string str) => new(str, str.StartsWithAny(["-", "+"]));
}
public class ArgIterator : IReadOnlyCollection<string>
{
  public Collection<Arg> Args { get; } = [];
  public Collection<string> Files =>
    [.. Args.Where(item => !item.IsOption).Select(item => item.Content)];
  public Collection<string> Options =>
    [.. Args.Where(item => item.IsOption).Select(item => item.Content)];

  public int Count => Args.Count;
  public bool IsEmpty => Args.Count == 0;
  public bool HasFiles => Files.Count > 0;

  public void Add (Arg arg) => Args.Add(arg);

  public ArgIterator (string[] args)
  {
    foreach (string arg in args)
    {
      Add(arg);
    }
  }

  public IEnumerator<string> GetEnumerator () => Files.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}