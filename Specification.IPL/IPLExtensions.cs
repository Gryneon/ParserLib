#pragma warning disable IDE0028 // Simplify collection initialization

namespace Specification.IPL;

/// <summary>Extensions for this Spec.</summary>
public static class IPLExtensions
{
  /// <summary>Converts this <see cref="MatchDataSet"/> into a <see cref="CommandDataSet"/> object.</summary>
  /// <param name="mdd">The match data to use.</param>
  /// <returns>A <see cref="CommandDataSet"/> object.</returns>
  public static CommandDataSet ToCommandData (this MatchDataSet mdd) => new(mdd);
}
