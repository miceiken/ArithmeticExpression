using ArithmeticExpression.Expression;
using ArithmeticExpression.Expression.BinTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmeticExpression.Interpreter
{
    public class InputParser
    {
        public InputParser(ExpressionTree expression)
        {
            Expression = expression;
        }

        public ExpressionTree Expression { get; private set; }

        public void Parse(string input)
        {
            Expression.Build(CreatePostfixExpression(input));
        }

        private static TokenParser<Tokens> CreateExpressionTokenizer()
        {
            var t = new TokenParser<Tokens>(Tokens.Variable);

            t.AddRule("=", Tokens.Equals);
            t.AddRule("+", Tokens.Plus);
            t.AddRule("-", Tokens.Minus);
            t.AddRule("*", Tokens.Asterisk);
            t.AddRule("/", Tokens.Slash);
            t.AddRule("^", Tokens.Caret);
            t.AddRule("%", Tokens.Percent);
            t.AddRule("(", Tokens.LeftParanthesis);
            t.AddRule(")", Tokens.RightParanthesis);
            t.AddRule(" ", Tokens.Whitespace);
            t.AddRule(new NumberLexerComparer(), Tokens.Number);

            t.Ignore(Tokens.Whitespace);

            return t;
        }

        public static Stack<TokenMatch<Tokens>> CreatePostfixTokenStack(string infix)
        {
            var tokenizer = CreateExpressionTokenizer();
            var tokens = tokenizer.Parse(infix);

            var stack = new Stack<TokenMatch<Tokens>>();
            var postfix = new Stack<TokenMatch<Tokens>>();

            foreach (var token in tokens)
            {
                // Operator
                if ((Tokens.Operators & token.Type) == token.Type)
                {
                    while (stack.Count > 0
                        && (Tokens.Operators & stack.Peek().Type) == stack.Peek().Type
                        && Algebra.OperatorPrecedence[TokenArithmeticOperators[token.Type]] >= Algebra.OperatorPrecedence[TokenArithmeticOperators[stack.Peek().Type]])
                        postfix.Push(stack.Pop());
                    stack.Push(token);
                }

                if (token.Type == Tokens.LeftParanthesis)
                {
                    stack.Push(token);
                }

                if (token.Type == Tokens.RightParanthesis)
                {
                    while (stack.Count > 0)
                    {
                        var pop = stack.Pop();
                        if (pop.Type == Tokens.LeftParanthesis)
                            continue;
                        postfix.Push(pop);
                    }
                }

                if ((Tokens.Operands & token.Type) == token.Type)
                {
                    postfix.Push(token);
                }
            }

            while (stack.Count > 0)
                postfix.Push(stack.Pop());

            return postfix;
        }

        public static IEnumerable<ExpressionNode> CreatePostfixExpression(string infix)
        {
            foreach (var token in CreatePostfixTokenStack(infix).Reverse())
            {
                if ((Tokens.Operators & token.Type) == token.Type)
                    yield return new OperatorNode(TokenArithmeticOperators[token.Type]);
                if ((Tokens.Operands & token.Type) == token.Type)
                {
                    if (token.Type == Tokens.Number)
                        yield return new ValueNode(double.Parse(token.Match));
                    if (token.Type == Tokens.Variable)
                        yield return new VariableNode(token.Match);
                }
            }
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

        [Flags]
        public enum Tokens : ushort
        {
            Equals = 1 << 0,
            Plus = 1 << 1,
            Minus = 1 << 2,
            Asterisk = 1 << 3,
            Slash = 1 << 4,
            Caret = 1 << 5,
            Percent = 1 << 6,
            LeftParanthesis = 1 << 7,
            RightParanthesis = 1 << 8,
            Number = 1 << 9,
            Variable = 1 << 10,
            Whitespace = 1 << 11,

            Operands = Number | Variable,
            Operators = Equals | Plus | Minus | Asterisk | Slash | Caret,
            Unary = Minus
        }
    }
}
