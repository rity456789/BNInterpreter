using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    class VarSymbol : Symbol
    {
        public VarSymbol(string name, Symbol type) : base(name, type)
        {
        }

        public string GetString()
        {
            return '<' + this.name + ':' + this.type.name + '>';
        }
    }
}
