#pragma warning disable IDE0072 // Add missing cases

namespace Parser.V2;

public interface IFileStorage
{
  bool HasData { get; }
  Collection<string> Files { get; }
  int Count { get; }
  void Add (string filename);
  string? PopNext ();
}
