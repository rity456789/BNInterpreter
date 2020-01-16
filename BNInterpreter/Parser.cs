using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;
using Errors;

namespace BNInterpreter
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

        private void ShowError(ErrorCode errorCode, Token token)
        {
            var message = errorCode.ToString() + " -> " + token.ShowToken();
            var error = new ParserError(errorCode, token, message);
            error.ShowError();
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
                this.ShowError(ErrorCode.UNEXPECTED_TOKEN, this.currentToken);
            }
        }

        private Block Block()
        {
            List<AST> declarationNodes = this.Declarations();
            Compound CompoundStatementNode = this.CompoundStatement();
            Block node = new Block(declarationNodes, CompoundStatementNode);

            return node;
        }


        private List<AST> Declarations()
        {
            List<AST> declarations = new List<AST>();

            while (true)
            {
                if (this.currentToken.type == TokenType.VAR)
                {
                    this.Eat(TokenType.VAR);
                    while (this.currentToken.type == TokenType.ID)
                    {
                        List<VariableDeclaration> varDecl = this.VariableDeclaration();
                        declarations.AddRange(varDecl);
                        this.Eat(TokenType.SEMI);
                    }
                }

                else if (this.currentToken.type == TokenType.PROCEDURE)
                {
                    ProcedureDeclaration procDecl = this.ProcedureDeclaration();
                    declarations.Add(procDecl);
                }
                else break;
            }
            return declarations;
        }

        private ProcedureDeclaration ProcedureDeclaration()
        {
            this.Eat(TokenType.PROCEDURE);
            var procName = this.currentToken.value;
            this.Eat(TokenType.ID);
            var _params = new List<Param>();

            if (this.currentToken.type == TokenType.LPAREN)
            {
                this.Eat(TokenType.LPAREN);
                _params = this.FormalParameterList();
                this.Eat(TokenType.RPAREN);
            }

            this.Eat(TokenType.SEMI);
            var blockNode = this.Block();
            var procDeclaration = new ProcedureDeclaration(procName, _params, blockNode);
            this.Eat(TokenType.SEMI);
            return procDeclaration;
        }

        private List<Param> FormalParameterList()
        {
            if (this.currentToken.type != TokenType.ID)
            {
                return new List<Param>();
            }

            var paramNodes = this.FormalParameters();

            while (this.currentToken.type == TokenType.SEMI)
            {
                this.Eat(TokenType.SEMI);
                paramNodes.AddRange(this.FormalParameters());
            }
            return paramNodes;
        }

        private List<Param> FormalParameters()
        {
            var paramNodes = new List<Param>();
            var paramTokens = new List<Token>();
            paramTokens.Add(this.currentToken);
            this.Eat(TokenType.ID);
            while (this.currentToken.type == TokenType.COMMA)
            {
                this.Eat(TokenType.COMMA);
                paramTokens.Add(this.currentToken);
                this.Eat(TokenType.ID);
            }
            this.Eat(TokenType.COLON);
            var typeNode = this.TypeSpec();

            foreach (var paramToken in paramTokens)
            {
                var paramNode = new Param(new Var(paramToken), typeNode);
                paramNodes.Add(paramNode);
            }
            return paramNodes;
        }

        private List<VariableDeclaration> VariableDeclaration()
        {
            List<Var> varNodes = new List<Var>();
            varNodes.Add(new Var(this.currentToken));
            this.Eat(TokenType.ID);

            while (this.currentToken.type == TokenType.COMMA)
            {
                this.Eat(TokenType.COMMA);
                varNodes.Add(new Var(this.currentToken));
                this.Eat(TokenType.ID);
            }

            this.Eat(TokenType.COLON);

            TypeAST typeNode = this.TypeSpec();
            List<VariableDeclaration> varDeclarations = new List<VariableDeclaration>();
            foreach (Var varNode in varNodes)
            {
                varDeclarations.Add(new VariableDeclaration(varNode, typeNode));
            }

            return varDeclarations;
        }

        private TypeAST TypeSpec()
        {
            Token token = this.currentToken;
            if (this.currentToken.type == TokenType.INTEGER)
            {
                this.Eat(TokenType.INTEGER);
            }
            else
            {
                this.Eat(TokenType.REAL);
            }

            TypeAST node = new TypeAST(token);

            return node;
        }

        private AST Program()
        {
            this.Eat(TokenType.PROGRAM);
            Var varNode = (Var)this.Variable();
            string programName = varNode.value;

            this.Eat(TokenType.SEMI);
            Block blockNode = this.Block();
            ProgramAST programNode = new ProgramAST(programName, blockNode);

            this.Eat(TokenType.DOT);
            return programNode;
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
                this.ShowError(ErrorCode.UNEXPECTED_TOKEN, this.currentToken);
            }

            return results;
        }

        private ProcedureCall ProcCallStatement()
        {
            var token = this.currentToken;
            var procName = this.currentToken.value;
            this.Eat(TokenType.ID);
            this.Eat(TokenType.LPAREN);

            var actualParams = new List<AST>();

            if (this.currentToken.type != TokenType.RPAREN)
            {
                var node = this.Expression();
                actualParams.Add(node);

                while (this.currentToken.type == TokenType.COMMA)
                {
                    this.Eat(TokenType.COMMA);
                    node = this.Expression();
                    actualParams.Add(node);
                }
            }

            this.Eat(TokenType.RPAREN);

            return new ProcedureCall(procName, actualParams, token);

        }

        private AST Statement()
        {
            if (currentToken.type == TokenType.BEGIN)
            {
                return this.CompoundStatement();
            }
            else if (currentToken.type == TokenType.ID && this.lexer.currentChar == '(')
            {
                return this.ProcCallStatement();
            }
            else if (this.currentToken.type == TokenType.ID)
            {
                return this.AssignmentStatement();
            }
            else
            {
                return this.Empty();
            }
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
            while (
                this.currentToken.type == TokenType.INTEGER_DIV
                || this.currentToken.type == TokenType.MULTIPLE
                || this.currentToken.type == TokenType.REAL_DIV
                )
            {
                Token token = this.currentToken;

                if (token.type == TokenType.MULTIPLE)
                {
                    Eat(TokenType.MULTIPLE);
                }
                else if (token.type == TokenType.INTEGER_DIV)
                {
                    Eat(TokenType.INTEGER_DIV);
                }
                else if (token.type == TokenType.REAL_DIV)
                {
                    Eat(TokenType.REAL_DIV);
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
            else if (token.type == TokenType.INTEGER_CONST)
            {
                Eat(TokenType.INTEGER_CONST);
                return new Num(token);
            }
            else if (token.type == TokenType.REAL_CONST)
            {
                Eat(TokenType.REAL_CONST);
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

            if (this.currentToken.type != TokenType.EOF)
            {
                this.ShowError(ErrorCode.UNEXPECTED_TOKEN, this.currentToken);
            }

            return node;
        }
    }
}
