using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class VariableDeclaration : AST
    {
        public Var varNode;
        public TypeAST typeNode;

        public VariableDeclaration(Var inputVarNode, TypeAST inputTypeNode)
        {
            varNode = inputVarNode;
            typeNode = inputTypeNode;
        }
    }
}
