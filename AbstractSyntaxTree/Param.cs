using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class Param : AST
    {
        public Var varNode;
        public TypeAST typeNode;

        public Param(Var varNode, TypeAST typeNode)
        {
            this.varNode = varNode;
            this.typeNode = typeNode;
        }
    }
}
