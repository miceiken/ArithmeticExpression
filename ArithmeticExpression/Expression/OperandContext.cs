using System;
using System.Collections.Generic;

namespace ArithmeticExpression.Expression
{
    public class OperandContext
    {
        public Dictionary<string, double> Variables { get; private set; } = new Dictionary<string, double>()
        {
            ["Pi"] = Math.PI,
            ["e"] = Math.E,
        };
    }
}
