﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Token.AddSampleOperator();
            Lexer.InitReservedKeywords();

            while (true)
            {
                string text;
                
                text = Input.Read();

                Lexer lexer = new Lexer(text);
                Parser parser = new Parser(lexer);
                Interpreter interpreter = new Interpreter(parser);
                int result = interpreter.Interpret();

                Console.WriteLine(result);
            }
        }
    }
}
