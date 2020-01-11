using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Lexer
    {
        private const char ENDCHAR = '@';

        private string text;
        private int pos;
        private char currentChar;
        public Lexer(string inputText)
        {
            text = OptimizeText(inputText);
            pos = 0;
            if (text.Length > 0)
            {
                currentChar = text[pos];
            }
        }

        /// <summary>
        /// lay token tiep theo
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            if (pos > text.Length - 1)
            {
                return new Token(TokenType.EOF, null);
            }

            if (char.IsDigit(currentChar))
            {
                return new Token(TokenType.INTERGER, GetInteger());

            }
            else
            {
                var currentOperator = currentChar;
                AdvanceCurrentChar();

                return GetTokenOperator(currentOperator);
            }

        }

        private Token GetTokenOperator(char currentOperator)
        {
            if (currentOperator == '+')
            {
                return new Token(TokenType.PLUS, currentOperator.ToString());
            }
            else if (currentOperator == '-')
            {
                return new Token(TokenType.MINUS, currentOperator.ToString());
            }
            else if (currentOperator == '*')
            {
                return new Token(TokenType.MULTIPLE, currentOperator.ToString());
            }
            else if (currentOperator == '/')
            {
                return new Token(TokenType.DIVIDE, currentOperator.ToString());
            }
            else if (currentOperator == '(')
            {
                return new Token(TokenType.LPAREN, currentOperator.ToString());
            }
            else if (currentOperator == ')')
            {
                return new Token(TokenType.RPAREN, currentOperator.ToString());
            }
            else return new Token(TokenType.EOF, null);
        }

        /// <summary>
        /// lấy số nguyên
        /// </summary>
        /// <returns></returns>
        private string GetInteger()
        {
            string currentTokenVal = "";
            while (char.IsDigit(currentChar))
            {
                currentTokenVal += currentChar;
                AdvanceCurrentChar();
            }
            return currentTokenVal;
        }

        /// <summary>
        /// dịch pos lên 1, cập nhật curChar
        /// </summary>
        private void AdvanceCurrentChar()
        {
            if (pos > text.Length - 2)
            {
                currentChar = ENDCHAR;
                return;
            }
            pos++;
            currentChar = text[pos];
        }

        private string OptimizeText(string inputText)
        {
            return inputText.Replace(" ", ""); 
        }
    }
}
