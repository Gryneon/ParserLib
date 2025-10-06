#pragma warning disable IDE0072 // Add missing cases

using Common.Extensions;

using OP = Parser.OpStatus;

namespace Parser.V2;

public delegate ResultPackage OperationCommand (dynamic input);

/// <summary>
/// An object that can be a collection of strings, a string, or anything similar to a string.
/// </summary>
public struct StringLikeObject : IComparable<string>, IEquatable<string>, IEnumerable<StringLikeObject>
{
  /// <summary>
  /// Empty object.
  /// </summary>
  public readonly bool IsEmpty => _value is null && Values is null;
  /// <summary>
  /// Contains 1 object.
  /// </summary>
  [MemberNotNullWhen(true, nameof(_value))]
  public readonly bool IsSingle => _value is not null;
  /// <summary>
  /// Contains more than 1 object.
  /// </summary>
  [MemberNotNullWhen(true, nameof(Values))]
  public readonly bool IsCollection => Values is not null;

  private object? _value;
  private IEnumerable<object>? Values { get; set; }

  public readonly IEnumerable<string> GetCollection () =>
    IsSingle ? [_value?.ToString() ?? SE]
  : IsCollection ? new Collection<string>([.. from item in Values select item.ToString()])
  : new Collection<string>([SE]);

  /// <summary>
  /// gets or sets any object into this property.
  /// </summary>
  public dynamic Value
  {
    readonly get => IsCollection ? Values : IsSingle ? _value : (dynamic) SE;
    set
    {
      if (value is null)
      {
        _value = null;
        Values = null;
        return;
      }
      if (value is string s)
      {
        _value = s;
        Values = null;
        return;
      }
      if (value is IEnumerable<string> list)
      {
        _value = null;
        Values = list;
        return;
      }
      if (value is IEnumerable<object> objlist)
      {
        _value = null;
        Values = [.. objlist];
        return;
      }
      if (value is object obj)
      {
        _value = obj;
        Values = null;
        return;
      }
    }
  }
  /// <inheritdoc/>
  public readonly bool Equals (string? other) => IsSingle && (_value?.ToString()?.Equals(other, SCO) ?? false);
  /// <inheritdoc/>
  public readonly int CompareTo (string? other) => IsSingle ? (_value?.ToString()?.CompareTo(other, SCO) ?? -1) : 1;
  public readonly IEnumerator<StringLikeObject> GetEnumerator () =>
    IsCollection ? (from o in Values select new StringLikeObject() { Value = o.ToString() ?? SE }).GetEnumerator() :
    IsSingle ? new StringLikeObject() { Value = _value.ToString() ?? SE }.GetEnumerator() :
    new StringLikeObject() { Value = SE }.GetEnumerator();
  readonly IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

  public static implicit operator Collection<string> (StringLikeObject from) => [.. from.GetCollection()];
  public static implicit operator string (StringLikeObject from) => from.IsSingle ? from._value.ToString() ?? SE : from.IsCollection ? from.Values.TextJoin() : SE;
  public static implicit operator StringLikeObject (string from) => new() { Value = from };
  public static implicit operator StringLikeObject (Collection<object> from) => new() { Value = from };
}

public abstract class TextOperation : IOperation<object, object>
{
  //Must define one of these:
  public abstract OperationCommand? Func { get; init; }
  public abstract ResultPackage DoOperationWork (dynamic input);

  public int FileIndex { get; init; }
  public bool ContinueOnFail { get; init; }
  public bool SkipOperation { get; init; }
  public bool EndOperation { get; init; }
  public bool StartOperation { get; init; }
}

public interface IOperation<TInput, TOutput>
{
  int FileIndex { get; init; }

  bool ContinueOnFail { get; init; }
  bool SkipOperation { get; init; }
  bool EndOperation { get; init; }
  bool StartOperation { get; init; }

  OperationCommand? Func { get; init; }

  void DoOperation (IWorkStorage<TInput> work)
  {
    try
    {
      if (work.Current is null)
        throw new ArgumentNullException(nameof(work));
      if (work.Current is not TInput input)
        throw new InvalidCastException();
      ResultPackage result = Func is null ? DoOperationWork(input) : Func.Invoke(input);
      work.SetStatus(FileIndex, result.Status);
      work.SetCurrent(FileIndex, result.Data);
    }
    catch (NullReferenceException)
    {
      work.SetStatus(FileIndex, OP.FailBadInputNull);
    }
    catch (InvalidCastException)
    {
      work.SetStatus(FileIndex, OP.FailBadInputType);
    }
    catch (Exception)
    {
      work.SetStatus(FileIndex, OP.FailBadOpResult);
    }
  }

  abstract ResultPackage DoOperationWork (dynamic input);
}
