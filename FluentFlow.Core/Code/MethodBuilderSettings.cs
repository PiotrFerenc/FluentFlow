using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public class MethodBuilderSettings
{
    public string Name { get; set; }
    public string ReturnType { get; set; }
    public List<SyntaxKind> Modifiers { get; set; }
    public BlockSyntax Body { get; set; }
    public List<SyntaxNodeOrToken> Parameters { get; set; } = new List<SyntaxNodeOrToken>();
}