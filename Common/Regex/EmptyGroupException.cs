namespace Common.Regex;

/// <summary>
/// Exception thrown when a match group is empty but is required to contain characters.
/// </summary>
/// <param name="group">The name of the empty group.</param>
public class EmptyGroupException (string group) : KeyNotFoundException($"Group '{group}' is Empty.") { }