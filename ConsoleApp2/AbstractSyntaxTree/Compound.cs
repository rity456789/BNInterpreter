using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.AbstractSyntaxTree
{
    class Compound : AST
    {
        // BEGIN .. END block

        public List<AST> children;
        public Compound()
        {
            this.children = new List<AST>();
        }
    }
}
