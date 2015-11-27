using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression
{
    [Flags]
    public enum Operators
    {
        Multiply,
        Divide,
        Add,
        Subtract,
        Exponent,
        Modulus,

        Define
    }

    public enum Precedence : ushort
    {
        Primary = 0,
        Unary,
        Multiplicative,
        Additive,
        Shift,
        Relational,
        Equality,
        Logical, // AND XOR OR
        Conditional, // AND OR
        Assignment
    }
}
