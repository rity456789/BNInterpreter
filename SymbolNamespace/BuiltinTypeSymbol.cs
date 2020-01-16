using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    /// <summary>
    /// Class chứa các symbol builtin (INTEGER, REAL)
    /// </summary>
    public class BuiltinTypeSymbol : Symbol
    {
        public BuiltinTypeSymbol(string name) : base(name)
        {
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
