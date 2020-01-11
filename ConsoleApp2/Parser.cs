using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Parser
    {
        private Lexer lexer;
        private Token currentToken;

        public Parser(Lexer inputLexer)
        {
            lexer = inputLexer;
            currentToken = lexer.GetNextToken();
        }



        /// <summary>
        /// dời curTOken lên
        /// </summary>
        /// <param name="type"></param>
        private void Eat(TokenType type)
        {
            if (currentToken.type == type)
            {
                currentToken = lexer.GetNextToken();
            }
            else throw new Exception("Error parsing");
        }


        private AST Term()
        {
            var node = Factor();

            //TO DO: code ngu
            while (currentToken.type == TokenType.DIVIDE || currentToken.type == TokenType.MULTIPLE)
            {
                var token = currentToken;

                if (token.type == TokenType.MULTIPLE)
                {
                    Eat(TokenType.MULTIPLE);
                }
                else if (token.type == TokenType.DIVIDE)
                {
                    Eat(TokenType.DIVIDE);
                }

                node = new BinOP(node, token, Factor());

            }

            return node;
        }

        private AST Factor()
        {
            var token = currentToken;
            if (token.type == TokenType.INTERGER)
            {
                Eat(TokenType.INTERGER);
                return new Num(token);
            }
            else if (token.type == TokenType.LPAREN)
            {
                Eat(TokenType.LPAREN);
                var node = Expression();
                Eat(TokenType.RPAREN);
                return node;
            }
            else throw new Exception("Error Factor");
        }


        /// <summary>
        /// Hàm chính để thực thi phiên dịch
        /// </summary>
        /// <returns></returns>
        public AST Expression()
        {
            var node = Term();
            //TO DO: code ngu
            while (currentToken.type == TokenType.PLUS || currentToken.type == TokenType.MINUS)
            {
                var token = currentToken;
                if (token.type == TokenType.PLUS)
                {
                    Eat(TokenType.PLUS);
                }
                else if (token.type == TokenType.MINUS)
                {
                    Eat(TokenType.MINUS);
                }
                node = new BinOP(node, token, Term());
            }
            return node;


        }

        public AST Parse()
        {
            return this.Expression();
        }
    }
}
