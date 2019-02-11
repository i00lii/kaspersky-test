using System;
using System.Collections.Generic;

namespace Kaspersky.Utils.Rpn.Operations
{
    internal sealed class BinaryOperation<TLogic> : IOperation
       where TLogic : struct, IBinaryOperationLogic
    {
        public void Apply( Stack<Operand> locals )
        {
            var y = PopValue();
            var x = PopValue();

            locals.Push( new Operand( default( TLogic ).Apply( x, y ) ) );

            double PopValue() => locals.Pop().Value;
        }
    }

    internal interface IBinaryOperationLogic
    {
        double Apply( double x, double y );
    }

    internal readonly struct Add : IBinaryOperationLogic
    {
        public double Apply( double x, double y ) => x + y;
    }

    internal readonly struct Substruct : IBinaryOperationLogic
    {
        public double Apply( double x, double y ) => x - y;
    }

    internal readonly struct Multiply : IBinaryOperationLogic
    {
        public double Apply( double x, double y ) => x * y;
    }

    internal readonly struct Divide : IBinaryOperationLogic
    {
        public double Apply( double x, double y ) => x / (y != 0d ? y : throw new DivideByZeroException());
    }

    internal readonly struct PowerOf : IBinaryOperationLogic
    {
        public double Apply( double x, double y ) => Math.Pow( x, y );
    }
}
