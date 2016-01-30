using System;

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
                throw new UndefinedVariableException("Attempt to reference unnamed variable");

            if (!ctx.Variables.ContainsKey(VariableName))
                throw new UndefinedVariableException("Undefined variable '" + VariableName + "' referenced");

            return ctx.Variables[VariableName];
        }

        public override string Literal { get { return VariableName; } }
    }

    public class UndefinedVariableException : Exception
    {
        public UndefinedVariableException()
        {
        }

        public UndefinedVariableException(string message)
            : base(message)
        { }

        public UndefinedVariableException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
