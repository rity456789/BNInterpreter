using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenNamespace
{
    /// <summary>
    /// Type của token
    /// </summary>
    public enum TokenType
    {
        // Constant Number
        INTEGER_CONST = 10,
        REAL_CONST = 11,
        // Types
        INTEGER = 20,
        REAL = 21,
        // Compare
        EQUAL = 31,
        NOT_EQUAL = 32,
        GREATER_THAN = 33,
        LESS_THAN = 34,
        GREATER_THAN_OR_EQUAL = 35,
        LESS_THAN_OR_EQUAL = 36,
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
        RETURN = 78,
        IF = 79,
        ELSE = 80,
        WHILE = 81,
        // Others
        ID = 91,
        ASSIGN = 92,
        SEMI = 93,
        DOT = 94,




        EOF = 199,
    }

    /// <summary>
    /// Class lưu trữ token gồm (type, value, dòng, cột)
    /// </summary>
    public class Token
    {
        private static List<TokenType> listCompareOperator = new List<TokenType>();
        public TokenType type { get; private set; } //type
        public string value { get; private set; }   //giá trị
        public int lineno;                          //dòng
        public int column;                          //cột

        public Token(TokenType type, string value, int lineno = -1, int column = -1)
        {
            this.type = type;
            this.value = value;
            this.lineno = lineno;
            this.column = column;
        }

        /// <summary>
        /// Hiển thị token dưới dạng chuỗi Token: {Loại}, {Giá trị}, {Vị trí} 
        /// </summary>
        /// <returns></returns>
        public string ShowToken()
        {
            // Token: {Loại}, {Giá trị}, {Vị trí} 
            return "Token: " + this.type.ToString() + ", " + this.value + ", position = " + this.lineno + ":" + this.column;
        }

        public static void AddSampleCompareOperator()
        {
            listCompareOperator.Add(TokenType.EQUAL);
            listCompareOperator.Add(TokenType.NOT_EQUAL);
            listCompareOperator.Add(TokenType.GREATER_THAN);
            listCompareOperator.Add(TokenType.LESS_THAN);
            listCompareOperator.Add(TokenType.GREATER_THAN_OR_EQUAL);
            listCompareOperator.Add(TokenType.LESS_THAN_OR_EQUAL);
        }

        public bool IsCompareOperator()
        {
            return listCompareOperator.Contains(this.type);
        }
    }
}
