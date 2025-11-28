using System.Reflection;

using Parser.Inference;

namespace Parser;

public class Library : KeyedCollection<string, ISpec>
{
  /// <summary>
  /// The singleton instance of this object.
  /// </summary>
  protected static Library Instance { get; } = [Spec.Unknown];
  protected static Dictionary<string, ReadOnlyCollection<IInferenceNode>> SpecInferences =>
    [.. Instance.Select(
      item => new KeyValuePair<string, ReadOnlyCollection<IInferenceNode>>(item.Name, item.FileInferences))];

  protected Library ()
  {
    IReadOnlyList<(Type Type, PropertyInfo Property)> matches = ReflectionHelper.GetTypesWithAttributeAndProperties<DefinitionExportAttribute>(typeof(ISpec));

    foreach ((Type? type, PropertyInfo? prop) in matches)
    {
      Console.WriteLine($"Type: {type.FullName}, Property: {prop.Name}");
    }
  }
  protected override string GetKeyForItem (ISpec item) => item?.Name ?? SE;

  public static void AddToLibrary (ISpec spec) => Instance.Add(spec);
  public static void AddToLibrary (IEnumerable<ISpec> specs) => Instance.AddRange(specs);
  public static void AddToLibrary (params Collection<ISpec> speclist) => Instance.AddRange(speclist);
  public static TSpec? Lookup<TSpec> (string? name) where TSpec : class, ISpec => (name is not null && Instance.Contains(name) ? Instance[name] : null) as TSpec;
  public static bool TryLookup<TSpec> (string name, [NotNullWhen(true)][MaybeNullWhen(false)] out TSpec spec) where TSpec : Spec
  {
    if (Instance.Contains(name) && Instance[name] is TSpec s)
    {
      spec = s;
      return true;
    }
    else
    {
      spec = null;
      return false;
    }
  }
  public static new int Count => Instance.Count();
  /// <summary>
  /// Provides the <see cref="Spec"/> for the provided file path.
  /// </summary>
  public static string? CheckFile (string path)
  {
    foreach (KeyValuePair<string, ReadOnlyCollection<IInferenceNode>> fi in SpecInferences)
    {
      foreach (IInferenceNode node in fi.Value)
      {
        if (node.CheckFile(path))
        {
          Log($"CheckFile({path})", "File match found. S");
          return fi.Key;
        }
      }
    }
    return null;
  }
  public static IEnumerable<ISpec> SpecList => Instance.ToCollection();
}
