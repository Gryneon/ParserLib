#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser;

public enum ByteReadMode
{
  Error = 0,
  Text = 1,
  Value = 2,
  Binary = 3,

  TextToList = 4,
  ValueToList = 5,
  BinaryToList = 6,
}
