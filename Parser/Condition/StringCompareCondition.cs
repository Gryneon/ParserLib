
using SCC = Parser.Condition.StringCompareConditionType;

namespace Parser.Condition;

/// <summary>The flags for a string compare condition.</summary>
[Flags]
public enum StringCompareConditionType
{
  /// <summary>Treat both sides as literals. (Not typically used)</summary>
  None = 0,
  /// <summary>Left value references a key.</summary>
  LeftIsKey = 1,
  /// <summary>Right value references a key.</summary>
  RightIsKey = 2,
  /// <summary>Not case sensitive.</summary>
  CaseInsensitive = 4,
  /// <summary>Trim whitespace from left.</summary>
  TrimLeft = 8,
  /// <summary>Trim whitespace from right.</summary>
  TrimRight = 16,
  /// <summary>Invert result.</summary>
  Not = 0x40000000,
}

public class StringCompareCondition (SCC type, string left, string right) : ICondition
{
  public SCC Type { get; } = type;
  public string Left { get; } = left;
  public string Right { get; } = right;
  public bool Evaluate (XParser parser)
  {
    parser.ThrowIfNull();

    string? left_value = Type.HasFlag(SCC.LeftIsKey) ? parser.Data[Left] as string : Left;
    string? right_value = Type.HasFlag(SCC.RightIsKey) ? parser.Data[Right] as string : Right;

    left_value = Type.HasFlag(SCC.TrimLeft) ? left_value?.Trim() : left_value;
    right_value = Type.HasFlag(SCC.TrimRight) ? right_value?.Trim() : right_value;

    bool? result = left_value?.Equals(right_value, Type.HasFlag(SCC.CaseInsensitive) ? SCOIC : SCO);

    return result is bool nResult && (Type.HasFlag(SCC.Not) ? !nResult : nResult);
  }
}
