using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenNamespace
{
    public enum TokenType
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
        PROCEDURE = 77,
        // Others
        ID = 81,
        ASSIGN = 82,
        SEMI = 83,
        DOT = 84,



        EOF = 99,
    }

    public class Token
    {
        private static Dictionary<TokenType, bool> listOperator = new Dictionary<TokenType, bool>();
        //private List<TokenType> listOperator = new List<TokenType>((int)TokenType.PLUS);
        public TokenType type { get; private set; }
        public string value { get; private set; }
        public int lineno;
        public int column;

        public Token(TokenType type, string value, int lineno = -1, int column = -1)
        {
            this.type = type;
            this.value = value;
            this.lineno = lineno;
            this.column = column;
        }

        public string ShowToken()
        {
            return "Token: " + this.type.ToString() + ", " + this.value + ", position = " + this.lineno + ":" + this.column;
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
