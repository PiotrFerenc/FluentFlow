using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public static class ParameterBuilder
{
    public static ParameterSyntax Build(string name, PropertyConst.CSharpTypes kind) =>
        SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
            .WithType(SyntaxFactory.IdentifierName(PropertyConst.TypeNames[kind]));

    public static ParameterSyntax Build(string name, string kind) =>
        SyntaxFactory.Parameter(SyntaxFactory.Identifier(name))
            .WithType(SyntaxFactory.IdentifierName(kind));
}