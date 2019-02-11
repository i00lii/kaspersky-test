using System;
using System.Collections.Generic;

namespace Kaspersky.Utils.Text
{
    public readonly struct Token : IEquatable<Token>
    {
        public string Value { get; }
        public Offset Offset { get; }

        public Token( string value, Offset offset )
        {
            Value = value;
            Offset = offset;
        }

        public override string ToString() => Value.Substring( Offset.Index, Offset.Count );

        public override bool Equals( object obj ) => obj is Token && Equals( (Token)obj );
        public bool Equals( Token other ) => Value == other.Value && Offset.Equals( other.Offset );

        public override int GetHashCode()
        {
            var hashCode = 371125220;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode( Value );
            hashCode = hashCode * -1521134295 + EqualityComparer<Offset>.Default.GetHashCode( Offset );
            return hashCode;
        }

        public static bool operator ==( Token token1, Token token2 ) => token1.Equals( token2 );
        public static bool operator !=( Token token1, Token token2 ) => !(token1 == token2);
    }
}
