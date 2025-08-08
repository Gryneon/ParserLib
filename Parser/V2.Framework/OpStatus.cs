namespace Parser.V2.Framework;

public enum OpStatus
{
  Any = -2,
  Error = -1,
  AtStart = 0,
  Pass = 1,
  Skipped = 2,
  FailOverride = 4,
  StartInnerLoop = 8,

  Fail = 256,

  FailBadInputNull = Fail * 2,
  FailBadOpDefinition = Fail * 3,
  FailBadOpResult = Fail * 4,
  FailBadOpImpossible = Fail * 5,
  FailInference = Fail * 6,
  FailBufferOverflow = Fail * 7,
  FailNoSpec = Fail * 8,
  FailNoSuchVarName = Fail * 9,
  FailBadInputType = Fail * 10,
  FailNullOpResult = Fail * 11,

  PastFail = Fail * 12,

  EndCommand = 65536,
  NoData = EndCommand * 2,
}
