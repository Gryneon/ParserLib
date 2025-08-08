#pragma warning disable IDE0072 // Add missing cases

namespace Parser.V2;

public class FileStorage (IEnumerable<string> files) : IFileStorage
{
  public int Count => Files.Count;
  public Collection<string> Files { get; } = [.. files];
  public bool HasData => Files.Count != 0;

  public FileStorage (string file) : this([file]) { }
  public void Add (string file) => Files.Add(file);
  public string? PopNext ()
  {
    if (Files.Count == 0)
      return null;

    string next = Files.First();
    Files.RemoveAt(0);
    return next;
  }
}