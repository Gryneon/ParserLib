#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Common;

/// <summary>Reresents an object sortable by its index.</summary>
public interface IIndexSortable
{
  /// <summary>The index of this object.</summary>
  int Index { get; }
  /// <summary>Checks if the index is valid.</summary>
  bool IsValidIndex => Index >= 0;
}

public static class ConsolePosition
{
  public static int Line { get; set; }
  public static int Column { get; set; }

  public static void WriteLine (string line_text)
  {
    Console.WriteLine(line_text);
    Line++;
    Column = 0;
  }
  public static void Write (string line_text)
  {
    line_text ??= SE;

    Console.Write(line_text);
    Column += line_text.Length;
  }
}
