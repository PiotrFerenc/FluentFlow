using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public class ConstructorSettings
{
    public string Name { get; set; }
    public BlockSyntax Body { get; set; }
    public SyntaxNodeOrToken[] Parameters { get; set; }
}