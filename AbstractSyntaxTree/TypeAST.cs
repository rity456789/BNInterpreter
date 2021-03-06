﻿using TokenNamespace;

namespace AbstractSyntaxTree
{
    public class TypeAST
    {
        public Token token;
        public string value;

        public TypeAST(Token inputToken)
        {
            token = inputToken;
            value = inputToken.value;
        }
    }
}
