using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;
using SymbolNamespace;

namespace BNInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Token.AddSampleCompareOperator();
            Lexer.InitReservedKeywords();

            string text = Input.Read();

            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);


            var tree = parser.Parse();
            var semanticAnalyzer = new SemanticAnalyzer();
            semanticAnalyzer.Visit(tree);

            Interpreter interpreter = new Interpreter(tree);
            int result = interpreter.Interpret();

            //Console.WriteLine(interpreter.GLOBAL_SCOPE["y"]);
            //Console.WriteLine(interpreter.GLOBAL_SCOPE["b"]);



            //test Symbol

        }
    }
}
