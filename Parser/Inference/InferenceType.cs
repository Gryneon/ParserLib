namespace Parser.Inference;

[Flags]
public enum InferenceType
{
  None = 0,

  Ext,
  FName,
  FileHeader,
  FileContent,
  FileSize,

  And,
  Or,

  Is = 0x1000,      // Case sensitive equals
  Like = 0x80000,   // Case insensitive equals
  End = 0x2000,     // EndsWith (case insensitive)
  Start = 0x4000,   // StartsWith (case insensitive)
  Contains = 0x8000,// contatins (case insensitive)
  Larger = 0x10000, // greater than (numeric
  Smaller = 0x20000,// less than (numeric)
  Not = 0x40000     // inverse
}
