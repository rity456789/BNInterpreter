
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
