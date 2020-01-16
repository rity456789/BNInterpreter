using AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;
using SymbolNamespace;
using Stack;
using System.Reflection;

namespace BNInterpreter
{

    class Interpreter : NodeVisitor
    {
        // Sử dụng mẫu Visitor để duyệt cây AST và execute tại mỗi node
        private AST tree;
        //public Dictionary<string, object> GLOBAL_SCOPE = new Dictionary<string, object>();
        private CallStack callStack;

        public Interpreter(AST tree)
        {
            this.tree = tree;
            this.callStack = new CallStack();
        }

        /// <summary>
        /// Xử lý Binary Operators cộng/trừ/nhân/chia_nguyên/chia_thực
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object VisitBinOP(BinOP node)
        {
            if (node.op.type == TokenType.PLUS) return Convert.ToSingle(this.Visit(node.left)) + Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.MINUS) return Convert.ToSingle(this.Visit(node.left)) - Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.MULTIPLE) return Convert.ToSingle(this.Visit(node.left)) * Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.INTEGER_DIV) return (int)(Convert.ToSingle(this.Visit(node.left)) / Convert.ToSingle(this.Visit(node.right)));
            else if (node.op.type == TokenType.REAL_DIV) return Convert.ToSingle(this.Visit(node.left)) / Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.EQUAL) return Convert.ToSingle(this.Visit(node.left)) == Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.NOT_EQUAL) return Convert.ToSingle(this.Visit(node.left)) != Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.GREATER_THAN) return Convert.ToSingle(this.Visit(node.left)) > Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.LESS_THAN) return Convert.ToSingle(this.Visit(node.left)) < Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.GREATER_THAN_OR_EQUAL) return Convert.ToSingle(this.Visit(node.left)) >= Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.LESS_THAN_OR_EQUAL) return Convert.ToSingle(this.Visit(node.left)) <= Convert.ToSingle(this.Visit(node.right));
            else throw new Exception("Error visit BinOP");
        }

        /// <summary>
        /// Xử lý khi gặp số
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object VisitNum(Num node)
        {
            return node.value;
        }

        /// <summary>
        /// Xử lý khi gặp Unary operator (Operator đứng trước number/var/func)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int VisitUnaryOP(UnaryOP node)
        {
            TokenType op = node.op.type;
            if (op == TokenType.PLUS)
            {
                return (int)this.Visit(node.expression);
            }
            else if (op == TokenType.MINUS)
            {
                return -(int)this.Visit(node.expression);
            }
            else throw new Exception("Error visit UnaryOP");
        }

        /// <summary>
        /// Xử lý khi gặp một compound
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object VisitCompound(AST node)
        {
            // Visit compound gồm nhiều statement, mỗi child là một statement
            foreach (AST child in ((Compound)node).children)
            {
                if(child.GetType().Name.ToString() == "Return")
                {
                    return this.Visit(child);
                }
                this.Visit(child);
            }

            return 0;
        }

        /// <summary>
        /// Xử lý khi gặp phép assign
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int VisitAssign(AST node)
        {
            // Visit operator assign, chỉ gán bằng value
            Assign AssignNode = (Assign)node;
            Var Variable = (Var)AssignNode.left;

            string VarName = Variable.value;

            ActivationRecord ar = (ActivationRecord)this.callStack.Peek();
            ar.SetItem(VarName, Visit(AssignNode.right));
            return 0;
        }

        /// <summary>
        /// Xử lý khi gặp Variable
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object VisitVar(AST node)
        {
            string VarName = ((Var)node).value;

            ActivationRecord ar = (ActivationRecord)this.callStack.Peek();
            var value = ar.GetItem(VarName);

            return value;
        }

        /// <summary>
        /// Các trường hợp còn lại
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int VisitNoOP(AST node)
        {
            return 0;
        }

        /// <summary>
        /// Xử lý khi bắt đầu chương trình
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int VisitProgramAST(ProgramAST node)
        {
            string programName = node.name;

            ActivationRecord ar = new ActivationRecord(programName, ARTYPE.PROGRAM, 1);
            callStack.Push(ar);

            this.Visit(node.block);

            callStack.Pop();

            return 0;
        }

        /// <summary>
        /// Xử lý khi gặp một block
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object VisitBlock(Block node)
        {
            // Đầu tiên xử lý tất cả declarations
            foreach (AST declaration in node.declarations)
            {
                this.Visit(declaration);
            }


            return this.Visit(node.compoundStatement);
        }

        public int VisitVariableDeclaration(AST node)
        {
            return 0;
        }

        public int VisitTypeAST(AST node)
        {
            return 0;
        }

        public object VisitProcedureCall(ProcedureCall node)
        {
            ActivationRecord curScope = this.callStack.Peek();

            if(curScope.builtinProcs.ContainsKey(node.procName))
            {
               return this.RunBuiltinProc(curScope, node);
            }

            ProcedureDeclaration proc = (ProcedureDeclaration)curScope.GetItem(node.procName);

            ActivationRecord newScope = new ActivationRecord(node.procName, ARTYPE.PROCEDURE, curScope.nestingLevel + 1, curScope);
            for (int i = 0; i < node.actualParams.Count; i++)
            {
                var temp = node.actualParams[i];
                newScope.SetItem(proc._params[i].varNode.token.value, Visit(temp));
            }

            callStack.Push(newScope);
            var result = this.Visit(proc.blockNode);
            callStack.Pop();
            return result;
        }

        private object RunBuiltinProc(ActivationRecord curScope, ProcedureCall node)
        {
            var method = curScope.builtinProcs[node.procName];
            List<object> actual = new List<object>();
            for (int i = 0; i < node.actualParams.Count; i++)
            {
                var temp = node.actualParams[i];
                actual.Add(Visit(temp));
            }

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(method);

            return theMethod.Invoke(this, actual.ToArray());
        }

        public int VisitParam(Param node)
        {
            return 0;
        }

        public int VisitProcedureDeclaration(ProcedureDeclaration node)
        {
            string procName = node.procName;
            var blockNode = node.blockNode;

            ActivationRecord ar = this.callStack.Peek();
            ar.SetItem(procName, node);
            return 0;
        }

        public object VisitReturn(Return node)
        {
            return Visit(node.result);
        }

        public void VisitPrint(object a)
        {
            Console.WriteLine(a);
        }

        public void VisitIf(If node)
        {
            if ((bool)Visit(node.expression))
            {
                this.Visit(node.ifBlock);
            }
            else
            {
                this.Visit(node.elseBlock);
            }
        }

        public void VisitWhile(While node)
        {
            while((bool)Visit(node.expression))
            {
                this.Visit(node.whileBlock);
            }
        }

        public object VisitMyString(MyString node)
        {
            return node.value;
        }

        /// <summary>
        /// Thông dịch chương trình
        /// </summary>
        /// <returns></returns>
        public int Interpret()
        {
            return (int)this.Visit(this.tree);
        }
    }
}
