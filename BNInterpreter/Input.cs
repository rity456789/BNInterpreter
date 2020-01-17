using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BNInterpreter
{
    class Input
    {
        public static string Read(string inputPath)
        {
            string path = Directory.GetCurrentDirectory() + "\\" + inputPath;

            string content = File.ReadAllText(path, Encoding.UTF8);
            return content;
            //return "PROGRAM TestAST; VAR y: REAL; BEGIN  y := 12 DIV 3 DIV 4; END.";
            //return "PROGRAM TestAST; VAR a, b : INTEGER; y: REAL; BEGIN {Comment} a := 2; b := 10 * a + 10 * a DIV 4; y := 20 / 7 + 3.14; END. {Comment}";
        }
    }
}
