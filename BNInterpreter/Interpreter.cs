using AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;


namespace BNInterpreter
{

    class Interpreter : NodeVisitor
    {
        // Sử dụng mẫu Visitor để duyệt cây AST và execute tại mỗi node
        private Parser parser;
        public Dictionary<string, object> GLOBAL_SCOPE = new Dictionary<string, object>();

        public Interpreter(Parser parser)
        {
            this.parser = parser;
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
        public int VisitCompound(AST node)
        {
            // Visit compound gồm nhiều statement, mỗi child là một statement
            foreach (AST child in ((Compound)node).children)
            {
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

            if (GLOBAL_SCOPE.ContainsKey(VarName))
            {
                GLOBAL_SCOPE[VarName] = this.Visit(AssignNode.right);
            }
            else
            {
                GLOBAL_SCOPE.Add(VarName, this.Visit(AssignNode.right));
            }

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

            object val = GLOBAL_SCOPE[VarName];

            if (val == null)
            {
                throw new Exception("Variable not in global scope");
            }
            else
            {
                return val;
            }
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
            this.Visit(node.block);
            return 0;
        }

        /// <summary>
        /// Xử lý khi gặp một block
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int VisitBlock(Block node)
        {
            // Đầu tiên xử lý tất cả declarations
            foreach (AST declaration in node.declarations)
            {
                this.Visit(declaration);
            }

            // Sau đó mới thực hiện compound
            this.Visit(node.compoundStatement);

            return 0;
        }

        public int VisitVariableDeclaration(AST node)
        {
            return 0;
        }

        public int VisitTypeAST(AST node)
        {
            return 0;
        }

        public int VisitProcedureCall(ProcedureCall node)
        {
            return 0;
        }
        
        /// <summary>
        /// Thông dịch chương trình
        /// </summary>
        /// <returns></returns>
        public int Interpret()
        {
            var tree = parser.Parse();
            return (int)this.Visit(tree);
        }
    }
}
