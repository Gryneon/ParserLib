namespace Parser;

public enum CursorType
{
  /// <summary>The cursor key is the current index of the iterative loop.</summary>
  KeyIsIndex,
  /// <summary>The cursor key is <see langword="true"/> while the loop is active.</summary>
  KeyIsBoolean,
  /// <summary>The cursor key is the <see cref="IList{T}"/> of the currently executing loop.</summary>
  KeyIsCollection
}
