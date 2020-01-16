using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
