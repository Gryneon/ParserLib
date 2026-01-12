#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Rule Violation

namespace Specification.ZScript;

//Decorate Prototype
//https://regex101.com/r/YtlFqj/1

//ZScript Tokenizer
//https://regex101.com/r/dM72bX/1

public enum ZScriptTokenType
{
  None,
  Class,
  Include,
  Num,
  Str,
  Name,
  State,
  Keyword,
  Comment,
  AddFlag,
  SubFlag,
  Property,
  FunctionCall,
  StateCmd,
  Value,
  Array,
  ClassRef
}
