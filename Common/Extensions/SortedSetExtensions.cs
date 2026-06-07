#pragma warning disable CS1591 // XML Documentation

namespace Common.Extensions;

public static class SortedSetExtensions
{
  extension(SortedSet<char>? set)
  {
    public string ToString ()
    {
      string s = SE;
      foreach (char c in set ?? [])
        s += c;
      return s;
    }
  }

  extension([NotNull] SortedSet<char> set)
  {
    public void Add (string chars) => set.UnionWith(chars);
  }
}
