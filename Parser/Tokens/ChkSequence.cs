#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public class ChkSequence<T> : IList<ChkToken<T>> where T : notnull
{
  private readonly List<ChkToken<T>> _sequence = [];
  public string? DataString { get; set; }

  public ChkToken<T> this[int index] { get => _sequence[index]; set => _sequence[index] = value; }
  public ChkSequence<T> this[Range rng] { get => [.. _sequence[rng]]; }
  public int Count => _sequence.Count;
  public bool IsReadOnly => _sequence.Count > 0;
  public bool AllOptional => _sequence.All(item => item.TokenRule.HasFlag(RT.Opt));
  public void Add (ChkToken<T> item) => _sequence.Add(item);
  public void Clear () => _sequence.Clear();
  public bool Contains (ChkToken<T> item) => _sequence.Contains(item);
  public void CopyTo (ChkToken<T>[] array, int arrayIndex) => _sequence.CopyTo(array, arrayIndex);
  public IEnumerator<ChkToken<T>> GetEnumerator () => _sequence.GetEnumerator();
  public int IndexOf (ChkToken<T> item) => _sequence.IndexOf(item);
  public void Insert (int index, ChkToken<T> item) => _sequence.Insert(index, item);
  public bool Remove (ChkToken<T> item) => _sequence.Remove(item);
  public void RemoveAt (int index) => _sequence.RemoveAt(index);
  IEnumerator IEnumerable.GetEnumerator () => _sequence.GetEnumerator();

  public ChkSequence (IEnumerable<ChkToken<T>> tokens) => _sequence = [.. tokens];
  public ChkSequence () { }
}
