#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Specification.JSON;

/// <summary>
/// Basic interface for JSON parts.
/// </summary>
public interface IJSONNode
{
  /// <summary>
  /// The value stored in this node.
  /// </summary>
  object? Value { get; }
  /// <summary>
  /// Gets the JSON of this node.
  /// </summary>
  /// <returns>The JSON as a string.</returns>
  string? ToString () => Value switch
  {
    null => "null",
    string str => $"\"{str}\"",
    bool b => b ? "true" : "false",
    IEnumerable<JSONProperty> props => $"[{props.TextJoin(",")}]",
    IEnumerable<JSONValue> vals => $"{{{vals.TextJoin(",")}}}",
    IConvertible iConv => iConv.ToString(CIIC),
    IEnumerable<IConvertible> iConvList => iConvList.TextJoin(","),
    _ => throw new InvalidCastException("Unknown object type.")
  };
}

public class JSONObject : IJSONNode, IDictionary<string, JSONValue>
{
  public object? Value => Properties;

  public Dictionary<string, JSONValue> Properties { get; } = [];

  public ICollection<string> Keys => Properties.Keys;

  public ICollection<JSONValue> Values => Properties.Values;

  public int Count => Properties.Count;

  public bool IsReadOnly => ((ICollection<KeyValuePair<string, JSONValue>>) Properties).IsReadOnly;

  public JSONValue this[string key] { get => ((IDictionary<string, JSONValue>) Properties)[key]; set => ((IDictionary<string, JSONValue>) Properties)[key] = value; }

  public void Add (string key, JSONValue value) => Properties.Add(key, value);
  public bool ContainsKey (string key) => Properties.ContainsKey(key);
  public bool Remove (string key) => Properties.Remove(key);
  public bool TryGetValue (string key, [MaybeNullWhen(false)] out JSONValue value) => Properties.TryGetValue(key, out value);
  public void Add (KeyValuePair<string, JSONValue> item) => Properties.Add(item);
  public void Clear () => Properties.Clear();
  public bool Contains (KeyValuePair<string, JSONValue> item) => ((ICollection<KeyValuePair<string, JSONValue>>) Properties).Contains(item);
  public void CopyTo (KeyValuePair<string, JSONValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, JSONValue>>) Properties).CopyTo(array, arrayIndex);
  public bool Remove (KeyValuePair<string, JSONValue> item) => ((ICollection<KeyValuePair<string, JSONValue>>) Properties).Remove(item);
  public IEnumerator<KeyValuePair<string, JSONValue>> GetEnumerator () => Properties.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => Properties.GetEnumerator();
}
