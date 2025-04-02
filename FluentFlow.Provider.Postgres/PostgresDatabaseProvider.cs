using System.Data;
using System.Data.Common;
using FluentFlow.Core;
using Npgsql;

namespace FluentFlow.Provider.Postgres
{
    public class PostgresDatabaseProvider : IDatabaseProvider
    {
        private NpgsqlConnection? _connection;

        public async Task<Result<bool>> TryConnect(ConnectionString connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString.Value);
                await _connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed($"Failed to connect to PostgreSQL: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Database>> GetDatabases()
        {
            if (_connection is not { State: ConnectionState.Open })
                throw new InvalidOperationException("Connection to the database has not been established.");

            var databases = new List<Database>();

            const string query = "SELECT datname FROM pg_database WHERE datistemplate = false;";
            await using var command = new NpgsqlCommand(query, _connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                databases.Add(new Database(new Name(reader.GetString(0))));
            }

            return databases;
        }

        public async Task<IEnumerable<Table>> GetTables(Database database)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Connection to the database has not been established.");

            var tables = new List<Table>();

            string query = $@"
                SELECT table_name 
                FROM information_schema.tables 
                WHERE table_schema = 'public' AND table_catalog = @DatabaseName;";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("DatabaseName", database.Name.Value);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tables.Add(new Table(new Name(reader.GetString(0))));
            }

            return tables;
        }

        public async Task<IEnumerable<Column>> GetColumns(Table table)
        {
            if (_connection is not { State: ConnectionState.Open })
                throw new InvalidOperationException("Connection to the database has not been established.");

            var columns = new List<Column>();

            string query = $@"SELECT
    c.column_name,
    c.data_type,
    c.is_nullable,
    c.character_maximum_length
FROM information_schema.columns AS c
         LEFT JOIN (
    SELECT
        a.attname AS column_name
    FROM pg_attribute AS a
    WHERE a.attnum > 0 AND NOT a.attisdropped
) AS ai
                   ON c.column_name = ai.column_name
WHERE c.table_name = '{table.Name.Value}';";

            await using var command = new NpgsqlCommand(query, _connection);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int? maxLength = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);

                columns.Add(new Column(
                    new Name(reader.GetString(0)),
                    new Type(MapType(reader.GetString(1))),
                    new IsNullable(reader.GetString(2) == "YES"),
                    new Length(maxLength)
                ));
            }

            return columns;
        }

        public ColumnType MapType(string type) => type switch
        {
            "character varying" => ColumnType.Varchar,
            "character" => ColumnType.Char,
            "text" => ColumnType.Text,
            "integer" => ColumnType.Integer,
            "bigint" => ColumnType.BigInt,
            "smallint" => ColumnType.SmallInt,
            "numeric" => ColumnType.Numeric,
            "decimal" => ColumnType.Decimal,
            "real" => ColumnType.Real,
            "double precision" => ColumnType.DoublePrecision,
            "boolean" => ColumnType.Boolean,
            "date" => ColumnType.Date,
            "timestamp without time zone" => ColumnType.TimestampWithoutTimeZone,
            "timestamp with time zone" => ColumnType.TimestampWithTimeZone,
            "time without time zone" => ColumnType.TimeWithoutTimeZone,
            "time with time zone" => ColumnType.TimeWithTimeZone,
            "json" => ColumnType.Json,
            "jsonb" => ColumnType.Jsonb,
            "uuid" => ColumnType.Uuid,
            "bytea" => ColumnType.Bytea,
            "array" => ColumnType.Array,
            "interval" => ColumnType.Interval,
            _ => throw new InvalidOperationException($"Unknown type: {type}")
        };
    }
}