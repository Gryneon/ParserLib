#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;

namespace Specification.WAD;

public struct WadLump : ICloneable, IEquatable<WadLump>
{
  public int Position { get; set; }
  public int Size { get; set; }
  public string Name { get; set; }
  public Memory<byte> Data { get; set; }

  public object Clone () => new WadLump()
  {
    Data = Data,
    Name = Name,
    Size = Size,
    Position = Position
  };

  public override readonly bool Equals (object? obj) =>
    obj is WadLump && obj.Equals(this);

  public override readonly int GetHashCode () =>
    HashCode.Combine(Position, Size, Name, Data);

  public static bool operator == (WadLump left, WadLump right) => left.Equals(right);

  public static bool operator != (WadLump left, WadLump right) => !(left == right);

  public readonly bool Equals (WadLump other) =>
    Position == other.Position &&
    Size == other.Size &&
    Name == other.Name &&
    Data.Span.SequenceEqual(other.Data.Span);
}
