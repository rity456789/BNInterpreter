using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp2
{

    class Interpreter : NodeVisitor
    {
        private Parser parser;

        public Interpreter(Parser parser)
        {
            this.parser = parser;   
        }

        private int VisitBinOP(BinOP node)
        {
            if (node.op.type == TokenType.PLUS) return this.Visit(node.left) + this.Visit(node.right);
            else if (node.op.type == TokenType.MINUS) return this.Visit(node.left) - this.Visit(node.right);
            else if (node.op.type == TokenType.MULTIPLE) return this.Visit(node.left) * this.Visit(node.right);
            else if (node.op.type == TokenType.DIVIDE) return this.Visit(node.left) / this.Visit(node.right);
            else throw new Exception("Error visit BinOP");
        }

        private int VisitNum(Num node)
        {
            return Int32.Parse(node.value);
        }

        private int VisitUnaryOP(UnaryOP node)
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

        private int Visit(AST node)
        {
            if (node.GetType().Name == "Num") return VisitNum((Num)node);
            else if (node.GetType().Name == "BinOP") return VisitBinOP((BinOP)node);
            else if (node.GetType().Name == "UnaryOP") return VisitUnaryOP((UnaryOP)node);
            else throw new Exception("Unsigned type of AST");
        }

        public int Interpret()
        {
            var tree = parser.Parse();
            return this.Visit(tree);
        }
    }
}
