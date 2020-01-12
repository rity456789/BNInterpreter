using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp2
{
    using AbstractSyntaxTree;

    class Interpreter : NodeVisitor
    {
        private Parser parser;
        public Dictionary<string, object> GLOBAL_SCOPE = new Dictionary<string, object>();

        public Interpreter(Parser parser)
        {
            this.parser = parser;   
        }

        public int VisitBinOP(BinOP node)
        {
            if (node.op.type == TokenType.PLUS) return this.Visit(node.left) + this.Visit(node.right);
            else if (node.op.type == TokenType.MINUS) return this.Visit(node.left) - this.Visit(node.right);
            else if (node.op.type == TokenType.MULTIPLE) return this.Visit(node.left) * this.Visit(node.right);
            else if (node.op.type == TokenType.DIVIDE) return this.Visit(node.left) / this.Visit(node.right);
            else throw new Exception("Error visit BinOP");
        }

        public int VisitNum(Num node)
        {
            return Int32.Parse(node.value);
        }

        public int VisitUnaryOP(UnaryOP node)
        {
            TokenType op = node.op.type;
            if (op == TokenType.PLUS)
            {
                return this.Visit(node.expression);
            }
            else if (op == TokenType.MINUS)
            {
                return -this.Visit(node.expression);
            }
            else throw new Exception("Error visit UnaryOP");
        }

        //private int Visit(AST node)
        //{
        //    if (node.GetType().Name == "Num") return VisitNum((Num)node);
        //    else if (node.GetType().Name == "BinOP") return VisitBinOP((BinOP)node);
        //    else if (node.GetType().Name == "UnaryOP") return VisitUnaryOP((UnaryOP)node);
        //    else throw new Exception("Unsigned type of AST");
        //}

        public int VisitCompound(AST node)
        {
            foreach(AST child in ((Compound)node).children)
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
            if(GLOBAL_SCOPE.ContainsKey(VarName))
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

            if(val == null)
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
            return this.Visit(tree);
        }
    }
}
