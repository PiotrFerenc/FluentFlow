using FluentFlow.Core;

namespace FluentFlow.Provider;

public interface IDatabaseProvider
{
    Task<Result<bool>> TryConnect(ConnectionString connectionString);
    Task<IEnumerable<Database>> GetDatabases();
    Task<IEnumerable<Table>> GetTables(Database database);
    Task<IEnumerable<Column>> GetColumns(Table table);
    CSharpType MapType(string type);
}

public record ConnectionString(string Value);

public record Name(string Value)
{
    public override string ToString() => Value;
};

public record Type(CSharpType Value);

public record IsNullable(bool Value);

public record Column(Name Name, Type Type, IsNullable IsNullable, Length Length)
{
    public override string ToString() => Name.Value;
};

public record Length(int? Value);

public record Database(Name Name);

public record Table(Name Name)
{
    public override string ToString() => Name.Value;
};