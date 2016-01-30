using ArithmeticExpression.Expression;
using ArithmeticExpression.Expression.BinTree;
using System;

namespace ArithmeticExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ArithmeticExpression";
            Console.WriteLine("'vars' to list variables, 'clear' to clear expression tree");

            var e = new ExpressionTree();
            var p = new Interpreter.InputParser(e);

            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                switch (line)
                {
                    case "vars": // Dump variables
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Variables:");
                        foreach (var kvp in e.Context.Variables)
                            Console.WriteLine("\t{0} = {1}", kvp.Key, kvp.Value);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case "clear":
                        e.Clear();
                        break;

                    default:
                        p.Parse(line);
                        try
                        {
                            Console.WriteLine();
                            Console.WriteLine(e.InfixExpression);
                            Console.WriteLine($"Stack: [{string.Join(" ", e.GetEvaluated())}]");
                        }
                        catch (UndefinedVariableException ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($">> {ex.Message}");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
