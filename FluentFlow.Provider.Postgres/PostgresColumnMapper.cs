using FluentFlow.Core;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Provider.Postgres;

public class PostgresColumnMapper : IColumnMapper
{
    public string MapColumnTypeToFluentMigratorType(Column column)
        => column.Type.Value switch
        {
            ColumnType.Serial => "AsInt32().Identity()",
            ColumnType.BigSerial => "AsInt64().Identity()",
            ColumnType.SmallSerial => "AsInt16().Identity()",
            ColumnType.Integer => "AsInt32()",
            ColumnType.BigInt => "AsInt64()",
            ColumnType.SmallInt => "AsInt16()",
            ColumnType.Decimal or ColumnType.Numeric => "AsDecimal()",
            ColumnType.Real => "AsFloat()",
            ColumnType.DoublePrecision => "AsDouble()",
            ColumnType.CharacterVarying or ColumnType.Varchar => $"AsString({column.Length})",
            ColumnType.Character or ColumnType.Char => $"AsFixedLengthString({column.Length})",
            ColumnType.Text => "AsString(int.MaxValue)",
            ColumnType.Boolean => "AsBoolean()",
            ColumnType.Date => "AsDate()",
            ColumnType.TimestampWithoutTimeZone => "AsDateTime()",
            ColumnType.TimestampWithTimeZone => "AsDateTimeOffset()",
            ColumnType.TimeWithoutTimeZone => "AsCustom(\"time\")",
            ColumnType.TimeWithTimeZone => "AsCustom(\"time with time zone\")",
            ColumnType.Json => "AsCustom(\"json\")",
            ColumnType.Jsonb => "AsCustom(\"jsonb\")",
            ColumnType.Uuid => "AsGuid()",
            ColumnType.Bytea => "AsBinary()",
            ColumnType.Array => "AsCustom(\"array\")",
            ColumnType.Interval => "AsCustom(\"interval\")",
            _ => throw new NotSupportedException($"Unsupported column type: {column.Type.Value}")
        };

    public PostgresColumnMapper()
    {
        SyntaxFactory.ExpressionStatement(
            SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("Create"),
                                            SyntaxFactory.IdentifierName("Table")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal("Log")))))),
                                SyntaxFactory.IdentifierName("WithColumn")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal("Id")))))),
                    SyntaxFactory.IdentifierName("AsInt64"))));
    }
}