using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression.BinTree
{
    public abstract class OperandNode : ExpressionNode { }

    public class ValueNode : OperandNode
    {
        public ValueNode(double value)
        {
            Value = value;
        }

        public double Value { get; private set; }

        public override double Evaluate(OperandContext ctx)
        {
            return Value;
        }

        public override string Literal { get { return Value.ToString(); } }
    }

    public class VariableNode : OperandNode
    {
        public VariableNode(string variableName)
        {
            VariableName = variableName;
        }

        public string VariableName
        {
            get;
            set;
        }

        public override double Evaluate(OperandContext ctx)
        {
            if (string.IsNullOrEmpty(VariableName))
                throw new Exception("Attempt to reference unnamed variable");

            if (!ctx.Variables.ContainsKey(VariableName))
                throw new Exception("Undefined variable '" + VariableName + "' referenced");

            return ctx.Variables[VariableName];
        }

        public override string Literal { get { return VariableName; } }
    }
}
