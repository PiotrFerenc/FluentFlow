using FluentFlow.Console.Exceptions;
using FluentFlow.Console.Model;
using FluentFlow.Core;
using FluentFlow.Core.Code;
using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spectre.Console;
using Name = FluentFlow.Provider.Name;
using Table = FluentFlow.Provider.Table;

namespace FluentFlow.Console;

public delegate AttributeArgumentSyntax Identification();

public static class IdentificationStrategy
{
    public static AttributeArgumentSyntax DateTimeStamp() =>
        AttributeArgument.Build((long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"))));
}

public static class Migrator
{
    public static void Main(Options options)
    {
        var provider = GetProvider(options.Provider);
        var connection = provider.TryConnect(new ConnectionString(options.ConnectionString)).GetAwaiter().GetResult();
        if (connection.IsFailed)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {connection.Error}");
            return;
        }

        var migrationOptions = new MigrationOptions();
        if (BuildConfig(provider, migrationOptions))
        {
            var migration = new FluentBuilder($"Create.Table(\"{migrationOptions.TableName}\")", true);
            foreach (var column in migrationOptions.Columns)
            {
                migration.AddStep("WithColumn", Argument.String(column.Name.ToString()))
                    .AddStep(ColumnMapper.Map(column.Type.Value));
            }

            var code = MigrationBuilder.Build(migrationOptions, migration.Build(), IdentificationStrategy.DateTimeStamp);

            System.Console.WriteLine(code.NormalizeWhitespace());
        }
    }

    private static IDatabaseProvider GetProvider(string name) => name switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> BuildConfig = (provider, options) =>
        GetDatabase!(provider, options) | GetTables!(provider, options) | GetColumns!(provider, options);

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetColumns = (provider, options) =>
    {
        options.Columns = provider.GetColumns(new Table(new Name(options.TableName))).GetAwaiter().GetResult();
        return true;
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetTables = (provider, options) =>
    {
        var tables = provider.GetTables(new Database(new Name(options.DatabaseName))).GetAwaiter().GetResult();
        var selectedTable = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select table")
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(tables.Select(x => x.Name.Value)));
        options.TableName = selectedTable;
        return true;
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetDatabase = (provider, options) =>
    {
        var databases = provider.GetDatabases().GetAwaiter().GetResult();
        var databaseName = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select database")
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(databases.Select(x => x.Name.Value)));

        options.DatabaseName = databaseName;
        return true;
    };
}