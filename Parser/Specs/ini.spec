Spec "ini"
{
  Format = Text;
  Inferences
  {
    ExtIs Or { "ini", "url", "vnc", "inf" }
  }
  Tokens
  {
    RegexOptions 
    {
      MultiLine,
      ExplicitCapture,
      IgnorePatternWhitespace,
      CaseInsensitive
    }
    Rules
    {
      // Comment Line
      // use "-" for "None" or omit type.
      // regex extends from after whitespace following type until the first line break
      // UNLESS the next line starts with a => sequence
      // You Must use Regex Comments with (?#Comment)
      TokenComment "-"       ;.*
      Competitive   "Value"   (?<==) \s* (?'keep'[^\\=\n;]|\\.)*? \s* (?=$|;)
      Competitive  "Key"     (?<= ^\s*) ([^[\]\s\\=\n;]|\\.)+
      =>                     (?# Continuation to the next line)
      TokenExact   "Eq"      =
      TokenExtract "Section" \[  (?'keep'.*?)  \]
    }
    Groups
    {
      // Group Constructs
      "Property"       n:Key x:Eq v:Value
      "SectionWProps"  n:Section pa:Property
    }
    Constructs
    {
      Construct "Property"
      {
        Condition = Type Is Property, ;
        Type = KeyValuePair<string,string>;
        Property "Key" = Name
      }
    }
  }
}