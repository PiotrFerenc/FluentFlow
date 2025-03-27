using CommandLine;

namespace FluentFlow.Console.Model;

public class Options
{
    [Option('p', "provider", Required = true, HelpText = "Database provider name.")]
    public string Provider { get; set; } = string.Empty;

    [Option('c', "connectionString", Required = true, HelpText = "Database connection string.")]
    public string ConnectionString { get; set; } = string.Empty;
}