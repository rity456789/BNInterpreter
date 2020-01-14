using System;

namespace Errors
{
    public class ErrorsHandler
    {
        public static void ShowError(string error)
        {
            Console.WriteLine(error);
            Environment.Exit(0);
        }
    }
}
