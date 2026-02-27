namespace Parser.Condition;

public enum OperationConditionType
{
  /// <summary>Bad condition type.</summary>
  Error = -1, //Error
  /// <summary>No condition is always a pass.</summary>
  None = 0, //Always pass
  /// <summary>Condition of 1 is always a fail.</summary>
  Fail = 1, //Always fail

  // Value 1
  LConstant = 2,
  LValueOfKey = 3,
  LNameOfKey = 4,
  LCountOfKey = 5,
  LTotalKeys = 6,

  // Operation
  Like = 256 * 4, //Case Insensitive Match
  Is = 256 * 5,
  LessThan = 256 * 6,
  GreaterThan = 256 * 7,
  LessThanOrEqualTo = 256 * 8,
  GreaterThanOrEqualTo = 256 * 9,
  IsAssignableTo = 256 * 10,
  IsAssignableFrom = 256 * 11,
  Contains = 256 * 12,
  StartsWith = 256 * 13,
  EndsWith = 256 * 14,
  Exists = 256 * 15,

  // Value 2
  RVal = 2 ^ 16,
  RConstant = 2 ^ 16 * LConstant,
  RValueOfKey = 2 ^ 16 * LValueOfKey,
  RNameOfKey = 2 ^ 16 * LNameOfKey,
  RCountOfKey = 2 ^ 16 * LCountOfKey,
  RTotalKeys = 2 ^ 16 * LTotalKeys,

  // Special Actions
  /// <summary>The condition is a logical AND of other conditions.</summary>
  And = 0x10000000,
  /// <summary>The condition is a logical OR of other conditions.</summary>
  Or = 0x20000000,
  /// <summary>The condition is a logical NOT of itself.</summary>
  Not = 0x40000000,
}
