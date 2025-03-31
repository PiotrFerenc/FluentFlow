using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class Method
{
    public static BlockSyntax Body(params StatementSyntax[] statements)
    {
        return SyntaxFactory.Block(statements);
    }

    public static BlockSyntax Body(List<StatementSyntax> statements)
    {
        return SyntaxFactory.Block(statements);
    }
}