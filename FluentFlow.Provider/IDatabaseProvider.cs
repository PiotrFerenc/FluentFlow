namespace FluentFlow.Provider;

public interface IDatabaseProvider
{
    Task<bool> TryConnect(string connectionString);
    Task<IEnumerable<Database>> GetDatabases();
    Task<IEnumerable<Table>> GetTables(Database database);
    Task<IEnumerable<Column>> GetColumns(Table table);
}

public record Column(string Name, string Type, bool IsPrimaryKey, bool IsIdentity, bool IsNullable, bool IsUnique);

public record Database(string Name);

public record Table(string Name);