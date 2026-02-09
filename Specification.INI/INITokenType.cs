namespace Specification.INI;

public enum INITokenType
{
  /// <summary>No type defined.</summary>
  None,
  /// <summary>A section title.</summary>
  Section,
  /// <summary>A property key and value.</summary>
  Property,
  /// <summary>A string.</summary>
  Str,
  /// <summary>A property key.</summary>
  Key,
  /// <summary>A property value.</summary>
  Value,
  /// <summary>A section with properties in it.</summary>
  SectionWProps,
  /// <summary>The equals sign.</summary>
  Eq
}
