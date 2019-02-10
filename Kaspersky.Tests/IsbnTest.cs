using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Kaspersky.Utils.Text;
using FluentAssertions;

namespace Kaspersky.Tests
{
    [TestFixture]
    public sealed class IsbnTest
    {
        [TestCaseSource( typeof( IsbnPositiveTestCases ) )]
        public void ValidIsbnIsRecognizedCorrectly( string value )
           => value
           .IsValidIsbnString()
           .Should()
           .BeTrue();

        [TestCaseSource( typeof( IsbnNegativeTestCases ) )]
        public void CorruptedIsbnIsRecognizedCorrectly( string value )
           => value
           .IsValidIsbnString()
           .Should()
           .BeFalse();

        private sealed class IsbnPositiveTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( "99921-58-10-7" );
                yield return new TestCaseData( "0-943396-04-2" );
                yield return new TestCaseData( "0-9752298-0-X" );

                yield return new TestCaseData( "978-3-16-148410-0" );
                yield return new TestCaseData( "978-1-4028-9462-6" );

                yield return new TestCaseData( "9783161484100" );

                yield return new TestCaseData( "3598215088" );
                yield return new TestCaseData( "359821507X" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class IsbnNegativeTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( "3-598-21508-9" );
                yield return new TestCaseData( "3-598-21507-A" );
                yield return new TestCaseData( "3-598-P1581-X" );
                yield return new TestCaseData( "3-598-2X507-9" );
                yield return new TestCaseData( "359821507" );
                yield return new TestCaseData( "3598215078X" );
                yield return new TestCaseData( "00" );
                yield return new TestCaseData( "3-598-21507" );
                yield return new TestCaseData( "3-598-21515-X" );
                yield return new TestCaseData( "134456729" );
                yield return new TestCaseData( "3132P34035" );
                yield return new TestCaseData( "98245726788" );
                yield return new TestCaseData( "98245726788" );
                yield return new TestCaseData( "978-1-4-0-28-94-62-6" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
