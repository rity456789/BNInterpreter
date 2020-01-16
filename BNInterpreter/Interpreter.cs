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
        private Parser parser;
        public Dictionary<string, object> GLOBAL_SCOPE = new Dictionary<string, object>();

        public Interpreter(Parser parser)
        {
            this.parser = parser;
        }

        public object VisitBinOP(BinOP node)
        {
            if (node.op.type == TokenType.PLUS) return Convert.ToSingle(this.Visit(node.left)) + Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.MINUS) return Convert.ToSingle(this.Visit(node.left)) - Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.MULTIPLE) return Convert.ToSingle(this.Visit(node.left)) * Convert.ToSingle(this.Visit(node.right));
            else if (node.op.type == TokenType.INTEGER_DIV) return (int)(Convert.ToSingle(this.Visit(node.left)) / Convert.ToSingle(this.Visit(node.right)));
            else if (node.op.type == TokenType.REAL_DIV) return Convert.ToSingle(this.Visit(node.left)) / Convert.ToSingle(this.Visit(node.right));
            else throw new Exception("Error visit BinOP");
        }

        public object VisitNum(Num node)
        {
            return node.value;
        }

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

        public int VisitCompound(AST node)
        {
            foreach (AST child in ((Compound)node).children)
            {
                this.Visit(child);
            }

            return 0;
        }

        public int VisitAssign(AST node)
        {
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

        public int VisitNoOP(AST node)
        {
            return 0;
        }

        public int Interpret()
        {
            var tree = parser.Parse();
            return (int)this.Visit(tree);
        }

        public int VisitProgramAST(ProgramAST node)
        {
            this.Visit(node.block);
            return 0;
        }

        public int VisitBlock(Block node)
        {
            foreach (AST declaration in node.declarations)
            {
                this.Visit(declaration);
            }
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
    }
}
