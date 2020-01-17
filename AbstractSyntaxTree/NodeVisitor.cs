using System;
using System.Reflection;

namespace AbstractSyntaxTree
{
    public class NodeVisitor
    {

        //TO DO: do not implement
        public object Visit(AST node)
        {
            AST[] parameters = new AST[1];
            parameters[0] = node;

            Type nodeType = node.GetType();
            Type thisType = this.GetType();

            string method = "Visit" + nodeType.Name;
            MethodInfo theMethod = thisType.GetMethod(method);
            return theMethod.Invoke(this, parameters);
        }

        public void GenericVisit(AST node)
        {
            throw new Exception("No visit method");
        }
    }
}
