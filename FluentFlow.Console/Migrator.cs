using FluentFlow.Console.Exceptions;
using FluentFlow.Console.Model;
using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;
using Spectre.Console;

namespace FluentFlow.Console;

public static class Migrator
{
    public static void Main(Options options)
    {
        var provider = GetProvider(options.Provider);
        provider.TryConnect(new ConnectionString(options.ConnectionString)).GetAwaiter().GetResult();
        var migrationOptions = new MigrationOptions();
        var config = BuildConfig(provider, migrationOptions);
    }

    private static IDatabaseProvider GetProvider(string name) => name switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> BuildConfig = (provider, options) => GetDatabase!(provider, options) | GetTables!(provider,options);

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetTables = (provider, options) =>
    {
        var tables = provider.GetTables(new Database(new Name(options.DatabaseName))).GetAwaiter().GetResult();
var selectedTable = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select table")
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(tables.Select(x => x.Name.Value)));
options.TableName = selectedTable;
return true;
    };
    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> GetDatabase = (provider, options) =>
    {
        var databases = provider.GetDatabases().GetAwaiter().GetResult();

        var databaseName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select database")
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(databases.Select(x => x.Name.Value)));

        options.DatabaseName = databaseName;
        return true;
    };
}