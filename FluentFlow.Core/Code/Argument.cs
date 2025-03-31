using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class Argument
{
    public static ArgumentSyntax String(string value)
        => Build(value, SyntaxKind.StringLiteralExpression);

    public static ArgumentSyntax Variable(string value)
        => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(value));

    public static ArgumentSyntax NameOf(string value)
        => SyntaxFactory.Argument(Name.Of(value));

    private static ArgumentSyntax Build(string value, SyntaxKind kind)
        => SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(kind, SyntaxFactory.Literal(value)));
}