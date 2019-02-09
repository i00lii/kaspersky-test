using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kaspersky.Utils.Text;
using NUnit.Framework;

namespace Kaspersky.Tests
{
    [TestFixture]
    public sealed class TokenizeTest
    {
        [TestCaseSource( typeof( TokenizerTestCases ) )]
        public void TokenSequenceIsParsed( string value )
        {
            var expected = value
               .Trim()
               .Split( new[] { " ", '\u00a0'.ToString() }, StringSplitOptions.RemoveEmptyEntries )
               .ToArray();

            var result = Tokenizer
               .EnumerateTokens( value )
               .Select( token => token.Value.Substring( token.Offset.Index, token.Offset.Count ) )
               .ToArray();

            result.Should().ContainInOrder( expected );
        }

        [TestCase( default( string ) )]
        [TestCase( "" )]
        [TestCase( "     " )]
        public void TokenSequenceIsEmptyWhenTextContainsNoTokens( string value )
           => Tokenizer
           .EnumerateTokens( value )
           .ToArray()
           .Should()
           .HaveCount( 0 );

        private sealed class TokenizerTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( "Single space between tokens." );
                yield return new TestCaseData( "Multiply spaces between     tokens." );
                yield return new TestCaseData( "  Spaces before token sequence." );
                yield return new TestCaseData( "Spaces after token sequence.   " );
                yield return new TestCaseData( "Toooooooooken" );
                yield return new TestCaseData( "Magical" + '\u00a0' + "whitespace." );
                yield return new TestCaseData( "x" );
                yield return new TestCaseData( " y" );
                yield return new TestCaseData( "z " );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
