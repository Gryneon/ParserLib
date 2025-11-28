#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser;

[Flags]
public enum ByteReadMode
{
  None = 0,
  Text = 1,
  Value = 2,
  Binary = 4,

  Increment = 256,
  UseVarSize = 1024,
}
