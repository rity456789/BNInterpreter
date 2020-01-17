using TokenNamespace;

namespace BNInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = "input.txt";
            if (args.Length > 0) file = args[0];

            Token.AddSampleCompareOperator();
            Lexer.InitReservedKeywords();

            string text = Input.Read(file);

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
