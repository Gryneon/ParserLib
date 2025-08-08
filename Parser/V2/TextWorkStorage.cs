#pragma warning disable IDE0072 // Add missing cases

using Common.Extensions;

using OP = Parser.OpStatus;

namespace Parser.V2;

//HACK: Redo this
public class TextWorkStorage : IWorkStorage<string>
{
  public bool HasData => Initial.Count > 0;
  public Collection<string> Initial { get; protected set; } = [];
  public Collection<object> Current { get; protected set; } = [];
  public Collection<OP> Status { get; protected set; } = [];
  public Collection<Collection<object>> History { get; } = [];
  public int Count => Current.Count;

  public TextWorkStorage ()
  {
    Initial = [];
    Current = [];
    History = [];
    Status = [];
    StoredData = [];
  }
  public TextWorkStorage (string input) => Add(input);
  public TextWorkStorage (IEnumerable<string> input)
  {
    foreach (string item in input)
      _ = Add(item);
  }

  public void SetInitial (int index, string input) => Initial[index] = input;
  public void SetCurrent (int index, dynamic value) => Current[index] = value;
  public void SetStatus (int index, OP status) => Status[index] = status;
  public void Update (int index, dynamic value)
  {
    object writeMe = GetHistorySafeObject(value);
    History[index].Add(writeMe);
    SetCurrent(index, value);
  }
  public void Update (dynamic value)
  {
    object writeMe = GetHistorySafeObject(value);
    History[Count - 1].Add(writeMe);
    SetCurrent(Count - 1, value);
  }

  public static object GetHistorySafeObject (dynamic obj) => obj switch
  {
    string str => str,
    ICloneable ic => ic.Clone(),
    IEnumerable<dynamic> ie => ie.ToCollection(),
    ValueType vt => vt,
    object obj2 => obj2,
  };
  public int Add (string item)
  {
    int index = Count;

    Current.Add(item);
    Initial.Add(item);
    History.Add([item]);
    Status.Add(OP.AtStart);
    StoredData.Add([]);

    return index;
  }
  public Collection<Dictionary<string, dynamic>> StoredData { get; } = [];
  public void StoreData (int index, string key, dynamic value) => StoredData[index][key] = value;
  public dynamic GetData (int index, string key) => StoredData[index][key];

  public void Clear ()
  {
    History.Clear();
    Current.Clear();
    Initial.Clear();
    Status.Clear();
  }
  public IEnumerator<Collection<object>> GetEnumerator () => History.GetEnumerator();
  #region Unused interface methods
  bool ICollection<ICollection<object>>.IsReadOnly => false;
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
  void ICollection<ICollection<object>>.Add (ICollection<object> item) { }
  bool ICollection<ICollection<object>>.Contains (ICollection<object> item) => false;
  void ICollection<ICollection<object>>.CopyTo (ICollection<object>[] array, int arrayIndex) { }
  bool ICollection<ICollection<object>>.Remove (ICollection<object> item) => false;
  IEnumerator<ICollection<object>> IEnumerable<ICollection<object>>.GetEnumerator () => GetEnumerator();
  #endregion
}

public ref struct DataInstance
{
  public int Index;
  public string Initial;
  public Dictionary<string, dynamic> StoredData;
  public dynamic Current;
  public Collection<object> History;
}