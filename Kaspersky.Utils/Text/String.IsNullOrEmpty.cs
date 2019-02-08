using System;

namespace Kaspersky.Utils.Text
{
    public static partial class StringExtensions
    {
        public static bool IsNullOrEmpty( this string value ) => String.IsNullOrEmpty( value );
    }
}
