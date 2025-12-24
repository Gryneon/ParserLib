#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public sealed class TokenFlag<T> : IToken<T> where T : notnull
{
  private bool _activate = true;

  public bool AddFlag
  {
    get => _activate;
    init => _activate = value;
  }
  public bool RemFlag
  {
    get => !_activate;
    init => _activate = !value;
  }
  public string Name => NameToken.Content;
  public required IToken<T> NameToken { get; init; }
  public T? Type { get; init; }
  public bool HasType => Type is not null;
  public bool Exempt { get; set; }
  public TokenCollection<T> Children { get; init; } = [];
  public int Index { get; init; }

  public int CompareTo (IToken<T>? other) => other is TokenFlag<T> f ? Name.CompareTo(f.Name, SCO) : -1;
  public override bool Equals (object? obj) => obj is TokenFlag<T> flag && Name.Equals(flag.Name, SCO) && AddFlag == flag.AddFlag;

  public override int GetHashCode () => HashCode.Combine(Name, AddFlag);

  public static bool operator == (TokenFlag<T> left, TokenFlag<T> right) => left is null ? right is null : left.Equals(right);
  public static bool operator != (TokenFlag<T> left, TokenFlag<T> right) => !(left == right);
  public static bool operator < (TokenFlag<T> left, TokenFlag<T> right) => left is null ? right is not null : left.CompareTo(right) < 0;
  public static bool operator <= (TokenFlag<T> left, TokenFlag<T> right) => left is null || left.CompareTo(right) <= 0;
  public static bool operator > (TokenFlag<T> left, TokenFlag<T> right) => left is not null && left.CompareTo(right) > 0;
  public static bool operator >= (TokenFlag<T> left, TokenFlag<T> right) => left is null ? right is null : left.CompareTo(right) >= 0;
}
