using Errors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;

namespace BNInterpreter
{

    class Lexer
    {
        private const char ENDCHAR = '@';

        private string text;
        private int pos;
        public char currentChar;
        private int lineno;
        private int column;

        private static Dictionary<string, Token> RESERVED_KEYWORDS = new Dictionary<string, Token>();

        public Lexer(string inputText)
        {
            text = inputText;
            pos = 0;
            if (text.Length > 0)
            {
                currentChar = text[pos];
            }
            lineno = 1;
            column = 1;
        }

        private void ShowError()
        {
            var message = "Lexer error on " + this.currentChar + " line: " + this.lineno + " column: " + this.column;
            var error = new LexerError(message: message);
            error.ShowError();
        }

        public static void InitReservedKeywords()
        {
            RESERVED_KEYWORDS.Add("BEGIN", new Token(TokenType.BEGIN, "BEGIN"));
            RESERVED_KEYWORDS.Add("END", new Token(TokenType.END, "END"));
            RESERVED_KEYWORDS.Add("VAR", new Token(TokenType.VAR, "VAR"));
            RESERVED_KEYWORDS.Add("PROGRAM", new Token(TokenType.PROGRAM, "PROGRAM"));
            RESERVED_KEYWORDS.Add("INTEGER", new Token(TokenType.INTEGER, "INTEGER"));
            RESERVED_KEYWORDS.Add("REAL", new Token(TokenType.REAL, "REAL"));
            RESERVED_KEYWORDS.Add("DIV", new Token(TokenType.INTEGER_DIV, "DIV"));
            RESERVED_KEYWORDS.Add("PROCEDURE", new Token(TokenType.PROCEDURE, "PROCEDURE"));
        }

        private bool IsEnd()
        {
            return this.currentChar == ENDCHAR;
        }

        private void SkipWhiteSpace()
        {
            while (!IsEnd() && Char.IsWhiteSpace(this.currentChar))
            {
                this.AdvanceCurrentChar();
            }
        }

        private void SkipComment()
        {
            while (this.currentChar != '}')
            {
                this.AdvanceCurrentChar();
            }

            this.AdvanceCurrentChar();
        }

        private Token Number()
        {
            string result = "";
            Token token;
            // Lấy phần nguyên
            while (!IsEnd() && Char.IsDigit(this.currentChar))
            {
                result += this.currentChar;
                this.AdvanceCurrentChar();
            }

            // Lấy phần thập phân
            if (this.currentChar == '.')
            {
                result += this.currentChar;
                this.AdvanceCurrentChar();

                while (!IsEnd() && Char.IsDigit(this.currentChar))
                {
                    result += this.currentChar;
                    this.AdvanceCurrentChar();
                }

                token = new Token(TokenType.REAL_CONST, result, this.lineno, this.column);
            }
            else
            {
                token = new Token(TokenType.INTEGER_CONST, result, this.lineno, this.column);
            }

            return token;
        }

        private float ParseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        private Token _id()
        {
            var result = "";

            while (!this.IsEnd() && Char.IsLetterOrDigit(this.currentChar))
            {
                result += currentChar;
                this.AdvanceCurrentChar();
            }

            return RESERVED_KEYWORDS.ContainsKey(result) ? RESERVED_KEYWORDS[result] : new Token(TokenType.ID, result, this.lineno, this.column);
        }

        /// <summary>
        /// lay token tiep theo
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            while (!this.IsEnd())
            {
                // White space
                if (Char.IsWhiteSpace(this.currentChar))
                {
                    this.SkipWhiteSpace();
                    continue;
                }
                // Comment
                else if (this.currentChar == '{')
                {
                    this.AdvanceCurrentChar();
                    this.SkipComment();
                    continue;
                }
                // Identifiers/Reserved words
                else if (Char.IsLetter(this.currentChar))
                {
                    return this._id();
                }
                // Digit
                else if (Char.IsDigit(currentChar))
                {
                    return this.Number();
                }
                // Assign
                else if (this.currentChar == ':' && this.peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.ASSIGN, currentChar.ToString(), this.lineno, this.column);
                }
                // End statement
                else if (this.currentChar == ';')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.SEMI, currentChar.ToString(), this.lineno, this.column);
                }
                // End program
                else if (this.currentChar == '.')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.DOT, currentChar.ToString(), this.lineno, this.column);
                }
                
                else if (this.currentChar == ':')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.COLON, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == ',')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.COMMA, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '/')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.REAL_DIV, currentChar.ToString(), this.lineno, this.column);
                }
                // Operators
                else
                {
                    var currentOperator = currentChar;
                    AdvanceCurrentChar();

                    return GetTokenOperator(currentOperator);
                }
            }

            return new Token(TokenType.EOF, null, this.lineno, this.column);

        }

        private Token GetTokenOperator(char currentOperator)
        {
            if (currentOperator == '+')
            {
                return new Token(TokenType.PLUS, currentOperator.ToString(), this.lineno, this.column);
            }
            else if (currentOperator == '-')
            {
                return new Token(TokenType.MINUS, currentOperator.ToString(), this.lineno, this.column);
            }
            else if (currentOperator == '*')
            {
                return new Token(TokenType.MULTIPLE, currentOperator.ToString(), this.lineno, this.column);
            }
            else if (currentOperator == '/')
            {
                return new Token(TokenType.DIVIDE, currentOperator.ToString(), this.lineno, this.column);
            }
            else if (currentOperator == '(')
            {
                return new Token(TokenType.LPAREN, currentOperator.ToString(), this.lineno, this.column);
            }
            else if (currentOperator == ')')
            {
                return new Token(TokenType.RPAREN, currentOperator.ToString(), this.lineno, this.column);
            }
            else return new Token(TokenType.EOF, null, this.lineno, this.column);
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
            if (this.currentChar == '\n')
            {
                this.lineno += 1;
                this.column = 0;
            }
            pos++;

            if (pos > text.Length - 1)
            {
                currentChar = ENDCHAR;
                return;
            }

            currentChar = text[pos];
            this.column += 1;
        }

        private char peek()
        {
            var peekPos = this.pos + 1;
            if (peekPos > this.text.Length - 1)
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
