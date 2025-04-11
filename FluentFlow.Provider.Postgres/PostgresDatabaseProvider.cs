using System.Data;
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

        public CSharpType MapType(string type) => type.ToLower() switch
        {
            "daterange" => CSharpType.DateTime,
            "character varying" => CSharpType.String,
            "character" => CSharpType.String,
            "text" => CSharpType.String,
            "integer" => CSharpType.Int32,
            "bigint" => CSharpType.Int64,
            "smallint" => CSharpType.Int16,
            "numeric" => CSharpType.Decimal,
            "decimal" => CSharpType.Decimal,
            "real" => CSharpType.Single,
            "double precision" => CSharpType.Double,
            "boolean" => CSharpType.Bool,
            "date" => CSharpType.DateTime,
            "timestamp without time zone" => CSharpType.DateTime,
            "timestamp with time zone" => CSharpType.DateTimeOffset,
            "time without time zone" => CSharpType.TimeSpan,
            "time with time zone" => CSharpType.String,
            "json" => CSharpType.String,
            "jsonb" => CSharpType.String,
            "uuid" => CSharpType.Guid,
            "bytea" => CSharpType.ByteArray,
            "array" => CSharpType.ObjectArray,
            "interval" => CSharpType.TimeSpan,
            "tsrange" => CSharpType.String,
            "tstzrange" => CSharpType.String,
            "money" => CSharpType.Decimal,
            "line" => CSharpType.String,
            "lseg" => CSharpType.String,
            "box" => CSharpType.String,
            "path" => CSharpType.String,
            "polygon" => CSharpType.String,
            "circle" => CSharpType.String,
            "inet" => CSharpType.String,
            "cidr" => CSharpType.String,
            "macaddr" => CSharpType.String,
            "macaddr8" => CSharpType.String,
            "xml" => CSharpType.String,
            "int4range" => CSharpType.String,
            "point" => CSharpType.String,
            "user-defined" => CSharpType.String,
            _ => throw new InvalidOperationException($"Unknown type: {type}")
        };
    }
}