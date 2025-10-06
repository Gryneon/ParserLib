namespace Parser.Ops;

[Flags]
public enum OperationCondition
{
  None = 0, //Always pass
  Fail = 1, //Always fail

  CompareKeyToKey,
  CompareKeyToObject,
  KeyContainsObject,
  KeyContainsKey,
  KeyCountIs,
  KeyCountGreaterThan,
  HasKey,

  Not = 0x800000,
}
