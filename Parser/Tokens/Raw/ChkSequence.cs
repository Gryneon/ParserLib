#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens.Raw;

public class ChkSequence<T> : IList<ChkToken<T>> where T : notnull
{
  private readonly List<ChkToken<T>> _sequence = [];
  private readonly List<string> _data_strings = [];

  public ChkToken<T> this[int index] { get => _sequence[index]; set => _sequence[index] = value; }
  public ChkSequence<T> this[Range rng] { get => [.. _sequence[rng]]; }
  public int Count => _sequence.Count;
  public bool IsReadOnly => _sequence.Count > 0;
  public bool AllOptional => _sequence.All(item => item.TokenRule.HasFlag(RT.Opt));

  public void Assign (string data)
  {
    // Only assign if uninitialized.
    if (IsReadOnly) return;

    _data_strings.AddRange([.. data?.Split([' ', '\t', '\n'], 255, SSORT) ?? []]);

    foreach (string item in _data_strings)
    {
      _sequence.Add(new(item));
    }
  }
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

  public ChkSequence (string data_string) => Assign(data_string);
  public ChkSequence (IEnumerable<ChkToken<T>> tokens) => _sequence = [.. tokens];
  public ChkSequence () { }
}
