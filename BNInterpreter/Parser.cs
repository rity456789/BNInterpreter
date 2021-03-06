﻿using System.Collections.Generic;
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
                //throw new Exception();
                this.ShowError(ErrorCode.UNEXPECTED_TOKEN, this.currentToken);
            }
        }

        /// <summary>
        /// Tạo block
        /// </summary>
        /// <returns></returns>
        private Block Block()
        {
            // Một Block có nhiều declarations và một compound
            this.Eat(TokenType.BEGIN);
            List<AST> declarationNodes = this.Declarations();
            Compound CompoundStatementNode = this.CompoundStatement();
            this.Eat(TokenType.END);
            Block node = new Block(declarationNodes, CompoundStatementNode);

            return node;
        }

        /// <summary>
        /// Tạo mảng declaration
        /// </summary>
        /// <returns></returns>
        private List<AST> Declarations()
        {
            // Lấy ra tất cả variable decalration và function declaration
            List<AST> declarations = new List<AST>();

            while (true)
            {
                // Tìm kiếm thấy Variable Declaration
                if (this.currentToken.type == TokenType.VAR)
                {
                    this.Eat(TokenType.VAR);
                    if (this.currentToken.type == TokenType.ID)
                    {
                        List<VariableDeclaration> varDecl = this.VariableDeclaration();
                        declarations.AddRange(varDecl);
                        this.Eat(TokenType.SEMI);
                    }
                }

                // Tìm kiếm thấy Function Declaration
                else if (this.currentToken.type == TokenType.PROCEDURE)
                {
                    ProcedureDeclaration procDecl = this.ProcedureDeclaration();
                    declarations.Add(procDecl);
                }

                // Không tìm thấy Declaration
                else break;
            }

            return declarations;
        }

        /// <summary>
        /// Tạo function declaration
        /// </summary>
        /// <returns></returns>
        private ProcedureDeclaration ProcedureDeclaration()
        {
            // Lấy tên function và tất cả params

            this.Eat(TokenType.PROCEDURE);
            var procName = this.currentToken.value;
            this.Eat(TokenType.ID);
            var _params = new List<Param>();

            // Khi gặp token '(' thì lấy tất cả params trong function cho đến khi gặp ')'
            if (this.currentToken.type == TokenType.LPAREN)
            {
                this.Eat(TokenType.LPAREN);
                _params = this.FormalParameterList();
                this.Eat(TokenType.RPAREN);
            }

            // Trong function là 1 block, tạo một block trong function và init function
            var blockNode = this.Block();
            var procDeclaration = new ProcedureDeclaration(procName, _params, blockNode);

            return procDeclaration;
        }

        /// <summary>
        /// Lấy các list formal params trong function
        /// </summary>
        /// <returns></returns>
        private List<Param> FormalParameterList()
        {
            // Không tìm thấy identifer, có nghĩa là function ko có params
            if (this.currentToken.type != TokenType.ID)
            {
                return new List<Param>();
            }

            // Tạo list các params cùng với type và add vào paramsNodes
            var paramNodes = this.FormalParameters();

            while (this.currentToken.type == TokenType.SEMI)
            {
                this.Eat(TokenType.SEMI);
                paramNodes.AddRange(this.FormalParameters());
            }

            return paramNodes;
        }

        /// <summary>
        /// Lấy các params trong 1 formal params và khởi tạo cùng với type
        /// </summary>
        /// <returns></returns>
        private List<Param> FormalParameters()
        {
            // function(a, b: int; c, d: real)
            // FormalParameters lấy phần 'a, b: int'
            var paramNodes = new List<Param>();
            var paramTokens = new List<Token>();

            // Lấy identifier đầu tiên
            paramTokens.Add(this.currentToken);
            this.Eat(TokenType.ID);

            // Lấy list các identifier params cách nhau bởi dấu phẩy
            while (this.currentToken.type == TokenType.COMMA)
            {
                this.Eat(TokenType.COMMA);
                paramTokens.Add(this.currentToken);
                this.Eat(TokenType.ID);
            }

            // Lấy type của các identifiers trên
            this.Eat(TokenType.COLON);
            var typeNode = this.TypeSpec();

            // Tạo Param với các variable là identifiers cùng với type đã lấy ở trên
            foreach (var paramToken in paramTokens)
            {
                var paramNode = new Param(new Var(paramToken), typeNode);
                paramNodes.Add(paramNode);
            }

            return paramNodes;
        }

        private List<VariableDeclaration> VariableDeclaration()
        {
            // Lấy list và tạo variable khi declaration
            List<Var> varNodes = new List<Var>();
            varNodes.Add(new Var(this.currentToken));
            this.Eat(TokenType.ID);

            // Mỗi variable cách nhau bởi dấu ','
            while (this.currentToken.type == TokenType.COMMA)
            {
                this.Eat(TokenType.COMMA);
                varNodes.Add(new Var(this.currentToken));
                this.Eat(TokenType.ID);
            }

            this.Eat(TokenType.COLON);

            // Lấy type và tạo variable với type tương ứng
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
            // Lấy type, hiện tại là int/real
            Token token = this.currentToken;
            if (this.currentToken.type == TokenType.INTEGER)
            {
                this.Eat(TokenType.INTEGER);
            }
            else if (this.currentToken.type == TokenType.REAL)
            {
                this.Eat(TokenType.REAL);
            }
            else if (this.currentToken.type == TokenType.STRING)
            {
                this.Eat(TokenType.STRING);
            }
            else if (this.currentToken.type == TokenType.BOOL)
            {
                this.Eat(TokenType.BOOL);
            }

            TypeAST node = new TypeAST(token);

            return node;
        }

        private AST Program()
        {
            // Khởi tạo chương trình chính
            this.Eat(TokenType.PROGRAM);
            Var varNode = (Var)this.Variable();
            string programName = varNode.value;

            // Chương trình chính chứa một block
            Block blockNode = this.Block();
            ProgramAST programNode = new ProgramAST(programName, blockNode);

            return programNode;
        }

        private Compound CompoundStatement()
        {
            // Một compound chứa nhiều statement
            List<AST> nodes = this.StatementList();

            // Khởi tạo compound và add các statement vào compound
            Compound root = new Compound();
            foreach (AST node in nodes)
            {
                root.children.Add(node);
            }

            return root;
        }

        private List<AST> StatementList()
        {
            // Lấy list các Statements
            AST node = this.Statement();

            List<AST> results = new List<AST>();
            results.Add(node);

            // Mỗi statement cách nhau bởi dấu ';'
            while (this.currentToken.type == TokenType.SEMI)
            {
                this.Eat(TokenType.SEMI);
                results.Add(this.Statement());
            }

            // Trường hợp statement chỉ chứa identifiers
            if (this.currentToken.type == TokenType.ID)
            {
                this.ShowError(ErrorCode.UNEXPECTED_TOKEN, this.currentToken);
            }

            return results;
        }

        private ProcedureCall ProcCallStatement()
        {
            // Nhận biết được khi nào gọi hàm
            var token = this.currentToken;
            var procName = this.currentToken.value;
            this.Eat(TokenType.ID);
            this.Eat(TokenType.LPAREN);

            // Lấy các params được truyền vào hàm khi gọi
            var actualParams = new List<AST>();

            // Nếu hàm có params
            if (this.currentToken.type != TokenType.RPAREN)
            {
                // Param cũng có thể là một expression
                var node = this.Expression();
                actualParams.Add(node);

                // Mỗi param truyền vào cách nhau bởi dấu ','
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
            // Lấy một statement

            // Nếu thấy dấu hiệu nhận biết một compound
            if (currentToken.type == TokenType.BEGIN)
            {
                return this.CompoundStatement();
            }
            // Nếu thấy dấu hiệu gọi hàm
            else if (currentToken.type == TokenType.ID && this.lexer.currentChar == '(')
            {
                return this.ProcCallStatement();
            }
            // Nếu thấy dấu hiệu identifiers
            else if (this.currentToken.type == TokenType.ID)
            {
                return this.AssignmentStatement();
            }
            else if (this.currentToken.type == TokenType.RETURN)
            {
                this.Eat(TokenType.RETURN);
                return new Return(this.currentToken, this.Expression());
            }
            else if (this.currentToken.type == TokenType.IF)
            {
                this.Eat(TokenType.IF);
                return this.IfStatement();
            }
            else if (this.currentToken.type == TokenType.WHILE)
            {
                this.Eat(TokenType.WHILE);
                return this.WhileStatement();
            }
            else
            {
                return this.Empty();
            }
        }

        private AST WhileStatement()
        {
            this.Eat(TokenType.LPAREN);
            //Lấy biểu thức điều kiện  
            var expression = this.CompareExpression();
            this.Eat(TokenType.RPAREN);

            Block whileBlock = this.Block();
            return new While(expression, whileBlock);
        }

        private AST IfStatement()
        {
            this.Eat(TokenType.LPAREN);
            //Lấy biểu thức điều kiện  
            var expression = this.CompareExpression();
            this.Eat(TokenType.RPAREN);

            Block ifBlock = this.Block();
            Block elseBlock = null;
            if (this.currentToken.type == TokenType.ELSE)
            {
                this.Eat(TokenType.ELSE);
                elseBlock = this.Block();
            }

            return new If(expression, ifBlock, elseBlock);
        }

        private BinOP CompareExpression()
        {
            var leftExpression = this.Expression();
            var op = this.currentToken;
            if (op.IsCompareOperator())
            {
                this.Eat(this.currentToken.type);
            }
            var rightExpression = this.Expression();
            return new BinOP(leftExpression, op, rightExpression);
        }

        private AST AssignmentStatement()
        {
            // Lấy statement thực hiện gán

            AST left = this.Variable();
            Token token = this.currentToken;

            this.Eat(TokenType.ASSIGN);

            AST right = this.Expression();
            AST node = new Assign(left, token, right);

            return node;
        }

        private AST Variable()
        {
            // Lấy variable và tạo identifiers
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
            // Xử lý expression, một expression có nhiều term, giữa mỗi term là operators cộng/trừ
            var node = Term();

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
            // Xử lý term, một term có nhiều factor, giữa mỗi factor là operators nhân/chia_thực/chia_nguyên
            AST node = this.Factor();

            //TO DO: code ngu
            while (
                this.currentToken.type == TokenType.INTEGER_DIV
                || this.currentToken.type == TokenType.MULTIPLE
                || this.currentToken.type == TokenType.REAL_DIV
                || this.currentToken.type == TokenType.INTEGER_MOD
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
                else if (token.type == TokenType.INTEGER_MOD)
                {
                    Eat(TokenType.INTEGER_MOD);
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
            // Xử lý Factor, factor có thể là operators cộng/trừ/"("/ 
            // hoặc là số số_thực/số_nguyên 
            // hoặc là variable
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
            else if (token.type == TokenType.STRING_CONST)
            {
                Eat(TokenType.STRING_CONST);
                return new MyString(token);
            }
            else if (token.type == TokenType.LPAREN)
            {
                Eat(TokenType.LPAREN);
                var node = Expression();
                Eat(TokenType.RPAREN);
                return node;
            }
            else if (currentToken.type == TokenType.ID && this.lexer.currentChar == '(')
            {
                return this.ProcCallStatement();
            }
            else if (currentToken.type == TokenType.BOOL_CONST)
            {
                Eat(TokenType.BOOL_CONST);
                return new MyBool(token);
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
