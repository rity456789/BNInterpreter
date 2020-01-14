using AbstractSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolNamespace
{
    //public class SymbolTableBuilder : NodeVisitor
    //{
    //    private ScopedSymbolTable symtab;

    //    public SymbolTableBuilder()
    //    {
    //        symtab = new ScopedSymbolTable();
    //    }

    //    public void VisitBlock(Block node)
    //    {
    //        foreach (AST declaration in node.declarations)
    //        {
    //            this.Visit(declaration);
    //        }
    //        this.Visit(node.compoundStatement);
    //    }

    //    public void VisitProgramAST(ProgramAST node)
    //    {
    //        this.Visit(node.block);
    //    }

    //    public void VisitBinOP(BinOP node)
    //    {
    //        this.Visit(node.left);
    //        this.Visit(node.right);
    //    }

    //    public void VisitNum(Num node)
    //    {
    //    }

    //    public void VisitUnaryOP(UnaryOP node)
    //    {
    //        this.Visit(node.expression);
    //    }

    //    public void VisitCompound(AST node)
    //    {
    //        foreach (AST child in ((Compound)node).children)
    //        {
    //            this.Visit(child);
    //        }
    //    }

    //    public void VisitNoOP(AST node)
    //    {
    //    }

    //    public void VisitVariableDeclaration(VariableDeclaration node)
    //    {
    //        var typeName = node.typeNode.value;
    //        var typeSymbol = this.symtab.Lookup(typeName);

    //        var varName = node.varNode.value;
    //        var varSymbol = new VarSymbol(varName, typeSymbol);
    //        this.symtab.Insert(varSymbol);
    //    }

    //    public void VisitProcedureDeclaration(ProcedureDeclaration node)
    //    {

    //    }

    //    public void VisitAssign(AST node)
    //    {
    //        Assign assignNode = (Assign)node;
    //        Var variable = (Var)assignNode.left;
    //        string varName = variable.value;
    //        var varSymbol = this.symtab.Lookup(varName);
    //        if(varSymbol == null)
    //        {
    //            Console.WriteLine("Do not declare variable name: " + varName);
    //            Environment.Exit(0);
    //        }
    //        this.Visit(assignNode.right);
    //    }
        
    //    public void VisitVar(AST node)
    //    {
    //        var varName = ((Var)node).value;
    //        var varSymbol = this.symtab.Lookup(varName);

    //        if(varSymbol == null)
    //        {
    //            Console.WriteLine("Do not declare variable name: " + varName);
    //            Environment.Exit(0);
    //        }
    //    }
    //}
}
