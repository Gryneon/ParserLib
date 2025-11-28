namespace Parser.Output;

public struct OutputValueStatic (string varName, string groupName, object value) : IOutputValue, IEquatable<IOutputValue>, IEquatable<OutputValueStatic>
{
  public string VarName { get; set; } = varName;
  public string GroupName { get; set; } = groupName;
  public object Value { get; set; } = value;
  public static OutputValueStatic operator ! (OutputValueStatic item) => new(item.VarName, item.GroupName, item.Value is bool b ? !b : item.Value);

  public override readonly bool Equals (object? obj) => obj is OutputValueStatic stat && VarName.Equals(stat.VarName, SCO) && GroupName.Equals(stat.GroupName, SCOIC);
  public override readonly int GetHashCode () => HashCode.Combine(VarName, GroupName);
  public static bool operator == (OutputValueStatic left, OutputValueStatic right) => left.Equals(right);
  public static bool operator != (OutputValueStatic left, OutputValueStatic right) => !(left == right);
  public readonly bool Equals (IOutputValue? other) => VarName.Equals(other?.VarName, SCO) && GroupName.Equals(other.GroupName, SCOIC);
  public readonly bool Equals (OutputValueStatic other) => VarName.Equals(other.VarName, SCO) && GroupName.Equals(other.GroupName, SCOIC);
}
