using System;
using System.Collections.Generic;
using Kaspersky.Utils.Rpn.Operations;
using Kaspersky.Utils.Text;

namespace Kaspersky.Utils.Rpn
{
    public static class RpnCalculator
    {
        private static readonly Dictionary<char, IOperation> _operationMap
           = new Dictionary<char, IOperation>()
           {
               ['+'] = new BinaryOperation<Add>(),
               ['-'] = new BinaryOperation<Substruct>(),
               ['*'] = new BinaryOperation<Multiply>(),
               ['/'] = new BinaryOperation<Divide>(),
               ['^'] = new BinaryOperation<PowerOf>()
           };

        public static unsafe double Run( string value )
        {
            var locals = new Stack<Operand>();

            foreach (var token in Tokenizer.EnumerateTokens( value ))
            {
                var span = token.Value.AsSpan( token.Offset.Index, token.Offset.Count );

                if (token.Offset.Count == 1 && _operationMap.TryGetValue( span[0], out var operation ))
                {
                    operation.Apply( locals );
                }
                else if (Double.TryParse( span, out var numeric ))
                {
                    locals.Push( new Operand( numeric ) );
                }
                else
                {
                    throw new InvalidExpressionFormatExteption( token.Value );
                }
            }

            return locals.Count == 1 ? locals.Pop().Value : throw new InvalidExpressionFormatExteption( value );
        }

        private sealed class InvalidExpressionFormatExteption : Exception
        {
            private readonly string _expression;
            public InvalidExpressionFormatExteption( string expression ) => _expression = expression;

            public override string Message => $"Looks like '{_expression}' is not valid PRN expression.";
        }
    }
}
