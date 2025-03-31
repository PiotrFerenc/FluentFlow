using Microsoft.CodeAnalysis.CSharp;

namespace FluentFlow.Core.Code;

public class PropertySettings
{
    public string Name { get; set; }
    public string Type { get; set; }

    public List<SyntaxKind> Modifiers { get; set; } = new List<SyntaxKind>()
    {
        SyntaxKind.PublicKeyword
    };

    public SyntaxKind ModifierSetProperty { get; set; } = SyntaxKind.None;
    public SyntaxKind ModifierGetProperty { get; set; } = SyntaxKind.None;
}