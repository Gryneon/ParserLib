//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

/// <summary>
/// Data for one capture.
/// </summary>
public class CaptureData : IMatchItem
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
  /// <summary>
  /// The Capture index in the group.
  /// </summary>
  public virtual int Index { get; init; }
  /// <inheritdoc/>
  public virtual bool IsNull => Pos < 0 || Len <= 0 || Content.Length == 0;

  internal CaptureData ()
  {
    Content = SE;
    Name = SE;
  }
  /// <summary>
  /// Constructor from a <see cref="Capture"/>.
  /// </summary>
  /// <param name="c">The <see cref="Capture"/> to make this object from.</param>
  /// <param name="groupName"></param>
  /// <param name="index"></param>
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
  /// <summary>
  /// Manual constructor.
  /// </summary>
  /// <param name="content"></param>
  /// <param name="pos"></param>
  /// <param name="len"></param>
  /// <param name="name"></param>
  /// <param name="index"></param>
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
}
