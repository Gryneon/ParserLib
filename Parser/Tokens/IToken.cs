#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>, IEquatable<IToken>
{
  string Type { get; set; }
  bool HasType { get; }
  bool Exempt { get; set; }
  bool Ignored { get; }
  IReadOnlyList<IToken> Children { get; init; }
  string Content => Children.Select(static t => t.Content).TextJoin();
  string ContentNoNewLine => Content.
    Replace("\n", "<LF>", SCO).
    Replace("\r", "<CR>", SCO);
  bool HasProperties => false;
  bool HasFlags => false;
  bool HasLeftRight => false;

  string ToStringSm () => this switch
  {
    TokenFlag f => $"{(f.State ? "+" : "-")} {f.Name}",
    TokenLabel l => $"{l.Name}",
    TokenTypedValue t => $"{t.ObjType} {t.Value}",
    TokenObject o => $"{o.Name} {(o.ObjType.IsNotEmpty() ? "as " + o.ObjType : "")}{GetProps(o)}",
    TokenProperty p => $"{p.Name} {(p.ObjType.IsNotEmpty() ? "as " + p.ObjType + " " : "")}: {p.Value}",
    TokenStatement s => $"{s.Name} {(s.ObjType.IsNotEmpty() ? "as " + s.ObjType + " " : "")}{GetParams(s)}",
    TokenExpression s => $"{s.LeftValue} {s.ObjType} {s.RightValue}",
    Token t => $"{t.Content}",
    _ => $"`{ContentNoNewLine}`"
  };
  private static string GetProps (TokenObject to) =>
    to is null || to.Properties.Count == 0 ? SE : $"{{{to.Properties.Select(i => i.ToStringSm()).TextJoin(", ")}}}";
  private static string GetParams (TokenStatement ts) =>
    ts is null || ts.Parameters.Count == 0 ? SE : $"{{{ts.Parameters.Select(i => i.ToStringSm()).TextJoin(", ")}}}";
}
