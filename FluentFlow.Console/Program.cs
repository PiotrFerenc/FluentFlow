using FluentFlow.Provider;
using FluentFlow.Provider.Postgres;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, FluentFlow!");

var services = new ServiceCollection();
try
{

var provider = GetProvider("postgres");
services.AddSingleton(provider);

var app = services.BuildServiceProvider();

}
catch (DatabaseProviderNotSupportException e)  
{
    Console.WriteLine(e);
    throw;
}

IDatabaseProvider GetProvider(string name) => name switch
{
    "postgres" => new PostgresDatabaseProvider(),
    _ => throw new DatabaseProviderNotSupportException(name)
};

public class DatabaseProviderNotSupportException : Exception
{
    public DatabaseProviderNotSupportException(string name) : base($"Database provider {name} is not supported.")
    {
    }
}