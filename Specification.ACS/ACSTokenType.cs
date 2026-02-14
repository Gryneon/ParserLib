#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1720 // Identifier contains type name

namespace Specification.ACS;

public enum ACSTokenType
{
  None,
  Str,
  Char,
  Int,
  Fixed,
  Bool,
  Name,
  FunctionCall,
  Script,
  Function,
  MapVar,
  ScriptType,
  Value,
  Bo, Bc,
  Po, Pc,
  Expression,
  Sc, Cm,
  Op, Co,
  Ao, Ac,
  Preprocessor,
  Eq,
  If,
  For,
  IncDec,
  LogNot,
  Assign,
  Minus,
  Unary,
  Binary,
  Net,
  Loop,
  Do,
  Switch,
  Case,
  Default,
  Return,
  SimpleJump,
  Void,
  Type,
  Wait,
  PreprocessorFull,
  IfBlock,
  ElseBlock,
  ElseIfBlock,
  VarDecl,
  VarDeclAssn,
  ArrayDecl,
  VarAssn,
  Else,
  Statement,
  BasicCmd,
  VarInc,
  Parameter,
  ArrayValue,
  ParenExpression,
  FunctionHeader,
  ScriptHeader,
  ParameterValue,
  WaitCall
}

public enum ModelDefTokenType
{
  None,

  //Structures
  Model,
  Skin,

}
