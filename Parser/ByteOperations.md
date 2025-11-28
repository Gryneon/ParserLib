 ## Byte Operations
    
    index = byte position
    varname = stored name in dictionary
    size = size in bytes of data or char count
    index_varname = name in dictionary of field that contains an index
    count_varname = name in dictionaty of field that contains an item count
    ops_loop = list of operations that are followed in sequence for each entry
    index_each_varname = name in inner dictionary that contains an index for each entry
    size_each_varname = name in inner dictionary that contains a byte size for each entry
    
    ByteStartAt = 1024, // (int index)
    ByteJumpVar, // (string index_varname)
    ByteReadInt, // (int size, string varname)
    ByteReadStr, // (int size, string varname)
    ByteReadDat, // (int size, string varname)
    ByteLdIndex, // (string varname, string count_varname, List<Operations> ops_loop)
    ByteLdDataB, // (string varname, string index_each_varname, string size_each_varname)
    ByteSavePos, // (string varname)
    ByteRdDatEn, // (string varname)