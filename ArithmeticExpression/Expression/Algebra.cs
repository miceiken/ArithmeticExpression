using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
