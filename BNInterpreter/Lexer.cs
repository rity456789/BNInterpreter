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
    /// <summary>
    /// Class phân tích chuỗi thành các token
    /// </summary>
    class Lexer
    {
        private const char ENDCHAR = '@';       // Kí tự kết thúc

        private string text;                    // Chuỗi cần phân tích thành token
        private int pos;                        // Vị trí hiện tại đang phân tích
        public char currentChar;                // Kí tự hiện tại
        private int lineno;                     // Dòng hiện tại trong chương trình người dùng nhập
        private int column;                     // Cột hiện tại trong chương trình người dùng nhập

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

        /// <summary>
        /// Hàm đẩy thông báo lỗi
        /// </summary>
        private void ShowError()
        {
            var message = "Lexer error on " + this.currentChar + " line: " + this.lineno + " column: " + this.column;
            var error = new LexerError(message: message);
            error.ShowError();
        }

        /// <summary>
        /// Hàm tĩnh khởi tạo các RESERVED_KEYWORDS của chương trình
        /// </summary>
        public static void InitReservedKeywords()
        {
            //RESERVED_KEYWORDS.Add("BEGIN", new Token(TokenType.BEGIN, "BEGIN"));
            //RESERVED_KEYWORDS.Add("END", new Token(TokenType.END, "END"));
            RESERVED_KEYWORDS.Add("var", new Token(TokenType.VAR, "VAR"));
            RESERVED_KEYWORDS.Add("program", new Token(TokenType.PROGRAM, "PROGRAM"));
            RESERVED_KEYWORDS.Add("int", new Token(TokenType.INTEGER, "INTEGER"));
            RESERVED_KEYWORDS.Add("real", new Token(TokenType.REAL, "REAL"));
            RESERVED_KEYWORDS.Add("string", new Token(TokenType.STRING, "STRING"));
            //RESERVED_KEYWORDS.Add("div", new Token(TokenType.INTEGER_DIV, "DIV"));
            RESERVED_KEYWORDS.Add("function", new Token(TokenType.PROCEDURE, "PROCEDURE"));
            RESERVED_KEYWORDS.Add("return", new Token(TokenType.RETURN, "RETURN"));
            RESERVED_KEYWORDS.Add("if", new Token(TokenType.IF, "IF"));
            RESERVED_KEYWORDS.Add("else", new Token(TokenType.ELSE, "ELSE"));
            RESERVED_KEYWORDS.Add("while", new Token(TokenType.WHILE, "WHILE"));
        }

        /// <summary>
        /// Hàm kiểm tra kết thúc file
        /// </summary>
        /// <returns></returns>
        private bool IsEnd()
        {
            return this.currentChar == ENDCHAR;
        }


        /// <summary>
        /// Hàm bỏ qua khoảng trắng
        /// </summary>
        private void SkipWhiteSpace()
        {
            while (!IsEnd() && Char.IsWhiteSpace(this.currentChar))
            {
                this.AdvanceCurrentChar();
            }
        }

        /// <summary>
        /// Hàm bỏ qua comment
        /// </summary>
        private void SkipComment()
        {
            while (this.currentChar != '\n')
            {
                this.AdvanceCurrentChar();
            }

            this.AdvanceCurrentChar();
        }

        /// <summary>
        /// Lấy token Number (1 là số nguyên, 2 là số thực)
        /// </summary>
        /// <returns></returns>
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

                // Lấy chuỗi số của phần thập phân
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

        /// <summary>
        /// parse string to float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ParseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Hàm trả về token Id (1 là Reserved words, 2 là Identifiers)
        /// </summary>
        /// <returns></returns>
        private Token _id()
        {
            var result = "";

            //Đọc hết chuỗi kí tự
            while (!this.IsEnd() && Char.IsLetterOrDigit(this.currentChar))
            {
                result += currentChar;
                this.AdvanceCurrentChar();
            }

            //Kiếm tra trong RESERVED_KEYWORDS
            //Nếu là RESERVED_KEYWORDS thì trả vè RESERVED_KEYWORDS
            //Ngược lại là trả về token Id
            return RESERVED_KEYWORDS.ContainsKey(result) ? RESERVED_KEYWORDS[result] : new Token(TokenType.ID, result, this.lineno, this.column);
        }

        /// <summary>
        /// Lấy token tiếp theo
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {   //Khi chưa hết file
            while (!this.IsEnd())
            {
                //Nếu là khoảng trắng thì bỏ qua
                if (Char.IsWhiteSpace(this.currentChar))
                {
                    this.SkipWhiteSpace();
                    continue;
                }
                //Nếu là comment thì bỏ qua
                else if (this.currentChar == '#')
                {
                    this.AdvanceCurrentChar();
                    this.SkipComment();
                    continue;
                }
                else if(this.currentChar == '"')
                {
                    return this.GetStringToken();
                }
                else if (this.currentChar == '{')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.BEGIN, "{", this.lineno, this.column);
                }
                else if (this.currentChar == '}')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.END, "}", this.lineno, this.column);
                }
                //Token Id(1 là Reserved words, 2 là Identifiers)
                else if (Char.IsLetter(this.currentChar))
                {
                    return this._id();
                }
                //Token số (integer hoặc real)
                else if (Char.IsDigit(currentChar))
                {
                    return this.Number();
                }
                else if(this.currentChar == '=' && this.Peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.EQUAL, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '!' && this.Peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.NOT_EQUAL, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '>' && this.Peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.GREATER_THAN_OR_EQUAL, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '<' && this.Peek() == '=')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.LESS_THAN_OR_EQUAL, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '>')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.GREATER_THAN, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '<')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.LESS_THAN, currentChar.ToString(), this.lineno, this.column);
                }
                
                //Token assign
                else if (this.currentChar == '=')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.ASSIGN, currentChar.ToString(), this.lineno, this.column);
                }
                //Token End statement
                else if (this.currentChar == ';')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.SEMI, currentChar.ToString(), this.lineno, this.column);
                }
                //Token End program
                else if (this.currentChar == '.')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.DOT, currentChar.ToString(), this.lineno, this.column);
                }
                //Token Colon
                else if (this.currentChar == ':')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.COLON, currentChar.ToString(), this.lineno, this.column);
                }
                //Token Comma
                else if (this.currentChar == ',')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.COMMA, currentChar.ToString(), this.lineno, this.column);
                }
                else if (this.currentChar == '/' && this.Peek() == '/')
                {
                    this.AdvanceCurrentChar();
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.INTEGER_DIV, currentChar.ToString(), this.lineno, this.column);
                }
                //Token Real div
                else if (this.currentChar == '/')
                {
                    this.AdvanceCurrentChar();
                    return new Token(TokenType.REAL_DIV, currentChar.ToString(), this.lineno, this.column);
                }
                //Token operator hoặc endfile
                else
                {
                    var currentOperator = currentChar;
                    AdvanceCurrentChar();

                    return GetTokenOperator(currentOperator);
                }
            }

            return new Token(TokenType.EOF, null, this.lineno, this.column);

        }

        private Token GetStringToken()
        {
            var value = "";
            this.AdvanceCurrentChar();
            while(!this.IsEnd() && this.currentChar!='"')
            {
                value += this.currentChar;
                this.AdvanceCurrentChar();
            }
            this.AdvanceCurrentChar();
            return new Token(TokenType.STRING_CONST, value, this.lineno, this.column);
        }

        /// <summary>
        /// Lấy token là operator
        /// </summary>
        /// <param name="currentOperator"></param>
        /// <returns></returns>
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
            else
            {
                return new Token(TokenType.UNDEFINED, null, this.lineno, this.column);
            }
        }

        /// <summary>
        /// Lấy token số nguyên
        /// </summary>
        /// <returns></returns>
        private string GetInteger()
        {
            string currentTokenVal = "";
            while (char.IsDigit(currentChar))
            {// Khi vẫn còn là số thì cứ đọc tiếp
                currentTokenVal += currentChar;
                AdvanceCurrentChar();
            }
            return currentTokenVal;
        }

        /// <summary>
        /// dịch pos lên 1, cập nhật curChar, dòng hiện tại, cột hiện tại
        /// </summary>
        private void AdvanceCurrentChar()
        {   
            if (this.currentChar == '\n')
            {//Hết dòng
                this.lineno += 1;
                this.column = 0;
            }
            pos++;

            if (pos > text.Length - 1)
            {//Hết file
                currentChar = ENDCHAR;
                return;
            }

            //Cập nhật kí tự tiếp theo
            currentChar = text[pos];
            this.column += 1;
        }

        /// <summary>
        /// Xem trước kí tự tiếp theo (không dịch pos)
        /// </summary>
        /// <returns></returns>
        private char Peek()
        {
            var peekPos = this.pos + 1;
            if (peekPos > this.text.Length - 1)
            {// Kết thúc file
                return ' ';
            }
            else
            {
                return this.text[peekPos];
            }
        }
    }
}
