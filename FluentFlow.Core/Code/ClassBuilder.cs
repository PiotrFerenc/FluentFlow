using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class ClassBuilder
{
    public static ClassDeclarationSyntax Build(Action<ClassDeclarationSettings> settings)
    {
        var set = new ClassDeclarationSettings();

        settings.Invoke(set);
        var members = new List<MemberDeclarationSyntax>();

        var result = SyntaxFactory
            .ClassDeclaration(set.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));


        if (set.Properties.Any())
        {
            members.AddRange(set.Properties);
        }

        if (set.Constructor is not null)
        {
            members.Add(set.Constructor);
        }

        if (set.Methods.Any())
        {
            members.AddRange(set.Methods);
        }

        result = result.WithMembers(SyntaxFactory.List(members));

        if (!string.IsNullOrWhiteSpace(set.Inheritance))
        {
            result = result.WithBaseList(SyntaxFactory.BaseList(
                SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName(set.Inheritance)))));
        }

        if (set.Attributes.Any())
        {
            result = result.WithAttributeLists(SyntaxFactory.SingletonList(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(set.Attributes))));
        }


        return result;
    }
}