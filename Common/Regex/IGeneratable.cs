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
}

/// <summary>Represents a type that can be generated from an input object.</summary>
public interface IGeneratable<TIn, TOut> : IGeneratable
    where TOut : IGeneratable<TIn, TOut>
{
  /// <summary>Generates a <typeparamref name="TOut"/> from a <typeparamref name="TIn"/>.</summary>
  /// <param name="input">The object to generate from.</param>
  /// <returns>The generated <typeparamref name="TOut"/>.</returns>
  static virtual TOut? Generate (TIn input) => ThrowImplementException<TOut>();
  static virtual bool TryGenerate (TIn input, [NotNullWhen(true)][MaybeNullWhen(false)] out TOut output)
  {
    try
    {
      output = TOut.Generate(input);
      return output is null ? throw new ArgumentNullException(nameof(output)) : true;
    }
    catch (ArgumentNullException)
    {
      output = default;
      return false;
    }
  }
  static virtual bool TryGenerate (object input, [NotNullWhen(true)][MaybeNullWhen(false)] out TOut output)
  {
    try
    {
      if (input is not TIn @in)
        throw new ArgumentException(nameof(@in));
      output = TOut.Generate(@in);
      return output is null ? throw new ArgumentNullException(nameof(output)) : true;
    }
    catch (ArgumentNullException)
    {
      output = default;
      return false;
    }
  }
}
