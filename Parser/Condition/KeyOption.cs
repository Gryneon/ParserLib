using static Parser.Condition.KeyOption;

namespace Parser.Condition;

public static class KOExtension
{
  extension(KeyOption ko)
  {
    public bool IsOperator => ko > OpStart;
    public bool UsesObjectInput => ko.IsWithin(OpIs, OpSeqEq);
    public bool UsesLogicalInput => ko.IsWithin(OpAnd, OpOr);
    public bool UsesNumericInput => ko.IsWithin(OpEq, OpSqt);
    public bool IsConstant => ko is Null or Literal or True or False or Integer or KeyOption.Decimal;
  }
}

public enum KeyOption
{
  Undefined = -1, // Initial value
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
  Embedded,       // SubExpression

  OpStart = 100,  // Operator start marker

  // Object In & Logical Out
  OpIs,           // is (ordinal compare, or type compare.)
  OpLike,         // like (case insensitive compare)
  OpSeqEq,        // matches (Sequence Equals for collections)

  // Logical In & Out
  OpAnd,          // && (logical and)
  OpOr,           // || (logical or)

  // Numeric In & Logical Out
  OpEq,           // == (equality)
  OpGt,           // > (greater than)
  OpLt,           // < (less than)
  OpGteq,         // >=
  OpLteq,         // <=
  OpNotEq,        // != (not equal to)

  // Numeric In & Out
  OpAdd,          // + (add)
  OpSub,          // - (subtract)
  OpDiv,          // / (divide)
  OpMul,          // * Multiply
  OpExp,          // ^ (Exponent)
  OpMod,          // % (Modulus)
  OpLBs,          // << (Left Bitshift)
  OpRBs,          // >> (Right Bitshift)
  OpSqt,          // root (Any Root, just negative exp)
}
