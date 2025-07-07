using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.DataAccess
{
    public class PaymentMethodData
    {
        private readonly string connectionString;

        public PaymentMethodData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private static PaymentMethod Map(SqlDataReader reader) => new()
        {
            PaymentMethodId = (int)reader["payment_method_id"],
            PaymentMethodName = (string)reader["payment_method_name"]
        };

        public List<PaymentMethod> GetAll()
        {
            var paymentMethods = new List<PaymentMethod>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_PaymentMethod_Get_All", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                paymentMethods.Add(Map(reader));
            }
            return paymentMethods;
        }
    }
}
