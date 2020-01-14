using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class Block : AST
    {
        public List<AST> declarations;
        public Compound compoundStatement;

        public Block(List<AST> inputDeclare, Compound inputCompound)
        {
            declarations = inputDeclare;
            compoundStatement = inputCompound;
        }
    }
}
