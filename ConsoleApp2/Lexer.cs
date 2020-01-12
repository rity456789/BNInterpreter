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
        private int peekPos;

        private static Dictionary<string, Token> RESERVED_KEYWORDS = new Dictionary<string, Token>();

        public Lexer(string inputText)
        {
            text = inputText;
            pos = 0;
            peekPos = 0;
            if (text.Length > 0)
            {
                currentChar = text[pos];
            }
        }

        public static void InitReservedKeywords()
        {
            RESERVED_KEYWORDS.Add("BEGIN", new Token(TokenType.BEGIN, "BEGIN"));
            RESERVED_KEYWORDS.Add("END", new Token(TokenType.END, "END"));
        }

        private bool IsEnd()
        {
            return this.currentChar == ENDCHAR;
        }

        private void SkipWhiteSpace()
        {
            while(!IsEnd() && Char.IsWhiteSpace(this.currentChar))
            {
                this.AdvanceCurrentChar();
            }
        }

        private Token _id()
        {
            var result = "";

            while(!this.IsEnd() && Char.IsLetterOrDigit(this.currentChar))
            {
                result += currentChar;
                this.AdvanceCurrentChar();
            }

            return RESERVED_KEYWORDS.ContainsKey(result) ? RESERVED_KEYWORDS[result] : new Token(TokenType.ID, result);
        }

        /// <summary>
        /// lay token tiep theo
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            while(!this.IsEnd())
            {
                if(Char.IsWhiteSpace(this.currentChar))
                {
                    this.SkipWhiteSpace();
                    continue;
                }
                else if(Char.IsLetter(this.currentChar))
                {
                    return this._id();
                }
                else if (Char.IsDigit(currentChar))
                {
                    return new Token(TokenType.INTERGER, GetInteger());
                }
                else if(this.currentChar == ':' && this.peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.ASSIGN, currentChar.ToString());
                }
                else if(this.currentChar == ';')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.SEMI, currentChar.ToString());
                }
                else if(this.currentChar == '.')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.DOT, currentChar.ToString());
                }
                else
                {
                    var currentOperator = currentChar;
                    AdvanceCurrentChar();

                    return GetTokenOperator(currentOperator);
                }
            }

            return new Token(TokenType.EOF, null);

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
            pos++;

            if (pos > text.Length - 1)
            {
                currentChar = ENDCHAR;
                return;
            }

            currentChar = text[pos];
        }

        private string OptimizeText(string inputText)
        {
            return inputText.Replace(" ", ""); 
        }

        private char peek()
        {
            this.peekPos = this.pos + 1;
            if (this.peekPos > this.text.Length - 1)
            {
                return ' ';
            }
            else
            {
                return this.text[peekPos];
            }
        }
    }
}
