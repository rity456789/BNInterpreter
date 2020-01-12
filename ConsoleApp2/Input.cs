using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Input
    {
        public static string Read()
        {
            //return "PROGRAM TestAST; VAR y: REAL; BEGIN  y := 12 DIV 3 DIV 4; END.";
            return "PROGRAM TestAST; VAR a, b : INTEGER; y: REAL; BEGIN {Comment} a := 2; b := 10 * a + 10 * a DIV 4; y := 20 / 7 + 3.14; END. {Comment}";
        }
    }
}
