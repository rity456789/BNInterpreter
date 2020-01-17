using System.Collections.Generic;

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
