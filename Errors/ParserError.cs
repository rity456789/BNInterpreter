using TokenNamespace;

namespace Errors
{
    public class ParserError : Error
    {
        public ParserError(ErrorCode errorCode, Token token, string message):base(errorCode, token, message)
        {

        }
    }
}
