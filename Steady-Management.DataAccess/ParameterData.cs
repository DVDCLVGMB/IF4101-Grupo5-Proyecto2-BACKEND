using Microsoft.Data.SqlClient;
using SteadyManagement.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace Steady_Management.DataAccess
{
    public class ParameterData
    {
        private readonly string _connectionString;

        public ParameterData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Parameter GetParameter()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[dbo].[usp_GetParameter]", connection);
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Parameter
                {
                    NombreEmpresa = reader["Nombre_empresa"].ToString(),
                    TelefonoEmpresa = reader["Telefono_empresa"].ToString(),
                    CorreoEmpresa = reader["Correo_empresa"].ToString(),
                    TipoPago = reader["Tipo_pago"].ToString(),
                    CedulaJuridica = reader["Cedula_juridica"].ToString()
                };
            }

            return null;
        }
        public void UpdateParameter(Parameter parameterToUpdate)
        {
            if (parameterToUpdate == null)
            {
                throw new ArgumentNullException(nameof(parameterToUpdate), "El objeto Parameter no puede ser nulo.");
            }

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("[dbo].[usp_UpdateParameter]", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Numero_factura", parameterToUpdate.NumeroFactura);

            command.Parameters.AddWithValue("@Nombre_empresa", parameterToUpdate.NombreEmpresa ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Telefono_empresa", parameterToUpdate.TelefonoEmpresa ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Correo_empresa", parameterToUpdate.CorreoEmpresa ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Tipo_pago", parameterToUpdate.TipoPago ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Cedula_juridica", parameterToUpdate.CedulaJuridica ?? (object)DBNull.Value);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al actualizar el parámetro: {ex.Message}");
                throw;
            }
        }
    }
}