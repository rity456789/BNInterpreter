using System.Collections.Generic;

namespace AbstractSyntaxTree
{
    public class ProcedureDeclaration : AST
    {
        public string procName;
        public List<Param> _params;
        public Block blockNode;

        public ProcedureDeclaration(string inputprocName, List<Param> _params, Block inputblockNode)
        {
            this._params = _params;
            procName = inputprocName;
            blockNode = inputblockNode;
        }
    }
}
