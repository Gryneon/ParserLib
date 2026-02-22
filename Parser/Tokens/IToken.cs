#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>, IEquatable<IToken>
{
  string Type { get; set; }
  bool HasType { get; }
  bool Exempt { get; set; }
  bool Ignored { get; }
  IList<IToken> Children { get; init; }
  string Content => Children.Select(static t => t.Content).TextJoin();
  string ContentNoNewLine => Content.
    Replace("\n", "<LF>", SCO).
    Replace("\r", "<CR>", SCO);
  bool HasProperties => false;
  bool HasFlags => false;
  bool HasLeftRight => false;

  string ToStringSm () => this switch
  {
    TokenObject o => $"{o.Name} {(o.ObjType.IsNotEmpty() ? "as " + o.ObjType : "")}{GetProps(o)}",
    TokenExpression s => $"{s.LeftValue} {s.ObjType} {s.RightValue}",
    Token t => $"{t.Content}",
    ComplexToken ct => $"{ct}",
    _ => $"`{ContentNoNewLine}`"
  };
  private static string GetProps (TokenObject to) =>
    to is null || to.Properties.Count == 0 ? SE : $"{{{to.Properties.Select(i => i.ToStringSm()).TextJoin(", ")}}}";
}
