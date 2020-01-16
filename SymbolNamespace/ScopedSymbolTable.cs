using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    public class ScopedSymbolTable
    {
        public Dictionary<string, Symbol> symbols;
        public string scopeName;
        public int scopeLevel;
        public ScopedSymbolTable enclosingScope;

        public ScopedSymbolTable(string scopeName, int scopeLevel, ScopedSymbolTable enclosingScope = null)
        {
            this.scopeName = scopeName;
            this.scopeLevel = scopeLevel;
            symbols = new Dictionary<string, Symbol>();
            this.enclosingScope = enclosingScope;
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

        public Symbol Lookup(string name, bool onCurrentScopeOnly = false)
        {
            Console.WriteLine("Lookup " + name + ", Scope name: " + this.scopeName);
            if (symbols.ContainsKey(name))
            {
                return symbols[name];
            }
            else if (!onCurrentScopeOnly && this.enclosingScope != null)
            {
                return this.enclosingScope.Lookup(name);
            }
            else return null;
        }
    }
}
