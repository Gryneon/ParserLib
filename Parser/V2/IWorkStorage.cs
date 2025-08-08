#pragma warning disable IDE0072 // Add missing cases

using OP = Parser.OpStatus;

namespace Parser.V2;

public interface IWorkStorage<TInputData> : ICollection<ICollection<object>>
{
  bool HasData => Initial.Count > 0;
  Collection<TInputData> Initial { get; }
  Collection<dynamic> Current { get; }
  Collection<OP> Status { get; }
  Collection<Collection<object>> History { get; }
  Collection<Dictionary<string, dynamic>> StoredData { get; }
  void SetInitial (int index, TInputData input);
  void SetCurrent (int index, dynamic value);
  void SetStatus (int index, OP status);
  void Update (int index, dynamic value);
  int Add (TInputData input)
  {
    if (input is null)
      return -1;

    Initial.Add(input);
    Current.Add(input);
    Status.Add(OP.AtStart);
    History.Add([]);
    return Initial.Count - 1;
  }
  void StoreData (int index, string key, dynamic value);
  dynamic GetData (int index, string key);
}
