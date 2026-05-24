namespace Parser.Condition;

public enum KeyOption
{
  LoadKey,        // [key_name]
  CountOfKey,     // countof[key_name]
  CheckKeyExists, // exists[key_name]
  TypeOfKey,      // typeof[key_name]

  Literal,        // {literal text}
  True,           // true OR TRUE OR TrUe
  False,          // false OR FALSE OR FaLsE
  Null,
  Integer,
  Decimal,

  OpIs,           // is (ordinal compare)
  OpLike,         // like (case insensitive compare)
  OpEq,           // == (equality)
  OpGt,           // > (greater than)
  OpLt,           // <
  OpGteq,         // >=
  OpLteq,         // <=
  OpNotEq,        // != (not equal to)

  OpAnd,          // && (logical and)
  OpOr,           // || (logical or)

}
