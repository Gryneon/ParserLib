namespace Common.Regex;

public class AbsentGroupException (string group) : KeyNotFoundException($"Group '{group}' not found.") { }