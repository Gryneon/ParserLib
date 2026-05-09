#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace Parser.Tokens;

public interface IParseObject
{
  bool HasContent (object keyName);
  bool HasMarker (object keyName);
  object GetData (object keyName);
  object this[object keyName] { get => GetData(keyName); }
}
