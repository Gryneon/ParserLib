#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE1006 // Naming Styles

using System.Xml.Linq;

namespace Parser.Tokens;

public abstract class TokenBase : IToken
{
  public virtual string Content
  {
    get => Children.Select(static t => t.Content).TextJoin();
    set => throw new NotSupportedException("Must override Content to set.");
  }
  public bool Ignored { get; set; }
  public bool Exempt { get; set; }
  public int Index { get; init; }
  public string Type { get; set; } = SE;
  public IReadOnlyList<IToken> Children { get; init; } = [];
  public virtual int Count => Children.Count;
  public bool HasType => Type is not null;
  internal string ContentNoNewLine => Content.
    Replace("\n", "<LF>", SCO).
    Replace("\r", "<CR>", SCO);
  private static string GetProps (TokenObject to) =>
  to is null || to.Properties.Count == 0 ? SE : $"{{{to.Properties.Select(i => i.ToString()).TextJoin(", ")}}}";
  private static string GetParams (TokenStatement ts) =>
  ts is null || ts.Parameters.Count == 0 ? SE : $"{{{ts.Parameters.Select(i => i.ToString()).TextJoin(", ")}}}";
  public override string ToString ()
  {

    string content = this switch
    {
      TokenFlag f => $"{(f.AddFlag ? "+" : "-")} {f.Name}",
      TokenLabel l => $"{l.Name}",
      TokenTypedValue t => $"{t.ObjType} {t.Value}",
      TokenObject o => $"{o.Name} {(o.ObjType.IsNotEmpty() ? "as " + o.ObjType : "")}{GetProps(o)}",
      TokenProperty p => $"{p.Name} {(p.ObjType.IsNotEmpty() ? "as " + p.ObjType + " " : "")}: {p.Value}",
      TokenStatement s => $"{s.Name} {(s.ObjType.IsNotEmpty() ? "as " + s.ObjType + " " : "")}{GetParams(s)}",
      TokenExpression s => $"{s.LeftValue} {s.Type} {s.RightValue}",
      Token t => $"{t.Content}",
      _ => $"`{ContentNoNewLine}`"
    };

    return $"{Index} : {GetType().Name} is {Type} = {content}";
  }
  public override bool Equals (object? obj) => obj is IToken rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
  public int CompareTo (IToken? other) => Index.CompareTo(other?.Index);
  public virtual bool Equals (IToken? other) => GetHashCode() == other?.GetHashCode();
  public static bool operator == (TokenBase left, IToken right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenBase left, IToken right) => !(left == right);
  public static bool operator < (TokenBase left, IToken right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenBase left, IToken right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenBase left, IToken right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenBase left, IToken right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
