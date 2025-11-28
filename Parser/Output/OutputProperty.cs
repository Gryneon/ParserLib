using OC = Parser.Output.OutputConditionOperator;

namespace Parser.Output;

public class OutputProperty
{
  public required Collection<IOutputCondition> TypeConditions { get; init; } = [];
  public required Type OutType { get; init; }
  public required Collection<OutputPropertyNode> Nodes { get; init; } = [];

  public bool ConditionMet (MatchDataSet mdd) =>
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

  public bool ConditionMet (MatchDataSet mdd) =>
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
  bool ConditionMet (MatchDataSet mdd);

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

  public virtual bool ConditionMet (MatchDataSet mdd) => Children.Any(item => item.ConditionMet(mdd));
}
public class AndOutputCondition (IEnumerable<IOutputCondition> conditions) : OrOutputCondition(conditions)
{
  public override bool ConditionMet (MatchDataSet mdd) => Children.All(item => item.ConditionMet(mdd));
}
public readonly struct GroupPresenceOutputCondition (string groupName) : IOutputCondition, IEquatable<GroupPresenceOutputCondition>
{
  private bool Invert { get; init; } = false;
  private string GroupName { get; init; } = groupName;

  internal static GroupPresenceOutputCondition Inverse (GroupPresenceOutputCondition condition) => new()
  {
    Invert = !condition.Invert,
    GroupName = condition.GroupName,
  };
  internal GroupPresenceOutputCondition Inverse () => Inverse(this);

  public bool ConditionMet (MatchDataSet mdd) => mdd is not null && mdd.HasGroup(GroupName) != Invert;

  public override bool Equals (object? obj) => obj is GroupPresenceOutputCondition gpoc && Equals(gpoc);

  public override int GetHashCode () => HashCode.Combine(GroupName, Invert);

  public static bool operator == (GroupPresenceOutputCondition left, GroupPresenceOutputCondition right) => left.Equals(right);

  public static bool operator != (GroupPresenceOutputCondition left, GroupPresenceOutputCondition right) => !(left == right);

  public bool Equals (GroupPresenceOutputCondition other) => GroupName == other.GroupName && Invert == other.Invert;
}
public readonly struct CaptureCountOutputCondition (string groupName, OC op, int count) : IOutputCondition, IEquatable<CaptureCountOutputCondition>
{
  private string GroupName { get; init; } = groupName;
  private OC Operator { get; init; } = op;
  private int Count { get; init; } = count;

  private static Dictionary<OC, OC> IVLookup => new() {
    (OC.Equals, OC.DoesNotEqual),
    (OC.DoesNotEqual, OC.Equals),
    (OC.MoreThan, OC.LessThanOrEqual),
    (OC.LessThanOrEqual, OC.MoreThan),
    (OC.LessThan, OC.MoreThanOrEqual),
    (OC.MoreThanOrEqual, OC.LessThan)
  };

  internal static CaptureCountOutputCondition Inverse (CaptureCountOutputCondition condition) => new()
  {
    Operator = IVLookup[condition.Operator],
    Count = condition.Count,
    GroupName = condition.GroupName,
  };
  internal CaptureCountOutputCondition Inverse () => Inverse(this);
  public bool ConditionMet (MatchDataSet mdd) => mdd is not null && Operator switch
  {
    OC.Equals => mdd[GroupName].Count == Count,
    OC.DoesNotEqual => mdd[GroupName].Count != Count,
    OC.MoreThanOrEqual => mdd[GroupName].Count >= Count,
    OC.LessThan => mdd[GroupName].Count < Count,
    OC.LessThanOrEqual => mdd[GroupName].Count <= Count,
    OC.MoreThan => mdd[GroupName].Count > Count,
    _ => false
  };

  public override bool Equals (object? obj) => obj is CaptureCountOutputCondition ccoc && GroupName.Like(ccoc.GroupName) && Operator == ccoc.Operator && Count == ccoc.Count;

  public override int GetHashCode () => HashCode.Combine(GroupName, Operator, Count);

  public static bool operator == (CaptureCountOutputCondition left, CaptureCountOutputCondition right) => left.Equals(right);

  public static bool operator != (CaptureCountOutputCondition left, CaptureCountOutputCondition right) => !(left == right);

  public bool Equals (CaptureCountOutputCondition other) => GroupName.Like(other.GroupName) && Operator == other.Operator && Count == other.Count;
}
