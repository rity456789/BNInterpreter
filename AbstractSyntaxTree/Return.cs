using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class Return :AST
    {
        public Token token;
        public AST result;

        public Return(Token token, AST result)
        {
            this.token = token;
            this.result = result;
        }
    }
}
