
namespace AbstractSyntaxTree
{
    public class While : AST
    {
        public BinOP expression;
        public Block whileBlock;

        public While(BinOP expression, Block whileBlock)
        {
            this.whileBlock = whileBlock;
            this.expression = expression;
        }
    }
}
