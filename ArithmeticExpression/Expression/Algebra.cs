using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression
{
    public static class Algebra
    {
        public static readonly Dictionary<char, Operators> CharOperators
            = new Dictionary<char, Operators>()
            {
                ['*'] = Operators.Multiply,
                ['/'] = Operators.Divide,
                ['+'] = Operators.Add,
                ['-'] = Operators.Subtract,
                ['^'] = Operators.Exponent,
                //['-'] = Operators.Negate,
                ['='] = Operators.Define
            };

        public static readonly Dictionary<Operators, Func<double, double, double>> Evaluators
            = new Dictionary<Operators, Func<double, double, double>>()
            {
                [Operators.Multiply] = (l, r) => l * r,
                [Operators.Divide] = (l, r) => l / r,
                [Operators.Add] = (l, r) => l + r,
                [Operators.Subtract] = (l, r) => l - r,
                [Operators.Exponent] = (l, r) => Math.Pow(l, r),
                [Operators.Define] = (l, r) => r,
            };
    }
}
