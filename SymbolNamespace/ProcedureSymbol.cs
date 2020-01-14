using AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    public class ProcedureSymbol : Symbol
    {
        public List<VarSymbol> _params;
        public ProcedureSymbol(string name, List<VarSymbol> _params = null) : base(name)
        {
            this._params = _params == null ? new List<VarSymbol>() : _params;
        }


    }
}
