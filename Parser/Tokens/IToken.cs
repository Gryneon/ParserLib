#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IToken : IIndexSortable, IComparable<IToken>, IEquatable<IToken>
{
  /// <summary>Assigned at match, cleared if match fails, kept if passes.</summary>
  TokenRef? AssignTo { get; set; }
  string Type { get; set; }
  bool HasType { get; }
  bool Exempt { get; set; }
  IList<IToken> Children { get; set; }
  string Content => Children.Select(static t => t.Content).TextJoin(" ");
  string ContentNoNewLine => Content.
    Replace("\n", "<LF>", SCO).
    Replace("\r", "<CR>", SCO);

  string ToStringSm (string indent) => this switch
  {
    Token t => $"{t.Content}",
    ComplexToken ct => $"{ct.ToString(indent)}",
    _ => $"`{ContentNoNewLine}`"
  };
  string ToString (string indent) => $"{Type} : {Content}";
  void Print (int indent_for_children);
}
