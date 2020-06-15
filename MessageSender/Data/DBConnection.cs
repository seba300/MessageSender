using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    public class DBConnection
    {
        private static string connectionString { get; set; } = @"Data Source=B2B\SQLEXPRESS;Initial Catalog=HardwareStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection connection { get; set; } = new SqlConnection(connectionString);
        public DBConnection()
        {
            connection.Open();
        }
        ~DBConnection()
        {
            //connection.Close();
        }

        //Returns query result
        public SqlDataReader GetResult(string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            return command.ExecuteReader();
        }
    }
}
