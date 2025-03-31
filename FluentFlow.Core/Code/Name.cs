using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class Name
{
    public static InvocationExpressionSyntax Of(string of)
        =>
            SyntaxFactory.InvocationExpression(
                    SyntaxFactory.IdentifierName(
                        SyntaxFactory.Identifier(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.NameOfKeyword,
                            "nameof",
                            "nameof",
                            SyntaxFactory.TriviaList())))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName(of)))));
}