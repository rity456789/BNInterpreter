
namespace AbstractSyntaxTree
{
    public class Param : AST
    {
        public Var varNode;
        public TypeAST typeNode;

        public Param(Var varNode, TypeAST typeNode)
        {
            this.varNode = varNode;
            this.typeNode = typeNode;
        }
    }
}
