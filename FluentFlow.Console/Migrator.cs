using FluentFlow.Console.Exceptions;
using FluentFlow.Console.Model;
using FluentFlow.Core;
using FluentFlow.Core.Code;
using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;
using Microsoft.CodeAnalysis;
using Spectre.Console;
using Name = FluentFlow.Provider.Name;
using Table = FluentFlow.Provider.Table;

namespace FluentFlow.Console;

public static class Migrator
{

    public static void Main(Options options)
    {
        if (!TrySetupConnection(options, out var provider, out var migrationOptions))
            return;

        ConfigureMigration(provider, migrationOptions);
        var migrationCode = BuildMigrationCode(migrationOptions);

        PrintMigrationCode(migrationCode);
    }

    private static bool TrySetupConnection(Options options, out IDatabaseProvider provider, out MigrationOptions migrationOptions)
    {
        provider = GetProvider(options.Provider);
        var connectionResult = provider.TryConnect(new ConnectionString(options.ConnectionString)).GetAwaiter().GetResult();

        if (connectionResult.IsFailed)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {connectionResult.Error}");
            migrationOptions = null;
            return false;
        }

        migrationOptions = new MigrationOptions();
        return true;
    }

    private static void ConfigureMigration(IDatabaseProvider provider, MigrationOptions options)
    {
        SelectDatabase(provider, options);
        SelectTable(provider, options);
        LoadColumns(provider, options);
    }

    private static string BuildMigrationCode(MigrationOptions options)
    {
        var migration = new FluentBuilder($"{FluentFlowConsts.CreateTableTemplate}(\"{options.TableName}\")", true);

        foreach (var column in options.Columns)
        {
            migration.AddStep("WithColumn", Argument.String(column.Name.ToString()))
                .AddStep(ColumnMapper.Map(column.Type.Value));
        }

        return MigrationBuilder.Build(options, migration.Build(), IdentificationStrategy.DateTimeStamp).NormalizeWhitespace().ToString();
    }

    private static void PrintMigrationCode(string code) =>
        System.Console.WriteLine(code);

    private static IDatabaseProvider GetProvider(string providerName) => providerName switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(providerName)
    };

    private static void LoadColumns(IDatabaseProvider provider, MigrationOptions options)
    {
        options.Columns = provider.GetColumns(new Table(new Name(options.TableName)))
            .GetAwaiter()
            .GetResult();
    }

    private static void SelectTable(IDatabaseProvider provider, MigrationOptions options)
    {
        var tables = provider.GetTables(new Database(new Name(options.DatabaseName)))
            .GetAwaiter()
            .GetResult();

        options.TableName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select table")
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(tables.Select(x => x.Name.Value))
        );
    }

    private static void SelectDatabase(IDatabaseProvider provider, MigrationOptions options)
    {
        var databases = provider.GetDatabases()
            .GetAwaiter()
            .GetResult();

        options.DatabaseName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select database")
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(databases.Select(x => x.Name.Value))
        );
    }
}