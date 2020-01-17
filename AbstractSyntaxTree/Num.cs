using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class Num : AST
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
