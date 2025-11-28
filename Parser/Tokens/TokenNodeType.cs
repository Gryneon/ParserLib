#pragma warning disable IDE1006 // Naming Styles

namespace Parser.Tokens;

/// <summary>
/// The type of token node.
/// </summary>
public enum TokenNodeType
{
  /// <summary>
  /// No type, this is not valid.
  /// </summary>
  None = 0,
  /// <summary>
  /// Group start '('.
  /// </summary>
  GroupSt = 1,
  /// <summary>
  /// Group end ')'.
  /// </summary>
  GroupEn = 2,
  /// <summary>
  /// Alternative '|'.
  /// </summary>
  Or = 3,
  /// <summary>
  /// A command modifying the previous item.
  /// </summary>
  Command = 4,
  /// <summary>
  /// Any (Opt &amp; More) '*'.
  /// </summary>
  Any = 5,
  /// <summary>
  /// One or many '+'.
  /// </summary>
  More = 6,
  /// <summary>
  /// Optional '?'.
  /// </summary>
  Opt = 7,
  /// <summary>
  /// Literal text or characters.
  /// </summary>
  Literal = 8,
  /// <summary>
  /// Reference to another template group that this group contains.
  /// </summary>
  Ref = 9,
  /// <summary>
  /// Reference to a basic regex group that this group contains.
  /// </summary>
  Base = 10,
  /// <summary>
  /// A fully formed and stackable group.
  /// </summary>
  Group = 11,
}
