using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    public class SymbolTable
    {
        private Dictionary<string, Symbol> symbols;

        public SymbolTable()
        {
            symbols = new Dictionary<string, Symbol>();
            this.InitBuiltIns();
        }

        private void InitBuiltIns()
        {
            this.Insert(new BuiltinTypeSymbol("INTEGER"));
            this.Insert(new BuiltinTypeSymbol("REAL"));
        }

        //public void Define(Symbol symbol)
        //{
        //    Console.WriteLine("Define " + symbol.name);
        //    symbols[symbol.name] = symbol;
        //}

        public void Insert(Symbol symbol)
        {
            Console.WriteLine("Insert " + symbol.name);
            symbols[symbol.name] = symbol;
        }

        public Symbol Lookup(string name)
        {
            Console.WriteLine("Lookup " + name);
            if (symbols.ContainsKey(name))
            {
                return symbols[name];
            }
            else return null;
        }
    }
}
