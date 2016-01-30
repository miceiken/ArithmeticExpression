using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArithmeticExpression.Expression.BinTree;
using System.Diagnostics;

namespace ArithmeticExpression.Expression
{
    public class ExpressionTree
    {
        public ExpressionTree()
        { }

        public Stack<ExpressionNode> Expression { get; set; } = new Stack<ExpressionNode>();
        public OperandContext Context { get; set; } = new OperandContext();

        public bool IsComplete => Expression.Count == 1;
        public void Clear() => Expression.Clear();

        public ExpressionNode Build(IEnumerable<ExpressionNode> nodes)
        {
            Expression = new Stack<ExpressionNode>();

            foreach (var node in nodes)
            {
                if (node is OperatorNode)
                {
                    var right = Expression.Pop();
                    var left = Expression.Pop();

                    node.SetChildren(left, right);
                }

                if (!node.Consume(Context))
                    Expression.Push(node);
            }

            //Debug.Assert(IsComplete, "Incomplete expression, stack has more than 1 elements");

            return Expression.FirstOrDefault();
        }
    }
}
