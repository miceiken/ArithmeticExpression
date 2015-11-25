using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression
{
    public class Operand
    {
        public string Variable { get; set; }
        public double Number { get; set; }

        public double GetValue(OperandContext ctx)
        {
            if (!string.IsNullOrEmpty(Variable))
            {
                if (!ctx.Variables.ContainsKey(Variable))
                    throw new Exception("Undefined variable '" + Variable + "' referenced");
                return ctx.Variables[Variable];
            }
            return Number; // double is a struct so it will default to 0.0 if not set
        }
    }
}
