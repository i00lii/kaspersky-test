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
    public sealed class AuthorValidationTest
    {
        private Author.Validator _validator;

        [SetUp]
        public void SetUp() => _validator = new Author.Validator();

        [TestCaseSource( typeof( ValidAuthorTestCases ) )]
        public void NoValidationErrorsFoundWhenAuthorIsValid( Author item ) => _validator.Validate( item ).IsValid.Should().BeTrue();


        [TestCaseSource( typeof( InvalidSurnameTestCaseses ) )]
        public void ValidationErrorsFoundWhenSurnameIsInvalid( Author item ) => _validator.ShouldHaveValidationErrorFor( x => x.Surname, item );

        [TestCaseSource( typeof( InvalidNameTestCaseses ) )]
        public void ValidationErrorsFoundWhenNameIsInvalid( Author item ) => _validator.ShouldHaveValidationErrorFor( x => x.Name, item );

        private sealed class ValidAuthorTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Author() { Name = "Карл", Surname = "Маркс" } );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidSurnameTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Author() { Surname = default } ).SetName( "Author.Surname is null" );
                yield return new TestCaseData( new Author() { Surname = String.Empty } ).SetName( "Author.Surname is empty" );
                yield return new TestCaseData( new Author() { Surname = new string( '0', 21 ) } ).SetName( "Author.Surname is longer than 20 chars" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidNameTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Author() { Name = default } ).SetName( "Author.Name is null" );
                yield return new TestCaseData( new Author() { Name = String.Empty } ).SetName( "Author.Name is empty" );
                yield return new TestCaseData( new Author() { Name = new string( '0', 21 ) } ).SetName( "Author.Name is longer than 20 chars" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
