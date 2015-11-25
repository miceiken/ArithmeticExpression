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
        static Expression.Expression e = new Expression.Expression();
        static void Main(string[] args)
        {
            Console.Title = "ArithmeticExpression";

            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                e.Add(line);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Variables:");
                foreach (var kvp in e.Context.Variables)
                    Console.WriteLine("{0} = {1}", kvp.Key, kvp.Value);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Result: {0}", e.Evaluate());
            }
        }
    }
}
