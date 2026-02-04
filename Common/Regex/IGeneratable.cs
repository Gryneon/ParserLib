using System.Diagnostics.CodeAnalysis;

namespace Common.Regex;

/// <summary>Represents a type that can be generated from an input object.</summary>
public interface IGeneratable
{
  /// <summary>Throws a <see cref="NotImplementedException"/> to indicate that the method needs to be overridden in the implementing class.</summary>
  /// <typeparam name="T">The dummy return for in-line use.</typeparam>
  /// <returns>Nothing.</returns>
  /// <exception cref="NotImplementedException"/>
  [DoesNotReturn]
  static T ThrowImplementException<T> () => throw new NotImplementedException("This needs to be overridden in the targeted class.");
  static TOut Generate<TIn, TOut> (TIn input) => ThrowImplementException<TOut>();
  static virtual bool TryGenerate<TIn, TOut> (TIn input, [NotNullWhen(true)][MaybeNullWhen(false)] out TOut output)
  {
    try
    {
      if (input is not TIn @in)
        throw new ArgumentException(nameof(@in));
      output = Generate<TIn, TOut>(@in);
      return output is null ? throw new ArgumentNullException(nameof(output)) : true;
    }
    catch (ArgumentNullException)
    {
      output = default;
      return false;
    }
  }

}
