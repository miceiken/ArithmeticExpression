using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression.BinTree
{
    public class OperatorNode : ExpressionNode
    {
        public static readonly Dictionary<Operators, Precedence> OperatorPrecedence = new Dictionary<Operators, Precedence>()
        {
            [Operators.Multiply] = Precedence.Multiplicative,
            [Operators.Divide] = Precedence.Multiplicative,
            [Operators.Exponent] = Precedence.Multiplicative,
            [Operators.Modulus] = Precedence.Multiplicative,
            [Operators.Add] = Precedence.Additive,
            [Operators.Subtract] = Precedence.Additive,
            [Operators.Define] = Precedence.Assignment,
        };

        public OperatorNode(Operators oper)
        {
            Operator = oper;
        }

        public Operators Operator { get; private set; }
        public Precedence Precedence { get { return OperatorPrecedence[Operator]; } }

        public override bool Consume(OperandContext ctx)
        {
            if (Operator == Operators.Define)
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
            return Algebra.Evaluators[Operator](Left.Evaluate(ctx), Right.Evaluate(ctx));
        }

        public override string Literal { get { return string.Format("{0} ({1})", Operator, Precedence); } }
    }
}
