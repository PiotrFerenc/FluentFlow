using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class AttributeArgument
{
    public static AttributeArgumentSyntax Build(long value) => Build(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value.ToString(), value));

    private static AttributeArgumentSyntax Build(SyntaxKind kind, SyntaxToken literal)
        => SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(kind, literal));
}