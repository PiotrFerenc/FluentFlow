using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class NameOfBuilder
{
    public static InvocationExpressionSyntax Build(Action<NameOfSettings> settings)
    {
        var set = new NameOfSettings();
        settings.Invoke(set);

        var result = SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.NameOfKeyword,
                        "nameof",
                        "nameof",
                        SyntaxFactory.TriviaList())))
            .WithArgumentList(
                SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(set.Type)
                        ))));

        return result;
    }


    public class NameOfSettings
    {
        public string Type { get; set; }
    }
}