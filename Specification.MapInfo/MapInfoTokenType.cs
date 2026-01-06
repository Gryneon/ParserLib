#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable IDE1006 // Naming Styles

namespace Specification.MapInfo;

public enum MapInfoTokenType
{
  Ws,
  LnComment,
  BlkComment,
  Include,
  Damagetype,
  Doomednums,
  Property,
  Class,
  AInt,
  Dec,
  Str,
  LangRef,
  AChar,
  Op,
  Bool,
  BlockKeyword,
  Keyword,
  Name,
}
