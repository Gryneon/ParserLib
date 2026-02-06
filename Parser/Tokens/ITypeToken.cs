#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface ITypeToken
{
  string? ObjType { get; }
  IToken? TypeToken { get; set; }
}
