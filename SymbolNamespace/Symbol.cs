using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    public class Symbol
    {
        public string name;
        public Symbol type;
        public object category;

        public Symbol(string name, Symbol type = null)
        {
            this.name = name;
            this.type = type;
        }
    }
}
