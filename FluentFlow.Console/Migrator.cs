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
        var config = _chain(provider, migrationOptions);
    }

    private static IDatabaseProvider GetProvider(string name) => name switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };

    private static Func<IDatabaseProvider, MigrationOptions, bool> _chain = (provider, options) => _getDatabase(provider, options);

    private static readonly Func<IDatabaseProvider, MigrationOptions, bool> _getDatabase = (provider, options) =>
    {
        var databases = provider.GetDatabases().GetAwaiter().GetResult();

        var databaseName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select database")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                .AddChoices(databases.Select(x => x.Name.Value)));


        options.DatabaseName = databaseName;
        return true;
    };
}