using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class Assign : AST
    {
        public AST left;
        public Token op;
        public AST right;

        public Assign(AST left, Token op, AST right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
}
