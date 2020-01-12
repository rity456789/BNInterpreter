using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.AbstractSyntaxTree
{
    class Block : AST
    {
        public List<VariableDeclaration> declarations;
        public Compound compoundStatement;

        public Block(List<VariableDeclaration> inputDeclare, Compound inputCompound)
        {
            declarations = inputDeclare;
            compoundStatement = inputCompound;
        }
    }
}
