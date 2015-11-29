using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArithmeticExpression.Interpreter
{
    public class TokenParser<T> where T : struct
    {
        public TokenParser(T def)
        {
            _default = def;
        }

        private T _default;

        public IEnumerable<TokenMatch<T>> Parse(string text)
        {
            var rules = _ruleDefinitions.Keys;

            var gIdx = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (rules.Any(c => !string.IsNullOrEmpty(c.Compare(text, i, text.Length))))
                {
                    if (gIdx > -1)
                    {
                        yield return new TokenMatch<T> { Type = _default, Match = text.Substring(gIdx, i - gIdx) };
                        gIdx = -1;
                    }

                    var rule = rules.First(c => !string.IsNullOrEmpty(c.Compare(text, i, text.Length)));
                    if (_ignored.Contains(_ruleDefinitions[rule]))
                        continue;

                    var cmp = rule.Compare(text, i, text.Length);
                    yield return new TokenMatch<T> { Type = _ruleDefinitions[rule], Match = cmp };
                    i += cmp.Length - 1;
                    continue;
                }

                if (gIdx == -1) gIdx = i;
            }

            if (gIdx > -1) yield return new TokenMatch<T> { Type = _default, Match = text.Substring(gIdx, text.Length - gIdx) };
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

    public abstract class LexerComparer
    {
        public abstract string Compare(string control, int start, int size);
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

        public override string Compare(string control, int start, int size)
        {
            if (start + ToMatch.Length > size || control.Substring(start, ToMatch.Length) != ToMatch) return null;
            return control.Substring(start, ToMatch.Length);
        }
    }

    public class NumberLexerComparer : LexerComparer
    {
        public override string Compare(string control, int start, int size)
        {
            var c = 0;
            var i = new Func<int>(() => start + c);
            while (i() < size && (char.IsDigit(control[i()]) || control[i()].Equals('.'))) c++;
            if (c == 0) return null;
            return control.Substring(start, c);
        }
    }

    public class RegexLexerComparer : LexerComparer
    {
        public RegexLexerComparer(Regex regex)
        {
            rgx = regex;
        }

        public RegexLexerComparer(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new Exception("RegexLexerComparer initialized with empty string");
            rgx = new Regex(pattern);
        }

        private Regex rgx;

        public override string Compare(string control, int start, int size)
        {
            if (!rgx.IsMatch(control, start)) return null;
            return rgx.Match(control, start).Value;
        }

        public static readonly RegexLexerComparer EmailPreset = new RegexLexerComparer(@"^[^<>\s\@]+(\@[^<>\s\@]+(\.[^<>\s\@]+)+)$");
    }
}
