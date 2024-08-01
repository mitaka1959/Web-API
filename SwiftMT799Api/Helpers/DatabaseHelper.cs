using Microsoft.Data.Sqlite;

namespace SwiftMT799Api.Helpers
{
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=swiftmt799.db";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS SwiftMessages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Field20 TEXT,
                        Field21 TEXT,
                        Field79 TEXT
                    )";
                command.ExecuteNonQuery();
            }
        }

        public static void InsertSwiftMessage(string field20, string field21, string field79)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO SwiftMessages (Field20, Field21, Field79)
                    VALUES ($field20, $field21, $field79)";
                command.Parameters.AddWithValue("$field20", field20);
                command.Parameters.AddWithValue("$field21", field21);
                command.Parameters.AddWithValue("$field79", field79);
                command.ExecuteNonQuery();
            }
        }
    }
}
