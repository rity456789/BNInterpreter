using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    /// <summary>
    /// Class lưu danh sách các symbol (gồm builtin symbol và các biến được khai báo) hiện tại có trong scope
    /// </summary>
    public class ScopedSymbolTable
    {
        public Dictionary<string, Symbol> symbols;  //Danh sách các symbol
        public string scopeName;                    //Tên của scope hiện tại (global hoặc tên của proc)
        public int scopeLevel;                      //Cấp độ hiện tại của scope (global là 1)
        public ScopedSymbolTable enclosingScope;    //Scope cha

        public ScopedSymbolTable(string scopeName, int scopeLevel, ScopedSymbolTable enclosingScope = null)
        {
            this.scopeName = scopeName;
            this.scopeLevel = scopeLevel;
            symbols = new Dictionary<string, Symbol>();
            this.enclosingScope = enclosingScope;
            this.InitBuiltIns();
        }

        /// <summary>
        /// Tạo các builtin symbol
        /// </summary>
        private void InitBuiltIns()
        {
            this.Insert(new BuiltinTypeSymbol("INTEGER"));
            this.Insert(new BuiltinTypeSymbol("REAL"));
            this.Insert(new BuiltinTypeSymbol("STRING"));
            this.Insert(new BuiltinTypeSymbol("BOOL"));
            //this.Insert(new ProcedureSymbol("PRINT"))
        }

        /// <summary>
        /// Thêm 1 symbol mới vào trong scope
        /// </summary>
        /// <param name="symbol"></param>
        public void Insert(Symbol symbol)
        {
            //Console.WriteLine("Insert " + symbol.name);
            symbols[symbol.name] = symbol;
        }

        /// <summary>
        /// Tìm kiếm 1 symbol trong scope
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onCurrentScopeOnly"></param>
        /// <returns></returns>
        public Symbol Lookup(string name, bool onCurrentScopeOnly = false)
        {
            //Console.WriteLine("Lookup " + name + ", Scope name: " + this.scopeName);
            if (symbols.ContainsKey(name))
            {// Nếu có symbol trong scope
                return symbols[name];
            }
            else if (!onCurrentScopeOnly && this.enclosingScope != null)
            {// Nếu không có trong scope hiện tại và được phép sử dụng biến trong scope cha
                //Vào scope cha tìm tiếp
                return this.enclosingScope.Lookup(name);
            }
            else return null;   // Không tìm thấy
        }
    }
}
