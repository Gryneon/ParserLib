
namespace Parser.Tokens;

/// <summary>A token template, for assembling tokens.</summary>
public class TemplateSet : IReadOnlyCollection<CToken>
{
  /// <summary>The type of token to be assembled.</summary>
  public required string Type { get; init; }
  public required Collection<CToken> Tokens { get; init; }

  public int Count => Tokens.Count;

  public CToken this[int index] => Tokens[index];

  public IEnumerator<CToken> GetEnumerator () => Tokens.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
