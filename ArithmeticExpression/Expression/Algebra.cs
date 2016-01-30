using System;
using System.Collections.Generic;

namespace ArithmeticExpression.Expression
{
    public static class Algebra
    {
        public static readonly Dictionary<ArithmeticOperators, Func<double, double, double>> ArithmeticEvaluators
            = new Dictionary<ArithmeticOperators, Func<double, double, double>>()
            {
                [ArithmeticOperators.Multiply] = (l, r) => l * r,
                [ArithmeticOperators.Divide] = (l, r) => l / r,
                [ArithmeticOperators.Add] = (l, r) => l + r,
                [ArithmeticOperators.Subtract] = (l, r) => l - r,
                [ArithmeticOperators.Exponent] = (l, r) => Math.Pow(l, r),
                [ArithmeticOperators.Modulus] = (l, r) => l % r,

                [ArithmeticOperators.Define] = (l, r) => r,
            };

        public static readonly Dictionary<ArithmeticOperators, Precedence> OperatorPrecedence = new Dictionary<ArithmeticOperators, Precedence>()
        {
            [ArithmeticOperators.Multiply] = Precedence.Multiplicative,
            [ArithmeticOperators.Divide] = Precedence.Multiplicative,
            [ArithmeticOperators.Exponent] = Precedence.Multiplicative,
            [ArithmeticOperators.Modulus] = Precedence.Multiplicative,
            [ArithmeticOperators.Add] = Precedence.Additive,
            [ArithmeticOperators.Subtract] = Precedence.Additive,
            [ArithmeticOperators.Define] = Precedence.Assignment,
        };

        public static readonly Dictionary<ArithmeticOperators, char> OperatorLiterals = new Dictionary<ArithmeticOperators, char>()
        {
            [ArithmeticOperators.Multiply] = '*',
            [ArithmeticOperators.Divide] = '/',
            [ArithmeticOperators.Exponent] = '^',
            [ArithmeticOperators.Modulus] = '%',
            [ArithmeticOperators.Add] = '+',
            [ArithmeticOperators.Subtract] = '-',
            [ArithmeticOperators.Define] = '=',
        };
    }
}
