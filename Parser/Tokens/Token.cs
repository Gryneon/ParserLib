#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class Token : IToken, IPrintable
{
  public TokenRef? AssignTo { get; set; }
  public string Content { get; set; } = SE;
  // Calculated Properties
  public int LastPosition => Index + Length - 1;
  public int Length => Content.Length;
  public bool Exempt { get; set; }
  public int Index { get; init; }
  public string Type { get; set; } = SE;
  public IList<IToken> Children { get; set; } = [];
  public virtual int Count => Children.Count;
  public bool HasType => Type is not null;
  public override string ToString () => $"{Type} : {Content}";
  public string ToString (int indent) => $"{Type} : {Content}";
  public void Print (int indent)
  {
    LogPart(MsgClass.Forced, $"{Type}");
    LogPart(MsgClass.BlueInfo, $" : ");
    if (Count is 0 or 1)
    {
      LogPart(MsgClass.GreenInfo, Content);
    }
    else
    {
      foreach (IToken t in Children)
      {
        NewLine();
        LogPart(MsgClass.Forced, new(' ', indent));
        t.Print(indent + IPrintable.TabSize);
      }
    }
  }
  public override bool Equals (object? obj) => obj is IToken rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public virtual bool Equals (IToken? other) => GetHashCode() == other?.GetHashCode();
  public static bool operator == (Token left, IToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (Token left, IToken right) => !(left == right);
  public static bool operator < (Token left, IToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (Token left, IToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (Token left, IToken right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (Token left, IToken right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
