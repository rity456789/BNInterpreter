using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class If : AST
    {
        public BinOP expression;
        public Block ifBlock;
        public Block elseBlock; 

        public If(BinOP expression, Block ifBlock, Block elseBlock)
        {
            this.ifBlock = ifBlock;
            this.expression = expression;
            this.elseBlock = elseBlock;
        }
    }
}
