using OC = Parser.Text.Output.OutputConditionOperator;

namespace Parser.Text.Output;

public class OutputProperty
{
  public required List<IOutputCondition> TypeConditions { get; init; } = [];
  public required Type OutType { get; init; }
  public required List<OutputPropertyNode> Nodes { get; init; } = [];

  public bool ConditionMet (MatchDataDictionary mdd) =>
    new OrOutputCondition(TypeConditions).ConditionMet(mdd);

  [SetsRequiredMembers]
  public OutputProperty (Type type, IEnumerable<IOutputCondition> typeConditions, IEnumerable<OutputPropertyNode>? nodes = null)
  {
    TypeConditions.AddRange(typeConditions);
    OutType = type;
    Nodes.AddRange(nodes ?? []);
  }
  [SetsRequiredMembers]
  public OutputProperty (Type type, IOutputCondition condition, IEnumerable<OutputPropertyNode>? nodes = null)
  {
    TypeConditions.Add(condition);
    OutType = type;
    Nodes.AddRange(nodes ?? []);
  }
}
public class OutputPropertyNode
{
  public Collection<IOutputCondition> Conditions { get; init; } = [];
  public Collection<IOutputValue> Values { get; init; } = [];

  public bool ConditionMet (MatchDataDictionary mdd) =>
    new AndOutputCondition(Conditions).ConditionMet(mdd);

  public OutputPropertyNode (string groupName, IOutputValue value)
  {
    Conditions = [new GroupPresenceOutputCondition(groupName)];
    Values = [value];
  }
  public OutputPropertyNode (string groupName, IEnumerable<IOutputValue> values)
  {
    Conditions = [new GroupPresenceOutputCondition(groupName)];
    Values = [.. values];
  }
  public OutputPropertyNode (string varname, string groupname)
  {
    Conditions = [new GroupPresenceOutputCondition(groupname)];
    Values = [new OutputValueContent(varname, groupname)];
  }
  public OutputPropertyNode (string groupName, string keyName, string parseType)
  {
    Conditions = [new GroupPresenceOutputCondition(groupName)];
    Values = [new OutputValueParse(keyName, groupName, parseType)];
  }
  public OutputPropertyNode (IEnumerable<string> groupNames, IOutputValue value)
  {
    Conditions = [.. groupNames.Select(item => new GroupPresenceOutputCondition(item))];
    Values = [value];
  }
  public OutputPropertyNode (IEnumerable<string> groupNames, IEnumerable<IOutputValue> values)
  {
    Conditions = [.. groupNames.Select(item => new GroupPresenceOutputCondition(item))];
    Values = [.. values];
  }
}

public interface IOutputCondition
{
  bool ConditionMet (MatchDataDictionary mdd);

  static IOutputCondition operator ! (IOutputCondition condition) => condition switch
  {
    AndOutputCondition and => new OrOutputCondition(and.Children.Select(item => !item)),
    OrOutputCondition or => new AndOutputCondition(or.Children.Select(item => !item)),
    GroupPresenceOutputCondition gp => gp.Inverse(),
    CaptureCountOutputCondition cc => cc.Inverse(),
    _ => throw new InvalidOperationException("Invalid output condition."),
  };
}
public enum OutputConditionOperator
{
  Equals,
  MoreThan,
  LessThan,
  MoreThanOrEqual,
  LessThanOrEqual,
  DoesNotEqual
}
public class OrOutputCondition (IEnumerable<IOutputCondition> conditions) : IOutputCondition
{
  protected internal Collection<IOutputCondition> Children { get; } = [.. conditions];

  public virtual bool ConditionMet (MatchDataDictionary mdd) => Children.Any(item => item.ConditionMet(mdd));
}
public class AndOutputCondition (IEnumerable<IOutputCondition> conditions) : OrOutputCondition(conditions)
{
  public override bool ConditionMet (MatchDataDictionary mdd) => Children.All(item => item.ConditionMet(mdd));
}
public readonly struct GroupPresenceOutputCondition (string groupName) : IOutputCondition
{
  private bool Invert { get; init; } = false;
  private string GroupName { get; init; } = groupName;

  internal static GroupPresenceOutputCondition Inverse (GroupPresenceOutputCondition condition) => new()
  {
    Invert = !condition.Invert,
    GroupName = condition.GroupName,
  };
  internal GroupPresenceOutputCondition Inverse () => Inverse(this);

  public bool ConditionMet (MatchDataDictionary mdd) => mdd.HasGroup(GroupName) != Invert;
}
public readonly struct CaptureCountOutputCondition (string groupName, OC op, int count) : IOutputCondition
{
  private string GroupName { get; init; } = groupName;
  private OC Operator { get; init; } = op;
  private int Count { get; init; } = count;

  private static Dictionary<OC, OC> _iVLookup => new() {
    (OC.Equals, OC.DoesNotEqual),
    (OC.DoesNotEqual, OC.Equals),
    (OC.MoreThan, OC.LessThanOrEqual),
    (OC.LessThanOrEqual, OC.MoreThan),
    (OC.LessThan, OC.MoreThanOrEqual),
    (OC.MoreThanOrEqual, OC.LessThan)
  };

  internal static CaptureCountOutputCondition Inverse (CaptureCountOutputCondition condition) => new()
  {
    Operator = _iVLookup[condition.Operator],
    Count = condition.Count,
    GroupName = condition.GroupName,
  };
  internal CaptureCountOutputCondition Inverse () => Inverse(this);
  public bool ConditionMet (MatchDataDictionary mdd) => Operator switch
  {
    OC.Equals => mdd[GroupName].Count == Count,
    OC.DoesNotEqual => mdd[GroupName].Count != Count,
    OC.MoreThanOrEqual => mdd[GroupName].Count >= Count,
    OC.LessThan => mdd[GroupName].Count < Count,
    OC.LessThanOrEqual => mdd[GroupName].Count <= Count,
    OC.MoreThan => mdd[GroupName].Count > Count,
    _ => false
  };
}