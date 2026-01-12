#pragma warning disable CA1720 // Identifier contains type name

namespace Specification.UDMF;

/// <summary>Token types for UDMF Spec.</summary>
public enum UDMFTokenType
{
  /// <summary>Undefined or unmatched token.</summary>
  None,

  /// <summary>Vertex keyword.</summary>
  Vertex,
  /// <summary>Thing keyword.</summary>
  Thing,
  /// <summary>Namespace keyword.</summary>
  Namespace,
  /// <summary>SideDef keyword.</summary>
  SideDef,
  /// <summary>LineDef keyword.</summary>
  LineDef,
  /// <summary>Sector keyword.</summary>
  Sector,

  // Data Types
  Str,      // Quoted text
  Name,     // Unquoted text
  PInt,     // Positive Integer
  Int,      // Any Integer
  Dec,      // Decimal Value
  Value,    // Any Data Type
  Bool,

  /// <summary>Equals sign.</summary>
  Eq,
  /// <summary>Semicolon.</summary>
  Sc,
  /// <summary>Bracket open '{'.</summary>
  Bo,
  /// <summary>Bracket close '}'.</summary>
  Bc,
  /// <summary>An assembled property definition.</summary>
  Property,
  Op,
  Object
}

