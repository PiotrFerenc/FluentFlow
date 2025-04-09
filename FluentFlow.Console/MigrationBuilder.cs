using FluentFlow.Console.Model;
using FluentFlow.Core;
using FluentFlow.Core.Code;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Console;

public static class MigrationBuilder
{
    public static ClassDeclarationSyntax Build(MigrationOptions migrationOptions, ExpressionStatementSyntax upMethod, Identification identification)
    {
        return ClassBuilder.Build(x =>
        {
            x.Name = $"{migrationOptions.TableName.Pascalize()}{FluentFlowConsts.Migration}";
            x.Inheritance = FluentFlowConsts.Migration;
            x.Attributes =
                [ClassAttributeBuilder.Build(FluentFlowConsts.Migration, IdentificationStrategy.DateTimeStamp())];
            x.Methods =
            [
                MethodBuilder.Build(m =>
                {
                    m.Name = FluentFlowConsts.UpMethod;
                    m.Modifiers =
                    [
                        SyntaxKind.PublicKeyword,
                        SyntaxKind.OverrideKeyword
                    ];
                    m.Body = Method.Body(upMethod);
                    m.ReturnType = FluentFlowConsts.VoidReturnType;
                }),

                MethodBuilder.Build(m =>
                {
                    m.Name = FluentFlowConsts.DownMethod;
                    m.Modifiers =
                    [
                        SyntaxKind.PublicKeyword,
                        SyntaxKind.OverrideKeyword
                    ];
                    m.ReturnType = FluentFlowConsts.VoidReturnType;
                    m.Body = Method.Body(
                        new FluentBuilder("Delete", true)
                            .AddStep("Table", Argument.String(migrationOptions.TableName))
                            .Build()
                    );
                })
            ];
        });
    }
}