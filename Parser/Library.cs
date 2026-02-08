#pragma warning disable CA1710 // Rename Parser.Library to end in either 'Dictionary' or 'Collection'

using Parser.Inference;

namespace Parser;

public sealed class Library : IReadOnlyDictionary<string, Spec>
{
  private readonly Dictionary<string, Spec> _specs = [];
  /// <summary>The singleton instance of this object.</summary>
  private static Library? Instance { get; set; }
  private static Dictionary<string, ReadOnlyCollection<IInferenceNode>> SpecInferences =>
    [.. Instance?.Select(
      item => new KeyValuePair<string, ReadOnlyCollection<IInferenceNode>>(item.Key, item.Value.FileInferences)) ?? throw new InvalidOperationException("Library must be initialized.")];

  private Library (AppDomain domain)
  {
    List<Assembly> assemblies = [.. domain.GetAssemblies()];

    IOrderedEnumerable<Assembly> sorted = assemblies.OrderBy(static i => i.GetName().Name);

    foreach (Assembly assembly in assemblies)
    {
      Type[] types = assembly.GetTypes();

      IEnumerable<Type> relevant_types = types.Where(t => t.CustomAttributes.Any(ca => ca.AttributeType == typeof(DefinitionExportAttribute)));

      foreach (Type type in relevant_types)
      {
        foreach (PropertyInfo? prop in type.GetProperties().Where(prop => prop.PropertyType == typeof(Spec) && prop.PropertyType.CustomAttributes.Any(prop_ca => prop_ca.AttributeType == typeof(ExportAttribute))))
        {
          Spec check = (Spec) prop.GetValue(null)!;
          _specs.Add(check.Name, check);
        }
      }
    }
  }

  public static Spec? Lookup (string? name) => name is not null && Instance is not null && Instance.ContainsKey(name) ? Instance[name] : null;
  public static bool TryLookup (string name, [NotNullWhen(true)][MaybeNullWhen(false)] out Spec spec)
  {
    if (Instance?.ContainsKey(name) ?? false)
    {
      spec = Instance[name];
      return true;
    }
    else
    {
      spec = null;
      return false;
    }
  }

  public static void InitializeLibrary (AppDomain domain)
  {
    domain.ThrowIfNull();
    Instance = new(domain);
  }
  public static Spec LookupOrDefault (string? name) => (name is null || !TryLookup(name, out Spec? spec)) ? DefaultSpec.Unknown : spec;
  public int Count => _specs.Count;
  /// <summary>Provides the <see cref="Spec"/> for the provided file path.</summary>
  public static string? CheckFile (string path)
  {
    if (Instance is null)
      throw new InvalidOperationException("Library must be initialized.");

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

  public void Add (string key, Spec value) => _specs.Add(key, value);
  public void AddRange (IEnumerable<KeyValuePair<string, Spec>> list) => _specs.AddRange(list);
  public bool ContainsKey (string key) => _specs.ContainsKey(key);
  public bool TryGetValue (string key, [MaybeNullWhen(false)] out Spec value) => _specs.TryGetValue(key, out value);
  public void Add (KeyValuePair<string, Spec> item) => _specs.Add(item);
  public IEnumerator<KeyValuePair<string, Spec>> GetEnumerator () => _specs.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator () => _specs.GetEnumerator();
  public static ReadOnlyCollection<Spec> SpecList => Instance is null ? throw new InvalidOperationException("Library must be initialized.") : [.. Instance._specs.Values];

  IEnumerable<string> IReadOnlyDictionary<string, Spec>.Keys => _specs.Keys;
  IEnumerable<Spec> IReadOnlyDictionary<string, Spec>.Values => _specs.Values;

  public Spec this[string key] { get => _specs[key]; set => _specs[key] = value; }
}
