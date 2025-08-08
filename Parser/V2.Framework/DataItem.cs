namespace Parser.V2.Framework;

public abstract class DataItem : IDataItem
{
  public abstract dynamic Value { get; }
}
public class DataItem<T> : DataItem
  where T : notnull
{
  public override dynamic Value => InternalValue1;

  [NotNull]
  protected virtual T? InternalValue1 { get; set; }

  public DataItem (T value) => InternalValue1 = value;

  protected DataItem () { }

  public static DataItem Generate (T from)
  {
    ArgumentNullException.ThrowIfNull(from);

    return from switch
    {
      string str => new DataItem<string>(str),
      _ => throw new ArgumentException($"Unknown object sent to DataItem. ({from.GetType()})"),
    };
  }
}
public class DataItem<T1, T2> : DataItem<T1>
  where T1 : notnull
  where T2 : notnull
{
  public override dynamic Value => AllNull ? throw new ArgumentNullException(nameof(Value)) :
    InternalValue1 is not null ? InternalValue1 :
    InternalValue2;
  [NotNull]
  protected override T1? InternalValue1 { get; set; }
  protected virtual T2? InternalValue2 { get; set; }
  [MemberNotNullWhen(false, nameof(InternalValue1), nameof(InternalValue2))]
  protected virtual bool AllNull => InternalValue1 is null && InternalValue2 is null;

  protected DataItem () : base() { }
  public DataItem (T1 value) : base(value) => InternalValue1 = value;
  public DataItem (T2 value) : base() => InternalValue2 = value;
}
public class DataItem<T1, T2, T3> : DataItem<T1, T2>
  where T1 : notnull
  where T2 : notnull
  where T3 : notnull
{
  public override dynamic Value => AllNull ? throw new ArgumentNullException(nameof(Value)) :
    InternalValue1 is not null ? InternalValue1 :
    InternalValue2 is not null ? InternalValue2 :
    InternalValue3;
  [NotNull]
  protected override T1? InternalValue1 { get; set; }
  [NotNull]
  protected override T2? InternalValue2 { get; set; }
  protected virtual T3? InternalValue3 { get; set; }
  [MemberNotNullWhen(false, nameof(InternalValue1), nameof(InternalValue2), nameof(InternalValue3))]
  protected override bool AllNull => InternalValue1 is null && InternalValue2 is null && InternalValue3 is null;

  public DataItem (T1 value) : base(value) => InternalValue1 = value;
  public DataItem (T2 value) : base(value) => InternalValue2 = value;
  public DataItem (T3 value) : base() => InternalValue3 = value;
}