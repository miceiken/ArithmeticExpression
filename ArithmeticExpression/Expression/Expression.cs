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

        public void Add(string token)
        { // cheap ass parser
            if (token.Any(char.IsWhiteSpace))
            { // If there are whitespaces, then these are tokenSSSSSS
                foreach (var t in token.Split(' '))
                    Add(t);
                return;
            }

            double number;
            if (token.Length == 1 && Algebra.CharOperators.ContainsKey(token[0]))
                Add(new OperatorNode(Algebra.CharOperators[token[0]]));
            else if (double.TryParse(token, out number))
                Add(new ValueNode(number));
            else
                Add(new VariableNode(token));
        }

        public ExpressionNode Build()
        {
            if (TreeStack.Count == 0) throw new Exception("Empty expression tree");
            if (TreeStack.Count == 1) return TreeStack[0];

            var hasNext = new Func<int, bool>(i => i + 1 < TreeStack.Count);

            var precedents = new Stack<int>();
            for (var i = 0; i < TreeStack.Count; i++)
            {
                // TODO: unary check

                if (TreeStack[i] is OperandNode && i + 1 < TreeStack.Count)
                    continue;

                if (TreeStack[i] is OperatorNode)
                {
                    if (precedents.Count == 0 || ((OperatorNode)TreeStack[i]).Precedence < ((OperatorNode)TreeStack[precedents.Peek()]).Precedence)
                    {
                        precedents.Push(i);
                        if (i + 1 < TreeStack.Count)
                            continue;
                    }
                }

                while (precedents.Count > 0)
                {
                    i = precedents.Pop();
                    var oper = (OperatorNode)TreeStack[i];
                    oper.SetChildren(TreeStack[i - 1], TreeStack[i + 1]);
                    if (oper.Activate(Context))
                    {
                        TreeStack.RemoveAt(i + 1);
                        TreeStack.RemoveAt(i - 1);
                    }
                    else
                        TreeStack.RemoveRange(i - 1, 3);      

                    i--;
                }
            }

            if (TreeStack.Count == 0)
                return null; // This can happen if we i.e. define variables and the stack otherwise is empty

            return TreeStack[0];
        }
    }
}
