using System;
using System.Collections.Generic;

namespace Kaspersky.Utils.Text
{
    public static partial class StringExtensions
    {
        private const int _isbn10Size = 10;
        private const int _isbn13Size = 13;
        private const int _zero = '0';
        private const char _hyphen = '-';
        private const char _x = 'X';

        public static bool IsValidIsbnString( this string value )
        {
            return value.IsNullOrEmpty() || !(TryNormalize() is var normalizedIsbn) ? default
               : LooksLikeOldGoodIsbn() ? ValidateChecksum<Isbn10ChecksumValidator>()
               : LooksLikeModernIsbn() ? ValidateChecksum<Isbn13ChecksumValidator>()
               : default;

            bool LooksLikeOldGoodIsbn()
                => normalizedIsbn.Count == _isbn10Size
                && (normalizedIsbn.Groups.Count == 1 || normalizedIsbn.Groups.Count == 4);

            bool LooksLikeModernIsbn()
                => normalizedIsbn.Count == _isbn13Size
                && (normalizedIsbn.Groups.Count == 1 || normalizedIsbn.Groups.Count == 5)
                && StartsWith978or979();

            bool StartsWith978or979()
            {
                var group = normalizedIsbn.Groups[0];
                var prefix = group.Count >= 3 ? Int32.Parse( value.AsSpan( group.Index, 3 ) ) : default;
                return prefix <= 979 && prefix >= 978;
            }

            bool ValidateChecksum<TChecksumValidator>()
               where TChecksumValidator : struct, IIsbnChecksumValidator
               => default( TChecksumValidator ).IsValidChecksum( FlattenGroups() );

            IEnumerable<int> FlattenGroups()
            {
                foreach (var offset in normalizedIsbn.Groups)
                {
                    var end = offset.Index + offset.Count;
                    for (var idx = offset.Index; idx < end; idx++)
                    {
                        var ch = value[idx];
                        yield return ch == _x ? 10 : (ch - _zero);
                    }
                }
            }

            (int Count, IReadOnlyList<Offset> Groups) TryNormalize()
            {
                var groups = new List<Offset>( 5 );

                var begin = 0;
                var count = 0;
                var groupFound = false;

                for (var idx = 0; idx < value.Length; idx++)
                {
                    var currentChar = value[idx];

                    if (IsIsbnDigit())
                    {
                        count++;

                        if (!groupFound)
                        {
                            groupFound = true;
                            begin = idx;
                        }

                        if (IsLastChar())
                        {
                            idx++;
                            Flush();
                            break;
                        }
                    }
                    else
                    {
                        if (!groupFound)
                            return default;

                        if (Char.IsWhiteSpace( currentChar ) || currentChar == _hyphen)
                        {
                            Flush();
                            groupFound = false;
                        }
                    }

                    bool IsIsbnDigit()
                        => Char.IsDigit( currentChar )
                        || idx == value.Length - 1
                        && currentChar == _x;

                    bool IsLastChar()
                        => idx == value.Length - 1;

                    void Flush()
                        => groups.Add( new Offset( begin, idx - begin ) );
                }

                return (count, groups);
            }
        }

        private interface IIsbnChecksumValidator
        {
            bool IsValidChecksum( IEnumerable<int> isbnGroups );
        }

        private readonly struct Isbn10ChecksumValidator : IIsbnChecksumValidator
        {
            public bool IsValidChecksum( IEnumerable<int> isbnDigits )
            {
                const int module = 11;

                var sum = 0;
                var multiplier = 10;

                foreach (var digit in isbnDigits)
                {
                    sum += digit * multiplier--;
                }

                return sum % module == 0;
            }
        }

        private readonly struct Isbn13ChecksumValidator : IIsbnChecksumValidator
        {
            public bool IsValidChecksum( IEnumerable<int> isbnDigits )
            {
                const int module = 10;
                const int evenMultiplyer = 1;
                const int oddMultiplyer = 3;

                var sum = 0;
                var position = 0;

                foreach (var digit in isbnDigits)
                {
                    sum += digit * ((position++ % 2 == 0) ? evenMultiplyer : oddMultiplyer);
                }

                return sum % module == 0;
            }
        }
    }
}
