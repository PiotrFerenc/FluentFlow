using CommandLine;
using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, FluentFlow!");

Parser.Default.ParseArguments<Options>(args).WithParsed(Migrator.Main);


public class DatabaseProviderNotSupportException(string name) : Exception($"Database provider {name} is not supported.");

public class Options
{
    [Option('p', "provider", Required = true, HelpText = "Database provider name.")]
    public string Provider { get; set; } = string.Empty;

    [Option('c', "connectionString", Required = true, HelpText = "Database connection string.")]
    public string ConnectionString { get; set; } = string.Empty;
}

public static class Migrator
{
    public static void Main(Options options)
    {
        var services = new ServiceCollection();
        try
        {
            var provider = GetProvider("postgres");
            services.AddSingleton(provider);

            var app = services.BuildServiceProvider();
            var database = app.GetRequiredService<IDatabaseProvider>();
            var databases = database.GetDatabases().GetAwaiter().GetResult();
        }
        catch (DatabaseProviderNotSupportException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static IDatabaseProvider GetProvider(string name) => name switch
    {
        "postgres" => new PostgresDatabaseProvider(),
        _ => throw new DatabaseProviderNotSupportException(name)
    };
}