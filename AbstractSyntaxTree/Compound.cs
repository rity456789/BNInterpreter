using System.Collections.Generic;

namespace AbstractSyntaxTree
{
    public class Compound : AST
    {
        // BEGIN .. END block

        public List<AST> children;
        public Compound()
        {
            this.children = new List<AST>();
        }
    }
}
