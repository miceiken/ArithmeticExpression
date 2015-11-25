using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression.BinTree
{
    public class ExpressionNode
    {
        public ExpressionNode() { }

        public ExpressionNode(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public ExpressionNode(string var)
        {
            Operator = Operators.Operand;

            Operand = new Operand();
            Operand.Variable = var;
        }

        public ExpressionNode(double number)
        {
            Operator = Operators.Operand;

            Operand = new Operand();
            Operand.Number = number;
        }

        public ExpressionNode(Operators op)
        {
            Operator = op;
        }

        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        public Operators Operator { get; set; }
        public Operand Operand { get; private set; }

        public double GetEvaluated(OperandContext ctx)
        {
            if (Operator == Operators.Operand)
                return Operand.GetValue(ctx);
            return Algebra.Evaluators[Operator](Left.GetEvaluated(ctx), Right.GetEvaluated(ctx));
        }

        public string GetLiteralLabel()
        {
            if (Operator == Operators.Operand)
                return Operand.Variable ?? Operand.Number.ToString();
            return Operator.ToString();
        }

        public string GetEvaluatedLabel(OperandContext ctx)
        {
            if (Operator == Operators.Define)
                return string.Format("{0} = {1}", Left.Operand.Variable, Right.Operand.Number);
            return GetEvaluated(ctx).ToString();
        }
    }
}
