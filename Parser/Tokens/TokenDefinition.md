# Token Rules Guide

  This is a guide to explain the new token system, and the rule syntax.

  The type parameter 'T' represents the token type, often expressed as an enum. Any enum will be accepted here.
  Strings are also accepted, the formal requirement is that it is not nullable.

## 1. Tokenizer Flags

* <code>None</code> = <code>0</code> No flags defined. Using <code>None</code> is likely an error.<br/>
### These specify the type of operation for the Tokenizer.

* <code>TokenExact</code> - This Token Rule will exactly match the string provided.
* <code>TokenMatch</code> - This Token Rule will match to the regex provided.
* <code>SplitMatch</code> - This Token Rule will split the input at the regex provided, limiting future matches.
* <code>SplitExact</code> - This Token Rule will split the input at the exact string provided, limiting future matches.
* <code>StoreExtra</code> - This Token Rule will store the unmatched data parts that match the regex provided as tokens with this type.
* <code>StoreOther</code> - This Token Rule will store the unmatched data parts as tokens with this type.
* <code>TokenExtract</code> - This Token Rule will store the unmatched data parts as tokens with this type.
* <code>ErrorMatch</code> - This Token Rule will fail the assembly if matched.
### These specify the type of assembly for the Token Assembler.
* <code>BuildProperty</code> - This Token Group Rule will assemble a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenProperty\<T\></font></b></code>.
* <code>BuildArray</code> - This Token Group Rule will assemble a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenArray\<T\></font></b></code>.
* <code>BuildObject</code> - This Token Group Rule will assemble a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenObject\<T\></font></b></code>.
* <code>BuildFlag</code> - This Token Group Rule will assemble a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenFlag\<T\></font></b></code>.
* <code>BuildTypedValue</code> - This Token Group Rule will assemble a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenTypedValue\<T\></font></b></code>.<br/>
### These specify the type of assembly each ChkToken in a Token Assembly string.
* <code>AssignValue</code> - This Token Group Token Code will store the value as the 'Value' in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenProperty\<T\></font></b></code> or <code><b><font name="cascadia code" size=3 color=#5588FF>TokenArray\<T\></font></b></code>.
* <code>AssignName</code> - This Token Group Token Code will store the value as the 'Name' in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenProperty\<T\></font></b></code> or <code><b><font name="cascadia code" size=3 color=#5588FF>TokenObject\<T\></font></b></code>.
* <code>AssignType</code> - This Token Group Token Code will store the value as the 'Type' in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenObject\<T\></font></b></code>.
* <code>AddProperty</code> - This Token Group Token Code will store the value as a 'Property' in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenObject\<T\></font></b></code>.
* <code>AddFlag</code> - This Token Group Token Code will set AddFlag to <font name="cascadia code" size=3 color=#2233FF>true</font> in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenFlag\<T\></font></b></code>.
* <code>RemFlag</code> - This Token Group Token Code will set AddFlag to <font color="#2233FF">false</font> in a <code><b><font name="cascadia code" size=3 color=#5588FF>TokenFlag\<T\></font></b></code>.<br/>
### These are flags and modifiers.
* <code>FromTokens</code> - This Token Rule will only match from existing tokens.
* <code>ExemptAllWithin</code> - This Token Rule will exempt all matches from being checked.
* <code>IgnoredToken</code> - Flags the created token as ignored.
* <code>IgnoreCase</code> - Exact matches and regex will ignore case.
* <code>Competitive</code> - All Token Match Rules with this flag will run concurrently and exclusively.
* <code>Recursive</code> - The rule will execute until no matches occur.
* <code>Opt</code> - This token sequence entry is not required, but will be consumed if present.
* <code>Mult</code> - This token sequence entry can have additional entries, and will consume them if present.
### This is the mask to remove all of the flags to get the type.
* <code>FlagBits</code> = <code>Mult | Opt | Recursive | Competitive | IgnoreCase | IgnoredToken | ExemptAllWithin | FromTokens</code>


## Token Assembly Syntax

    (1) prefix:content
    (2) prefix:(type1-type2-type3)
    (3) prefix:(type1|type2|type3)
    (4) prefix:token_type
    (5) prefix:token_type{content}

No spaces (or tabs, or line feeds) can be between any single token assembly structure.
The hyphens in the 2nd example can be interchanged with '&', '|', or '+', like in the 3rd example. <br><br>
If you classify many tokens specifically, but need a general token to represent multiple other possibiliies, be sure to define <code>TokenCompatLookup</code> in your Spec to make your rule strings more clear.

## Prefix Letters

Some letters are optional. You must have at least 2 letters to make a valid definition. You can have a maximum of 5, both the start, the end, and all 3 options.<br><br>
Your prefix must contain one of these letters.

    t - Value is Token Type (see syntax line 2,3,4)
    c - Value is String Literal (see syntax line 1)
    b - Value contains Both (see syntax line 5)

They specify what the value is, so they are important.
Your prefix can have one of each of these:

    i - Ignore Case (String Literal Only)
    m - One or many, this token will repeat as long as it can, Possessive, Greedy.
    o - Optional, this token does not trigger a fail if it does not match. Greedy.

If 'm' and 'o' are both specified, it acts as the '*' operator, meaning zero or many, but it stays Greedy.
Defining 'm' alone or 'im' makes it Possessive, meaning it will not give any matches back, to attempt to find a more suitable match.
It will simply fail the match entirely, like an atomic group.

Must have only one of these:

    x - Ignore Token
    n - Token is 'Name' in object, property, or label
    y - Token is 'Type' in object or typedvalue
    v - Token is 'Value' in array, property, or typedvalue
    p - Token is 'Property' in object
    f - Token is 'Name' in flag and AddFlag is true.
    r - Token is 'Name' in flag and AddFlag is false.

### Example

    tv:typename

    * t : typename is the token type
    * v : it is stored in the value field

    cvi:null

    * t : null is the string literal
    * v : it is stored in the value field
    * i : it is not case sensitive
    
    tpm:Property

    * t : Property is the token type
    * p : it is stored in the property field
    * m : it will consume as many properties as it can find, giving nothing back

    bn:Keyword{\bscript\b}

    * b : Keyword is the token type, and \bscript\b is the regex string to match
    * n : it is stored in the name field