namespace Parser.Output;

/// <summary>
/// Stores an output value for generating objects.
/// This one stores the content in a group and stores it in a field.
/// </summary>
/// <param name="varName">The field name.</param>
/// <param name="groupName">The group name in the <see cref="MatchDataSet"/>.</param>
public struct OutputValueContent (string varName, string groupName) : IOutputValue, IEquatable<OutputValueContent>, IEquatable<IOutputValue>
{
  public string VarName { get; set; } = varName;
  public string GroupName { get; set; } = groupName;

  public override readonly bool Equals (object? obj) => obj is OutputValueContent ovc && VarName.Is(ovc.VarName) && GroupName.Like(ovc.GroupName);

  public override readonly int GetHashCode () => HashCode.Combine(VarName, GroupName);

  public static bool operator == (OutputValueContent left, OutputValueContent right) => left.Equals(right);

  public static bool operator != (OutputValueContent left, OutputValueContent right) => !(left == right);

  public readonly bool Equals (OutputValueContent other) => Equals((IOutputValue) other);

  public readonly bool Equals (IOutputValue? other) => VarName.Is(other?.VarName) && GroupName.Like(other?.GroupName);
}
