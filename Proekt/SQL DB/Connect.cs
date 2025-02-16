using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Proekt.SQL_DB
{
    public class MainConnector
    {
        private readonly SqlConnection connection;

        public MainConnector()
        {
            connection = new SqlConnection(ConnectionString.MsSqlConnection);
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }

        public SqlConnection GetConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open(); // Открываем подключение, если оно закрыто
            }
            return connection;
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public class DbExecutor
    {
        private readonly MainConnector connector;

        public DbExecutor(MainConnector connector)
        {
            this.connector = connector;
        }
    }

    public class Manager
    {
        private MainConnector connector;
        private DbExecutor dbExecutor;
        public MainConnector Connector => connector;
        public Manager()
        {
            connector = new MainConnector();
        }
        public void Connect()
        {
            
            var result =  connector.ConnectAsync();

            if (result.Result)
            {
                Console.WriteLine("Подключено успешно!");
                dbExecutor = new DbExecutor(connector);
            }
            else
            {
                Console.WriteLine("Ошибка подключения!");
            }
        }
    }

    public static class ConnectionString
    {
        public static string MsSqlConnection => @"Data Source=.\SQLEXPRESS01;Database=Proekt;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}

