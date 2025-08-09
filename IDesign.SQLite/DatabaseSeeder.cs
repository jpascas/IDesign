using Microsoft.Data.Sqlite;

namespace IDesign.SQLite
{
    public class DatabaseSeeder
    {
        public DatabaseSeeder() { }
        public DatabaseSeeder(string databasePath)
        {
            databasePath = databasePath;
        }

        public string DatabasePath { get; }

        public void SeedDatabase()
        {
            if (string.IsNullOrWhiteSpace(DatabasePath) || !File.Exists(DatabasePath))
            {
                throw new FileNotFoundException("Database file not found.", DatabasePath);
            }
            using (var conn = new SqliteConnection($"Data Source={DatabasePath}"))
            {
                conn.Open();
                // Run create_and_seed.sql
                var sqlFile = Path.Combine(AppContext.BaseDirectory, "create_and_seed.sql");
                if (File.Exists(sqlFile))
                {
                    var sqlScript = File.ReadAllText(sqlFile);
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sqlScript;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
