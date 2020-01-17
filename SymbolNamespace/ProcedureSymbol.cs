using AbstractSyntaxTree;
using System;
using System.Collections.Generic;

namespace SymbolNamespace
{
    /// <summary>
    /// Class lưu các hàm được khai báo
    /// </summary>
    public class ProcedureSymbol : Symbol
    {
        public List<VarSymbol> _params;
        public ProcedureSymbol(string name, List<VarSymbol> _params = null) : base(name)
        {
            this._params = _params == null ? new List<VarSymbol>() : _params;
        }
    }
}
