#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;

namespace Specification.WAD;

public struct WadLump (object position, object size, object name, object data) : ICloneable, IEquatable<WadLump>
{
  public int Position { get; set; } = (int) position;
  public int Size { get; set; } = (int) size;
  public string Name { get; set; } = (string) name;
  public Memory<byte> Data { get; set; } = (Memory<byte>) data;

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
