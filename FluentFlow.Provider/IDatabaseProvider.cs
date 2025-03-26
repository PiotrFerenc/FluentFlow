using System.Data.Common;
using FluentFlow.Core;

namespace FluentFlow.Provider;

public interface IDatabaseProvider
{
    Task<Result<bool>> TryConnect(ConnectionString connectionString);
    Task<IEnumerable<Database>> GetDatabases();
    Task<IEnumerable<Table>> GetTables(Database database);
    Task<IEnumerable<Column>> GetColumns(Table table);
}

public record ConnectionString(string Value);

public record Name(string Value);

public record Type(string Value);

public record IsPrimaryKey(bool Value);

public record IsIdentity(bool Value);

public record IsNullable(bool Value);

public record IsUnique(bool Value);

public record Column(Name Name, Type Type, IsPrimaryKey IsPrimaryKey, IsIdentity IsIdentity, IsNullable IsNullable, IsUnique IsUnique);

public record Database(Name Name);

public record Table(Name Name);