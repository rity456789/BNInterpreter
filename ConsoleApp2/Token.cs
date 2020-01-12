using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    enum TokenType
    {
        // Constant Number
        INTEGER_CONST = 10,
        REAL_CONST = 11,
        // Types
        INTEGER = 20,
        REAL = 21,
        // Operators
        PLUS = 61,
        MINUS = 62,
        MULTIPLE = 63,
        DIVIDE = 64,
        LPAREN = 65,
        RPAREN = 66,
        INTEGER_DIV = 67,
        REAL_DIV = 68,
        // Reserved Keywords
        BEGIN = 71,
        END = 72,
        PROGRAM = 73,
        VAR = 74,
        COMMA = 75,
        COLON = 76,
        // Others
        ID = 81,
        ASSIGN = 80,
        SEMI = 81,
        DOT = 82,



        EOF = 99,
    }

    class Token
    {
        private static Dictionary<TokenType, bool> listOperator = new Dictionary<TokenType, bool>();
        //private List<TokenType> listOperator = new List<TokenType>((int)TokenType.PLUS);
        public TokenType type { get; private set; }
        public string value { get; private set; }

        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public static void AddSampleOperator()
        {
            listOperator.Add(TokenType.INTEGER_CONST, false);
            listOperator.Add(TokenType.PLUS, true);
            listOperator.Add(TokenType.MINUS, true);
            listOperator.Add(TokenType.MULTIPLE, true);
            listOperator.Add(TokenType.DIVIDE, true);
            listOperator.Add(TokenType.LPAREN, false);
            listOperator.Add(TokenType.RPAREN, false);


            listOperator.Add(TokenType.EOF, false);
        }

        public bool IsOperator()
        {
            return listOperator.ContainsKey(this.type) && listOperator[this.type];
        }
    }
}
