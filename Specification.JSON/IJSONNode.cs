#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Text.Json;

namespace Specification.JSON;

/// <summary>Basic interface for JSON parts.</summary>
public interface IJSONNode
{
  /// <summary>The type of node this is.</summary>
  JsonValueKind Type { get; }
  /// <summary>The value stored in this node.</summary>
  object? Value { get; }
}
