using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public class FieldBuilderSettings
{
    public string Name { get; set; }
    public string Type { get; set; }

    public List<SyntaxKind> Modifiers { get; set; } = new List<SyntaxKind>()
    {
        SyntaxKind.PublicKeyword
    };
}

public static class FieldBuilder
{
    public static FieldDeclarationSyntax Build(Action<FieldBuilderSettings> settings)
    {
        var set = new FieldBuilderSettings();
        settings.Invoke(set);


        var declaration = SyntaxFactory.FieldDeclaration(
            SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName(set.Type)
                )
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(set.Name)))));


        declaration = declaration.WithModifiers(SyntaxFactory.TokenList(set.Modifiers.Select(SyntaxFactory.Token)));


        return declaration;
    }
}