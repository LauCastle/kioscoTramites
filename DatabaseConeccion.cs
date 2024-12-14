using System.Data.SqlClient;

namespace KioscoTramites
{
    public class DatabaseConneccion
    {
        private const string V = "Server=localhost/SQLEXPRESS;Database=KioscoColima;Trusted_Connection=True;";

        // Solo una definición de connectionString
        private readonly string connectionString = V;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}