using Microsoft.Data.SqlClient;

public class DatabaseService
{
    private readonly string _connectionString;
    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ShopSqlConnection")!;
    }

    public void ResetIdentity(string tableName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand($"DBCC CHECKIDENT ('{tableName}', RESEED, 0)", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}