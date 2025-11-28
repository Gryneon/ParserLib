namespace Parser;

/// <summary>Represents a marker used to denote the start and end of a depth or region, along with associated marker type and
/// inclusion options.</summary>
/// <remarks>A DepthMarker defines a pair of string markers and related options that can be used to annotate or
/// delimit regions, such as in parsing, formatting, or code generation scenarios. The struct is immutable and supports
/// value equality.</remarks>
public readonly struct DepthMarker : IEquatable<DepthMarker>
{
  public string Open { get; init; }
  public string Close { get; init; }
  public string Type { get; init; }
  public bool DescendBeforeToken { get; init; }
  public bool AscendAfterToken { get; init; }

  public override bool Equals (object? obj)
  {
    return GetHashCode() == obj?.GetHashCode();
  }

  public override int GetHashCode () => HashCode.Combine(Type, Open, Close, AscendAfterToken, DescendBeforeToken);

  public static bool operator == (DepthMarker left, DepthMarker right) => left.Equals(right);

  public static bool operator != (DepthMarker left, DepthMarker right) => !(left == right);

  public bool Equals (DepthMarker other) => GetHashCode() == other.GetHashCode();
}
