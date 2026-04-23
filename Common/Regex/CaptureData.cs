//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

/// <summary>Data for one capture.</summary>
public class CaptureData : IMatchItem, IEquatable<CaptureData>, IComparable<CaptureData>
{
  /// <inheritdoc/>
  public int Pos { get; init; } = -1;
  /// <inheritdoc/>
  public virtual int Len { get; init; }
  /// <inheritdoc/>
  public virtual int NextPos => Pos + Len;
  /// <inheritdoc/>
  public int EndPos => NextPos - 1;
  /// <inheritdoc/>
  public string Content { get; init; }
  /// <inheritdoc/>
  public virtual string Name { get; }
  /// <summary>The Capture index in the group.</summary>
  public virtual int Index { get; init; }
  /// <inheritdoc/>
  public virtual bool IsNull => Pos < 0 || Len <= 0 || Content.Length == 0;

  internal CaptureData ()
  {
    Content = SE;
    Name = SE;
  }
  /// <summary>Constructor from a <see cref="Capture"/>.</summary>
  /// <param name="c">The <see cref="Capture"/> to make this object from.</param>
  /// <param name="groupName">The name of the group.</param>
  /// <param name="index">The index of the capture.</param>
  public CaptureData (Capture? c, string groupName = EmptyString, int index = -1)
  {
    if (c is null)
    {
      Pos = -1;
      Len = 0;
      Content = SE;
    }
    else
    {
      Pos = c.Index;
      Len = c.Length;
      Content = c.Value;
    }
    Index = index;
    Name = groupName;
  }
  /// <summary>Manual constructor.</summary>
  /// <param name="content">The text of the capture.</param>
  /// <param name="pos">The position in the string.</param>
  /// <param name="len">The length of the string.</param>
  /// <param name="name">The name of the group.</param>
  /// <param name="index">The index of the capture.</param>
  protected CaptureData (string content, int pos, int len, string name, int index)
  {
    Name = name;
    Content = content;
    Pos = pos;
    Len = len;
    Index = index;
  }

  public override string ToString () => Content.ContainsAny([Chars.CRLF, Chars.LFs, Chars.CRs])
      ? $"\"{Content.Replace([Chars.CRLF, Chars.CRs, Chars.LFs], "<NL>")}\" @ {Pos} ({Len})"
      : $"\"{Content}\" @ {Pos} ({Len})";
  public bool Equals (CaptureData? other) => other is not null && Pos == other.Pos && Name == other.Name && Index == other.Index && Content.Is(other.Content);
  public override bool Equals (object? obj) => Equals(obj as CaptureData);
  public override int GetHashCode () => HashCode.Combine(Pos, Index, Name, Content);
  public int CompareTo (CaptureData? other) => Pos.CompareTo(other?.Pos);

  public static bool operator == (CaptureData left, CaptureData right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (CaptureData left, CaptureData right) => !(left == right);
  public static bool operator < (CaptureData left, CaptureData right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (CaptureData left, CaptureData right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (CaptureData left, CaptureData right) => left?.CompareTo(right) > 0;
  public static bool operator >= (CaptureData left, CaptureData right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
