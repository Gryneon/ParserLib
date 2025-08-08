#pragma warning disable CS1591 // XML Documentation

namespace Common.Extensions;

public static class SortedSetExtensions
{
  public static string ToString (this SortedSet<char>? set)
  {
    string s = SE;
    foreach (char c in set ?? [])
      s += c;
    return s;
  }
  public static void Add (this SortedSet<char> set, string chars) => set.UnionWith(chars);
}