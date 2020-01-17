using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class MyString: AST
    {
        public Token token;
        public string value;

        public MyString(Token token)
        {
            this.token = token;
            this.value = token.value;
        }
    }
}
