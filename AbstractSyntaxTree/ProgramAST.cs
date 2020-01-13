﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractSyntaxTree
{
    public class ProgramAST : AST
    {
        public string name;
        public Block block;

        public ProgramAST(string inputName, Block inputBlock)
        {
            name = inputName;
            block = inputBlock;
        }
    }
}