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

            string query = $@"
                SELECT column_name, data_type, is_nullable, is_identity, is_primary 
                FROM information_schema.columns
                LEFT JOIN (
                    SELECT
                        a.attname AS column_name,
                        't'::boolean AS is_identity,
                        't'::boolean AS is_primary
                    FROM pg_attribute AS a
                    WHERE a.attnum > 0 AND NOT a.attisdropped
                ) AS additional_info
                ON column_name = column_name  
                WHERE table_name = @TableName;";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("TableName", table.Name.Value);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                columns.Add(new Column(
                    new Name(reader.GetString(0)),
                    new Type(reader.GetString(1)),
                    new IsPrimaryKey(reader.GetBoolean(4)),
                    new IsIdentity(reader.GetBoolean(3)),
                    new IsNullable(reader.GetString(2) == "YES"),
                    new IsUnique(false)
                ));
            }

            return columns;
        }
    }
}