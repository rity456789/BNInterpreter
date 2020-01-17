using System.Collections.Generic;

namespace Stack
{
    public enum ARTYPE
    {
        PROGRAM = 0,
        PROCEDURE =1,
    }

    public class ActivationRecord
    {
        public string name;
        public ARTYPE type;
        public int nestingLevel;
        public ActivationRecord parentScope;
        public Dictionary<string, object> members;
        public Dictionary<string, string> builtinProcs;

        public ActivationRecord(string name, ARTYPE type, int nestingLevel, ActivationRecord parentScope = null)
        {
            this.name = name;
            this.type = type;
            this.nestingLevel = nestingLevel;
            this.parentScope = parentScope;
            this.members = new Dictionary<string, object>();
            builtinProcs = new Dictionary<string, string>();
            this.InitBuiltInProcs();
        }

        private void InitBuiltInProcs()
        {           
            members.Add("print", "VisitPrint");
            builtinProcs.Add("print", "VisitPrint");
        }

        public object GetItem(string key)
        {
            if (members.ContainsKey(key))
            {
                return members[key];
            }
            else if (parentScope != null)
            {
                return parentScope.GetItem(key);
            }
            else return null;
        }

        public void SetItem(string key, object value)
        {
            if(members.ContainsKey(key))
            {
                members[key] = value;
            }
            else
            {
                members.Add(key, value);
            }
        }
    }
}
