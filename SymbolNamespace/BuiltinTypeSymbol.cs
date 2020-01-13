using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    class BuiltinTypeSymbol : Symbol
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
