using System;

namespace Kaspersky.Utils.Rpn
{
    internal readonly struct Operand : IEquatable<Operand>
    {
        public double Value { get; }
        public Operand( double value ) => Value = value;

        public override bool Equals( object obj ) => obj is Operand && Equals( (Operand)obj );
        public bool Equals( Operand other ) => Value == other.Value;
        public override int GetHashCode() => -1937169414 + Value.GetHashCode();

        public static bool operator ==( Operand value1, Operand value2 ) => value1.Equals( value2 );
        public static bool operator !=( Operand value1, Operand value2 ) => !(value1 == value2);
    }
}
