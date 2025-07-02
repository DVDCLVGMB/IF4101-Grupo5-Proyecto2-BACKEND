using Microsoft.Data.SqlClient;
using Steady_Management.Domain;
using SteadyManagement.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace Steady_Management.DataAccess
{
    public class OrderDetailsData
    {

        private readonly string connectionString;

        public OrderDetailsData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<OrderDetail> GetAll()
        {
            var list = new List<OrderDetail>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_OrderDetails_ReadByOrderId", connection);
            command.CommandType = CommandType.StoredProcedure;

            // Pasa NULL para que el SP devuelva todos los registros
            command.Parameters.AddWithValue("@OrderId", DBNull.Value);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        private static OrderDetail Map(SqlDataReader reader) => new()
        {
            OrderId = (int)reader["OrderId"],
            ProductId = (int)reader["ProductId"],
            Quantity = (int)reader["Quantity"],
            UnitPrice = (decimal)Convert.ToSingle(reader["UnitPrice"])
        };


    }
}