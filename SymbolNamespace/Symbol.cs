﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SymbolNamespace
{
    /// <summary>
    /// Class Symbol (thực ra chỉ là class ảo)
    /// </summary>
    public class Symbol
    {
        public string name;
        public Symbol type;

        public Symbol(string name, Symbol type = null)
        {
            this.name = name;
            this.type = type;
        }
    }
}
