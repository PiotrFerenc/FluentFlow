using FluentFlow.Console.Exceptions;
using FluentFlow.Console.Model;
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
        var provider = GetProvider(options.Provider);
        provider.TryConnect(new ConnectionString(options.ConnectionString)).GetAwaiter().GetResult();
        var migrationOptions = new MigrationOptions();
        if (BuildConfig(provider, migrationOptions))
        {
            var migration = new FluentBuilder("Create.Table", true);
            foreach (var column in migrationOptions.Columns)
            {
                migration.AddStep(column.Name.ToString());
            }

            System.Console.WriteLine(migration.Build().NormalizeWhitespace());
        }
    }

    private static IDatabaseProvider GetProvider(string name) => name switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };

    private static IColumnMapper GetMapper(string name) => name switch
    {
        "postgres" => new PostgresColumnMapper(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> BuildConfig = (provider, options) => GetDatabase!(provider, options) | GetTables!(provider, options) | GetColumns!(provider, options);

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetColumns = (provider, options) =>
    {
        options.Columns = provider.GetColumns(new Table(new Name(options.TableName))).GetAwaiter().GetResult();
        return true;
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetTables = (provider, options) =>
    {
        var tables = provider.GetTables(new Database(new Name(options.DatabaseName))).GetAwaiter().GetResult();
        var selectedTable = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select table").MoreChoicesText("[grey](Move up and down to reveal more choices)[/]").AddChoices(tables.Select(x => x.Name.Value)));
        options.TableName = selectedTable;
        return true;
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetDatabase = (provider, options) =>
    {
        var databases = provider.GetDatabases().GetAwaiter().GetResult();
        var databaseName = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select database").MoreChoicesText("[grey](Move up and down to reveal more choices)[/]").AddChoices(databases.Select(x => x.Name.Value)));

        options.DatabaseName = databaseName;
        return true;
    };
}