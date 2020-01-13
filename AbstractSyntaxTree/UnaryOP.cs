using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class UnaryOP : AST
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
