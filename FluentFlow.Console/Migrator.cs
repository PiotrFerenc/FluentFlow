using FluentFlow.Console.Exceptions;
using FluentFlow.Console.Model;
using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;

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
        foreach (var database in databases)
        {
            System.Console.WriteLine(database.Name);
        }

        var name = System.Console.ReadLine();
        options.DatabaseName = name;
        return true;
    };
}