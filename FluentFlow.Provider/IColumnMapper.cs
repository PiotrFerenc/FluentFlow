namespace FluentFlow.Provider;

public interface IColumnMapper
{
    string MapColumnTypeToFluentMigratorType(Column column);
}

public class ColumnMapper : IColumnMapper
{
    public string MapColumnTypeToFluentMigratorType(Column column)
        => column.Type.Value switch
        {
            "serial" => "AsInt32().Identity()",
            "bigserial" => "AsInt64().Identity()",
            "smallserial" => "AsInt16().Identity()",
            "integer" => "AsInt32()",
            "bigint" => "AsInt64()",
            "smallint" => "AsInt16()",
            "decimal" or "numeric" => "AsDecimal()",
            "real" => "AsFloat()",
            "double precision" => "AsDouble()",
            "character varying" or "varchar" => $"AsString({column.Length})",
            "character" or "char" => $"AsFixedLengthString({column.Length})",
            "text" => "AsString(int.MaxValue)",
            "boolean" => "AsBoolean()",
            "date" => "AsDate()",
            "timestamp without time zone" => "AsDateTime()",
            "timestamp with time zone" => "AsDateTimeOffset()",
            "time without time zone" => "AsCustom(\"time\")",
            "time with time zone" => "AsCustom(\"time with time zone\")",
            "json" => "AsCustom(\"json\")",
            "jsonb" => "AsCustom(\"jsonb\")",
            "uuid" => "AsGuid()",
            "bytea" => "AsBinary()",
            "array" => "AsCustom(\"array\")",
            "interval" => "AsCustom(\"interval\")",
            _ => throw new NotSupportedException($"Unsupported column type: {column.Type.Value}")
        };
}