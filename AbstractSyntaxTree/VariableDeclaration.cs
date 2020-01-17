
namespace AbstractSyntaxTree
{
    public class VariableDeclaration : AST
    {
        public Var varNode;
        public TypeAST typeNode;

        public VariableDeclaration(Var inputVarNode, TypeAST inputTypeNode)
        {
            varNode = inputVarNode;
            typeNode = inputTypeNode;
        }
    }
}
