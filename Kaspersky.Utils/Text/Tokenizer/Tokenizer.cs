using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaspersky.Utils.Text
{
    public static class Tokenizer
    {
        public static IEnumerable<Token> EnumerateTokens( string value )
        {
            return value.IsNullOrEmpty() ? Enumerable.Empty<Token>() : EnumerateTokens();

            IEnumerable<Token> EnumerateTokens()
            {
                var begin = FindTokenStart( 0 );

                while (begin < value.Length)
                {
                    var end = FindTokenEnd( CharAfter( begin ) );
                    yield return new Token( value, new Offset( begin, end - begin ) );

                    begin = FindTokenStart( CharAfter( end ) );
                }
            }

            int FindTokenStart( int from ) => FindNext( from, ch => !Char.IsWhiteSpace( ch ) );

            int FindTokenEnd( int from ) => FindNext( from, ch => Char.IsWhiteSpace( ch ) );

            int CharAfter( int idx ) => idx + 1;

            int FindNext( int from, Func<char, bool> predicate )
            {
                for (var idx = from; idx < value.Length; idx++)
                {
                    if (predicate( value[idx] ))
                        return idx;
                }

                return value.Length;
            }
        }
    }
}
