#pragma warning disable IDE0060 // Remove unused parameter

using Parser.Byte.Ops;
using Parser.Ops;

using static Parser.OpStatus;

using BDD = Parser.Byte.ByteDataDictionary;

namespace Parser.Byte;

/// <summary>
/// A parser that reads binary files.
/// </summary>
/// <param name="bytes">The raw file data.</param>
/// <param name="spec">The spec to use.</param>
public sealed class ByteParser (byte[] bytes, Spec spec)
{
  #region Private Static Methods
  private static IOEx _noKeyErr (string key) => new($"BT.Data: Data doesn't have key '{key}'");
  #endregion
  #region Internal Buffer Functions
  internal bool ContainsKey (string key) => ByteObjects.ContainsKey(key);
  internal T? Load<T> (string key) where T : notnull
  {
    object? temp = ByteObjects.TryGetValue(key, out object? value) ? value : null;

    if (temp is null)
    {
      Log("ByteParser.Load", "Loaded value is null.");
      return (T?)temp;
    }
    return temp is null ? throw _noKeyErr(key) : (T)temp;
  }
  internal void Save<Span> (string key, Span<byte> data)
  {
    if (ByteObjects.TryGetValue(key, out object? atKey))
    {
      if (atKey is Collection<byte[]> binary)
      {
        Log($"ByteParser.Save<Span>({key}, Span<byte>)", $"Sent data and added to the collection.");
        binary.Add(data.ToArray());
      }
      else if (atKey is byte[] array)
      {
        // Overwrite
        Log($"ByteParser.Save<Span>({key}, Span<byte>)", $"Sent data and started the collection.");
        ByteObjects[key] = new Collection<byte[]>() { array, data.ToArray() };
      }
    }
    else
    {
      ByteObjects[key] = data.Length > 0 ? data.ToArray() : [];
    }
  }
  internal void Save<T> (string key, T? data)
  {
    if (ByteObjects.TryGetValue(key, out object? atKey))
    {
      if (atKey is Collection<T> list && data is not null)
      {
        list.Add(data);
      }
      else if (atKey is T item && data is not null)
      {
        Collection<T> newlist = [item, data];
        ByteObjects[key] = newlist;
      }
      else if (data is null && ByteObjects.Remove(key))
      {
        Log($"BT.Save<T>({key}, null): Sent null to Data and cleared key '{key}'.");
      }
      else
      {
        Collection<object> newlist = [atKey, data];
        ByteObjects[key] = newlist;
      }
    }
    else if (data is not null)
    {
      ByteObjects[key] = data;
    }
    else
    {
      Log($"BT.Save<T>({key}, null): Sent null to Data and cleared key '{key}'.");
    }
  }
  internal void Clear (string key) => ByteObjects.Remove(key);
  internal Span<byte> ReadNext (int count)
  {
    Span<byte> result = FileContents.AsSpan().Slice(BytePos, count);
    BytePos += count;
    return result;
  }
  internal long ReadLong () => ReadNext(8).ToInt64();
  internal int ReadInt () => ReadNext(4).ToInt32();
  internal short ReadShort () => ReadNext(2).ToInt16();
  internal byte ReadByte () => ReadNext(1)[0];
  internal sbyte ReadSByte () => (sbyte)ReadNext(1)[0];
  internal ushort ReadUShort () => (ushort)ReadNext(2).ToInt16();
  internal uint ReadUInt () => (uint)ReadNext(4).ToInt32();
  internal ulong ReadULong () => (ulong)ReadNext(8).ToInt64();
  internal string ReadString (int length) => ReadNext(length).ByteArrToString();
  internal int GetLength () => FileContents.Length;
  #endregion
  /// <summary>
  /// The total operation count.
  /// </summary>
  public int OpCount => Operations.Count;
  /// <summary>
  /// The current operation.
  /// </summary>
  public ByteOperation CurrentOp =>
    (ByteOperation)(OpIndex < OpCount ? Operations[OpIndex] : Operation.End);

  #region Result Storage
  // Result Storage
  [MemberNotNullWhen(true, nameof(Result))]
  public bool HasResult => Result is not null;
  public object? Result { get; protected set; }
  #endregion

  protected internal List<OpLoopData> LoopData { get; } = [];
  protected internal int LoopDepth => LoopData.Count - 1;
  protected internal int LoopCountRemaining { get; set; } = DNE;
  protected internal OpLoopData? CurrentLoop => LoopDepth >= LoopData.Count ? null : LoopData[LoopDepth];
  public Collection<IOperation> Operations => CurrentLoop?.Operations ?? [];
  public Dictionary<string, object> Metadata => ByteObjects.ToDictionary();
  /// <summary>
  /// The last status from the last operation executed.
  /// </summary>
  public OpStatus LastStatus
  {
    get => CurrentLoop?.Status ?? EndCommand;
    set => CurrentLoop?.Status = value;
  }
  /// <summary>
  /// The current operation index.
  /// </summary>
  public int OpIndex
  {
    get => CurrentLoop?.OpIndex ?? 0;
    protected internal set => CurrentLoop!.OpIndex = value;
  }

  /// <summary>
  /// The loaded binary file.
  /// </summary>
  protected byte[] FileContents = bytes;
  /// <summary>
  /// The data stored from the file.
  /// </summary>
  public BDD ByteObjects { get; } = [];

  /// <summary>
  /// The current position in the binary file.
  /// </summary>
  public int BytePos { get; set; }
  /// <summary>
  /// The loaded specification.
  /// </summary>
  public required Spec Spec { get; init; } = spec;

  /// <summary>
  /// Parses the provided binary data.
  /// </summary>
  /// <param name="bytes">The binary data.</param>
  /// <returns><see cref="OpStatus.Pass"/> if successful, or an error code.</returns>
  public OpStatus Parse (byte[] bytes)
  {
    FileContents = bytes;
    return Parse();
  }
  /// <summary>
  /// Parses the provided binary data.
  /// </summary>
  /// <returns><see cref="OpStatus.Pass"/> if successful, or an error code.</returns>
  public OpStatus Parse ()
  {
    LoopData.Add(new(Spec.Operations, false));
    OpStatus result = DoByteOperations();
    return result;
  }
  protected internal OpStatus DoByteOperations ()
  {
    LastStatus = AtStart;
    OpIndex = 0;

    while (LastStatus < Fail && OpIndex < OpCount)
    {
      LastStatus = CurrentOp.DoOperation(this);
      if (LastStatus.IsFail())
      {
        Log("ByteParser.DoByteOperations", $"Failure encountered at operation[{OpIndex}].");
        break;
      }
      OpIndex++;
    }

    return LastStatus;
  }
}
