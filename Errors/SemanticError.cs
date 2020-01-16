using System;
using System.Collections.Generic;
using System.Text;
using TokenNamespace;

namespace Errors
{
    public class SemanticError : Error
    {
        public SemanticError(ErrorCode errorCode, Token token, string message) : base(errorCode, token, message)
        {

        }
    }
}
