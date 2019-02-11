using FluentAssertions;
using FluentValidation.TestHelper;
using Kaspersky.Api.Bookshelf.Models;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kaspersky.Tests.Validation
{
    [TestFixture]
    public sealed class ThumbnailValidationTest
    {
        private Thumbnail.Validator _validator;

        [SetUp]
        public void SetUp() => _validator = new Thumbnail.Validator();

        [TestCaseSource( typeof( VaildThumbnailTestCases ) )]
        public void NoValidationErrorsFoundWhenThumbnailIsValid( Thumbnail item ) => _validator.Validate( item ).IsValid.Should().BeTrue();

        [TestCaseSource( typeof( InvaildThumbnailTestCases ) )]
        public void VaildationErrorsFoundWhenDataIsInvalid( Thumbnail item ) => _validator.ShouldHaveValidationErrorFor( x => x.Data, item );

        private sealed class VaildThumbnailTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Thumbnail() { Data = new byte[1] { 42 } } );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvaildThumbnailTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Thumbnail() { Data = Array.Empty<byte>() } ).SetName( "Thumbnail.Data is null" );
                yield return new TestCaseData( new Thumbnail() { Data = null } ).SetName( "Thumbnail.Data is empty" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
