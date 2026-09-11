using System.Data.SqlClient;
using System.DirectoryServices;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VulnerableApi.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Server=localhost;Database=Users;User Id=sa;Password=Password123!";
    }

    public string GetUserById(string userId)
    {
        var query = $"SELECT * FROM Users WHERE Id = '{userId}'";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand(query, connection);
        var result = command.ExecuteScalar();
        return result?.ToString() ?? "Not found";
    }

    public bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
