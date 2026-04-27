namespace Parser.Inference;

[Flags]
public enum InferenceType
{
  None = 0,

  //Property to look at
  Ext = 0x1,
  FName = 0x2,
  FileHeader = 0x4,
  FileContent = 0x8,
  FileSize = 0x10,

  //Special for alternates
  And = 0x100,
  Or = 0x200,

  //Text Comparison
  Is = 0x1000,      // Case sensitive equals
  End = 0x2000,     // EndsWith (case insensitive)
  Start = 0x4000,   // StartsWith (case insensitive)
  Contains = 0x8000,// contatins (case insensitive)

  //Numeric Comparison
  Larger = 0x10000, // greater than (numeric)
  Smaller = 0x20000,// less than (numeric)
  Equal = 0x40000,  // equal to (numeric)
  LargerOrEqual = Larger | Equal,
  SmallerOrEqual = Smaller | Equal,
  Like = 0x80000,   // Case insensitive equals

  // Logical
  Not = 0x100000     // Inverse can be applied to any combination.
}
