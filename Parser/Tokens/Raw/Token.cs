#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class Token<T> : TokenBase<T> where T : notnull
{
  // Required Properties
  public bool Ignored { get; set; }
  public override string Content
  {
    get => _content;
    set => _content = value;
  }
  // Calculated Properties
  public int LastPosition => Index + Length - 1;
  public int Length => Content.Length;
  public override string ToString () => $"{Index} : {Type} = \"{ContentNoNewLine}\"";
  public override bool Equals (object? obj) => obj is IToken<T> rt && Equals(rt);
  public override int GetHashCode () => HashCode.Combine(Content, Index, Type);
}
