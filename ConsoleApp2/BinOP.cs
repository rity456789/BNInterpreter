﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class BinOP : AST
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