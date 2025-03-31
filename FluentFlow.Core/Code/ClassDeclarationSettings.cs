using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public class ClassDeclarationSettings
{
    public string Name { get; set; } = string.Empty;
    public string Inheritance { get; set; } = string.Empty;
    public List<MemberDeclarationSyntax> Properties { get; set; }
    public List<MemberDeclarationSyntax> Methods { get; set; }
    public List<AttributeSyntax> Attributes { get; set; }
    public MemberDeclarationSyntax? Constructor { get; set; }

    public ClassDeclarationSettings()
    {
        Properties = new List<MemberDeclarationSyntax>();
        Methods = new List<MemberDeclarationSyntax>();
        Attributes = new List<AttributeSyntax>();
    }
}