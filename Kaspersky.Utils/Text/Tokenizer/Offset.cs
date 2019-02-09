using System;

namespace Kaspersky.Utils.Text
{
    public readonly struct Offset : IEquatable<Offset>
    {
        public int Index { get; }
        public int Count { get; }

        public Offset( int index, int count )
        {
            Index = index;
            Count = count;
        }

        public override bool Equals( object obj ) => obj is Offset && Equals( (Offset)obj );
        public bool Equals( Offset other ) => Index == other.Index && Count == other.Count;

        public override int GetHashCode()
        {
            int hashCode = 173447405;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }

        public static bool operator ==( Offset slice1, Offset slice2 ) => slice1.Equals( slice2 );
        public static bool operator !=( Offset slice1, Offset slice2 ) => !(slice1 == slice2);
    }
}
