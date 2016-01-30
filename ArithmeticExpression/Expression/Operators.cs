using System;

namespace ArithmeticExpression.Expression
{
    [Flags]
    public enum AssignmentOperators
    {
        Define
    }

    [Flags]
    public enum UnaryOperators
    {
        Plus,
        Minus,
        Increment,
        Decrement,
        Invert
    }

    [Flags]
    public enum ArithmeticOperators
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
