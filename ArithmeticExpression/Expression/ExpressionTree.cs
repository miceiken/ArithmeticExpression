using ArithmeticExpression.Expression.BinTree;
using System.Collections.Generic;
using System.Linq;

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

        public void Build(IEnumerable<ExpressionNode> nodes)
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
        }

        public IEnumerable<double> GetEvaluated() => Expression.Select(n => n.Evaluate(Context));

        public string InfixExpression => string.Join(" ", Expression.Select(n => GetLiteral(n, true)));

        public string GetLiteral(ExpressionNode node, bool useContext)
        {
            var ret = string.Empty;
            if (node is OperatorNode)
                ret += "(";
            if (node.Left != null)
                ret += GetLiteral(node.Left, useContext);
            if (node is OperatorNode)
                ret += Algebra.OperatorLiterals[((OperatorNode)node).Operator];
            if (node is OperandNode)
            {
                if (node is ValueNode)
                    ret += ((ValueNode)node).Value.ToString();
                if (node is VariableNode)
                    ret += useContext ? ((VariableNode)node).Evaluate(Context).ToString() : ((VariableNode)node).VariableName;
            }
            if (node.Right != null)
                ret += GetLiteral(node.Right, useContext);
            if (node is OperatorNode)
                ret += ")";
            return ret;
        }
    }
}
