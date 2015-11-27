using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Expression.BinTree
{
    public abstract class ExpressionNode
    {
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        public void SetChildren(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public virtual bool Consume(OperandContext ctx) { return false; }

        public abstract double Evaluate(OperandContext ctx);
        public abstract string Literal { get; }

        public override string ToString()
        {
            return Literal;
        }
    }
}
