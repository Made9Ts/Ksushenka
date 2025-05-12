using Npgsql;
using System;
using System.Configuration;

public class DatabaseContext
{
    private string connString;

    public DatabaseContext()
    {
        connString = ConfigurationManager.ConnectionStrings["KsushenkaDB"].ConnectionString;
        if (string.IsNullOrEmpty(connString))
        {
            connString = "Host=127.0.0.1;Username=postgres;Password=5472;Database=postgres";
        }
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connString);
    }
}