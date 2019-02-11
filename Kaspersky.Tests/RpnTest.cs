using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Kaspersky.Utils.Rpn;
using NUnit.Framework;

namespace Kaspersky.Tests
{
    [TestFixture]
    public sealed class RpnTest
    {
        [TestCaseSource( typeof( RpnPositiveTestCases ) )]
        public void ValueIsCalculatedRight( string value, double expected ) => RpnCalculator.Run( value ).Should().Be( expected );

        [TestCaseSource( typeof( RpnNegativeTestCases ) )]
        public void ExceptionIsThrownWhenTokenSequenceIsCorrupted( string value )
        {
            Action action = () => RpnCalculator.Run( value );
            action.Should().Throw<Exception>();
        }

        private sealed class RpnPositiveTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( "10 4 2 / -", 8 );
                yield return new TestCaseData( "1 2 + 4 * 3 +", 15 );
                yield return new TestCaseData( $"1 -{2.5} +", -1.5 );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class RpnNegativeTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( "42 is more than just number!" );
                yield return new TestCaseData( "42 43 44 +" );
                yield return new TestCaseData( "42 * *" );
                yield return new TestCaseData( "4 0 /" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
