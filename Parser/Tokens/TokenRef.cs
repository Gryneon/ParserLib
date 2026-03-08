#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

/// <summary>A tokenPiece reference.</summary>
public enum TokenRef
{
  Error = -1,
  Ignore = 0,

  Name,
  Type,
  Value,
  ValueList,
  Parameter,
  ParameterList,
  Property,
  PropertyList,
  Statement,
  StatementList,
  Left,
  Center,
  Right,
  AddFlag,
  SubFlag,
  AddFlagList,
  SubFlagList,

  Custom,
  /// <summary>This token sequence entry will supply any fields not already filled by other definitions from its own respective values.</summary>
  /// <remarks>If a <see cref="ComplexToken"/> was passed to any field of a <see cref="ComplexToken"/> with this flag assigned, all of its parts would populate the one being created..
  /// These properties would be overwritten by any defined token sequence entries.</remarks>
  Inherit,
}
