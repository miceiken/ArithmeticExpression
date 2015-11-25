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
            Operand = new Operand();
            Operand.Variable = var;
        }

        public ExpressionNode(double number)
        {
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
    }
}
