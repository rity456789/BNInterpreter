using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class BinOP : AST
    {
        public AST left;
        public Token op;
        public AST right;

        public BinOP(AST left, Token op, AST right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }
}
