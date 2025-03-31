using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class ClassAttributeBuilder
{
    public static AttributeSyntax Build(string name, AttributeArgumentSyntax value)
    {
        return
            SyntaxFactory.Attribute(
                    SyntaxFactory.IdentifierName(name))
                .WithArgumentList(
                    SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(value)));
    }
}