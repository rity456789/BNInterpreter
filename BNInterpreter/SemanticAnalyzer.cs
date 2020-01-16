using AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SymbolNamespace;
using Errors;
using TokenNamespace;

namespace BNInterpreter
{
    /// <summary>
    /// Class có nhiệm vụ kiểm tra các lỗi semantic trong chương trình trước khi thực thi chương trình
    /// </summary>
    class SemanticAnalyzer : NodeVisitor
    {
        private ScopedSymbolTable currentScope;

        public SemanticAnalyzer()
        {
            currentScope = null;
        }

        private void ShowError(ErrorCode errorCode, Token token)
        {
            var message = errorCode.ToString() + " -> " + token.ShowToken();
            var error = new SemanticError(errorCode, token, message);
            error.ShowError();
        }

        /// <summary>
        /// Kiểm tra lỗi chương trình chính
        /// </summary>
        /// <param name="node"></param>
        public void VisitProgramAST(ProgramAST node)
        {
            Console.WriteLine("Enter scope: global");
            var globalScope = new ScopedSymbolTable("global", 1, this.currentScope);

            this.currentScope = globalScope;
            this.Visit(node.block);
            this.currentScope = this.currentScope.enclosingScope;
        }

        /// <summary>
        /// Kiếm tra lỗi trong một block
        /// </summary>
        /// <param name="node"></param>
        public void VisitBlock(Block node)
        {
            // Kiểm tra lỗi ở các declarations
            foreach (AST declaration in node.declarations)
            {
                this.Visit(declaration);
            }

            // Kiểm tra lỗi ở compound
            this.Visit(node.compoundStatement);
        }


        public void VisitBinOP(BinOP node)
        {
            this.Visit(node.left);
            this.Visit(node.right);
        }

        public void VisitNum(Num node)
        {
        }

        public void VisitUnaryOP(UnaryOP node)
        {
            this.Visit(node.expression);
        }

        public void VisitCompound(AST node)
        {
            foreach (AST child in ((Compound)node).children)
            {
                this.Visit(child);
            }
        }

        public void VisitNoOP(AST node)
        {
        }

        public void VisitVariableDeclaration(VariableDeclaration node)
        {
            var typeName = node.typeNode.value;
            var typeSymbol = this.currentScope.Lookup(typeName);

            var varName = node.varNode.value;
            var varSymbol = new VarSymbol(varName, typeSymbol);

            if (this.currentScope.Lookup(varName, true) != null)
            {
                this.ShowError(ErrorCode.DUPLICATE_ID, node.varNode.token);
                //ErrorsHandler.ShowError("Duplicate declare variable: " + varName);
            }

            this.currentScope.Insert(varSymbol);
        }

        public void VisitProcedureDeclaration(ProcedureDeclaration node)
        {
            var procName = node.procName;
            var procSymbol = new ProcedureSymbol(procName);
            this.currentScope.Insert(procSymbol);

            Console.WriteLine("Enter scope: " + procName);
            var procedureScope = new ScopedSymbolTable(procName, this.currentScope.scopeLevel + 1, this.currentScope);
            this.currentScope = procedureScope;

            foreach (var param in node._params)
            {
                var paramType = this.currentScope.Lookup(param.typeNode.value);
                var paramName = param.varNode.value;
                var varSymbol = new VarSymbol(paramName, paramType);
                this.currentScope.Insert(varSymbol);
                procSymbol._params.Add(varSymbol);
            }

            this.Visit(node.blockNode);

            this.currentScope = this.currentScope.enclosingScope;
            Console.WriteLine("Leave scope: " + procName);
        }

        public void VisitAssign(AST node)
        {//con khac
            Assign assignNode = (Assign)node;
            Var variable = (Var)assignNode.left;
            string varName = variable.value;
            var varSymbol = this.currentScope.Lookup(varName);
            if (varSymbol == null)
            {
                
                //ErrorsHandler.ShowError("Do not declare variable name: " + varName);
            }
            this.Visit(assignNode.right);
        }

        public void VisitVar(AST node)
        {
            var varName = ((Var)node).value;
            var varSymbol = this.currentScope.Lookup(varName);

            if (varSymbol == null)
            {
                var varToken = ((Var)node).token;
                this.ShowError(ErrorCode.ID_NOT_FOUND, varToken);
                //ErrorsHandler.ShowError("Do not declare variable name: " + varName);
            }
        }

        public void VisitReturn(Return node)
        {
            Visit(node.result);
        }

        public void VisitPrint(object a)
        {

        }

        public void VisitIf(If node)
        {
            this.Visit(node.expression);
            this.Visit(node.ifBlock);
            this.Visit(node.elseBlock);
        }

        public void VisitWhile(While node)
        {
            this.Visit(node.expression);
            this.Visit(node.whileBlock);
        }

        public void VisitMyString(MyString node)
        {
            
        }

        public void VisitProcedureCall(ProcedureCall node)
        {
            foreach(var paramNode in node.actualParams)
            {
                this.Visit(paramNode);
            }
        }
    }
}
