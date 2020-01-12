using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace ConsoleApp2
{
    using AbstractSyntaxTree;

    class NodeVisitor
    {

        //TO DO: do not implement
        public int Visit(AST node)
        {
            AST[] parameters = new AST[1];
            parameters[0] = node;

            Type nodeType = node.GetType();
            Type thisType = this.GetType();

            string method = "Visit" + nodeType.Name;
            MethodInfo theMethod = thisType.GetMethod(method);
            return (int)theMethod.Invoke(this, parameters);
        }

        public void GenericVisit(AST node)
        {
            throw new Exception("No visit method");
        }
    }
}
