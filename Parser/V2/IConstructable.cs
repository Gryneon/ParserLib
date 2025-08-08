#pragma warning disable IDE0072 // Add missing cases

using Common.Regex;

namespace Parser.V2;

public interface IConstructable<out TOutput> where TOutput : IConstructable<TOutput>, new()
{
  MatchDataDictionary StoredInput { get; set; }

  static abstract TOutput Generate (MatchDataDictionary input);
}
