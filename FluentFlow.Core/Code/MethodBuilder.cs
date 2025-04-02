using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class MethodBuilder
{
    public static MethodDeclarationSyntax Build(Action<MethodBuilderSettings> settings)
    {
        var set = new MethodBuilderSettings();
        settings.Invoke(set);

        return SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.IdentifierName(set.ReturnType),
                    SyntaxFactory.Identifier(set.Name))
                .WithModifiers(SyntaxFactory.TokenList(set.Modifiers.Select(SyntaxFactory.Token)))
                .WithBody(set.Body)
                .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(set.Parameters)));
    }
}