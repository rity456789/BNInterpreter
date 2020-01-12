using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.AbstractSyntaxTree
{
    class TypeAST
    {
        public Token token;
        public string value;

        public TypeAST(Token inputToken)
        {
            token = inputToken;
            value = inputToken.value;
        }
    }
}
