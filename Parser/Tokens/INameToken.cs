#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface INameToken
{
  string? Name { get; }
  IToken? NameToken { get; set; }
}
