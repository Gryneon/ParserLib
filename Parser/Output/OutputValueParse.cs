
namespace Parser.Output;

public struct OutputValueParse (string varName, string groupName, string value) : IOutputValue, IEquatable<IOutputValue>, IEquatable<OutputValueParse>
{
  public override readonly bool Equals (object? obj) => obj is IOutputValue val && VarName.Equals(val.VarName, SCO) && GroupName.Equals(val.GroupName, SCOIC);
  public override readonly int GetHashCode () => HashCode.Combine(VarName, GroupName);
  public readonly bool Equals (IOutputValue? other) => VarName.Equals(other?.VarName, SCO) && GroupName.Equals(other.GroupName, SCOIC);
  public readonly bool Equals (OutputValueParse other) => VarName.Equals(other.VarName, SCO) && GroupName.Equals(other.GroupName, SCOIC);
  public static bool operator != (OutputValueParse left, OutputValueParse right) => !(left == right);
  public static bool operator == (OutputValueParse left, OutputValueParse right) => left.Equals(right);
  public string GroupName { get; set; } = groupName;
  public string ParseType { get; set; } = value;
  public string VarName { get; set; } = varName;
}

public interface IReq<in T> where T : IParseItem
{
  bool MeetsRequirement (IEnumerable<IParseItem> items);
  bool MeetsRequirement (T item);
}
public interface IContextReq<in T> : IReq<T> where T : IParseItem
{
  bool AllMeetRequirement (IEnumerable<T> items);
}
public interface IParseItem
{
  string KeyName { get; init; }
  string? Content { get; init; }
  Dictionary<string, string> Properties { get; init; }
  bool HasProperties { get; }
  bool IsValid { get; }
  IParseNode Node { get; init; }

  bool TryGetProperty (string key, out string? value);
  string? GetProperty (string key);
  bool HasProperty (string key);
}
public interface IParseItemWithChildren : IParseItem
{
  Collection<IParseItem> Children { get; }
  int Count { get; }
  void Add (IParseItem item);
  IParseItem this[int index] { get; }
}
public interface IParseNode
{
  string Type { get; }
  Collection<IReq<IParseItem>> Requirements { get; }
  bool HoldsChildren { get; }
  string Format { get; }

  bool MeetsRequirements (IParseItem item);
}

public class HasPropertyReq (string keyName) : IReq<IParseItem>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    item.HasProperty(keyName);
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
}
public class PropertyIsTypeReq<T> (string keyName) : IReq<IParseItem>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    item.HasProperty(keyName) &&
    item.Properties[keyName] is T;
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
}
public class PropertyEqualsReq<T> (string keyName, T desired_value) : IReq<IParseItem> where T : IEquatable<T>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    item.HasProperty(keyName) &&
    item.Properties[keyName] is T casted &&
    casted.Equals(desired_value);
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
}
public class ContentContainsReq (string contains_value, StringComparison sc) : IReq<IParseItem>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    (item.Content?.Contains(contains_value, sc) ?? false);
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
}
public class ContentLengthReq (int min_length, int max_length) : IReq<IParseItem>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    item.Content?.Length <= (max_length < 0 ? 0x7fffffff : max_length) &&
    item.Content.Length >= min_length;
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
  public ContentLengthReq (int exact_length) : this(exact_length, exact_length) { }
}
public class NameIsNotReq (IEnumerable<string> list_of_names, StringComparison sc) : IReq<IParseItem>
{
  public bool MeetsRequirement (IParseItem item) =>
    item is not null &&
    list_of_names.All(bad_name => !item.KeyName.Equals(bad_name, sc));
  public bool MeetsRequirement (IEnumerable<IParseItem> items) => items.All(MeetsRequirement);
  public NameIsNotReq (string single_name, StringComparison sc) : this([single_name], sc) { }
}
public class ChildTypeIsReq (IEnumerable<string> list_of_types) : IReq<IParseItemWithChildren>
{
  public bool MeetsRequirement (IParseItemWithChildren item) =>
    item is not null &&
    list_of_types.
      All(bad_type => !item.Children.
        All(child => child.Node.Type.
          Equals(bad_type, SCOIC)));
  public bool MeetsRequirement (IEnumerable<IParseItem> items) =>
    items.
      Select(i2 => i2 as IParseItemWithChildren).
      All(item => item is null || MeetsRequirement(item));

  public ChildTypeIsReq (string single_name) : this([single_name]) { }
}

public class ParseNode : IParseNode
{
  public required string Type { get; init; }
  public Collection<IReq<IParseItem>> Requirements { get; init; } = [];
  public bool HoldsChildren { get; init; }
  public bool RestrictPropertiesToList => AllowedProperties.Count != 0;
  public Collection<string> AllowedProperties { get; init; } = [];
  public Collection<string> AllowedChildrenTypes { get; init; } = [];
  public bool RequiresContext { get; init; }
  public required string Format { get; init; }

  public bool CheckAllItems (IEnumerable<IParseItem> items) => Requirements.All(req => req.MeetsRequirement(items));
  public bool MeetsRequirements (IParseItem item) => Requirements.All(req => req.MeetsRequirement(item));
}

public class Statement : IParseItem, IToken
{
  public bool HasProperties { get; }
  public bool IsValid => Node.MeetsRequirements(this);
  public Dictionary<string, string> Properties { get; init; } = [];
  public int Length => Content?.Length ?? 0;
  public int Depth { get; set; }
  public required int Position { get; set; }
  public required IParseNode Node { get; init; }
  public required string? Content { get; init; }
  public required string Format { get; init; }
  public required string KeyName { get; init; }
  public required string Type { get; init; }
  public TokenNode? LinkNode { get; set; }
  public TokenNodeGroup? FromNode { get; init; }
  public int EndPos { get; }
  CToken? IToken.Node { get; set; }

  [SetsRequiredMembers]
  public Statement (IToken token, IParseNode node)
  {
    token.ThrowIfNull();
    node.ThrowIfNull();
    KeyName = Properties["name"];
    Content = token.Content;
    Format = node.Format;
    Node = node;
    Position = token.Position;
    Type = token.Type;
  }

  public bool TryGetProperty (string key, out string? value)
  {
    if (key is not null && Properties.TryGetValue(key.ToUpperInvariant(), out value))
      return true;
    value = null;
    return false;
  }
  public string? GetProperty (string key) => key is not null && Properties[key.ToUpperInvariant()] is string s ? s : null;
  public bool HasProperty (string key) => key is not null && Properties.ContainsKey(key.ToUpperInvariant());
}

public class Expression : IParseItem
{
  public bool HasProperties { get; }
  public bool IsConstant { get; init; }
  public bool IsValid { get; }
  public Dictionary<string, string> Properties { get; init; } = [];
  public required IParseNode Node { get; init; }
  public required string? Content { get; init; }
  public required string Type { get; init; }
  public required string KeyName { get; init; }

  public string? GetProperty (string key) => throw new NotImplementedException();
  public bool HasProperty (string key) => throw new NotImplementedException();
  public bool TryGetProperty (string key, out string? value) => throw new NotImplementedException();
}
