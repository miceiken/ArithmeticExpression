using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression.BinTree
{
    public class OperatorNode : ExpressionNode
    {
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

        public OperatorNode(ArithmeticOperators oper)
        {
            Operator = oper;
        }

        public ArithmeticOperators Operator { get; private set; }
        public Precedence Precedence { get { return OperatorPrecedence[Operator]; } }

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
