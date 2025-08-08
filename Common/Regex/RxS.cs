//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Common.Regex;

/// <summary>
/// A box that contains a string representing a regular expression.
/// </summary>
/// <seealso cref="string" />
public readonly struct RxS : IEquatable<string>, IComparable<string>
{
  /// <summary>
  /// Gets the string value of this regular expression.
  /// </summary>
  /// <value>
  /// The content of this regular expression.
  /// </value>
  public string Content { get; init; }
  /// <summary>
  /// Gets the length of the content.
  /// </summary>
  /// <value>
  /// The length of the content.
  /// </value>
  public int Length => Content.Length;
  /// <summary>
  /// Retrieves the regular expression stored.
  /// </summary>
  /// <returns>The regular expression as a string.</returns>
  public override string ToString () => Content;
  /// <summary>
  /// Compares the regular expressions for equality.
  /// </summary>
  /// <param name="other">The string to compare to.</param>
  /// <returns><see langword="true"/> if the two regular expressions are based on the same pattern string, <see langword="false"/> otherwise.</returns>
  public bool Equals ([SS("Regex")] string? other) => Content.Equals(other, SCO);
  public int CompareTo (string? other) => Content.CompareTo(other);

  public static implicit operator string (RxS rx) => rx.Content;
  public static implicit operator RxS ([SS("Regex")] string str) => Rx(str);

  public static RxS operator + (RxS rx1, [SS("Regex")] string rx2) => $"{rx1}{rx2}";
  public static RxS operator ! (RxS rx1) => NegLkAhd(rx1);
  /// <summary>
  /// Negative lookahead shorthand.
  /// </summary>
  /// <param name="rx1">Lefthand regular expression.</param>
  /// <param name="rx2">Righthand regular expression.</param>
  /// <returns>A regular expression that is a joining of the 2 expressions.</returns>
  public static RxS operator >> (RxS rx1, [SS("Regex")] string rx2) => rx1 + NegLkAhd(rx2);
  /// <summary>
  /// Negative lookbehind shorthand.
  /// </summary>
  /// <param name="rx1">Lefthand regular expression.</param>
  /// <param name="rx2">Righthand regular expression.</param>
  /// <returns>A regular expression that is a joining of the 2 expressions.</returns>
  public static RxS operator << (RxS rx1, [SS("Regex")] string rx2) => rx1 + NegLkBhd(rx2);
  /// <summary>
  /// Positive lookahead shorthand.
  /// </summary>
  /// <param name="rx1">Lefthand regular expression.</param>
  /// <param name="rx2">Righthand regular expression.</param>
  /// <returns>A regular expression that is a joining of the 2 expressions.</returns>
  public static RxS operator >= (RxS rx1, [SS("Regex")] string rx2) => rx1 + PosLkAhd(rx2);
  /// <summary>
  /// Positive lookbehind shorthand.
  /// </summary>
  /// <param name="rx1">Lefthand regular expression.</param>
  /// <param name="rx2">Righthand regular expression.</param>
  /// <returns>A regular expression that is a joining of the 2 expressions.</returns>
  public static RxS operator <= (RxS rx1, [SS("Regex")] string rx2) => rx1 + PosLkBhd(rx2);

  public RxS Lazy => $"{Content}?";
  public RxS Any => $"{Grp(Content)}*";
  public RxS Many => $"{Grp(Content)}+";
  public RxS Opt => $"{Grp(Content)}?";

  public RxS Bk => @$"{Content}\b";
  public RxS End => @$"{Content}$";

  public RxS WS => @$"{Content}\s+";
  public RxS WSO => @$"{Content}\s*";

  public RxS Or ([SS("Regex")] string content) => $"{Content}|{Grp(content)}";
  public RxS Or (IEnumerable<string> list) => $"{Content}|{Grp(list.TextJoin("|"))}";
  public static RxS Or ([SS("Regex")] string content, params Collection<string> values) => Grp($"{content}|{values.TextJoin("|")}");
  /// <summary>
  /// Append a pipe operator.
  /// </summary>
  public RxS Orr => $"{Content}|";

  public RxS ([SS("Regex")] string s) => Content = s;
  public RxS (RxS rx) => Content = rx;

  public static RxS Start => Rx(@"\A");
  public static RxS LnStart => Rx("^");
  public static RxS TruEnd => Rx(@"\z");

  public static RxS Rx ([SS("Regex")] string rx) => new(rx);
  public static RxS Grp ([SS("Regex")] string rx) => $"(?:{rx})";
  public static RxS GrpNm (string name, [SS("Regex")] string rx) => $"(?<{name}>{rx})";
  public static RxS PosLkAhd ([SS("Regex")] string s) => $"(?={s})";
  public static RxS NegLkAhd ([SS("Regex")] string s) => $"(?!{s})";
  public static RxS PosLkBhd ([SS("Regex")] string s) => $"(?<={s})";
  public static RxS NegLkBhd ([SS("Regex")] string s) => $"(?<!{s})";
  public static RxS If (int backRef, [SS("Regex")] string ifMatch, [SS("Regex")] string ifNot) => $"(?({backRef}){ifMatch}|{ifNot})";
  public static RxS If ([SS("Regex")] string expr, [SS("Regex")] string ifMatch, [SS("Regex")] string ifNot) => $"(?({expr}){ifMatch}|{ifNot})";
  public static RxS BackRef (string name) => $@"\k<{name}>";
  public static RxS BackRef (int num) => $@"\k<{num}>";
  public static RxS Atomic ([SS("Regex")] string rx) => $"(?>{rx})";
  public static RxS Char (string allowed) => $"[{allowed}]";
  public static RxS NChar (string notAllowed) => $"[^{notAllowed}]";
  public static RxS OOr (IEnumerable<string> list) => list.AggregateRegex();

  public RxS Add ([SS("Regex")] string append) => $"{Content}{Grp(append)}";

  public RxS Qty (int qty) => Grp(this) + $"{{{qty}}}";
  public RxS Qty (int min, int max) => Grp(this) + $"{{{min},{max}}}";

  // Substitution

  public static RxS Ref (int group) => $"\\{group}";
  public static RxS Ref (string name) => $"${{{name}}}";

  public static RxS BeforeMatch => "$`";
  public static RxS AfterMatch => "$'";
  public static RxS EntireMatch => "$&";
  public static RxS EntireInput => "$_";
  public static RxS LastGroup => "$+";

  public bool Is (RxS other) => Content.Is(other);
  public bool Like (RxS other) => Content.Like(other);
}
