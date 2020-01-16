using System;
using System.Collections.Generic;
using System.Text;

namespace Errors
{
    public class LexerError : Error
    {
        public LexerError(string message) : base(message: message)
        {

        }
    }
}
