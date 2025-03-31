using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class ConstructorBuilder
{
    public static ConstructorDeclarationSyntax Build(Action<ConstructorSettings> settings)
    {
        var set = new ConstructorSettings();
        settings.Invoke(set);

        return SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier(set.Name))
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(
                set.Parameters)))
            .WithBody(set.Body);
    }
}
