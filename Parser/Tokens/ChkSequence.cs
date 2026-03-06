#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class ChkSequence : IList<ChkToken>
{
  private readonly List<ChkToken> _sequence = [];
  public string? DataString { get; set; }

  public ChkToken this[int index] { get => _sequence[index]; set => _sequence[index] = value; }
  public ChkSequence this[Range rng] => [.. _sequence[rng]];
  public int Count => _sequence.Count;

  public bool AllOptional => _sequence.All(item => item.TokenRule.HasFlag(RT.Opt));

  bool ICollection<ChkToken>.IsReadOnly { get; }

  public void Add (ChkToken item) => _sequence.Add(item);
  public void Clear () => _sequence.Clear();
  public IEnumerator<ChkToken> GetEnumerator () => _sequence.GetEnumerator();
  public int IndexOf (ChkToken item) => _sequence.IndexOf(item);
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  void IList<ChkToken>.Insert (int index, ChkToken item) => throw new NotImplementedException();
  void IList<ChkToken>.RemoveAt (int index) => throw new NotImplementedException();
  bool ICollection<ChkToken>.Contains (ChkToken item) => throw new NotImplementedException();
  void ICollection<ChkToken>.CopyTo (ChkToken[] array, int arrayIndex) => throw new NotImplementedException();
  bool ICollection<ChkToken>.Remove (ChkToken item) => throw new NotImplementedException();

  public ChkSequence (IEnumerable<ChkToken> tokens) => _sequence = [.. tokens];
  public ChkSequence () { }
}
