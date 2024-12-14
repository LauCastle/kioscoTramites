using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using KioscoTramites;

namespace KioscoTramite
{
    public class TramiteService
    {
        private readonly DatabaseConneccion _dbConnection;

        public TramiteService()
        {
            _dbConnection = new DatabaseConneccion();
        }

        public void CrearTramite(string nombre, string descripcion, decimal precio)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                var query = "INSERT INTO Tramites (Nombre, Descripcion, Precio) VALUES (@Nombre, @Descripcion, @Precio)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@Precio", precio);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Trámite creado exitosamente.");
        }

        public List<(int Id, string Nombre, string Descripcion, decimal Precio, DateTime FechaCreacion)> ObtenerTramites()
        {
            var tramites = new List<(int, string, string, decimal, DateTime)>();
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                var query = "SELECT Id, Nombre, Descripcion, Precio, FechaCreacion FROM Tramites";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tramites.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3),
                                reader.GetDateTime(4)
                            ));
                        }
                    }
                }
            }
            return tramites;
        }

        public void ActualizarTramite(int id, string nombre, string descripcion, decimal precio)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                var query = "UPDATE Tramites SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Descripcion", descripcion);
                    command.Parameters.AddWithValue("@Precio", precio);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Trámite actualizado exitosamente.");
                    else
                        Console.WriteLine("No se encontró el trámite con el ID especificado.");
                }
            }
        }

        public void EliminarTramite(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                var query = "DELETE FROM Tramites WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Trámite eliminado exitosamente.");
                    else
                        Console.WriteLine("No se encontró el trámite con el ID especificado.");
                }
            }
        }
    }
}