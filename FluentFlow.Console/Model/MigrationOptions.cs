using FluentFlow.Provider;

namespace FluentFlow.Console.Model;

public class MigrationOptions
{
    public string DatabaseName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public IEnumerable<Column> Columns { get; set; }
}