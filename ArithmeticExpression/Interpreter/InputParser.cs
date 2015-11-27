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
            Tree.Clear();

            var tokens = _tokenizer.Parse(input).ToArray();

            var bounds = new Func<int, bool>(i => i > -1 && i < tokens.Length);
            var ensureOperand = new Func<int, bool>(i => bounds(i) && Operands.HasFlag(tokens[i].Type));
            var ensureOperator = new Func<int, bool>(i => bounds(i) && Operators.HasFlag(tokens[i].Type));
            var ensureUnary = new Func<int, bool>(i => bounds(i) && Unary.HasFlag(tokens[i].Type));

            var parDepth = 0;

            for (int i = 0; i < tokens.Count(); i++)
            {
                var token = tokens[i];
                if (token.Type == Tokens.LeftParanthesis)
                    parDepth++;
                if (token.Type == Tokens.RightParanthesis)
                    parDepth--;

                Tree.Add(token.Match);
                //if (ensureOperand(i - 1)) //  && (ensureUnary(i) || (ensureOperator(i) && ensureOperand(i + 1)))
                //{
                //    //if (ensureUnary(i))
                //    //    Console.WriteLine("Un {0} {1}", tokens[i - 1], token);
                //    //else if (ensureOperator(i) && ensureOperand(i + 1))
                //    //    Console.WriteLine("Op {0} {1} {2}", tokens[i - 1], token, tokens[i + 1]);
                //    if (ensureOperator(i) && ensureOperand(i + 1))
                //    {
                //        Tree.Add(tokens[i - 1].Match);
                //        Tree.Add(token.Match);
                //        Tree.Add(tokens[i + 1].Match);
                //    }
                //}
            }

            Tree.PrecedenceBuild();
        }

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
            LeftParanthesis,
            RightParanthesis,
            Number,
            Variable,
            Whitespace
        }
    }
}
