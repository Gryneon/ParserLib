#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable format // Formatting

namespace Common;
public interface IEntity : IParsedEntity
{
  IDictionary<string, IList<IParsedEntity>> PropertyCollections { get; }
  IList<IParsedEntity> Children { get; }
  IDictionary<string, IParsedEntity> PropertyValues { get; }
  IDictionary<string, object?> DataValues { get; }
}
