using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArithmeticExpression.Expression.BinTree;

namespace ArithmeticExpression.Expression
{
    public class Expression
    {
        public Expression()
        { }

        public List<ExpressionNode> TreeStack { get; private set; } = new List<ExpressionNode>();
        public OperandContext Context { get; set; } = new OperandContext();

        public bool IsComplete
        {
            get { return TreeStack.Count == 1; }
        }

        public ExpressionNode Tree
        {
            get
            {
                if (!IsComplete)
                    return null; // Tree is not complete
                return Build();
            }
        }

        public void SetExpression(ExpressionNode node)
        {
            TreeStack = new List<ExpressionNode> { node };
        }

        public void Add(ExpressionNode node)
        {
            TreeStack.Add(node);
        }

        public void Clear()
        {
            TreeStack.Clear();
        }

        public double Evaluate()
        {
            return Tree.Evaluate(Context);
        }

        public ExpressionNode PrecedenceBuild()
        {
            if (TreeStack.Count == 0) throw new Exception("Empty expression tree");
            if (TreeStack.Count == 1) return TreeStack[0];

            var hasNext = new Func<int, bool>(i => i + 1 < TreeStack.Count);

            var precedents = new Stack<int>();
            for (var i = 0; i < TreeStack.Count; i++)
            {
                // TODO: unary check

                if (TreeStack[i] is OperandNode && hasNext(i))
                    continue;

                if (TreeStack[i] is OperatorNode)
                {
                    if (i > 0 && (precedents.Count == 0 || ((OperatorNode)TreeStack[i]).Precedence < ((OperatorNode)TreeStack[precedents.Peek()]).Precedence))
                    {
                        precedents.Push(i);
                        if (hasNext(i))
                            continue;
                    }
                }

                while (precedents.Count > 0)
                {
                    i = precedents.Pop();
                    var oper = (OperatorNode)TreeStack[i];
                    oper.SetChildren(TreeStack[i - 1], TreeStack[i + 1]);
                    TreeStack.RemoveRange(--i, 3);
                    if (!oper.Consume(Context))
                        TreeStack.Insert(i, oper);
                }
            }

            if (TreeStack.Count == 0)
                return null; // This can happen if we i.e. define variables and the stack otherwise is empty

            return TreeStack[0];
        }

        public ExpressionNode Build()
        {
            if (TreeStack.Count == 0) throw new Exception("Empty expression tree");
            if (TreeStack.Count == 1) return TreeStack[0];

            var hasNext = new Func<int, bool>(i => i + 1 < TreeStack.Count);

            for (var i = 0; i < TreeStack.Count; i++)
            {
                // TODO: unary check

                if (TreeStack[i] is OperandNode)
                    continue;

                if (TreeStack[i] is OperatorNode)
                { 
                    var oper = (OperatorNode)TreeStack[i];
                    oper.SetChildren(TreeStack[i - 1], TreeStack[i + 1]);
                    TreeStack.RemoveRange(--i, 3);
                    if (!oper.Consume(Context))
                        TreeStack.Insert(i, oper);
                }
            }

            if (TreeStack.Count == 0)
                return null; // This can happen if we i.e. define variables and the stack otherwise is empty

            return TreeStack[0];
        }
    }
}
