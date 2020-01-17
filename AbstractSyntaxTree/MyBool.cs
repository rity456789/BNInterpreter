using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class MyBool : AST
    {
        public Token token;
        public bool value;

        public MyBool(Token token)
        {
            this.token = token;
            this.value = token.value == "TRUE" ? true : false;
        }
    }
}
