using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

public static class ConnectionHelper
{
    private static readonly string connectionString;

    static ConnectionHelper()
    {
        string server = "DESKTOP-BH3VIDG";
        string database = "QLNS_v2";
        string userId = "sa";
        string password = "12345";

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = server;
        builder.InitialCatalog = database;
        builder.UserID = userId;
        builder.Password = password;

        connectionString = builder.ToString();
    }

    public static SqlConnection GetConnection()
    {
        SqlConnection conn = new SqlConnection(connectionString);
        return conn;
    }
}
