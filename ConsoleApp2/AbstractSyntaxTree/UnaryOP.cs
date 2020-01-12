using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.AbstractSyntaxTree
{
    class UnaryOP : AST
    {
        public Token op;
        public AST expression;

        public UnaryOP(Token inputOp, AST inputExpression)
        {
            op = inputOp;
            expression = inputExpression;
        }
    }
}
