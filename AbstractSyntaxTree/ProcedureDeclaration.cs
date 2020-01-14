using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class ProcedureDeclaration : AST
    {
        public string procName;
        public Block blockNode;

        public ProcedureDeclaration(string inputprocName, Block inputblockNode)
        {
            procName = inputprocName;
            blockNode = inputblockNode;
        }
    }
}
