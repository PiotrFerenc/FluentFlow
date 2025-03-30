namespace FluentFlow.Provider;

public interface IColumnMapper
{
    string MapColumnTypeToFluentMigratorType(Column column);
}