using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Core.Code;

public class FluentBuilder
{
    private readonly List<ExpressionSyntax> _steps = new();

    public FluentBuilder(string invocationName, bool isStatic)
    {
        if (isStatic)
        {
            var invocation = SyntaxFactory.IdentifierName(invocationName);
            _steps.Add(invocation);
        }
        else
        {
            var invocation = SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(invocationName));
            _steps.Add(invocation);
        }
    }

    public FluentBuilder AddStep(string stepName, params ArgumentSyntax[] arguments)
    {
        var last = _steps.LastOrDefault();
        if (last is null) return this;

        var args = SyntaxFactory.SeparatedList(arguments);

        var step = SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                last,
                SyntaxFactory.IdentifierName(stepName)), SyntaxFactory.ArgumentList(args));

        _steps.Add(step);

        return this;
    }

    public ExpressionStatementSyntax Build(bool isAwait = false)
    {
        var last = _steps.LastOrDefault();
        if (isAwait)
        {
           return SyntaxFactory.ExpressionStatement(SyntaxFactory.AwaitExpression(last));
        }
        return SyntaxFactory.ExpressionStatement(last);
    }
}