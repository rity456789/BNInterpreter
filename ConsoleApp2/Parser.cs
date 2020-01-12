using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    using AbstractSyntaxTree;

    class Parser
    {
        private Lexer lexer;
        private Token currentToken;

        public Parser(Lexer inputLexer)
        {
            lexer = inputLexer;
            currentToken = lexer.GetNextToken();
        }

        private Exception error(string exception = "default")
        {
            return new Exception("Invalid Syntax");
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
            else
            {
                throw this.error();
            }
        }

        private AST Program()
        {
            AST node = this.CompoundStatement();
            this.Eat(TokenType.DOT);
            return node;
        }

        private Compound CompoundStatement()
        {
            this.Eat(TokenType.BEGIN);
            List<AST> nodes = this.StatementList();
            this.Eat(TokenType.END);

            Compound root = new Compound();
            foreach (AST node in nodes)
            {
                root.children.Add(node);
            }

            return root;
        }

        private List<AST> StatementList()
        {
            AST node = this.Statement();

            List<AST> results = new List<AST>();
            results.Add(node);

            while (this.currentToken.type == TokenType.SEMI)
            {
                this.Eat(TokenType.SEMI);
                results.Add(this.Statement());
            }

            if (this.currentToken.type == TokenType.ID)
            {
                throw this.error();
            }

            return results;
        }

        private AST Statement()
        {
            AST node;

            if(currentToken.type == TokenType.BEGIN)
            {
                node = this.CompoundStatement();    
            }
            else if (currentToken.type == TokenType.ID)
            {
                node = this.AssignmentStatement();
            }
            else
            {
                node = this.Empty();
            }

            return node;
        }

        private AST AssignmentStatement()
        {
            AST left = this.Variable();
            Token token = this.currentToken;

            this.Eat(TokenType.ASSIGN);

            AST right = this.Expression();
            AST node = new Assign(left, token, right);

            return node;
        }

        private AST Variable()
        {
            AST node = new Var(this.currentToken);
            this.Eat(TokenType.ID);

            return node;
        }

        private AST Empty()
        {
            return new NoOP();
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

        private AST Term()
        {
            AST node = this.Factor();

            //TO DO: code ngu
            while (this.currentToken.type == TokenType.DIVIDE || this.currentToken.type == TokenType.MULTIPLE)
            {
                Token token = this.currentToken;

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

            if (token.type == TokenType.PLUS)
            {
                Eat(TokenType.PLUS);
                var node = new UnaryOP(token, Factor());
                return node;
            }
            else if (token.type == TokenType.MINUS)
            {
                Eat(TokenType.MINUS);
                var node = new UnaryOP(token, Factor());
                return node;
            }
            else if (token.type == TokenType.INTERGER)
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
            else
            {
                AST node = this.Variable();
                return node;
            }
        }


       

        public AST Parse()
        {
            AST node = this.Program();

            if(this.currentToken.type != TokenType.EOF)
            {
                throw this.error();
            }

            return node;
        }
    }
}
