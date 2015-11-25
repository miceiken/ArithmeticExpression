using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArithmeticExpression.Expression;
using ArithmeticExpression.Expression.BinTree;

namespace ArithmeticExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ArithmeticExpression";
            Console.WriteLine("'vars' to list variables, 'clear' to clear expression tree");

            var e = new Expression.Expression();

            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                switch (line)
                {
                    case "vars":
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Variables:");
                        foreach (var kvp in e.Context.Variables)
                            Console.WriteLine("{0} = {1}", kvp.Key, kvp.Value);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case "clear":
                        e.Clear();
                        Console.Clear();
                        break;
                    default:
                        e.Add(line);
                        Console.WriteLine("Result: {0}", e.Evaluate());
                        break;
                }             
            }
        }
    }
}
