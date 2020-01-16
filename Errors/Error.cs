using System;
using System.Collections.Generic;
using System.Text;
using TokenNamespace;

namespace Errors
{
    public enum ErrorCode
    {
        UNEXPECTED_TOKEN = 0,
        ID_NOT_FOUND = 1,
        DUPLICATE_ID = 2,

        UNSIGNED = 99
    }


    public class Error
    {
        public ErrorCode errorCode;
        public Token token;
        public string message;

        public Error(ErrorCode errorCode = ErrorCode.UNSIGNED, Token token =null, string message=null)
        {
            this.errorCode = errorCode;
            this.token = token;
            Type type = this.GetType();
            this.message = type.Name.ToString() + ": " + message;
        }

        public void ShowError()
        {
            Console.WriteLine(this.message);
            Environment.Exit(0);
        }
    }
}
