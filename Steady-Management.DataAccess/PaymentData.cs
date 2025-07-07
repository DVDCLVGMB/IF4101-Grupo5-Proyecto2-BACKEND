using System.Data;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.DataAccess
{
    public class PaymentData
    {
        private readonly string connectionString;

        public PaymentData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Insert(Payment payment, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_Payment_Insert", connection, transaction);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@payment_method_id", payment.PaymentMethodId);
            command.Parameters.AddWithValue("@order_id", payment.OrderId);
            command.Parameters.AddWithValue("@credit_card_number", payment.CreditCardNumber);
            command.Parameters.AddWithValue("@payment_quantity", payment.PaymentQuantity);
            command.Parameters.AddWithValue("@payment_date", payment.PaymentDate);

            command.ExecuteNonQuery();
        }

        public Payment? GetByOrderId(int orderId)
        {
            string sql = @"
                SELECT payment_id, payment_method_id, order_id, credit_card_number, 
                       payment_quantity, payment_date
                FROM Payment
                WHERE order_id = @order_id";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", orderId);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Payment(
                    paymentId: reader.GetInt32(reader.GetOrdinal("payment_id")),
                    paymentMethodId: reader.GetInt32(reader.GetOrdinal("payment_method_id")),
                    orderId: reader.GetInt32(reader.GetOrdinal("order_id")),
                    creditCardNumber: reader.GetString(reader.GetOrdinal("credit_card_number")),
                    paymentQuantity: reader.GetDecimal(reader.GetOrdinal("payment_quantity")),
                    paymentDate: reader.GetDateTime(reader.GetOrdinal("payment_date"))
                );
            }

            return null;
        }

        public string GetPaymentMethodName(int paymentMethodId)
        {
            string sql = @"SELECT payment_method_name FROM PaymentMethod WHERE payment_method_id = @id";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", paymentMethodId);

            connection.Open();
            return command.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        public void DeleteByOrderId(int orderId, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_Payment_Delete", connection, transaction);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@order_id", orderId);
            command.ExecuteNonQuery();
        }
    }
}
