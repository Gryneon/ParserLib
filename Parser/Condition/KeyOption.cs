namespace Parser.Condition;

public enum KeyOption
{
  LoadKey,        // [key_name]
  CountOfKey,     // count[key_name]
  CheckKeyExists, // exists[key_name]
  TypeOfKey,      // type[key_name]
  Literal,        // {literal text}
  True,           // true
  False,          // false

  OpIs,           // is
  OpEq,           // ==
  OpGt,           // >
  OpLt,           // <
  OpGteq,         // >=
  OpLteq,         // <=
  OpNotEq,        // !=

  OpAnd,          // &&
  OpOr,           // ||

}
