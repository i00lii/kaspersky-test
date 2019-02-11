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
    public sealed class BookValidationTest
    {
        private Book.Validator _validator;

        [SetUp]
        public void SetUp() => _validator = new Book.Validator();

        [TestCaseSource( typeof( ValidBookTestCases ) )]
        public void NoValidationErrorsFoundWhenBookIsValid( Book item ) => _validator.Validate( item ).IsValid.Should().BeTrue();

        [TestCaseSource( typeof( InvalidTitleTestCaseses ) )]
        public void ValidationErrorsFoundWhenTitleIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.Title, item );

        [TestCaseSource( typeof( InvalidPublisherTestCaseses ) )]
        public void ValidationErrorsFoundWhenPublisherIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.Publisher, item );

        [TestCaseSource( typeof( InvalidIsbnTestCaseses ) )]
        public void ValidationErrorsFoundWhenIsbnIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.Isbn, item );

        [TestCaseSource( typeof( InvalidPagesTotalTestCaseses ) )]
        public void ValidationErrorsFoundWhenPagesTotalIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.PagesTotal, item );

        [TestCaseSource( typeof( InvalidPublicationYearTestCaseses ) )]
        public void ValidationErrorsFoundWhenPublicationYearIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.PublicationYear, item );

        [TestCaseSource( typeof( InvalidAuthorListTestCaseses ) )]
        public void ValidationErrorsFoundWhenAuthorListIsInvalid( Book item ) => _validator.ShouldHaveValidationErrorFor( x => x.Authors, item );

        private sealed class ValidBookTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData
                (
                    new Book()
                    {
                        Title = "IT",
                        Publisher = "Viking",
                        Isbn = "0-670-81302-8",
                        PagesTotal = 1138,
                        PublicationYear = 1986,
                        Authors = new[]
                        {
                            new Author()
                            {
                                Name = "Stephen",
                                Surname = "King"
                            }
                        }
                    }
                );

                yield return new TestCaseData
                (
                    new Book()
                    {
                        Title = "The Little Sisters of Eluria",
                        Isbn = "0-670-81302-8",
                        PagesTotal = 1000,
                        PublicationYear = 1800,
                        Authors = new[]
                        {
                            new Author()
                            {
                                Name = "Stephen",
                                Surname = "King"
                            }
                        }
                    }
                ).SetName( "Publiser is null" );

                yield return new TestCaseData
                (
                    new Book()
                    {
                        Title = "Twinkle twinke litte star",
                        Isbn = "0-670-81302-8",
                        Publisher = "Viking",
                        PagesTotal = 1000,
                        Authors = new[]
                        {
                            new Author()
                            {
                                Name = "Stephen",
                                Surname = "King"
                            }
                        }
                    }
                ).SetName( "Publication year is null" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidTitleTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { Title = default } ).SetName( "Book.Title is null" );
                yield return new TestCaseData( new Book() { Title = String.Empty } ).SetName( "Book.Title is empty" );
                yield return new TestCaseData( new Book() { Title = new string( '0', 31 ) } ).SetName( "Book.Title is longer than 30 chars" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidPublisherTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { Publisher = String.Empty } ).SetName( "Book.Publisher is empty" );
                yield return new TestCaseData( new Book() { Publisher = new string( '0', 31 ) } ).SetName( "Book.Publisher is longer than 30 chars" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidIsbnTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { Isbn = default } ).SetName( "Book.Isbn is null" );
                yield return new TestCaseData( new Book() { Isbn = String.Empty } ).SetName( "Book.Isbn is empty" );
                yield return new TestCaseData( new Book() { Isbn = "ISBN" } ).SetName( "Book.Isbn is invalid" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidPagesTotalTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { PagesTotal = default } ).SetName( "Book.PagesTotal is 0" );
                yield return new TestCaseData( new Book() { PagesTotal = 10001 } ).SetName( "Book.PagesTotal is more than 10000" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidPublicationYearTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { PublicationYear = 1799 } ).SetName( "Book.PublicationYear is less than 1800" );
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class InvalidAuthorListTestCaseses : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData( new Book() { Authors = default } ).SetName( "Book.Authors is null" );
                yield return new TestCaseData( new Book() { Authors = Array.Empty<Author>() } ).SetName( "Book.Authors is empty" );
                yield return new TestCaseData( new Book() { Authors = new Author[] { null } } ).SetName( "Book.Authors contains null inside" );

                ///The case with invalid author inside author list was here... 
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


    }
}
