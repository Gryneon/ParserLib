



# Recommended Names

Recommended standard names for various elements in the parser, including data keys, parser tokens, and other identifiers. These are not enforced, but are recommended to maintain consistency and clarity in your code.

## Data Keys

These are the naming conventions for data keys.

### Predefined

    initial   - The initial file text as a string.
    result    - The end result

### Textual Names

    text      - Working block of text, as a string.
    textparts - Working segments of text, as a Collection<string>.
    lines     - Working lines of text, as a Collection<string>.

### Data Names

    matches   - Results of DictionaryOperation, as a Collection<MatchData>.
    tokens    - Results of TokenizeOperation, as a Collection<IToken>.







## Parser Tokens

These are the recommended naming conventions for parser tokens.

### Ignored Content

    ws         - Use for whitespace (can include line endings).
    ln         - Use for line endings (crlf, lf, cr).
    blkcomment - Use for multiline or block comments.
    lncomment  - Use for single line comments.

### Data Content

    int         - Use for integers.
    dec         - Use for floating point, fixed point, and decimal numbers.
    char        - Use for single characters, or character codes.
    bool        - Use for logical boolean values (typically true or false)
    str[ing|qt] - Use for double or single quoted strings.
    key         - Use for property names, or keys from other key/value pairs.
    value       - Use for property values, or values from other key/value pairs.
    name        - Use for non-quoted names, or other identifiers

### Operators and Delimiters

    qt  - Use for quotes, unless you need to be more specific.
    dqt - Use for double quotes if you need to.
    sqt - Use for single quotes if you need to.
    op  - Use for operators.