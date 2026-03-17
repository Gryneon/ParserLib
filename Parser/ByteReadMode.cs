#pragma warning disable IDE0060 // Remove unused parameter

namespace Parser;

[Flags]
public enum ByteReadMode
{
  None = 0,
  Text = 1,
  Value = 2,
  Binary = 3,

  ToList = 4,
}
