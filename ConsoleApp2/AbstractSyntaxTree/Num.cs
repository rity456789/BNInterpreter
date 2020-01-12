using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.AbstractSyntaxTree
{
    class Num : AST
    {
        public Token token;
        public string value;

        public Num(Token token)
        {
            this.token = token;
            this.value = token.value;
        }
    }
}
