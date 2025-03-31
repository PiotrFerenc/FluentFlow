using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class PropertyBuilder
{
    public static PropertyDeclarationSyntax Build(Action<PropertySettings> settings)
    {
        var set = new PropertySettings();
        settings.Invoke(set);
        var accessors = new List<AccessorDeclarationSyntax>();
        var setaccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        if (set.ModifierSetProperty is not SyntaxKind.None)
        {
            setaccessor = setaccessor.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(set.ModifierSetProperty)));
        }

        accessors.Add(setaccessor);
        var getaccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        if (set.ModifierSetProperty is not SyntaxKind.None)
        {
            getaccessor = getaccessor.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(set.ModifierGetProperty)));
        }

        accessors.Add(getaccessor);

        var declaration = SyntaxFactory.PropertyDeclaration(
            SyntaxFactory.IdentifierName(set.Type),
            SyntaxFactory.Identifier(set.Name));

        declaration = declaration.WithModifiers(SyntaxFactory.TokenList(set.Modifiers.Select(SyntaxFactory.Token)));

        declaration = declaration.WithAccessorList(
            SyntaxFactory.AccessorList(
                SyntaxFactory.List(accessors)));
        return declaration;
    }
}