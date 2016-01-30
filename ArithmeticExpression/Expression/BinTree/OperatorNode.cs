using System;

namespace ArithmeticExpression.Expression.BinTree
{
    public class OperatorNode : ExpressionNode
    {
        public OperatorNode(ArithmeticOperators oper)
        {
            Operator = oper;
        }

        public ArithmeticOperators Operator { get; private set; }
        public Precedence Precedence { get { return Algebra.OperatorPrecedence[Operator]; } }

        public override bool Consume(OperandContext ctx)
        {
            if (Operator == ArithmeticOperators.Define)
            {
                ctx.Variables[((VariableNode)Left).VariableName] = Right.Evaluate(ctx);
                return true;
            }
            return false;
        }

        public override double Evaluate(OperandContext ctx)
        {
            if (Left == null || (Right == null && Precedence != Precedence.Unary))
                throw new Exception("Trying to evaluate operator withouth any operands");
            return Algebra.ArithmeticEvaluators[Operator](Left.Evaluate(ctx), Right.Evaluate(ctx));
        }

        public override string Literal { get { return string.Format("{0} ({1})", Operator, Precedence); } }
    }
}
