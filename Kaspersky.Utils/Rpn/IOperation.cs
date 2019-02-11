using System.Collections.Generic;

namespace Kaspersky.Utils.Rpn
{
    internal interface IOperation
    {
        void Apply( Stack<Operand> locals );
    }
}
