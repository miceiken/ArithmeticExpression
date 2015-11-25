﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArithmeticExpression.Expression.BinTree;

namespace ArithmeticExpression.Expression
{
    public class Expression
    {
        public Expression() { }

        private Stack<ExpressionNode> _tree = new Stack<ExpressionNode>();
        public OperandContext Context { get; set; } = new OperandContext();        

        public void Add(ExpressionNode node)
        {
            if (node.Operator != Operators.Operand)
            {
                // We push left - right, so we are popping right - left
                node.Right = _tree.Pop();
                node.Left = _tree.Pop();

                if (node.Operator == Operators.Define)
                { // Special case -- defining variables
                    if (node.Left == null || node.Right == null)
                        return;
                    Context.Variables[node.Left.Operand.Variable] = node.Right.Operand.Number;
                    return; // Don't push it back on the stack. Or maybe do? for cool effects
                }
            }

            _tree.Push(node);
        }

        public double Evaluate()
        {
            return Evaluate(RootNode);
        }

        public double Evaluate(ExpressionNode node)
        {
            if (node == null) return 0;

            if (node.Operator != Operators.Operand)
            {
                if (node.Left == null || node.Right == null)
                    return 0; // this shouldn't happen
                if (System.Diagnostics.Debugger.IsAttached) {
                    var l = Evaluate(node.Left);
                    var r = Evaluate(node.Right);
                    var res = Algebra.Evaluators[node.Operator](l, r);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}({1}, {2})[{3}]",
                        node.Operator, l, r,
                        res);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return res;
                }
                return Algebra.Evaluators[node.Operator](Evaluate(node.Left), Evaluate(node.Right));
            }
            return node.Operand.GetValue(Context);
        }

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
            _tree.Clear();
        }

        public ExpressionNode RootNode
        {
            get
            {
                if (_tree.Count != 1)
                    return null; // Tree is not complete
                return _tree.Peek();
            }
        }
    }
}
