using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticExpression.Interpreter
{
    public class TokenParser<T> where T : struct
    {
        public TokenParser(T general)
        {
            _general = general;
        }

        private T _general;

        public IEnumerable<TokenMatch<T>> Parse(string text)
        {
            var rules = _ruleDefinitions.Keys;

            var gIdx = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (rules.Any(c => c.Compare(text, i, text.Length).IsMatch))
                {
                    if (gIdx > -1)
                    {
                        yield return new TokenMatch<T> { Type = _general, Match = text.Substring(gIdx, i - gIdx) };
                        gIdx = -1;
                    }

                    var rule = rules.First(c => c.Compare(text, i, text.Length).IsMatch);
                    if (_ignored.Contains(_ruleDefinitions[rule]))
                        continue;

                    var cmp = rule.Compare(text, i, text.Length);
                    if (string.IsNullOrEmpty(cmp.Matched)) throw new Exception("Comparer returned empty match");
                    yield return new TokenMatch<T> { Type = _ruleDefinitions[rule], Match = cmp.Matched };
                    i += cmp.Length - 1;
                    continue;
                }

                if (gIdx == -1)
                    gIdx = i;
            }

            if (gIdx > -1)
                yield return new TokenMatch<T> { Type = _general, Match = text.Substring(gIdx, text.Length - gIdx) };
        }

        private Dictionary<LexerComparer, T> _ruleDefinitions = new Dictionary<LexerComparer, T>();
        public void AddRule(LexerComparer cmp, T def)
        {
            _ruleDefinitions.Add(cmp, def);
        }

        public void AddRule(string cmp, T def)
        {
            _ruleDefinitions.Add(new StringLexerComparer(cmp), def);
        }

        private List<T> _ignored = new List<T>();
        public void Ignore(T def)
        {
            _ignored.Add(def);
        }

    }

    public struct TokenMatch<T>
    {
        public T Type;
        public string Match;

        public override string ToString()
        {
            return string.Format("<{0},{1}>", Type, Match);
        }
    }

    public struct LexerMatch
    {
        public bool IsMatch;
        public int Length;
        public string Matched;
    }

    public abstract class LexerComparer
    {
        public abstract LexerMatch Compare(string control, int start, int size);
    }

    public class StringLexerComparer : LexerComparer
    {
        public StringLexerComparer(string toMatch)
        {
            if (string.IsNullOrEmpty(toMatch))
                throw new Exception("StringLexerComparer initialized with empty string");
            ToMatch = toMatch;
        }

        private string ToMatch;

        public override LexerMatch Compare(string control, int start, int size)
        {
            if (start + ToMatch.Length < size && control.Substring(start, ToMatch.Length) == ToMatch)
                return new LexerMatch { IsMatch = true, Length = ToMatch.Length, Matched = control.Substring(start, ToMatch.Length) };
            return new LexerMatch { IsMatch = false };
        }
    }

    public class NumberLexerComparer : LexerComparer
    {
        public override LexerMatch Compare(string control, int start, int size)
        {
            var c = 0;
            var i = new Func<int>(() => start + c);
            while (i() < size && (char.IsDigit(control[i()]) || control[i()].Equals('.'))) c++;
            if (c == 0)
                return new LexerMatch { IsMatch = false };
            return new LexerMatch { IsMatch = true, Length = c, Matched = control.Substring(start, c) };
        }
    }
}
