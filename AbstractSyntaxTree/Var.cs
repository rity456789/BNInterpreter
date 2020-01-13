using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class Var : AST
    {
        public Token token;
        public string value;

        public Var(Token inputToken)
        {
            this.token = inputToken;
            this.value = inputToken.value;
        }
    }
}
