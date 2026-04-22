using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Parser.Condition;

public class AndCondition (params Collection<ICondition> conditions) : BasicCondition(), ICanAddChildren<ICondition>
{
  public override bool NoInputData => true;
  public Collection<ICondition> Conditions { get; } = conditions;
  public int Count => Conditions.Count;

  public void Add (ICondition child) => Conditions.Add(child);
  public void AddRange (IEnumerable<ICondition> children)
  {
    children.ThrowIfNull();
    foreach (ICondition child in children)
    {
      Add(child);
    }
  }
  protected override void Execute () => Result = Conditions.All(item => item.Evaluate(Parser));
}

public enum KeyOption
{
  LoadKey,
  CountOfKey,
  CheckKeyExists,
  TypeOfKey
}

public abstract class BasicCondition : ICondition
{
  #region Public Properties
  public virtual bool NoInputData { get; }
  public virtual bool DoNotEvaluate { get; }
  #endregion
  [AllowNull]
  protected XParser Parser { get; set; }
  [AllowNull]
  protected DataStore Data { get; set; }
  protected bool Result { get; set; }
  protected string DebugMsg { get; set; } = SE;
  protected string InputKeyL { get; set; }
  protected KeyOption InputKeyLOpt { get; set; }
  protected string? InputKeyR { get; set; }
  protected KeyOption InputKeyROpt { get; set; }
  protected object? PassedLiteral { get; set; }
  [AllowNull]
  protected object LeftValue { get; set; }
  [AllowNull]
  protected object RightValue { get; set; }
  protected BasicCondition ()
  {
    InputKeyL = SE;
    InputKeyR = SE;
  }
  protected BasicCondition (string left_key, KeyOption left_key_options, string right_key, KeyOption right_key_options)
  {
    InputKeyL = left_key;
    InputKeyLOpt = left_key_options;
    InputKeyR = right_key;
    InputKeyROpt = right_key_options;
  }
  protected BasicCondition (string only_key, KeyOption only_key_options, object literal)
  {
    InputKeyL = only_key;
    InputKeyLOpt = only_key_options;
    PassedLiteral = literal;
  }
  private object GetValue (string key, KeyOption type)
  {
    return type switch
    {
      KeyOption.LoadKey => Data[key],
      KeyOption.CountOfKey => Data.CanLoad(key) ? Data[key] is IEnumerable ien ? ien.Count() : 1 : 0,
      KeyOption.CheckKeyExists => Data.CanLoad(key),
      KeyOption.TypeOfKey => Data[key].GetType(),
      _ => Op.ThrowBadDef($"Invalid key option {type}")
    };
  }
  [MemberNotNull(nameof(Parser), nameof(Data))]
  protected void Initialize (XParser parser)
  {
    Parser = parser;
    Data = Parser.Data;

    if (!NoInputData && InputKeyL.IsNotEmpty())
    {
      LeftValue = GetValue(InputKeyL, InputKeyLOpt);
    }

    if (!NoInputData && InputKeyR.IsNotEmpty())
      RightValue = GetValue(InputKeyR, InputKeyROpt);
    else if (PassedLiteral is not null)
      RightValue = PassedLiteral;
    else
      RightValue = null;

    if (LeftValue is null || RightValue is null)
    {
      _ = Op.ThrowBadDef($"Invalid data loaded R: {RightValue}, L: {LeftValue}");
    }
  }
  public bool Evaluate (XParser parser)
  {
    Initialize(parser);

    if (!DoNotEvaluate)
    {
      Execute();
    }

    if (DebugMsg.IsNotEmpty())
    {
      Log(MsgClass.Debug, DebugMsg);
    }

    return Result;
  }

  /// <summary>THis method must set <c><see cref="Result"/></c> with the results of the condition,
  /// or it needs to throw an <see cref="OperationException"/> to be caught by the parser.<br/>
  /// This may optionally set <c><see cref="DebugMsg"/></c> as well.</summary>
  protected abstract void Execute ();
}
