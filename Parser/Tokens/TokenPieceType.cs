#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public enum TokenPieceType
{
  /// <summary>The token representing this object's name.</summary>
  Name,
  /// <summary>The token representing this object's type.</summary>
  Type,
  /// <summary>The token representing this object's value.</summary>
  Value,
  /// <summary>The tokens representing this object's paramenters.</summary>
  /// <remarks>This will be a TokenCollection</remarks>
  ParameterList,
  /// <summary>The tokens representing this object's properties.</summary>
  /// <remarks>This will be a TokenCollection</remarks>
  PropertyList,
  FlagList,
  /// <summary>The tokens representing this object's values.</summary>
  ValueList,
  /// <summary>The token representing this object's left item.</summary>
  Left,
  /// <summary>The token representing this object's right item.</summary>
  Right,
  /// <summary>The token representing this object's center item.</summary>
  Center
}
