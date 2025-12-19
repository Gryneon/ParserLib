namespace Common.Types;

/// <summary>A definition for a file extension.</summary>
public class FileExt
{
  /// <summary>The file extension.</summary>
  public required string Ext { get; set; }
  public required string Class { get; set; }
  public string? ContentType { get; set; }
  public string? PerceivedType { get; set; }
}

/// <summary>A definition for an object class.</summary>
public class FileClass
{
  public required string Name { get; set; }
  public string? Icon { get; set; }
}
