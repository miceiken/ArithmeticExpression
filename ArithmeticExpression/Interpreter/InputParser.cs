using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArithmeticExpression.Expression;
using ArithmeticExpression.Expression.BinTree;

namespace ArithmeticExpression.Interpreter
{
    public class InputParser
    {
        public InputParser(Expression.Expression tree)
        {
            Tree = tree;

            // If none of the rules match, we assume that the token is a variable
            _tokenizer = new TokenParser<Tokens>(Tokens.Variable);

            _tokenizer.AddRule("=", Tokens.Equals);
            _tokenizer.AddRule("+", Tokens.Plus);
            _tokenizer.AddRule("-", Tokens.Minus);
            _tokenizer.AddRule("*", Tokens.Asterisk);
            _tokenizer.AddRule("/", Tokens.Slash);
            _tokenizer.AddRule("^", Tokens.Caret);
            _tokenizer.AddRule("%", Tokens.Percent);
            _tokenizer.AddRule("(", Tokens.LeftParanthesis);
            _tokenizer.AddRule(")", Tokens.RightParanthesis);
            _tokenizer.AddRule(" ", Tokens.Whitespace);
            _tokenizer.AddRule(new NumberLexerComparer(), Tokens.Number);

            _tokenizer.Ignore(Tokens.Whitespace);
        }

        private TokenParser<Tokens> _tokenizer;

        public Expression.Expression Tree { get; private set; }

        public void Parse(string input)
        {
            var tokens = _tokenizer.Parse(input).ToArray();

            var bounds = new Func<int, bool>(i => i > -1 && i < tokens.Length);
            var ensureOperand = new Func<int, bool>(i => bounds(i) && Operands.HasFlag(tokens[i].Type));
            var ensureOperator = new Func<int, bool>(i => bounds(i) && Operators.HasFlag(tokens[i].Type));
            var ensureUnary = new Func<int, bool>(i => bounds(i) && Unary.HasFlag(tokens[i].Type));

            var order = new Queue<ExpressionNode>();

            var parDepth = 0;

            for (int i = 0; i < tokens.Count(); i++)
            {
                var token = tokens[i];
                if (token.Type == Tokens.LeftParanthesis) parDepth++;
                if (token.Type == Tokens.RightParanthesis)
                {
                    if (parDepth > 0)
                    {
                        while (order.Count > 0) Tree.Add(order.Dequeue());
                        Tree.PrecedenceBuild();
                        parDepth--;
                    }
                    else throw new Exception("( expected");
                }

                if (ensureUnary(i) && ensureOperand(i - 1))
                    order.Enqueue(new OperatorNode(TokenArithmeticOperators[token.Type])); // TokenUnaryOperators
                else if (ensureOperator(i) && ensureOperand(i - 1) && ensureOperand(i + 1))
                    order.Enqueue(new OperatorNode(TokenArithmeticOperators[token.Type]));
                else if (ensureOperand(i) && token.Type == Tokens.Number)
                    order.Enqueue(new ValueNode(double.Parse(token.Match)));
                else if (ensureOperand(i) && token.Type == Tokens.Variable)
                    order.Enqueue(new VariableNode(token.Match));
            }
            if (parDepth > 0) throw new Exception(") expected");
            while (order.Count > 0) Tree.Add(order.Dequeue());
            Tree.PrecedenceBuild();
        }

        public static readonly Dictionary<Tokens, AssignmentOperators> TokenAssignmentOperators = new Dictionary<Tokens, AssignmentOperators>()
        {
            [Tokens.Equals] = AssignmentOperators.Define,
        };

        public static readonly Dictionary<Tokens, UnaryOperators> TokenUnaryOperators = new Dictionary<Tokens, UnaryOperators>()
        {
            [Tokens.Minus] = UnaryOperators.Minus,
        };

        public static readonly Dictionary<Tokens, ArithmeticOperators> TokenArithmeticOperators = new Dictionary<Tokens, ArithmeticOperators>()
        {
            [Tokens.Plus] = ArithmeticOperators.Add,
            [Tokens.Minus] = ArithmeticOperators.Subtract,
            [Tokens.Asterisk] = ArithmeticOperators.Multiply,
            [Tokens.Slash] = ArithmeticOperators.Divide,
            [Tokens.Caret] = ArithmeticOperators.Exponent,
            [Tokens.Percent] = ArithmeticOperators.Modulus,

            [Tokens.Equals] = ArithmeticOperators.Define,
        };

        public static readonly Tokens Operands = Tokens.Number | Tokens.Variable;
        public static readonly Tokens Operators = Tokens.Equals | Tokens.Plus | Tokens.Minus | Tokens.Asterisk | Tokens.Slash | Tokens.Caret;
        public static readonly Tokens Unary = Tokens.Minus;

        public enum Tokens
        {
            Equals,
            Plus,
            Minus,
            Asterisk,
            Slash,
            Caret,
            Percent,
            LeftParanthesis,
            RightParanthesis,
            Number,
            Variable,
            Whitespace
        }
    }
}
