using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    class SymbolTable
    {
        private Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            symbols = new Dictionary<string, Symbol>();
        }

        public void Define(Symbol symbol)
        {
            symbols[symbol.name] = symbol;
        }

        public Symbol Lookup(string name)
        {
            if (symbols.ContainsKey(name))
            {
                return symbols[name];
            }
            else return null;
        }
    }
}
