#pragma warning disable IDE0060 // Remove unused parameter

using Parser.Binary.Ops;
using Parser.Ops;

using static Parser.OpStatus;

using BDD = Parser.Binary.ByteDataDictionary;

namespace Parser.Binary;

/// <summary>
/// A parser that reads binary files.
/// </summary>
/// <remarks>Creates a new parser object.</remarks>
/// <param name="bytes">The raw file data.</param>
/// <param name="spec">The spec to use.</param>
public sealed class ByteParser (Spec? spec = null, byte[]? bytes = null) : IParser
{
  public static Spec DefaultSpec => new()
  {
    FileInferences = [],
    Name = "default",
    Operations = [
      new ByteReadOperation("result", -1, ByteReadMode.Binary),
      Operation.End
    ]
  };

  #region Internal Buffer Functions
  internal bool ContainsKey (string key) => Work.ContainsKey(key);
  internal T? Load<T> (string key) where T : notnull
  {
    object? temp = Work.TryGetValue(key, out object? value) ? value : null;

    if (temp is null)
    {
      Log("ByteParser.Load", "Loaded value is null.");
      return (T?) temp;
    }
    return (T) temp;
  }
  internal void Save<Span> (string key, Span<byte> data)
  {
    if (Work.TryGetValue(key, out object? atKey))
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
        Work[key] = new Collection<byte[]>() { array, data.ToArray() };
      }
    }
    else
    {
      Work[key] = data.Length > 0 ? data.ToArray() : [];
    }
  }
  internal void Save<T> (string key, T? data)
  {
    if (Work.TryGetValue(key, out object? atKey))
    {
      if (atKey is Collection<T> list && data is not null)
      {
        list.Add(data);
      }
      else if (atKey is T item && data is not null)
      {
        Collection<T> newlist = [item, data];
        Work[key] = newlist;
      }
      else if (data is null && Work.Remove(key))
      {
        Log($"BT.Save<T>({key}, null): Sent null to Data and cleared key '{key}'.");
      }
      else if (data is null)
      {
        Log($"BT.Save<T>({key}, null): Sent null to Data but the key '{key}' could not be removed.");
      }
      else
      {
        Collection<object> newlist = [atKey, data];
        Work[key] = newlist;
      }
    }
    else if (data is not null)
    {
      Work[key] = data;
    }
    else
    {
      Log($"BT.Save<T>({key}, null): Sent null to Data and cleared key '{key}'.");
    }
  }
  internal void Clear (string key) => Work.Remove(key);
  internal Span<byte> ReadNext (int count)
  {
    Span<byte> result = _fileContents.AsSpan().Slice(BytePos, count);
    BytePos += count;
    return result;
  }
  internal long ReadLong () => ReadNext(8).ToInt64();
  internal int ReadInt () => ReadNext(4).ToInt32();
  internal short ReadShort () => ReadNext(2).ToInt16();
  internal byte ReadByte () => ReadNext(1)[0];
  internal sbyte ReadSByte () => (sbyte) ReadNext(1)[0];
  internal ushort ReadUShort () => (ushort) ReadNext(2).ToInt16();
  internal uint ReadUInt () => (uint) ReadNext(4).ToInt32();
  internal ulong ReadULong () => (ulong) ReadNext(8).ToInt64();
  internal string ReadString (int length) => ReadNext(length).ByteArrToString();
  internal int GetLength () => _fileContents.Length;
  internal void SetPos (int pos) => BytePos = pos;
  #endregion
  /// <summary>
  /// The total operation count.
  /// </summary>
  public int OpCount => Operations.Count;
  /// <summary>
  /// The current operation.
  /// </summary>
  public ByteOperation CurrentOp =>
    (ByteOperation) (OpIndex < OpCount ? Operations[OpIndex] : Operation.End);
  public DictionaryMode Mode { get; set; } = DictionaryMode.Overwrite;
  #region Result Storage
  // Result Storage
  [MemberNotNullWhen(true, nameof(Result))]
  public bool HasResult => Result is not null;
  public object? Result { get; private set; }
  #endregion
  public int Cursor { get; set; }
  internal List<OpLoopData> LoopData { get; } = [];
  internal int LoopDepth => LoopData.Count - 1;
  internal int LoopCountRemaining { get; set; } = DNE;
  internal OpLoopData? CurrentLoop => LoopDepth >= LoopData.Count ? null : LoopData[LoopDepth];
  public Collection<IOperation> Operations => CurrentLoop?.Operations ?? [];
  public Dictionary<string, object> Metadata => Work.ToDictionary();
  public int CountOfKey (string key) => Work.TryGetValue(key, out object? value) ? value.AsCollection().Count : -1;
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
    internal set => CurrentLoop!.OpIndex = value;
  }

  /// <summary>
  /// The loaded binary file.
  /// </summary>
  private byte[] _fileContents = bytes ?? [];
  /// <summary>
  /// The current position in the binary file.
  /// </summary>
  public int BytePos { get; set; }
  /// <summary>
  /// The size of the binary file.
  /// </summary>
  public int ByteRemain => _fileContents.Length - BytePos - 1;
  /// <summary>
  /// The size of the binary file.
  /// </summary>
  public int ByteSize => _fileContents.Length;
  /// <summary>
  /// The loaded specification.
  /// </summary>
  public Spec Spec { get; init; } = spec ?? DefaultSpec;
  public int NextOpIndex { get; set; }
  public BDD Work { get; } = [];
  IDictionary<string, object> IParser.Work => Work;
  public string? CursorKey { get; set; }

  /// <summary>
  /// Parses the provided binary data.
  /// </summary>
  /// <param name="bytes">The binary data.</param>
  /// <returns><see cref="Pass"/> if successful, or an error code.</returns>
  public OpStatus Parse (byte[] bytes)
  {
    _fileContents = bytes;
    return Parse();
  }
  /// <summary>
  /// Parses the provided binary data.
  /// </summary>
  /// <returns><see cref="Pass"/> if successful, or an error code.</returns>
  public OpStatus Parse ()
  {
    LoopData.Add(new(Spec.Operations, false));
    OpStatus result = DoByteOperations();
    return result;
  }
  internal OpStatus DoByteOperations ()
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

  public void AddOperationSequence (IEnumerable<IOperation> ops) => throw new NotImplementedException();
}
