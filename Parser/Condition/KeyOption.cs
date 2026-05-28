namespace Parser.Condition;

public enum KeyOption
{
  Null,           // null OR NULL OR NuLl
  LoadKey,        // [key_name]
  CountOfKey,     // countof[key_name]
  CheckKeyExists, // exists[key_name]
  TypeOfKey,      // typeof[key_name]

  Literal,        // {literal text}
  True,           // true OR TRUE OR TrUe
  False,          // false OR FALSE OR FaLsE
  Integer,        // integer
  Decimal,        // decimal

  OpStart = 100,  // Operator start marker

  OpIs,           // is (ordinal compare, or type compare.)
  OpLike,         // like (case insensitive compare)
  OpEq,           // == (equality)
  OpGt,           // > (greater than)
  OpLt,           // < (less than)
  OpGteq,         // >=
  OpLteq,         // <=
  OpNotEq,        // != (not equal to)

  OpAnd,          // && (logical and)
  OpOr,           // || (logical or)
  OpAdd,          // + (add)
  OpSub,          // - (subtract)
  OpDiv,          // / (divide)
  OpMul,          // * Multiply
  OpExp,          // ^ (Exponent)
  OpMod,          // % (Modulus)
  OpLBs,          // << (Left Bitshift)
  OpRBs,          // >> (Right Bitshift)
  OpSqt,          // root (Any Root, just negative exp)
  OpSeqEq,        // matches (Sequence Equals for collections)

}
