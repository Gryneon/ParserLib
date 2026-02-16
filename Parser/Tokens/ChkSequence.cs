#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class ChkSequence : IList<ChkToken>
{
  private readonly List<ChkToken> _sequence = [];
  public string? DataString { get; set; }

  public ChkToken this[int index] { get => _sequence[index]; set => _sequence[index] = value; }
  public ChkSequence this[Range rng] => [.. _sequence[rng]];
  public int Count => _sequence.Count;
  public bool IsReadOnly => _sequence.Count > 0;
  public bool AllOptional => _sequence.All(item => item.TokenRule.HasFlag(RT.Opt));
  public void Add (ChkToken item) => _sequence.Add(item);
  public void Clear () => _sequence.Clear();
  public bool Contains (ChkToken item) => _sequence.Contains(item);
  public void CopyTo (ChkToken[] array, int arrayIndex) => _sequence.CopyTo(array, arrayIndex);
  public IEnumerator<ChkToken> GetEnumerator () => _sequence.GetEnumerator();
  public int IndexOf (ChkToken item) => _sequence.IndexOf(item);
  public void Insert (int index, ChkToken item) => _sequence.Insert(index, item);
  public bool Remove (ChkToken item) => _sequence.Remove(item);
  public void RemoveAt (int index) => _sequence.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  public ChkSequence (IEnumerable<ChkToken> tokens) => _sequence = [.. tokens];
  public ChkSequence () { }
}
