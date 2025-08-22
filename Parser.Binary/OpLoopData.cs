using Parser.Ops;

namespace Parser.Binary;

public class OpLoopData (IEnumerable<IOperation> operations, bool makecollections = false, int loops = 1)
{
  public Collection<IOperation> Operations { get; init; } = [.. operations];

  public ByteDataDictionary ByteObjects { get; } = [];
  public int BytePos { get; set; }
  public int OpIndex { get; set; }
  public IOperation Current => Operations[OpIndex];
  public OpStatus Status { get; set; } = OpStatus.AtStart;
  public int LoopsLeft { get; set; } = loops - 1;
  public bool MakeCollectionsOnValues { get; set; } = makecollections;
}
