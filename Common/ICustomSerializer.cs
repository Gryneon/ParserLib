namespace Common;

public interface ICustomSerializer<in TSelf, out TOutput> where TSelf : ICustomSerializer<TSelf, TOutput>
{
  abstract TOutput Serialize ();
  static TOutput Serialize (TSelf item) => item.Serialize();
}
