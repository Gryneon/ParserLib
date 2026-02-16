#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public sealed class TokenLabel : TokenBase, INameToken
{
  public string? Name => NameToken?.Content;
  public required IToken? NameToken { get; set; }

  public static explicit operator TokenFlag (TokenLabel label)
  {
    label.ThrowIfNull();

    return new()
    {
      State = true,
      NameToken = label.NameToken,
      Children = label.Children,
      Ignored = label.Ignored,
      Exempt = label.Exempt,
      Content = label.Content,
      Index = label.Index,
      Type = label.Type
    };
  }

  public override bool Equals (object? obj) => obj is TokenLabel tv && (Name?.Equals(tv.Name, SCO) ?? false);
  public override int GetHashCode () => Name?.GetHashCode(SCO) ?? 0;
}
