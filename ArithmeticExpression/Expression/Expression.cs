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
            : this(DepthTraversalMethod.PostOrder)
        { }

        public Expression(DepthTraversalMethod traversal)
        {
            Traversal = traversal;
        }
        
                public Stack<ExpressionNode> TreeStack { get; private set; } = new Stack<ExpressionNode>();
        public OperandContext Context { get; set; } = new OperandContext();
        public DepthTraversalMethod Traversal { get; private set; } = DepthTraversalMethod.PostOrder;

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
                return TreeStack.Peek();
            }
        }

        public void Add(ExpressionNode node)
        {
            TreeStack.Push(node);

            while (TreeStack.Count > 0 && TreeStack.Count % 3 == 0)
            {
                ExpressionNode leftOperand, rightOperand, exprOperator;

                switch (Traversal)
                {
                    case DepthTraversalMethod.PreOrder:
                        rightOperand = TreeStack.Pop();
                        leftOperand = TreeStack.Pop();
                        exprOperator = TreeStack.Pop();
                        break;

                    case DepthTraversalMethod.InOrder:
                        rightOperand = TreeStack.Pop();
                        exprOperator = TreeStack.Pop();
                        leftOperand = TreeStack.Pop();
                        break;

                    default:
                    case DepthTraversalMethod.PostOrder:
                        exprOperator = TreeStack.Pop();
                        rightOperand = TreeStack.Pop();
                        leftOperand = TreeStack.Pop();
                        break;
                }

                exprOperator.Left = leftOperand;
                exprOperator.Right = rightOperand;
                if (exprOperator.Operator == Operators.Define)
                { // Special case -- defining variables
                    Context.Variables[leftOperand.Operand.Variable] = rightOperand.Operand.Number;
                    continue; // Don't push defines on stack
                }

                TreeStack.Push(exprOperator);
            }
        }

        public double Evaluate()
        {
            return Tree.GetEvaluated(Context);
            //return Evaluate(Tree);
        }

        //public double Evaluate(ExpressionNode node)
        //{
        //    if (node == null) return 0;

        //    if (node.Operator != Operators.Operand)
        //    {
        //        if (node.Left == null || node.Right == null)
        //            return 0; // this shouldn't happen

        //        // cue ugly debug code
        //        if (System.Diagnostics.Debugger.IsAttached)
        //        {
        //            var l = Evaluate(node.Left);
        //            var r = Evaluate(node.Right);
        //            var res = Algebra.Evaluators[node.Operator](l, r);
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("{0}({1}, {2}) = {3}", node.Operator, l, r, res);
        //            Console.ForegroundColor = ConsoleColor.Gray;
        //            return res;
        //        }
        //        // the end

        //        return Algebra.Evaluators[node.Operator](Evaluate(node.Left), Evaluate(node.Right));
        //    }
        //    return node.Operand.GetValue(Context);
        //}

        public void Add(string token)
        {
            if (token.Any(char.IsWhiteSpace))
            { // If there are whitespaces, then these are tokenSSSSSS
                foreach (var t in token.Split(' '))
                    Add(t);
                return;
            }

            double number;
            if (token.Length == 1 && Algebra.CharOperators.ContainsKey(token[0])) // Operator
                Add(new ExpressionNode(Algebra.CharOperators[token[0]]));
            else if (double.TryParse(token, out number)) // Number Operand
                Add(new ExpressionNode(number));
            else // Variable operand
                Add(new ExpressionNode(token));
        }

        public void Clear()
        {
            TreeStack.Clear();
        }
    }
}
