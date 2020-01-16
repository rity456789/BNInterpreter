using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class ProcedureCall : AST
    {
        public string procName;
        public List<AST> actualParams;
        public Token token;

        public ProcedureCall(string procName, List<AST> actualParams, Token token)
        {
            this.procName = procName;
            this.actualParams = actualParams;
            this.token = token;
        }
    }
}
