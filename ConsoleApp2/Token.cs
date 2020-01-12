using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    enum TokenType
    {
        INTERGER = 0,
        PLUS = 1,
        MINUS = 2,
        MULTIPLE = 3,
        DIVIDE = 4,
        LPAREN = 5,
        RPAREN = 6,

        BEGIN = 7,
        END = 8,
        ID = 9,
        ASSIGN = 10,
        SEMI = 11,
        DOT = 12,

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
            listOperator.Add(TokenType.INTERGER, false);
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
